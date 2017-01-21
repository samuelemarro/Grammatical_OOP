using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public class Population : List<Generation>
    {
        public Population(int populationSize, int chromosomeSize, int maximumChromosomeSize)
        {
            Generation firstGeneration = new Generation();
            for (int i = 0; i < populationSize; i++)
            {
                firstGeneration.Add(new Chromosome(chromosomeSize, maximumChromosomeSize));
            }
            base.Add(firstGeneration);
        }

        public Generation CurrentGeneration
        {
            get
            {
                return base[base.Count - 1];
            }
        }

        internal Population() : base() { }
    }
}
