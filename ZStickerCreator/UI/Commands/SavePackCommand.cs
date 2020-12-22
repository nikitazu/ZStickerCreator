using System.IO;
using System.Linq;
using System.Windows.Input;
using ZStickerCreator.Core.Persist;
using ZStickerCreator.UI.Framework;
using ZStickerCreator.UI.Main;

namespace ZStickerCreator.UI.Commands
{
    internal class SavePackCommand : SimpleCommand, ICommand
    {
        private const string OutputPath = "pack.xml";

        private readonly XmlDataPersister _persister;

        public SavePackCommand(XmlDataPersister persister)
        {
            _persister = persister;
        }

        protected override void OnExecute(object param)
        {
            var main = (MainWindowViewModel)param;
            using (var file = File.Create(OutputPath))
            {
                _persister.Save(main.StickerItems.Select(StickerToXml), file);
            }
        }

        private XmlDataPersister.StickerData StickerToXml(StickerItemViewModel sticker) =>
            new XmlDataPersister.StickerData()
            {
                Emoji = sticker.Emoji,
                Title = sticker.Title,
                MemePictureUrl = sticker.MemePictureUrl,
            };
    }
}
