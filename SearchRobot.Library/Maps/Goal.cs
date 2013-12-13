using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SearchRobot.Library.Global;

namespace SearchRobot.Library.Maps
{
	public class Goal : UniqueMandatoryMapElement
	{
		private Ellipse _uiElement;
	    private const int Radius = 5;

		public Goal(Map map) : base(map)
		{ }

		public Goal() { }

        protected override Geometry GeometryShape
        {
            get
            {
                return new EllipseGeometry(GeometryHelper.Convert(StartPosition), Radius, Radius);
            }
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
                                 Width = Radius * 2,
                                 Height = Radius * 2,
                                 Fill = new SolidColorBrush(Color.FromRgb(ItemColors.TargetColor.R, ItemColors.TargetColor.G, ItemColors.TargetColor.B))
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

        public override void Move(Canvas canvas, int offsetX, int offsetY)
		{
			StartPosition.X += offsetX;
			StartPosition.Y += offsetY;

			Canvas.SetLeft(_uiElement, Canvas.GetLeft(_uiElement) + offsetX);
			Canvas.SetTop(_uiElement, Canvas.GetTop(_uiElement) + offsetY);
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override object Clone()
		{
			return new Goal {StartPosition = StartPosition.Clone()};
		}
	}
}
