using CloudRealtime.RealTime.model;
using Newtonsoft.Json.Linq;
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

            return JObject.Parse(client.Execute(request).Content)
                .GetValue("data").ToString();
        }

        public List<Alarm> getAlarmList()
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/alarm/stockItem");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);
            var result = JObject.Parse(client.Execute(request).Content).GetValue("data");
            
            Console.WriteLine(result);

            JArray jArray = JArray.Parse(result.ToString());

            List<Alarm> alarmList = new List<Alarm>();
            foreach (JObject item in jArray)
            {
                alarmList.Add(new Alarm() { 
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
