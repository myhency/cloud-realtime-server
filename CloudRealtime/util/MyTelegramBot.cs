﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CloudRealtime.util
{
    public partial class MyTelegramBot
    {
        private const string START = "/start";

        IniFile ini = new IniFile();
        private string telegramToken = "";
        private TelegramBotClient bot;
        private string userId;

        public MyTelegramBot()
        {
            initializer();
        }

        private void initializer()
        {
            ini.Load(Application.StartupPath + "\\settings.ini");
            this.telegramToken = ini["Settings"]["Token"].ToString();
            this.userId = ini["Settings"]["UserId"].ToString();

            if (telegramToken.Equals("") || telegramToken == null)
            {
                MessageBox.Show("설정 탭으로 이동하여 BotFather에서 발급받은 토큰을 입력하세요\n" +
                    "토큰이 없을 경우 실시간 알람이 정상작동 하지 않습니다.", "Warning");
            }
            else
            {
                bot = new TelegramBotClient(telegramToken);
                bot.OnMessage += bot_OnMessage;
                bot.StartReceiving();
            }
        }

        private void bot_OnMessage(object sender, MessageEventArgs e)
        {
            Telegram.Bot.Types.Message message = e.Message;

            if (message.Type == MessageType.Text)
            {
                string content = message.Text;
                long userId = message.Chat.Id;
                this.userId = userId.ToString();

                switch (content) //알람 사용자 추가
                {
                    case START:
                        bot.SendTextMessageAsync(userId, "알람을 시작합니다.");
                        ini["Settings"]["LicenseKey"] = ini["Settings"]["LicenseKey"].ToString();
                        ini["Settings"]["Token"] = ini["Settings"]["Token"].ToString();
                        ini["Settings"]["UserId"] = this.userId;
                        ini.Save(Application.StartupPath + "\\settings.ini");

                        //var menu = new InlineKeyboardMarkup(new[] { 
                        //    new[]
                        //    {
                        //        new InlineKeyboardButton{ Text = "item", CallbackData = "1" },
                        //        new InlineKeyboardButton{ Text = "item", CallbackData = "2" },
                        //    }
                        //});

                        //bot.SendTextMessageAsync(userId, "Text", replyMarkup: menu);
                        break;
                    default:
                        break;
                }
            }
        }

        public void setLoginResult(int nErrCode)
        {
            switch (nErrCode)
            {
                case 0:
                    sendTextMessageAsyncToBot("로그인 성공");
                    break;
                default:
                    sendTextMessageAsyncToBot("로그인 실패");
                    break;
            }
        }

        public void sendTextMessageAsyncToBot(string alarmMessage)
        {
            bot.SendTextMessageAsync(long.Parse(this.userId), alarmMessage, parseMode: ParseMode.Markdown);
        }
    }
}