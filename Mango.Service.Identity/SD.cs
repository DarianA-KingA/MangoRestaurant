using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Service.Identity
{
    public static class SD // Static Details (SD)
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        //here we stablish the information that can be request for authentication
        //spanish:IdentityResources (recursos de identidad) se refieren a la información
        //sobre la identidad del usuario que se puede solicitar y recibir en el proceso de autenticación.
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        //this stablish rules and limit about data that client can request
        //spanish:un ApiScope (alcance de API) se refiere a un conjunto de permisos o derechos que un cliente de API
        //puede solicitar para acceder a recursos protegidos. Puedes pensar en un ApiScope como un contrato entre el cliente de API y
        //el servidor de autenticación. Define qué acciones específicas puede realizar el cliente de API en nombre del usuario autenticado.

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("mango","Mango Server"),
                new ApiScope(name:"read",displayName:"Read your data."),
                new ApiScope(name:"write",displayName:"write your data."),
                new ApiScope(name:"delete",displayName:"delete your data.")
            };
        //our client are how request user data from our identity server
        //spanish:Un cliente de API es cualquier aplicación o sistema que consume o utiliza los servicios de una API (Application Programming Interface). 

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId ="client",
                    ClientSecrets ={ new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={"read","write","profile"}
                },
                new Client
                {
                    ClientId ="mango",
                    ClientSecrets ={ new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris={ "https://localhost:44336/signin-oidc", "https://localhost:7020/signin-oidc"},
                    PostLogoutRedirectUris ={ "https://localhost:44336/signout-callback-oidc"},
                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "mango"
                    }
                },

            };
    }
}
