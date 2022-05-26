using System.Text;

namespace YouRest
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
