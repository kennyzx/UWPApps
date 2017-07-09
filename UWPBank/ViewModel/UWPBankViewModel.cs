using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;

namespace UWPBank.ViewModel
{
    public class UWPBankViewModel : ViewModelBase
    {
        INavigationService _navigationService;

        public UWPBankViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }        

        public RelayCommand<String> NavigationCommand
        {
            get
            {
                return new RelayCommand<String>((key) =>
                {
                    try
                    {
                        _navigationService.NavigateTo(key);                        
                    }
                    catch { }
                });
            }
        }

        public RelayCommand NavigationToHomeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        _navigationService.NavigateTo(NavigationService.RootPageKey);
                    }
                    catch { }
                });
            }
        }

        public RelayCommand TriggerHockeyAppCrashCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    throw new Exception("This is an unhandled exception and will be reported to HockeyApp");
                });
            }
        }

        public RelayCommand FullScreenModeToggleCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var view = ApplicationView.GetForCurrentView();
                    if (view.IsFullScreenMode)
                    {
                        view.ExitFullScreenMode();
                    }
                    else
                    {
                        if (view.TryEnterFullScreenMode())
                        {

                        }
                        else
                        {

                        }
                    }
                });
            }
        }

        public RelayCommand LaunchDesktopProcessCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        await Full​Trust​Process​Launcher.LaunchFullTrustProcessForAppAsync("App");
                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        //if the runFullTrust capability is not declared in manifest
                        await new MessageDialog("Acess Denied: runFullTrust capability is not declared in manifest").ShowAsync();
                    }
                });
            }
        }

        private string _appServiceText;
        public string AppServiceText
        {
            get { return _appServiceText; }
            set
            {
                _appServiceText = value;
                RaisePropertyChanged("AppServiceText");
            }
        }

        public RelayCommand ConsumeAppServiceCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    AppServiceText = await AppServiceConsumer.ConnectAppService(AppServiceText);
                });
            }                    
        }

        private LibraryViewModel _LibraryViewModelInstance;
        public LibraryViewModel LibraryViewModelInstance
        {
            get
            {
                if (_LibraryViewModelInstance == null)
                {
                    _LibraryViewModelInstance = new LibraryViewModel();
                }
                return _LibraryViewModelInstance;
            }
        }
    }
}
