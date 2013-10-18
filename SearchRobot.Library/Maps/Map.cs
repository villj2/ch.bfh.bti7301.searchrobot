using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchRobot.Library.Maps
{
	public class Map
	{
		private readonly List<MapElement> _mapElements;
 
		public Map()
		{
			_mapElements = new List<MapElement>();
		}

		public IReadOnlyCollection<MapElement> Elements { get { return _mapElements.AsReadOnly(); } }
 
		public void Add(MapElement mapElement)
		{
			_mapElements.Add(mapElement);
		}

		public void Remove(MapElement mapElement)
		{
			_mapElements.Remove(mapElement);
		}

		public void ApplyToCanvas(Canvas canvas)
		{
			foreach (var mapelement in Elements)
			{
				mapelement.ApplyTo(canvas);
			}
		}
	}
}
