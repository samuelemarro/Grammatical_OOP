using System;

namespace Grammatical_OOP
{
    public class ObjectFunction : Expression
    {
        public Func<GrammaticalObject, ProductionRule> Lambda { get; }

        public ObjectFunction(Func<GrammaticalObject, ProductionRule> lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException("lambda");
            Lambda = lambda;
        }

        internal ProductionRule Execute(GrammaticalObject go)
        {
            return Lambda(go);
        }
    }

    public class ObjectFunction<T> : ObjectFunction where T : GrammaticalObject
    {
        public ObjectFunction(Func<T, ProductionRule> lambda) : base(ConvertFunction(lambda)) { }
    
        private static Func<GrammaticalObject, ProductionRule> ConvertFunction(Func<T, ProductionRule> originalFunction)
        {
            return new Func<GrammaticalObject, ProductionRule>(o => originalFunction((T)o));
        }
    }
}
