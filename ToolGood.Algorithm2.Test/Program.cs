using System;
using System.Text;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PetaTest.Runner.RunMain(args);

        }
    }
}
