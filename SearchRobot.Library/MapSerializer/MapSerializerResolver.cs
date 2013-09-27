using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.MapSerializer
{
	public static class MapSerializerResolver
	{
		private static readonly Lazy<List<IMapSerializer>> _serializer;
		
		static MapSerializerResolver()
		{
			_serializer = new Lazy<List<IMapSerializer>>(LoadSerializer);
		}

		private static List<IMapSerializer> LoadSerializer()
		{
			Type iFormulatedSearchType = typeof(IMapSerializer);
			List<IMapSerializer> types = new List<IMapSerializer>();

			typeof(MapSerializerResolver).Assembly.GetTypes()
				.Where(type => iFormulatedSearchType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).ToList()
				.ForEach(t => types.Add((IMapSerializer)Activator.CreateInstance(t)));

			return types;
		}

		public static List<IMapSerializer> Serializer
		{
			get
			{
				return _serializer.Value;
			}
		}

		public static IMapSerializer DefaultSerializer
		{
			get
			{
				return new PlainTextMapSerializer();
			}
		}
	}
}
