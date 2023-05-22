using System.Net.Http.Headers;

namespace YouRest.Interface.Body;

public class WWWFormBody : IBody
{
    public List<KeyValuePair<string, string>> Content { get; set; }
    public WWWFormBody(List<KeyValuePair<string, string>> content)
    {
        Content = content;
    }
    public MediaTypeHeaderValue GetContentType()
    {
        // return new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        return GetContent().Headers.ContentType;

    }
    public HttpContent GetContent()
    {
        return new FormUrlEncodedContent(Content);
    }
}
