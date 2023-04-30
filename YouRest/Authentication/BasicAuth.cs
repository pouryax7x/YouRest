using System.Text;
using YouRest.Interface;

namespace YouRest.Authentication
{
    public class BasicAuth : Authorization
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public AuthorizationHeaderDetail GetAuthorizationHeader()
        {
            return new AuthorizationHeaderDetail
            {
                Scheme = "Basic",
                Parameter = Convert.ToBase64String(Encoding.Default.GetBytes($"{Username}:{Password}"))
            };
        }
    }
}
