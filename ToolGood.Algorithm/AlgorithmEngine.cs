using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm
{
    public partial class AlgorithmEngine
    {
        private Random rand;
        private List<string> m_Operators = new List<string>() { ".", "(", ")", "!", "*", "/", "%", "+", "-", "<", ">", "=", "&", "|", ",", "==", "!=", ">=", "<=", "<>", "@" };
        private List<object> m_tokens = new List<object>();        //最终逆波兰式堆栈
        private Dictionary<string, List<object>> tokenDict = new Dictionary<string, List<object>>();
        private Dictionary<string, Func<List<Operand>, Operand>> funcDict = new Dictionary<string, Func<List<Operand>, Operand>>();
        private bool _useExcelIndex = true;
        private int excelIndex = 1;
        /// <summary>
        /// 使用EXCEL索引
        /// </summary>
        public bool UseExcelIndex { get { return _useExcelIndex; } set { _useExcelIndex = value; excelIndex = value ? 1 : 0; } }
        /// <summary>
        /// 最后一个错误
        /// </summary>
        public string LastError { get; private set; }

        public AlgorithmEngine()
        {
            var seed = Math.Abs(Guid.NewGuid().GetHashCode());
            rand = new Random(seed);


            AddDateTimeFunction();
            AddFlowFunction();
            AddMathFunction();
            AddStringFunction();
            AddSumFunction();
            AddCSharp();
        }

        #region 可重写的方法

        protected virtual Operand GetParameter(Operand curOpd)
        {
            return null;
        }
        protected void AddFunction(string funName, Func<List<Operand>, Operand> function)
        {
            addFunc(funName, function);
        }

        #endregion

        #region Parse
        /// <summary>
        /// 编译公式，默认
        /// </summary>
        /// <param name="exp">公式</param>
        /// <returns></returns>
        public bool Parse(string exp)
        {
            if (string.IsNullOrEmpty(exp) || exp.Trim() == "" || !this.isMatching(exp)) {
                LastError = "exp无效";
                return false;
            }
            var obj = parse(exp, out string error);
            if (obj == null) {
                return false;
            }
            m_tokens = obj;
            return true;
        }
        /// <summary>
        /// 编译公式,并输出错误，默认
        /// </summary>
        /// <param name="exp">公式</param>
        /// <param name="error">输出错误</param>
        /// <returns></returns>
        public bool Parse(string exp, out string error)
        {
            if (string.IsNullOrEmpty(exp) || exp.Trim() == "" || !this.isMatching(exp)) {
                error = "exp无效";
                LastError = "exp无效";
                return false;
            }
            var obj = parse(exp, out error);
            if (obj == null) {
                return false;
            }
            m_tokens = obj;
            return true;
        }
        /// <summary>
        /// 编译公式
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="exp">公式</param>
        /// <returns></returns>
        public bool Parse(string name, string exp)
        {
            if (string.IsNullOrEmpty(exp) || exp.Trim() == "" || !this.isMatching(exp)) {
                LastError = "exp无效";
                return false;
            }
            var obj = parse(exp, out string error);
            if (obj == null) {
                return false;
            }
            tokenDict[name] = obj;
            return true;
        }
        /// <summary>
        /// 编译公式,并输出错误
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="exp">公式</param>
        /// <param name="error">输出错误</param>
        /// <returns></returns>
        public bool Parse(string name, string exp, out string error)
        {
            if (string.IsNullOrEmpty(exp) || exp.Trim() == "" || !this.isMatching(exp)) {
                error = "exp无效";
                LastError = "exp无效";
                return false;
            }
            var obj = parse(exp, out error);
            if (obj == null) {
                return false;
            }
            tokenDict[name] = obj;
            return true;
        }


        private List<object> parse(string exp, out string error)
        {
            error = null;
            var tmp = parse(exp);
            var names = GetParameterNames(tmp);
            foreach (var item in names) {
                if (GetParameter(new Operand(item)) == null) {
                    error = $"参数{item}无效!";
                    LastError = error;
                    return null;
                }
            }
            var funcnames = GetFunctionNames(tmp);
            foreach (var item in funcnames) {
                if (funcDict.ContainsKey(item.ToLower()) == false) {
                    error = $"方法{item}无效!";
                    LastError = error;
                    return null;
                }
            }
            return tmp;
        }

        private List<object> parse(string exp)
        {
            List<object> operands = new List<object>();             //操作数堆栈
            Stack<Operator> operators = new Stack<Operator>();      //运算符堆栈
            OperatorType optType = OperatorType.ERR;                //运算符类型
            string curOpd = "";                                 //当前操作数
            string curOpt = "";                                 //当前运算符
            int curPos = 0;                                     //当前位置
            int funcCount = 0;                                        //函数数量
            bool hasPoint = false;

            var texts = splitText(exp);
            texts.Add("@");
            corrections(texts);

            while (curPos < texts.Count) {
                //获取 当前操作数
                curOpd = getOperand(texts, curPos);
                if (curOpd != "") {
                    if (operators.Count > 0 && hasPoint) {
                        Operator op = operators.Pop();
                        if (op.Type == OperatorType.POINT) {
                            op.Type = OperatorType.POINTCHILD;
                        }
                        operators.Push(op);
                        hasPoint = false;
                    }
                    operands.Add(new Operand(curOpd));
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
                            operands.Add(operators.Pop());
                        } else {
                            operators.Pop();
                            break;
                        }
                    }
                    continue;
                }
                #endregion

                optType = Operator.ConvertOperator(curOpt, texts[curPos]);
                if (optType == OperatorType.PARAMETER) {
                    operands.Add(new Operand(curOpt));
                    continue;
                }
                funcCount = optType != OperatorType.FUNC ? 0 : getFunctionCount(texts, curPos);
                if (operators.Count > 0 && hasPoint) {
                    Operator op = operators.Pop();
                    if (op.Type == OperatorType.POINT) {
                        op.Type = OperatorType.POINTFUNC;
                        funcCount++;
                    }
                    operators.Push(op);
                    hasPoint = false;
                }
                hasPoint = optType == OperatorType.POINT;


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
                            operands.Add(operators.Pop());

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

            while (operators.Count > 0) {
                operands.Add(operators.Pop());
            }
            return operands;
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
            if (t.StartsWith("[")) {
                return t;
            }
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
            for (int i = texts.Count - 1; i >= 0; i--) {
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
                    } else if (chr == '.') {
                        if (end - start - 1 > 0) {
                            var num = exp.Substring(start, end - start - 1).Trim();
                            if (long.TryParse(num, out long d) == false) {
                                TryAddList(list, num);
                                start = end - 1;
                            }
                        }
                    } else if ("(){}*&%+-=/<>!|, ，（）".Contains(chr)) {
                        if (start < end - 1) {
                            var t = exp.Substring(start, end - start - 1).Trim();
                            TryAddList(list, t);
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
                var t = exp.Substring(start, end - start).Trim();
                TryAddList(list, t);
            }
            return list;
        }
        private void TryAddList(List<string> list, string text)
        {
            //list.Add(text);

            if (text.Length > 1 && text.StartsWith(".")) {
                list.Add(".");
                list.Add(text.Substring(1));
            } else {
                list.Add(text);
            }
        }

        #endregion

        #region GetParameterNames
        /// <summary>
        /// 获取所有参数名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetParameterNames()
        {
            return GetParameterNames(m_tokens);
        }

        private List<string> GetParameterNames(List<object> tokens)
        {
            List<string> list = new List<string>();
            foreach (var item in tokens) {
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

        private List<string> GetFunctionNames(List<object> tokens)
        {
            List<string> list = new List<string>();
            foreach (var item in tokens) {
                var curOpd = item as Operator;
                if (curOpd != null) {
                    if (curOpd.Type == OperatorType.FUNC) {
                        var name = curOpd.Value;
                        list.Add(name);
                    }
                }
            }
            return list;
        }

        #endregion

        #region Evaluate
        /// <summary>
        /// 执行,默认
        /// </summary>
        /// <returns></returns>
        public object Evaluate()
        {
            if (m_tokens.Count == 0) {
                LastError = "请编译公式！";
                throw new Exception("请编译公式！");
            }
            return evaluate(m_tokens);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public object Evaluate(string name)
        {
            if (tokenDict.TryGetValue(name, out List<object> tokens)) {
                return evaluate(tokens);
            }
            LastError = "请编译公式！";
            throw new Exception("请编译公式！");
        }

        private object evaluate(List<object> tokens)
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

            Stack<Operand> opds = new Stack<Operand>();

            foreach (object item in tokens) {
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
                        #region POINT POINTCHILD POINTFUNC
                        case OperatorType.POINTFUNC: break;
                        case OperatorType.POINT:
                        case OperatorType.POINTCHILD:
                            list.Insert(0, opds.Pop());
                            list.Insert(0, opds.Pop());
                            opds.Push(getChild(list));
                            break; 
                        #endregion

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
                            opds.Push(F_Mod(list));
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
                            opds.Push(doFunc(curOpt, list));
                            break;
                            #endregion

                    }
                }
            }
            object value = null;
            if (opds.Count == 1) {
                var outopd = opds.Pop();
                if (outopd.Type == OperandType.ERROR) {
                    LastError = outopd.StringValue;
                    throw new Exception(outopd.StringValue);
                }
                value = outopd.Value;
            }

            return value;
        }

        #endregion

        #region TryEvaluate
        public short TryEvaluate(string exp, short def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is Date) return (short)(double)(Date)obj;
                    return (short)(double)obj;
                } catch (Exception) { }
            }
            return def;
        }
        public int TryEvaluate(string exp, int def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is Date) return (int)(double)(Date)Evaluate();
                    return (int)(double)obj;
                } catch (Exception) { }
            }
            return def;
        }
        public long TryEvaluate(string exp, long def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is Date) return (long)(double)(Date)obj;
                    return (long)(double)obj;
                } catch (Exception) {
                }
            }
            return def;
        }

        public ushort TryEvaluate(string exp, ushort def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is Date) return (ushort)(double)(Date)obj;
                    return (ushort)(double)obj;
                } catch (Exception) {
                }
            }
            return def;
        }
        public uint TryEvaluate(string exp, uint def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is Date) return (uint)(double)(Date)obj;
                    return (uint)(double)obj;
                } catch (Exception) {
                }
            }
            return def;
        }
        public ulong TryEvaluate(string exp, ulong def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is Date) return (ulong)(double)(Date)obj;
                    return (ulong)(double)obj;
                } catch (Exception) {
                }
            }
            return def;
        }

        public float TryEvaluate(string exp, float def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is Date) return (float)(double)(Date)obj;
                    return (float)(double)obj;
                } catch (Exception) {
                }
            }
            return def;
        }
        public double TryEvaluate(string exp, double def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is Date) return (double)(Date)obj;
                    return (double)obj;
                } catch (Exception) {
                }
            }
            return def;
        }

        public string TryEvaluate(string exp, string def)
        {
            if (Parse(exp)) {
                try {
                    return Evaluate().ToString();
                } catch (Exception) {
                }
            }
            return def;
        }
        public bool TryEvaluate(string exp, bool def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is string) {
                        return bool.Parse(obj.ToString());
                    }
                    if (obj is bool) {
                        return (bool)obj;
                    }
                    if (obj is Date) return def;
                    return decimal.Parse(obj.ToString()) != 0;
                } catch (Exception) {
                }
            }
            return def;
        }

        public DateTime TryEvaluate(string exp, DateTime def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is string) {
                        if (DateTime.TryParse(obj.ToString(), out DateTime dt)) {
                            return dt;
                        }
                        return def;
                    }
                    return (DateTime)(Date)Evaluate();
                } catch (Exception) { }
            }
            return def;
        }

        public TimeSpan TryEvaluate(string exp, TimeSpan def)
        {
            if (Parse(exp)) {
                try {
                    var obj = Evaluate();
                    if (obj is string) {
                        if (TimeSpan.TryParse(obj.ToString(), out TimeSpan dt)) {
                            return dt;
                        }
                        return def;
                    }
                    return (TimeSpan)(Date)Evaluate();
                } catch (Exception) {
                }
            }
            return def;
        }

        #endregion

        private void addFunc(string name, Func<List<Operand>, Operand> func)
        {
            name = name.ToLower().Trim();
            //m_Operators.Add(name);
            funcDict[name] = func;
        }


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
            } catch (FunctionException ex) {
                return ex.Operand;
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

        private Operand ThrowError(string errMsg, List<Operand> ops)
        {
            foreach (var item in ops) {
                if (item.Type == OperandType.ERROR) {
                    return item;
                }
            }
            return new Operand(OperandType.ERROR, errMsg);
        }

        private void CheckArgsCount(string funcName, List<Operand> ops, OperandType[][] operandTypes)
        {
            if (ops.Count < operandTypes.Min(q => q.Length)) throw new FunctionException(ThrowError(funcName + "参数不足！", new List<Operand>()));
            if (ops.Count > operandTypes.Max(q => q.Length)) throw new FunctionException(ThrowError(funcName + "参数过多！", new List<Operand>()));

            foreach (var operands in operandTypes) {
                if (operands.Length != ops.Count) continue;
                var success = true;
                for (int i = 0; i < operands.Length; i++) {
                    if (ops[i].CanTransitionTo(operands[i]) == false) {
                        success = false;
                        break;
                    }
                }
                if (success) {
                    return;
                }
            }
            throw new FunctionException(ThrowError(funcName + "参数类型出错！", new List<Operand>()));
        }

        #region 取子数
        private Operand getChild(List<Operand> ops)
        {
            var obj = ops[0];
            var op = ops[1].StringValue;
            try {
                if (obj.Type == OperandType.ARRARY) {
                    var list = obj.GetValueList();
                    if (op.ToLower() == "length") {
                        return new Operand(OperandType.NUMBER, (double)list.Count);
                    }
                    if (int.TryParse(op, out int index)) {
                        return list[index - excelIndex];
                    }
                }
                if (obj.Type == OperandType.JSON) {
                    var json = obj.Value as JsonData;
                    if (json.IsArray) {
                        if (op.ToLower() == "length") {
                            return new Operand(OperandType.NUMBER, (double)json.Count);
                        } else if (int.TryParse(op, out int index)) {
                            var v = json[index - excelIndex];
                            if (v.IsString) {
                                return new Operand(OperandType.STRING, v.ToString());
                            } else if (v.IsBoolean) {
                                return new Operand(OperandType.BOOLEAN, bool.Parse(v.ToString()));
                            } else if (v.IsDouble) {
                                return new Operand(OperandType.NUMBER, double.Parse(v.ToString()));
                            } else if (v.IsInt) {
                                return new Operand(OperandType.NUMBER, double.Parse(v.ToString()));
                            } else if (v.IsLong) {
                                return new Operand(OperandType.NUMBER, double.Parse(v.ToString()));
                            } else {
                                return new Operand(OperandType.JSON, v);
                            }
                        }
                    } else {
                        var v = json[op];
                        if (v.IsString) {
                            return new Operand(v.ToString());
                        } else if (v.IsBoolean) {
                            return new Operand(OperandType.BOOLEAN, bool.Parse(v.ToString()));
                        } else if (v.IsDouble) {
                            return new Operand(OperandType.NUMBER, double.Parse(v.ToString()));
                        } else if (v.IsInt) {
                            return new Operand(OperandType.NUMBER, double.Parse(v.ToString()));
                        } else if (v.IsLong) {
                            return new Operand(OperandType.NUMBER, double.Parse(v.ToString()));
                        } else if (v.IsObject) {
                            return new Operand(OperandType.JSON, v);
                        }
                    }
                }
            } catch (Exception) { }
            return new Operand(OperandType.ERROR, op + "操作无效！");
        }


        #endregion

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
            //if (ops[1].Type == OperandType.STRING || ops[0].Type == OperandType.STRING) {
            //    return new Operand(OperandType.STRING, ops[0].StringValue + ops[1].StringValue);
            //}
            return ThrowError("两个参数不能相加", ops);
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
            return ThrowError("两个参数不能相减", ops);
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
            return ThrowError("两个参数不能相乘", ops);
        }
        private Operand div(List<Operand> ops)
        {
            if (ops[1].Type == OperandType.NUMBER) {
                if (ops[1].NumberValue == 0) {
                    return ThrowError("无法除0", ops);
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
            return ThrowError("两个参数不能相除", ops);
        }
        private Operand F_Mod(List<Operand> ops)
        {
            if (ops[1].Type == OperandType.NUMBER) {
                if (ops[1].NumberValue == 0) {
                    return ThrowError("无法除0", ops);
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
            return ThrowError("两个参数不能相除取余", ops);
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
