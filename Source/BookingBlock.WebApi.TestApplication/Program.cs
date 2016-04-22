using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for web server to start. Press any key when ready...");
            Console.ReadKey(true);

            Task.Run(Action);

            Console.WriteLine("Press escape to exit...");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                
            }

        }

        private static async Task Action()
        {
            Console.WriteLine("Getting users...");
            BookingBlockClient client = new BookingBlockClient();

            var t = await client.IdentityGetUsersAync();

            foreach (ApplicationUserInfo applicationUserInfo in t)
            {
                Console.WriteLine($"\r{applicationUserInfo.Id} [{applicationUserInfo.Email}]");
            }

        }
    }
}
