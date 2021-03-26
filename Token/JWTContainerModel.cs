using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;

namespace PatchNotes.Token
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public string SecretKey { get; set; } = "54eFjXJYC6ZZ8kAY";

        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

        public int ExpireMinutes { get; set; } = 10080; // 7days.

        public DateTime? NotValidBefore { get; set; }

        public Claim[] Claims { get; set; }
    }
}
