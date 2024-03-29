﻿using CloudRealtime.RealTime.model;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.service
{
    public partial class AlarmService
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private const string BASE_URL = "http://myhency.duckdns.org:18080";
        private static string V2BASE_URL = ConfigurationManager.AppSettings.Get("V2BaseUrl");
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
            client = new RestClient(V2BASE_URL + "/api/v1/platform/auth/login");
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
                return jObject.SelectToken("data.token").ToString();
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
                        losscutPrice = int.Parse(item.GetValue("losscutPrice").ToString()),
                        theme = item.GetValue("theme").ToString(),
                        comment = item.GetValue("comment").ToString(),
                        alarmStatus = item.GetValue("alarmStatus").ToString()
                    });
                }

                return alarmList;
            }
        }

        public void buyAlarm(long alarmId)
        {
            client = new RestClient(BASE_URL + $"/api/v1/platform/alarm/stockItem/buy/{alarmId}");
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to change alarm status");
                return;
            }
            else
            {
                Logger.Info("Success to change alarm status");
                return;
            }
        }

        public void losscutAlarm(long alarmId)
        {
            client = new RestClient(BASE_URL + $"/api/v1/platform/alarm/stockItem/losscut/{alarmId}");
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to change alarm status");
                return;
            }
            else
            {
                Logger.Info("Success to change alarm status");
                return;
            }
        }

        public void updateClosingPrice(long alarmId, string itemCode, int closingPrice)
        {
            client = new RestClient(BASE_URL + $"/api/v1/platform/alarm/stockItem/closingPrice/{alarmId}");
            client.Timeout = -1;
            request = new RestRequest(Method.POST);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                itemCode = itemCode,
                closingPrice = closingPrice,
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to change alarm closing price");
                return;
            }
            else
            {
                Logger.Info("Success to change alarm closing price");
                return;
            }
        }
    }
}
