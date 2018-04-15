using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace UWPBank.ViewModel
{
    class GoogleOAuth
    {
        public async Task SignIn()
        {
            try
            {
                //Stackoverflow.com also uses Google OAuth to login users (as one of the options), this is what their URL looks like
                //https://accounts.google.com/signin/oauth/oauthchooseaccount?client_id=717762328687-p17pldm5fteklla3nplbss3ai9slta0a.apps.googleusercontent.com&as=zzNLi1F9oliXUN37C-TxGg&destination=https%3A%2F%2Fstackauth.com&approval_state=!ChQydUhBWmZIU3lkTm5wY0NDVzdMMBIfY195aUc4cExNOFVYd0ktM2lWMU9TaWQzRVBXWExCWQ%E2%88%99AB8iHBUAAAAAWtSlS6MQmJSMn4_79Ea3e8KtNs1hAMZc&xsrfsig=AHgIfE9oALSfQnNfGN5c8K9RBcKIliGaig&flowName=GeneralOAuthFlow

                String GoogleURL = "https://accounts.google.com/o/oauth2/auth?client_id="
                + Uri.EscapeDataString("{REPLACE WITH THE ACTUAL CLIENT ID}.apps.googleusercontent.com")
                + "&redirect_uri="
                + Uri.EscapeDataString("urn:ietf:wg:oauth:2.0:oob")
                + "&response_type=code&scope=" 
                + Uri.EscapeDataString("http://picasaweb.google.com/data");

                Uri StartUri = new Uri(GoogleURL);
                // When using the desktop flow, the success code is displayed in the html title of this end uri
                Uri EndUri = new Uri("https://accounts.google.com/o/oauth2/approval");

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, StartUri, EndUri);
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    System.Diagnostics.Debug.WriteLine(WebAuthenticationResult.ResponseData.ToString());
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    System.Diagnostics.Debug.WriteLine("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
