/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    public interface IIntStream
    {
        void Consume();
        int LA(int i);
        int Mark();
        void Release(int marker);
        int Index
        {
            get;
        }
        void Seek(int index);
        int Size
        {
            get;
        }
        string SourceName
        {
            get;
        }
    }
    public static class IntStreamConstants
    {
        public const int EOF = -1;
        public const string UnknownSourceName = "<unknown>";
    }
}
