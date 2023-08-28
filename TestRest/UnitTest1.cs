using System.Text.Json.Nodes;
using YouRest.Interface.Body;

namespace TestRest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var Authorization = GetValueFromInput<string>(input, "Authorization");
            var RequestTraceId = GetValueFromInput<string>(input, "RequestTraceId");
            var Timestamp = GetValueFromInput<string>(input, "Timestamp");
            var Body = GetValueFromInput<object>(input, "Body");
            RestCaller restCaller = new RestCaller(new RestStaticProperties
            {
                BaseAddress = "https://tp.tax.gov.ir/req/",
                Timeout = TimeSpan.FromSeconds(sendTimeoutInSeconds)
            });
            var caller = restCaller.CallRestService<object>(new RestRequest_VM
            {
                ReletiveAddress = "api/self-tsp/sync/GET_SERVICE_STUFF_LIST",
                HttpMethod = HttpMethod.Post,
                Authorization = new BearerToken() { Token = Authorization },
                Headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("RequestTraceId" , RequestTraceId),
                    new KeyValuePair<string, string>("Timestamp" , Timestamp),
                },
                Body = new JsonBody(Body),
                EnsureSuccessStatusCode = true,
            });
            return caller.GetResponse();
        }
    }
}