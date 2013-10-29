using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchRobot.Library.Maps
{
	public class MoveTool : MapTool
	{
		private MapElement _mapElement;

		private MapElement _copy;

		private Point _startPoint;

		public MoveTool(Map map) : base(map)
		{ }

		public override void MouseDown(Canvas canvas, Point point)
		{
			_mapElement = GetElementAtPoint(canvas, point);

			if (_mapElement != null)
			{
				_startPoint = point.Clone();
				_copy = _mapElement.Clone() as MapElement;
			}
		}

		public override void MouseUp(Canvas canvas, Point point)
		{
			if (_mapElement != null && _mapElement.IsValid())
			{
				_mapElement.Remove(canvas);
				_copy.ApplyTo(canvas);
			}
		}

		public override void MouseMove(Canvas canvas, Point point)
		{
			if (_mapElement != null)
			{
				_mapElement.Move(canvas, point.X - _startPoint.X, point.Y - _startPoint.Y);
				_startPoint.X = point.X;
				_startPoint.Y = point.Y;
			}
		}

		public override void MouseLeave(Canvas canvas)
		{
			if (_mapElement != null)
			{
				_mapElement.Remove(canvas);
				_copy.ApplyTo(canvas);
			}
		}
	}
}
