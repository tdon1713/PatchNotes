using System;
using System.Security.Claims;

namespace PatchNotes.Token
{
    public interface IAuthContainerModel
    {
        #region Members
        string SecretKey { get; set; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }
        DateTime? NotValidBefore { get; set; }

        Claim[] Claims { get; set; }
        #endregion
    }
}