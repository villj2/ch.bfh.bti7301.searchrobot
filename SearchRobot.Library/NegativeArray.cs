using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library
{
    /// <summary>
    /// 2D Array which supports negative index.
    /// </summary>
    /// <typeparam name="TDataType">Datatype of the array elements.</typeparam>
    public class NegativeArray<TDataType>
    {
        private readonly TDataType[,] _data;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int XOffset { get; private set; }

        public int YOffset { get; private set; }

        public TDataType this[int x, int y]
        {
            get { return _data[x + XOffset, y + YOffset]; }
            set { _data[x + XOffset, y + YOffset] = value; }
        }

        public NegativeArray(int width, int height, int xOffset, int yOffset)
        {
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;
            _data = new TDataType[width, height];
        }
    }
}
