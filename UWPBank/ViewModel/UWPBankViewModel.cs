using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Storage;
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

        private string _appServiceResult;
        public string AppServiceResult
        {
            get { return _appServiceResult; }
            set
            {
                _appServiceResult = value;
                RaisePropertyChanged("AppServiceResult");
            }
        }

        public RelayCommand ConsumeAppServiceCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    AppServiceResult = await AppServiceConsumer.ConnectAppService(AppServiceResult);
                });
            }                    
        }

        private PictureLibraryViewModel _PictureLibraryViewModelInstance;
        public PictureLibraryViewModel PictureLibraryViewModelInstance
        {
            get
            {
                if (_PictureLibraryViewModelInstance == null)
                {
                    _PictureLibraryViewModelInstance = new PictureLibraryViewModel();
                }
                return _PictureLibraryViewModelInstance;
            }
        }

        private AppThemeViewModel _AppThemeViewModelInstance;
        public AppThemeViewModel AppThemeViewModelInstance
        {
            get
            {
                if (_AppThemeViewModelInstance == null)
                {
                    _AppThemeViewModelInstance = new AppThemeViewModel();
                }
                return _AppThemeViewModelInstance;
            }
        }

        
        public AppMode SelectedAppMode
        {
            get
            {
                var val = ApplicationData.Current.LocalSettings.Values["AppMode"];
                if (val == null)
                {
                    ApplicationData.Current.LocalSettings.Values["AppMode"] = (int)AppMode.Feature;
                    return AppMode.Feature;
                }
                else return (AppMode)val;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values["AppMode"] = (int)value;
                RaisePropertyChanged("SelectedAppMode");
            }
        }
    }
}
