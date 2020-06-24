using System.Windows.Input;
using ZStickerCreator.Core.ImageProcessing;
using ZStickerCreator.UI.Framework;

namespace ZStickerCreator.UI.Commands
{
    internal class CreateImageCommand : SimpleCommand, ICommand
    {
        private const string OutputPath = "out.png";

        private readonly MainWindowViewModel _main;

        public CreateImageCommand(MainWindowViewModel main)
        {
            _main = main;
        }

        protected override void OnExecute(object param)
        {
            var ib = new ImageBuilder();
            ib.BuildImageFile(new ImageBuilder.Settings
            {
                Surface = _main.PreviewImage.ImageCanvas,
                OutputPath = OutputPath,
                TransparentBackground = _main.TransparentBackground,
            });
        }
    }
}
