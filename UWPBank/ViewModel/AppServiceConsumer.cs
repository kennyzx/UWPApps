using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace UWPBank.ViewModel
{
    public class AppServiceConsumer
    {
        public static async Task<string> ConnectAppService(string input)
        {
            if (_inventoryService == null)
            {
                _inventoryService = new AppServiceConnection()
                {
                    AppServiceName = "InProcessAppService",
                    PackageFamilyName = "UWPBank.CommonAppService_h461r800hztwe"
                };
                var status = await _inventoryService.OpenAsync();
                if (status != AppServiceConnectionStatus.Success)
                {
                    return "Failed to connect.";
                }
            }

            //call the service
            var message = new ValueSet
            {
                { "Request", "CAPITALIZE" },
                { "Value", input }
            };
            AppServiceResponse response = await _inventoryService.SendMessageAsync(message);
            string result = "";
            if (response.Status == AppServiceResponseStatus.Success)
            {
                result += response.Message["Response"] as string;
            }

            return result;
        }

        private static AppServiceConnection _inventoryService;
    }
}
