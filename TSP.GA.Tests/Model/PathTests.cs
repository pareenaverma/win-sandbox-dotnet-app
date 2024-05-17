using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TSP.GA.Model;
using Xunit;

namespace TSP.GA.Tests.Model
{
    public class PathTests
    {
        [Fact]
        public void CalcDistanceTest()
        {
            // arrange  (Excel is your friend)
            List<Point> points = new List<Point>
            {
                new Point(57,16),
                new Point(96,68),
                new Point(12,58),
                new Point(12,9),
                new Point(43,15),
                new Point(82,79),
                new Point(11,28),
                new Point(58,51),
                new Point(73,52),
                new Point(61,86),
                new Point(92,28),
                new Point(73,47),
                new Point(2,60),
                new Point(73,64),
                new Point(59,56),
                new Point(87,98),
                new Point(39,90),
                new Point(55,60),
                new Point(46,43),
                new Point(53,24),
                new Point(59,8),
                new Point(77,11),
                new Point(0,29),
            };

            double expected = 1035.036432;

            // act
            Path p1 = new Path(points);

            // assert
            Assert.Equal(expected, p1.Length, 5);
        }

        [Fact]
        public void CrossOverTest()
        {
            // arrange
            List<Point> points1 = new List<Point>
            {
                new Point(1, 2),
                new Point(2, 3),
                new Point(3, 4),
                new Point(4, 5),
                new Point(5, 6),
                new Point(6, 7)
            };

            List<Point> points2 = new List<Point>
            {
                new Point(5, 6),
                new Point(4, 5),
                new Point(6, 7),
                new Point(3, 4),
                new Point(2, 3),
                new Point(1, 2),
            };


            Path p1 = new Path(points1); 
            Path p2 = new Path(points2);

            // act
            Path p3 = Path.CrossOver(p1, p2);
            Path p4 = Path.CrossOver(p2, p1);

            // assert
            Assert.NotEqual(p3, p4);
        }
    }
}
