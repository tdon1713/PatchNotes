using PatchNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PatchNotes.Token
{
    public class TokenManager
    {
        public static bool IsTokenValid(string token)
        {
            bool IsValid = false;

            JWTContainerModel model = new JWTContainerModel();
            var authService = new JWTService(model.SecretKey);

            if (authService.IsTokenValid(token))
            {
                IsValid = true;
            }

            return IsValid;
        }

        public static TDSUser GetTDSUser(string token)
        {
            TDSUser info = new TDSUser();

            JWTContainerModel model = new JWTContainerModel();
            var authService = new JWTService(model.SecretKey);

            try
            {
                List<Claim> claims = authService.GetTokenClaims(token).ToList();
                info.Username = claims.FirstOrDefault(c => c.Type.Equals("user")).Value;

                return info;
            }
            catch
            {
                return info;
            }
        }

        public static string GenerateTDSUser(string username, int expireMinutes = 1440)
        {
            JWTContainerModel model = new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim("user", username)
                }
            };

            return GenerateToken(model, expireMinutes);
        }

        private static string GenerateToken(JWTContainerModel model, int expireMinutes = 1440, DateTime? notValidBefore = null)
        {
            model.ExpireMinutes = expireMinutes;

            if (notValidBefore != null)
            {
                model.NotValidBefore = notValidBefore;
            }

            var authService = new JWTService(model.SecretKey);
            return authService.GenerateToken(model);
        }
    }
}
