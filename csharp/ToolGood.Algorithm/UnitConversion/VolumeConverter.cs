using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class VolumeConverter 
    {
		private static readonly Dictionary<string, decimal> units2 = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase) {
			{"l",1 },{"lt",1 },{"ltr",1 },{"liter",1 },{"litre",1 },
			{"dm³",1 }, {"dm3",1 }, {"cubic decimetre",1 },{"cubic decimeter",1 },{"升",1 },{"立方分米",1 },
			{"m³",0.001m }, {"m3",0.001m }, {"cubic metre",0.001m },{"cubic meter",0.001m },{"立方米",0.001m },
			{"km³",0.001m* 0.001m* 0.001m* 0.001m }, {"km3",0.001m* 0.001m* 0.001m* 0.001m }, {"cubic kilometre",0.001m* 0.001m* 0.001m* 0.001m },{"cubic kilometer",0.001m* 0.001m* 0.001m* 0.001m },{"立方千米",0.001m* 0.001m* 0.001m* 0.001m },
			{"cm³",1000 }, {"cm3",1000 }, {"cubic centimetre",1000 },{"cubic centimeter",1000 },{"立方厘米",1000 },{"毫升",1000 },{"ml",1000 },
			{"mm³",1000000 }, {"mm3",1000000 }, {"cubic millimetre",1000000 },{"cubic millimeter",1000000 },{"立方毫米",1000000 },
			{"ft³",1m / 28.316846592m }, {"ft3",1m / 28.316846592m }, {"cubic foot",1m / 28.316846592m },{"cubic feet",1m / 28.316846592m },{"立方英尺",1m / 28.316846592m },{"cu ft",1m / 28.316846592m },
			{"in³",125000000m / 2048383 }, {"in3",125000000m / 2048383 }, {"cubic in",125000000m / 2048383 },{"cubic inch",125000000m / 2048383 },{"立方英寸",125000000m / 2048383 },

			{"imperial pint",800000m / 454609 }, {"imperial pt",800000m / 454609 }, {"imperial p",800000m / 454609 },
			{"imperial gallon",100000m / 454609 }, {"imperial gal",100000m / 454609 },
			{"imperial quart",400000m / 454609 }, {"imperial qt",400000m / 454609 },
			{"US pint",1000000000m / 473176473 }, {"US pt",1000000000m / 473176473 },{"US p",1000000000m / 473176473 },
			{"US gallon",125000000m / 473176473 }, {"US gal",125000000m / 473176473 },
			{"US quart",1000000000m / 946352946 }, {"US qt",1000000000m / 946352946 },
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