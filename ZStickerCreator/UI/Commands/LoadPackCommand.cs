using System.IO;
using System.Linq;
using System.Windows.Input;
using ZStickerCreator.Core.Persist;
using ZStickerCreator.UI.Framework;

namespace ZStickerCreator.UI.Commands
{
    internal class LoadPackCommand : SimpleCommand, ICommand
    {
        private const string InputPath = "pack.xml";

        private readonly XmlDataPersister _persister;

        public LoadPackCommand(XmlDataPersister persister)
        {
            _persister = persister;
        }

        protected override void OnExecute(object param)
        {
            var main = (MainWindowViewModel)param;
            using (var file = File.OpenRead(InputPath))
            {
                main.StickerItems = _persister.Load(file).Select(StickerFromData).ToList();
                main.SelectedStickerItem = main.StickerItems.FirstOrDefault();
            }
        }

        private Main.StickerItemViewModel StickerFromData(XmlDataPersister.StickerData data) =>
            new Main.StickerItemViewModel
            {
                Emoji = data.Emoji,
                Title = data.Title,
            };
    }
}
