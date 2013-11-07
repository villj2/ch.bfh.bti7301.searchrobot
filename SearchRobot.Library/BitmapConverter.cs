using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using Size = System.Windows.Size;

namespace SearchRobot.Library
{
	public class BitmapConverter
	{
		private readonly RenderTargetBitmap _renderTargetBitmap;

		public BitmapConverter(Size size)
		{
			_renderTargetBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
		}

		public Bitmap ToBitmap(Canvas canvas)
		{
			_renderTargetBitmap.Render(canvas);

			var encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(_renderTargetBitmap));

			using (var stream = new MemoryStream())
			{
				encoder.Save(stream);

				return new Bitmap(stream);
			}
		}

        public static BitmapImage Convert(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static Image AsImage(BitmapImage bitmapImage)
        {
            Image image = new Image();
            image.Source = bitmapImage;

            return image;
        }

        public static Image AsImage(Bitmap bitmap)
        {
            return AsImage(Convert(bitmap));
        }
	}
}
