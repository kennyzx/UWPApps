using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace UWPBank.ViewModel
{
    public class AppThemeViewModel : ViewModelBase
    {
        public ElementTheme AppTheme
        {
            get
            {
                var val = ApplicationData.Current.LocalSettings.Values["AppTheme"];
                if (val == null)
                {
                    ApplicationData.Current.LocalSettings.Values["AppTheme"] = (int)ElementTheme.Default;
                    return ElementTheme.Default;
                }
                else return (ElementTheme)val;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values["AppTheme"] = (int)value;
                RaisePropertyChanged("AppTheme");
            }
        }
    }
}
