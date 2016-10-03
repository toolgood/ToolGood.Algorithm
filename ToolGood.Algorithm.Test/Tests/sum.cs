using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;
using ToolGood.Algorithm.MathNet.Numerics;
using ToolGood.Algorithm.MathNet.Numerics.Distributions;

namespace ToolGood.Algorithm.Test.Tests
{
    [TestFixture]
    class sum
    {
        public void d()
        {
            Dictionary<int, double> dict = new Dictionary<int, double>();
            int index = 0;
            //=EXPONDIST(0.6,2,1)
            //0.698805788
            dict[index++] = ExcelFunctions.ExponDist(0.6, 2, true);
            //=EXPONDIST(0.6,2,0)
            //0.602388424
            dict[index++] = ExcelFunctions.ExponDist(0.6, 2, false);

            //=HYPGEOMDIST(1,7,3,45)
            //0.346793517
            dict[index++] = ExcelFunctions.HypgeomDist(1, 7, 3, 45);

            //=NEGBINOMDIST(34,45,0.67)
            //0.009164036 0.00916403565521149]}
            dict[index++] = ExcelFunctions.NegbinomDist(34, 45, 0.67);


            //=LOGNORMDIST(1,4,7) 
            //0.283854583   0.283854583098676]}
            //dict[index++] = LogNormal.CDF(4, 7, 1);
            dict[index++] = ExcelFunctions.LognormDist(1, 4, 7);
            //dict[index++] = LogNormal.InvCDF(4, 7, 0.283854583);
            dict[index++] = ExcelFunctions.LogInv(0.283854583, 4, 7);

            //Binomial
            //=BINOMDIST(23,50,0.5,0)
            //0.095961686
            //dict[index++] = Binomial.PMF(0.5, 50, 23);
            dict[index++] = ExcelFunctions.BinomDist(23, 50, 0.5, false);


            //=BINOMDIST(23,50,0.5,1)
            //0.335905517
            //dict[index++] = Binomial.CDF(0.5, 50, 23);
            dict[index++] = ExcelFunctions.BinomDist(23, 50, 0.5, true);


            //=POISSON(1,5,0)
            //0.033689735
            //dict[index++] = Poisson.PMF(5, 1);
            dict[index++] = ExcelFunctions.POISSON(1, 5, false);


            //=POISSON(1,5,1)
            //0.040427682
            //dict[index++] = Poisson.CDF(5, 1);
            dict[index++] = ExcelFunctions.POISSON(1, 5, true);

            //=WEIBULL(1,2,3,0)
            //            0.198853182
            //dict[index++] = Weibull.PDF(2, 3, 1);
            dict[index++] = ExcelFunctions.WEIBULL(1, 2, 3, false);


            //=WEIBULL(1,2,3,1)
            //0.105160683
            //dict[index++] = Weibull.CDF(2, 3, 1);
            dict[index++] = ExcelFunctions.WEIBULL(1, 2, 3, true);

        }


    }
}
