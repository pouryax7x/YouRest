using System.Net;

namespace YouRest
{
    public class RestCaller
    {
        private readonly HttpClient Client = new HttpClient();
        private readonly Dictionary<HttpMethods, Func<RestRequest_VM, HttpResponseMessage>> MethodList = new Dictionary<HttpMethods, Func<RestRequest_VM, HttpResponseMessage>>();
        public RestCaller(RestStaticProperties properties)
        {
            //Fill MethodList
            MethodList.Add(HttpMethods.Post, Post);
            MethodList.Add(HttpMethods.Get, Get);

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
            request.SubAddress = AddParametersToAddress(request.Params, request.SubAddress);

            //Get Response
            HttpResponseMessage? responseMessage = MethodList[request.HttpMethod](request);
            responseMessage.EnsureSuccessStatusCode();

            response.ApiResponse = responseMessage.Content.ReadAsStringAsync().Result;
            response.StatusCode = responseMessage.StatusCode;
            response.IsSuccess = responseMessage.IsSuccessStatusCode;
            response.Headers = responseMessage.Headers;

            return response;
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
            return Client.GetAsync(request.SubAddress).Result;
        }
        private HttpResponseMessage Post(RestRequest_VM request)
        {
            //chose based on Body Class
            HttpContent? content = request.Body.GetContent();
            content.Headers.ContentType = request.Body.GetContentType();
            return Client.PostAsync(request.SubAddress, content).Result;
        }
    }
}
