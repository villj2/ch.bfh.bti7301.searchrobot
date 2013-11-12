using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchRobot.Library.Maps;

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

        public Point TopLeftCoordinate { get; private set; }

        public Point TopRightCoordinate { get; private set; }

        public Point BottomLeftCoordinate { get; private set; }

        public Point BottomRightCoordinate { get; private set; }

        public TDataType this[int x, int y]
        {
            get { return _data[x - XOffset, y - YOffset]; }
            set { _data[x - XOffset, y - YOffset] = value; }
        }

        public NegativeArray(int width, int height, int xOffset, int yOffset)
        {
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;

            BottomLeftCoordinate = new Point(xOffset, yOffset);
            TopRightCoordinate = new Point(xOffset + width - 1, yOffset + height - 1);
            TopLeftCoordinate = new Point(xOffset, yOffset + height - 1);
            BottomRightCoordinate = new Point(xOffset + width - 1, yOffset);

            _data = new TDataType[width, height];
        }
    }
}
