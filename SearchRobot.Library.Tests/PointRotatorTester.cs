using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library.Tests
{
    [TestClass]
    public class PointRotatorTester
    {
        [TestMethod]
        public void RotationTest()
        {
            NegativeArray<int> src = new NegativeArray<int>(11, 11, -5, -5);
            NegativeArrayTester.FillArray(src);

            PointRotator rotator = new PointRotator(90);

            var result = rotator.Rotate(src);

            Assert.AreEqual(result[-5, -5], -25);
            Assert.AreEqual(result[3, -3], 9);
            Assert.AreEqual(result[5, 5], -25);
            Assert.AreEqual(result[5, -5], 25);
        }

        [TestMethod]
        public void RotationTestUnshapped()
        {
            NegativeArray<int> src = new NegativeArray<int>(11, 7, -5, -3);
            NegativeArrayTester.FillArray(src);

            src[5, -3] = int.MaxValue;

            PointRotator rotator = new PointRotator(90);

            var result = rotator.Rotate(src);
            
            Assert.AreEqual(result.TopLeftCoordinate.X, -3);
            Assert.AreEqual(result.TopLeftCoordinate.Y, 5);

            Assert.AreEqual(result.BottomLeftCoordinate.X, -3);
            Assert.AreEqual(result.BottomLeftCoordinate.Y, -5);
                
            Assert.AreEqual(result.BottomRightCoordinate.X, 3);
            Assert.AreEqual(result.BottomRightCoordinate.Y, -5);
        }

        [TestMethod]
        public void RotationTest90Degree()
        {
            Point p = new Point(3, 2);

            PointRotator rotator = new PointRotator(90);

            var rotate = rotator.Rotate(p);

            Assert.AreEqual(rotate.X, -2);
            Assert.AreEqual(rotate.Y, 3);
        }

    }
}
