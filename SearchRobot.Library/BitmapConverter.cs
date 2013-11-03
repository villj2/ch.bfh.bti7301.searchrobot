using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
	}
}
