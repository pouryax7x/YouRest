using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YouRest.Interface;

namespace YouRest.Body;

public class FromDataBody : IBody
{
    public MultipartFormDataContent Content = new MultipartFormDataContent();
    public FromDataBody(MultipartFormDataContent content)
    {
        Content = content;
    }
    public FromDataBody(List<KeyValuePair<string, string>> contentField)
    {
        foreach (var item in contentField)
        {
            Content.Add(new StringContent(item.Value), item.Key);
        }
    }
    public FromDataBody(List<Tuple<string, string , byte[]>> contentFile)
    {
        foreach (var item in contentFile)
        {
            Content.Add(new ByteArrayContent(item.Item3), item.Item1, item.Item2);
        }
    }
    public void AddField(string fieldName, string value)
    {
        Content.Add(new StringContent(value), fieldName);
    }

    public void AddFile(string fieldName, byte[] fileBytes, string fileName)
    {
        Content.Add(new ByteArrayContent(fileBytes), fieldName, fileName);
    }
    public HttpContent GetContent()
    {
        return Content;
    }

    public MediaTypeHeaderValue GetContentType()
    {
        return Content.Headers.ContentType;
    }
}
