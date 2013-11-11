using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
