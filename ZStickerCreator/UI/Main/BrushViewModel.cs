using System.ComponentModel;
using System.Windows.Media;

namespace ZStickerCreator.UI.Main
{
    internal class BrushViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static class PropArgs
        {
            public static readonly PropertyChangedEventArgs BrushValue = new PropertyChangedEventArgs(nameof(BrushValue));
            public static readonly PropertyChangedEventArgs Title = new PropertyChangedEventArgs(nameof(Title));
        }

        // Fields
        //

        private Brush _brushValue;
        private string _title;

        // Properties
        //

        public Brush BrushValue
        {
            get => _brushValue;
            set
            {
                _brushValue = value;
                PropertyChanged?.Invoke(this, PropArgs.BrushValue);
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
