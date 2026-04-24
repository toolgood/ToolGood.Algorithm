using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_Number2 : Function_0
	{
		private readonly decimal d;
		private readonly string unit;

		public Function_Number2(decimal func1, string func2)
		{
			d = func1;
			unit = func2;
		}

		public override string Name => "Num";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var dict = GetUnitTypedict();
			var d2 = TransformationUnit(d, dict[unit], engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit);
			return Operand.Create(d2);
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(d);
			stringBuilder.Append(unit);
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		private static readonly Dictionary<string, NumberUnitType> unitTypedict = new Dictionary<string, NumberUnitType>(StringComparer.OrdinalIgnoreCase) {
			["KM"] = NumberUnitType.KM,
			["M"] = NumberUnitType.M,
			["DM"] = NumberUnitType.DM,
			["CM"] = NumberUnitType.CM,
			["MM"] = NumberUnitType.MM,

			["KM2"] = NumberUnitType.KM2,
			["M2"] = NumberUnitType.M2,
			["DM2"] = NumberUnitType.DM2,
			["CM2"] = NumberUnitType.CM2,
			["MM2"] = NumberUnitType.MM2,

			["M3"] = NumberUnitType.M3,
			["DM3"] = NumberUnitType.DM3,
			["CM3"] = NumberUnitType.CM3,
			["MM3"] = NumberUnitType.MM3,
			["KM3"] = NumberUnitType.KM3,

			["L"] = NumberUnitType.DM3,
			["ML"] = NumberUnitType.CM3,

			["T"] = NumberUnitType.T,
			["KG"] = NumberUnitType.KG,
			["G"] = NumberUnitType.G
		};

		/// <summary>
		/// ��ȡ ��λ�ֵ�
		/// </summary>
		/// <returns></returns>
		internal static Dictionary<string, NumberUnitType> GetUnitTypedict()
		{
			return unitTypedict;
		}

		internal static decimal TransformationUnit(decimal src, NumberUnitType type1, DistanceUnitType distance, AreaUnitType area, VolumeUnitType volume, MassUnitType mass)
		{
			if(type1 == NumberUnitType.MM) {
				return distance switch {
					DistanceUnitType.MM => src,
					DistanceUnitType.CM => src * 0.1m,
					DistanceUnitType.DM => src * 0.01m,
					DistanceUnitType.M => src * 0.001m,
					DistanceUnitType.KM => src * 0.000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.CM) {
				return distance switch {
					DistanceUnitType.MM => src * 10,
					DistanceUnitType.CM => src,
					DistanceUnitType.DM => src * 0.1m,
					DistanceUnitType.M => src * 0.01m,
					DistanceUnitType.KM => src * 0.00001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.DM) {
				return distance switch {
					DistanceUnitType.MM => src * 100,
					DistanceUnitType.CM => src * 10,
					DistanceUnitType.DM => src,
					DistanceUnitType.M => src * 0.1m,
					DistanceUnitType.KM => src * 0.0001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.M) {
				return distance switch {
					DistanceUnitType.MM => src * 1000,
					DistanceUnitType.CM => src * 100,
					DistanceUnitType.DM => src * 10,
					DistanceUnitType.M => src,
					DistanceUnitType.KM => src * 0.001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.KM) {
				return distance switch {
					DistanceUnitType.MM => src * 1000000,
					DistanceUnitType.CM => src * 100000,
					DistanceUnitType.DM => src * 10000,
					DistanceUnitType.M => src * 1000,
					DistanceUnitType.KM => src,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.MM2) {
				return area switch {
					AreaUnitType.MM2 => src,
					AreaUnitType.CM2 => src * 0.01m,
					AreaUnitType.DM2 => src * 0.0001m,
					AreaUnitType.M2 => src * 0.000001m,
					AreaUnitType.KM2 => src * 0.000000000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.CM2) {
				return area switch {
					AreaUnitType.MM2 => src * 100,
					AreaUnitType.CM2 => src,
					AreaUnitType.DM2 => src * 0.01m,
					AreaUnitType.M2 => src * 0.0001m,
					AreaUnitType.KM2 => src * 0.0000000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.DM2) {
				return area switch {
					AreaUnitType.MM2 => src * 10000,
					AreaUnitType.CM2 => src * 100,
					AreaUnitType.DM2 => src,
					AreaUnitType.M2 => src * 0.01m,
					AreaUnitType.KM2 => src * 0.00000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.M2) {
				return area switch {
					AreaUnitType.MM2 => src * 1000000,
					AreaUnitType.CM2 => src * 10000,
					AreaUnitType.DM2 => src * 100,
					AreaUnitType.M2 => src,
					AreaUnitType.KM2 => src * 0.000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.KM2) {
				return area switch {
					AreaUnitType.MM2 => src * 1000000000000,
					AreaUnitType.CM2 => src * 10000000000,
					AreaUnitType.DM2 => src * 100000000,
					AreaUnitType.M2 => src * 1000000,
					AreaUnitType.KM2 => src,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.MM3) {
				return volume switch {
					VolumeUnitType.MM3 => src,
					VolumeUnitType.CM3 => src * 0.001m,
					VolumeUnitType.DM3 => src * 0.000001m,
					VolumeUnitType.M3 => src * 0.000000001m,
					VolumeUnitType.KM3 => src * 0.000000000000000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.CM3) {
				return volume switch {
					VolumeUnitType.MM3 => src * 1000,
					VolumeUnitType.CM3 => src,
					VolumeUnitType.DM3 => src * 0.001m,
					VolumeUnitType.M3 => src * 0.000001m,
					VolumeUnitType.KM3 => src * 0.000000000000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.DM3) {
				return volume switch {
					VolumeUnitType.MM3 => src * 1000000,
					VolumeUnitType.CM3 => src * 1000,
					VolumeUnitType.DM3 => src,
					VolumeUnitType.M3 => src * 0.001m,
					VolumeUnitType.KM3 => src * 0.000000000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.M3) {
				return volume switch {
					VolumeUnitType.MM3 => src * 1000000000,
					VolumeUnitType.CM3 => src * 1000000,
					VolumeUnitType.DM3 => src * 1000,
					VolumeUnitType.M3 => src,
					VolumeUnitType.KM3 => src * 0.000000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.KM3) {
				return volume switch {
					VolumeUnitType.MM3 => src * 1000000000000000000,
					VolumeUnitType.CM3 => src * 1000000000000000,
					VolumeUnitType.DM3 => src * 1000000000000,
					VolumeUnitType.M3 => src * 1000000000,
					VolumeUnitType.KM3 => src,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.G) {
				return mass switch {
					MassUnitType.G => src,
					MassUnitType.KG => src * 0.001m,
					MassUnitType.T => src * 0.000001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.KG) {
				return mass switch {
					MassUnitType.G => src * 1000,
					MassUnitType.KG => src,
					MassUnitType.T => src * 0.001m,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			} else if(type1 == NumberUnitType.T) {
				return mass switch {
					MassUnitType.G => src * 1000000,
					MassUnitType.KG => src * 1000,
					MassUnitType.T => src,
					_ => throw new ArgumentException("Number unit is error!"),
				};
			}
			throw new ArgumentException("Number unit is error!");
		}

	}

}
