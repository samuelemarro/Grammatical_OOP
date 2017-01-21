using System;
using System.Collections.Generic;

namespace BetterPECLE_v3
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
