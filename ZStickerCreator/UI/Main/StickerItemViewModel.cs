using System.ComponentModel;

namespace ZStickerCreator.UI.Main
{
    internal class StickerItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static class PropArgs
        {
            public static readonly PropertyChangedEventArgs Title = new PropertyChangedEventArgs(nameof(Title));
        }

        // Fields
        //

        private string _title;

        // Properties
        //

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, PropArgs.Title);
            }
        }

        public StickerItemViewModel()
        {
            // TODO: init
        }
    }
}
