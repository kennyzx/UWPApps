using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
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
                    AppServiceText = await ConnectAppService(AppServiceText);
                });
            }                    
        }

        private async Task<string> ConnectAppService(string input)
        {
            if (_inventoryService == null)
            {
                _inventoryService = new AppServiceConnection();
                _inventoryService.AppServiceName = "InProcessAppService";
                _inventoryService.PackageFamilyName = "UWPBank.CommonAppService_m40mq4mvr89fy";

                var status = await _inventoryService.OpenAsync();
                if (status != AppServiceConnectionStatus.Success)
                {                     
                    return "Failed to connect.";
                }
            }

            //call the service
            var message = new ValueSet();
            message.Add("Request", "CAPITALIZE");
            message.Add("Value", input);
            AppServiceResponse response = await _inventoryService.SendMessageAsync(message);
            string result = "";
            if (response.Status == AppServiceResponseStatus.Success)
            {
                result += response.Message["Response"] as string;
            }

            return result;
        }

        private AppServiceConnection _inventoryService;
    }
}
