using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace AuthExample.Utilities
{
    // This class is needed to inject the JWT validation options including the public key
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private const string PUBLIC_KEY = @"MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA5f4mI8Lx+DuCH06gH0MgFgZkdVqnGqR3aSEHAO0EIHiKgmovx+htec37kut5mIy2bt8cEafAizGXyXFI/VdyjD9+TmMt19yCiCc2N5nmfAGAJjMsvBQ99jt1LXnCyPepfiiJRpxAuCW5MWZxuhKFNXKMWRWmYBNVIlpGkH7m9pom+FQ6BZUZ59k3RsEQjLvkE7x6RNVdi/c6/hj+F/yefgLU0Dstz13jP/pWRsSUtmNkZK0VQR8G27qcuretWBWXD2aYLGqKOu54VaidqsXvIjL6YT05NgxG1JCsjgN9zI+JbFVAZVqxyrp1bNE66aUULhENqfxNGQ7SjwRezHsIK5kbPiAJy8h8hSxz/YjB+C1r4DEYjb80cI3HQO83Rdbo+eS30DJI1AFLp420kx+0MikiUT/dPOUPLUDC03McTfxyU5Fy+MMRhL8MRwNB+XaHlS9lwraA1kdUMGVzzweQwg2Of3X0y+Nl23pRs3qlYf0Yt/d7go3TvB1veqVpB1qtAWauzgXK7vSBNtIGXrSFI/yDW1bTrYut/jWojIEfsCSz7Xcdz9U+6xIFOeZbfJL4Ds7zwuktsrbffwUIPKQt1yIjv55XXhIJQdQVI4jXh40Wj7ZFUiABuRtXJjhfPI2KveMzvmnK0cOs+DBYvw7rRlCQyANlmiJKXCjs+sZlvRcCAwEAAQ==";

        public ConfigureJwtBearerOptions()
        {
            Console.WriteLine("Hello");
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            RSA rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(PUBLIC_KEY), out _);

            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidateIssuer = true,
                ValidIssuer = "AuthService",
                ValidateAudience = true,
                ValidAudience = "myApi",
                CryptoProviderFactory = new CryptoProviderFactory()
                {
                    CacheSignatureProviders = false
                }
            };
        }
    }
}
