using Antlr4.Runtime;
using System;
using static mathParser;

namespace ToolGood.Algorithm.Internals
{
    internal class ConditionCache
    {

        public string CategoryName;

        public string Remark;

        public string ConditionString;

        private ProgContext _Condition;
        public ProgContext Condition {
            get {
                if (_Condition == null && string.IsNullOrEmpty(ConditionString) == false && string.IsNullOrEmpty(LastError)) {
                    _Condition = Parse(ConditionString);
                }
                return _Condition;
            }
            set { _Condition = value; }
        }

        public string FormulaString;
        private ProgContext _Formula;
        public ProgContext Formula {
            get {
                if (_Formula == null && string.IsNullOrEmpty(FormulaString) == false && string.IsNullOrEmpty(LastError)) {
                    _Formula = Parse(FormulaString);
                }
                return _Formula;
            }
            set { _Formula = value; }
        }


        public string LastError;
        private ProgContext Parse(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp)) { return null; }
            try {
                var stream = new CaseChangingCharStream(new AntlrInputStream(exp));
                var lexer = new mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new mathParser(tokens);
                var antlrErrorListener = new AntlrErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(antlrErrorListener);

                var context = parser.prog();
                var end = context.Stop.StopIndex;
                if (end + 1 < exp.Length) {

                    LastError = "Parameter exp invalid !";
                    return null;
                }
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
