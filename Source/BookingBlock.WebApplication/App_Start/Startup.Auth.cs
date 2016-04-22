using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using BookingBlock.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using BookingBlock.WebApplication.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;

namespace BookingBlock.WebApplication
{
    public partial class Startup
    {
        private static bool IsApiRequest(IOwinRequest request)
        {
            string apiPath = VirtualPathUtility.ToAbsolute("~/api/");
            return request.Uri.LocalPath.StartsWith(apiPath);
        }
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",

                Provider = new CookieAuthenticationProvider()
                {
                    OnApplyRedirect = ctx =>
                    {
                        if (!IsApiRequest(ctx.Request))
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                    }
                }
            });

            string redirectUri = SiteAppSettings.SslUrl;

            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            //{
            //    Authority = "https://localhost:44300/identity",
            //    RequiredScopes = new[] { "sampleApi" }
            //});
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = IdentityServerAppSettings.SslUrl,
                ClientId = "mvc",
                RedirectUri = redirectUri,
                PostLogoutRedirectUri = SiteAppSettings.SslUrl,
                ResponseType = "id_token",
                Scope = "openid profile roles",
                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = SecurityTokenValidated,
                    RedirectToIdentityProvider = async n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token").Value;
                            n.ProtocolMessage.IdTokenHint = idTokenHint;
                        }

                        
                    }
                }
            });

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(CreateCallback);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            return;
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(CreateCallback);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

  
        private async Task SecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> securityTokenValidatedNotification)
        {
            var n = securityTokenValidatedNotification;

            var id = n.AuthenticationTicket.Identity;

            // we want to keep first name, last name, subject and roles
            var givenName = id.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.GivenName);
            var familyName = id.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.FamilyName);
            var sub = id.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.Subject);
            var roles = id.FindAll(IdentityServer3.Core.Constants.ClaimTypes.Role);
            var id2 = id.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var name = id.FindFirst("preferred_username");

            // create new identity and set name and role claim type
            var nid = new ClaimsIdentity(
                id.AuthenticationType,
               IdentityServer3.Core.Constants.ClaimTypes.GivenName,
               IdentityServer3.Core.Constants.ClaimTypes.Role);


            if (givenName != null) nid.AddClaim(givenName);
            if (familyName != null) nid.AddClaim(familyName);
            if (sub != null) nid.AddClaim(sub);
            if (roles != null) nid.AddClaims(roles);

            nid.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", name?.Value ?? "UNKNOWN"));





            nid.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                id2.Value));
            nid.AddClaim(
            new Claim(
                "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "idsrv"));
            nid.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "BUBGO"));


            // add some other app specific claim
            nid.AddClaim(new Claim("app_specific", "some data"));
            nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
            n.AuthenticationTicket = new AuthenticationTicket(
                nid,
                n.AuthenticationTicket.Properties);
        }
    }
}