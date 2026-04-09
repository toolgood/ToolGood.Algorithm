/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
namespace Antlr4.Runtime.Tree
{
    public class ErrorNodeImpl : TerminalNodeImpl, IErrorNode
    {
        public ErrorNodeImpl(IToken token)
            : base(token)
        {
        }
        public override T Accept<T>(IParseTreeVisitor<T> visitor)
        {
            return visitor.VisitErrorNode(this);
        }
    }
}
