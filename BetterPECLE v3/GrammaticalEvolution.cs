using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterPECLE_v3
{
    [Serializable]
    public class GrammaticalEvolution : ICloneable
    {
        public ProductionRule StartingProductionRule { get; set; }

        public GrammaticalObject StartingGO { get; set; }

        public List<GrammaticalObject> BluePrintGOs { get; set; }

        private Chromosome _genome;
        private GenerationStats _currentGenerationStats;

        internal int codonPosition = -1;

        internal static Random random = new Random();

        internal Tuple<string, bool> Generate(Chromosome genome, GenerationStats currentGenerationStats)
        {
            _genome = genome;
            _currentGenerationStats = currentGenerationStats;

            string code = "";

            bool compileAgain = true;
            bool correctionApplied = false;

            while (compileAgain)
            {
                try
                {
                    codonPosition = -1;
                    GrammaticalObject go = (GrammaticalObject)Activator.CreateInstance(StartingGO.GetType());
                    go.instance = this;
                    go.Initialize(go.GetInitialProductionRules());
                    code = ConvertToCode(StartingProductionRule, go);
                    break;
                }
                catch (ErrorCheck.ErrorCheckFailException e)
                {
                    compileAgain = false;
                    List<GrammaticalObject> stackTrace = e.exceptionGO.GetStackTrace();

                    _currentGenerationStats.failedErrorChecks++;

                    for (int stackTracePosition = 0; stackTracePosition < stackTrace.Count && !compileAgain; stackTracePosition++)
                    {
                        int initialPosition = stackTrace[stackTracePosition].FirstUsedIndex;
                        int finalPosition = stackTrace[stackTracePosition].LastUsedIndex;

                        if (CanCorrectChromosome(_genome, initialPosition, finalPosition))
                        {
                            compileAgain = true;
                            correctionApplied = true;
                            CorrectChromosome(_genome, initialPosition, finalPosition);
                        }
                    }
                    if (!compileAgain)
                    {
                        _currentGenerationStats.failedErrorCorrections++;
                        throw new ErrorCorrectionFailedException();
                    }
                }
            }

            if (correctionApplied)
                _currentGenerationStats.successfulErrorCorrections++;
            return new Tuple<string, bool>(code, correctionApplied);
        }

        private static bool CanCorrectChromosome(Chromosome chromosome, int initialPosition, int finalPosition)
        {
            int subArraySize = finalPosition - initialPosition + 1;

            //If there are not any backupCodons, the correction cannot be performed
            if (chromosome.BackupCodons == null)
                return false;

            //If the subAarray of the backupCodons has a length of zero or less, it cannot be used
            if (initialPosition >= chromosome.BackupCodons.Count)
                return false;
            //If the subArray of the backupCodons is shorter than the one of the genome, they are necessarily different
            if (subArraySize > chromosome.BackupCodons.Count - initialPosition)
                return true;

            //If both subArrays are equally long, check for differences between the elements
            byte[] subCodons = chromosome.Skip(initialPosition).Take(subArraySize).ToArray();
            byte[] subBackupCodons = chromosome.BackupCodons.Skip(initialPosition).Take(subArraySize).ToArray();

            return !subCodons.SequenceEqual(subBackupCodons);
        }

        private static void CorrectChromosome(Chromosome chromosome, int initialPosition, int finalPosition)
        {
            //There are three possible cases: the subArray of the codons and the backupCodons have
            //the same length(case 1), the one of the codons is longer(case 2) and the one of the backupCodons is longer(case 3).

            int codonsSize = finalPosition - initialPosition + 1;
            int backupCodonsSize = chromosome.BackupCodons.Count - initialPosition;

            //Case 1 and 3:
            /*
            Before:
              Codons: ABCDEF|GHI|J
              Backup: KLMNOP|QRS|T[...]

            After:
              Codons: ABCDEF|QRS|J 
              Backup: KLMNOP|QRS|T[..]
            */

            //Case 2:
            /*
            Before:
             Codons: ABCDEF|GHI|J
             Backup: KLMNOP|QR |

            After:
              Codons: ABCDEF|QR|
              Backup: KLMNOP|QR|

            Note: anything in the codons after the chosen piece has been removed.
            */

            if (codonsSize >= backupCodonsSize)//Case 2
            {
                for (int i = initialPosition; i <= finalPosition && i < chromosome.BackupCodons.Count; i++)
                {
                    chromosome[i] = chromosome.BackupCodons[i];
                }
                while (chromosome.Count > chromosome.BackupCodons.Count)
                {
                    chromosome.RemoveAt(chromosome.Count - 1);
                }
            }
            else//Case 1 and 3
            {
                while (chromosome.Count > initialPosition)
                {
                    chromosome.RemoveAt(chromosome.Count - 1);
                }
                for (int i = initialPosition; i <= finalPosition; i++)
                {
                    chromosome.Add(chromosome.BackupCodons[i]);
                }
            }

        }

        internal string ConvertToCode(ProductionRule productionRule, GrammaticalObject go)
        {
            string finalCode = "";

            foreach (Expression expr in productionRule)
            {
                if (expr is Terminal)
                    finalCode += (Terminal)expr;
                else if (expr is NonTerminal)
                {
                    NonTerminal nonTerminal = (NonTerminal)expr;

                    if (nonTerminal.NewGrammaticalObjectLambda == null)
                        finalCode += nonTerminal.ToCode(go);
                    else
                        finalCode += nonTerminal.ToCode(nonTerminal.NewGrammaticalObjectLambda(go));

                }
                else if (expr is ErrorCheck)
                {
                    ((ErrorCheck)expr).Check(go);
                    //If an exception is not thrown, the check was passed
                    _currentGenerationStats.passedErrorChecks++;
                }
                else if (expr is ObjectAction)
                    (((ObjectAction)expr)).Execute(go);
                else if (expr is ObjectFunction)
                    finalCode += ConvertToCode(((ObjectFunction)expr).Execute(go), go);
                else
                    throw new NotImplementedException();
            }
            return finalCode;
        }

        public byte NextCodon()
        {
            codonPosition++;
            _genome.LastUsedCodonPosition = codonPosition;

            if (codonPosition >= _genome.Count)
            {
                throw new EndOfCodonQueueException();
            }

            return _genome[codonPosition];
        }

        public object Clone()
        {
            return GetClone();
        }

        public GrammaticalEvolution GetClone()
        {
            GrammaticalEvolution ge = new GrammaticalEvolution();
            if (BluePrintGOs != null)
            {
                ge.BluePrintGOs = new List<GrammaticalObject>();
                foreach (GrammaticalObject go in BluePrintGOs)
                {
                    ge.BluePrintGOs.Add(go.GetClone());
                }
            }
            ge.codonPosition = codonPosition;
            ge.StartingGO = StartingGO == null ? null : StartingGO.GetClone();
            ge.StartingProductionRule = StartingProductionRule == null ? null : new ProductionRule(StartingProductionRule.ToArray());
            ge._currentGenerationStats = _currentGenerationStats == null ? null : _currentGenerationStats.GetClone();
            ge._genome = _genome == null ? null : _genome.GetClone();
            return ge;
        }

        [Serializable]
        public class ErrorCorrectionFailedException : Exception
        {
            public ErrorCorrectionFailedException() : base("Failed to correct the genome.") { }
        }

        public class EndOfCodonQueueException : Exception
        {
            public EndOfCodonQueueException() : base("Reached the end of the codon queue.") { }
        }

    }
}
