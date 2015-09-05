namespace SexyFishHorse.WauwBot.View.UiElements
{
    using System;
    using System.Windows.Forms;

    public class ChatListViewItem : ListViewItem
    {
        public string Provider { get; set; }

        public string Username { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        public ChatListViewItem(string provider, string username, DateTime timestamp, string message)
            : base(provider)
        {
            Provider = provider;
            Username = username;
            Timestamp = timestamp;
            Message = message;

            SubItems.Add(username);
            SubItems.Add(timestamp.ToShortTimeString());
            SubItems.Add(message);
        }
    }
}
