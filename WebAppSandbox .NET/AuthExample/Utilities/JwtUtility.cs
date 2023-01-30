using AuthExample.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace AuthService.Utilities
{
    public class JwtUtility : IJwtUtility
    {
        private readonly RsaSecurityKey _key;
        public JwtUtility(IConfiguration configuration)
        {
            RSA privateRSA = RSA.Create();
            privateRSA.ImportRSAPrivateKey(Convert.FromBase64String(configuration.GetValue<string>("JwtPrivateSigningKey")), out _);
            _key = new RsaSecurityKey(privateRSA);
        }

        public string CreateUserAuthToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "myApi",
                Issuer = "AuthExample",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60), // TODO: Is this too short... or too long? 
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateJwtSources(string userId, string authorization)
        {
            bool result = false;

            string accessToken = string.Empty;

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                // We have a valid AuthenticationHeaderValue that has the following details:
                // scheme will be "Bearer"
                // parmameter will be the token itself.
                var scheme = headerValue.Scheme;
                accessToken = headerValue.Parameter;
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                var securityToken = DecodeToken(accessToken);

                if (securityToken != null)
                {
                    var idThing = securityToken.Payload.Claims.FirstOrDefault();

                    if (idThing != null && idThing.Value == userId)
                    {
                        result = true;
                    }
                }
            }

            return result;
            
        }

        private JwtSecurityToken DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.ReadJwtToken(token);
        }

        // TODO: We probably also need to validate the token itself...
        private bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "AuthExample",
                ValidAudience = "myApi",
                IssuerSigningKey = _key // The same key as the one that generate the token
            };
        }
    }
}