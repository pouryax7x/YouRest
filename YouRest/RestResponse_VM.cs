using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouRest
{
    public class RestResponse_VM<T> where T : class
    {
        private string _apiResponse { get; set; }
        public Exception Exception { get; set; }
        public bool IsSuccess { get; set; }
        public string ApiResponse
        {
            set => _apiResponse = value;
        }
        public HttpStatusCode StatusCode { get; set; }
        public HttpHeaders Headers { get; set; }
        public T? GetResponse() => string.IsNullOrWhiteSpace(_apiResponse) ? default : JsonConvert.DeserializeObject<T>(_apiResponse);

    }
}
