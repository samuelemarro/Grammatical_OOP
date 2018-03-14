using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammatical_OOP
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
