using System.ComponentModel;

namespace ZStickerCreator.UI.Main
{
    internal class StickerItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static class PropArgs
        {
            public static readonly PropertyChangedEventArgs Emoji = new PropertyChangedEventArgs(nameof(Emoji));
            public static readonly PropertyChangedEventArgs Title = new PropertyChangedEventArgs(nameof(Title));
        }

        // Fields
        //

        private string _emoji;
        private string _title;


        // Properties
        //

        public string Emoji
        {
            get => _emoji;
            set
            {
                _emoji = value;
                PropertyChanged?.Invoke(this, PropArgs.Emoji);
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, PropArgs.Title);
            }
        }
    }
}
