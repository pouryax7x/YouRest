using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace YouRest
{
    public class RestRequest_VM
    {
        private List<KeyValuePair<string, string>> _headers { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }

        private string _subAddress { get; set; }
        public Authorization Authorization { get; set; }

        public string SubAddress
        {
            set => _subAddress = string.IsNullOrWhiteSpace(value) ? "" : value;
            get => _subAddress;
        }
        public HttpMethods HttpMethod { get; set; }
        public Body Body { get; set; }
        public List<KeyValuePair<string, string>> Params { get; set; }
        public IgnoreSSL IgnoreSSL { get; set; }
    }
    public enum IgnoreSSL
    {
        NotIgnored = 0,
        Ignored = 1,
    }
    public class JsonBody : Body
    {
        public object Content { get; set; }
        public JsonBody(object inputClass)
        {
            Content = inputClass;
        }
        public MediaTypeHeaderValue GetContentType()
        {
            return new MediaTypeHeaderValue("application/json");

        }
        public HttpContent GetContent()
        {
            string? jsonContent = JsonConvert.SerializeObject(Content,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            return new StringContent(jsonContent);
        }
    }

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
    //public class MultiPartFormDataBody : Body
    //{
    //    public List<KeyValuePair<string, string>> Content { get; set; }
    //    public MultiPartFormDataBody(List<KeyValuePair<string, string>> content)
    //    {
    //        Content = content;
    //    }
    //    public MediaTypeHeaderValue GetContentType()
    //    {
    //        return new MediaTypeHeaderValue("application/x-www-form-urlencoded");

    //    }
    //    public HttpContent GetContent()
    //    {
    //        return new MultipartFormDataContent(Content);
    //    }
    //}
    public interface Body
    {
        MediaTypeHeaderValue GetContentType();
        HttpContent GetContent();
    }
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
    public interface Authorization
    {
        AuthorizationHeaderDetail GetAuthorizationHeader();
    }

    public class AuthorizationHeaderDetail
    {
        public string Scheme { get; set; }
        public string Parameter { get; set; }
    }
    public enum HttpMethods
    {
        Get = 0,
        Post = 1,
        Put = 2,
        Delete = 3,
        Patch = 4
    }
}
