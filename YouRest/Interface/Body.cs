using System.Net.Http.Headers;

namespace YouRest.Interface
{
    public interface IBody
    {
        MediaTypeHeaderValue GetContentType();
        HttpContent GetContent();
    }
}
