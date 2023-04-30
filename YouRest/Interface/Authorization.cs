using YouRest.Authentication;

namespace YouRest.Interface
{
    public interface Authorization
    {
        AuthorizationHeaderDetail GetAuthorizationHeader();
    }
}
