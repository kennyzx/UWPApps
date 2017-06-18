﻿using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace UWPBank
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var _navigationService = new NavigationService();
            _navigationService = new NavigationService();
            _navigationService.Configure(NavigationService.RootPageKey, typeof(MainPage));
            _navigationService.Configure("AppServiceConsumerPage", typeof(AppServiceConsumerPage));
            _navigationService.Configure("AdvertisementPage", typeof(AdvertisementPage));
            _navigationService.Configure("MapControlPage", typeof(MapControlPage));
            _navigationService.Configure("LaunchDesktopProcessPage", typeof(Desktop.LaunchDesktopProcessPage));

            //Register your services used here
            SimpleIoc.Default.Register<INavigationService>(() => _navigationService);
            SimpleIoc.Default.Register<UWPBankViewModel>();
        }

        public UWPBankViewModel UWPBankViewModelInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UWPBankViewModel>();
            }
        }
    }
}
