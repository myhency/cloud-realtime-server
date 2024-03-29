﻿using CloudRealtime.RealCondition.model;
using CloudRealtime.util;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudRealtime.RealCondition.handler
{
    public partial class RealConditionEventHandler
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private Opt10001EventHandler opt10001EventHandler;
        private MyTelegramBot myTelegramBot;
        private List<Condition> conditionList;
        string[] itemCodeList = { };
        string[] breadShuttleItemCodeList = { };
        private IFirebaseConfig config;
        private IFirebaseClient client;
        private Boolean isMorningThreadStarted = false;


        public RealConditionEventHandler(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.opt10001EventHandler = new Opt10001EventHandler(axKHOpenAPI);
            this.myTelegramBot = new MyTelegramBot();
            initialize();
        }

        private void initialize()
        {
            axKHOpenAPI1.OnReceiveConditionVer += axKHOpenAPI1_OnReceiveConditionVer;
            axKHOpenAPI1.OnReceiveTrCondition += axKHOpenAPI1_OnReceiveTrCondition;
            axKHOpenAPI1.OnReceiveRealCondition += axKHOpenAPI1_OnReceiveRealCondition;
            axKHOpenAPI1.OnReceiveTrData += axKHOpenAPI1_OnReceiveTrData;
            axKHOpenAPI1.OnReceiveRealData += axKHOpenAPI1_OnReceiveRealData;

            axKHOpenAPI1.SetRealReg("8000", "000660", "215,214", "1");

            config = new FirebaseConfig
            {
                AuthSecret= "mYXKscnYNaaHOfLp6Lgyo6xpZBQT1ZoA5N1iv5Vl",
                BasePath= "https://bread-stock.firebaseio.com/"
            };

            client = new FireSharp.FirebaseClient(config);

            if (client != null)
            {
                Logger.Info("Connected to firebase");
            }

            /**
             * 사용자 조건검색식의 실행순서:
             *  1. GetConditionLoad() -> 사용자의 조건식으로 로딩
             *  2. axKHOpenAPI1_OnReceiveConditionVer -> 사용자의 조건식리스트를 검색
             *  3. SendCondition -> 특정 조건식으로 종목검색
             *  4.1. axKHOpenAPI1_OnReceiveTrCondition -> 정적 검색 결과
             *  4.2. axKHOpenAPI1_OnReceiveRealCondition -> 실시간 검색 결과
             */
            this.axKHOpenAPI1.GetConditionLoad(); //사용자 조건검색식 로딩

            this.conditionList = new List<Condition>(); //사용자 조건식 리스트

            TimeSpan morningTriggerTime = new TimeSpan(09, 00, 0);
            TimeSpan marketEndTriggerTime = new TimeSpan(15, 20, 0);
            TimeSpan afternoonTriggerTime = new TimeSpan(15, 41, 0);
            TimeSpan timeNow = DateTime.Now.TimeOfDay;
            if (timeNow >= morningTriggerTime && timeNow < marketEndTriggerTime)
            {
                startThread(3);
                startThread(4);
                startThread(5);
            } 
            else if (timeNow >= afternoonTriggerTime)
            {
                startThread(1);
                startThread(2);
            }

        }

        private void startThread(int threadNumber)
        {
            // TODO.test 후에 주석 풀기
            // t1과 t2의 용도:
            //  - t1은 16:00가 되면 "유통거래량" 이라는 조건식을 검색해서 종목을 찾는다.
            //  - t2는 t1이 종목을 다 찾으면 종목하나씩 tr 요청을 해서 종목정보를 받고 db에 업데이트 한다.
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(10000);
                Logger.Info("유통거래량 전송 쓰레드 시작");
                sendCondition("3000", "유통거래량", false);
            }));

            Thread t2 = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(20000);
                Logger.Info("유통거래량 수집 쓰레드 시작");
                if (itemCodeList.Length > 0)
                {
                    foreach (string itemCode in itemCodeList)
                    {
                        if (itemCode.Length > 0)
                        {
<<<<<<< HEAD
                            foreach (string itemCode in itemCodeList)
                            {
                                if (itemCode.Length > 0)
                                {
                                    opt10001EventHandler.requestTrOpt10001(itemCode, $"유통거래량_{itemCodeList.Length.ToString()}");
                                    Thread.Sleep(1500);
                                    itemCodeList = itemCodeList.Where(v => v != itemCode).ToArray();
                                }
                            }

                            Thread.Sleep(30000);
                            DateTime today = DateTime.Now;
                            DateTime startMarketTime = new DateTime(today.Year, today.Month, today.Day, 09, 0, 0);
                            string strNow = today.ToString("yyyy년 MM월 dd일");
                            string strDay = today.ToString("yyyy-MM-dd");
                            this.myTelegramBot.sendTextMessageAsyncToSwingBot(
                                $"✔️ {strNow} 유통주식대비 거래량 비율 업데이트가 완료되었습니다. ");
                            break;
=======
                            opt10001EventHandler.requestTrOpt10001(itemCode, $"유통거래량_{itemCodeList.Length.ToString()}");
                            Thread.Sleep(1500);
                            itemCodeList = itemCodeList.Where(v => v != itemCode).ToArray();
>>>>>>> server-change
                        }
                    }

                    Thread.Sleep(30000);
                    DateTime today = DateTime.Now;
                    DateTime startMarketTime = new DateTime(today.Year, today.Month, today.Day, 09, 0, 0);
                    string strNow = today.ToString("yyyy년 MM월 dd일");
                    string strDay = today.ToString("yyyy-MM-dd");
                    this.myTelegramBot.sendTextMessageAsyncToBot(
                        $"✔️ {strNow} 유통주식대비 거래량 비율 업데이트가 완료되었습니다. ");
                }
            }));

            Thread t3 = new Thread(new ThreadStart(() =>
            {
                Logger.Info("250일 신고거래량 조건검색 쓰레드 시작");
                Thread.Sleep(30000);
                sendCondition("3000", "오늘돈이몰린종목", true);
            }));

            Thread t4 = new Thread(new ThreadStart(() =>
            {
                Logger.Info("유통거래량_코스피 조건검색 쓰레드 시작");
                Thread.Sleep(40000);
                sendCondition("3000", "유통거래량_코스피", true);
            }));

            Thread t5 = new Thread(new ThreadStart(() =>
            {
                Logger.Info("유통거래량_코스닥 조건검색 쓰레드 시작");
                Thread.Sleep(60000);
                sendCondition("3000", "유통거래량_코스닥", true);
            }));

            switch(threadNumber)
            {
                case 1:
                    if (!t1.IsAlive)
                        t1.Start();
                    break;
                case 2:
                    if (!t2.IsAlive)
                        t2.Start();
                    break;
                case 3:
                    if (!t3.IsAlive)
                        t3.Start();
                    break;
                case 4:
                    if (!t4.IsAlive)
                        t4.Start();
                    break;
                case 5:
                    if (!t5.IsAlive)
                        t5.Start();
                    break;
                default:
                    break;
            }
        }

        private void axKHOpenAPI1_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e) 
        {
            /* 
             * ThreadNumber
             * 1: 유통거래량 전송 쓰레드 15:40 + 10s
             * 2: 유통거래량 수집 쓰레드 15:40 + 20s
             * 3: 250일 신고거래량 조건검색 쓰레드 08:30 + 30s
             * 4: 유통거래량_코스피 조건검색 쓰레드 08:30 + 40s
             * 5: 유통거래량_코스닥 조건검색 쓰레드 08:30 + 60s
             */
            Logger.Debug("e.sRealType:  " + e.sRealType);
            if (e.sRealType.Equals("장시작시간"))
            {
                string value = axKHOpenAPI1.GetCommRealData(e.sRealKey, 215);
                Logger.Debug("장시작시간:  " + value);
                if(value.Equals("0"))
                {
                    TimeSpan triggerTime = new TimeSpan(08, 30, 0);
                    TimeSpan timeNow = DateTime.Now.TimeOfDay;
                    if (timeNow > triggerTime && !isMorningThreadStarted)
                    {
                        startThread(3);
                        startThread(4);
                        startThread(5);
                        isMorningThreadStarted = true;
                    }
                }
                else if (value.Equals("a"))
                {
                    startThread(1);
                    startThread(2);
                }
            }
        }

        private void axKHOpenAPI1_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveTrData");
        }

        private void axKHOpenAPI1_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveRealCondition");
            if (e.strType.Equals("I"))
            {
                //여기서 빵셔틀 조건식을 필터링 한다.
                if (e.strConditionName.Equals("오늘돈이몰린종목"))
                {
                    string itemCode = e.sTrCode;
                    string itemName = axKHOpenAPI1.GetMasterCodeName(itemCode);
                    DateTime today = DateTime.Now;
                    string strNow = today.ToString("yyyyMMddHmmss");
                    string strToday = today.ToString("yyyyMMdd");
                    string path = $"bread-local-test/shuttle/{itemCode}";

                    // # shuttle_ref 는 실시간, today_ref는 오늘 검색된 종목 다 포함
                    // 1. firebase db를 세팅
                    // 2. firebase db shuttle_ref에 값을 넣는다.
                    //     {
                    //       "code": itemCode,
                    //       "name": itemName,
                    //       "updatetime": strNow
                    //     }
                    // 3. 최초등록시간이 있는지 확인해보고
                    // 없으면 today_ref에 최초등록시간을 넣어주고 텔레그램 발송,
                    // 있으면 today_ref에 updatetime을 strNow로 업데이트한다.

                    //ref. https://www.tutorialspoint.com/how-to-run-multiple-async-tasks-and-waiting-for-them-all-to-complete-in-chash
                    var t = Task.Run(async () => await selectDataFromFirebase(path));

                    Task.WaitAll(t);

                    if (t.Result == null)  //최초
                    {
                        var data = new BreadShuttleData
                        {
                            code = itemCode,
                            name = itemName,
                            updatetime = strNow,
                            firsttime = strNow
                        };

                        Task.Run(async () => await insertDataToFirebase(path, data));
                        Logger.Info($"[firebase] {itemName} 종목 최초 등록 완료");
                    }
                    else // 두 번째 부터
                    {
                        var data = new BreadShuttleData
                        {
                            code = itemCode,
                            name = itemName,
                            updatetime = strNow,
                            firsttime = t.Result.firsttime
                        };

                        Task.Run(async () => await updateDataToFirebase(path, data));
                        Logger.Info($"[firebase] {itemName} 종목 두 번째 등록 완료");
                    }

                    opt10001EventHandler.requestTrOpt10001(itemCode, "빵셔틀종목");
                }
                else if (e.strConditionName.Equals("빵셔틀단타"))
                {
                    string itemCode = e.sTrCode;
                    opt10001EventHandler.requestTrOpt10001(itemCode, "빵셔틀단타");
                }
                else if (e.strConditionName.Equals("유통거래량_코스피") || e.strConditionName.Equals("유통거래량_코스닥"))
                {
                    string itemCode = e.sTrCode;
                    string itemName = this.axKHOpenAPI1.GetMasterCodeName(itemCode).Trim();
                    DateTime today = DateTime.Now;
                    string strNow = today.ToString("yyyyMMddHmmss");
                    string strToday = today.ToString("yyyyMMdd");
                    string path = $"bread-local-test/volume/{strToday}/{itemCode}";
                    var data = new BreadShuttleData
                    {
                        code = itemCode
                    };

                    try
                    {
                        Task.Run(async () => await insertDataToFirebase(path, data));
                        this.myTelegramBot.sendTextMessageAsyncToSwingBot($"{e.strConditionName} - {itemName}({itemCode})");
                    } catch (Exception exception)
                    {
                        Logger.Error(exception.Message.ToString());
                    }
                }
            }
            else
            {
                if (e.strConditionName.Equals("오늘돈이몰린종목"))
                {
                    string itemCode = e.sTrCode;
                    string path = $"bread-local-test/shuttle/{itemCode}";
                    //실시간 조건검색 에서 이탈하는 경우
                    //firebase db의 shuttle_ref에서 해당코드를 지운다.
                    deleteDataFromFirebase(path);
                }
            }
        }

        private void axKHOpenAPI1_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveTrCondition");
            if (e.strConditionName.Equals("유통거래량"))
            {
                Logger.Debug(e.strCodeList);
                this.itemCodeList = e.strCodeList.TrimEnd(';').Split(';');
            }
        }

        private void axKHOpenAPI1_OnReceiveConditionVer(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveConditionVer");
            string conditionList = axKHOpenAPI1.GetConditionNameList();

            Logger.Info("사용자 조건식 로딩 완료");

            string[] conditionArray = conditionList.TrimEnd(';').Split(';');

            foreach (string conditionInfo in conditionArray)
            {
                if (conditionInfo.Length > 0)
                {
                    //condition[0] : 조건식 인덱스
                    //condition[1] : 조건식 이름
                    string[] condition = conditionInfo.Split('^');

                    this.conditionList.Add(new Condition(int.Parse(condition[0]), condition[1]));
                }
            }
        }

        public void sendCondition(string screenNumber, string conditionName, bool isRealTime)
        {
            Logger.Debug("sendCondition");
            foreach (Condition item in this.conditionList)
            {
                if (item.name.Equals(conditionName))
                {
                    int result = axKHOpenAPI1.SendCondition(
                        screenNumber,
                        item.name,
                        item.index,
                        isRealTime ? 1 : 0
                    );
                    Logger.Info("sendcondition result: " + result);
                    break;
                }
            }
        }

        public async Task<BreadShuttleData> insertDataToFirebase(string path, BreadShuttleData data)
        {
            SetResponse response = await client.SetAsync(path, data);
            Logger.Info($"[firebase] Success to insert data:{response.ResultAs<BreadShuttleData>().name}");
            return response.ResultAs<BreadShuttleData>();
        }

        public async Task<BreadShuttleData> selectDataFromFirebase(string path)
        {
            FirebaseResponse response = await client.GetAsync(path);
            return response.ResultAs<BreadShuttleData>();
        }

        public async Task updateDataToFirebase(string path, BreadShuttleData data)
        {
            FirebaseResponse response = await client.UpdateAsync(path, data);
            Logger.Info($"[firebase] Success to update data:{response.ResultAs<BreadShuttleData>().name}");
        }

        public async void deleteDataFromFirebase(string path)
        {
            FirebaseResponse response = await client.DeleteAsync(path);
            Logger.Info($"[firebase] Success to delete data:{path} --> {response.StatusCode}");
        }
    }
}
