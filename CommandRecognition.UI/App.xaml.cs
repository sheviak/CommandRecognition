using CommandRecognition.IOC;

namespace CommandRecognition.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            IocKernel.IocKernel.Initialize(new IocConfiguration());
        }
    }
}
