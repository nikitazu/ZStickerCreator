using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZStickerCreator.UI.Framework;
using ZStickerCreator.UI.Main;

namespace ZStickerCreator
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static class PropArgs
        {
            public static readonly PropertyChangedEventArgs Text = new PropertyChangedEventArgs(nameof(Text));
            public static readonly PropertyChangedEventArgs TextFillList = new PropertyChangedEventArgs(nameof(TextFillList));
            public static readonly PropertyChangedEventArgs SelectedTextFill = new PropertyChangedEventArgs(nameof(SelectedTextFill));
            public static readonly PropertyChangedEventArgs TransparentBackground = new PropertyChangedEventArgs(nameof(TransparentBackground));
        }

        // Fields
        //

        private string _text;
        private List<BrushViewModel> _textFillList;
        private BrushViewModel _selectedTextFill;
        private bool _transparentBackground;

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

        public List<BrushViewModel> TextFillList
        {
            get => _textFillList;
            set
            {
                _textFillList = value;
                PropertyChanged?.Invoke(this, PropArgs.TextFillList);
            }
        }

        public BrushViewModel SelectedTextFill
        {
            get => _selectedTextFill;
            set
            {
                _selectedTextFill = value;
                PropertyChanged?.Invoke(this, PropArgs.SelectedTextFill);
            }
        }

        public bool TransparentBackground
        {
            get => _transparentBackground;
            set
            {
                _transparentBackground = value;
                PropertyChanged?.Invoke(this, PropArgs.TransparentBackground);
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
            Text = "Всё Ацтой!";
            TextFillList = new List<BrushViewModel>
            {
                new BrushViewModel
                {
                    BrushValue = Brushes.White,
                    Title = "White",
                },
                new BrushViewModel
                {
                    BrushValue = Brushes.Red,
                    Title = "Red",
                },
                new BrushViewModel
                {
                    BrushValue = Brushes.Orange,
                    Title = "Orange",
                },
                new BrushViewModel
                {
                    BrushValue = Brushes.Green,
                    Title = "Green",
                },
                new BrushViewModel
                {
                    BrushValue = Brushes.Olive,
                    Title = "Olive",
                },
                new BrushViewModel
                {
                    BrushValue = Brushes.Cyan,
                    Title = "Cyan",
                },
                new BrushViewModel
                {
                    BrushValue = Brushes.Blue,
                    Title = "Blue",
                },
            };
            SelectedTextFill = TextFillList.First();
            TransparentBackground = true;

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

            var surfaceBackground = surface.Background;
            if (TransparentBackground)
            {
                surface.Background = Brushes.Transparent;
            }

            // Get the size of canvas
            Size size = new Size(surface.Width, surface.Height);

            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            var renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32
            );
            renderBitmap.Render(surface);

            using (var file = File.Create(path))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(file);
            }

            if (TransparentBackground)
            {
                surface.Background = surfaceBackground;
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
