using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library.Storage
{
	public class XmlMapStorageManager : IMapStorageManager
	{
		private static readonly Lazy<XmlSerializer> Serializer = 
			new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(Map)));

		public Map Load(string filename)
		{
			if (File.Exists(filename))
			{
				using (var filestream = File.OpenRead(filename))
				{
					using (var xmlReader = new XmlTextReader(filestream))
					{
						if (Serializer.Value.CanDeserialize(xmlReader))
						{
							return Serializer.Value.Deserialize(xmlReader) as Map;
						}
						else
						{
							throw new InvalidDataException("Data in the File is not valid.");
						}
					}
				}
			}
			else
			{
				throw new FileNotFoundException("File with Path \"{0}\" was not found.");
			}
		}

		public void Save(string filename, Map map)
		{
			using (var fileStream = File.OpenWrite(filename))
			{
				Serializer.Value.Serialize(fileStream, map);
			}
		}
	}
}
