using CloudRealtime.StockItem.model;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.StockItem.service
{
    public partial class StockItemService
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private const string BASE_URL = "http://192.168.29.242:8080";
        private string token;
        private RestClient client;
        private RestRequest request;
        private IRestResponse response;

        public StockItemService()
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

        public void updateItemName(Item item)
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/item/stockItem/changeName");
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                itemCode = item.itemCode,
                itemName = item.itemName,
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error(response);
                Logger.Error("Error to change itemName");
            }
            else
            {
                Logger.Info("Success to change itemName");
            }
        }

        public void createItem(Item item)
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/item/stockItem");
            client.Timeout = -1;
            request = new RestRequest(Method.POST);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                itemCode = item.itemCode,
                itemName = item.itemName,
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Logger.Error(response);
                Logger.Error("Error to create stock item");
            }
            else
            {
                Logger.Info("Success to create stock item");
            }
        }

        public List<Item> getRegisterItems()
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/item/stockItem");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", "Bearer " + this.token, ParameterType.HttpHeader);

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to get alarm list");
                return null;
            }
            else
            {
                Logger.Info("Success to get alarm list");
                var result = JObject.Parse(response.Content).GetValue("data");
                JArray jArray = JArray.Parse(result.ToString());

                List<Item> itemList = new List<Item>();
                foreach (JObject item in jArray)
                {
                    itemList.Add(new Item()
                    {
                        itemName = item.GetValue("itemName").ToString(),
                        itemCode = item.GetValue("itemCode").ToString()
                    });
                }

                return itemList;
            }
        }
    }
}
