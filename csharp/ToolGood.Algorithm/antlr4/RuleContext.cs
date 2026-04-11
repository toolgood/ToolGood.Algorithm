/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
namespace Antlr4.Runtime
{
    internal class RuleContext : IRuleNode
    {
        private Antlr4.Runtime.RuleContext _parent;
        public int invokingState = -1;
        public RuleContext()
        {
        }
        public RuleContext(Antlr4.Runtime.RuleContext parent, int invokingState)
        {
            this._parent = parent;
            this.invokingState = invokingState;
        }
 
        public virtual Interval SourceInterval
        {
            get
            {
                return Interval.Invalid;
            }
        }
        RuleContext IRuleNode.RuleContext
        {
            get
            {
                return this;
            }
        }
        public virtual Antlr4.Runtime.RuleContext Parent
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
        public virtual string GetText()
        {
            if (ChildCount == 0)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < ChildCount; i++)
            {
                builder.Append(GetChild(i).GetText());
            }
            return builder.ToString();
        }
        public virtual int RuleIndex
        {
            get
            {
                return -1;
            }
        }

	public virtual void setAltNumber(int altNumber) { }
        public virtual IParseTree GetChild(int i)
        {
            return null;
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
            return visitor.VisitChildren(this);
        }
    }
}
