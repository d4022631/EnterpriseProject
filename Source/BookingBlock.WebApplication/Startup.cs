using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using BookingBlock.WebApplication.Models;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Resources;
using IdentityServer3.Core.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Twitter;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookingBlock.WebApplication.Startup))]
namespace BookingBlock.WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureIdentityServer(app);

            ConfigureAuth(app);
        }

        private void ConfigureIdentityServer(IAppBuilder app)
        {
            var pathMatch = IdentityServerAppSettings.PathMatch;

            app.Map(pathMatch, builder =>
            {
                var authenticationOptions = new AuthenticationOptions();

                authenticationOptions.EnablePostSignOutAutoRedirect = true;

                authenticationOptions.IdentityProviders = ConfigureIdentityProviders;



                builder.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Booking Block: IdentityServer",
                    SigningCertificate = LoadCertificate(),

                    Factory = CreateServiceFactory(),
                    AuthenticationOptions = authenticationOptions,


                });
            });
        }

        private static IdentityServerServiceFactory CreateServiceFactory()
        {
            var factory = new IdentityServerServiceFactory();

            factory.UseInMemoryScopes(IdentityServerScopes.Get());
            factory.UseInMemoryUsers(IdentityServerUsers.Get());
      
            factory.ClientStore = new Registration<IClientStore, IdentityServerClientStore>();


            factory.UserService = new Registration<IUserService>(Factory);

            //   factory.ClientStore = new Registration<IClientStore, BookingBlockClientStore>();
            // factory.EventService = new Registration<IEventService, BookingBlockIdentityEventService>();

            // register the application db context with the factory.
            //  factory.Register(new Registration<BookingBlockDbContext>(resolver => BookingBlockDbContext.Create()));

            // factory.UserService = new Registration<IUserService>(UserServiceFactory);
            //factory.Register(new Registration<ApplicationUserManager>());
            //factory.Register(new Registration<UserStore<User>>());

            return factory;
        }

        private static IUserService Factory(IDependencyResolver dependencyResolver)
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());

            ApplicationUserManager manager = new ApplicationUserManager(userStore);

            return new IdentityServerUserService(manager);
        }


        private static void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                AuthenticationType = "Google",
                Caption = "Sign-in with Google",
                SignInAsAuthenticationType = signInAsType,

                ClientId = "28751939105-qp1ud0pms2pffpu9ssji6rhhpms45bhu.apps.googleusercontent.com",
                ClientSecret = "ul3fdy8YOi3nWmsdQ9rap6Vn"
            });

            TwitterAuthenticationOptions twitterAuthenticationOptions = new TwitterAuthenticationOptions();


            twitterAuthenticationOptions.ConsumerKey = "9w4EYvCfs9wjfqZDeCDll4ZBg";
            twitterAuthenticationOptions.ConsumerSecret = "jcQ35Eo8ZjQMLVOddqwB0OyzUuTPN3h8XZrjm4kFZGOCOAWFas";
            twitterAuthenticationOptions.Caption = "Sign-in with Twitter";
            twitterAuthenticationOptions.SignInAsAuthenticationType = signInAsType;

            twitterAuthenticationOptions.BackchannelCertificateValidator =
                new Microsoft.Owin.Security.CertificateSubjectKeyIdentifierValidator(new[]
                {
                        "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                        "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                        "7FD365A7C2DDECBBF03009F34339FA02AF333133",
                        // VeriSign Class 3 Public Primary Certification Authority - G5
                        "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                        "‎add53f6680fe66e383cbac3e60922e3b4c412bed", // Symantec Class 3 EV SSL CA - G3
                        "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", // VeriSign Class 3 Primary CA - G5
                        "5168FF90AF0207753CCCD9656462A212B859723B", // DigiCert SHA2 High Assurance Server C‎A 
                        "B13EC36903F8BF4701D498261A0802EF63642BC3" // DigiCert High Assurance EV Root CA
                });

            app.UseTwitterAuthentication(twitterAuthenticationOptions);


            MicrosoftAccountAuthenticationOptions microsoftAccountAuthenticationOptions = new MicrosoftAccountAuthenticationOptions();

            microsoftAccountAuthenticationOptions.ClientId = "000000004018E347";
            microsoftAccountAuthenticationOptions.ClientSecret = "MOqHuPKgbLngN6EHrR6IR0w7779tFLyb";
            microsoftAccountAuthenticationOptions.Caption = "Sign-in with Microsoft";
            microsoftAccountAuthenticationOptions.SignInAsAuthenticationType = signInAsType;

            app.UseMicrosoftAccountAuthentication(microsoftAccountAuthenticationOptions);

            app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
            {
                Caption = "Sign-in with Facebook",
                SignInAsAuthenticationType = signInAsType,
                AppId = "1585563455100467",
                AppSecret = "ff9ab6a79875e037db94701e79c57f0d"
            });

            //if (IdentityServer3AppSettings.AdditionalIdentityProvidersEnabled)
            //{
            //    // if google authentication is enabled.
            //    if (GoogleAuthenticationAppSettings.Enabled)
            //    {

            //    }


            //    LinkedInAuthenticationOptions linkedInAuthenticationOptions = new LinkedInAuthenticationOptions();


            //    linkedInAuthenticationOptions.ClientId = "77vc0fkijvxj8l";
            //    linkedInAuthenticationOptions.ClientSecret = "44ALEkiMBMc2rsSK";
            //    linkedInAuthenticationOptions.Caption = "Sign-in with LinkedIn";
            //    linkedInAuthenticationOptions.SignInAsAuthenticationType = signInAsType;

            //    app.UseLinkedInAuthentication(linkedInAuthenticationOptions);


            //}

        }

        static X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }
    }
}
