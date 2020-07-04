using System;


namespace ToolGood.Algorithm.LitJson
{
    class JsonException : ApplicationException
    {

        internal JsonException(ParserToken token, Exception inner_exception) :
            base(String.Format("Invalid token '{0}' in input string", token), inner_exception)
        {
        }

        internal JsonException(int c) :
            base(String.Format("Invalid character '{0}' in input string", (char)c))
        {
        }


        public JsonException(string message) : base(message)
        {
        }

    }
}
