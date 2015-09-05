namespace SexyFishHorse.WauwBot.View.UiElements
{
    using System;
    using ListBox;
    using SexyFishHorse.WauwBot.Model;

    public class ChatLine : ParseMessageEventArgs
    {
        public ChatLine(ChatMessage message)
            : base(ParseMessageType.None, string.Format("{0} [{1}][{2}]", message.Timestamp.ToString("T"), message.Provider, message.Username), message.Message)
        {
            Provider = message.Provider;
            Username = message.Username;
            Timestamp = message.Timestamp;
            Message = message.Message;
        }

        public Provider Provider { get; set; }

        public string Username { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{2} [{0}][{1}]: {3}", Provider, Username, Timestamp.ToString("T"), Message);
        }
    }
}
