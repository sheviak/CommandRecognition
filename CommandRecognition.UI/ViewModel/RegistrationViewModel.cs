using CommandRecognition.BL.Interfaces;
using CommandRecognition.UI.Command;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommandRecognition.UI.ViewModel
{
    public class RegistrationViewModel : BaseViewModel
    {
        private string _login;
        public string Login { get => _login; set => SetField(ref _login, value); }
        private string _question;
        public string Question { get => _question; set => SetField(ref _question, value); }
        private string _answer;
        public string Answer { get => _answer; set => SetField(ref _answer, value); }

        private IAccountServices _accountServices;
        public RegistrationViewModel(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        private ICommand _registerCommand;
        public ICommand RegisterCommand { get { return _registerCommand ?? (_registerCommand = new DelegateCommand<object>(a => OnRegisterCommand(a))); } }

        private async void OnRegisterCommand(object item)
        {
            var (status, message) = await Task.Run(() =>
            {
                var parameters = item as object[];
                var pass1 = parameters[0] as PasswordBox;
                var pass2 = parameters[1] as PasswordBox;

                return _accountServices.CreateAccount(Login, pass1.Password, pass2.Password, Question, Answer);
            });

            if (status)
            {
                MessageBox.Show(message);
                this.CloseAction();
            }
            else
            {
                MessageBox.Show(message);
            }
        }
    }
}