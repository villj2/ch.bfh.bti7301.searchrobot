using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library.Storage
{
	public interface IMapStorageManager
	{
		Map Load(string filename);

		void Save(string filename, Map map);
	}
}
