namespace SexyFishHorse.WauwBot.Model
{
    using System;

    public class ChatMessage
    {
        public string Provider { get; set; }

        public string Username { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }
    }
}
