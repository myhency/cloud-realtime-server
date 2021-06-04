﻿using CloudRealtime.RealTime.handler;
using CloudRealtime.RealTime.model;
using CloudRealtime.RealTime.service;
using CloudRealtime.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.controller
{
    public partial class RealTimeController : IRealTimeController
    {
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private AlarmService alarmService;
        private RealDataEventHandler realDataEventHandler;
        private List<Alarm> alarmList;
        private MyTelegramBot myTelegramBot;

        public RealTimeController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.alarmService = new AlarmService();
            this.myTelegramBot = new MyTelegramBot();
            //알리미서버에서 가져오는 알람리스트
            //TODO. 실시간으로 입력되는 알람은 Kafka consumer가 가져오도록 구현해야 함.
            this.alarmList = this.alarmService.getAlarmList();
            this.realDataEventHandler = new RealDataEventHandler(this, axKHOpenAPI, this.alarmList);
            this.myTelegramBot.sendTextMessageAsyncToBot("알리미 시작");
            initialize();
        }

        public void sendTextMessageAsyncToBot(string alarmMessage)
        {
            this.myTelegramBot.sendTextMessageAsyncToBot(alarmMessage);
        }

        private void initialize()
        {
            foreach(Alarm item in this.alarmList)
            {
                this.realDataEventHandler.setRealReg("2000", item.itemCode, "20;10;11;12;15;13;14;16;17;18;30", "1");
            }
        }
    }
}
