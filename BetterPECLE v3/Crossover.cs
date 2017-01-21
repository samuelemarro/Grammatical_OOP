using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public abstract class Crossover
    {
        public abstract Tuple<Chromosome,Chromosome> Cross(Chromosome parent1, Chromosome parent2, double crossoverProbability); 
    }
}
