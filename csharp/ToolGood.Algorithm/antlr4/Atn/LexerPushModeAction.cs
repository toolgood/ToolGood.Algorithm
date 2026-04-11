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
    internal sealed class LexerPushModeAction : ILexerAction
    {
        private readonly int mode;
        public LexerPushModeAction(int mode)
        {
            this.mode = mode;
        }
        public int Mode
        {
            get
            {
                return mode;
            }
        }
        public LexerActionType ActionType
        {
            get
            {
                return LexerActionType.PushMode;
            }
        }
        public bool IsPositionDependent
        {
            get
            {
                return false;
            }
        }
        public void Execute(Lexer lexer)
        {
            lexer.PushMode(mode);
        }
        public override int GetHashCode()
        {
            int hash = MurmurHash.Initialize();
            hash = MurmurHash.Update(hash, (int)(ActionType));
            hash = MurmurHash.Update(hash, mode);
            return MurmurHash.Finish(hash, 2);
        }
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (!(obj is LexerPushModeAction action))
            {
                return false;
            }
            return mode == action.mode;
        }
    }
}
