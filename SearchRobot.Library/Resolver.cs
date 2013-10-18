using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchRobot.Library.Storage;

namespace SearchRobot.Library
{
	public static class Resolver
	{
		public static IMapStorageManager StorageManager{
			get
			{
				return new XmlMapStorageManager();
			}
		}
	}
}
