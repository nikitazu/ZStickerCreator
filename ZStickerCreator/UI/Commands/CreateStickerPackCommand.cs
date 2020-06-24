using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ZStickerCreator.Core.ImageProcessing;
using ZStickerCreator.UI.Controls;
using ZStickerCreator.UI.Framework;

namespace ZStickerCreator.UI.Commands
{
    internal class CreateStickerPackCommand : SimpleCommand, ICommand
    {
        private const string OutputDir = "output";

        private readonly MainWindowViewModel _main;

        public CreateStickerPackCommand(MainWindowViewModel main)
        {
            _main = main;
        }

        protected override void OnExecute(object param)
        {
            if (Directory.Exists(OutputDir))
            {
                Directory.Delete(OutputDir, true);
            }

            Directory.CreateDirectory(OutputDir);

            var ib = new ImageBuilder();

            for (int i = 0; i < _main.StickerItems.Count; i++)
            {
                var item = _main.StickerItems[i];

                var outName = item.Emoji;
                foreach (var c in Path.GetInvalidPathChars())
                {
                    outName = outName.Replace(c.ToString(), "");
                }
                foreach (var c in Path.GetInvalidFileNameChars())
                {
                    outName = outName.Replace(c.ToString(), "");
                }
                outName = outName.Replace(" ", "_");

                var path = Path.Combine(OutputDir, $"{i}_{outName}.png");
                var xxx = new PreviewImage();
                xxx.DataContext = new
                {
                    SelectedStickerItem = item,
                    SelectedTextFill = _main.SelectedTextFill,
                };

                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new Action(() =>
                    {
                        ib.BuildImageFile(new ImageBuilder.Settings
                        {
                            Surface = xxx.ImageCanvas,
                            OutputPath = path,
                            TransparentBackground = _main.TransparentBackground,
                        });
                    })
                );
            }
        }
    }
}
