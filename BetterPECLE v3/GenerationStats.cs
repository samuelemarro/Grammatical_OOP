using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    [Serializable]
    public class GenerationStats
    {
        public Chromosome bestChromosome;

        public List<double> fitnessValues;

        public double executedEvaluations;

        public double passedErrorChecks;
        public double failedErrorChecks;
        public double successfulErrorCorrections;
        public double failedErrorCorrections;

        public double generationErrors;
        public double compilationErrors;
        public double executionExceptions;

        public double MaxFitness
        {
            get
            {
                return fitnessValues.Max();
            }
        }

        public double MinFitness
        {
            get
            {
                return fitnessValues.Min();
            }
        }

        public double AverageFitness
        {
            get
            {
                return fitnessValues.Average();
            }
        }
    }
}
