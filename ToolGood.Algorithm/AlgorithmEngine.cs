using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    public partial class AlgorithmEngine
    {
        List<string> m_Operators = new List<string>() { "(", ")", "!", "*", "/", "%", "+", "-", "<", ">", "=", "&", "|", ",", "==", "!=", ">=", "<=", "<>", "@" };
        Stack<object> m_tokens = new Stack<object>();        //最终逆波兰式堆栈
        Dictionary<String, Func<List<Operand>, Operand>> funcDict = new Dictionary<string, Func<List<Operand>, Operand>>();

        public AlgorithmEngine()
        {
            AddDateTimeFunction();
            AddFlowFunction();
            AddMathFunction();
            AddStringFunction();
            AddSumFunction();

        }
        private void addFunc(string name, Func<List<Operand>, Operand> func)
        {
            name = name.ToLower();
            m_Operators.Add(name);
            funcDict[name] = func;
        }



        #region 可重写的方法

        protected virtual Operand GetParameter(Operand curOpd)
        {
            return null;
        }

        #endregion


        #region Parse

        public bool Parse(string exp)
        {
            m_tokens.Clear();//清空语法单元堆栈
            if (string.IsNullOrEmpty(exp) || exp.Trim() == "" || !this.isMatching(exp)) {
                return false;
            }

            Stack<object> operands = new Stack<object>();             //操作数堆栈
            Stack<Operator> operators = new Stack<Operator>();      //运算符堆栈
            OperatorType optType = OperatorType.ERR;                //运算符类型
            string curOpd = "";                                 //当前操作数
            string curOpt = "";                                 //当前运算符
            int curPos = 0;                                     //当前位置
            int funcCount = 0;                                        //函数数量

            var texts = splitText(exp);
            texts.Add("@");
            corrections(texts);

            while (curPos < texts.Count) {
                //获取 当前操作数
                curOpd = getOperand(texts, curPos);
                if (curOpd != "") {
                    operands.Push(new Operand(curOpd));
                    curPos++;
                }
                // 获取 当前运算符
                curOpt = texts[curPos++];
                if (curOpt == "@") break;

                #region 解释括号
                //若当前运算符为左括号,则直接存入堆栈。
                if (curOpt == "(") {
                    operators.Push(new Operator(OperatorType.LB, "("));
                    continue;
                }

                //若当前运算符为右括号,则依次弹出运算符堆栈中的运算符并存入到操作数堆栈,直到遇到左括号为止,此时抛弃该左括号.
                if (curOpt == ")") {
                    while (operators.Count > 0) {
                        if (operators.Peek().Type != OperatorType.LB) {
                            operands.Push(operators.Pop());
                        } else {
                            operators.Pop();
                            break;
                        }
                    }
                    continue;
                }
                #endregion

                optType = Operator.ConvertOperator(curOpt);
                funcCount = optType != OperatorType.FUNC ? 0 : getFunctionCount(texts, curPos);

                //若运算符堆栈为空,或者若运算符堆栈栈顶为左括号,则将当前运算符直接存入运算符堆栈.
                if (operators.Count == 0 || operators.Peek().Type == OperatorType.LB) {
                    operators.Push(new Operator(optType, curOpt, funcCount));
                    continue;
                }

                //若当前运算符优先级大于运算符栈顶的运算符,则将当前运算符直接存入运算符堆栈.
                if (Operator.ComparePriority(optType, operators.Peek().Type) > 0) {
                    operators.Push(new Operator(optType, curOpt, funcCount));
                } else {
                    //若当前运算符若比运算符堆栈栈顶的运算符优先级低或相等，则输出栈顶运算符到操作数堆栈，直至运算符栈栈顶运算符低于（不包括等于）该运算符优先级，
                    //或运算符栈栈顶运算符为左括号
                    //并将当前运算符压入运算符堆栈。
                    while (operators.Count > 0) {
                        if (Operator.ComparePriority(optType, operators.Peek().Type) <= 0 && operators.Peek().Type != OperatorType.LB) {
                            operands.Push(operators.Pop());

                            if (operators.Count == 0) {
                                operators.Push(new Operator(optType, curOpt, funcCount));
                                break;
                            }
                        } else {
                            operators.Push(new Operator(optType, curOpt, funcCount));
                            break;
                        }
                    }
                }
            }

            //转换完成,若运算符堆栈中尚有运算符时,
            //则依序取出运算符到操作数堆栈,直到运算符堆栈为空
            while (operators.Count > 0) {
                operands.Push(operators.Pop());
            }
            //调整操作数栈中对象的顺序并输出到最终栈
            while (operands.Count > 0) {
                m_tokens.Push(operands.Pop());
            }
            return true;
        }
        private int getFunctionCount(List<string> texts, int curPos)
        {
            var hasCount = 0;
            var count = 0;
            Stack<string> list = new Stack<string>();
            for (int i = curPos + 1; i < texts.Count; i++) {
                var t = texts[i];
                if (t == ")") {
                    if (list.Count == 0) break;
                    list.Pop();
                } else if (t == "(") {
                    list.Push(t);
                } else if (t == "," && list.Count == 0) {
                    count++;
                }
                hasCount = 1;
            }
            return count + hasCount;
        }
        private string getOperand(List<string> texts, int curPos)
        {
            var t = texts[curPos];
            if (curPos + 1 < texts.Count) {
                var t2 = texts[curPos + 1];
                if (t2 == "(") {
                    return "";
                }
            }
            if (m_Operators.Contains(t.ToLower())) {
                return "";
            }
            return t;
        }

        /// <summary>
        /// 更正信息
        /// </summary>
        /// <param name="texts"></param>
        private void corrections(List<string> texts)
        {
            for (int i = texts.Count - 1; i >= 1; i--) {
                var t = texts[i];
                if (i > 0) {
                    var t2 = texts[i - 1];
                    if (t == "-" && (t2 == "(" || t2 == "{" || t2 == ",")) {
                        texts.RemoveAt(i);
                        texts[i] = "-" + texts[i];
                    } else if (t == "+" && (t2 == "(" || t2 == "{" || t2 == ",")) {
                        texts.RemoveAt(i);
                    }
                } else if (i == 0) {
                    if (t == "-") {
                        texts.RemoveAt(i);
                        texts[i] = "-" + texts[i];
                    } else if (t == "+") {
                        texts.RemoveAt(i);
                    }
                }

                if (t == "}") {
                    texts[i] = ")";
                } else if (t == "{") {
                    texts[i] = "(";
                    texts.Insert(i, "array");
                }
            }
        }

        /// <summary>
        /// 验证字符串
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private bool isMatching(string exp)
        {
            List<char> opts = new List<char>();
            bool inString = false;

            var index = 0;
            while (index < exp.Length) {
                var chr = exp[index++];
                if (inString) {
                    if (chr == '\\') {
                        index++;
                    } else if (opts.Last() == chr) {
                        opts.RemoveAt(opts.Count - 1);
                        inString = false;
                    }
                } else {
                    if ("\"'".Contains(chr)) {
                        opts.Add(chr);
                        inString = true;
                    } else if (chr == '（') {
                        opts.Add('(');
                    } else if (chr == '【') {
                        opts.Add('[');
                    } else if (chr == '(' || chr == '[' || chr == '{') {
                        opts.Add(chr);
                    } else if ((chr == ')' || chr == '）') && opts.Count > 0 && opts.Last() == '(') {
                        opts.RemoveAt(opts.Count - 1);
                    } else if ((chr == ']' || chr == '】') && opts.Count > 0 && opts.Last() == '[') {
                        opts.RemoveAt(opts.Count - 1);
                    } else if (chr == '}' && opts.Count > 0 && opts.Last() == '{') {
                        opts.RemoveAt(opts.Count - 1);
                    } else {
                        //return false;
                    }
                }
            }
            return opts.Count == 0;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private List<string> splitText(string exp)
        {
            List<string> list = new List<string>();
            char opts = 'a';
            bool inString = false;
            bool inParameter = false;
            var end = 0;
            var start = 0;

            while (end < exp.Length) {
                var chr = exp[end++];
                if (inString) {
                    if (chr == '\\') {
                        end++;
                    } else if (opts == chr) {
                        inString = false;
                        list.Add(exp.Substring(start, end - start));
                        start = end;
                    }
                } else if (inParameter) {
                    if (chr == ']' || chr == '】') {
                        inParameter = false;
                        if (start < end - 1) {
                            list.Add(exp.Substring(start, end - start).Trim());
                        }
                        start = end;
                    }
                } else {
                    if ("\"'".Contains(chr)) {
                        opts = chr;
                        inString = true;
                        if (start < end - 1) {
                            list.Add(exp.Substring(start, end - start - 1).Trim());
                        }
                        start = end - 1;
                    } else if (chr == '[' || chr == '【') {
                        inParameter = true;
                        if (start < end - 1) {
                            list.Add(exp.Substring(start, end - start - 1).Trim());
                        }
                        start = end - 1;
                    } else if ("(){}*&%+-=/<>!|, ，（）".Contains(chr)) {
                        if (start < end - 1) {
                            list.Add(exp.Substring(start, end - start - 1).Trim());
                            start = end - 1;
                        }
                        if (start + 2 < exp.Length) {
                            var str = exp.Substring(start, 2);
                            if (str == "!=" || str == "==" || str == "<>" || str == ">=" || str == "<=" /*|| str == "&&" || str == "||"*/) {
                                end++;
                            }
                        }

                        var str2 = exp.Substring(start, end - start).Trim()
                             .Replace("，", ",")
                             .Replace("【", "[")
                             .Replace("】", "]")
                             .Replace("（", "(")
                             .Replace("）", ")");
                        if (str2 != "") list.Add(str2);
                        start = end;
                    }
                }
            }
            if (end > start) {
                list.Add(exp.Substring(start, end - start).Trim());
            }
            return list;
        }

        #endregion

        #region GetParameterNames

        public List<string> GetParameterNames()
        {
            List<string> list = new List<string>();
            foreach (var item in m_tokens) {
                var curOpd = item as Operand;
                if (curOpd != null) {
                    if (curOpd.Type == OperandType.PARAMETER) {
                        var name = curOpd.Value.ToString();
                        list.Add(name);
                    }
                }
            }
            return list;
        }

        #endregion

        public object Evaluate()
        {
            /*
              逆波兰表达式求值算法：
            、循环扫描语法单元的项目。
            、如果扫描的项目是操作数，则将其压入操作数堆栈，并扫描下一个项目。
            、如果扫描的项目是一个二元运算符，则对栈的顶上两个操作数执行该运算。
            、如果扫描的项目是一个一元运算符，则对栈的最顶上操作数执行该运算。
            、将运算结果重新压入堆栈。
            、重复步骤2-5，堆栈中即为结果值。
            */
            if (m_tokens.Count == 0) return null;

            object value = null;
            Stack<Operand> opds = new Stack<Operand>();
            Stack<object> pars = new Stack<object>();

            foreach (object item in m_tokens) {
                var curOpd = item as Operand;
                if (curOpd != null) {
                    if (curOpd.Type == OperandType.PARAMETER) {
                        var opd = GetParameter(curOpd);
                        if (opd != null) {
                            opds.Push(opd);
                            continue;
                        }
                    }
                    opds.Push((Operand)item);
                } else {
                    var curOpt = (Operator)item;
                    List<Operand> list = new List<Operand>();
                    switch (curOpt.Type) {

                        #region 乘,*,multiplication
                        case OperatorType.MUL:
                            list.Insert(0, opds.Pop());
                            list.Insert(0, opds.Pop());
                            opds.Push(mul(list));
                            break;
                        #endregion

                        #region 除,/,division
                        case OperatorType.DIV:
                            list.Insert(0, opds.Pop());
                            list.Insert(0, opds.Pop());
                            opds.Push(div(list));
                            break;
                        #endregion

                        #region 余,%,modulus
                        case OperatorType.MOD:
                            list.Insert(0, opds.Pop());
                            list.Insert(0, opds.Pop());
                            opds.Push(mod(list));
                            break;
                        #endregion

                        #region 加,+,Addition
                        case OperatorType.ADD:
                            list.Insert(0, opds.Pop());
                            list.Insert(0, opds.Pop());
                            opds.Push(add(list));
                            break;
                        #endregion

                        #region 加,&,Addition
                        case OperatorType.StringADD:
                            list.Insert(0, opds.Pop());
                            list.Insert(0, opds.Pop());
                            opds.Push(stringAdd(list));
                            break;
                        #endregion

                        #region 减,-,subtraction
                        case OperatorType.SUB:
                            list.Insert(0, opds.Pop());
                            list.Insert(0, opds.Pop());
                            opds.Push(sub(list));
                            break;
                        #endregion

                        #region 小于 小于或等于 大于 大于或等于 等于 不等于
                        case OperatorType.LT:
                        case OperatorType.LE:
                        case OperatorType.GT:
                        case OperatorType.GE:
                        case OperatorType.ET:
                        case OperatorType.UT:
                            list.Insert(0, opds.Pop());
                            list.Insert(0, opds.Pop());
                            opds.Push(compare(list, curOpt.Type));
                            break;
                        #endregion

                        #region FUNC
                        case OperatorType.FUNC:
                            for (int i = 0; i < curOpt.ArgsCount; i++) {
                                list.Insert(0, opds.Pop());
                            }
                            var fun = funcDict[curOpt.Value.ToLower()];
                            if (curOpt.Value.ToLower() != "iferror") {
                                var o = hasError(list);
                                if (o != null) {
                                    opds.Push(o);
                                    break;
                                }
                            }
                            opds.Push(fun(list));
                            break;
                            #endregion

                    }
                }
            }

            if (opds.Count == 1) {
                var outopd = opds.Pop();
                if (outopd.Type == OperandType.ERROR) {
                    throw new Exception(outopd.StringValue);
                }
                value = outopd.Value;
            }
            return value;
        }

        #region TryEvaluate
        public short TryEvaluate(string exp, short def)
        {
            if (Parse(exp)) {
                return (short)(double)Evaluate();
            }
            return def;
        }
        public int TryEvaluate(string exp, int def)
        {
            if (Parse(exp)) {
                return (int)(double)Evaluate();
            }
            return def;
        }
        public long TryEvaluate(string exp, long def)
        {
            if (Parse(exp)) {
                return (long)(double)Evaluate();
            }
            return def;
        }

        public ushort TryEvaluate(string exp, ushort def)
        {
            if (Parse(exp)) {
                return (ushort)(double)Evaluate();
            }
            return def;
        }
        public uint TryEvaluate(string exp, uint def)
        {
            if (Parse(exp)) {
                return (uint)(double)Evaluate();
            }
            return def;
        }
        public ulong TryEvaluate(string exp, ulong def)
        {
            if (Parse(exp)) {
                return (ulong)(double)Evaluate();
            }
            return def;
        }

        public float TryEvaluate(string exp, float def)
        {
            if (Parse(exp)) {
                return (float)(double)Evaluate();
            }
            return def;
        }
        public double TryEvaluate(string exp, double def)
        {
            if (Parse(exp)) {
                return (double)Evaluate();
            }
            return def;
        }

        public string TryEvaluate(string exp, string def)
        {
            if (Parse(exp)) {
                return Evaluate().ToString();
            }
            return def;
        }
        public bool TryEvaluate(string exp, bool def)
        {
            if (Parse(exp)) {
                return (bool)Evaluate();
            }
            return def;
        }

        public DateTime TryEvaluate(string exp, DateTime def)
        {
            if (Parse(exp)) {
                return (DateTime)Evaluate();
            }
            return def;
        }

        #endregion




        private Operand doFunc(Operator curOpt, List<Operand> ops)
        {
            try {
                var fun = funcDict[curOpt.Value.ToLower()];
                if (curOpt.Value.ToLower() != "iferror") {
                    var o = hasError(ops);
                    if (o != null) {
                        return o;
                    }
                }
                return fun(ops);
            } catch (Exception ex) {
                return new Operand(OperandType.ERROR, curOpt.ToString() + "Error:" + ex.Message);
            }
        }

        private Operand hasError(List<Operand> ops)
        {
            foreach (var item in ops) {
                if (item.Type == OperandType.ERROR) {
                    return item;
                }
            }
            return null;
        }

        private Operand throwError(string errMsg, List<Operand> ops)
        {
            foreach (var item in ops) {
                if (item.Type == OperandType.ERROR) {
                    return item;
                }
            }
            return new Operand(OperandType.ERROR, errMsg);
        }

        #region 加减乘除 连接 取余
        private Operand stringAdd(List<Operand> ops)
        {
            return new Operand(OperandType.STRING, ops[0].StringValue + ops[1].StringValue);
        }
        private Operand add(List<Operand> ops)
        {
            if (ops[1].Type == OperandType.NUMBER) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue + ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue + ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) + ops[1].NumberValue);
                }
            }
            if (ops[1].Type == OperandType.BOOLEAN) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue + (ops[1].BooleanValue ? 1.0 : 0.0));
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue + (ops[1].BooleanValue ? 1.0 : 0.0));
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) + (ops[1].BooleanValue ? 1.0 : 0.0));
                }
            }
            if (ops[1].Type == OperandType.DATE) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue + ops[1].DateValue);
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue + ops[1].DateValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) + ops[1].DateValue);
                }
            }

            if (ops[1].Type == OperandType.STRING || ops[0].Type == OperandType.STRING) {
                return new Operand(OperandType.STRING, ops[0].StringValue + ops[1].StringValue);
            }
            return throwError("两个参数不能相加", ops);
        }
        private Operand sub(List<Operand> ops)
        {
            if (ops[1].Type == OperandType.NUMBER) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue - ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue - ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) - ops[1].NumberValue);
                }
            }
            if (ops[1].Type == OperandType.BOOLEAN) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue - (ops[1].BooleanValue ? 1.0 : 0.0));
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue - (ops[1].BooleanValue ? 1.0 : 0.0));
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) - (ops[1].BooleanValue ? 1.0 : 0.0));
                }
            }
            if (ops[1].Type == OperandType.DATE) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue - ops[1].DateValue);
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue - ops[1].DateValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) - ops[1].DateValue);
                }
            }
            return throwError("两个参数不能相减", ops);
        }
        private Operand mul(List<Operand> ops)
        {
            if (ops[1].Type == OperandType.NUMBER) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue * ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue * ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) * ops[1].NumberValue);
                }
            }
            if (ops[1].Type == OperandType.BOOLEAN) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue * (ops[1].BooleanValue ? 1.0 : 0.0));
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue * (ops[1].BooleanValue ? 1.0 : 0.0));
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) * (ops[1].BooleanValue ? 1.0 : 0.0));
                }
            }
            if (ops[1].Type == OperandType.DATE) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue * ops[1].DateValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) * ops[1].DateValue);
                }
            }
            return throwError("两个参数不能相乘", ops);
        }
        private Operand div(List<Operand> ops)
        {
            if (ops[1].Type == OperandType.NUMBER) {
                if (ops[1].NumberValue == 0) {
                    return throwError("无法除0", ops);
                }

                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue / ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue / ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) / ops[1].NumberValue);
                }
            }
            if (ops[1].Type == OperandType.DATE) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue / ops[1].DateValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) / ops[1].DateValue);
                }
            }
            return throwError("两个参数不能相除", ops);
        }
        private Operand mod(List<Operand> ops)
        {
            if (ops[1].Type == OperandType.NUMBER) {
                if (ops[1].NumberValue == 0) {
                    return throwError("无法除0", ops);
                }
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue % ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.DATE) {
                    return new Operand(OperandType.DATE, ops[0].DateValue % ops[1].NumberValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) % ops[1].NumberValue);
                }
            }
            if (ops[1].Type == OperandType.DATE) {
                if (ops[0].Type == OperandType.NUMBER) {
                    return new Operand(OperandType.NUMBER, ops[0].NumberValue % ops[1].DateValue);
                } else if (ops[0].Type == OperandType.BOOLEAN) {
                    return new Operand(OperandType.NUMBER, (ops[0].BooleanValue ? 1.0 : 0.0) % ops[1].DateValue);
                }
            }
            return throwError("两个参数不能相除取余", ops);
        }
        #endregion

        #region 小于 小于或等于 大于 大于或等于 等于 不等于
        private Operand compare(List<Operand> ops, OperatorType type)
        {
            var a = ops[0];
            var b = ops[1];
            int r = 0;
            if (a.Type == b.Type) {
                if (a.Type == OperandType.STRING) {
                    r = compare(a.StringValue, b.StringValue);
                } else {
                    r = compare(a.NumberValue, b.NumberValue);
                }
            } else if ((a.Type == OperandType.DATE && b.Type == OperandType.STRING) || (b.Type == OperandType.DATE && a.Type == OperandType.STRING)) {
                r = compare(a.StringValue, b.StringValue);
            } else if (a.Type == OperandType.STRING || b.Type == OperandType.STRING) {
                return new Operand(OperandType.ERROR, "两个类型无法比较");
            } else {
                r = compare(a.NumberValue, b.NumberValue);
            }
            if (type == OperatorType.LT) {
                return new Operand(OperandType.BOOLEAN, r == -1);
            } else if (type == OperatorType.LE) {
                return new Operand(OperandType.BOOLEAN, r <= 0);
            } else if (type == OperatorType.GT) {
                return new Operand(OperandType.BOOLEAN, r == 1);
            } else if (type == OperatorType.GE) {
                return new Operand(OperandType.BOOLEAN, r >= 0);
            } else if (type == OperatorType.ET) {
                return new Operand(OperandType.BOOLEAN, r == 0);
            }
            return new Operand(OperandType.BOOLEAN, r != 0);
        }
        private int compare(double t1, double t2)
        {
            t1 = Math.Round(t1, 12);
            t2 = Math.Round(t2, 12);
            if (t1 == t2) {
                return 0;
            } else if (t1 > t2) {
                return 1;
            }
            return -1;
        }
        private int compare(string t1, string t2)
        {
            if (t1 == t2) {
                return 0;
            }
            List<string> ts = new List<string>() { t1, t2 };
            ts = ts.OrderBy(q => q).ToList();
            if (t1 == ts[1]) {
                return 1;
            }
            return -1;
        }

        #endregion

    }






}
