using System;

namespace BetterPECLE_v3
{
    [Serializable]
    public class Terminal : Expression
    {
        public string Code { get; }

        public Terminal(string code)
        {
            Code = code;
        }

        public static explicit operator Terminal(string code)
        {
            return new Terminal(code);
        }
        public static explicit operator string(Terminal t)
        {
            return t.Code;
        }

        public override string ToString()
        {
            return Code;
        }

    }
}
