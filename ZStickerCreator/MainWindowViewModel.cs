using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZStickerCreator.UI.Commands;
using ZStickerCreator.UI.Controls;
using ZStickerCreator.UI.Framework;
using ZStickerCreator.UI.Main;

namespace ZStickerCreator
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static class PropArgs
        {
            public static readonly PropertyChangedEventArgs IsEnabled = new PropertyChangedEventArgs(nameof(IsEnabled));
            public static readonly PropertyChangedEventArgs StickerItems = new PropertyChangedEventArgs(nameof(StickerItems));
            public static readonly PropertyChangedEventArgs SelectedStickerItem = new PropertyChangedEventArgs(nameof(SelectedStickerItem));
            public static readonly PropertyChangedEventArgs TextFillList = new PropertyChangedEventArgs(nameof(TextFillList));
            public static readonly PropertyChangedEventArgs SelectedTextFill = new PropertyChangedEventArgs(nameof(SelectedTextFill));
            public static readonly PropertyChangedEventArgs TransparentBackground = new PropertyChangedEventArgs(nameof(TransparentBackground));
        }

        // Fields
        //

        private bool _isEnabled;
        private List<StickerItemViewModel> _stickerItems;
        private StickerItemViewModel _selectedStickerItem;
        private List<BrushViewModel> _textFillList;
        private BrushViewModel _selectedTextFill;
        private bool _transparentBackground;

        // Properties
        //

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                PropertyChanged?.Invoke(this, PropArgs.IsEnabled);
            }
        }

        public List<StickerItemViewModel> StickerItems
        {
            get => _stickerItems;
            set
            {
                _stickerItems = value;
                PropertyChanged?.Invoke(this, PropArgs.StickerItems);
            }
        }

        public StickerItemViewModel SelectedStickerItem
        {
            get => _selectedStickerItem;
            set
            {
                _selectedStickerItem = value;
                PropertyChanged?.Invoke(this, PropArgs.SelectedStickerItem);
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

        public ICommand AddStickerItemCommand { get; }
        public ICommand RemoveStickerItemCommand { get; }
        public CreateImageCommand CreateImageCommand { get; }
        public CreateStickerPackCommand CreateStickerPackCommand { get; }
        public ICommand OpenImageDirectoryCommand { get; }
        public SavePackCommand SavePackCommand { get; }
        public LoadPackCommand LoadPackCommand { get; }

        // UI
        //

        public PreviewImage PreviewImage { get; set; }

        public MainWindowViewModel(
            LoadPackCommand loadPackCommand,
            SavePackCommand savePackCommand,
            Func<List<StickerItemViewModel>> getExampleData
        )
        {
            IsEnabled = false;
            string[] emoji = new string[]
            {
                "grinning", "smiley", "smile", "grin", "satisfied", "laughing", "sweat_smile", "joy",
            };
            StickerItems = emoji
                .Select(emo => new StickerItemViewModel
                {
                    Emoji = emo,
                    Title = "Фпеши Свой Ацтой!"
                })
                .ToList();
            SelectedStickerItem = StickerItems.First();
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

            // All state MUST be initialized before this point
            // It's fine to leak `this` starting from here

            AddStickerItemCommand = new RelayCommand(_ => RunAddStickerItem());
            RemoveStickerItemCommand = new RelayCommand(_ => RunRemoveStickerItem());
            CreateImageCommand = new CreateImageCommand(this);
            CreateStickerPackCommand = new CreateStickerPackCommand(this);
            OpenImageDirectoryCommand = new RelayCommand(_ => RunOpenImageDirectory());
            SavePackCommand = savePackCommand;
            LoadPackCommand = loadPackCommand;

            // Async load state

            var currentDispatcher = Dispatcher.CurrentDispatcher;
            Task.Run(() =>
            {
                var data = getExampleData();
                currentDispatcher.Invoke(() =>
                {
                    StickerItems = data;
                    IsEnabled = true;
                });
            });
        }

        private void RunAddStickerItem()
        {
            StickerItems = StickerItems.Concat(new StickerItemViewModel[] {
                new StickerItemViewModel
                {
                    Emoji = $"sticker_{StickerItems.Count}",
                    Title = "Да Пошли Вы Все!",
                }
            }).ToList();
        }

        private void RunRemoveStickerItem()
        {
            StickerItems = StickerItems.Where(x => x != SelectedStickerItem).ToList();
        }

        private void RunOpenImageDirectory()
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }
    }
}
