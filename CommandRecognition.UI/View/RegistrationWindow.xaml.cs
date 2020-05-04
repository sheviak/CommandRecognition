using CommandRecognition.UI.ViewModel;
using System;
using System.Windows;

namespace CommandRecognition.UI.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            var vm = IocKernel.IocKernel.Get<RegistrationViewModel>();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
        }
    }
}