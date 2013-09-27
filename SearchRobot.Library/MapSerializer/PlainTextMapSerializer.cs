using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.MapSerializer
{
	public class PlainTextMapSerializer : IMapSerializer
	{
		/// <summary>
		/// Header of the Serialization Files used for Identification.
		/// </summary>
		public string SerializerHeader { get { return "PlainTextMapSerializer-V1.0"; } }

		/// <summary>
		/// Validates if the <see cref="Stream"/> is to be serialized with the given <see cref="IMapSerializer"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> containing the data.</param>
		/// <returns>Flag if can be validated.</returns>
		public bool Validate(Stream stream)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Unserializes the <see cref="Stream"/> into a <see cref="Map"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> containing the data.</param>
		/// <returns>Deserialized Map.</returns>
		public Map Unserialize(Stream stream)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Serializes a <see cref="Map"/> to a <see cref="Stream"/>.
		/// </summary>
		/// <param name="map"><see cref="Map"/> to be serialized.</param>
		/// <param name="stream"><see cref="Stream"/> to be used to store the serialized Data.</param>
		/// <exception cref="InvalidDataException">Thrown if not a valid Data file.</exception>
		public void Serialize(Map map, Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
