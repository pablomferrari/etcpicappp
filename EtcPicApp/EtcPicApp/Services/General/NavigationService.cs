﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Contracts.Services.General;
using EtcPicApp.ViewModels;
using EtcPicApp.ViewModels.Base;
using EtcPicApp.Views;
using Xamarin.Forms;

namespace EtcPicApp.Services.General
{
    public class NavigationService : INavigationService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly Dictionary<Type, Type> _mappings;

        protected Application CurrentApplication => Application.Current;

        public NavigationService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public async Task InitializeAsync()
        {
            if (_authenticationService.IsUserAuthenticated())
            {
                await NavigateToAsync<MainViewModel>();
            }
            else
            {
                await NavigateToAsync<LoginViewModel>();
            }
        }

        public async Task ClearBackStack()
        {
            await CurrentApplication.MainPage.Navigation.PopToRootAsync();
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                mainPage.Detail.Navigation.RemovePage(
                    mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public async Task PopToRootAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                await mainPage.Detail.Navigation.PopToRootAsync();
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            var page = CreatePage(viewModelType, parameter);

            if (page is MainView || page is LoginView)
            {
                CurrentApplication.MainPage = page;
            }
            else if (CurrentApplication.MainPage is MainView)
            {
                var mainPage = CurrentApplication.MainPage as MainView;

                if (mainPage.Detail is ETLNavigationPage navigationPage)
                {
                    var currentPage = navigationPage.CurrentPage;

                    if (currentPage.GetType() != page.GetType())
                    {
                        await navigationPage.PushAsync(page);
                    }
                }
                else
                {
                    navigationPage = new ETLNavigationPage(page);
                    mainPage.Detail = navigationPage;
                }

                mainPage.IsPresented = false;
            }
            else
            {
                var navigationPage = CurrentApplication.MainPage as ETLNavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage = new ETLNavigationPage(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }

        protected Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            //if (page != null)
            //{
            //    page.BindingContext = parameter;
            //}

            return page;
        }

        private void CreatePageViewModelMappings()
        {
            _mappings.Add(typeof(LoginViewModel), typeof(LoginView));
            _mappings.Add(typeof(MainViewModel), typeof(MainView));
            _mappings.Add(typeof(MenuViewModel), typeof(MenuView));
            _mappings.Add(typeof(JobListViewModel), typeof(JobListView));
            _mappings.Add(typeof(MaterialListViewModel), typeof(MaterialListView));
            _mappings.Add(typeof(SampleListViewModel), typeof(SampleListView));
            _mappings.Add(typeof(MaterialDetailViewModel), typeof(MaterialDetailView));
            _mappings.Add(typeof(SampleDetailViewModel), typeof(SampleDetailView));
            _mappings.Add(typeof(RoomsViewModel), typeof(RoomsView));
            _mappings.Add(typeof(DeliveriesViewModel), typeof(DeliveriesView));
            _mappings.Add(typeof(HistoryListViewModel), typeof(HistoryListView));
            _mappings.Add(typeof(HistoryDetailViewModel), typeof(HistoryDetailView));
            _mappings.Add(typeof(ReportViewModel), typeof(ReportView));
        }
    }
}
