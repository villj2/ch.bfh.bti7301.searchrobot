using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchRobot.Library.Global;
using SearchRobot.Library.Maps;
using SearchRobot.Library.Simulation.EdgeDetection;

namespace SearchRobot.Library.Tests
{
	[TestClass]
	public class EdgeDetectionTester
	{
		private const MapElementStatus B = MapElementStatus.Blocked;
		private const MapElementStatus D = MapElementStatus.Discovered;
		private const MapElementStatus U = MapElementStatus.Undiscovered;

		private MapElementStatus[,] _map;

		[TestInitialize]
		public void Setup()
		{
			// ever "row" here is a "col" on map

			_map = new [,]
				{
					{ U, U, D, D, D, D, D, D, U}, // x:0
					{ U, U, U, D, D, D, D, U, U},
					{ B, B, B, D, D, D, U, U, U},
					{ U, U, U, D, D, D, U, U, U},
					{ U, U, U, D, D, D, U, U, U}
				};
		}

		[TestMethod]
		public void TestFindEdgePoints()
		{
			var edgeDetection = new EdgeDetectionAlgorithm();
			var points = edgeDetection.GetEdgePoints(_map);

			Assert.IsTrue(points.Contains(new Point { X = 0, Y = 2 }));
			Assert.IsTrue(points.Contains(new Point { X = 1, Y = 3 }));

			Assert.IsTrue(points.Contains(new Point { X = 3, Y = 3 }));
			Assert.IsTrue(points.Contains(new Point { X = 4, Y = 3 }));

			Assert.IsTrue(points.Contains(new Point { X = 0, Y = 7 }));
			Assert.IsTrue(points.Contains(new Point { X = 1, Y = 6 }));
			Assert.IsTrue(points.Contains(new Point { X = 2, Y = 5 }));
			Assert.IsTrue(points.Contains(new Point { X = 3, Y = 5 }));
			Assert.IsTrue(points.Contains(new Point { X = 4, Y = 5 }));
		}

		[TestMethod]
		public void TestFindEdges()
		{
			var edgeDetection = new EdgeDetectionAlgorithm();
			var points = edgeDetection.GetEdgePoints(_map);

			var edges = edgeDetection.GroupToEdges(points);

			Assert.IsTrue(edges.Count() == 3);
		}
	}
}
