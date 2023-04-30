using System.Net.Http.Headers;

namespace YouRest.Interface
{
    public interface Body
    {
        MediaTypeHeaderValue GetContentType();
        HttpContent GetContent();
    }
}
