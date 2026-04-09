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
    public sealed class LexerIndexedCustomAction : ILexerAction
    {
        private readonly int offset;
        private readonly ILexerAction action;
        public LexerIndexedCustomAction(int offset, ILexerAction action)
        {
            this.offset = offset;
            this.action = action;
        }
        public int Offset
        {
            get
            {
                return offset;
            }
        }
        public ILexerAction Action
        {
            get
            {
                return action;
            }
        }
        public LexerActionType ActionType
        {
            get
            {
                return action.ActionType;
            }
        }
        public bool IsPositionDependent
        {
            get
            {
                return true;
            }
        }
        public void Execute(Lexer lexer)
        {
            action.Execute(lexer);
        }
        public override int GetHashCode()
        {
            int hash = MurmurHash.Initialize();
            hash = MurmurHash.Update(hash, offset);
            hash = MurmurHash.Update(hash, action);
            return MurmurHash.Finish(hash, 2);
        }
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            else
            {
                if (!(obj is Antlr4.Runtime.Atn.LexerIndexedCustomAction))
                {
                    return false;
                }
            }
            Antlr4.Runtime.Atn.LexerIndexedCustomAction other = (Antlr4.Runtime.Atn.LexerIndexedCustomAction)obj;
            return offset == other.offset && action.Equals(other.action);
        }
    }
}
