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
            if (string.IsNullOrEmpty(exp) || exp.Trim() == "")
            {
                LastError = "exp无效";
                return false;
            }
            try
            {
                ProgContext = null;

                var stream = new CaseChangingCharStream(new AntlrInputStream(exp), true);
                var lexer = new mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new mathParser(tokens);
                ProgContext = parser.prog();

                return true;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }

        public Operand Evaluate()
        {
            if (ProgContext == null)
            {
                LastError = "请编译公式！";
                throw new Exception("请编译公式！");
            }
            var visitor = new MathVisitor();
            return visitor.Visit(ProgContext);
        }


        #region TryEvaluate

        public int TryEvaluate(string exp, int def)
        {
            if (Parse(exp))
            {
                try
                {
                    var obj = Evaluate();
                    obj = obj.ToNumber();
                    if (obj.IsError)
                        return def;
                    return obj.IntValue;
                }
                catch (Exception) { }
            }
            return def;
        }

        public double TryEvaluate(string exp, double def)
        {
            if (Parse(exp))
            {
                try
                {
                    var obj = Evaluate();
                    obj = obj.ToNumber();
                    if (obj.IsError)
                        return def;
                    return obj.NumberValue;
                }
                catch (Exception) { }
            }
            return def;
        }

        public string TryEvaluate(string exp, string def)
        {
            if (Parse(exp))
            {
                try
                {
                    var obj = Evaluate();
                    obj = obj.ToString();
                    if (obj.IsError)
                        return def;
                    return obj.StringValue;
                }
                catch (Exception) { }
            }
            return def;
        }
        public bool TryEvaluate(string exp, bool def)
        {
            if (Parse(exp))
            {
                try
                {
                    var obj = Evaluate();
                    obj = obj.ToBoolean();
                    if (obj.IsError)
                        return def;
                    return obj.BooleanValue;
                }
                catch (Exception) { }
            }
            return def;
        }

        public DateTime TryEvaluate(string exp, DateTime def)
        {
            if (Parse(exp))
            {
                try
                {
                    var obj = Evaluate();
                    obj = obj.ToBoolean();
                    if (obj.IsError)
                        return def;
                    return (DateTime) obj.DateValue;
                }
                catch (Exception) { }
            }
            return def;
        }

        public TimeSpan TryEvaluate(string exp, TimeSpan def)
        {
            if (Parse(exp))
            {
                try
                {
                    var obj = Evaluate();
                    obj = obj.ToBoolean();
                    if (obj.IsError)
                        return def;
                    return (TimeSpan) obj.DateValue;
                }
                catch (Exception) { }
            }
            return def;
        }
        #endregion
    }
}
