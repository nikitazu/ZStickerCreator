using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ZStickerCreator.UI.Commands;
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

        private Canvas _previewImage;
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

                CreateImageCommand.TransparentBackground = value;
            }
        }

        // Commands
        //

        public CreateImageCommand CreateImageCommand { get; } = new CreateImageCommand
        {
            OutputPath = "out.png"
        };

        public ICommand OpenImageDirectoryCommand { get; }

        // UI
        //

        public Canvas PreviewImage
        {
            get => _previewImage;
            set
            {
                _previewImage = value;
                CreateImageCommand.Surface = value;
            }
        }

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

            OpenImageDirectoryCommand = new RelayCommand(_ => RunOpenImageDirectory());
        }

        private void RunOpenImageDirectory()
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }
    }
}
