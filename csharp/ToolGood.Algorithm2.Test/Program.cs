using System;
using System.Globalization;
using System.Text;
using System.Threading;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            double t = 0.0;
            //if (engine.Parse("1+2[][]")) {
            //    t = (double)engine.Evaluate().NumberValue;
            //}

            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PetaTest.Runner.RunMain(args);

        }
    }
}
