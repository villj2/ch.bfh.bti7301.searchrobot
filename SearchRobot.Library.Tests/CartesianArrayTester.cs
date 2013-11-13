using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SearchRobot.Library.Tests
{
    [TestClass]
    public class CartesianArrayTester
    {
        public static void FillArray(CartesianArray<int> array)
        {
            for (int x = array.XOffset; x < (array.XOffset + array.Width); x++)
            {
                for (int y = array.YOffset; y < (array.YOffset + array.Height); y++)
                {
                    array[x, y] = x*y;
                }
            }
        }

        [TestMethod]
        public void NegativeIndexTest()
        {
            // array -5 to 5
            CartesianArray<int> array = new CartesianArray<int>(11, 11, -5, -5);
            FillArray(array);

            Assert.AreEqual(array[-5, -5], 25);
            Assert.AreEqual(array[5, 5], 25);
            Assert.AreEqual(array[0, 5], 0);
            Assert.AreEqual(array[5, 0], 0);
            Assert.AreEqual(array[1, 5], 5);
            Assert.AreEqual(array[5, 1], 5);
            Assert.AreEqual(array[0, 0], 0);
            Assert.AreEqual(array[3, 2], 6);
        }

        [TestMethod]
        public void NegativeIndexUnevenShaped()
        {
            // array -5 to 5, -5 to 3
            CartesianArray<int> array = new CartesianArray<int>(11, 9, -5, -5);
            FillArray(array);

            Assert.AreEqual(array[-5, -5], 25);
            Assert.AreEqual(array[5, 3], 15);
            Assert.AreEqual(array[0, 0], 0);
            Assert.AreEqual(array[5, 1], 5);
            Assert.AreEqual(array[3, 3], 9);
            Assert.AreEqual(array[-5, 3], -15);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void NegativeIndexUnevenShapedException()
        {
            // array -5 to 5, -5 to 3
            CartesianArray<int> array = new CartesianArray<int>(11, 9, -5, -5);
            FillArray(array);

            Assert.AreEqual(array[15, 15], 0);
        }

        [TestMethod]
        public void CreateFromArray()
        {
            // [11] [21] [31]
            // [12] [22] [23]
            // [13] [23] [33]

            int[,] src = new int[3,3] { {11, 21, 31}, { 12 , 22, 23}, { 13, 23, 33} };

            var cartesianArray = CartesianArray<int>.FromArray(src);

            Assert.AreEqual(cartesianArray[0, 0], src[0, 2]);
            Assert.AreEqual(cartesianArray[2, 0], src[2, 2]);
            Assert.AreEqual(cartesianArray[1, 1], src[1, 1]);
            Assert.AreEqual(cartesianArray[0, 2], src[0, 0]);
        }

        [TestMethod]
        public void ToArray()
        {
            // [0] [2] [4]
            // [0] [1] [2]
            // [0] [0] [0]

            var cartesianArray = new CartesianArray<int>(3, 3, 0, 0);
            FillArray(cartesianArray);

            var array = cartesianArray.ToArray();

            Assert.AreEqual(array[0, 0], 0);
            Assert.AreEqual(array[1, 0], 2);
            Assert.AreEqual(array[2, 0], 4);

            Assert.AreEqual(array[1, 1], 1);
            Assert.AreEqual(array[2, 1], 2);
            Assert.AreEqual(array[2, 2], 0);

            Assert.AreEqual(array[0, 2], 0);
        }

        [TestMethod]
        public void CloneTestMatching()
        {
            // X : -10 -> +14 ; Y : 10 -> 34
            var cartesianArray = new CartesianArray<int>(25, 25, -10, 10);

            var clonedArray = cartesianArray.Clone();

            Assert.AreEqual(cartesianArray[-5, 15], clonedArray[-5, 15]);
            Assert.AreEqual(cartesianArray[5, 25], clonedArray[5, 25]);
            Assert.AreEqual(cartesianArray[10, 34], clonedArray[10, 34]);
            Assert.AreEqual(cartesianArray[0, 10], clonedArray[0, 10]);

            Assert.AreEqual(cartesianArray.BottomLeftCoordinate, clonedArray.BottomLeftCoordinate);
            Assert.AreEqual(cartesianArray.BottomRightCoordinate, clonedArray.BottomRightCoordinate);
            Assert.AreEqual(cartesianArray.TopLeftCoordinate, clonedArray.TopLeftCoordinate);
            Assert.AreEqual(cartesianArray.TopRightCoordinate, clonedArray.TopRightCoordinate);

            Assert.AreEqual(cartesianArray.YOffset, clonedArray.YOffset);
            Assert.AreEqual(cartesianArray.XOffset, clonedArray.XOffset);
            Assert.AreEqual(cartesianArray.Width, clonedArray.Width);
            Assert.AreEqual(cartesianArray.Height, clonedArray.Height);
        }

        [TestMethod]
        public void CloneTestUnlinked()
        {
            // X : -10 -> +14 ; Y : 10 -> 34
            var cartesianArray = new CartesianArray<int>(25, 25, -10, 10);

            var clonedArray = cartesianArray.Clone();

            cartesianArray[-5, 10] = int.MaxValue;
            clonedArray[5, 10] = int.MinValue;
            Assert.AreNotEqual(cartesianArray[-5, 10], clonedArray[-5, 10]);
            Assert.AreNotEqual(cartesianArray[5, 10], clonedArray[5, 10]);

            cartesianArray.YOffset = 0;
            clonedArray.XOffset = 0;
            Assert.AreNotEqual(cartesianArray.YOffset, clonedArray.YOffset);
            Assert.AreNotEqual(cartesianArray.XOffset, clonedArray.XOffset);
        }
    }
}
