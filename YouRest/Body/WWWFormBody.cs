using System.Net.Http.Headers;

namespace YouRest
{
    public class WWWFormBody : Body
    {
        public List<KeyValuePair<string, string>> Content { get; set; }
        public WWWFormBody(List<KeyValuePair<string, string>> content)
        {
            Content = content;
        }
        public MediaTypeHeaderValue GetContentType()
        {
            return new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        }
        public HttpContent GetContent()
        {
            return new FormUrlEncodedContent(Content);
        }
    }
}
