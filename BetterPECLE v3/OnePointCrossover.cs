using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public class OnePointCrossover : Crossover
    {
        public override Tuple<Chromosome, Chromosome> Cross(Chromosome parent1, Chromosome parent2, double crossoverProbability)
        {
            if (GrammaticalEvolution.random.NextDouble() < crossoverProbability)
            {
                int crossoverPoint = GrammaticalEvolution.random.Next(0, Math.Min(parent1.Count, parent2.Count));

                Chromosome child1 = new Chromosome(0, parent1.MaximumSize);
                Chromosome child2 = new Chromosome(0, parent2.MaximumSize);

                for (int i = 0; i < crossoverPoint; i++)
                {
                    child1.Add(parent1[i]);
                    child2.Add(parent2[i]);
                }

                for (int j = crossoverPoint; j < parent2.Count; j++)
                {
                    child1.Add(parent2[j]);
                }

                for (int k = crossoverPoint; k < parent1.Count; k++)
                {
                    child2.Add(parent1[k]);
                }

                if(GrammaticalEvolution.random.NextDouble() > 0.5)
                {
                    child1.BackupCodons = parent1.BackupCodons == null ? null : parent1.BackupCodons.ToList();
                    child2.BackupCodons = parent2.BackupCodons == null ? null : parent2.BackupCodons.ToList();
                }
                else
                {
                    child1.BackupCodons = parent2.BackupCodons == null ? null : parent2.BackupCodons.ToList();
                    child2.BackupCodons = parent1.BackupCodons == null ? null : parent1.BackupCodons.ToList();
                }

                return new Tuple<Chromosome, Chromosome>(child1, child2);
            }
            else
            {
                return new Tuple<Chromosome, Chromosome>(parent1.GetClone(), parent2.GetClone());
            }
        }
    }
}
