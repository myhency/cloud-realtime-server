using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealCondition.service
{
    public partial class StockItemService
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private const string BASE_URL = "http://192.168.29.189:8080";
        private string token;
        private RestClient client;
        private RestRequest request;
        private IRestResponse response;

        public StockItemService()
        {
            this.token = getToken();
            Console.WriteLine(this.token);
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
                Logger.Error("Error to get a login token");
                return null;
            }
            else
            {
                Logger.Info("Success to get a login token");
                var jObject = JObject.Parse(response.Content);
                return jObject.GetValue("data").ToString();
            }
        }

        public string getTheme(string itemCode)
        {
            client = new RestClient(BASE_URL + $"/api/v1/platform/item/stockItem/theme/{itemCode}");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to get theme");
                return null;
            }
            else
            {
                Logger.Info("Success to get theme");
                var result = JObject.Parse(response.Content).GetValue("theme");

                return result.ToString();
            }
        }

        public void createVolume(Opt10001VO opt10001VO)
        {
            client = new RestClient(BASE_URL + $"/api/v1/platform/analyze/volumeByShares");
            client.Timeout = -1;
            request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                itemName = opt10001VO.종목명,
                itemCode = opt10001VO.종목코드,
                closingPrice = opt10001VO.현재가,
                fluctuationRate = opt10001VO.등락율,
                volume = opt10001VO.거래량,
                numberOfOutstandingShares = opt10001VO.유통주식,
                marketCap = opt10001VO.시가총액,
                marketType = opt10001VO.거래소구분,
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Logger.Error("Error to create volume");
                return;
            }
            else
            {
                Logger.Info("Success to create volume");
                return;
            }
        }
    }


}
