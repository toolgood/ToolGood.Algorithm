/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using System;
namespace Antlr4.Runtime
{
    public class DefaultErrorStrategy : IAntlrErrorStrategy
    {
        protected internal bool errorRecoveryMode = false;
        protected internal int lastErrorIndex = -1;
        protected internal IntervalSet lastErrorStates;
        /**
         * This field is used to propagate information about the lookahead following
         * the previous match. Since prediction prefers completing the current rule
         * to error recovery efforts, error reporting may occur later than the
         * original point where it was discoverable. The original context is used to
         * compute the true expected sets as though the reporting occurred as early
         * as possible.
         */
        protected ParserRuleContext nextTokensContext;
        public virtual void Reset(Parser recognizer)
        {
            EndErrorCondition(recognizer);
        }
        protected internal virtual void BeginErrorCondition(Parser recognizer)
        {
            errorRecoveryMode = true;
        }
        public virtual bool InErrorRecoveryMode(Parser recognizer)
        {
            return errorRecoveryMode;
        }
        protected internal virtual void EndErrorCondition(Parser recognizer)
        {
            errorRecoveryMode = false;
            lastErrorStates = null;
            lastErrorIndex = -1;
        }
        public virtual void ReportMatch(Parser recognizer)
        {
            EndErrorCondition(recognizer);
        }
        public virtual void ReportError(Parser recognizer, RecognitionException e)
        {
            if (InErrorRecoveryMode(recognizer))
            {
                return;
            }
            BeginErrorCondition(recognizer);
            if (e is NoViableAltException)
            {
                ReportNoViableAlternative(recognizer, (NoViableAltException)e);
            }
            else
            {
                if (e is InputMismatchException)
                {
                    ReportInputMismatch(recognizer, (InputMismatchException)e);
                }
                else
                {
                    {
                        System.Console.Error.WriteLine("unknown recognition error type: " + e.GetType().FullName);
                        NotifyErrorListeners(recognizer, e.Message, e);
                    }
                }
            }
        }
        protected internal virtual void NotifyErrorListeners(Parser recognizer, string message, RecognitionException e)
        {
            recognizer.NotifyErrorListeners(e.OffendingToken, message, e);
        }
        public virtual void Recover(Parser recognizer, RecognitionException e)
        {
            if (lastErrorIndex == ((ITokenStream)recognizer.InputStream).Index && lastErrorStates != null && lastErrorStates.Contains(recognizer.State))
            {
                recognizer.Consume();
            }
            lastErrorIndex = ((ITokenStream)recognizer.InputStream).Index;
            if (lastErrorStates == null)
            {
                lastErrorStates = new IntervalSet();
            }
            lastErrorStates.Add(recognizer.State);
            IntervalSet followSet = GetErrorRecoverySet(recognizer);
            ConsumeUntil(recognizer, followSet);
        }
        public virtual void Sync(Parser recognizer)
        {
            ATNState s = recognizer.Interpreter.atn.states[recognizer.State];
            if (InErrorRecoveryMode(recognizer))
            {
                return;
            }
            ITokenStream tokens = ((ITokenStream)recognizer.InputStream);
            int la = tokens.LA(1);
            var nextTokens = recognizer.Atn.NextTokens(s);
            if (nextTokens.Contains(la))
            {
                nextTokensContext = null;
                return;
            }
            if (nextTokens.Contains(TokenConstants.EPSILON))
            {
                if (nextTokensContext == null)
                {
                    nextTokensContext = recognizer.Context;
                }
                return;
            }
            switch (s.StateType)
            {
                case StateType.BlockStart:
                case StateType.StarBlockStart:
                case StateType.PlusBlockStart:
                case StateType.StarLoopEntry:
                {
                    if (SingleTokenDeletion(recognizer) != null)
                    {
                        return;
                    }
                    throw new InputMismatchException(recognizer);
                }
                case StateType.PlusLoopBack:
                case StateType.StarLoopBack:
                {
                    ReportUnwantedToken(recognizer);
                    IntervalSet expecting = recognizer.GetExpectedTokens();
                    IntervalSet whatFollowsLoopIterationOrRule = expecting.Or(GetErrorRecoverySet(recognizer));
                    ConsumeUntil(recognizer, whatFollowsLoopIterationOrRule);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        protected internal virtual void ReportNoViableAlternative(Parser recognizer, NoViableAltException e)
        {
            ITokenStream tokens = ((ITokenStream)recognizer.InputStream);
            string input;
            if (tokens != null)
            {
                if (e.StartToken.Type == TokenConstants.EOF)
                {
                    input = "<EOF>";
                }
                else
                {
                    input = tokens.GetText(e.StartToken, e.OffendingToken);
                }
            }
            else
            {
                input = "<unknown input>";
            }
            string msg = "no viable alternative at input " + EscapeWSAndQuote(input);
            NotifyErrorListeners(recognizer, msg, e);
        }
        protected internal virtual void ReportInputMismatch(Parser recognizer, InputMismatchException e)
        {
            string msg = "mismatched input " + GetTokenErrorDisplay(e.OffendingToken) + " expecting ";
            NotifyErrorListeners(recognizer, msg, e);
        }
        protected internal virtual void ReportUnwantedToken(Parser recognizer)
        {
            if (InErrorRecoveryMode(recognizer))
            {
                return;
            }
            BeginErrorCondition(recognizer);
            IToken t = recognizer.CurrentToken;
            string tokenName = GetTokenErrorDisplay(t);
            IntervalSet expecting = GetExpectedTokens(recognizer);
            string msg = "extraneous input " + tokenName + " expecting " + expecting.ToString(recognizer.Vocabulary);
            recognizer.NotifyErrorListeners(t, msg, null);
        }
        protected internal virtual void ReportMissingToken(Parser recognizer)
        {
            if (InErrorRecoveryMode(recognizer))
            {
                return;
            }
            BeginErrorCondition(recognizer);
            IToken t = recognizer.CurrentToken;
            IntervalSet expecting = GetExpectedTokens(recognizer);
            string msg = "missing " + expecting.ToString(recognizer.Vocabulary) + " at " + GetTokenErrorDisplay(t);
            recognizer.NotifyErrorListeners(t, msg, null);
        }
        public virtual IToken RecoverInline(Parser recognizer)
        {
            IToken matchedSymbol = SingleTokenDeletion(recognizer);
            if (matchedSymbol != null)
            {
                recognizer.Consume();
                return matchedSymbol;
            }
            if (SingleTokenInsertion(recognizer))
            {
                return GetMissingSymbol(recognizer);
            }
            throw new InputMismatchException(recognizer);
        }
        protected internal virtual bool SingleTokenInsertion(Parser recognizer)
        {
            int currentSymbolType = ((ITokenStream)recognizer.InputStream).LA(1);
            ATNState currentState = recognizer.Interpreter.atn.states[recognizer.State];
            ATNState next = currentState.Transition(0).target;
            ATN atn = recognizer.Interpreter.atn;
			IntervalSet expectingAtLL2 = atn.NextTokens(next, recognizer.RuleContext);
            if (expectingAtLL2.Contains(currentSymbolType))
            {
                ReportMissingToken(recognizer);
                return true;
            }
            return false;
        }
        protected internal virtual IToken SingleTokenDeletion(Parser recognizer)
        {
            int nextTokenType = ((ITokenStream)recognizer.InputStream).LA(2);
            IntervalSet expecting = GetExpectedTokens(recognizer);
            if (expecting.Contains(nextTokenType))
            {
                ReportUnwantedToken(recognizer);
                recognizer.Consume();
                IToken matchedSymbol = recognizer.CurrentToken;
                ReportMatch(recognizer);
                return matchedSymbol;
            }
            return null;
        }
        protected internal virtual IToken GetMissingSymbol(Parser recognizer)
        {
            IToken currentSymbol = recognizer.CurrentToken;
            IntervalSet expecting = GetExpectedTokens(recognizer);
            int expectedTokenType = expecting.MinElement;
            string tokenText;
            if (expectedTokenType == TokenConstants.EOF)
            {
                tokenText = "<missing EOF>";
            }
            else
            {
                tokenText = "<missing " + recognizer.Vocabulary.GetDisplayName(expectedTokenType) + ">";
            }
            IToken current = currentSymbol;
            IToken lookback = ((ITokenStream)recognizer.InputStream).LT(-1);
            if (current.Type == TokenConstants.EOF && lookback != null)
            {
                current = lookback;
            }
            return ConstructToken(((ITokenStream)recognizer.InputStream).TokenSource, expectedTokenType, tokenText, current);
        }
        protected internal virtual IToken ConstructToken(ITokenSource tokenSource, int expectedTokenType, string tokenText, IToken current)
        {
            ITokenFactory factory = tokenSource.TokenFactory;
            return factory.Create(Tuple.Create(tokenSource, current.TokenSource.InputStream), expectedTokenType, tokenText, TokenConstants.DefaultChannel, -1, -1, current.Line, current.Column);
        }
        protected internal virtual IntervalSet GetExpectedTokens(Parser recognizer)
        {
            return recognizer.GetExpectedTokens();
        }
        protected internal virtual string GetTokenErrorDisplay(IToken t)
        {
            if (t == null)
            {
                return "<no token>";
            }
            string s = GetSymbolText(t);
            if (s == null)
            {
                if (GetSymbolType(t) == TokenConstants.EOF)
                {
                    s = "<EOF>";
                }
                else
                {
                    s = "<" + GetSymbolType(t) + ">";
                }
            }
            return EscapeWSAndQuote(s);
        }
        protected internal virtual string GetSymbolText(IToken symbol)
        {
            return symbol.Text;
        }
        protected internal virtual int GetSymbolType(IToken symbol)
        {
            return symbol.Type;
        }
        protected internal virtual string EscapeWSAndQuote(string s)
        {
            s = s.Replace("\n", "\\n");
            s = s.Replace("\r", "\\r");
            s = s.Replace("\t", "\\t");
            return "'" + s + "'";
        }
        protected internal virtual IntervalSet GetErrorRecoverySet(Parser recognizer)
        {
            ATN atn = recognizer.Interpreter.atn;
			RuleContext ctx = recognizer.RuleContext;
            IntervalSet recoverSet = new IntervalSet();
            while (ctx != null && ctx.invokingState >= 0)
            {
                ATNState invokingState = atn.states[ctx.invokingState];
                RuleTransition rt = (RuleTransition)invokingState.Transition(0);
                IntervalSet follow = atn.NextTokens(rt.followState);
                recoverSet.AddAll(follow);
                ctx = ctx.Parent;
            }
            recoverSet.Remove(TokenConstants.EPSILON);
            return recoverSet;
        }
        protected internal virtual void ConsumeUntil(Parser recognizer, IntervalSet set)
        {
            int ttype = ((ITokenStream)recognizer.InputStream).LA(1);
            while (ttype != TokenConstants.EOF && !set.Contains(ttype))
            {
                recognizer.Consume();
                ttype = ((ITokenStream)recognizer.InputStream).LA(1);
            }
        }
    }
}
