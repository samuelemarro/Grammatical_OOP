using System;
using System.Collections;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BetterPECLE_v3
{
    class Program
    {
        static void Main(string[] args)
        {


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
            ExtractData(250, 1, @"C:\Users\Samuele\Documents\BetterPECLE\v3", 20, 50, 20, 100);
            /*List<GeneticAlgorithmResult> results = OpenResults(@"C:\Users\Samuele\Documents\BetterPECLE\v3\T\PECLE");

            double initialMaxFitness = results.Select(x => x.stats[0].MaxFitness).Average();
            double initialMinFitness = results.Select(x => x.stats[0].MinFitness).Average();
            double finalMaxFitness = results.Select(x => x.stats[49].MaxFitness).Average();
            double finalMinFitness = results.Select(x => x.stats[49].MinFitness).Average();

            double averageDifference = results.Select(x => x.stats[49].MaxFitness - x.stats[0].MaxFitness).Average();

            List<List<double>> relativeErrors = results.Select(x => x.stats.Select(y => (y.executionExceptions + y.compilationErrors + y.failedErrorCorrections) / y.executedEvaluations).ToList()).ToList();
            List<double> relativeErrors2 = relativeErrors.Select(x => x.Average()).ToList();
            double finalRelativeError = relativeErrors2.Average();
            */
            /*double errorImprovement = results.Select(x => (x.stats[49].generationErrors / x.stats[49].executedEvaluations) - (x.stats[0].generationErrors / x.stats[0].executedEvaluations)).Average();
            
            GeneticAlgorithmResult averageResult = AverageResults(results);*/
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

        private static void ExtractData(int executionNumber, int partitionSize, string path, int populationSize, int generationNumber, int initialChromosomeSize, int maximumChromosomeSize)
        {
            List<GeneticAlgorithmResult> pecleResults = new List<GeneticAlgorithmResult>();
            List<GeneticAlgorithmResult> pecleResults_NoImprovements = new List<GeneticAlgorithmResult>();
            int executionCount = 0;
            for (int i = 0; i < executionNumber; i++)
            {

                executionCount++;
                int elitismSize = GrammaticalEvolution.random.Next(0, 5);
                int tournamentSize = GrammaticalEvolution.random.Next(2, 6);
                double crossoverProbability = GrammaticalEvolution.random.NextDouble();
                double mutationProbability = GrammaticalEvolution.random.NextDouble();
                double duplicationProbability = GrammaticalEvolution.random.NextDouble();
                double pruningProbability = GrammaticalEvolution.random.NextDouble();

                pecleResults.Add(PECLETest(populationSize, generationNumber, initialChromosomeSize, maximumChromosomeSize, elitismSize, tournamentSize, crossoverProbability, mutationProbability, duplicationProbability, pruningProbability));
                pecleResults_NoImprovements.Add(PECLETest_NoImprovements(populationSize, generationNumber, initialChromosomeSize, maximumChromosomeSize, elitismSize, tournamentSize, crossoverProbability, mutationProbability, duplicationProbability, pruningProbability));


                if (executionCount == partitionSize)
                {
                    WriteToBinaryFile(path + "\\PECLE\\" + (i / partitionSize) + ".pecle", pecleResults);
                    WriteToBinaryFile(path + "\\Default\\" + (i / partitionSize) + ".pecle", pecleResults_NoImprovements);

                    pecleResults = new List<GeneticAlgorithmResult>();
                    pecleResults_NoImprovements = new List<GeneticAlgorithmResult>();

                    executionCount = 0;
                }

            }
        }

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
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
    }

    [Serializable]
    public class OneMaxGO : GrammaticalObject, ISerializable
    {
        public int codeCounter;
        public int conditionCounter;
        public int valueCounter;

        public const int maxSelfReferenceDepth = 5;
        public const int arraySize = 64;
        public const string genericCodeWrapper = "namespace CodeExecutor{public class Executor{public TYPE Execute(){\n PECLECODE \n}}}";

        public override GrammaticalObject GetClone()
        {
            OneMaxGO go = new OneMaxGO(codeCounter, conditionCounter, valueCounter);

            return go;
        }

        public OneMaxGO() { }

        public OneMaxGO(int codeCounter, int conditionCounter, int valueCounter)
        {
            this.codeCounter = codeCounter;
            this.conditionCounter = conditionCounter;
            this.valueCounter = valueCounter;
        }

        protected OneMaxGO(SerializationInfo info, StreamingContext context)
        {
            codeCounter = info.GetInt32("codeCounter");
            conditionCounter = info.GetInt32("conditionCounter");
            valueCounter = info.GetInt32("valueCounter");
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("codeCounter", codeCounter);
            info.AddValue("conditionCounter", conditionCounter);
            info.AddValue("valueCounter", valueCounter);
        }

        public override Dictionary<string, List<ProductionRule>> GetInitialProductionRules()
        {
            return new Dictionary<string, List<ProductionRule>>()
            {
                { "code", new List<ProductionRule>()
                {
                    new ProductionRule(new ObjectAction<OneMaxGO>(go => go.codeCounter++),new ErrorCheck<OneMaxGO>(go => go.codeCounter <= maxSelfReferenceDepth), new NonTerminal("codeImplementation", (go => go.CreateChild<OneMaxGO>())), new ObjectAction<OneMaxGO>(go => go.codeCounter--))
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
                    new ProductionRule(new ObjectAction<OneMaxGO>(go => go.conditionCounter++), new ErrorCheck<OneMaxGO>(go => go.conditionCounter < maxSelfReferenceDepth), (NonTerminal)"conditionImplementation",new ObjectAction<OneMaxGO>(go => go.conditionCounter--))
                }},
                { "conditionImplementation", new List<ProductionRule>()
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
                    new ProductionRule(new ObjectAction<OneMaxGO>(go => go.valueCounter++), new ErrorCheck<OneMaxGO>(go => go.valueCounter < maxSelfReferenceDepth), (NonTerminal)"valueImplementation",new ObjectAction<OneMaxGO>(go => go.valueCounter--))
                }},
                { "valueImplementation", new List<ProductionRule>()
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
                    new ProductionRule((Terminal)"while(",(NonTerminal)"condition",(Terminal)")\n{\n",(NonTerminal)"code",(Terminal)"}"),
                    new ProductionRule((Terminal)"do\n{", (NonTerminal)"code", (Terminal)"}while(",(NonTerminal)"condition",(Terminal)");"),
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

        private static ProductionRule CreateForLoop(OneMaxGO go)
        {
            go.ConvertToCode(new ProductionRule(new ErrorCheck<OneMaxGO>(g => g["unusedVariables"].Count > 0)));//Check that it is possible to use a variable
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
