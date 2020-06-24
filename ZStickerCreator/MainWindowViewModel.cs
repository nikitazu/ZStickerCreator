using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZStickerCreator.UI.Framework;

namespace ZStickerCreator
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static class PropArgs
        {
            public static readonly PropertyChangedEventArgs Text = new PropertyChangedEventArgs(nameof(Text));
        }

        // Fields
        //

        private string _text= "Всё Ацтой!";

        // Properties
        //

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                PropertyChanged?.Invoke(this, PropArgs.Text);
            }
        }

        // Commands
        //

        public ICommand CreateImageCommand { get; }
        public ICommand OpenImageDirectoryCommand { get; }

        // UI
        //

        public Canvas PreviewImage { get; set; }

        public MainWindowViewModel()
        {
            CreateImageCommand = new RelayCommand(_ => RunCreateImage());
            OpenImageDirectoryCommand = new RelayCommand(_ => RunOpenImageDirectory());
        }

        private void RunCreateImage()
        {
            var surface = PreviewImage;
            var path = "out.png";

            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(surface.Width, surface.Height);
            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            using (var file = File.Create(path))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(file);
            }

            // Restore previously saved layout
            surface.LayoutTransform = transform;
        }

        private void RunOpenImageDirectory()
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }
    }
}
