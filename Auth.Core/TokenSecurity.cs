using Auth.Core.Models;
using Auth.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auth.Core
{
    public interface ITokenSecurity
    {
        Task<bool> IsLatestRequest(long timestamp);
        Task<bool> ValidateToken(ILoginDetails loginDetails);

        Task<ILoginDetails> GetUserDetails(ILoginDetails loginDetails);
    }
    public class TokenSecurity : ITokenSecurity
    {
        IHttpGoogleClientService _googleServiceHelper;
        IHashGenerator _hashGenerator;
        ISettings _settings;
        public TokenSecurity(IHttpGoogleClientService serviceHelper, IHashGenerator hashGenerator, ISettings settings)
        {
            _googleServiceHelper = serviceHelper;
            _hashGenerator = hashGenerator;
            _settings = settings;
        }
        public async Task<ILoginDetails> GetUserDetails(ILoginDetails loginDetails)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateToken(ILoginDetails loginDetails)
        {
            try
            {
                var isSecurityTokenValid = await ValidateSecurityToken(loginDetails);
                var isSSOTokenValid = false;
                if (isSecurityTokenValid)
                {
                    isSSOTokenValid = await ValidateSSOToken(loginDetails);
                }

                return isSecurityTokenValid && isSSOTokenValid;
            }
            catch(Exception expection)
            {
                Console.WriteLine(expection.Message);
                return false;
            }
        }

        public async Task<bool> IsLatestRequest(long timestamp)
        {
            DateTime requestDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            requestDateTime = requestDateTime.AddSeconds(timestamp).ToUniversalTime();
            DateTime currentUtcTime = DateTime.UtcNow;

            return (currentUtcTime - requestDateTime).TotalMinutes >= 0 && (currentUtcTime - requestDateTime).TotalMinutes < 5;
        }
        private async Task<bool> ValidateSecurityToken(ILoginDetails loginDetails)
        {
            var userLoginDetails = (LoginDetails)loginDetails;
            var computedHash = _hashGenerator.CreateSha256Hash<TokenDetails>(userLoginDetails.TokenDetails, "salt");
     
            return computedHash == userLoginDetails.SecurityToken;
        }

        private async Task<bool> ValidateSSOToken(ILoginDetails loginDetails)
        {

            var googleUserTokenDetails = (LoginDetails)loginDetails;
            string methodUri = "tokeninfo";
            string queryParam = "id_token=" + googleUserTokenDetails.TokenDetails.IdToken;
            
            var response = await _googleServiceHelper.Get<object>(methodUri, queryParam);
            
            return response != null;

        }
    }
}
