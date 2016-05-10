using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BookingBlock.WebApi.Client;

namespace BookingBlock.WebApi.TestApplication
{
    class Program
    {
        static BookingBlockClient bookingBlockClient = new BookingBlockClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for web server to start. Press any key when ready...");
            Console.ReadKey(true);

            Task.Run(MainMenu);



            autoResetEvent.WaitOne();
        }

        static AutoResetEvent autoResetEvent = new AutoResetEvent(false);


        private static async Task MainMenu()
        {
           Menu maiMenu = new Menu() { Title = "Main Menu" };

            maiMenu.Add(ConsoleKey.B, "Businesses", BusinessAction);
            maiMenu.Add(ConsoleKey.P, "Postcodes", PostCodesMenu);
            maiMenu.Add(ConsoleKey.A, "Account", AccountsMenu);
            maiMenu.Add(ConsoleKey.X, "Extra", Action);
            maiMenu.Add(ConsoleKey.S, "Search", SearchAction);
            maiMenu.Run();

            autoResetEvent.Set();

        }

        private static void SearchAction()
        {
            Console.WriteLine("Search");

            string type = Read<string>("type");
            string postcode = Read<string>("postcode");
            double distance = Read<double>("distance");

            

        }

        private static void BusinessAction()
        {
            Menu businessMenu = new Menu() {Title = "Businesses"};

            businessMenu.Add(ConsoleKey.R, "Random", RandomBusiness);
            businessMenu.Add(ConsoleKey.C, "Create", CreateAction);
            businessMenu.Add(ConsoleKey.M, "My Businesses", MyBusinessesAction);
            businessMenu.Add(ConsoleKey.S, "Show Businesses", ShowBusinessAction);
            businessMenu.Run();
        }

        private static void ShowBusinessAction()
        {
            UserBusinessInfoList myBusinesses = bookingBlockClient.BusinessesMyBusinesses();

            if (myBusinesses.Count != 0)
            {
                int index = 0;

                Menu menu = new Menu() {Title = $"Business: {myBusinesses[index].Name} [{myBusinesses[index].Id}]"};
                menu.Add(ConsoleKey.P, "Previous", () =>
                {
                    index--;

                    if (index < 0)
                    {
                        index = 0;
                    }

                    menu.Title = $"Business: {myBusinesses[index].Name} [{myBusinesses[index].Id}]";
                });
                menu.Add(ConsoleKey.N, "Next", () =>
                {
                    index++;

                    if (index > myBusinesses.Count - 1)
                    {
                        index = myBusinesses.Count - 1;
                    }

                    menu.Title = $"Business: {myBusinesses[index].Name} [{myBusinesses[index].Id}]";
                });

                menu.Add(ConsoleKey.C, "Change", () =>
                {
                    BusinessActionAddress(myBusinesses[index]);
                } );

                menu.Run();
            }
        }

        private static void BusinessActionAddress(UserBusinessInfo myBusiness)
        {
            Menu menu = new Menu() {Title = $"Change Business: {myBusiness.Name} [{myBusiness.Id}]" };

            menu.Add(ConsoleKey.N, "Change Name", () => ChangeNameAction(myBusiness.Id));
            menu.Add(ConsoleKey.C, "Change Address", () => ChangeAddressAction(myBusiness.Id));
            menu.Add(ConsoleKey.T, "Change Type", () => ChangeTypeAction(myBusiness.Id));
            menu.Run();
        }

