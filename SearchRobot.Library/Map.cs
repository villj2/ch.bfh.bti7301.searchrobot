using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library
{
    /// <summary>
    /// Represents the underlying Map.
    /// </summary>
    public class Map
    {
        private readonly int[][] _map;


        /// <summary>
        /// Loads a Map from a stream.
        /// </summary>
        /// <param name="stream">Stream as source of the map.</param>
        /// <returns>Loaded Map.</returns>
        public static Map Load(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                reader.ReadLine();
            }
        }

        /// <summary>
        /// Writes the map structure to a stream.
        /// </summary>
        /// <param name="stream">Stream to be used to save the Map.</param>
        public void Save(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                foreach (int[] column in _map)
                {
                    foreach (int val in column)
                    {
                        writer.Write(val);
                    }

                    writer.WriteLine();
                }
            }
        }

    }
}
