using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammatical_OOP
{
    [Serializable]
    public class GeneticAlgorithmResult
    {
        public List<GenerationStats> stats;

        public Chromosome BestChromosome
        {
            get
            {
                return stats.Select(x => x.bestChromosome).OrderByDescending(x => x.Fitness).First();
            }
        }

        public double MaxFitness
        {
            get
            {
                return stats.SelectMany(x => x.fitnessValues).Max();
            }
        }

        public double MinFitness
        {
            get
            {
                return stats.SelectMany(x => x.fitnessValues).Min();
            }
        }

        public double AverageFitness
        {
            get
            {
                return stats.SelectMany(x => x.fitnessValues).Average();
            }
        }

        public double FitnessStandardDeviation
        {
            get
            {
                List<double> totalFitnessValues = stats.SelectMany(x => x.fitnessValues).ToList();

                double average = totalFitnessValues.Average();
                double varianceSum = 0;

                for (int i = 0; i < totalFitnessValues.Count; i++)
                {
                    varianceSum += (totalFitnessValues[i] - average) * (totalFitnessValues[i] - average);
                }
                return Math.Sqrt(varianceSum / totalFitnessValues.Count);
            }
        }
    }
}
