﻿using System;
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
            get { return _data[x - XOffset, y - YOffset]; }
            set { _data[x - XOffset, y - YOffset] = value; }
        }

        public CartesianArray(int width, int height, int xOffset, int yOffset)
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

        public TDataType[,] ToArray()
        {
            int height = _data.GetUpperBound(1);
            TDataType[,] result = new TDataType[_data.GetUpperBound(0) + 1, _data.GetUpperBound(1) + 1];

            for (var x = 0; x <= _data.GetUpperBound(0); x++)
            {
                for (var y = 0; y <= _data.GetUpperBound(1); y++)
                {
                    result[x, height - y] = _data[x, y];
                }
            }

            return result;
        }

        public static CartesianArray<TDataType> FromArray(TDataType[,] array)
        {
            int height = array.GetUpperBound(1);
            var result = new CartesianArray<TDataType>(array.GetUpperBound(0) + 1, height + 1, 0, 0);
           
            for (var x = 0; x <= array.GetUpperBound(0); x++)
            {
                for (var y = 0; y <= array.GetUpperBound(1); y++)
                {
                    result[x, height - y] = array[x, y];
                }
            }

            return result;
        }
    }
}
