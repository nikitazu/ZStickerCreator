using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZStickerCreator.Core.ImageProcessing
{
    public class ImageBuilder
    {
        public struct Settings
        {
            public Canvas Surface { get; set; }
            public string OutputPath { get; set; }
            public bool? TransparentBackground { get; set; }
        }

        public void BuildImageFile(Settings settings)
        {
            if (settings.Surface == null)
            {
                throw new ArgumentException($"{nameof(settings.Surface)} is null");
            }

            if (string.IsNullOrWhiteSpace(settings.OutputPath))
            {
                throw new ArgumentException($"{nameof(settings.OutputPath)} is empty");
            }

            if (settings.TransparentBackground == null)
            {
                throw new ArgumentException($"{nameof(settings.TransparentBackground)} is null");
            }

            // Save current canvas transform
            Transform transform = settings.Surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            settings.Surface.LayoutTransform = null;

            var surfaceBackground = settings.Surface.Background;
            if (settings.TransparentBackground.Value)
            {
                settings.Surface.Background = Brushes.Transparent;
            }

            // Get the size of canvas
            Size size = new Size(settings.Surface.Width, settings.Surface.Height);

            // Measure and arrange the surface
            // VERY IMPORTANT
            settings.Surface.Measure(size);
            settings.Surface.Arrange(new Rect(size));

            var renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32
            );
            renderBitmap.Render(settings.Surface);

            using (var file = File.Create(settings.OutputPath))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(file);
            }

            if (settings.TransparentBackground.Value)
            {
                settings.Surface.Background = surfaceBackground;
            }

            // Restore previously saved layout
            settings.Surface.LayoutTransform = transform;
        }
    }
}
