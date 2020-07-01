using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using static mathParser;

namespace ToolGood.Algorithm
{
    public class AlgorithmEngine
    {
        /// <summary>
        /// 最后一个错误
        /// </summary>
        public string LastError { get; private set; }

        private ProgContext ProgContext;

        /// <summary>
        /// 编译公式，默认
        /// </summary>
        /// <param name="exp">公式</param>
        /// <returns></returns>
        public bool Parse(string exp)
        {
            if (string.IsNullOrEmpty(exp) || exp.Trim() == "") {
                LastError = "exp无效";
                return false;
            }
            try {
                ProgContext = null;

                var stream = new CaseChangingCharStream(new AntlrInputStream(exp), true);
                var lexer = new mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new mathParser(tokens);
                ProgContext = parser.prog();

                return true;
            } catch (Exception ex) {
                LastError = ex.Message;
                return false;
            }
        }

        public Operand Evaluate()
        {
            if (ProgContext == null) {
                LastError = "请编译公式！";
                throw new Exception("请编译公式！");
            }
            var visitor = new MathVisitor();
            return visitor.Visit(ProgContext);
        }



    }
}
