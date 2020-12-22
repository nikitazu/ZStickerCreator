using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ZStickerCreator.Core.Persist
{
    public class XmlDataPersister
    {
        private readonly static XmlWriterSettings _settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
        };

        public class StickerData
        {
            public string Emoji { get; set; }
            public string Title { get; set; }
            public string MemePictureUrl { get; set; }
        }

        public void Save(IEnumerable<StickerData> items, Stream output)
        {
            var xdoc = new XDocument(
                new XElement(
                    "Pack",
                    new XElement(
                        "Stickers",
                        items.Select(StickerToXml)
                    )
                )
            );
            using (var xml = XmlWriter.Create(output, _settings))
            {
                xdoc.Save(xml);
            }
        }

        public List<StickerData> Load(Stream input)
        {
            var xdoc = XDocument.Load(input);
            return xdoc.Root
                .Element("Stickers")
                .Elements("Sticker")
                .Select(StickerFromXml)
                .ToList();
        }

        private XElement StickerToXml(StickerData sticker) =>
            new XElement(
                "Sticker",
                new XElement("Emoji", sticker.Emoji),
                new XElement("Title", sticker.Title),
                new XElement("MemePictureUrl", sticker.MemePictureUrl)
            );

        private StickerData StickerFromXml(XElement xml) =>
            new StickerData
            {
                Emoji = xml.Element("Emoji").Value,
                Title = xml.Element("Title").Value,
                MemePictureUrl = xml.Element("MemePictureUrl")?.Value ?? string.Empty
            };
    }
}
