using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterPECLE_v3
{
    [Serializable]
    public abstract class GrammaticalObject : Dictionary<string, List<ProductionRule>>
    {
        public abstract Dictionary<string, List<ProductionRule>> GetInitialProductionRules();

        public abstract GrammaticalObject GetClone();

        internal GrammaticalObject Parent { get; private set; }

        internal GrammaticalEvolution instance;

        internal int FirstUsedIndex
        {
            get
            {
                return firstUsedIndex;
            }
            set
            {
                firstUsedIndex = value;
                if (Parent != null && Parent.FirstUsedIndex == -1)
                    Parent.FirstUsedIndex = value;
            }
        }

        internal int LastUsedIndex
        {
            get
            {
                return lastUsedIndex;
            }
            set
            {
                lastUsedIndex = value;
                if (Parent != null)
                    Parent.LastUsedIndex = value;
            }
        }

        private int firstUsedIndex = -1;
        private int lastUsedIndex = -1;

        public GrammaticalObject() : base() { }
        
        internal void Initialize(Dictionary<string,List<ProductionRule>> productionRules)
        {
            foreach(var kvp in productionRules)
            {
                base.Add(kvp.Key, kvp.Value.ToList());
            }
        }
        
        public T CreateChild<T>() where T : GrammaticalObject
        {
            GrammaticalObject blueprintGO = instance.BluePrintGOs.Find(x=> x.GetType() == typeof(T));

            if (blueprintGO == null)
                throw new BluePrintGONotFound(typeof(T));

            GrammaticalObject go = blueprintGO.GetClone();
            go.instance = instance;
            go.Initialize(this);//Creates a copy that has the same production rules as the parent

            go.Parent = this;

            return (T)go;
        }

        public string ConvertToCode(ProductionRule productionRule)
        {
            return instance.ConvertToCode(productionRule, this);
        }

        public string ConvertToCode(params Expression[] expressions)
        {
            return ConvertToCode(new ProductionRule(expressions));
        }

        internal List<GrammaticalObject> GetStackTrace()
        {
            List<GrammaticalObject> stackTrace = new List<GrammaticalObject>();

            stackTrace.Add(this);
            if (Parent != null)
                stackTrace.AddRange(Parent.GetStackTrace());

            return stackTrace;
        }
        

        public class BluePrintGONotFound : Exception
        {
            public BluePrintGONotFound(Type type) : base("Could not find a blueprint for a Grammatical Object of type \"" + type.FullName + "\".") { }
        }
    }
}
