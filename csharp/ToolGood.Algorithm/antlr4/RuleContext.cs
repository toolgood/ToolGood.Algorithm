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
        public virtual int Depth()
        {
            int n = 0;
            Antlr4.Runtime.RuleContext p = this;
            while (p != null)
            {
                p = p._parent;
                n++;
            }
            return n;
        }
        public virtual bool IsEmpty
        {
            get
            {
                return invokingState == -1;
            }
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
        IRuleNode IRuleNode.Parent
        {
            get
            {
                return Parent;
            }
        }
        IParseTree IParseTree.Parent
        {
            get
            {
                return Parent;
            }
        }
        ITree ITree.Parent
        {
            get
            {
                return Parent;
            }
        }
        public virtual Antlr4.Runtime.RuleContext Payload
        {
            get
            {
                return this;
            }
        }
        object ITree.Payload
        {
            get
            {
                return Payload;
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

	/* Set the outer alternative number for this context node. Default
	 * implementation does nothing to avoid backing field overhead for
	 * trees that don't need it.  Create
     * a subclass of ParserRuleContext with backing field and set
     * option contextSuperClass.
	 */
	public virtual void setAltNumber(int altNumber) { }
        public virtual IParseTree GetChild(int i)
        {
            return null;
        }
        ITree ITree.GetChild(int i)
        {
            return GetChild(i);
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

        public virtual string ToString(IRecognizer recog, Antlr4.Runtime.RuleContext stop)
        {
            string[] ruleNames = recog != null ? recog.RuleNames : null;
            IList<string> ruleNamesList = ruleNames != null ? Arrays.AsList(ruleNames) : null;
            return ToString(ruleNamesList, stop);
        }
        public virtual string ToString(IList<string> ruleNames, Antlr4.Runtime.RuleContext stop)
        {
            StringBuilder buf = new StringBuilder();
            Antlr4.Runtime.RuleContext p = this;
            buf.Append("[");
            while (p != null && p != stop)
            {
                if (ruleNames == null)
                {
                    if (!p.IsEmpty)
                    {
                        buf.Append(p.invokingState);
                    }
                }
                else
                {
                    int ruleIndex = p.RuleIndex;
                    string ruleName = ruleIndex >= 0 && ruleIndex < ruleNames.Count ? ruleNames[ruleIndex] : ruleIndex.ToString();
                    buf.Append(ruleName);
                }
                if (p.Parent != null && (ruleNames != null || !p.Parent.IsEmpty))
                {
                    buf.Append(" ");
                }
                p = p.Parent;
            }
            buf.Append("]");
            return buf.ToString();
        }
    }
}
