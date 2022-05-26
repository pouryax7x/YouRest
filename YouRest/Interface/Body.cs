using System.Net.Http.Headers;

namespace YouRest
{
    public interface Body
    {
        MediaTypeHeaderValue GetContentType();
        HttpContent GetContent();
    }
}
