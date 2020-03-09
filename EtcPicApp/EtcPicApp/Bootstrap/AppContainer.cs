using System;
using Autofac;
using EtcPicApp.Contracts.Repository;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Contracts.Services.General;
using EtcPicApp.Services.Data;
using EtcPicApp.Services.General;
using EtcPicApp.Services.Repository;
using EtcPicApp.ViewModels;

namespace EtcPicApp.Bootstrap
{
    public class AppContainer
    {
        private static IContainer _container;

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //ViewModels
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<MenuViewModel>();
            builder.RegisterType<JobListViewModel>();
            builder.RegisterType<LoginViewModel>();
            builder.RegisterType<PhotoListViewModel>();
            builder.RegisterType<PhotoDetailViewModel>();
            //General
            builder.RegisterType<GenericRepository>().As<IGenericRepository>();

            //services - data
            builder.RegisterType<ApiService>().As<IApiService>();
            builder.RegisterType<DataService>().As<IDataService>();
            builder.RegisterType<SqlLiteService>().As<ISqlLiteService>();
            builder.RegisterType<SynchronizationService>().As<ISynchronizationService>();

            //services - general
            builder.RegisterType<ConnectionService>().As<IConnectionService>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<DialogService>().As<IDialogService>();
            builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();

            _container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return _container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }    
}
