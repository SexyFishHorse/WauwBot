namespace SexyFishHorse.WauwBot.View.UiElements.MessageListBox
{
    using System.Collections;
    using ListBox;

    public delegate void AddedEventHandler();

    public delegate void InsertEventHandler(int index);

    public class ListBoxList
    {
        private readonly ArrayList messages;

        private readonly ArrayList messagesInfo;

        public ListBoxList()
        {
            messages = new ArrayList();
            messagesInfo = new ArrayList();
        }

        public event AddedEventHandler ItemAdded;

        public event InsertEventHandler ItemInserted;

        public int Count
        {
            get { return messages.Count; }
        }

        public ParseMessageEventArgs this[int index]
        {
            get { return (ParseMessageEventArgs)messages[index]; }
        }

        public int Add(ParseMessageEventArgs pmea)
        {
            var index = messages.Add(pmea);
            messagesInfo.Add(new ItemInfo(pmea));
            OnAdd();
            return index;
        }

        public void Clear()
        {
            messages.Clear();
            messagesInfo.Clear();
        }

        public int IndexOf(object obj)
        {
            return messages.IndexOf(obj);
        }

        public int IndexOf(ParseMessageEventArgs pmea)
        {
            return messages.IndexOf(pmea);
        }

        public ItemInfo Info(int index)
        {
            return (ItemInfo)messagesInfo[index];
        }

        public void Insert(int index, ParseMessageEventArgs pmea)
        {
            messages.Insert(index, pmea);
            messagesInfo.Insert(index, new ItemInfo(pmea));
            OnInsert(index);
        }

        private void OnAdd()
        {
            if (ItemAdded != null)
            {
                ItemAdded();
            }
        }

        private void OnInsert(int index)
        {
            if (ItemInserted != null)
            {
                ItemInserted(index);
            }
        }
    }
}
