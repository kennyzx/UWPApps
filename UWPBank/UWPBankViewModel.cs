using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;

namespace UWPBank
{
    internal class UWPBankViewModel : ViewModelBase
    {
        public UWPBankViewModel()
        {
            _contentPage = typeof(RootVisual);
        }

        private Type _contentPage;
        public Type ContentPage
        {
            get { return _contentPage; }
            set
            {
                _contentPage = value;
                RaisePropertyChanged("ContentPage");
            }
        }

        public RelayCommand<String> NavigationCommand
        {
            get
            {
                return new RelayCommand<String>((pageName) =>
                {
                    try
                    {
                        Type nextPage = Type.GetType(pageName);
                        if (nextPage != null)
                            ContentPage = nextPage;
                    }
                    catch { }
                });
                //, (typeName) => {
                //    return true;
                //});
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

        public RelayCommand ConsumeAppServiceCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }                    
        }
    }
}
