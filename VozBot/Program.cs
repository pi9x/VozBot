using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using VozBotLibrary;
using VozBotLibrary.DataAccess;
using VozBotLibrary.Models;

namespace VozBot
{
    class Program
    {
        static ITelegramBotClient vozBot;

        static void Main(string[] args)
        {
            vozBot = new TelegramBotClient(ConfigGetter.GetAPI());
            var me = vozBot.GetMeAsync().Result;
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

            vozBot.OnMessage += Bot_OnMessage;
            vozBot.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            // UPDATE ADMINS
            ChatMember[] chatAdmins = await vozBot.GetChatAdministratorsAsync(e.Message.Chat);
            List<Admin> admins = new List<Admin>();
            for (int i = 0; i < chatAdmins.Length; i++)
            {
                admins.Add(new Admin { UserId = chatAdmins[i].User.Id });
            }
            AccessAdmin.UpdateAdmin(admins);

            // WELCOME
            //-- Get welcome
            if (e.Message.NewChatMembers != null && e.Message.NewChatMembers.Length > 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(AccessWelcome.GetWelcome().Count());
                await vozBot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: MessageHelper.Unmasking(AccessWelcome.GetWelcome()[index].Message, e.Message.From.FirstName, e.Message.From.LastName, e.Message.From.Username, e.Message.Chat.Title)
                );
            }
            //-- Set welcome
            if (e.Message.Text != null)
                if (e.Message.Text.ToUpper().Contains("/WELCOME "))
                    if (AccessAdmin.GetAdmin().Exists(x => x.UserId == e.Message.From.Id) && MessageHelper.Reform(e.Message.Text.Remove(0, 8)) != "")
                    {
                        AccessWelcome.AddWelcome(new Welcome() { Message = MessageHelper.Reform(e.Message.Text.Remove(0, 8)) });

                        await vozBot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Xong!",
                            replyToMessageId: e.Message.MessageId
                        );
                    }
                    else
                    {
                        await vozBot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Không được đâu bạn êy!",
                            replyToMessageId: e.Message.MessageId
                        );
                    }


            // NOTE




            // FILTER
            //-- Get filter
            if (e.Message.Text != null)
            {
                double max = 0;
                Filter mostMatch = new Filter();
                foreach (var filter in AccessFilter.GetFilter())
                {
                    if (MessageHelper.MatchingRate(e.Message.Text, filter.Key) > max)
                    {
                        max = MessageHelper.MatchingRate(e.Message.Text, filter.Key);
                        mostMatch = filter;
                    }
                }
                if (max > 0)
                    await vozBot.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: MessageHelper.Unmasking(mostMatch.Message, e.Message.From.FirstName, e.Message.From.LastName, e.Message.From.Username, e.Message.Chat.Title),
                        replyToMessageId: e.Message.MessageId
                    );
            }

            //-- Set filter
            if (e.Message.Text != null)
                if (e.Message.Text.ToUpper().Contains("/FILTER "))
                    if (AccessAdmin.GetAdmin().Exists(x => x.UserId == e.Message.From.Id) && MessageHelper.Reform(e.Message.Text.Remove(0, 7)) != "")
                    {
                        try
                        {
                            char delim = '|';
                            List<string> keys = MessageHelper.Reform(e.Message.Text.Remove(0, 7)).Split(delim, StringSplitOptions.RemoveEmptyEntries).ToList();
                            for (int i = 0; i < keys.Count; i++)
                            {
                                keys[i] = keys[i].Trim();
                            }
                            foreach (string key in keys)
                            {
                                if (key == "") keys.Remove(key);
                            }
                            Filter filter = new Filter { Message = keys.Last() };
                            keys.Remove(keys.Last());
                            filter.Key = keys;
                            AccessFilter.AddFilter(filter);

                            await vozBot.SendTextMessageAsync(
                                chatId: e.Message.Chat,
                                text: "Đã xong!",
                                replyToMessageId: e.Message.MessageId
                            );
                        }
                        catch (NullReferenceException)
                        {
                            await vozBot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Lỗi rồi comrade ơi :(",
                            replyToMessageId: e.Message.MessageId
                            );
                        }
                        
                    }


            // Bio
            //-- Get bio
            if (e.Message.Text != null)
                if (e.Message.Text.ToUpper() == "/BIO")
                {
                    bool found = false;
                    if (e.Message.ReplyToMessage == null)
                    {
                        foreach (var bio in AccessBio.GetBio())
                        {
                            if (e.Message.From.Id == bio.UserId)
                            {
                                await vozBot.SendTextMessageAsync(
                                    chatId: e.Message.Chat,
                                    text: MessageHelper.Unmasking(bio.UserBio, e.Message.From.FirstName, e.Message.From.LastName, e.Message.From.Username, e.Message.Chat.Title),
                                    replyToMessageId: e.Message.MessageId
                                );
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            await vozBot.SendTextMessageAsync(
                                chatId: e.Message.Chat,
                                text: MessageHelper.Reform($"{e.Message.From.FirstName} {e.Message.From.LastName} chưa có cái bio nào cả. Comrade nào viết bio cho {e.Message.From.FirstName} {e.Message.From.LastName} đi mà..."),
                                replyToMessageId: e.Message.MessageId
                            );
                        }
                    }
                    else
                    {
                        foreach (var bio in AccessBio.GetBio())
                        {
                            if (e.Message.ReplyToMessage.From.Id == bio.UserId)
                            {
                                await vozBot.SendTextMessageAsync(
                                    chatId: e.Message.Chat,
                                    text: MessageHelper.Unmasking(bio.UserBio, e.Message.ReplyToMessage.From.FirstName, e.Message.ReplyToMessage.From.LastName, e.Message.ReplyToMessage.From.Username, e.Message.Chat.Title),
                                    replyToMessageId: e.Message.MessageId
                                );
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            await vozBot.SendTextMessageAsync(
                                chatId: e.Message.Chat,
                                text: MessageHelper.Reform($"{e.Message.ReplyToMessage.From.FirstName} {e.Message.ReplyToMessage.From.LastName} chưa có cái bio nào cả. Comrade nào viết bio cho {e.Message.ReplyToMessage.From.FirstName} {e.Message.ReplyToMessage.From.LastName} đi mà..."),
                                replyToMessageId: e.Message.MessageId
                            );
                        }
                    }
                }

            //-- Set bio
            if (e.Message.Text != null)
                if (e.Message.Text.ToUpper().Contains("/SETBIO "))
                    if (e.Message.ReplyToMessage == null || e.Message.From.Id == e.Message.ReplyToMessage.From.Id)
                    {
                        await vozBot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Tự trọng đê bạn êi!",
                            replyToMessageId: e.Message.MessageId
                        );
                    }
                    else if (AccessAdmin.GetAdmin().Exists(x => x.UserId == e.Message.From.Id) && MessageHelper.Reform(e.Message.Text.Remove(0, 7)) != "")
                    {
                        string biotext = MessageHelper.Reform($"{e.Message.ReplyToMessage.From.FirstName} {e.Message.ReplyToMessage.From.LastName}:\n") + MessageHelper.Reform(e.Message.Text.Remove(0, 7));
                        AccessBio.AddBio(new Bio() { UserId = e.Message.ReplyToMessage.From.Id, UserBio = biotext });

                        await vozBot.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: MessageHelper.Reform($"Đã set bio cho {e.Message.ReplyToMessage.From.FirstName} {e.Message.ReplyToMessage.From.LastName}."),
                        replyToMessageId: e.Message.MessageId
                        );
                    }
                    else
                    {
                        await vozBot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Không set được đâu bạn êy!",
                            replyToMessageId: e.Message.MessageId
                        );
                    }


            /// Private chat
            
        }
    }
}
