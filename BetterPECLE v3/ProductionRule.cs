using System;
using System.Collections.Generic;

namespace Grammatical_OOP
{
    [Serializable]
    public class ProductionRule : List<Expression>
    {
        public ProductionRule(params Expression[] expressions)
        {
            base.AddRange(expressions);
        }
    }
}
