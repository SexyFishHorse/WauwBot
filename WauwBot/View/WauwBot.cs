namespace SexyFishHorse.WauwBot.View
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using SexyFishHorse.Irc.Client;
    using SexyFishHorse.Irc.Client.Clients;
    using SexyFishHorse.Irc.Client.Configuration;
    using SexyFishHorse.Irc.Client.EventHandling.EventArgs;
    using SexyFishHorse.Irc.Client.Models;
    using SexyFishHorse.WauwBot.Model;
    using SexyFishHorse.WauwBot.View.UiElements;

    public partial class WauwBot : Form
    {
        public WauwBot(
            ITwitchIrcClient twitchClient,
            IConfiguration twitchConfiguration)
        {
            TwitchClient = twitchClient;

            InitializeComponent();

            Running = true;
            twitchClient.Connect();

            var twitchThread = new Thread(ReadTwitchMessages);
            twitchThread.Start();

            twitchClient.SendRawMessage(IrcCommandsFactory.CapReq(twitchConfiguration.TwitchIrcMembershipCapability));
            twitchClient.SendRawMessage(IrcCommandsFactory.Join(twitchConfiguration.TwitchIrcNickname));
        }

        private delegate void AddChatMessage(ChatMessage msg);

        public ITwitchIrcClient TwitchClient { get; set; }

        public bool Running { get; set; }

        public void MessageSent(OnMessageSentEventArgs obj)
        {
            AddChatItem(new ChatMessage { Message = obj.Message, Provider = Provider.Omni, Timestamp = DateTime.Now, Username = "YOU" });
        }

        private void AddChatItem(ChatMessage chatMessage)
        {
            if (chatListBox.InvokeRequired)
            {
                AddChatMessage d = AddChatItem;
                Invoke(d, chatMessage);
            }
            else
            {
                chatListBox.Items.Add(new ChatLine(chatMessage));
                chatListBox.Invalidate();
            }
        }

        private void SendButtonClick(object sender, EventArgs e)
        {
            var message = messageTextBox.Text.Trim();

            TwitchClient.SendChatMessage(message);
            AddChatItem(new ChatMessage { Message = message, Provider = Provider.Omni, Username = "YOU", Timestamp = DateTime.Now });
            messageTextBox.Clear();
        }

        private void ReadTwitchMessages()
        {
            ReadMessages(TwitchClient, string.Format("#{0}", "imasser"));
        }

        private void ReadMessages(IIrcClient client, string channelName)
        {
            while (Running)
            {
                IrcMessage ircMessage;

                try
                {
                    ircMessage = client.ReadIrcMessage();
                }
                catch (IOException)
                {
                    continue;
                }

                if (ircMessage == null)
                {
                    continue;
                }

                var rawMessage = ircMessage.Raw;

                if (rawMessage.Contains(string.Format(" PRIVMSG {0}", channelName)))
                {
                    var username = rawMessage.Substring(1, rawMessage.IndexOf('!') - 1);

                    var privMsgEnd = string.Format("PRIVMSG {0} :", channelName);
                    var message = rawMessage.Substring(rawMessage.IndexOf(privMsgEnd, StringComparison.Ordinal) + privMsgEnd.Length);

                    Console.WriteLine("<{0}> {1}", username, message.Trim());

                    AddChatItem(
                        new ChatMessage
                        {
                            Message = message,
                            Provider = Provider.Twitch,
                            Timestamp = DateTime.Now,
                            Username = username
                        });
                }
                else if (rawMessage.Contains(string.Format(" JOIN {0}", channelName)))
                {
                    var username = rawMessage.Substring(1, rawMessage.IndexOf('!') - 1);
                    Console.WriteLine("<SYSTEM> {0} JOINED!", username);
                }
                else if (rawMessage.Contains(string.Format(" MODE {0}", channelName)))
                {
                    var messageParts = rawMessage.Split(' ');
                    var username = messageParts[messageParts.Length - 1];

                    if (rawMessage.Contains("+o"))
                    {
                        Console.WriteLine("<SYSTEM> {0} OBTAINED OPERATOR", username);
                    }
                    else if (rawMessage.Contains("-o"))
                    {
                        Console.WriteLine("<SYSTEM> {0} LOST OPERATOR", username);
                    }
                    else
                    {
                        Console.WriteLine("<SYSTEM> Unknown mode for {0}, here's the raw message:\n{1}", username, rawMessage);
                    }
                }
                else
                {
                    Console.WriteLine("<RAW MESSAGE> {0}", rawMessage);
                }
            }
        }
    }
}
