
﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TSP.GA.Model
{
    public class Path
    {
        private readonly double _length;
        private readonly List<Point> _points;

        public Path(List<Point> points)
        {
            _points = points;
            _length = CalcLength();
        }

        public double Length => _length;

        public List<Point> Points => _points;

        private double CalcLength()
        {
            double len = 0.0;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                double distance = CalcDistance(_points[i], _points[i + 1]);
                len += distance;
            }

            return len;
        }

        private double CalcDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Sqr(p1.X - p2.X) + Sqr(p1.Y - p2.Y));

            double Sqr(double x) => x * x;
        }

        internal static Path CrossOver(Path p1, Path p2)
        {
            int crossoverPoint = p1.Points.Count() / 2;     // can also be randomized

            List<Point> pts = new List<Point>(p1.Points.Take(crossoverPoint));
            foreach (Point p in p2.Points.Skip(crossoverPoint))
            {
                if (!pts.Contains(p))
                    pts.Add(p);
                else
                {
                    foreach (Point q in p2.Points)
                    {
                        if (!pts.Contains(q))
                        {
                            pts.Add(q);
                            break;
                        }
                    }
                }
            }

            return new Path(pts);
        }

        public override string ToString()
        {
            string s = string.Join(",", _points.Select(p => $"({p.X},{p.Y})"));
            return $"Length: {_length}[{s}]";
        }
    }
}
