using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class VolumeConverter 
    {
		private static Dictionary<string, decimal> units2 = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase) {
			{"l",1 },{"lt",1 },{"ltr",1 },{"liter",1 },{"litre",1 },
			{"dm³",1 }, {"dm3",1 }, {"cubic decimetre",1 },{"cubic decimeter",1 },{"升",1 },{"立方分米",1 },
			{"m³",0.001m }, {"m3",0.001m }, {"cubic metre",0.001m },{"cubic meter",0.001m },{"立方米",0.001m },
			{"km³",0.001m* 0.001m* 0.001m* 0.001m }, {"km3",0.001m* 0.001m* 0.001m* 0.001m }, {"cubic kilometre",0.001m* 0.001m* 0.001m* 0.001m },{"cubic kilometer",0.001m* 0.001m* 0.001m* 0.001m },{"立方千米",0.001m* 0.001m* 0.001m* 0.001m },
			{"cm³",1000 }, {"cm3",1000 }, {"cubic centimetre",1000 },{"cubic centimeter",1000 },{"立方厘米",1000 },{"毫升",1000 },{"ml",1000 },
			{"mm³",1000000 }, {"mm3",1000000 }, {"cubic millimetre",1000000 },{"cubic millimeter",1000000 },{"立方毫米",1000000 },
			{"ft³",0.0353147m }, {"ft3",0.0353147m }, {"cubic foot",0.0353147m },{"cubic feet",0.0353147m },{"立方英尺",0.0353147m },{"cu ft",0.0353147m },
			{"in³",61.0237m }, {"in3",61.0237m }, {"cubic in",61.0237m },{"cubic inch",61.0237m },{"立方英寸",61.0237m },

			{"imperial pint",1.75975m }, {"imperial pt",1.75975m }, {"imperial p",1.75975m },
			{"imperial gallon",0.219969m }, {"imperial gal",0.219969m },
			{"imperial quart",0.879877m }, {"imperial qt",0.879877m },
			{"US pint",2.11337643513819m }, {"US pt",2.11337643513819m },{"US p",2.11337643513819m },
			{"US gallon",0.264172m }, {"US gal",0.264172m },
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