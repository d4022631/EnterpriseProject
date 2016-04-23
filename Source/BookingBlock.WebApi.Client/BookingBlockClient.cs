using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookingBlock.WebApi.Client
{
    public class BookingBlockClient
    {
        public readonly string _url;

        private HttpClient httpClient;

        public BookingBlockClient(string url = "https://localhost:44383/")
        {
            _url = url;

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_url);
        }

        public void SetLiveUrl()
        {
            httpClient.BaseAddress = new Uri("https://bookingblock.azurewebsites.net/");
        }

        public void SetDevUrl()
        {
            httpClient.BaseAddress = new Uri(_url);
        }

        public async Task<IEnumerable<ApplicationUserInfo>> IdentityGetUsersAync()
        {
            //api/identity/get-users

            var t = await httpClient.GetStringAsync("api/identity/get-users");

            return JsonConvert.DeserializeObject<IEnumerable<ApplicationUserInfo>>(t);
        }

        public string ApiKey { get; set; }

        private string AddApiKey(string url)
        {
            var newUrl = url;

            if (!string.IsNullOrWhiteSpace(ApiKey))
            {
                if (newUrl.Contains("?"))
                {
                    newUrl += "&api_key=" + ApiKey;
                }
                else
                {
                    newUrl += "?api_key=" + ApiKey;
                }
            }


            return newUrl;
        }

        private string GetString(string url)
        {
            var newUrl = AddApiKey(url);

            return httpClient.GetStringAsync(newUrl).GetAwaiter().GetResult();
        }

        private T GetJsonObject<T>(string url)
        {
            var q = GetString(url);

            return JsonConvert.DeserializeObject<T>(q);
        }

        public IEnumerable<string> PostcodesAutocomplete(string pc)
        {

            return GetJsonObject<IEnumerable<string>>("api/postcodes/autocomplete/" + pc);
        }

        public string IdentityWhoAmI()
        {
            return GetJsonObject<string>("api/identity/who");
        }

        public void BusinessesRegister(BusinessRegistrationData businessRegistrationData)
        {
            var q = httpClient.PostAsJsonAsync(AddApiKey("api/businesses/regster"), businessRegistrationData).Result;

            q.EnsureSuccessStatusCode();
        }

        public UserBusinessInfoList BusinessesMyBusinesses()
        {

            return GetJsonObject<UserBusinessInfoList>("api/businesses/my-businesses");
        }

        public void BusinessesChangeType(ChangeBusinessTypeRequest request)
        {
            var q = httpClient.PostAsJsonAsync(AddApiKey("api/businesses/change-type"), request).Result;

            q.EnsureSuccessStatusCode();
        }

        public void BusinessesChangeName(ChangeBusinessNameRequest request)
        {
            var q = httpClient.PostAsJsonAsync(AddApiKey("api/businesses/change-name"), request).Result;

            q.EnsureSuccessStatusCode();
        }

        public void BusinessesChangeAddress(ChangeBusinessAddressRequest request)
        {
            var q = httpClient.PostAsJsonAsync(AddApiKey("api/businesses/change-address"), request).Result;

            q.EnsureSuccessStatusCode();
        }

        public void BusinessesRandom()
        {
            // do something
        }
    }
}
