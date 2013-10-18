using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SearchRobot.Library.Maps
{
	public class Goal : UniqueMandatoryMapElement
	{
		private Ellipse _uiElement;

		public Goal(Map map) : base(map)
		{
		
		}

		public Goal() { }

		protected override Geometry GeometryShape
		{
			get { return _uiElement.RenderedGeometry; }
		}

		public override void MouseDown(Canvas canvas, Point point)
		{
			StartPosition = point;
			ApplyTo(canvas);
		}

		public override void MouseUp(Canvas canvas, Point point)
		{
			if (IsUnique())
			{
				Map.Add(this);
			}
			else
			{
				canvas.Children.Remove(_uiElement);
			}
		}

		public override void MouseMove(Canvas canvas, Point point)
		{
		}

		public override void ApplyTo(Canvas canvas)
		{
			_uiElement = new Ellipse();
			_uiElement.Width = 10;
			_uiElement.Height = 10;
			_uiElement.Fill = Brushes.DarkRed;

			Canvas.SetLeft(_uiElement, StartPosition.X);
			Canvas.SetTop(_uiElement, StartPosition.Y);

			canvas.Children.Add(_uiElement);
		}

		public override void Remove(Canvas canvas)
		{
			Map.Remove(this);
			canvas.Children.Remove(_uiElement);
		}
	}
}
