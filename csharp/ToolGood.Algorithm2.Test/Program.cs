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

   


            double a = 0.0;
            if (engine.Parse("1+2")) {
                var o = engine.Evaluate();
                a = o.NumberValue;
            }

            var b = engine.TryEvaluate("1=1 && 1<2 and 7-8>1", 0);// Support(支持) && || and or 
            var c = engine.TryEvaluate("2+3", 0);
            var d = engine.TryEvaluate("count(array(1,2,3,4))", 0);//{} represents array, return: 4 {}代表数组,返回:4
            var s = engine.TryEvaluate("'aa'&'bb'", ""); //String connection, return: AABB 字符串连接,返回:aabb
            var r = engine.TryEvaluate("(1=1)*9+2", 0); //Return: 11 返回:11
            var e = engine.TryEvaluate("'2016-1-1'+1", DateTime.MinValue); //Return date: 2016-1-2 返回日期:2016-1-2
            var t = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.MinValue);//Return datetime:2016-1-1 9:0  返回日期:2016-1-1 9:0
            var j = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", null);//Return 51 返回51
            var k = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Name].Trim()", null);//Return to "William Shakespeare"  返回"William Shakespeare" (不带空格)
            var l = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null); ;//Return "ill"  返回"ill"



            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PetaTest.Runner.RunMain(args);

        }
    }
}
