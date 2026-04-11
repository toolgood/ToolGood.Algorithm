/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
namespace Antlr4.Runtime.Tree
{
    internal abstract class AbstractParseTreeVisitor<Result> : IParseTreeVisitor<Result>
    {
        public virtual Result Visit(IParseTree tree)
        {
            return tree.Accept(this);
        }
        public virtual Result VisitChildren(IRuleNode node)
        {
            Result result = DefaultResult;
            int n = node.ChildCount;
            for (int i = 0; i < n; i++)
            {
                if (!ShouldVisitNextChild(node, result))
                {
                    break;
                }
                IParseTree c = node.GetChild(i);
                Result childResult = c.Accept(this);
                result = AggregateResult(result, childResult);
            }
            return result;
        }
        public virtual Result VisitTerminal(ITerminalNode node)
        {
            return DefaultResult;
        }
        public virtual Result VisitErrorNode(IErrorNode node)
        {
            return DefaultResult;
        }
        protected internal virtual Result DefaultResult
        {
            get
            {
                return default(Result);
            }
        }
        protected internal virtual Result AggregateResult(Result aggregate, Result nextResult)
        {
            return nextResult;
        }
        protected internal virtual bool ShouldVisitNextChild(IRuleNode node, Result currentResult)
        {
            return true;
        }
    }
}
