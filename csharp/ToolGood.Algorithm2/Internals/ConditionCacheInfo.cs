using Antlr4.Runtime;
using System;
using static mathParser;

namespace ToolGood.Algorithm.Internals
{
    /// <summary>
    /// 条件缓存
    /// </summary>
    public class ConditionCacheInfo
    {
        /// <summary>
        /// 类型名
        /// </summary>
        public string CategoryName { get; internal set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; internal set; }
        /// <summary>
        /// 条件字符串
        /// </summary>
        public string ConditionString { get; internal set; }

        private ProgContext _ConditionProg;
        internal ProgContext ConditionProg {
            get {
                if (_ConditionProg == null && string.IsNullOrEmpty(ConditionString) == false && string.IsNullOrEmpty(LastError)) {
                    _ConditionProg = Parse(ConditionString);
                }
                return _ConditionProg;
            }
            set { _ConditionProg = value; }
        }
        /// <summary>
        /// 工式字符串
        /// </summary>
        public string FormulaString;
        private ProgContext _FormulaProg;
        internal ProgContext FormulaProg {
            get {
                if (_FormulaProg == null && string.IsNullOrEmpty(FormulaString) == false && string.IsNullOrEmpty(LastError)) {
                    _FormulaProg = Parse(FormulaString);
                }
                return _FormulaProg;
            }
            set { _FormulaProg = value; }
        }

        /// <summary>
        /// 最后错误信息
        /// </summary>
        public string LastError;
        private ProgContext Parse(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp)) { return null; }
            try {
                var stream = new AntlrCharStream(new AntlrInputStream(exp));
                var lexer = new mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new mathParser(tokens);
                //var antlrErrorListener = new AntlrErrorListener();
                //parser.RemoveErrorListeners();
                //parser.AddErrorListener(antlrErrorListener);

                var context = parser.prog();
                //if (antlrErrorListener.IsError) {
                //    LastError = antlrErrorListener.ErrorMsg;
                //    return null;
                //}
                return context;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return null;
        }
    }
}
