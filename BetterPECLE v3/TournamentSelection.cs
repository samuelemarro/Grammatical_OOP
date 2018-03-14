using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammatical_OOP
{
    public class TournamentSelection : Selection
    {
        public int TournamentSize { get; }
        public bool FirstWinnerCanCompeteAgain { get; }

        public TournamentSelection(int tournamentSize,bool firstWinnerCanCompeteAgain)
        {
            TournamentSize = tournamentSize;
            FirstWinnerCanCompeteAgain = firstWinnerCanCompeteAgain;
        }

        public override Tuple<Chromosome, Chromosome> Select(Generation generation)
        {
            if (TournamentSize > generation.Count - (FirstWinnerCanCompeteAgain ? 0 : 1))
                throw new ArgumentException("The number of individuals(" + generation.Count + ") is smaller than " + (FirstWinnerCanCompeteAgain ? "" : "or equal to ") + "the tournament size(" + TournamentSize + ").");

            List<Chromosome> chromosomes = generation.ToList();

            List<Chromosome> firstTournament = new List<Chromosome>();
            
            for(int i = 0; i < TournamentSize; i++)
            {
                firstTournament.Add(chromosomes[GrammaticalEvolution.random.Next(0, chromosomes.Count)]);
            }

            Chromosome firstParent = firstTournament.OrderByDescending(x => x.Fitness).First();

            if (!FirstWinnerCanCompeteAgain)
                chromosomes.Remove(firstParent);

            List<Chromosome> secondTournament = new List<Chromosome>();
            for (int j = 0; j < TournamentSize; j++)
            {
                secondTournament.Add(chromosomes[GrammaticalEvolution.random.Next(0, chromosomes.Count)]);
            }

            Chromosome secondParent = secondTournament.OrderByDescending(x => x.Fitness).First();

            return new Tuple<Chromosome, Chromosome>(firstParent, secondParent);
        }
    }
}
