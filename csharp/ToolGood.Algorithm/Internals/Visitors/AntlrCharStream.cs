using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace ToolGood.Algorithm.Internals.Visitors
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
        private readonly ICharStream stream;

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
        /// 获取当前字符流位置
        /// </summary>
        public int Index => stream.Index;

        /// <summary>
        /// 获取字符流大小
        /// </summary>
        public int Size => stream.Size;

        /// <summary>
        /// 获取源名称
        /// </summary>
        public string SourceName => stream.SourceName;

        /// <summary>
        /// 消费字符
        /// </summary>
        public void Consume()
        {
            stream.Consume();
        }

        /// <summary>
        /// 获取指定区间的文本
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public string GetText(Interval interval)
        {
            return stream.GetText(interval);
        }

        /// <summary>
        /// 查看当前位置的字符
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
        /// 标记当前位置
        /// </summary>
        /// <returns></returns>
        public int Mark()
        {
            return stream.Mark();
        }

        /// <summary>
        /// 释放标记
        /// </summary>
        /// <param name="marker"></param>
        public void Release(int marker)
        {
            stream.Release(marker);
        }

        /// <summary>
        /// 设置读取位置
        /// </summary>
        /// <param name="index"></param>
        public void Seek(int index)
        {
            stream.Seek(index);
        }
    }
}