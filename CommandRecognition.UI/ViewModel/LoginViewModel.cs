using CommandRecognition.BL.Interfaces;
using CommandRecognition.UI.Command;
using CommandRecognition.UI.View;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommandRecognition.UI.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _login;
        public string Login { get => _login; set => SetField(ref _login, value); }

        private bool _isShow;
        public bool IsShow { get => _isShow; set => SetField(ref _isShow, value); }

        private string _errorMesssage;
        public string ErrorMessage { get => _errorMesssage; set => SetField(ref _errorMesssage, value); }

        private Visibility _visibility;
        public Visibility Visibility { get => _visibility; set => SetField(ref _visibility, value); }

        public ICommand _loginCommand;
        public ICommand _registerCommand;
        public ICommand LoginCommand { get { return _loginCommand ?? (_loginCommand = new DelegateCommand<object>(a => OnLoginCommand(a))); } }
        public ICommand RegisterCommand { get { return _registerCommand ?? (_registerCommand = new DelegateCommand<object>(a => OnRegisterCommand())); } }

        private IAccountServices _accountServices;
        public LoginViewModel(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        private async void OnLoginCommand(object item)
        {
            IsShow = true;
            Visibility = Visibility.Visible;
            ErrorMessage = string.Empty;

            var user = await Task.Run(() =>
            {
                var password = ((item as object) as PasswordBox).Password;
                var result = _accountServices.AutorizeUser(Login, password);
                return result;
            });

            if (user is null)
            {
                Visibility = Visibility.Collapsed;
                ErrorMessage = "Проверьте введенные данные!";
            } else
            {
                var mainView = new MainWindow();
                var mainViewModel = IocKernel.IocKernel.Get<MainViewModel>();
                mainViewModel.Initialize(user.Id);
                mainView.DataContext = mainViewModel;
                mainView.Show();
                this.CloseAction();
            }
        }

        private void OnRegisterCommand()
        {
            var registerWindow = new RegistrationWindow();
            registerWindow.ShowDialog();
        }
    }
}