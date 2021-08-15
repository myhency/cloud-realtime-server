using CloudRealtime.SevenBread.model;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudRealtime.SevenBread.service
{
    public partial class SevenBreadService
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private const string BASE_URL = "http://192.168.29.189:8080";
        private string token;
        private RestClient client;
        private RestRequest request;
        private IRestResponse response;

        public SevenBreadService()
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

        public List<SevenBreadItem> getSevenBreadItemList()
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/sevenbread/item");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);

            response = client.Execute<List<SevenBreadItem>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to get sevenbread item list");
                return null;
            } else
            {
                Logger.Info("Success to get sevenbread item list");
                var result = JObject.Parse(response.Content).GetValue("data");
                JArray jArray = JArray.Parse(result.ToString());

                List<SevenBreadItem> sevenBreadItemList = new List<SevenBreadItem>();

               
                foreach (JObject item in jArray)
                {
                    try
                    {
                        sevenBreadItemList.Add(new SevenBreadItem()
                        {
                            id = long.Parse(item.GetValue("id").ToString()),
                            itemCode = item.GetValue("itemCode").ToString(),
                            itemName = item.GetValue("itemName").ToString(),
                            capturedPrice = item.GetValue("capturedPrice") == null ? 9999999 : int.Parse(item.GetValue("capturedPrice").ToString()),
                            closingPrice = item.GetValue("closingPrice") == null ? 99999999 : int.Parse(item.GetValue("closingPrice").ToString()),
                            capturedDate = item.GetValue("capturedDate").ToString(),
                            majorHandler = item.GetValue("majorHandler").ToString(),
                            theme = item.GetValue("theme").ToString(),
                        });
                    } catch (Exception e)
                    {
                        Logger.Error($"[aaaaaaget sevenBread list]{e.Message}");
                    }
                        
                }
                
                return sevenBreadItemList;
            }
        }

        public string updateSevenBreadItemToday(Opt10001VO opt10001VO)
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/sevenbread/item/today/" + opt10001VO.종목코드);
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                closingPrice = opt10001VO.현재가,
                fluctuationRate = opt10001VO.등락율,
                priceByYesterday = opt10001VO.전일대비,
                volume = opt10001VO.거래량,
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to update today sevenbread item");
                return null;
            }
            else
            {
                Logger.Info("Success to update today sevenbread item");
                var jObject = JObject.Parse(response.Content);
                return jObject.GetValue("data").ToString();
            }
        }

        public string updateSevenBreadItemCapturedDay(Opt10001VO opt10001VO)
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/sevenbread/item/" + opt10001VO.종목코드);
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                closingPrice = opt10001VO.현재가,
                capturedPrice = opt10001VO.현재가,
                fluctuationRate = opt10001VO.등락율,
                priceByYesterday = opt10001VO.전일대비,
                volume = opt10001VO.거래량,
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to update captured day sevenbread item");
                return null;
            }
            else
            {
                Logger.Info("Success to update captured day sevenbread item");
                var jObject = JObject.Parse(response.Content);
                return jObject.GetValue("data").ToString();
            }
        }
    }
}
