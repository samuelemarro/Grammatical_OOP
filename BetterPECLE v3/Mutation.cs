using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public abstract class Mutation
    {
        public abstract void Mutate(Chromosome original, double mutationProbability);
    }
}
