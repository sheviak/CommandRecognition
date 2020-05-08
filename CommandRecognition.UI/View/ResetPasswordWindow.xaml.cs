using CommandRecognition.UI.ViewModel;
using System;
using System.Windows;

namespace CommandRecognition.UI.View
{
    /// <summary>
    /// Логика взаимодействия для ResetPasswordWindow.xaml
    /// </summary>
    public partial class ResetPasswordWindow : Window
    {
        public ResetPasswordWindow()
        {
            InitializeComponent();
            var vm = IocKernel.IocKernel.Get<ResetPasswordViewModel>();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
        }
    }
}
