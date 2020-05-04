using CommandRecognition.UI.ViewModel;
using System;
using System.Windows;

namespace CommandRecognition.UI.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var vm = IocKernel.IocKernel.Get<LoginViewModel>();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
        }
    }
}