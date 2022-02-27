using CloudRealtime.services;
using CloudRealtime.StockItem.model;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace CloudRealtime.StockItem.service
{
    public partial class StockItemService : CloudService
    {
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
                logger.Error(response);
                logger.Error("Error to change itemName");
            }
            else
            {
                logger.Info("Success to change itemName");
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
                logger.Error(response);
                logger.Error("Error to create stock item");
            }
            else
            {
                logger.Info("Success to create stock item");
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
                logger.Error("Error to get alarm list");
                return null;
            }
            else
            {
                logger.Info("Success to get alarm list");
                var result = JObject.Parse(response.Content).SelectToken("data.content");
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
