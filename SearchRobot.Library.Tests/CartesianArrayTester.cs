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
    }
}
