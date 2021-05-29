using CloudRealtime.RealTime.model;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.service
{
    public partial class AlarmService
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private const string BASE_URL = "http://localhost:8080";
        private string token;
        private RestClient client;
        private RestRequest request;
        private IRestResponse response;

        public AlarmService()
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
            @"    ""userName"": ""cloud"",
            " + "\n" +
            @"    ""password"": ""1234""
            " + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            response = client.Execute(request);

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to get a login token");
                return null;
            } else
            {
                Logger.Info("Success to get a login token");
                var jObject = JObject.Parse(response.Content);
                return jObject.GetValue("data").ToString();
            }

            
        }

        public List<Alarm> getAlarmList()
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/alarm/stockItem");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to get alarm list");
                return null;
            } else
            {
                Logger.Info("Success to get alarm list");
                var result = JObject.Parse(response.Content).GetValue("data");
                JArray jArray = JArray.Parse(result.ToString());

                List<Alarm> alarmList = new List<Alarm>();
                foreach (JObject item in jArray)
                {
                    alarmList.Add(new Alarm()
                    {
                        alarmId = long.Parse(item.GetValue("alarmId").ToString()),
                        itemName = item.GetValue("itemName").ToString(),
                        itemCode = item.GetValue("itemCode").ToString(),
                        recommendPrice = int.Parse(item.GetValue("recommendPrice").ToString()),
                        losscutPrice = int.Parse(item.GetValue("losscutPrice").ToString())
                    });
                }

                return alarmList;
            }
        }
    }
}
