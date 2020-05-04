using CommandRecognition.BL.Interfaces;
using CommandRecognition.BL.Services;
using CommandRecognition.DAL.Context;
using CommandRecognition.DAL.Interface;
using CommandRecognition.DAL.UnitOfWork;
using Ninject.Modules;

namespace CommandRecognition.IOC
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();
            Bind<IApplicationContext>().To<ApplicationContext>().InSingletonScope();
            Bind<IAccountServices>().To<AccountServices>().InSingletonScope();
            Bind<IDataServices>().To<DataServices>().InSingletonScope();
        }
    }
}