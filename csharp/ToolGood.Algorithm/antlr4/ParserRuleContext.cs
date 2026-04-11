/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
namespace Antlr4.Runtime
{
    internal class ParserRuleContext : RuleContext
    {
        public static readonly Antlr4.Runtime.ParserRuleContext EMPTY = new Antlr4.Runtime.ParserRuleContext();
        public IList<IParseTree> children;
        private IToken _start;
        private IToken _stop;
        public RecognitionException exception;
        public ParserRuleContext()
        {
        }
        public static Antlr4.Runtime.ParserRuleContext EmptyContext
        {
            get
            {
                return EMPTY;
            }
        }
        public virtual void CopyFrom(Antlr4.Runtime.ParserRuleContext ctx)
        {
            this.Parent = ctx.Parent;
            this.invokingState = ctx.invokingState;
            this._start = ctx._start;
            this._stop = ctx._stop;
            if (ctx.children != null)
            {
                children = new List<IParseTree>();
                foreach (var child in ctx.children)
                {
                    var errorChildNode = child as ErrorNodeImpl;
                    if (errorChildNode != null)
                    {
                        children.Add(errorChildNode);
                        errorChildNode.Parent = this;
                    }
                }
            }
        }
        public ParserRuleContext(Antlr4.Runtime.ParserRuleContext parent, int invokingStateNumber)
            : base(parent, invokingStateNumber)
        {
        }
        public virtual void EnterRule(IParseTreeListener listener)
        {
        }
        public virtual void ExitRule(IParseTreeListener listener)
        {
        }
        public virtual void AddChild(ITerminalNode t)
        {
            if (children == null)
            {
                children = new List<IParseTree>();
            }
            children.Add(t);
        }
        public virtual void AddChild(RuleContext ruleInvocation)
        {
            if (children == null)
            {
                children = new List<IParseTree>();
            }
            children.Add(ruleInvocation);
        }
        public virtual void RemoveLastChild()
        {
            if (children != null)
            {
                children.RemoveAt(children.Count - 1);
            }
        }
        public virtual ITerminalNode AddChild(IToken matchedToken)
        {
            TerminalNodeImpl t = new TerminalNodeImpl(matchedToken);
            AddChild(t);
            t.Parent = this;
            return t;
        }
        public virtual IErrorNode AddErrorNode(IToken badToken)
        {
            ErrorNodeImpl t = new ErrorNodeImpl(badToken);
            AddChild(t);
            t.Parent = this;
            return t;
        }

        public override IParseTree GetChild(int i)
        {
            return children != null && i >= 0 && i < children.Count ? children[i] : null;
        }
        public virtual T GetChild<T>(int i)
            where T : IParseTree
        {
            if (children == null || i < 0 || i >= children.Count)
            {
                return default(T);
            }
            int j = -1;
            foreach (IParseTree o in children)
            {
                if (o is T tree)
                {
                    j++;
                    if (j == i)
                    {
                        return tree;
                    }
                }
            }
            return default(T);
        }
        public virtual ITerminalNode GetToken(int ttype, int i)
        {
            if (children == null || i < 0 || i >= children.Count)
            {
                return null;
            }
            int j = -1;
            foreach (IParseTree o in children)
            {
                if (o is ITerminalNode tnode)
                {
                    IToken symbol = tnode.Symbol;
                    if (symbol.Type == ttype)
                    {
                        j++;
                        if (j == i)
                        {
                            return tnode;
                        }
                    }
                }
            }
            return null;
        }
        public virtual T GetRuleContext<T>(int i)
            where T : Antlr4.Runtime.ParserRuleContext
        {
            return GetChild<T>(i);
        }
        public virtual T[] GetRuleContexts<T>()
            where T : Antlr4.Runtime.ParserRuleContext
        {
            if (children == null)
            {
                return Collections.EmptyList<T>();
            }
            List<T> contexts = null;
            foreach (IParseTree o in children)
            {
                if (o is T context)
                {
                    if (contexts == null)
                    {
                        contexts = new List<T>();
                    }
                    contexts.Add(context);
                }
            }
            if (contexts == null)
            {
                return Collections.EmptyList<T>();
            }
            return contexts.ToArray();
        }
        public override int ChildCount
        {
            get { return children != null ? children.Count : 0; }
        }
        public override Interval SourceInterval
        {
            get
            {
                if (_start == null || _stop == null)
                {
                    return Interval.Invalid;
                }
                return Interval.Of(_start.TokenIndex, _stop.TokenIndex);
            }
        }
        public virtual IToken Start
        {
            get { return _start; }
            set { _start = value; }
        }
        public virtual IToken Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }
    }
}
