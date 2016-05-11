using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BookingBlock.EntityFramework;
using BookingBlock.Identity;
using BookingBlock.WebApplication.Models;
using BookingBlock.WebApplication.Models.ValidationAttributes;
using MarkEmbling.PostcodesIO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace BookingBlock.WebApplication.ApiControllers
{
    public static class ApplicationDbContextUserStoreExtensions
    {
        public static ApplicationUserStore CreateApplicationUserStore(this ApplicationDbContext applicationDbContext)
        {
            return new ApplicationUserStore(applicationDbContext);
        }
    }

    public class ChangePasswordRequest
    {
        [BookingBlockPasswordValidator, Required]
        public string CurrentPassword { get; set; }

        [BookingBlockPasswordValidator, Required]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match.")]
        [Required]
        public string ConfirmNewPassword { get; set; }
    }


    [System.Web.Http.RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        /// <summary>
        /// Allows a user to change there password.
        /// </summary>
        /// <param name="changePasswordRequest">The change password request containing the users current password and the new password.</param>
        /// <returns>A HTTP action result containing the result of the change password request.</returns>
        [Route("change-password"), HttpPost]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            if (changePasswordRequest == null)
            {
                return Content(HttpStatusCode.BadRequest, "Change password request was not given.");
            }

            if (!IsUserAuthenticated)
            {
                return Content(HttpStatusCode.Forbidden, "You must be logged in to change your password");
            }

            if (!ModelState.IsValid)
            {
                return InvalidModel();
            }

            var applicationUserStore = db.CreateApplicationUserStore();

            ApplicationUserManager applicationUserManager = new ApplicationUserManager(applicationUserStore);
            
            string userId = base.UserId;

            IdentityResult result = await applicationUserManager.ChangePasswordAsync(userId, changePasswordRequest.CurrentPassword,
                changePasswordRequest.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(string.Join(", ", result.Errors));
        } 

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

        private async Task<AccountRegistrationRequest> GenerateRadomAccount()
        {
            AccountRegistrationRequest accountRegistrationRequest = new AccountRegistrationRequest();

            accountRegistrationRequest.Password = await GenerateRadomPassword();
            accountRegistrationRequest.ConfirmPassword = accountRegistrationRequest.Password;

            HttpClient ApiControllers = new HttpClient();

            var json = await ApiControllers.GetStringAsync("https://randomuser.me/api/?nat=gb");

            var t = JsonConvert.DeserializeObject<RandomUserMeResponse>(json);

            accountRegistrationRequest.DateOfBirth = (new DateTime(1970, 1, 1).AddSeconds(t.results[0].dob));
            accountRegistrationRequest.Gender = t.results[0].gender;
            accountRegistrationRequest.FirstName = t.results[0].name.first;
            accountRegistrationRequest.LastName = t.results[0].name.last;
            accountRegistrationRequest.EmailAddress = t.results[0].email;
            accountRegistrationRequest.MobileNumber = t.results[0].cell.Replace("-", "");

            //Latitude,Longitude,Number,Road,Locality,Post town,Admin area level 3,Admin area level 2,Admin area level 1,Postcode,Country

            string[] d;

            do
            {
                d = RA();

                accountRegistrationRequest.AddressLine1 = d[2] + " " + d[3];
                accountRegistrationRequest.AddressLine2 = d[4];
                accountRegistrationRequest.TownCity = d[5];
                accountRegistrationRequest.Postcode = d[9];
                accountRegistrationRequest.Country = d[10];

            } while (d == null || string.IsNullOrWhiteSpace(d[2]));

            return accountRegistrationRequest;

        }
        class RandomDates
        {
            private Random random = new Random();

            public DateTime Date(DateTime? start = null, DateTime? end = null)
            {
                if (start.HasValue && end.HasValue && start.Value >= end.Value)
                    throw new Exception("start date must be less than end date!");

                DateTime min = start ?? DateTime.MinValue;
                DateTime max = end ?? DateTime.MaxValue;

                // for timespan approach see: http://stackoverflow.com/q/1483670/1698987
                TimeSpan timeSpan = max - min;

                // for random long see: http://stackoverflow.com/a/677384/1698987
                byte[] bytes = new byte[8];
                random.NextBytes(bytes);

                long int64 = Math.Abs(BitConverter.ToInt64(bytes, 0)) % timeSpan.Ticks;

                TimeSpan newSpan = new TimeSpan(int64);

                return min + newSpan;
            }
        }

        //[SwaggerResponse(HttpStatusCode.OK, "Returns 200 : OK when a user with the given email address has been sucessfully found in the database. The body of the response will contain the ID of the user.", typeof(string))]
        //[SwaggerResponse(HttpStatusCode.BadRequest, "Return 400 : Bad Request when the email address supplied is not given or is not a valid email address.")]
        //[SwaggerResponse(HttpStatusCode.NotFound, "Returns 404 : Not Found if a user in the database could not be found with the given in.")]
        [Route("get-userid"), HttpGet]
        public async Task<IHttpActionResult> GetUserId(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Content(HttpStatusCode.BadRequest, $"A value for the parameter {nameof(email)} must be supplied.");
            }

            ApplicationUserStore userStore = new ApplicationUserStore(db);

            ApplicationUser applicationUser = await userStore.FindByEmailAsync(email);

            if (applicationUser == null)
            {
                return Content(HttpStatusCode.NotFound,
                    $"A user with the given e-mail address {email} could not be found");
            }

            return Ok(applicationUser.Id);
        }

        [Route("create-random-account"), HttpGet]
        public async Task<IHttpActionResult> CreateRandomAccount()
        {
            var a = await GenerateRadomAccount();
            RandomDates randomDates = new RandomDates();

            var registrationDate = randomDates.Date(new DateTime(2016, 1, 1), DateTime.Now);

            return await Register2(a, registrationDate, true);
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

        private async Task<IHttpActionResult> Register2(AccountRegistrationRequest accountRegistrationRequest, DateTime? registrationDate, bool dummy = false)
        {
            if (accountRegistrationRequest != null)
            {
                if (ModelState.IsValid || IsUserAuthenticated)
                {
                    ApplicationDbContext context = ApplicationDbContext.Create();

                    UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);

                    ApplicationUserManager userManager = new ApplicationUserManager(userStore);

                    string password = accountRegistrationRequest.Password;

                    string[] addressParts = {
                        accountRegistrationRequest.AddressLine1,
                        accountRegistrationRequest.AddressLine2,
                        accountRegistrationRequest.TownCity
                    };

                    string address = string.Join(",\r\n",
                        addressParts.Where(part => !string.IsNullOrWhiteSpace(part)));

                    ApplicationUser newApplicationUser = new ApplicationUser();


                    if (IsUserAuthenticated)
                    {
                        newApplicationUser = await userStore.FindByIdAsync(this.UserId);
                    }


                    if (accountRegistrationRequest.FirstName != null)
                        newApplicationUser.FirstName = accountRegistrationRequest.FirstName;
                    if (accountRegistrationRequest.LastName != null)
                        newApplicationUser.LastName = accountRegistrationRequest.LastName;

                    newApplicationUser.Address = address;

                    if (accountRegistrationRequest.Postcode != null)
                        newApplicationUser.Postcode = accountRegistrationRequest.Postcode;

                    if (accountRegistrationRequest.EmailAddress != null)
                    {
                        newApplicationUser.Email = accountRegistrationRequest.EmailAddress;


                    }

                    if (!IsUserAuthenticated)
                    {
                        if (accountRegistrationRequest.EmailAddress != null)
                            newApplicationUser.UserName = accountRegistrationRequest.EmailAddress;
                    }

                    if (accountRegistrationRequest.DateOfBirth != default(DateTime))
                    {
                        newApplicationUser.DateOfBirth = accountRegistrationRequest.DateOfBirth;
                    }

                    

                    if (accountRegistrationRequest.Gender != null)
                    {

                        if (accountRegistrationRequest.Gender.Contains("female"))
                        {
                            newApplicationUser.Gender = Gender.Female;
                        }
                    }


                    if (registrationDate != null)
                    {
                        newApplicationUser.RegistrationDate = registrationDate.Value;
                    }
                    else
                    {
                        newApplicationUser.RegistrationDate = DateTime.Now;
                    }

                    if (accountRegistrationRequest.MobileNumber != null)
                        newApplicationUser.PhoneNumber = accountRegistrationRequest.MobileNumber;

                    if (accountRegistrationRequest.Postcode != null)
                    {
                        newApplicationUser.Location = PostcodesService.Lookup(newApplicationUser.Postcode);
                    }
                        

                    if (IsUserAuthenticated)
                    {
                        var result2 = await userManager.UpdateAsync(newApplicationUser);

                        if (result2.Succeeded)
                        {
                            return Ok();
                        }

                        return BadRequest(string.Join(", ", result2.Errors));
                    }


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

        [HttpGet, Route("my-info")]
        public async Task<IHttpActionResult> MyInfo()
        {
            AccountRegistrationRequest ar = new AccountRegistrationRequest();

            if (IsUserAuthenticated)
            {

                ApplicationDbContext context = ApplicationDbContext.Create();

                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);

                ApplicationUserManager userManager = new ApplicationUserManager(userStore);


                var usr = await userManager.FindByIdAsync(this.UserId);

                if (usr != null)
                {
                    ar.AddressLine1 = usr.Address;
                    ar.DateOfBirth = usr.DateOfBirth;
                    ar.EmailAddress = usr.Email;
                    ar.FirstName = usr.FirstName;
                    ar.LastName = usr.LastName;
                    ar.Gender = usr.Gender.ToString();
                    ar.MobileNumber = usr.PhoneNumber;

                }

            }


            return Ok(ar);
        }

        [HttpPost, Route("Update")]
        public async Task<IHttpActionResult> Update(AccountRegistrationRequest accountRegistrationRequest)
        {

            return await Register2(accountRegistrationRequest, DateTime.Now);
        }

        [HttpPost, Route("register")]
        public async Task<IHttpActionResult> Register(AccountRegistrationRequest accountRegistrationRequest)
        {

            return await Register2(accountRegistrationRequest, DateTime.Now);
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