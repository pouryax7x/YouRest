using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace YouRest.Interface.Body;

public class JsonBody : IBody
{
    public object Content { get; set; }
    public string JsonContent { get; set; }
    public JsonBody(object inputClass)
    {
        Content = inputClass;
    }
    public JsonBody(string jsonContent)
    {
        JsonContent = jsonContent;
    }
    public MediaTypeHeaderValue GetContentType()
    {
         return new MediaTypeHeaderValue("application/json");
        //return GetContent().Headers.ContentType;

    }
    public HttpContent GetContent()
    {
        if (!string.IsNullOrEmpty(JsonContent))
        {
            return new StringContent(JsonContent);
        }
        string? jsonContent = JsonConvert.SerializeObject(Content,
                        Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
        return new StringContent(jsonContent);
    }
}
