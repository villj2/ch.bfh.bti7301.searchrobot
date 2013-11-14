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
    public class CartesianArray<TDataType>
    {
        private readonly TDataType[,] _data;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int XOffset { get; set; }

        public int YOffset { get; set; }

        public Point TopLeftCoordinate { get; private set; }

        public Point TopRightCoordinate { get; private set; }

        public Point BottomLeftCoordinate { get; private set; }

        public Point BottomRightCoordinate { get; private set; }

        public TDataType this[int x, int y]
        {
            get { return _data[x - XOffset, (Height - 1 -(y - YOffset))]; }
            set { _data[x - XOffset, (Height - 1 - (y - YOffset))] = value; }
        }

        public TDataType this[Point point]
        {
            get { return this[point.X, point.Y]; }
            set { this[point.X, point.Y] = value; }
        }

        public CartesianArray(int width, int height, int xOffset, int yOffset)
        {
            InitBoundaries(width, height, xOffset, yOffset);
            _data = new TDataType[width, height];
        }

        private CartesianArray(int width, int height, int xOffset, int yOffset, TDataType[,] data)
        {
            InitBoundaries(width, height, xOffset, yOffset);
            _data = data.Clone() as TDataType[,];
        }

        private void InitBoundaries(int width, int height, int xOffset, int yOffset)
        {
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;

            BottomLeftCoordinate = new Point(xOffset, yOffset);
            TopRightCoordinate = new Point(xOffset + width - 1, yOffset + height - 1);
            TopLeftCoordinate = new Point(xOffset, yOffset + height - 1);
            BottomRightCoordinate = new Point(xOffset + width - 1, yOffset);
        }

        public TDataType[,] ToArray()
        {
            return _data.Clone() as TDataType[,];
        }

        public static CartesianArray<TDataType> FromArray(TDataType[,] array)
        {
            return new CartesianArray<TDataType>(array.GetUpperBound(0) + 1, array.GetUpperBound(1) + 1, 0, 0, array);
        }

        public CartesianArray<TDataType> Clone()
        {
            return new CartesianArray<TDataType>(Width, Height, XOffset, YOffset, _data);
        }
    }
}
