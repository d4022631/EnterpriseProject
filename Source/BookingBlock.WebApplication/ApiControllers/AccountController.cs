using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace BookingBlock.WebApplication.ApiControllers
{
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }

    [System.Web.Http.RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        private string GenerateRadonPassword(PasswordValidator passwordValidator)
        {
            Random random = new Random();

            int passwordLength = passwordValidator.RequiredLength + random.Next(0, 8);

         

            char[] chars = new char[72];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!\"£$%^&*()".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[passwordLength];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(passwordLength);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();

        }

        private async Task<Account> GenerateRadomAccount()
        {
            Account account = new Account();

            account.Password = await GenerateRadomPassword();
            account.ConfirmPassword = account.Password;

            HttpClient ApiControllers = new HttpClient();

            var json = await ApiControllers.GetStringAsync("https://randomuser.me/api/?nat=gb");

            var t = JsonConvert.DeserializeObject<RandomUserMeResponse>(json);

            account.DateOfBirth = (new DateTime(1970, 1, 1).AddSeconds(t.results[0].dob));
            account.Gender = t.results[0].gender;
            account.FirstName = t.results[0].name.first;
            account.LastName = t.results[0].name.last;
            account.EmailAddress = t.results[0].email;
            account.MobileNumber = t.results[0].cell.Replace("-", "");

            //Latitude,Longitude,Number,Road,Locality,Post town,Admin area level 3,Admin area level 2,Admin area level 1,Postcode,Country

            string[] d;

            do
            {
                d = RA();

                account.AddressLine1 = d[2] + " " + d[3];
                account.AddressLine2 = d[4];
                account.TownCity = d[5];
                account.Postcode = d[9];
                account.Country = d[10];

            } while (d == null || string.IsNullOrWhiteSpace(d[2]));

            return account;

        }

        [Route("create-random-account"), HttpGet]
        public async Task<IHttpActionResult> CreateRandomAccount()
        {
            var a = await GenerateRadomAccount();

            return await Register(a);
        }


        [Route("random-account"), HttpGet]
        public async Task<IHttpActionResult> RandomAccount()
        {


            return Ok(GenerateRadomAccount());
        }

        private string[] RA()
        {
            string root = HttpContext.Current.Server.MapPath("~/Content/data");

            string[] files = Directory.GetFiles(root);

            string file = files.PickRandom();

            List<string[]> dataList = new List<string[]>();

            using (var ff = File.OpenRead(file))
            {
                using (StreamReader streamReader = new StreamReader(ff))
                {
                    int lineCount = 0;

                    while (true)
                    {
                        var t = streamReader.ReadLine();

                        if (t == null)
                        {
                            break;
                        }

                        if (lineCount > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(t))
                            {
                                string[] columns = t.Split(',');

                                dataList.Add(columns);
                            }
                        }

                        lineCount++;
                    }
                }
            }

            var radom = dataList.PickRandom();

            return radom;
        }

        //http://www.doogal.co.uk/RandomAddresses.php
        [Route("random-address"), HttpGet]
        public async Task<IHttpActionResult> RandomAddress()
        {
            


            //Latitude,Longitude,Number,Road,Locality,Post town,Admin area level 3,Admin area level 2,Admin area level 1,Postcode,Country

            return Ok(string.Join(",", RA()));
        }

        private async Task<string> GenerateRadomPassword()
        {
            BookingBlockPasswordValidator passwordValidator = new BookingBlockPasswordValidator();

            bool isPasswordValid = false;

            string password = String.Empty;

            do
            {
                password = GenerateRadonPassword(passwordValidator);

                var result = await passwordValidator.ValidateAsync(password);

                isPasswordValid = result.Succeeded;

            } while (!isPasswordValid);

            return password;
        }

        [Route("random-password"), HttpGet]
        public async Task<IHttpActionResult> RadomPassword()
        {
            return Ok(await GenerateRadomPassword());
        }

        [HttpPost, Route("register")]
        public async Task<IHttpActionResult> Register(Account account)
        {
            if (account != null)
            {
                if (ModelState.IsValid)
                {
                    ApplicationDbContext context = ApplicationDbContext.Create();

                    UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);

                    ApplicationUserManager userManager = new ApplicationUserManager(userStore);

                    string password = account.Password;

                    string address = string.Join(",\r\n",
                        new string[]
                        {
                            account.AddressLine1,
                            account.AddressLine2,
                            account.TownCity
                        });

                    ApplicationUser newApplicationUser = new ApplicationUser();

                    newApplicationUser.FirstName = account.FirstName;
                    newApplicationUser.LastName = account.LastName;

                    newApplicationUser.Address = address;
                    newApplicationUser.Postcode = account.Postcode;

                    newApplicationUser.Email = account.EmailAddress;
                    newApplicationUser.UserName = account.EmailAddress;

                    newApplicationUser.DateOfBirth = account.DateOfBirth;

                    if (account.Gender.Contains("female"))
                    {
                        newApplicationUser.Gender = Gender.Female;
                    }

                    PostcodesIOClient client = new PostcodesIOClient();

                    var postcodeLookup = client.Lookup(newApplicationUser.Postcode);

                    newApplicationUser.Location = GeoUtils.CreatePoint(postcodeLookup.Latitude, postcodeLookup.Latitude);

                    

                   var result = await userManager.CreateAsync(newApplicationUser, password);

                    if (result.Succeeded)
                    {
                        return Ok();
                    }

                    return BadRequest(string.Join(", ", result.Errors));
                }

                string validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return BadRequest(validationErrors);
            }

            return BadRequest("registration data is null.");
        }

        [HttpGet, Route("countries")]
        public async Task<IHttpActionResult> Countries()
        {
            CultureInfo[] specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            List< string > countries = new List<string>();

            foreach (CultureInfo specificCulture in specificCultures)
            {
                var lcid = specificCulture.LCID;

                RegionInfo regionInfo = new RegionInfo(lcid);

                if (!countries.Contains(regionInfo.EnglishName))
                {
                    countries.Add(regionInfo.EnglishName);
                }

               
            }

            await Task.Run(() => countries.Sort());

            return Ok(countries);
        } 
    }
}