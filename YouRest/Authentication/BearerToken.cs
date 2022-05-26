namespace YouRest
{
    public class BearerToken : Authorization
    {
        public string Token { get; set; }

        public AuthorizationHeaderDetail GetAuthorizationHeader()
        {
            return new AuthorizationHeaderDetail
            {
                Parameter = Token,
                Scheme = "Bearer"
            };
        }
    }
}
