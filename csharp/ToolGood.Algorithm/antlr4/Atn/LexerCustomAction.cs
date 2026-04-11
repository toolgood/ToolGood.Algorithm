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
    internal sealed class LexerCustomAction : ILexerAction
    {
        private readonly int ruleIndex;
        private readonly int actionIndex;
        public LexerCustomAction(int ruleIndex, int actionIndex)
        {
            this.ruleIndex = ruleIndex;
            this.actionIndex = actionIndex;
        }
        public LexerActionType ActionType
        {
            get
            {
                return LexerActionType.Custom;
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
            lexer.Action(null, ruleIndex, actionIndex);
        }
        public override int GetHashCode()
        {
            int hash = MurmurHash.Initialize();
            hash = MurmurHash.Update(hash, (int)(ActionType));
            hash = MurmurHash.Update(hash, ruleIndex);
            hash = MurmurHash.Update(hash, actionIndex);
            return MurmurHash.Finish(hash, 3);
        }
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (!(obj is LexerCustomAction other))
            {
                return false;
            }
            return ruleIndex == other.ruleIndex && actionIndex == other.actionIndex;
        }
    }
}
