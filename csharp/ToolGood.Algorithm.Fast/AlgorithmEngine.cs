using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm
{
    public class AlgorithmEngine
    {
        public int excelIndex;
        public bool useLocalTime;
        public DistanceUnitType DistanceUnit = DistanceUnitType.M;
        public AreaUnitType AreaUnit = AreaUnitType.M2;
        public VolumeUnitType VolumeUnit = VolumeUnitType.M3;
        public MassUnitType MassUnit = MassUnitType.KG;

        public virtual Operand GetParameter(string parameter)
        {
            return Operand.Error($"Parameter [{parameter}] is missing.");
        }
        public virtual Operand DiyFunction(string parameter, List<Operand> args)
        {
            return Operand.Error($"DiyFunction [{parameter}] is missing.");
        }




    }
}
