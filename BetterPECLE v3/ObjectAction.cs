using System;

namespace BetterPECLE_v3
{
    public class ObjectAction : Expression
    {
        public Action<GrammaticalObject> Lambda { get; protected set; }
        public ObjectAction(Action<GrammaticalObject> lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException("lambda");
            Lambda = lambda;
        }

        internal void Execute(GrammaticalObject go)
        {
            Lambda(go);
        }
    }

    public class ObjectAction<T> : ObjectAction where T : GrammaticalObject
    {
        public ObjectAction(Action<T> lambda) : base(ConvertAction(lambda)) { }
        
        private static Action<GrammaticalObject> ConvertAction(Action<T> originalAction)
        {
            return new Action<GrammaticalObject>(o => originalAction((T)o));
        }
    }
}
