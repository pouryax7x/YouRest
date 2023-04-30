using YouRest.Enum;
using YouRest.Interface;

namespace YouRest
{
    public class RestRequest_VM
    {
        private List<KeyValuePair<string, string>> _headers { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }

        private string _reletiveAddress { get; set; }
        public Authorization Authorization { get; set; }

        public string ReletiveAddress
        {
            set => _reletiveAddress = string.IsNullOrWhiteSpace(value) ? "" : value;
            get => _reletiveAddress;
        }
        public HttpMethod HttpMethod { get; set; }
        public Body Body { get; set; }
        public List<KeyValuePair<string, string>> Params { get; set; }
        public IgnoreSSL IgnoreSSL { get; set; }
        public bool EnsureSuccessStatusCode { get; set; }
    }
}
