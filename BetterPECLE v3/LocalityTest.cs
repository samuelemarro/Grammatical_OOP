using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{

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

        public Tuple<List<double>,List<double>> ExecuteTest(int testnumber, int testSize, int mutatedIndividuals, int initialChromosomeSize, double failureFitnessValue, bool useGE1ForBase, bool includeFailedCorrections)
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
                for (int j = 0; j < mutatedIndividuals; j++)//TODO: Perché le differenze del 2 è di 1?.
                {
                    Chromosome mutatedChromosome1 = SingleMutation(baseChromosome);
                    mutatedChromosome1.BackupCodons = baseChromosome.ToList();
                    Chromosome mutatedChromosome2 = mutatedChromosome1.GetClone();

                    Tuple<double, bool> result1 = Evaluate(mutatedChromosome1, false, GE1);
                    Tuple<double, bool> result2 = Evaluate(mutatedChromosome2, false, GE2);
                    double minDistance = (double)1 / 64;

                    localityResults1.Add(Math.Abs(result1.Item1 - baseFitness - minDistance));

                    double secondResult = result2.Item2 ? baseFitness : result2.Item1;
                    localityResults2.Add(Math.Abs(secondResult - baseFitness - minDistance));
                }
                Console.WriteLine(i + " out of " + testSize);
            }
            Console.WriteLine("Media 1:" + localityResults1.Average());
            Console.WriteLine("Media 2:" + localityResults2.Average());
            Console.WriteLine("Miglioramento:" + ((localityResults1.Average() - localityResults2.Average()) / localityResults1.Average()));
            return new Tuple<List<double>, List<double>>(localityResults1, localityResults2);
            /*string percorso = @"C:\Users\Samuele\Documents\BetterPECLE\v3\Studio località\";
            //string avviso = "ATTENZIONE: QUESTI RISULTATI SONO STATI DEFINITI PREVENTIVAMENTE COME TEST.\n" +
            //    "AL FINE DI RISPETTARE L'ETICA SCIENTIFICA, NON USARE QUESTI DATI IN NESSUNA PUBBLICAZIONE.\n\n";

            double miglioramento = ((localityResults1.Average() - localityResults2.Average()) / localityResults1.Average());

            string spiegazione = "I seguenti dati indicano la differenza di fitness medio per mutazione. Più la differenza\n" +
                "è bassa, più la rappresentazione è locale.\n\n";
            File.WriteAllText(percorso + "Studio" + testnumber + ".txt",
                spiegazione + "Media default: " + localityResults1.Average() +
                "\nDeviazione Standard: " + StdDev(localityResults1) +
                "\nMedia PECLE: " + localityResults2.Average() +
                "\nDeviazione Standard: " + StdDev(localityResults2) +
                "\nMiglioramento: " + miglioramento);*/
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

            if (generationException == null && compilationErrors == null && executionException == null && saveBackupCodons)
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
        public static double StdDev(IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }
    }
}
