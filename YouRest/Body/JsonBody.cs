using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace YouRest.BodyClasses
{
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
}
