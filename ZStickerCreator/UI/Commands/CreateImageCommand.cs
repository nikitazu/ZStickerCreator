using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZStickerCreator.UI.Framework;

namespace ZStickerCreator.UI.Commands
{
    internal class CreateImageCommand : SimpleCommand, ICommand
    {
        public Canvas Surface { get; set; }
        public string OutputPath { get; set; }
        public bool? TransparentBackground { get; set; }

        protected override void OnExecute(object param)
        {
            if (Surface == null)
            {
                throw new InvalidOperationException($"{nameof(Surface)} is null");
            }

            if (string.IsNullOrWhiteSpace(OutputPath))
            {
                throw new InvalidOperationException($"{nameof(OutputPath)} is empty");
            }

            if (TransparentBackground == null)
            {
                throw new InvalidOperationException($"{nameof(TransparentBackground)} is null");
            }

            // Save current canvas transform
            Transform transform = Surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            Surface.LayoutTransform = null;

            var surfaceBackground = Surface.Background;
            if (TransparentBackground.Value)
            {
                Surface.Background = Brushes.Transparent;
            }

            // Get the size of canvas
            Size size = new Size(Surface.Width, Surface.Height);

            // Measure and arrange the surface
            // VERY IMPORTANT
            Surface.Measure(size);
            Surface.Arrange(new Rect(size));

            var renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32
            );
            renderBitmap.Render(Surface);

            using (var file = File.Create(OutputPath))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(file);
            }

            if (TransparentBackground.Value)
            {
                Surface.Background = surfaceBackground;
            }

            // Restore previously saved layout
            Surface.LayoutTransform = transform;
        }
    }
}
