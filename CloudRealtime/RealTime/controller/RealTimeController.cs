using CloudRealtime.RealTime.handler;
using CloudRealtime.RealTime.model;
using CloudRealtime.RealTime.service;
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

namespace CloudRealtime.RealTime.controller
{
    public partial class RealTimeController : IRealTimeController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private AlarmService alarmService;
        private RealDataEventHandler realDataEventHandler;
        private List<Alarm> alarmList;
        private MyTelegramBot myTelegramBot;

        public RealTimeController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            DateTime today = DateTime.Now;
            DateTime startMarketTime = new DateTime(today.Year, today.Month, today.Day, 09, 0, 0);
            string strNow = today.ToString("yyyy년 MM월 dd일");
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.alarmService = new AlarmService();
            this.myTelegramBot = new MyTelegramBot();
            //알리미서버에서 가져오는 알람리스트
            //TODO. 실시간으로 입력되는 알람은 Kafka consumer가 가져오도록 구현해야 함.
            this.alarmList = this.alarmService.getAlarmList();
            this.realDataEventHandler = new RealDataEventHandler(this, axKHOpenAPI, this.alarmList);
            //this.myTelegramBot.sendTextMessageAsyncToBot($"🤑 {strNow} 클라우드의 주식 훈련소알리미 출발합니다 🤑");
            initialize();
        }

        public void sendTextMessageAsyncToBot(string alarmMessage)
        {
            this.myTelegramBot.sendTextMessageAsyncToBot(alarmMessage);
        }

        private void consumer(RealDataEventHandler realDataEventHandler)
        {
            // https://eecs.blog/c-sending-and-receiving-data-from-a-thread/ 이거 참고해서 만들 것
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "192.168.29.189:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe("test_topic");

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
                            // 여기서 setRealReg 에 등록해 줘야 함.
                            Alarm item = JsonConvert.DeserializeObject<Alarm>(cr.Message.Value);
                            Alarm alarm = this.alarmList.FirstOrDefault(v => v.itemCode.Equals(item.itemCode));
                            if(alarm is null)
                            {
                                realDataEventHandler.setRealReg("2000", item.itemCode, "20;10;11;12;15;13;14;16;17;18;30", "1");
                                this.alarmList.Add(item);
                                realDataEventHandler.setAlarmList(this.alarmList);
                                Logger.Info($"{item.itemName}({item.itemCode}) 종목이 등록되었습니다.");
                            }
                            else
                            {
                                //this.alarmList.Where(v => v.recommendPrice)
                                alarm.losscutPrice = item.losscutPrice;
                                alarm.recommendPrice = item.recommendPrice;
                                realDataEventHandler.setAlarmList(this.alarmList);
                                Logger.Info($"{item.itemName}({item.itemCode}) 종목이 변경되었습니다.");
                            }
                            
                        }
                        catch (ConsumeException e)
                        {
                            Logger.Info($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }

        private void initialize()
        {
            foreach (Alarm item in this.alarmList)
            {
                Logger.Info($"{item.itemName}({item.itemCode}) 종목이 등록되었습니다.");
                this.realDataEventHandler.setRealReg("2000", item.itemCode, "20;10;11;12;15;13;14;16;17;18;30", "1");
            }

            Thread t1 = new Thread(new ThreadStart(() =>
            {
                consumer(this.realDataEventHandler);
            }));

            t1.Start();
        }
    }
}
