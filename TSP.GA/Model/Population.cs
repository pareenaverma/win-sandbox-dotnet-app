using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TSP.GA.Model
{
    public class Population
    {
        private readonly double _mutationRate = 0.01;
        private readonly Random _random;
        private List<Path> _paths;

        public Path Best
        {
            get
            {
                return _paths.First();
            }
        }

        public Population(List<Point> path, int populationsize, int randomSeed)
        {
            _random = new Random(randomSeed);
            _paths = CreatePopulation(path, populationsize).OrderBy(x => x.Length).ToList();
        }

        private List<Path> CreatePopulation(List<Point> path, int populationsize)
        {
            List<Path> pop = new List<Path>(populationsize);

            for (int i = 0; i < populationsize; i++)
            {
                Path p = new Path(new List<Point>(path));
                pop.Add(p);
                path = NextVariation(path);
            }

            return pop;
        }

        private List<Point> NextVariation(List<Point> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Mutate(path);
            }

            return path;
        }

        private void Mutate(List<Point> path)
        {
            int c = path.Count;
            int fst = _random.Next(c - 1);
            int snd = _random.Next(c - fst) + fst;

            Point p = path[fst];
            path[fst] = path[snd];
            path[snd] = p;
        }

        public void Next()
        {
            // Prepare selection
            List<double> fitness = _paths.Select(p => 1.0 / p.Length).ToList();
            double total = fitness.Sum();

            List<Path> newPopulation = new List<Path>(_paths.Count);
            newPopulation.Add(_paths.First());     // allways keep the current best

            for (int i = 0; i < _paths.Count / 2; i++)
            {
                Path p1 = Select(fitness, total);
                Path p2 = Select(fitness, total);
                Path p = Path.CrossOver(p1, p2);
                if (_random.NextDouble() < _mutationRate)
                    Mutate(p.Points);       // side effect
                newPopulation.Add(p);
                Path q = Path.CrossOver(p2, p1);
                if (_random.NextDouble() < _mutationRate)
                    Mutate(q.Points);       // side effect
                newPopulation.Add(q);
            }

            _paths = newPopulation.OrderBy(x => x.Length).ToList();
        }

        private Path Select(List<double> fitness, double total)
        {
            double limit = _random.NextDouble() * total;
            double runningTotal = 0.0;

            int ndx = 0;
            foreach (double f in fitness)
            {
                runningTotal += f;
                if (runningTotal > limit)
                    break;

                ndx++;
            }

            return _paths[ndx];
        }
    }
}
