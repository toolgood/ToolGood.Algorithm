namespace ToolGood.Algorithm.WebAssembly
{
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.JSInterop;
    using ToolGood.Algorithm;
    public class Program
    {
        private static async Task Main(string[] args)
        {
        }

       
        ///// <summary>
        ///// 给js调用的函数
        ///// </summary>
        ///// <param name="i"></param>
        ///// <param name="j"></param>
        ///// <returns></returns>
        //[JSInvokable]
        //public static byte[] Add(int i, int j)
        //{
        //    return new byte[] { (byte)i, (byte)j };
        //}

        [JSInvokable]
        public static string TryEvaluate(string exp)
        {
            var ae = new AlgorithmEngine();
            return ae.TryEvaluate(exp, null);
        }
    }
}
