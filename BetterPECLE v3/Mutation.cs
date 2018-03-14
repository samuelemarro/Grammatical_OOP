using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammatical_OOP
{
    public abstract class Mutation
    {
        public abstract void Mutate(Chromosome original, double mutationProbability);
    }
}
