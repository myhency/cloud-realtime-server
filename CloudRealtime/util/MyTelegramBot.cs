using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using System.Configuration;
using System.Collections.Specialized;

namespace CloudRealtime.util
{
    public partial class MyTelegramBot
    {
        private const string START = "/start";

        private string telegramToken = "";
        private string swingTelegramToken = "";
        private TelegramBotClient bot;
        private TelegramBotClient swingbot;
        private string userId;
        private string swingId;

        public MyTelegramBot()
        {
            initializer();
        }

        private void initializer()
        {
            this.telegramToken = ConfigurationManager.AppSettings.Get("ProdToken");
            this.swingTelegramToken = ConfigurationManager.AppSettings.Get("SwingToken");
            this.userId = ConfigurationManager.AppSettings.Get("ProdBotId");
            this.swingId = ConfigurationManager.AppSettings.Get("SwingBotId");


            if (telegramToken.Equals("") || telegramToken == null)
            {
                MessageBox.Show("설정 탭으로 이동하여 BotFather에서 발급받은 토큰을 입력하세요\n" +
                    "토큰이 없을 경우 실시간 알람이 정상작동 하지 않습니다.", "Warning");
            }
            else
            {
                Console.WriteLine(this.telegramToken);
                bot = new TelegramBotClient(this.telegramToken);
                bot.OnMessage += bot_OnMessage;
                bot.StartReceiving();
                swingbot = new TelegramBotClient(this.swingTelegramToken);
                swingbot.OnMessage += bot_OnMessage;
                swingbot.StartReceiving();
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

        public async void sendTextMessageAsyncToBot(string alarmMessage)
        {
            try
            {
                await bot.SendTextMessageAsync(long.Parse(this.userId), alarmMessage);
            } catch (Exception e)
            {
                await swingbot.SendTextMessageAsync(long.Parse(this.swingId), e.Message);
            }
        }

        public async void sendTextMessageAsyncToSwingBot(string alarmMessage)
        {
            await swingbot.SendTextMessageAsync(long.Parse(this.swingId), alarmMessage);
        }

        public void sendFileAsyncToBot(string path)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            {
                InputOnlineFile inputOnlineFile = new InputOnlineFile(fs, path);
                bot.SendDocumentAsync(long.Parse(this.userId), inputOnlineFile);
            }
        }
    }
}
