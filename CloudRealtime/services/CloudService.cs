using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System.Configuration;

namespace CloudRealtime.services
{
    public partial class CloudService
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        protected static string BASE_URL = ConfigurationManager.AppSettings.Get("V2BaseUrl");
        protected string token;
        protected RestClient client;
        protected RestRequest request;
        protected IRestResponse response;

        public CloudService()
        {
            this.token = getToken();
        }

        private string getToken()
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/auth/login");
            client.Timeout = -1;
            request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{
            " + "\n" +
            @"    ""userName"": ""admin"",
            " + "\n" +
            @"    ""password"": ""admin1234admin""
            " + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                logger.Error("Error to get a login token");
                return null;
            }
            else
            {
                logger.Info("Success to get a login token");
                var jObject = JObject.Parse(response.Content);
                return jObject.SelectToken("data.token").ToString();
            }
        }
    }
}