        private static void ChangeTypeAction(Guid id)
        {
            Console.WriteLine("Change Type");

            ChangeBusinessTypeRequest request = new ChangeBusinessTypeRequest();

            request.Type = Read<string>("Type");

            request.BusinessId = id;
            try
            {
                bookingBlockClient.BusinessesChangeType(request);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);

            }
        }

        private static void ChangeNameAction(Guid id)
        {
            Console.WriteLine("Change Name");

            ChangeBusinessNameRequest request = new ChangeBusinessNameRequest();

            request.Name = Read<string>("name");

            request.BusinessId = id;
            try
            {
                bookingBlockClient.BusinessesChangeName(request);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);

            }
        }

        private static void ChangeAddressAction(Guid id)
        {
            Console.WriteLine("Change Address");
            ChangeBusinessAddressRequest request = new ChangeBusinessAddressRequest();
            FillBusinessAddress(request);
            request.BusinessId = id;
            try
            {
                bookingBlockClient.BusinessesChangeAddress(request);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);

            }
        }

        private static void MyBusinessesAction()
        {
            try
            {
                var myBusinesses = bookingBlockClient.BusinessesMyBusinesses();

                foreach (UserBusinessInfo userBusinessInfo in myBusinesses)
                {
                    Console.WriteLine($"{userBusinessInfo.Name} [{userBusinessInfo.Id}] {userBusinessInfo.IsOwner}");
                }
            }
            catch (Exception exception)
            {
                
            }

            Pause();
        }

        private static T Read<T>(string prompt)
        {
            Console.Write($"{prompt}: ");

            var q = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(q))
            {
                try
                {
                    TypeConverter s = TypeDescriptor.GetConverter(typeof (T));

                    return (T) s.ConvertFromInvariantString(q);
                }
                catch (Exception)
                {
                    
                    //throw;
                }
            }

            return default(T);
        }

        private static void FillBusinessAddress(IBusinessAddress businessAddress)
        {
            businessAddress.AddressLine1 = Read<string>("Address 1");
            businessAddress.AddressLine2 = Read<string>("Address 2");
            businessAddress.TownCity = Read<string>("Town/City");
            businessAddress.Postcode = Read<string>("Postcode");
            businessAddress.Country = Read<string>("Country");
        }

        private static void CreateAction()
        {
            Console.WriteLine("Register New Business");
            BusinessRegistrationData businessRegistrationData = new BusinessRegistrationData();

            businessRegistrationData.Name = Read<string>("Name");
            businessRegistrationData.Type = Read<string>("Type");
            
            businessRegistrationData.ContactName = Read<string>("Contact Name");
            businessRegistrationData.ContactEmail = Read<string>("Contact E-Mail");
            businessRegistrationData.ContactNumber = Read<string>("Contact number");
            businessRegistrationData.ContactFax = Read<string>("Contact Fax");

            FillBusinessAddress(businessRegistrationData);
            

            var openingTimes = businessRegistrationData.GetOpeningTimes();

            foreach (DayOfWeek value in Enum.GetValues(typeof(DayOfWeek)))
            {
                openingTimes.SetOpeningTime(value, Read<DateTime?>($"Opening Time {value}"));

                if (openingTimes.GetOpeningTime(value).HasValue)
                {
                    openingTimes.SetClosingTime(value, Read<DateTime?>($"Closing Time {value}"));
                }
            }

            businessRegistrationData.Website = Read<string>("Website");


            try
            {//regster

                bookingBlockClient.BusinessesRegister(businessRegistrationData);

            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);


                //throw;
            }

            Pause();
        }

        private static void RandomBusiness()
        {
            bookingBlockClient.BusinessesRandom();
        }

        private static void Action()
        {
            Menu m = new Menu() {Title = "Extras"};

            m.Add(ConsoleKey.W, "Who Am I?", WhoAmIAction);
            m.Add(ConsoleKey.R, "Radom User", MakeRadom);
            m.Add(ConsoleKey.L, "List Users", ListUsers);
            m.Add(ConsoleKey.A, "Admin", ads);
            m.Add(ConsoleKey.S, "Toggle Source", Toggle);
            m.Run();

        }

        private static bool b = true;

        private static void Toggle()
        {
            if (b)
            {
                bookingBlockClient.SetLiveUrl();
            }
            else
            {
                bookingBlockClient.SetDevUrl();
            }

            b = !b;
        }

        private static void ads()
        {
            try
            {
                var t = bookingBlockClient.IdentityGetUsersAync().Result;

                var q = t.FirstOrDefault(info => info.Email== "webmaster@bookingblock.azurewebsites.net");

                bookingBlockClient.ApiKey = q.Id;

                WhoAmIAction();

            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);

                Pause();
                //throw;
            }
        }

        private static void ListUsers()
        {
            try
            {
                var t = bookingBlockClient.IdentityGetUsersAync().Result;


                foreach (ApplicationUserInfo applicationUserInfo in t)
                {
                    Console.WriteLine($"\t{applicationUserInfo.Id} {applicationUserInfo.Email} {applicationUserInfo.Username}");
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);

           
                //throw;
            }

            Pause();
        }

        private static void MakeRadom()
        {
            try
            {
                var t = bookingBlockClient.IdentityGetUsersAync().Result;

                var q = t.OrderBy(ts => Guid.NewGuid()).FirstOrDefault();

                bookingBlockClient.ApiKey = q.Id;

                WhoAmIAction();

            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);

                Pause();
                //throw;
            }
        }

        private static void WhoAmIAction()
        {
            Console.WriteLine("You are:");

            try
            {
                var q = bookingBlockClient.IdentityWhoAmI();

                Console.WriteLine($"\t{q}");

            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }

            Pause();

        }

        private static void AccountsMenu()
        {
            Menu postcodesMenu = new Menu() { Title = "Accounts" };



            postcodesMenu.Run();
        }

        private static void PostCodesMenu()
        {
            Menu postcodesMenu = new Menu() {Title = "Postcodes"};

            postcodesMenu.Add(ConsoleKey.A, "Autocomplete", PostcodeAutocomplete);
            
            postcodesMenu.Run();
        }

        private static void PostcodeAutocomplete()
        {
            Console.WriteLine("Postcode Autocomplete");
            Console.Write("Postcode: ");

            var pc = Console.ReadLine();

            try
            {
                IEnumerable<string> postcodes = bookingBlockClient.PostcodesAutocomplete(pc);

                Console.WriteLine(string.Join(", ", postcodes));
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
                
                //throw;
            }


            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
        }

        static void Pause()
        {

            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
        }
    }

    public class MenuKey
    {
        public ConsoleKey Key { get; set; }

        public string Title { get; set; }

        public Action Action { get; set; }
    }

    public class Menu
    {
        public string Title { get; set; }

        List<MenuKey> menuKeys = new List<MenuKey>();

        public void Add(ConsoleKey key, string title, Action action)
        {
            MenuKey k = new MenuKey();
            k.Key = key;
            k.Title = title;
            k.Action = action;

            menuKeys.Add(k);
        }

        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine(Title);
                foreach (MenuKey menuKey in menuKeys)
                {
                    Console.WriteLine($"\t{menuKey.Key}. {menuKey.Title}");
                }
                Console.WriteLine();
                Console.WriteLine("<esc>. Exit Application");

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    default:

                        foreach (MenuKey menuKey in menuKeys)
                        {
                            if (key == menuKey.Key)
                            {
                                Console.Clear();
                                menuKey.Action.Invoke();
                            }
                        }

                        break;
                }

            }
        }
    }
}
