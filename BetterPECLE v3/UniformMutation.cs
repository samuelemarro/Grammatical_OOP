using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public class UniformMutation : Mutation
    {
        public override void Mutate(Chromosome original, double mutationProbability)
        {
            for(int i = 0; i < original.Count; i++)
            {
                if(GrammaticalEvolution.random.NextDouble() < mutationProbability)
                {
                    original[i] = original.NewCodon();
                    original.Fitness = null;
                    original.LastUsedCodonPosition = -1;
                }
            }
        }
    }
}
