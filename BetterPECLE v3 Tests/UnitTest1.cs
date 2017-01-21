using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterPECLE_v3;
using System.Collections.Generic;

namespace BetterPECLE_v3_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            GrammaticalEvolution ge = new GrammaticalEvolution();
            ge.BluePrintGOs = new List<GrammaticalObject>() { new OneMaxGO() };
            ge.StartingGO = new OneMaxGO();
            ge.StartingProductionRule = new ProductionRule(new NonTerminal("code"));

            Chromosome chromosome = new Chromosome(0, 40);
            chromosome.AddRange(new byte[] { 4, 2, 0, 2, 1, 1, 3, 0 });

            //string code = ge.Generate(chromosome, new GenerationStats());
        }
    }
}
