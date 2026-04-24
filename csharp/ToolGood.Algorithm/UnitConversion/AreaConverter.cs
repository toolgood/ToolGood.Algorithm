using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class AreaConverter 
    {
		private static readonly Dictionary<string, decimal> units2 = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase) {
			{"m²",1 }, {"m2",1 }, {"square metre",1 },{"square meter",1 },{"centiare",1 },{"平方米",1 },{"平方公尺",1 },
			{"km²",0.000001m }, {"km2",0.000001m }, {"square kilometre",0.000001m },{"square kilometer",0.000001m },{"平方千米",0.000001m },
			{"dm²",100 }, {"dm2",100 }, {"square decimetre",100 },{"square decimeter",100 },{"平方分米",100 },
			{"cm²",10000 }, {"cm2",10000 }, {"square centimetre",10000 },{"square centimeter",10000 },{"平方厘米",10000 },
			{"mm²",1000000 }, {"mm2",1000000 }, {"square millimetre",1000000 },{"square millimeter",1000000 },{"平方毫米",1000000 },
			{"ft²",1m /  0.3048m /  0.3048m }, {"ft2",1m /  0.3048m /  0.3048m }, {"square foot",1m /  0.3048m /  0.3048m },{"square feet",1m /  0.3048m /  0.3048m },{"sq ft",1m /  0.3048m /  0.3048m },{"平方英尺",1m /  0.3048m /  0.3048m },
			{"yd²",1m /  0.9144m /  0.9144m }, {"yd2",1m /  0.9144m /  0.9144m }, {"sq yd",1m /  0.9144m /  0.9144m },{"square yard",1m /  0.9144m /  0.9144m },{"平方码",1m /  0.9144m /  0.9144m },
			{"a",0.01m  },{"are",0.01m  },
			{"ha",0.0001m }, {"hectare",0.0001m }, {"公顷",0.0001m },
			{"in²",1m / 0.00064516m }, {"in2",1m / 0.00064516m }, {"sq in",1m / 0.00064516m },{"square inch",1m / 0.00064516m },{"平方英寸",1m / 0.00064516m },
			{"mi²",1m / 2589988.110336m }, {"mi2",1m / 2589988.110336m }, {"sq mi",1m / 2589988.110336m },{"square mile",1m / 2589988.110336m },{"平方英里",1m / 2589988.110336m },
			{"亩",1m / 666.667m  },
		};

		public static bool TryConvert(string leftSynonym, string rightSynonym, decimal left, out decimal right)
		{
			if(units2.TryGetValue(leftSynonym, out decimal l)) {
				if(units2.TryGetValue(rightSynonym, out decimal r)) {
					right = left / l * r;
					return true;
				}
			}
			right = decimal.Zero;
			return false;
		}

	 
    }
}