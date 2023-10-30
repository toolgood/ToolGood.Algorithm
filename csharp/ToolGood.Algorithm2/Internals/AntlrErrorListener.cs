using Antlr4.Runtime;
using System.IO;

namespace ToolGood.Algorithm.Internals
{
    /// <summary>
    /// 自定义ErrorListener
    /// </summary>
    public sealed class AntlrErrorListener : IAntlrErrorListener<IToken>
    {
        /// <summary>
        /// 是否出错
        /// </summary>
        public bool IsError { get; private set; }
        /// <summary>
        /// 出错信息
        /// </summary>
        public string ErrorMsg { get; private set; }
        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="output"></param>
        /// <param name="recognizer"></param>
        /// <param name="offendingSymbol"></param>
        /// <param name="line"></param>
        /// <param name="charPositionInLine"></param>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            IsError = true;
            ErrorMsg = msg;
        }
    }
}
