using Antlr4.Runtime;
using System;
using static mathParser;

namespace ToolGood.Algorithm.Internals
{
    internal class ConditionCacheInfo
    {

        public string CategoryName;

        public string Remark;

        public string ConditionString;

        private ProgContext _ConditionProg;
        public ProgContext ConditionProg {
            get {
                if (_ConditionProg == null && string.IsNullOrEmpty(ConditionString) == false && string.IsNullOrEmpty(LastError)) {
                    _ConditionProg = Parse(ConditionString);
                }
                return _ConditionProg;
            }
            set { _ConditionProg = value; }
        }

        public string FormulaString;
        private ProgContext _FormulaProg;
        public ProgContext FormulaProg {
            get {
                if (_FormulaProg == null && string.IsNullOrEmpty(FormulaString) == false && string.IsNullOrEmpty(LastError)) {
                    _FormulaProg = Parse(FormulaString);
                }
                return _FormulaProg;
            }
            set { _FormulaProg = value; }
        }


        public string LastError;
        private ProgContext Parse(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp)) { return null; }
            try {
                var stream = new AntlrCharStream(new AntlrInputStream(exp));
                var lexer = new mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new mathParser(tokens);
                var antlrErrorListener = new AntlrErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(antlrErrorListener);

                var context = parser.prog();
                if (antlrErrorListener.IsError) {
                    LastError = antlrErrorListener.ErrorMsg;
                    return null;
                }
                return context;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return null;
        }
    }
}
