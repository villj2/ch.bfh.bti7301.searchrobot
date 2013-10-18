using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace SearchRobot.Library.Maps
{
	[Serializable]
	public class Map
	{
		private readonly List<MapElement> _mapElements;
 
		public Map()
		{
			_mapElements = new List<MapElement>();
		}

		public List<MapElement> Elements { get { return _mapElements; } }
 
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
