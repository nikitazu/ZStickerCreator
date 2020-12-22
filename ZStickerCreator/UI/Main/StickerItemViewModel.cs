using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace ZStickerCreator.UI.Main
{
    internal class StickerItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static class PropArgs
        {
            public static readonly PropertyChangedEventArgs Emoji = new PropertyChangedEventArgs(nameof(Emoji));
            public static readonly PropertyChangedEventArgs Title = new PropertyChangedEventArgs(nameof(Title));
            public static readonly PropertyChangedEventArgs MemePictureUrl = new PropertyChangedEventArgs(nameof(MemePictureUrl));
            public static readonly PropertyChangedEventArgs MemePicture = new PropertyChangedEventArgs(nameof(MemePicture));
        }

        // Fields
        //

        private string _emoji;
        private string _title;
        private string _memePictureUrl;
        private BitmapImage _memePicture;


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

        public string MemePictureUrl
        {
            get => _memePictureUrl;
            set
            {
                _memePictureUrl = value;
                PropertyChanged?.Invoke(this, PropArgs.MemePictureUrl);
                OnMemePictureUrlUpdated();
            }
        }

        public BitmapImage MemePicture
        {
            get => _memePicture;
            set
            {
                _memePicture = value;
                PropertyChanged?.Invoke(this, PropArgs.MemePicture);
            }
        }

        private void OnMemePictureUrlUpdated()
        {
            if (Uri.TryCreate(MemePictureUrl, UriKind.Absolute, out var memePictureUri))
            {
                MemePicture = new BitmapImage(memePictureUri);
            }
            else
            {
                MemePicture = null;
            }
        }
    }
}
