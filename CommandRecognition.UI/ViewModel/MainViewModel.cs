using CommandRecognition.BL.Interfaces;
using CommandRecognition.CORE;
using CommandRecognition.UI.Command;
using CommandRecognition.UI.View;
using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CommandRecognition.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private CultureInfo _culture;
        private SpeechRecognitionEngine _sre;
        private Choices programs = new Choices();

        private int _userId;
        public int UserId { get => _userId; set => SetField(ref _userId, value); } // Id пользователя

        private bool? Flag = null; // флаг установки добавление или редактирование выполняется
        // свойство открытия popup menu окна добавлнени/редактирования
        private bool _isOpenDialog = false;
        public bool IsOpenDialog { get => _isOpenDialog; set => SetField(ref _isOpenDialog, value); }

        // віюранный пунтк из коллекции и его индекс
        private VoiceCommand _selectedRecCommand;
        public VoiceCommand SelectedRecCommand { get => _selectedRecCommand; set => SetField(ref _selectedRecCommand, value); }
        private int _selectedIndex;
        public int SelectedIndex { get => _selectedIndex; set => SetField(ref _selectedIndex, value); }

        // коллекция всех наших команд
        private ObservableCollection<VoiceCommand> _recCommand = new ObservableCollection<VoiceCommand>();
        public ObservableCollection<VoiceCommand> RecCommand { get => _recCommand; set => SetField(ref _recCommand, value); }

        private IDataServices _dataServices;

        public MainViewModel(IDataServices dataServices)
        {
            _dataServices = dataServices;
        }

        public void Initialize()
        {
            GetRecCommand();

            try
            {
                _culture = new CultureInfo("ru-ru");
                _sre = new SpeechRecognitionEngine(_culture);
                // Setup event handlers
                _sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);
                // select input source
                _sre.SetInputToDefaultAudioDevice();
                // load grammar
                _sre.LoadGrammar(CreateSampleGrammarStart());
                _sre.LoadGrammar(CreateSampleGrammarClose());
                // start recognition
                _sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetRecCommand()
        {
            RecCommand.Clear();
            programs = new Choices();
            var data = _dataServices.GetRecCommand(UserId) as List<VoiceCommand>;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    RecCommand.Add(item);
                    programs.Add(new SemanticResultValue(item.Command, item.Path));
                }
            }
        }

        private ICommand _openDialog;
        public ICommand OpedDialog
        {
            get
            {
                return _openDialog ?? (_openDialog = new DelegateCommand<object>(a =>
                {
                    if (a.ToString() == "edit") { Flag = true; }
                    else
                    {
                        Flag = false;
                        SelectedRecCommand = new VoiceCommand();
                    }
                    IsOpenDialog = true;
                }));
            }
        }

        private ICommand _deleteItem;
        public ICommand DeleteItem { get { return _deleteItem ?? (_deleteItem = new DelegateCommand<object>(a => OnDeleteItem())); } }

        private ICommand _editItem;
        public ICommand EditItem { get { return _editItem ?? (_editItem = new DelegateCommand<object>(a => OnEditItem())); } }

        private ICommand _openSettings;
        public ICommand OpenSettings { get { return _openSettings ?? (_openSettings = new DelegateCommand<object>(a => OnOpenSettings())); } }

        private void OnOpenSettings()
        {
            var settings = new SettingsWindow();
            settings.DataContext = IocKernel.IocKernel.Get<SettingsViewModel>();
            settings.ShowDialog();
        }

        private void OnDeleteItem()
        {
            if (SelectedRecCommand == null) return;
            _dataServices.DeleteRecCommand(SelectedRecCommand.Id);
            // RecCommand.Remove(SelectedRecCommand);
            Initialize();
        }

        private void OnEditItem()
        {
            if (Flag == true)
            {
                _dataServices.UpdateRecCommand(SelectedRecCommand);
                //RecCommand[SelectedIndex].Command = SelectedRecCommand.Command;
                //RecCommand[SelectedIndex].Path = SelectedRecCommand.Path;
            }
            else
            {
                SelectedRecCommand.UserId = _userId;
                var recCommand = _dataServices.AddRecCommand(SelectedRecCommand);
                //programs.Add(new SemanticResultValue(recCommand.Command, recCommand.Path));
                //RecCommand.Add(recCommand);
            }

            Initialize();

            Flag = null;
            IsOpenDialog = false;
        }

        /*
         * Блок распознования
         */
        
        private Grammar CreateSampleGrammarStart()
        {
            var grammarBuilder = new GrammarBuilder("запустить", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = _culture;
            grammarBuilder.Append(new SemanticResultKey("start", programs));

            return new Grammar(grammarBuilder);
        }

        private Grammar CreateSampleGrammarClose()
        {
            var grammarBuilder = new GrammarBuilder("закрыть", SubsetMatchingMode.SubsequenceContentRequired);
            grammarBuilder.Culture = _culture;
            grammarBuilder.Append(new SemanticResultKey("close", programs));

            return new Grammar(grammarBuilder);
        }

        private void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < 0.3f)
                return;

            foreach (var s in e.Result.Semantics)
            {
                var program = (string)s.Value.Value;

                switch (s.Key)
                {
                    case "start":
                        Process.Start(program);
                        break;
                    case "close":
                        var p = Process.GetProcessesByName(program);
                        if (p.Length > 0)
                            p[0].Kill();
                        break;
                }
            }
        }
    }
}