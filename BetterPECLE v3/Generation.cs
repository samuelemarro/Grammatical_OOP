using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public class Generation : List<Chromosome>
    {
        public GenerationStats Stats { get; set; }
        public Generation() : base()
        {
            Stats = new GenerationStats();
        }
    }
}
