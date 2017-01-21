using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BetterPECLE_v3
{
    [Serializable]
    public class Chromosome : List<byte>
    {
        public int LastUsedCodonPosition { get; internal set; }

        public List<byte> BackupCodons { get; internal set; }

        public int MaximumSize { get; }

        public double? Fitness { get; internal set; }

        public bool ReEvaluate { get; internal set; }

        public Chromosome(int initialSize, int maximumSize)
        {
            byte[] genes = new byte[initialSize];

            GrammaticalEvolution.random.NextBytes(genes);
            foreach (byte b in genes)
            {
                base.Add(b);
            }
            LastUsedCodonPosition = -1;
            MaximumSize = maximumSize;
        }

        public new byte this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                Fitness = new double?();
                base[index] = value;
            }
        }

        public byte NewCodon()
        {
            return (byte)GrammaticalEvolution.random.Next(0, byte.MaxValue + 1);
        }

        public Chromosome GetClone()
        {
            Chromosome chromosome = new Chromosome(0, MaximumSize);

            foreach (byte b in this)
            {
                chromosome.Add(b);
            }
            chromosome.Fitness = Fitness;
            chromosome.ReEvaluate = ReEvaluate;

            chromosome.LastUsedCodonPosition = LastUsedCodonPosition;
            if (BackupCodons != null)
                chromosome.BackupCodons = new List<byte>(BackupCodons);

            return chromosome;
        }
        

    }
}
