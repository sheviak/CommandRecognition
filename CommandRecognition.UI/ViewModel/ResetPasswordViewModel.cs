using CommandRecognition.BL.Interfaces;
using CommandRecognition.CORE;
using CommandRecognition.UI.Command;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommandRecognition.UI.ViewModel
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private bool _isShow;
        public bool IsShow { get => _isShow; set => SetField(ref _isShow, value); }

        private string _login;
        public string Login { get => _login; set => SetField(ref _login, value); }

        private int _code;
        public int Code { get => _code; set => SetField(ref _code, value); }

        private int? _inputCode;
        public int? InputCode { get => _inputCode; set => SetField(ref _inputCode, value); }

        private string _question;
        public string Question { get => _question; set => SetField(ref _question, value); }

        private string _answer;
        public string Answer { get => _answer; set => SetField(ref _answer, value); }

        private User User;

        #region visibility part UI

        private Visibility _visibleLoginPart = Visibility.Visible;
        public Visibility VisibleLoginPart { get => _visibleLoginPart; set => SetField(ref _visibleLoginPart, value); }

        private Visibility _visibleAnswerPart = Visibility.Collapsed;
        public Visibility VisibleAnswerPart { get => _visibleAnswerPart; set => SetField(ref _visibleAnswerPart, value); }

        private Visibility _visiblePassPart = Visibility.Collapsed;
        public Visibility VisiblePassPart { get => _visiblePassPart; set => SetField(ref _visiblePassPart, value); }

        #endregion

        private Random Random = new Random();
        private IAccountServices _accountServices;

        public ResetPasswordViewModel(IAccountServices accountServices)
        {
            _accountServices = accountServices;
            Code = Random.Next(1000, 10000);
        }

        public ICommand _checkLogin;
        public ICommand CheckLogin { get { return _checkLogin ?? (_checkLogin = new DelegateCommand<object>(a => OnCheckLogin())); } }

        public ICommand _checkAnswer;
        public ICommand CheckAnswer { get { return _checkAnswer ?? (_checkAnswer = new DelegateCommand<object>(a => OnCheckAnswer())); } }

        public ICommand _changePassword;
        public ICommand ChangePassword { get { return _changePassword ?? (_changePassword = new DelegateCommand<object>(a => OnChangePassword(a))); } }

        private async void OnCheckLogin()
        {
            IsShow = true;

            var res = await Task.Run(() =>
            {
                if (InputCode == Code && Login.Trim() != null)
                {
                    (bool result, User user) = _accountServices.CheckUserLogin(Login);
                    if (result)
                    {
                        User = user;
                        Question = user.Question;
                        VisibleLoginPart = Visibility.Collapsed;
                        VisibleAnswerPart = Visibility.Visible;
                        return true;
                    } else return false;
                }
                else return false;
            });

            if (!res)
            {
                MessageBox.Show("Проверьте корректность всех полей!");
                Code = Random.Next(1000, 10000);
                InputCode = null;
            }
            
            IsShow = false;
        }

        private async void OnCheckAnswer()
        {
            IsShow = true;

            var result = await Task.Run(() =>
            {
                Task.Delay(1000).Wait();

                if (Answer.ToUpper() == User.Answer)
                {
                    VisibleAnswerPart = Visibility.Collapsed;
                    VisiblePassPart = Visibility.Visible;
                    return true;
                } else return false;
            });

            if(!result)
                MessageBox.Show("Ответ не верный!");

            IsShow = false;
        }

        private async void OnChangePassword(object item)
        {
            IsShow = true;

            var result = await Task.Run(() =>
            {
                var parameters = item as object[];
                var pass1 = (parameters[0] as PasswordBox).Password;
                var pass2 = (parameters[1] as PasswordBox).Password;
                if (pass1 == pass2)
                {
                    _accountServices.ChangePassword(User, pass1);
                    return true;
                }
                else return false; 
            });

            if (result)
            {
                MessageBox.Show("Пароль был успешно изменен!");
                this.CloseAction();
            } else MessageBox.Show("Пароли не совпадают!");

            IsShow = false;
        }
    }
}