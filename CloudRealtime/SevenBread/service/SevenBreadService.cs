using CloudRealtime.model;
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
using System.Configuration;
using System.Collections.Specialized;

namespace CloudRealtime.SevenBread.service
{
    public partial class SevenBreadService
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private static string BASE_URL = ConfigurationManager.AppSettings.Get("BaseUrl");
        private static string V2BASE_URL = ConfigurationManager.AppSettings.Get("V2BaseUrl");
        private string token;
        private string v2token;
        private RestClient client;
        private RestRequest request;
        private IRestResponse response;

        public SevenBreadService()
        {
            this.v2token = getV2Token();
        }

        private string getV2Token()
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

        public List<SevenBreadItem> getSevenBreadV2ItemList()
        {
            client = new RestClient(V2BASE_URL + "/api/v1/platform/v2/sevenbread/item");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);

            response = client.Execute<List<SevenBreadItem>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to get sevenbread item list");
                return null;
            }
            else
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
                            itemCode = item.GetValue("itemCode").ToString(),
                            itemName = item.GetValue("itemName").ToString(),
                            capturedPrice = item.GetValue("capturedPrice").Type == JTokenType.Null ? 0 : int.Parse(item.GetValue("capturedPrice").ToString()),
                            capturedDate = item.GetValue("capturedDate").ToString(),
                            majorHandler = item.GetValue("majorHandler").ToString(),
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.Error($"[get sevenBread list] {e.Message}");
                        Logger.Info($"[get sevenBread list] possibly item added");
                    }

                }

                return sevenBreadItemList;
            }
        }

        public List<SevenBreadItem> getSevenBreadItemList()
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/sevenbread/item");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);

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
                        Logger.Error($"[get sevenBread list] {e.Message}");
                        Logger.Info($"[get sevenBread list] possibly item added");
                    }
                        
                }
                
                return sevenBreadItemList;
            }
        }

        public List<SevenBreadDeletedItem> getSevenBreadDeletedItemList()
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/sevenbread/deleted/history");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);

            response = client.Execute<List<SevenBreadDeletedItem>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to get sevenbread deleted item list");
                return null;
            }
            else
            {
                Logger.Info("Success to get sevenbread deleted item list");
                var result = JObject.Parse(response.Content).GetValue("data");
                JArray jArray = JArray.Parse(result.ToString());

                List<SevenBreadDeletedItem> sevenBreadDeletedItemList = new List<SevenBreadDeletedItem>();

                foreach (JObject item in jArray)
                {
                    try
                    {
                        sevenBreadDeletedItemList.Add(new SevenBreadDeletedItem()
                        {
                            id = long.Parse(item.GetValue("id").ToString()),
                            itemCode = item.GetValue("itemCode").ToString(),
                            itemName = item.GetValue("itemName").ToString(),
                            capturedPrice = item.GetValue("capturedPrice") == null ? 9999999 : int.Parse(item.GetValue("capturedPrice").ToString()),
                            capturedDate = item.GetValue("capturedDate").ToString(),
                            majorHandler = item.GetValue("majorHandler").ToString(),
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.Error($"[get sevenBread deleted list] {e.Message}");
                        Logger.Info($"[get sevenBread deleted list] possibly item added");
                    }
                }

                return sevenBreadDeletedItemList;
            }
        }

        public string updateSevenBreadItemToday(Opt10001VO opt10001VO)
        {
            Logger.Info(opt10001VO.종목코드);
            client = new RestClient(BASE_URL + "/api/v1/platform/sevenbread/item/today/" + opt10001VO.종목코드);
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);
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
                Logger.Error("Error to update today sevenbread item v1");
                return null;
            }
            else
            {
                Logger.Info("Success to update today sevenbread item v1");
                var jObject = JObject.Parse(response.Content);
                return jObject.GetValue("data").ToString();
            }
        }

        public string updateSevenBreadItemCapturedDay(string itemCode, Opt10086VO opt10086VO)
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/sevenbread/item/" + itemCode);
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                closingPrice = opt10086VO.종가,
                capturedPrice = opt10086VO.종가,
                fluctuationRate = opt10086VO.등락률,
                priceByYesterday = opt10086VO.전일비,
                volume = opt10086VO.거래량,
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to update captured day sevenbread item v1");
                return null;
            }
            else
            {
                Logger.Info("Success to update captured day sevenbread item v1");
                var jObject = JObject.Parse(response.Content);
                return jObject.GetValue("data").ToString();
            }
        }

        public string updateSevenBreadV2ItemCapturedDay(string itemCode, Opt10086VO opt10086VO)
        {
            client = new RestClient(V2BASE_URL + "/api/v1/platform/v2/sevenbread/item/" + itemCode);
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                capturedPrice = opt10086VO.종가,
                lowestPrice = opt10086VO.저가
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to update captured day sevenbread item v2  " + response.StatusCode);
                return null;
            }
            else
            {
                Logger.Info("Success to update captured day sevenbread item v2");
                //var jObject = JObject.Parse(response.Content);
                //return jObject.GetValue("data").ToString();
                return "success";
            }
        }

        public string updateSevenBreadChartToday(Opt10081VO opt10081VO)
        {
            client = new RestClient(V2BASE_URL + "/api/v1/platform/v2/sevenbread/item/daily/chart");
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                closingDate = opt10081VO.일자,
                itemCode = opt10081VO.종목코드,
                highestPrice = opt10081VO.고가,
                lowestPrice = opt10081VO.저가,
                closingPrice = opt10081VO.현재가,
                modifyPriceType = opt10081VO.수정주가구분,
                modifyRate = opt10081VO.수정비율,
                modifyEvent = opt10081VO.수정주가이벤트,
            });

            response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return "Conflict";
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to update today sevenbread item v2");
                Logger.Error(response);
                return "Fail";
            }
            else
            {
                Logger.Info("Success to update today sevenbread item v2");
                return "Success";
            }
        }

        public string createSevenBreadItemHistory(string itemName, string itemCode, int startingPrice,
            int highestPrice, int lowestPrice, int closingPrice, string capturedDate, int capturedPrice)
        {
            client = new RestClient(BASE_URL + "/api/v1/platform/sevenbread/item/history");
            client.Timeout = -1;
            request = new RestRequest(Method.POST);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                itemName,
                itemCode,
                startingPrice,
                highestPrice,
                lowestPrice,
                closingPrice,
                capturedDate,
                capturedPrice,
            });

            response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Logger.Error("Error to insert sevenbread item history");
                Logger.Error(response);
                return null;
            }
            else
            {
                Logger.Info("Success to sevenbread item history");
                var jObject = JObject.Parse(response.Content);
                return jObject.GetValue("data").ToString();
            }
        }

        public string updateSevenBreadItemBuyingInfo(string itemCode, Opt10086VO opt10086VO)
        {
            client = new RestClient(V2BASE_URL + "/api/v1/platform/v2/sevenbread/item/daily/buying");
            client.Timeout = -1;
            request = new RestRequest(Method.PUT);
            request.AddParameter("Authorization", "Bearer " + this.v2token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                itemCode = itemCode,
                closingDate = opt10086VO.날짜,
                wBuyAmount = opt10086VO.외인순매수,
                gBuyAmount = opt10086VO.기관순매수,
                pBuyAmount = opt10086VO.개인순매수
            });

            Logger.Debug(itemCode);
            Logger.Debug(opt10086VO.날짜);
            Logger.Debug(opt10086VO.외인순매수);
            Logger.Debug(opt10086VO.기관순매수);
            Logger.Debug(opt10086VO.개인순매수);

            response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return "Conflict";
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error("Error to update today buying sevenbread item v2");
                return "Fail";
            }
            else
            {
                Logger.Info(response.Content.ToString());
                Logger.Info("Success to update today buying sevenbread item v2");
                return "Success";
            }
        }
    }
}
