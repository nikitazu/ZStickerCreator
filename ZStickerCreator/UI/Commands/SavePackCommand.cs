using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using ZStickerCreator.UI.Framework;

namespace ZStickerCreator.UI.Commands
{
    internal class SavePackCommand : SimpleCommand, ICommand
    {
        private const string OutputPath = "pack.xml";

        private readonly MainWindowViewModel _main;

        public SavePackCommand(MainWindowViewModel main)
        {
            _main = main;
        }

        protected override void OnExecute(object param)
        {
            var xdoc = new XDocument(
                new XElement(
                    "Pack",
                    new XElement(
                        "Stickers",
                        _main.StickerItems.Select(sticker => new XElement("Sticker", sticker.Title))
                    )
                )
            );
            xdoc.Save(OutputPath);
        }
    }
}
