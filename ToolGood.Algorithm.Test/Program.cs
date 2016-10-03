using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //RPN rpn = new RPN();
            //rpn.Parse("tan(1)+2+333");


            AlgorithmEngine engine = new AlgorithmEngine();
            if (engine.Parse("countif({0,1,2,3,-1},'>1')")) {
                var t = engine.Evaluate();
            }


            if (engine.Parse("min(1,{0,1,2,3,-1})")) {
                var t = engine.Evaluate();
            }

            if (engine.Parse("NORMINV(0.369441340181764,5,3)")) {
                var t = engine.Evaluate();
            }

            if (engine.Parse("NORMDIST(4,5,3,0)")) {
                var t = engine.Evaluate();
            }

            if (engine.Parse("'2016-1-1'+'12:22'")) {
                var t = engine.Evaluate();
            }

            if (engine.Parse("'sss'+'bbb'")) {
                var t = engine.Evaluate();
            }

            if (engine.Parse("acosh(2)")) {
                var t = engine.Evaluate();
            }



            if (engine.Parse("1+sum(1,2,3)")) {
                var t = engine.Evaluate();
            }

            if (engine.Parse("1+2-3+4*5+5/(9-8)+sum(1,2,3)")) {
                var t = engine.Evaluate();
            }



            //var t = engine.Parse("[11]* (1+(1*2+[112]))+ld(11)+'\"222'");
            //var count = t.Count;
        }
    }
}
