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
    internal class LexerTypeAction : ILexerAction
    {
        private readonly int type;
        public LexerTypeAction(int type)
        {
            this.type = type;
        }
        public virtual int Type
        {
            get
            {
                return type;
            }
        }
        public virtual LexerActionType ActionType
        {
            get
            {
                return LexerActionType.Type;
            }
        }
        public virtual bool IsPositionDependent
        {
            get
            {
                return false;
            }
        }
        public virtual void Execute(Lexer lexer)
        {
            lexer.Type = type;
        }
        public override int GetHashCode()
        {
            int hash = MurmurHash.Initialize();
            hash = MurmurHash.Update(hash, (int)(ActionType));
            hash = MurmurHash.Update(hash, type);
            return MurmurHash.Finish(hash, 2);
        }
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (!(obj is LexerTypeAction action))
            {
                return false;
            }
            return type == action.type;
        }
    }
}
