using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SearchRobot.Library.Maps
{
	public class Goal : UniqueMandatoryMapElement
	{
		private Ellipse _uiElement;

		public Goal(Map map) : base(map)
		{ }

		public Goal() { }

		protected override Geometry GeometryShape
		{
			get { return _uiElement.RenderedGeometry; }
		}

        public override UIElement UiElement
	    {
            get { return _uiElement; }
	    }

	    public override void MouseDown(Canvas canvas, Point point)
		{
			StartPosition = point;
			ApplyTo(canvas);
		}

		public override void MouseMove(Canvas canvas, Point point)
		{ }

		public override void ApplyTo(Canvas canvas)
		{
			_uiElement = new Ellipse
			                 {
			                     Width = 10,
                                 Height = 10,
                                 Fill = Brushes.DarkRed
			                 };

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
