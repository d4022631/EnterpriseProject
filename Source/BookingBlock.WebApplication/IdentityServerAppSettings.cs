using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.InMemory;

namespace BookingBlock.WebApplication
{
    public class IdentityServerClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            Client client = await Task.Run(() => FindClientById(clientId));

            return client;
        }

        private Client FindClientById(string clientId)
        {
            return IdentityServerClients.Get().FirstOrDefault(client => client.ClientId == clientId);
        }
    }

    public static class IdentityServerClients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "MVC Client",
                    ClientId = "mvc",
                    Flow = Flows.Implicit,
                    AllowedScopes = new List<string> { "openid","profile","roles" }
                    ,
                    RedirectUris = new List<string>
                    {
                        SiteAppSettings.SslUrl
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        SiteAppSettings.SslUrl
                    }
                },
            new Client
            {
                Enabled = true,
                ClientName = "MVC Client (service communication)",
                ClientId = "mvc_service",
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },

                Flow = Flows.ClientCredentials
            }, new Client()
            {
                //https://enterpriseproject-d4022631.c9users.io
                    Enabled = true,
                    ClientName = "Cloud9 Workspace",
                    ClientId = "c9workspace",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    {
                        "https://enterpriseproject-d4022631.c9users.io/"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://enterpriseproject-d4022631.c9users.io/"
                    }
            },  new Client()
            {
                //https://localhost:44300/
                    Enabled = true,
                    ClientName = "Cloud9 Workspace Local",
                    ClientId = "c9workspacelocal",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    {
                        "https://localhost:44300/"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:44300/"
                    }
            }

            };
        }
    }

    public static class IdentityServerScopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Enabled = true,
                    Name = "roles",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
            new Scope
            {
                Enabled = true,
                Name = "sampleApi",
                Description = "Access to a sample API",
                Type = ScopeType.Resource
            }
            };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }

    public static class IdentityServerUsers
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "bob",
                    Password = "secret",
                    Subject = "1",

                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Bob"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                               new Claim(Constants.ClaimTypes.Subject, "BobSmith"),
                                    new Claim(Constants.ClaimTypes.Role, "Geek"),
                    new Claim(Constants.ClaimTypes.Role, "Foo")
                    }
                }
            };
        }
    }

    public static class IdentityServerAppSettings
    {
        public static string PathMatch
        {
            get { return AppSettings.GetValueOrDefault("IdentityServer.PathMath", "/identity"); }
        }

        public static string SslUrl
        {
            get
            {
                Uri result;

                if (Uri.TryCreate(new Uri(SiteAppSettings.SslUrl), PathMatch, out result))
                {
                    return result.ToString();
                }

                return PathMatch;
            }
        }

        public static string Url
        {
            get
            {
                Uri result;

                if (Uri.TryCreate(new Uri(SiteAppSettings.Url), PathMatch, out result))
                {
                    return result.ToString();
                }

                return PathMatch;
            }
        }
    }
}