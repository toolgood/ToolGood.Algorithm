/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    public class LexerActionExecutor
    {
        private readonly ILexerAction[] lexerActions;
        private readonly int hashCode;
        public LexerActionExecutor(ILexerAction[] lexerActions)
        {
            this.lexerActions = lexerActions;
            int hash = MurmurHash.Initialize();
            foreach (ILexerAction lexerAction in lexerActions)
            {
                hash = MurmurHash.Update(hash, lexerAction);
            }
            this.hashCode = MurmurHash.Finish(hash, lexerActions.Length);
        }
        public static Antlr4.Runtime.Atn.LexerActionExecutor Append(Antlr4.Runtime.Atn.LexerActionExecutor lexerActionExecutor, ILexerAction lexerAction)
        {
            if (lexerActionExecutor == null)
            {
                return new Antlr4.Runtime.Atn.LexerActionExecutor(new ILexerAction[] { lexerAction });
            }
            ILexerAction[] lexerActions = Arrays.CopyOf(lexerActionExecutor.lexerActions, lexerActionExecutor.lexerActions.Length + 1);
            lexerActions[lexerActions.Length - 1] = lexerAction;
            return new Antlr4.Runtime.Atn.LexerActionExecutor(lexerActions);
        }
        public virtual Antlr4.Runtime.Atn.LexerActionExecutor FixOffsetBeforeMatch(int offset)
        {
            ILexerAction[] updatedLexerActions = null;
            for (int i = 0; i < lexerActions.Length; i++)
            {
                if (lexerActions[i].IsPositionDependent && !(lexerActions[i] is LexerIndexedCustomAction))
                {
                    if (updatedLexerActions == null)
                    {
                        updatedLexerActions = (ILexerAction[])lexerActions.Clone();
                    }
                    updatedLexerActions[i] = new LexerIndexedCustomAction(offset, lexerActions[i]);
                }
            }
            if (updatedLexerActions == null)
            {
                return this;
            }
            return new Antlr4.Runtime.Atn.LexerActionExecutor(updatedLexerActions);
        }
        public virtual void Execute(Lexer lexer, ICharStream input, int startIndex)
        {
            bool requiresSeek = false;
            int stopIndex = input.Index;
            try
            {
                foreach (ILexerAction lexerAction in lexerActions)
                {
                    ILexerAction action = lexerAction;
                    if (action is LexerIndexedCustomAction)
                    {
                        int offset = ((LexerIndexedCustomAction)action).Offset;
                        input.Seek(startIndex + offset);
                        action = ((LexerIndexedCustomAction)action).Action;
                        requiresSeek = (startIndex + offset) != stopIndex;
                    }
                    else
                    {
                        if (action.IsPositionDependent)
                        {
                            input.Seek(stopIndex);
                            requiresSeek = false;
                        }
                    }
                    action.Execute(lexer);
                }
            }
            finally
            {
                if (requiresSeek)
                {
                    input.Seek(stopIndex);
                }
            }
        }
        public override int GetHashCode()
        {
            return this.hashCode;
        }
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            else
            {
                if (!(obj is Antlr4.Runtime.Atn.LexerActionExecutor))
                {
                    return false;
                }
            }
            Antlr4.Runtime.Atn.LexerActionExecutor other = (Antlr4.Runtime.Atn.LexerActionExecutor)obj;
            return hashCode == other.hashCode && Arrays.Equals(lexerActions, other.lexerActions);
        }
    }
}
