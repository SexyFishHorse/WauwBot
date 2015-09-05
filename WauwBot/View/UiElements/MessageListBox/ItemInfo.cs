namespace SexyFishHorse.WauwBot.View.UiElements.MessageListBox
{
    using ListBox;

    public class ItemInfo
    {
        private readonly ParseMessageEventArgs eventArgsMessage;

        private int height;

        public ItemInfo(ParseMessageEventArgs eventArgs)
        {
            height = 0;
            HeightValid = false;
            eventArgsMessage = eventArgs;
        }

        public ItemInfo(int height, bool heightValid, ParseMessageEventArgs eventArgs)
        {
            this.height = height;
            HeightValid = heightValid;
            eventArgsMessage = eventArgs;
        }

        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                HeightValid = true;
            }
        }

        public bool HeightValid { get; set; }

        public ParseMessageEventArgs Message
        {
            get { return eventArgsMessage; }
        }
    }
}
