using System.Net;
using System.Text.RegularExpressions;
using YouRest.Authentication;
using YouRest.Enum;

namespace YouRest
{
    public class RestCaller
    {
        private readonly HttpClient Client = new HttpClient();
        private readonly Dictionary<HttpMethod, Func<RestRequest_VM, HttpResponseMessage>> MethodList = new Dictionary<HttpMethod, Func<RestRequest_VM, HttpResponseMessage>>();
        public RestCaller(RestStaticProperties properties)
        {
            //Fill MethodList
            MethodList.Add(HttpMethod.Post, Post);
            MethodList.Add(HttpMethod.Get, Get);
            MethodList.Add(HttpMethod.Put, Put);
            MethodList.Add(HttpMethod.Patch, Patch);
            MethodList.Add(HttpMethod.Delete, Delete);

            //Set Connection Time Out
            Client.Timeout = properties.Timeout;

            //Set Base Address
            Client.BaseAddress = new Uri(properties.BaseAddress);

            //Add Accept Header
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

        public RestCaller(RestStaticProperties properties, HttpClient httpClient)
        {
            Client = httpClient;
            //Fill MethodList
            MethodList.Add(HttpMethod.Post, Post);
            MethodList.Add(HttpMethod.Get, Get);
            MethodList.Add(HttpMethod.Put, Put);
            MethodList.Add(HttpMethod.Patch, Patch);
            MethodList.Add(HttpMethod.Delete, Delete);

            //Set Connection Time Out
            Client.Timeout = properties.Timeout;

            //Set Base Address
            Client.BaseAddress = new Uri(properties.BaseAddress);

            //Add Accept Header
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }
        public RestResponse_VM<T> CallRestService<T>(RestRequest_VM request) where T : class
        {
            RestResponse_VM<T> response = new RestResponse_VM<T>();

            //Clear headers
            Client.DefaultRequestHeaders.Clear();

            //Set Headers
            if (request.Headers != null)
            {
                request.Headers.ForEach(x =>
                {
                    Client.DefaultRequestHeaders.Add(x.Key, x.Value);
                });
            }

            //Add Auth Header
            if (request.Authorization != null)
            {
                AuthorizationHeaderDetail? authInfo = request.Authorization.GetAuthorizationHeader();
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authInfo.Scheme, authInfo.Parameter);
            }



            //IgnoreSSL
            if (request.IgnoreSSL == IgnoreSSL.Ignored)
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                                       (se, cert, chain, sslerror) =>
                                       {
                                           return true;
                                       };
            }

            //Set Params
            request.ReletiveAddress = AddParametersToAddress(request.Params, request.ReletiveAddress);

            //Get Response
            HttpResponseMessage? responseMessage = MethodList[request.HttpMethod](request);
            if (request.EnsureSuccessStatusCode)
                responseMessage.EnsureSuccessStatusCode();

            var res = responseMessage.Content.ReadAsStringAsync().Result;

            res = Unescape(res);

            response.ApiResponse = res;
            response.StatusCode = responseMessage.StatusCode;
            response.IsSuccess = responseMessage.IsSuccessStatusCode;
            response.Headers = responseMessage.Headers;

            return response;
        }

        private static string Unescape(string res)
        {
            res = Regex.Replace(res, @"\\([\\/bfnrt]|u[0-9a-fA-F]{4})", m =>
            {
                string match = m.Groups[1].Value;
                switch (match)
                {
                    case "\\":
                        return "\\";
                    case "/":
                        return "/";
                    case "b":
                        return "\b";
                    case "f":
                        return "\f";
                    case "n":
                        return "\n";
                    case "r":
                        return "\r";
                    case "t":
                        return "\t";
                    default:
                        int codepoint = int.Parse(match.Substring(1), System.Globalization.NumberStyles.HexNumber);
                        return char.ConvertFromUtf32(codepoint);
                }
            });
            return res;
        }

        private string AddParametersToAddress(List<KeyValuePair<string, string>> @params, string address)
        {
            if (@params != null)
            {
                if (@params.Count != 0)
                {
                    address += "?";
                    @params.ForEach(x =>
                    {
                        address += $@"{x.Key}={x.Value}&";
                    });
                    address = RemoveLastItem(address);
                }
            }
            return address;
        }

        private string RemoveLastItem(string address)
        {
            return address.Remove(address.Length - 1);
        }
        private HttpResponseMessage Get(RestRequest_VM request)
        {
            return Client.GetAsync(request.ReletiveAddress).Result;
        }
        private HttpResponseMessage Post(RestRequest_VM request)
        {
            HttpContent? content = request.Body.GetContent();
            content.Headers.ContentType = request.Body.GetContentType();
            return Client.PostAsync(request.ReletiveAddress, content).Result;
        }
        private HttpResponseMessage Put(RestRequest_VM request)
        {
            HttpContent? content = request.Body.GetContent();
            content.Headers.ContentType = request.Body.GetContentType();
            return Client.PutAsync(request.ReletiveAddress, content).Result;
        }
        private HttpResponseMessage Patch(RestRequest_VM request)
        {
            HttpContent? content = request.Body.GetContent();
            content.Headers.ContentType = request.Body.GetContentType();
            return Client.PatchAsync(request.ReletiveAddress, content).Result;
        }
        private HttpResponseMessage Delete(RestRequest_VM request)
        {
            return Client.DeleteAsync(request.ReletiveAddress).Result;
        }
    }
}
