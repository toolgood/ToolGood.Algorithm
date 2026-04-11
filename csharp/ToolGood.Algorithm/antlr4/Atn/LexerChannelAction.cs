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
    internal sealed class LexerChannelAction : ILexerAction
    {
        private readonly int channel;
        public LexerChannelAction(int channel)
        {
            this.channel = channel;
        }
        public int Channel
        {
            get
            {
                return channel;
            }
        }
        public LexerActionType ActionType
        {
            get
            {
                return LexerActionType.Channel;
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
            lexer.Channel = channel;
        }
        public override int GetHashCode()
        {
            int hash = MurmurHash.Initialize();
            hash = MurmurHash.Update(hash, (int)(ActionType));
            hash = MurmurHash.Update(hash, channel);
            return MurmurHash.Finish(hash, 2);
        }
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (!(obj is LexerChannelAction action))
            {
                return false;
            }
            return channel == action.channel;
        }
    }
}
