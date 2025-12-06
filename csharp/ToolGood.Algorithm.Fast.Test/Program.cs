using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Fast.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PetaTest.Runner.RunMain(args);
        }
    }
}
