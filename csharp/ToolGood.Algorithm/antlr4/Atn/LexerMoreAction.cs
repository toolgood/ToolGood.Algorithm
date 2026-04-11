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
    internal sealed class LexerMoreAction : ILexerAction
    {
        public static readonly Antlr4.Runtime.Atn.LexerMoreAction Instance = new Antlr4.Runtime.Atn.LexerMoreAction();
        private LexerMoreAction()
        {
        }
        public LexerActionType ActionType
        {
            get
            {
                return LexerActionType.More;
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
            lexer.More();
        }
        public override int GetHashCode()
        {
            int hash = MurmurHash.Initialize();
            hash = MurmurHash.Update(hash, (int)(ActionType));
            return MurmurHash.Finish(hash, 1);
        }
        public override bool Equals(object obj)
        {
            return obj == this;
        }
    }
}
