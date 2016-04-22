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
        private readonly string _url;

        private HttpClient httpClient;

        public BookingBlockClient(string url = "https://localhost:44383/")
        {
            _url = url;

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_url);
        }

        public async Task<IEnumerable<ApplicationUserInfo>> IdentityGetUsersAync()
        {
            //api/identity/get-users

            var t = await httpClient.GetStringAsync("api/identity/get-users");

            return JsonConvert.DeserializeObject<IEnumerable<ApplicationUserInfo>>(t);
        }
    }
}
