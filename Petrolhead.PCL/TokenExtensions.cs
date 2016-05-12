using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public class TokenExpirationException : Data.LoginFailureException
    {
        public TokenExpirationException(string message, Exception innerException) : base(message, Petrolhead.Data.LoginStatus.UnknownError, innerException)
        {

        }
    }

    public static class TokenExtensions
    {
        public static bool IsTokenExpired(this IMobileServiceClient client)
        {


            try
            {
                // Get just the JWT part of the token.
                var jwt = client.CurrentUser
                    .MobileServiceAuthenticationToken
                    .Split(new Char[] { '.' })[1];

                // Undo the URL encoding.
                jwt = jwt.Replace('-', '+');
                jwt = jwt.Replace('_', '/');
                switch (jwt.Length % 4)
                {
                    case 0: break;
                    case 2: jwt += "=="; break;
                    case 3: jwt += "="; break;
                    default:
                        throw new System.Exception(
                   "The base64url string is not valid.");
                }

                // Decode the bytes from base64 and write to a JSON string.
                var bytes = Convert.FromBase64String(jwt);
                string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                // Parse as JSON object and get the exp field value, 
                // which is the expiration date as a JavaScript primative date.
                JObject jsonObj = JObject.Parse(jsonString);
                var exp = Convert.ToDouble(jsonObj["exp"].ToString());

                // Calculate the expiration by adding the exp value (in seconds) to the 
                // base date of 1/1/1970.
                DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var expire = minTime.AddSeconds(exp);

                return expire < DateTime.UtcNow ? true : false;
            }
            catch (Exception ex)
            {
                throw new TokenExpirationException(ex.Message, ex);
            }
        }
    }
}
