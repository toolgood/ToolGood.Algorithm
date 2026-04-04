using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.UnitConversion
{
	internal sealed class DistanceConverter
	{
		private static Dictionary<string, decimal> units2 = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase) {
			{"m",1 }, {"metre",1 }, {"米",1 },
			{"km",0.001m },{"kilometre",0.001m },{"千米",0.001m },
			{"dm",10 },{"decimetre",10 },{"分米",10 },
			{"cm",100 },{"centimetre",100 },{"厘米",100 },
			{"mm",1000 },{"millimetre",1000 },{"毫米",1000 },
			{"ft",1250m / 381 },{"foot",1250m / 381 },{"feet",1250m / 381 },{"英尺",1250m / 381 },
			{"yd",1250m / 1143 },{"yard",1250m / 1143 },{"码",1250m / 1143 },
			{"mile",125m / 201168  },{"英里",125m / 201168  },
			{"in",5000m / 127 },{"inch",5000m / 127 },{"英寸",5000m / 127 },
			{"au", 1m / 149600000000 },
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