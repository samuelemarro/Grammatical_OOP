using System;

namespace BetterPECLE_v3
{
    [Serializable]
    public class NonTerminal : Expression
    {
        public string NewNonTerminalName { get; }
        public Func<GrammaticalObject, GrammaticalObject> NewGrammaticalObjectLambda { get; }

        public NonTerminal(string newNonTerminalName)
        {
            NewNonTerminalName = newNonTerminalName;
        }
        /// <summary>
        /// Instantiates a new <see cref="NonTerminal"/> with a reference to another <see cref="GrammaticalObject"/>.
        /// </summary>
        /// <param name="newNonTerminalName">The name of the new referenced nonterminal.</param>
        /// <param name="newGrammaticalObjectLambda">The lambda that will be evaluated to choose the <see cref="GrammaticalObject"/>. The argument of the function is the <see cref="GrammaticalObject"/> where the <see cref="ProductionRule"/> is being evaluated.</param>
        public NonTerminal(string newNonTerminalName, Func<GrammaticalObject, GrammaticalObject> newGrammaticalObjectLambda)
        {
            NewNonTerminalName = newNonTerminalName;
            NewGrammaticalObjectLambda = newGrammaticalObjectLambda;
        }

        public static explicit operator NonTerminal(string newNonTerminalName)
        {
            return new NonTerminal(newNonTerminalName);
        }

        internal string ToCode(GrammaticalObject go)
        {
            int moddedCodon = 0;

            if (go[NewNonTerminalName] == null || go[NewNonTerminalName].Count == 0)
                throw new EmptyProductionRuleCollection(NewNonTerminalName, go);

            if (go[NewNonTerminalName].Count > 1)
            {
                byte codon = go.instance.NextCodon();

                if (go.FirstUsedIndex == -1)
                    go.FirstUsedIndex = go.instance.codonPosition;
                go.LastUsedIndex = go.instance.codonPosition;

                moddedCodon = (int)codon % go[NewNonTerminalName].Count;
            }

            ProductionRule chosenProductionRule = go[NewNonTerminalName][moddedCodon];

            return go.ConvertToCode(chosenProductionRule);
        }

        [Serializable]
        public class EmptyProductionRuleCollection : Exception
        {
            public EmptyProductionRuleCollection(string nonTerminalName, GrammaticalObject go) : base("The nonterminal \"" + nonTerminalName + "\" in Grammatical Object \"" + go.GetType().FullName + "\" is empty or null.") { }
        }

    }
}
