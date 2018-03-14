using System;

namespace Grammatical_OOP
{
    public class ErrorCheck : Expression
    {
        public Func<GrammaticalObject, bool> Lambda { get; }

        public ErrorCheck(Func<GrammaticalObject, bool> lambda)
        {
            Lambda = lambda;
        }

        public void Check(GrammaticalObject go)
        {
            if (!Lambda(go))
                throw new ErrorCheckFailException(go, this);
        }

        [Serializable]
        public class ErrorCheckFailException : Exception
        {
            internal GrammaticalObject exceptionGO;
            internal ErrorCheck failedEC;
            public ErrorCheckFailException(GrammaticalObject exceptionGO, ErrorCheck failedEC) : base("Failed Error Check in GrammaticalObject \"" + exceptionGO.ToString() + "\".")
            {
                this.exceptionGO = exceptionGO;
                this.failedEC = failedEC;
            }
        }
    }

    public class ErrorCheck<T> : ErrorCheck where T : GrammaticalObject
    {
        public ErrorCheck(Func<T, bool> lambda) : base(ConvertErrorCheck(lambda)) { }
        private static Func<GrammaticalObject, bool> ConvertErrorCheck(Func<T, bool> originalErrorCheck)
        {
            return new Func<GrammaticalObject, bool>(o => originalErrorCheck((T)o));
        }
    }
}
