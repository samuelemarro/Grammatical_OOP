using System;
using System.Collections;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;
using System.Threading;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace BetterPECLE_v3
{
    class Program
    {
        public const string genericCodeWrapper = "namespace CodeExecutor{public class Executor{public TYPE Execute(){\n PECLECODE \n}}}";
        public const string oneMaxCodeWrapper = "namespace CodeExecutor{public class Executor{public bool[] Execute(){\n bool[] grid = new bool[64]; int a = 0; int b = 0; int c = 0;\n PECLECODE return grid;\n}}}";

        static void Main(string[] args)
        {
            /*string p = @"C:\Users\Samuele\Documents\BetterPECLE\v3\Studio località\Studio";
            List<double> valori1 = new List<double>();
            List<double> valori2 = new List<double>();
            for (int i = 0; i < 20; i++)
            {
                //valori.Add(OttieniMiglioramento(p + i + ".txt"));
                valori1.Add(OttieniDefault(p + i + ".txt"));
                valori2.Add(OttieniPECLE(p + i + ".txt"));
            }

            double media1 = valori1.Average();
            double media2 = valori2.Average();
            double miglioramento = (media1 - media2) / media1;
            */
            /*var a = ReadFromBinaryFile<Tuple<GeneticAlgorithmResult,GeneticAlgorithmResult>>(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Studio fitness\Test 2.peclefitness");
            double averageGenerationErrors = a.Item2.stats.Select(x => x.generationErrors / x.executedEvaluations).Average();
            a = null;
            */

            /*int executionNumber = 100;

            if (args.Length > 0)
                executionNumber = int.Parse(args[0].Substring(1));

            Console.WriteLine("Esecuzione " + executionNumber + " di 100");

            var results = ExtractData(50, 200, 256);
            executionNumber++;

            WriteToBinaryFile(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Studio fitness\Test " + executionNumber + ".peclefitness", results);

            PlaySound();
            if (executionNumber < 105)
                Restart(executionNumber);*/
            UsaQuestoPerEsaminareIDati();

            //var phr = results1.SelectMany(x => x.stats).ToList();
            //var standard = results2.SelectMany(x => x.stats).ToList();

            //double mediaPHR = phr.Select(x => (x.compilationErrors + x.executionExceptions + x.failedErrorCorrections) / x.executedEvaluations).Average();
            //double deviazionePHR = LocalityTest.StdDev(phr.Select(x => (x.compilationErrors + x.executionExceptions + x.failedErrorCorrections) / x.executedEvaluations));
            //double mediaStandard = standard.Select(x => (x.compilationErrors + x.executionExceptions + x.failedErrorCorrections) / x.executedEvaluations).Average();
            //double deviazioneStandard = LocalityTest.StdDev(standard.Select(x => (x.compilationErrors + x.executionExceptions + x.failedErrorCorrections) / x.executedEvaluations));

            /*GrammaticalEvolution ge_noImprovements = new GrammaticalEvolution();
            ge_noImprovements.BluePrintGOs = new List<GrammaticalObject>() { new OneMaxGO_NoImprovements() };
            ge_noImprovements.StartingGO = new OneMaxGO_NoImprovements();
            ge_noImprovements.StartingProductionRule = new ProductionRule(new NonTerminal("code"));

            GrammaticalEvolution ge = new GrammaticalEvolution();
            ge.BluePrintGOs = new List<GrammaticalObject>() { new OneMaxGO() };
            ge.StartingGO = new OneMaxGO();
            ge.StartingProductionRule = new ProductionRule(new NonTerminal("code"));
            int iniziale = executionNumber;
            LocalityTest t = new LocalityTest(ge_noImprovements, ge, Calculator, new ExecutionParameters(oneMaxCodeWrapper, "CodeExecutor.Executor", "Execute"));
            Tuple<List<double>, List<double>> risultato = t.ExecuteTest(iniziale, 200, 15, 64, -1, true, true);

            executionNumber++;

            WriteToBinaryFile(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Studio località\Test " + executionNumber + ".peclelocality", risultato);

            PlaySound();
            Restart(executionNumber);*/


            /*Dictionary<double, int> distribuzione1 = new Dictionary<double, int>();
            Dictionary<double, int> distribuzione2 = new Dictionary<double, int>();

            for(int i = 1; i <= 50; i++)
            {
                var tuple = ReadFromBinaryFile<Tuple<List<double>, List<double>>>(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Studio località\Test " + i + ".peclelocality");
                foreach(double d1 in tuple.Item1)
                {
                    if (distribuzione1.Keys.Contains(d1))
                        distribuzione1[d1]++;
                    else
                        distribuzione1.Add(d1, 1);
                }
                foreach (double d2 in tuple.Item2)
                {
                    if (distribuzione2.Keys.Contains(d2))
                        distribuzione2[d2]++;
                    else
                        distribuzione2.Add(d2, 1);
                }
            }

            double media1 = distribuzione1.Select(x => x.Key * x.Value).Sum() / (double)distribuzione1.Values.Sum();
            double media2 = distribuzione2.Select(x => x.Key * x.Value).Sum() / (double)distribuzione2.Values.Sum();
            */
            /*GrammaticalEvolution ge = new GrammaticalEvolution();
            ge.BluePrintGOs = new List<GrammaticalObject>() { new OneMaxGO_NoImprovements() };
            ge.StartingGO = new OneMaxGO_NoImprovements();
            ge.StartingProductionRule = new ProductionRule(new NonTerminal("code"));
            
            GeneticAlgorithm ga = new GeneticAlgorithm(1, ge, Calculator, new TournamentSelection(3, false), new OnePointCrossover(), new UniformMutation(), new Population(60, 40, 64), new ExecutionParameters("namespace A{ public class B{ public bool[] C() { \nbool[] grid = new bool[64];\n int a = 0;\n int b = 0;\n int c = 0;\n PECLECODE\n return grid;}}}", "A.B", "C"));

            GeneticAlgorithmResult result = ga.Evolve(80, 0.5, 0.1, 0.4, 0.6);

            int count = result.stats.Select(x => x.executedEvaluations).Sum();
            double maxFitness = result.MaxFitness;*/
            /*
            try
            {
                object result = Executor.Execute(new ExecutionParameters("namespace A{ public class B{ public bool[] C() { PECLECODE }}}", "A.B", "C"), "bool[] grid = new bool[64];" + code + "\nreturn grid;");
            }
            catch (Exception e)
            {

            }*/
            //object result = Executor.Execute(new ExecutionParameters("namespace A{ public class B{ public int C() { PECLECODE }}}", "A.B", "C"), "return 1;");
            //object o = ReadFromBinaryFile<GeneticAlgorithmResult>(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Default\0.pecle");
            //Executor.Execute(new ExecutionParameters(genericCodeWrapper.Replace("TYPE", "int"), "CodeExecutor.Executor", "Execute")," int a = 5; return a / (5-a);");

            /*List<GeneticAlgorithmResult> results = OpenResults(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Test 6 - Valido, primi 50\Default");

            double initialMaxFitness = results.Select(x => x.stats[0].MaxFitness).Average();
            double initialMinFitness = results.Select(x => x.stats[0].MinFitness).Average();
            double finalMaxFitness = results.Select(x => x.stats[48].MaxFitness).Average();
            double finalMinFitness = results.Select(x => x.stats[48].MinFitness).Average();

            double averageDifference = results.Select(x => x.stats[48].MaxFitness - x.stats[0].MaxFitness).Average();
            
            List<List<double>> relativeErrors = results.Select(x => x.stats.Select(y => (y.executionExceptions + y.compilationErrors + y.failedErrorCorrections) / y.executedEvaluations).ToList()).ToList();
            List<double> relativeErrors2 = relativeErrors.Select(x => x.Average()).ToList();
            double finalRelativeError = relativeErrors2.Average();
            
            double errorDifference = results.Select(x => (x.stats[48].generationErrors / x.stats[48].executedEvaluations) - (x.stats[0].generationErrors / x.stats[0].executedEvaluations)).Average();
            
            GeneticAlgorithmResult averageResult = AverageResults(results);*/
            /*List<GeneticAlgorithmResult> results = OpenResults(@"C:\Users\Samuele\Documents\BetterPECLE\v3\PECLE");
            List<string> lines = new List<string>();

            for(int i = 0; i < results[0].stats.Count; i++)
            {
                string line = "";
                line += results.Select(x => (x.stats[i].compilationErrors) / x.stats[i].executedEvaluations).Average() + "\t";
                line += results.Select(x => (x.stats[i].executionExceptions) / x.stats[i].executedEvaluations).Average() + "\t";
                line += results.Select(x => (x.stats[i].failedErrorCorrections) / x.stats[i].executedEvaluations).Average() + "\t";

                line += results.Select(x => x.stats[i].passedErrorChecks / x.stats[i].executedEvaluations).Average() + "\t";
                line += results.Select(x => x.stats[i].failedErrorChecks / x.stats[i].executedEvaluations).Average() + "\t";
                line += results.Select(x => x.stats[i].successfulErrorCorrections / x.stats[i].executedEvaluations).Average() + "\t";
                line += results.Select(x => x.stats[i].failedErrorCorrections / x.stats[i].executedEvaluations).Average() + "\t";
                lines.Add(line);
            }

            File.WriteAllLines(@"C:\Users\Samuele\Documents\BetterPECLE\v3\PECLE.csv", lines.Select(x=> x.ToString()).ToArray());*/
            /*List<GeneticAlgorithmResult> results = OpenResults(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Test 6 - Valido, primi 50\Default");
            List<string> lines = new List<string>();

            for(int i = 0; i < results[0].stats.Count; i++)
            {
                string line = "";
                line += results.Select(x => x.stats[i].AverageFitness).Average() + "\t";
                line += results.Select(x => x.stats[i].MaxFitness).Average() + "\t";
                line += results.Select(x => x.stats[i].MinFitness).Average() + "\t";

                line += results.Select(x => x.stats[i].generationErrors / x.stats[i].executedEvaluations).Average() + "\t";
                lines.Add(line);
            }

            File.WriteAllLines(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Default_Fitness.csv", lines.Select(x=> x.ToString()).ToArray());*/
        }

        static void UsaQuestoPerEsaminareIDati()
        {
            List<string> files = Directory.EnumerateFiles(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Studio fitness").ToList();
            List<GeneticAlgorithmResult> results1 = new List<GeneticAlgorithmResult>();
            List<GeneticAlgorithmResult> results2 = new List<GeneticAlgorithmResult>();
            foreach (string file in files)
            {
                if (!file.EndsWith(".peclefitness"))
                    continue;

                Tuple<GeneticAlgorithmResult, GeneticAlgorithmResult> t = ReadFromBinaryFile<Tuple<GeneticAlgorithmResult, GeneticAlgorithmResult>>(file);
                results1.Add(t.Item1);
                results2.Add(t.Item2);
            }

            object[][] values = new object[results1[0].stats.Count][];

            for (int i = 0; i < results1[0].stats.Count; i++)
            {
                values[i] = new object[]
                {
                    results1.Select(x => x.stats[i].AverageFitness).Average(),
                    results1.Select(x=> x.stats[i].MaxFitness).Average(),
                    results1.Select(x=>x.stats[i].MinFitness).Average(),
                    "",
                    results2.Select(x=> x.stats[i].AverageFitness).Average(),
                    results2.Select(x=>x.stats[i].MaxFitness).Average(),
                    results2.Select(x=>x.stats[i].MinFitness).Average(),
                    "",
                    results1.Select(x=> x.stats[i].compilationErrors / x.stats[i].executedEvaluations).Average(),
                    results1.Select(x=> x.stats[i].executionExceptions / x.stats[i].executedEvaluations).Average(),
                    results1.Select(x=> x.stats[i].failedErrorCorrections / x.stats[i].executedEvaluations).Average(),
                    results1.Select(x=> x.stats[i].generationErrors / x.stats[i].executedEvaluations).Average(),
                    "",
                    results2.Select(x=> x.stats[i].compilationErrors / x.stats[i].executedEvaluations).Average(),
                    results2.Select(x=> x.stats[i].executionExceptions / x.stats[i].executedEvaluations).Average(),
                    results2.Select(x=> x.stats[i].failedErrorCorrections / x.stats[i].executedEvaluations).Average(),
                    results2.Select(x=> x.stats[i].generationErrors / x.stats[i].executedEvaluations).Average(),
                };
            }

            SaveToCSV(@"C:\Users\Samuele\Documents\BetterPECLE\v3\Studio fitness\AnalisiDati.csv", "\t", values);
        }

        private static void SaveToCSV(string path, string separator, object[][] values)
        {
            string file = "";
            for (int y = 0; y < values.Length; y++)
            {
                for (int x = 0; x < values[0].Length; x++)
                {
                    file += values[y][x] + separator;
                }
                file += "\n";
            }
            File.WriteAllText(path, file);
        }

        private static void Restart(int restartNumber)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Process.Start(path, "t" + restartNumber);
            Environment.Exit(0);
        }

        private static List<GeneticAlgorithmResult> OpenResults(string path)
        {
            List<GeneticAlgorithmResult> results = new List<GeneticAlgorithmResult>();
            foreach (string fileName in Directory.EnumerateFiles(path))
            {
                List<GeneticAlgorithmResult> chosenResults = ReadFromBinaryFile<List<GeneticAlgorithmResult>>(fileName);
                results.AddRange(chosenResults);
            }
            return results;
        }

        private static GeneticAlgorithmResult AverageResults(List<GeneticAlgorithmResult> results)
        {
            GeneticAlgorithmResult averageResult = new GeneticAlgorithmResult();
            averageResult.stats = new List<GenerationStats>();

            List<List<GenerationStats>> unitedStats = results.Select(x => x.stats).ToList();

            for (int i = 0; i < results[0].stats.Count; i++)
            {
                GenerationStats stat = new GenerationStats();

                stat.compilationErrors = unitedStats.Select(x => x[i].compilationErrors).Average();
                stat.executedEvaluations = unitedStats.Select(x => x[i].executedEvaluations).Average();
                stat.executionExceptions = unitedStats.Select(x => x[i].executionExceptions).Average();
                stat.failedErrorChecks = unitedStats.Select(x => x[i].failedErrorChecks).Average();
                stat.failedErrorCorrections = unitedStats.Select(x => x[i].failedErrorCorrections).Average();
                stat.generationErrors = unitedStats.Select(x => x[i].generationErrors).Average();
                stat.passedErrorChecks = unitedStats.Select(x => x[i].passedErrorChecks).Average();
                stat.successfulErrorCorrections = unitedStats.Select(x => x[i].successfulErrorCorrections).Average();

                stat.fitnessValues = new List<double>();
                for (int j = 0; j < unitedStats[0][0].fitnessValues.Count; j++)
                {
                    stat.fitnessValues.Add(unitedStats.Select(x => x[i].fitnessValues[j]).Average());
                }

                averageResult.stats.Add(stat);
            }

            return averageResult;
        }

        private static Tuple<GeneticAlgorithmResult, GeneticAlgorithmResult> ExtractData(int populationSize, int generationNumber, int maximumChromosomeSize)
        {
            int elitismSize = GrammaticalEvolution.random.Next(1, 5);
            int tournamentSize = GrammaticalEvolution.random.Next(2, 6);
            double crossoverProbability = GrammaticalEvolution.random.NextDouble();
            double mutationProbability = GrammaticalEvolution.random.NextDouble();
            double duplicationProbability = GrammaticalEvolution.random.NextDouble();
            double pruningProbability = GrammaticalEvolution.random.NextDouble();
            int initialChromosomeSize = GrammaticalEvolution.random.Next(64, maximumChromosomeSize);

            Console.WriteLine("Inizio test di PECLE...");
            GeneticAlgorithmResult pecleResult = PECLETest(populationSize, generationNumber, initialChromosomeSize, maximumChromosomeSize, elitismSize, tournamentSize, crossoverProbability, mutationProbability, duplicationProbability, pruningProbability);

            Console.WriteLine("Inizio test di Default...");
            GeneticAlgorithmResult noImprovementsResult = PECLETest_NoImprovements(populationSize, generationNumber, initialChromosomeSize, maximumChromosomeSize, elitismSize, tournamentSize, crossoverProbability, mutationProbability, duplicationProbability, pruningProbability);

            return new Tuple<GeneticAlgorithmResult, GeneticAlgorithmResult>(pecleResult, noImprovementsResult);
        }

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        private static GeneticAlgorithmResult PECLETest(int populationSize, int generationNumber, int initialChromosomeSize, int maximumChromosomeSize, int elitismSize, int tournamentSize, double crossoverProbability, double mutationProbability, double duplicationProbability, double pruningProbability)
        {
            GrammaticalEvolution ge = new GrammaticalEvolution();
            ge.BluePrintGOs = new List<GrammaticalObject>() { new OneMaxGO() };
            ge.StartingGO = new OneMaxGO();
            ge.StartingProductionRule = new ProductionRule(new NonTerminal("code"));

            GeneticAlgorithm ga = new GeneticAlgorithm(elitismSize, ge, Calculator, new TournamentSelection(tournamentSize, false), new OnePointCrossover(), new UniformMutation(), new Population(populationSize, initialChromosomeSize, maximumChromosomeSize), new ExecutionParameters("namespace A{ public class B{ public bool[] C() { \nbool[] grid = new bool[64];\n int a = 0;\n int b = 0;\n int c = 0;\n PECLECODE\n return grid;}}}", "A.B", "C"));

            return ga.Evolve(generationNumber, crossoverProbability, mutationProbability, duplicationProbability, pruningProbability);
        }

        private static GeneticAlgorithmResult PECLETest_NoImprovements(int populationSize, int generationNumber, int initialChromosomeSize, int maximumChromosomeSize, int elitismSize, int tournamentSize, double crossoverProbability, double mutationProbability, double duplicationProbability, double pruningProbability)
        {
            GrammaticalEvolution ge = new GrammaticalEvolution();
            ge.BluePrintGOs = new List<GrammaticalObject>() { new OneMaxGO_NoImprovements() };
            ge.StartingGO = new OneMaxGO_NoImprovements();
            ge.StartingProductionRule = new ProductionRule(new NonTerminal("code"));

            GeneticAlgorithm ga = new GeneticAlgorithm(elitismSize, ge, Calculator, new TournamentSelection(tournamentSize, false), new OnePointCrossover(), new UniformMutation(), new Population(populationSize, initialChromosomeSize, maximumChromosomeSize), new ExecutionParameters("namespace A{ public class B{ public bool[] C() { \nbool[] grid = new bool[64];\n int a = 0;\n int b = 0;\n int c = 0;\n PECLECODE\n return grid;}}}", "A.B", "C"));

            return ga.Evolve(generationNumber, crossoverProbability, mutationProbability, duplicationProbability, pruningProbability);
        }

        private static double Calculator(object result, Exception generationException, CompilerErrorCollection errors, Exception executionException)
        {
            if (generationException != null || errors != null || executionException != null)
                return -1;
            return (double)(((bool[])result).Count(x => x)) / (double)(((bool[])result).Length);
        }
        private static void PlaySound()
        {
            for (int i = 0; i < 4; i++)
            {
                SystemSounds.Beep.Play();
                Thread.Sleep(500);
            }
        }
    }

    [Serializable]
    public class OneMaxGO : GrammaticalObject, ISerializable
    {
        public const int maxSelfReferenceDepth = 5;
        public const int arraySize = 64;
        public const string genericCodeWrapper = "namespace CodeExecutor{public class Executor{public TYPE Execute(){\n PECLECODE \n}}}";

        public override GrammaticalObject GetClone()
        {
            OneMaxGO go = new OneMaxGO();

            return go;
        }

        public OneMaxGO() { }


        protected OneMaxGO(SerializationInfo info, StreamingContext context)
        {
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        public override Dictionary<string, List<ProductionRule>> GetInitialProductionRules()
        {
            return new Dictionary<string, List<ProductionRule>>()
            {
                { "code", new List<ProductionRule>()
                {
                    new ProductionRule(new NonTerminal("codeImplementation", (go => go.CreateChild<OneMaxGO>())))
                }},
                { "codeImplementation", new List<ProductionRule>()
                {
                    new ProductionRule((NonTerminal)"code",(Terminal)"\n",(NonTerminal)"code"),
                    new ProductionRule((Terminal)"grid[", new ObjectFunction<OneMaxGO>(go => CheckValueWithinBounds(go)), (Terminal)"] = ", (NonTerminal)"condition", (Terminal)";"),
                    new ProductionRule((NonTerminal)"variable", (Terminal)" = ", (NonTerminal)"value", (Terminal)";"),
                    new ProductionRule(new NonTerminal("conditionalStatement", go => go.CreateChild<OneMaxGO>())),
                    new ProductionRule(new NonTerminal("loop", go => go.CreateChild<OneMaxGO>()))
                }},
                { "variable", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"a"),
                    new ProductionRule((Terminal)"b"),
                    new ProductionRule((Terminal)"c")
                }},
                { "condition", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"true"),
                    new ProductionRule((Terminal)"false"),
                    new ProductionRule((Terminal)"(", (NonTerminal)"value", (NonTerminal)"compOp", (NonTerminal)"value", (Terminal)")"),
                    new ProductionRule((Terminal)"(", (NonTerminal)"condition", (Terminal)" || ", (NonTerminal)"condition", (Terminal)")"),
                    new ProductionRule((Terminal)"(", (NonTerminal)"condition", (Terminal)" && ", (NonTerminal)"condition", (Terminal)")"),
                    new ProductionRule((Terminal)"(!", (NonTerminal)"condition", (Terminal)")")
                }},
                { "compOp", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)">"),
                    new ProductionRule((Terminal)">="),
                    new ProductionRule((Terminal)"<"),
                    new ProductionRule((Terminal)"<="),
                    new ProductionRule((Terminal)"=="),
                    new ProductionRule((Terminal)"!=")
                }},
                { "value", new List<ProductionRule>()
                {
                    new ProductionRule((NonTerminal)"digit"),
                    new ProductionRule((NonTerminal)"variable"),
                    new ProductionRule((Terminal)arraySize.ToString()),
                    new ProductionRule((Terminal)"(",(NonTerminal)"mathExpression",(Terminal)")")
                }},
                { "digit", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"0"),
                    new ProductionRule((Terminal)"1"),
                    new ProductionRule((Terminal)"2"),
                    new ProductionRule((Terminal)"3"),
                    new ProductionRule((Terminal)"4"),
                    new ProductionRule((Terminal)"5"),
                    new ProductionRule((Terminal)"6"),
                    new ProductionRule((Terminal)"7"),
                    new ProductionRule((Terminal)"8"),
                    new ProductionRule((Terminal)"9")
                }},
                { "mathExpression", new List<ProductionRule>()
                {
                    new ProductionRule((NonTerminal)"value", (Terminal)" + ", (NonTerminal)"value"),
                    new ProductionRule((NonTerminal)"value", (Terminal)" - ", (NonTerminal)"value"),
                    new ProductionRule((NonTerminal)"value", (Terminal)" * ", (NonTerminal)"value"),
                    new ProductionRule((NonTerminal)"value", (Terminal)" / ", new ObjectFunction<OneMaxGO>(go => CheckNonZeroValue(go))),
                    new ProductionRule((NonTerminal)"value", (Terminal)" % ", new ObjectFunction<OneMaxGO>(go => CheckNonZeroValue(go)))
                }},
                { "conditionalStatement", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"if(", (NonTerminal)"condition", (Terminal)")\n{\n",(NonTerminal)"code", (Terminal)"}"),
                    new ProductionRule((Terminal)"if(", (NonTerminal)"condition", (Terminal)")\n{\n", (NonTerminal)"code", (Terminal)"}\n else \n{\n",(NonTerminal)"code", (Terminal)"\n}"),
                    new ProductionRule((Terminal)"if(", (NonTerminal)"condition", (Terminal)")\n{\n", (NonTerminal)"code", (Terminal)"}\n else if(", (NonTerminal)"condition", (Terminal)")\n{\n", (NonTerminal)"code", (Terminal)"\n}")
                }},
                { "loop", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"while(", new ObjectFunction<OneMaxGO>(go => CheckIfAlwaysTrue(go)), (Terminal)")\n{\n",(NonTerminal)"code", (Terminal)"}"),
                    new ProductionRule((Terminal)"do\n{", (NonTerminal)"code", (Terminal)"}\nwhile(",new ObjectFunction<OneMaxGO>(go => CheckIfAlwaysTrue(go)), (Terminal)");"),
                    new ProductionRule(new ObjectFunction<OneMaxGO>(go => CreateForLoop(go)))
                }},
                { "unusedVariables", new List<ProductionRule>()
                {
                    new ProductionRule(new Terminal("d")),
                    new ProductionRule(new Terminal("e")),
                    new ProductionRule(new Terminal("f"))
                }}
            };
        }

        private static ProductionRule CheckValueWithinBounds(OneMaxGO go)
        {
            string value = go.ConvertToCode(new ProductionRule((NonTerminal)"value"));

            Tuple<int, bool> parsingResult = EvaluateExpression(value, go);
            int calculatedValue = parsingResult.Item1;
            bool canExecute = parsingResult.Item2;

            go.ConvertToCode(new ErrorCheck<OneMaxGO>(g => !canExecute || (calculatedValue >= 0 && calculatedValue < arraySize)));

            return new ProductionRule(new Terminal(value));
        }

        private static ProductionRule CheckNonZeroValue(OneMaxGO go)
        {
            string value = go.ConvertToCode(new ProductionRule((NonTerminal)"value"));

            Tuple<int, bool> parsingResult = EvaluateExpression(value, go);
            int calculatedValue = parsingResult.Item1;
            bool canExecute = parsingResult.Item2;

            go.ConvertToCode(new ErrorCheck<OneMaxGO>(g => !canExecute || calculatedValue != 0));
            return new ProductionRule(new Terminal(value));
        }
        private static Tuple<int, bool> EvaluateExpression(string code, GrammaticalObject go)
        {
            bool canExecute = !code.Contains("a") && !code.Contains("b") && !code.Contains("c") && !code.Contains("d") && !code.Contains("e") && !code.Contains("f");
            bool canParse = canExecute && code.Length == 1;
            ExecutionParameters parameters = new ExecutionParameters(genericCodeWrapper.Replace("TYPE", "int"), "CodeExecutor.Executor", "Execute");

            int calculatedValue = 0;
            if (canParse)
            {
                calculatedValue = int.Parse(code);
            }
            else if (canExecute)
            {
                try
                {
                    calculatedValue = (int)Executor.Execute(parameters, "return " + code + ";");
                }
                catch
                {
                    canExecute = false;
                }
            }

            return new Tuple<int, bool>(calculatedValue, canExecute);
        }

        private static ProductionRule CheckIfAlwaysTrue(OneMaxGO go)
        {
            string code = go.ConvertToCode((NonTerminal)"condition");
            bool canExecute = !code.Contains("a") && !code.Contains("b") && !code.Contains("c") && !code.Contains("d") && !code.Contains("e") && !code.Contains("f");
            ExecutionParameters parameters = new ExecutionParameters(genericCodeWrapper.Replace("TYPE", "bool"), "CodeExecutor.Executor", "Execute");

            if (canExecute)
            {
                bool result = (bool)Executor.Execute(parameters, "return !" + code + ";");
                go.ConvertToCode(new ErrorCheck(g => result));
                return new ProductionRule(new Terminal(code));
                //If the value is always false, there can't be an infine loop (although a loop that is never executed is useless)
            }
            else//If the code contains variables, by definition its value isn't constant
            {
                return new ProductionRule(new Terminal(code));
            }

        }

        private static ProductionRule CreateForLoop(OneMaxGO go)
        {
            if (go["unusedVariables"].Count == 0)
            {
                return new ProductionRule((NonTerminal)"loop");
            }
            string chosenVariable = go.ConvertToCode(new ProductionRule((NonTerminal)"unusedVariables"));
            OneMaxGO newGO = go.CreateChild<OneMaxGO>();
            ProductionRule variableProductionRule = newGO["unusedVariables"].Find(x => ((Terminal)x[0]).Code == chosenVariable);
            newGO["unusedVariables"].Remove(variableProductionRule);
            newGO["variable"].Add(variableProductionRule);

            return new ProductionRule(new Terminal("for(int " + chosenVariable + " = 0; " + chosenVariable + " < "), (NonTerminal)"value", new Terminal("; " + chosenVariable + "++)\n{\n"), new NonTerminal("code", g => newGO), (Terminal)"\n}\n");
        }
    }

    [Serializable]
    public class OneMaxGO_NoImprovements : GrammaticalObject, ISerializable
    {
        public const int arraySize = 64;
        public const string genericCodeWrapper = "namespace CodeExecutor{public class Executor{public TYPE Execute(){\n PECLECODE \n}}}";

        public override GrammaticalObject GetClone()
        {
            return new OneMaxGO_NoImprovements();
        }

        public OneMaxGO_NoImprovements() { }


        protected OneMaxGO_NoImprovements(SerializationInfo info, StreamingContext context)
        {
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        public override Dictionary<string, List<ProductionRule>> GetInitialProductionRules()
        {
            return new Dictionary<string, List<ProductionRule>>()
            {
                { "code", new List<ProductionRule>()
                {
                    new ProductionRule((NonTerminal)"code",(Terminal)"\n",(NonTerminal)"code"),
                    new ProductionRule((Terminal)"grid[", (NonTerminal)"value", (Terminal)"] = ", (NonTerminal)"condition", (Terminal)";"),
                    new ProductionRule((NonTerminal)"variable", (Terminal)" = ", (NonTerminal)"value", (Terminal)";"),
                    new ProductionRule((NonTerminal)"conditionalStatement"),
                    new ProductionRule((NonTerminal)"loop")
                }},
                { "variable", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"a"),
                    new ProductionRule((Terminal)"b"),
                    new ProductionRule((Terminal)"c")
                }},
                { "condition", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"true"),
                    new ProductionRule((Terminal)"false"),
                    new ProductionRule((Terminal)"(", (NonTerminal)"value", (NonTerminal)"compOp", (NonTerminal)"value", (Terminal)")"),
                    new ProductionRule((Terminal)"(", (NonTerminal)"condition", (Terminal)" || ", (NonTerminal)"condition", (Terminal)")"),
                    new ProductionRule((Terminal)"(", (NonTerminal)"condition", (Terminal)" && ", (NonTerminal)"condition", (Terminal)")"),
                    new ProductionRule((Terminal)"(!", (NonTerminal)"condition", (Terminal)")")
                }},
                { "compOp", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)">"),
                    new ProductionRule((Terminal)">="),
                    new ProductionRule((Terminal)"<"),
                    new ProductionRule((Terminal)"<="),
                    new ProductionRule((Terminal)"=="),
                    new ProductionRule((Terminal)"!=")
                }},
                { "value", new List<ProductionRule>()
                {
                    new ProductionRule((NonTerminal)"digit"),
                    new ProductionRule((NonTerminal)"variable"),
                    new ProductionRule((Terminal)arraySize.ToString()),
                    new ProductionRule((Terminal)"(",(NonTerminal)"mathExpression",(Terminal)")")
                }},
                { "digit", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"0"),
                    new ProductionRule((Terminal)"1"),
                    new ProductionRule((Terminal)"2"),
                    new ProductionRule((Terminal)"3"),
                    new ProductionRule((Terminal)"4"),
                    new ProductionRule((Terminal)"5"),
                    new ProductionRule((Terminal)"6"),
                    new ProductionRule((Terminal)"7"),
                    new ProductionRule((Terminal)"8"),
                    new ProductionRule((Terminal)"9")
                }},
                { "mathExpression", new List<ProductionRule>()
                {
                    new ProductionRule((NonTerminal)"value", (Terminal)" + ", (NonTerminal)"value"),
                    new ProductionRule((NonTerminal)"value", (Terminal)" - ", (NonTerminal)"value"),
                    new ProductionRule((NonTerminal)"value", (Terminal)" * ", (NonTerminal)"value"),
                    new ProductionRule((NonTerminal)"value", (Terminal)" / ", (NonTerminal)"value"),
                    new ProductionRule((NonTerminal)"value", (Terminal)" % ", (NonTerminal)"value")
                }},
                { "conditionalStatement", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"if(", (NonTerminal)"condition", (Terminal)")\n{\n",(NonTerminal)"code", (Terminal)"}"),
                    new ProductionRule((Terminal)"if(", (NonTerminal)"condition", (Terminal)")\n{\n", (NonTerminal)"code", (Terminal)"}\n else \n{\n",(NonTerminal)"code", (Terminal)"\n}"),
                    new ProductionRule((Terminal)"if(", (NonTerminal)"condition", (Terminal)")\n{\n", (NonTerminal)"code", (Terminal)"}\n else if(", (NonTerminal)"condition", (Terminal)")\n{\n", (NonTerminal)"code", (Terminal)"\n}")
                }},
                { "loop", new List<ProductionRule>()
                {
                    new ProductionRule((Terminal)"while(",(NonTerminal)"condition",(Terminal)")\n{\n",(NonTerminal)"code",(Terminal)"}"),
                    new ProductionRule((Terminal)"do\n{", (NonTerminal)"code", (Terminal)"}while(",(NonTerminal)"condition",(Terminal)");"),
                    new ProductionRule(new ObjectFunction<OneMaxGO_NoImprovements>(go => CreateForLoop(go)))
                }},
                { "unusedVariables", new List<ProductionRule>()
                {
                    new ProductionRule(new Terminal("d")),
                    new ProductionRule(new Terminal("e")),
                    new ProductionRule(new Terminal("f"))
                }}
            };
        }

        private static ProductionRule CreateForLoop(OneMaxGO_NoImprovements go)
        {
            if (go["unusedVariables"].Count == 0)
            {
                return new ProductionRule((NonTerminal)"loop");
            }
            string chosenVariable = go.ConvertToCode(new ProductionRule((NonTerminal)"unusedVariables"));
            OneMaxGO_NoImprovements newGO = go.CreateChild<OneMaxGO_NoImprovements>();
            ProductionRule variableProductionRule = newGO["unusedVariables"].Find(x => ((Terminal)x[0]).Code == chosenVariable);
            newGO["unusedVariables"].Remove(variableProductionRule);
            newGO["variable"].Add(variableProductionRule);

            return new ProductionRule(new Terminal("for(int " + chosenVariable + " = 0; " + chosenVariable + " < "), (NonTerminal)"value", new Terminal("; " + chosenVariable + "++)\n{\n"), new NonTerminal("code", g => newGO), (Terminal)"\n}\n");
        }
    }
}
