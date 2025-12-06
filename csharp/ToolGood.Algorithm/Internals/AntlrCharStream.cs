/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace ToolGood.Algorithm.Internals
{
    /// <summary>
    /// This class supports case-insensitive lexing by wrapping an existing
    /// <see cref="ICharStream"/> and forcing the lexer to see either upper or
    /// lowercase characters. Grammar literals should then be either upper or
    /// lower case such as 'BEGIN' or 'begin'. The text of the character
    /// stream is unaffected. Example: input 'BeGiN' would match lexer rule
    /// 'BEGIN' if constructor parameter upper=true but getText() would return
    /// 'BeGiN'.
    /// </summary>
    sealed class AntlrCharStream : ICharStream
    {
        private ICharStream stream;

        /// <summary>
        /// Constructs a new CaseChangingCharStream wrapping the given <paramref name="stream"/> forcing
        /// all characters to upper case or lower case.
        /// </summary>
        /// <param name="stream">The stream to wrap.</param>
        public AntlrCharStream(ICharStream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        ///
        /// </summary>
        public int Index {
            get {
                return stream.Index;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int Size {
            get {
                return stream.Size;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string SourceName {
            get {
                return stream.SourceName;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Consume()
        {
            stream.Consume();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        [return: NotNull]
        public string GetText(Interval interval)
        {
            return stream.GetText(interval);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int LA(int i)
        {
            int c = stream.LA(i);

            if (c <= 0) {
                return c;
            }
            return CharUtil.StandardChar((char)c);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int Mark()
        {
            return stream.Mark();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="marker"></param>
        public void Release(int marker)
        {
            stream.Release(marker);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        public void Seek(int index)
        {
            stream.Seek(index);
        }
    }
}