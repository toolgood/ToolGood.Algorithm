using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class MassConverter 
    {
		private static readonly Dictionary<string, decimal> units2 = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase) {
			{"kg",1 }, {"kilogram",1 }, {"千克",1 },
			{"gram",1000 }, {"g",1000 }, {"克",1000 },
			{"ton",1M/1000 }, {"t",1M/1000 }, {"吨",1M/1000 },
			{"lb",100000000m / 45359237 }, {"lbs",100000000m / 45359237 }, {"pound",100000000m / 45359237 },{"pounds",100000000m / 45359237 },{"磅",100000000m / 45359237 },{"英磅",100000000m / 45359237 },
			{"st",50000000m / 317514659 }, {"stone",50000000m / 317514659 }, {"石",50000000m / 317514659 },
			{"oz",1600000000m / 45359237  }, {"ounce",1600000000m / 45359237  }, {"盎司",1600000000m / 45359237  },
			{"quintal",0.01m  }, {"英担",0.01m  },
			{"short ton",50000m / 45359237 }, {"net ton",50000m / 45359237 }, {"us ton",50000m / 45359237 },{"短吨",50000m / 45359237 },{"美吨",50000m / 45359237 },
			{"long ton",312500m / 317514659 }, {"weight ton",312500m / 317514659 }, {"gross ton",312500m / 317514659},{"imperial ton",312500m / 317514659},{"长吨",312500m / 317514659 },{"英吨",312500m / 317514659},
			{"mg",1000000 }, {"毫克",1000000 },
			{"斤",2 }, {"两",20 },
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