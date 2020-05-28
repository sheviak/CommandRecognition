using CommandRecognition.UI.Command;
using CommandRecognition.UI.View;
using Microsoft.Win32;
using System.Reflection;
using System.Windows.Input;

namespace CommandRecognition.UI.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private RegistryKey Key;
        private const string name = "Voice recognition";
        private const string pathRegistryKeyStartup = @"Software\Microsoft\Windows\CurrentVersion\Run\";

        public bool _isAutorun;
        public bool IsAutorun
        {
            get => _isAutorun;
            set
            {
                _isAutorun = value;
                if (_isAutorun) SetAutorun();
                else DeleteAutorun();
            }
        }

        public SettingsViewModel()
        {
            Key = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true);
            GetStatus();
        }

        private void GetStatus()
        {
            var value = (string)Key.GetValue(name);
            if (string.IsNullOrEmpty(value)) _isAutorun = false;
            else _isAutorun = true;
        }

        private void SetAutorun() => Key.SetValue(name, Assembly.GetExecutingAssembly().Location);

        private void DeleteAutorun() => Key.DeleteValue(name);

        public ICommand _resetCommand;
        public ICommand ResetCommand { get { return _resetCommand ?? (_resetCommand = new DelegateCommand<object>(a => OnResetCommand())); } }
        private void OnResetCommand() => new ResetPasswordWindow().ShowDialog();
    }
}
