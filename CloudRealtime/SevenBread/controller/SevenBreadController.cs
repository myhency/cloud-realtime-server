using CloudRealtime.SevenBread.handler;
using CloudRealtime.SevenBread.model;
using CloudRealtime.SevenBread.service;
using CloudRealtime.util;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudRealtime.SevenBread.controller
{
    public partial class SevenBreadController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private SevenBreadService sevenBreadService;
        private List<SevenBreadItem> sevenBreadItemList;
        private MyTelegramBot myTelegramBot;
        private Opt10001EventHandler opt10001EventHandler;
        private RealDataEventHandler realDataEventHandler;

        public SevenBreadController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.sevenBreadService = new SevenBreadService();
            this.myTelegramBot = new MyTelegramBot();
            this.opt10001EventHandler = new Opt10001EventHandler(axKHOpenAPI);
            
            initialize();
        }

        private void initialize()
        {
            //COMPLETE. 007빵에 새로운 종목이 등록되면 종목정보를 업데이트하기
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                sevenBreadItemconsumer();
            }));

            t1.Start();

            Thread t2 = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(10000);
                //COMPLETE. 007빵 리스트 가져오기 구현해야 함
                this.sevenBreadItemList = this.sevenBreadService.getSevenBreadItemList();
                this.realDataEventHandler = new RealDataEventHandler(this.axKHOpenAPI1, this.sevenBreadItemList);

                Logger.Info("===== 007빵리스트 종목 등록 중...");
                try
                {
                    if (this.sevenBreadItemList == null) throw new Exception("종목리스트 가져오기 실패");
                    foreach (SevenBreadItem item in this.sevenBreadItemList)
                    {
                        Logger.Info($"{item.itemName}({item.itemCode}) 종목이 등록되었습니다. 기준가격 : {item.capturedPrice} 원");
                        this.realDataEventHandler.setRealReg("7000", item.itemCode, "20;10;11;12;15;13;14;16;17;18;30", "1");
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                    myTelegramBot.sendTextMessageAsyncToSwingBot($"[007빵] 종목 등록 중 오류발생: {e.Message}");
                }
            }));

            t2.Start();


            Thread t3 = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(10000);
                Logger.Info("007빵 종목 종가정보 업데이트 쓰레드 시작");
                TimeSpan triggerTime = new TimeSpan(15, 35, 0);
                while (true)
                {
                    TimeSpan timeNow = DateTime.Now.TimeOfDay;

                    if (timeNow > triggerTime)
                    {
                        try
                        {
                            this.sevenBreadItemList = this.sevenBreadService.getSevenBreadItemList();
                            foreach (SevenBreadItem item in this.sevenBreadItemList)
                            {
                                opt10001EventHandler.requestTrOpt10001(item.itemCode, $"007빵종가업데이트_{item.itemName}");
                                Thread.Sleep(1500);
                            }

                            DateTime today = DateTime.Now;
                            string strNow = today.ToString("yyyy년 MM월 dd일");
                            string strDay = today.ToString("yyyy-MM-dd");

                            myTelegramBot.sendTextMessageAsyncToBot(
                                $"✔️ {strNow} 007빵 종목 종가 업데이트가 완료되었습니다.\n" +
                                "http://myhency.asuscomm.com:13000/service/seven-bread"
                            );
                            break;
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e.Message);
                        }
                    }
                }
            }));

            t3.Start();
        }

        private void sevenBreadItemconsumer()
        {
            // https://eecs.blog/c-sending-and-receiving-data-from-a-thread/ 이거 참고해서 만들 것
            var conf = new ConsumerConfig
            {
                GroupId = "sevenBread-consumer-group",
                BootstrapServers = "192.168.29.189:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe("seven_bread");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            // 카프카에 새로 등록된 007빵 종목
                            Logger.Info($"[007빵] 새롭게 추가된 종목 : {axKHOpenAPI1.GetMasterCodeName(cr.Message.Value)}[{cr.Message.Value}]");
                            myTelegramBot.sendTextMessageAsyncToSwingBot($"[007빵] 새롭게 추가된 종목 : {axKHOpenAPI1.GetMasterCodeName(cr.Message.Value)}[{cr.Message.Value}]");
                            opt10001EventHandler.requestTrOpt10001(cr.Message.Value, $"007빵");
                            Thread.Sleep(1000);

                        }
                        catch (ConsumeException e)
                        {
                            Logger.Error($"[007빵] Kafka consumer 오류:{e.Error.Reason}");
                            myTelegramBot.sendTextMessageAsyncToSwingBot($"[007빵] Kafka consumer 오류:{e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
    }
}