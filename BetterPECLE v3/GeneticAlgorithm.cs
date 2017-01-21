using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace BetterPECLE_v3
{
    public delegate double FitnessCalculator(object result, Exception generationException, CompilerErrorCollection compilationErrors, Exception executionException);

    public class GeneticAlgorithm
    {
        public int ElitismSize { get; }
        public GrammaticalEvolution GE { get; }
        public FitnessCalculator _FitnessCalculator { get; }
        public Crossover Crossover { get; }
        public Mutation Mutation { get; }
        public Population Population { get; }
        public Selection Selection { get; }
        public ExecutionParameters Parameters { get; }

        public GeneticAlgorithm(int elitismSize, GrammaticalEvolution ge, FitnessCalculator fitnessCalculator, Selection selection, Crossover crossover, Mutation mutation, Population population, ExecutionParameters parameters)
        {
            ElitismSize = elitismSize;
            GE = ge;
            _FitnessCalculator = fitnessCalculator;
            Selection = selection;
            Crossover = crossover;
            Mutation = mutation;
            Population = population;
            Parameters = parameters;
        }

        public GeneticAlgorithmResult Evolve(int generations, double crossoverProbability, double mutationProbability, double duplicationProbability, double pruningProbability)
        {
            GeneticAlgorithmResult result = new GeneticAlgorithmResult();
            result.stats = new List<GenerationStats>();

            //Evaluate and save the results of the first generation

            Evaluate(Population.CurrentGeneration);

            Population.CurrentGeneration.Stats.fitnessValues = Population.CurrentGeneration.Select(x => x.Fitness.Value).ToList();
            Population.CurrentGeneration.Stats.bestChromosome = Population.CurrentGeneration.OrderByDescending(x => x.Fitness).First();

            for (int i = 1; i < generations; i++)
            {
                Generation newGeneration = new Generation();

                //Finds all the individuals with a valid fitness and takes the best ones
                int elitesNumber = Math.Min(Population.CurrentGeneration.FindAll(x => x.Fitness.HasValue).Count, ElitismSize);

                List<Chromosome> elites = Population.CurrentGeneration.OrderByDescending(x => x.Fitness.Value).Take(elitesNumber).Select(x => x.GetClone()).ToList();

                int newGenerationSize = Population.CurrentGeneration.Count - elitesNumber;

                while (newGeneration.Count < newGenerationSize)
                {
                    Tuple<Chromosome, Chromosome> parents = Selection.Select(Population.CurrentGeneration);

                    Tuple<Chromosome, Chromosome> children = Crossover.Cross(parents.Item1, parents.Item2, crossoverProbability);

                    //If there is room for only one individual, choose randomly
                    if (newGeneration.Count == newGenerationSize - 1)
                    {
                        if (GrammaticalEvolution.random.NextDouble() > 0.5)
                            newGeneration.Add(children.Item1);
                        else
                            newGeneration.Add(children.Item2);
                    }
                    else
                    {
                        newGeneration.Add(children.Item1);
                        newGeneration.Add(children.Item2);
                    }
                }

                foreach (Chromosome c in newGeneration)
                {
                    Mutation.Mutate(c, mutationProbability);
                }

                Evaluate(newGeneration);
                
                Prune(newGeneration, pruningProbability);
                Duplicate(newGeneration, duplicationProbability);

                newGeneration.AddRange(elites);

                newGeneration.Stats.fitnessValues = newGeneration.Select(x => x.Fitness.Value).ToList();
                newGeneration.Stats.bestChromosome = newGeneration.OrderByDescending(x => x.Fitness).First().GetClone();
                
                Population.Add(newGeneration);

                result.stats.Add(newGeneration.Stats);
            }
            return result;
        }


        private void Prune(List<Chromosome> chromosomes, double pruningProbability)
        {
            foreach (Chromosome chromosome in chromosomes)
            {
                if (GrammaticalEvolution.random.NextDouble() < pruningProbability)
                {
                    while (chromosome.Count > chromosome.LastUsedCodonPosition + 1)
                    {
                        chromosome.RemoveAt(chromosome.Count - 1);
                    }
                }
            }
        }

        private void Duplicate(List<Chromosome> chromosomes, double duplicationProbability)
        {
            foreach (Chromosome chromosome in chromosomes)
            {
                if (GrammaticalEvolution.random.NextDouble() < duplicationProbability)
                {
                    int startPosition = GrammaticalEvolution.random.Next(0, chromosome.Count - 1);
                    int endPosition = GrammaticalEvolution.random.Next(startPosition + 1, chromosome.Count);

                    int subArraySize = Math.Min(endPosition - startPosition, chromosome.MaximumSize - chromosome.Count);

                    List<byte> newCodons = chromosome.Skip(startPosition).Take(subArraySize).ToList();

                    chromosome.AddRange(newCodons);

                    //Duplication can affect the behaviour of the individual if there was an EndOfCodonQueueException, so we mark the chromosome for reevalutation
                    chromosome.ReEvaluate = true;
                    chromosome.LastUsedCodonPosition = -1;
                }
            }

        }

        private void Evaluate(Generation generation)
        {
            foreach (Chromosome chromosome in generation)
            {
                if (chromosome.Fitness.HasValue && !chromosome.ReEvaluate)
                    continue;
                generation.Stats.executedEvaluations++;
                chromosome.ReEvaluate = false;

                GrammaticalEvolution newGE = (GrammaticalEvolution)DeepClone(GE);
                string code = null;
                object result = null;
                Exception generationException = null;
                CompilerErrorCollection compilationErrors = null;
                Exception executionException = null;

                try
                {
                    code = newGE.Generate(chromosome, generation.Stats);
                }
                catch (GrammaticalEvolution.ErrorCorrectionFailedException e)
                {
                    generationException = e;
                    generation.Stats.generationErrors++;
                }
                catch(GrammaticalEvolution.EndOfCodonQueueException e)
                {
                    generationException = e;
                    generation.Stats.generationErrors++;
                }

                if (generationException == null)
                {
                    try
                    {
                        result = Execute(code);
                    }
                    catch (Executor.CompilationErrorException e)
                    {
                        compilationErrors = e.Errors;
                        generation.Stats.compilationErrors++;
                    }
                    catch (Executor.ExecutionExceptionException e)
                    {
                        executionException = e.InnerException;
                        generation.Stats.executionExceptions++;
                    }
                }
                chromosome.Fitness = _FitnessCalculator(result, generationException, compilationErrors, executionException);

                if (generationException == null && compilationErrors == null && executionException == null)
                    chromosome.BackupCodons = chromosome.ToList();//If the compilation was successful the codons are copied to the backupCodons
            }
        }

        private object DeepClone(object initialObject)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, initialObject);

            stream.Seek(0, SeekOrigin.Begin);

            object result = formatter.Deserialize(stream);
            stream.Dispose();
            return result;
        }

        public object Execute(string code)
        {
            return Executor.Execute(Parameters, code);
        }


    }
}
