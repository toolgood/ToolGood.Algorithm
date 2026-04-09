/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    public interface IToken
    {
        string Text
        {
            get;
        }
        int Type
        {
            get;
        }
        int Line
        {
            get;
        }
        int Column
        {
            get;
        }
        int Channel
        {
            get;
        }
        int TokenIndex
        {
            get;
        }
        int StartIndex
        {
            get;
        }
        int StopIndex
        {
            get;
        }
        ITokenSource TokenSource
        {
            get;
        }
        ICharStream InputStream
        {
            get;
        }
    }
    public static class TokenConstants
    {
        public const int InvalidType = 0;
        public const int EPSILON = -2;
        public const int MinUserTokenType = 1;
        public const int EOF = IntStreamConstants.EOF;
        public const int DefaultChannel = 0;
        public const int HiddenChannel = 1;
        public const int MinUserChannelValue = 2;
    }
}
