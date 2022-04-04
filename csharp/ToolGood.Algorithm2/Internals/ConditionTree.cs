using Antlr4.Runtime;
using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.Internals
{
    public class ConditionTree
    {
        public List<ConditionTree> Nodes { get; internal set; }

        public int Start { get; internal set; }
        public int End { get; internal set; }

        public ConditionType Type { get; internal set; }

        public String ErrorMessage { get; internal set; }

        internal ConditionTree()
        {
            Nodes = new List<ConditionTree>();
        }

        public enum ConditionType
        {
            String,
            And,
            Or,
            Error
        }

        public static ConditionTree Parse(string condition)
        {
            ConditionTree tree = new ConditionTree();
            if (string.IsNullOrWhiteSpace(condition)) {
                tree.Type = ConditionType.Error;
                tree.ErrorMessage = "condition 为空";
                return tree;
            }
            try {
                var stream = new AntlrCharStream(new AntlrInputStream(condition));
                var lexer = new mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new mathParser(tokens);
                var antlrErrorListener = new AntlrErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(antlrErrorListener);

                var context = parser.prog();
                if (antlrErrorListener.IsError) {
                    tree.Type = ConditionType.Error;
                    tree.ErrorMessage = antlrErrorListener.ErrorMsg;
                    return tree;
                }
                var visitor = new MathSplitVisitor();
                return visitor.Visit(context);

            } catch (Exception ex) {
                tree.Type = ConditionType.Error;
                tree.ErrorMessage = ex.Message + "\r\n" + ex.StackTrace;
            }
            return tree;
        }
    }
}
