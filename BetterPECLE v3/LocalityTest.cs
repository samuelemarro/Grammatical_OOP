using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    /*
    Si genera un cromosoma casuale e lo si valuta senza applicare correzioni. Si fanno delle modifiche casuali
    e si 
    */
    public class LocalityTest
    {
        public GrammaticalEvolution GE1 { get; private set; }
        public GrammaticalEvolution GE2 { get; private set; }
        public FitnessCalculator Calculator { get; private set; }
        public ExecutionParameters Parameters { get; private set; }

        public LocalityTest(GrammaticalEvolution ge1, GrammaticalEvolution ge2, FitnessCalculator calculator, ExecutionParameters parameters)
        {
            GE1 = ge1;
            GE2 = ge2;
            Calculator = calculator;
            Parameters = parameters;
        }

        public void ExecuteTest(int testSize, int mutatedIndividuals, int initialChromosomeSize, double failureFitnessValue, bool useGE1ForBase, bool includeFailedCorrections)
        { 
            List<double> localityResults1 = new List<double>();
            List<double> localityResults2 = new List<double>();
            for (int i = 0; i < testSize; i++)
            {
                Chromosome baseChromosome = null;
                double baseFitness = failureFitnessValue;
                while (baseFitness == failureFitnessValue)//Check that it is a correct individual
                {
                    baseChromosome = new Chromosome(initialChromosomeSize, 64);
                    GrammaticalEvolution baseGE = (useGE1ForBase ? GE1 : GE2).GetClone();
                    baseFitness = Evaluate(baseChromosome, true, baseGE).Item1;
                }
                for (int j = 0; j < mutatedIndividuals; j++)//TODO: Controllare che salvi (aggiornamento: non lo fa). Perché le differenze del 2 è di 1?.
                {
                    Chromosome mutatedChromosome1 = SingleMutation(baseChromosome);
                    mutatedChromosome1.BackupCodons = baseChromosome.ToList();
                    Chromosome mutatedChromosome2 = mutatedChromosome1.GetClone();

                    Tuple<double, bool> result1 = Evaluate(mutatedChromosome1, false, GE1);
                    Tuple<double, bool> result2 = Evaluate(mutatedChromosome2, false, GE2);

                    if (result1.Item1 != baseFitness /*&& (!result1.Item2 || (includeFailedCorrections && result1.Item2))*/)
                        localityResults1.Add(Math.Abs(result1.Item1 - baseFitness));
                    if (result2.Item1 != baseFitness /*&& (!result2.Item2 || (includeFailedCorrections && result2.Item2))*/)
                        localityResults2.Add(Math.Abs(result2.Item1 - baseFitness));

                }
            }
        }
        private Tuple<double, bool> Evaluate(Chromosome chromosome, bool saveBackupCodons, GrammaticalEvolution ge)
        {
            GrammaticalEvolution newGE = ge.GetClone();
            string code = null;
            object result = null;
            Exception generationException = null;
            CompilerErrorCollection compilationErrors = null;
            Exception executionException = null;
            GenerationStats s = new GenerationStats();

            try
            {
                code = newGE.Generate(chromosome, s);
            }
            catch (GrammaticalEvolution.ErrorCorrectionFailedException e)
            {
                generationException = e;
            }
            catch (GrammaticalEvolution.EndOfCodonQueueException e)
            {
                generationException = e;
            }

            if (generationException == null)
            {
                try
                {
                    result = Executor.Execute(Parameters, code);
                }
                catch (Executor.CompilationErrorException e)
                {
                    compilationErrors = e.Errors;
                }
                catch (Executor.ExecutionExceptionException e)
                {
                    executionException = e.InnerException;
                }
            }

            if (generationException != null && compilationErrors != null && executionException != null && saveBackupCodons)
            {
                chromosome.BackupCodons = chromosome.ToList();
            }

            return new Tuple<double, bool>(Calculator(result, generationException, compilationErrors, executionException), s.failedErrorCorrections > 0);
        }
        private static Chromosome SingleMutation(Chromosome chromosome)
        {
            Chromosome newChromosome = chromosome.GetClone();
            newChromosome[GrammaticalEvolution.random.Next(chromosome.Count)] = (byte)GrammaticalEvolution.random.Next(0, 256);
            return newChromosome;
        }
    }
}
