﻿using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using ZStickerCreator.UI.Framework;

namespace ZStickerCreator.UI.Commands
{
    internal class LoadPackCommand : SimpleCommand, ICommand
    {
        private const string InputPath = "pack.xml";

        private readonly MainWindowViewModel _main;

        public LoadPackCommand(MainWindowViewModel main)
        {
            _main = main;
        }

        protected override void OnExecute(object param)
        {
            var xdoc = XDocument.Load(InputPath);
            _main.StickerItems = xdoc.Root
                .Element("Stickers")
                .Elements("Sticker")
                .Select(StickerFromXml)
                .ToList();
            _main.SelectedStickerItem = _main.StickerItems.First();
        }

        private Main.StickerItemViewModel StickerFromXml(XElement xml) =>
            new Main.StickerItemViewModel
            {
                Emoji = xml.Element("Emoji").Value,
                Title = xml.Element("Title").Value
            };
    }
}