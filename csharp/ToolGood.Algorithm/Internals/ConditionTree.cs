using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals
{
    /// <summary>
    /// 条件树
    /// </summary>
    public sealed class ConditionTree
    {
        /// <summary>
        /// 子节点
        /// </summary>
        public List<ConditionTree> Nodes { get; internal set; }

        /// <summary>
        /// 开始位置
        /// </summary>
        public int Start { get; internal set; }

        /// <summary>
        /// 结束位置
        /// </summary>
        public int End { get; internal set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ConditionTreeType Type { get; internal set; }

        /// <summary>
        /// 条件
        /// </summary>
        public string ConditionString { get; internal set; }

        /// <summary>
        /// 出错信息
        /// </summary>
        public string ErrorMessage { get; internal set; }

        internal ConditionTree()
        {
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static ConditionTree Parse(string condition)
        {
            var tree = new ConditionTree();
            if (string.IsNullOrWhiteSpace(condition)) {
                tree.Type = ConditionTreeType.Error;
                tree.ErrorMessage = "condition is null";
                return tree;
            }
            try {
                var stream = new AntlrCharStream(new AntlrInputStream(condition));
                var lexer = new math.mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new math.mathParser(tokens);
                var antlrErrorListener = new AntlrErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(antlrErrorListener);

                var context = parser.prog();
                if (antlrErrorListener.IsError) {
                    tree.Type = ConditionTreeType.Error;
                    tree.ErrorMessage = antlrErrorListener.ErrorMsg;
                    return tree;
                }
                var visitor = new MathSplitVisitor();
                return visitor.Visit(context);
            } catch (Exception ex) {
                tree.Type = ConditionTreeType.Error;
                tree.ErrorMessage = ex.Message + "\r\n" + ex.StackTrace;
            }
            return tree;
        }
    }
}