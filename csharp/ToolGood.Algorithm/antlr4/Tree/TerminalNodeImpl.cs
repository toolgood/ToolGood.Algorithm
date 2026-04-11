/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime.Misc;
namespace Antlr4.Runtime.Tree
{
    internal class TerminalNodeImpl : ITerminalNode
    {
        private IToken _symbol;
        private IRuleNode _parent;
        public TerminalNodeImpl(IToken symbol)
        {
            this._symbol = symbol;
        }
        public virtual IParseTree GetChild(int i)
        {
            return null;
        }
        public virtual IToken Symbol
        {
            get
            {
                return _symbol;
            }
        }
        public virtual IRuleNode Parent
        {
            get
            {
                return _parent;
            }
			set
			{
				_parent = value;
			}
        }
        public virtual int ChildCount
        {
            get
            {
                return 0;
            }
        }
        public virtual T Accept<T>(IParseTreeVisitor<T> visitor)
        {
            return visitor.VisitTerminal(this);
        }
        public virtual string GetText()
        {
            if (Symbol != null)
            {
                return Symbol.Text;
            }
            return null;
        }
    }
}
