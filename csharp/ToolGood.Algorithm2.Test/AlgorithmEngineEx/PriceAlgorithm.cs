using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.Test.AlgorithmEngineEx
{
    public class PriceAlgorithm : ToolGood.Algorithm.AlgorithmEngineEx
    {
        private Desk _disk;
        public PriceAlgorithm(ConditionCache multiConditionCache, Desk desk) : base(multiConditionCache)
        {
            _disk = desk;
        }

        public override Operand GetParameter(string parameter)
        {
            if (parameter == "长") {
                return Operand.Create(_disk.Length);
            }
            if (parameter == "宽") {
                return Operand.Create(_disk.Width);
            }
            if (parameter == "高") {
                return Operand.Create(_disk.Heigth);
            }
            if (parameter == "半径") {
                return Operand.Create(_disk.Radius);
            }
            if (parameter == "方桌") {
                return Operand.Create(_disk.IsRoundTable == false);
            }
            if (parameter == "圆桌") {
                return Operand.Create(_disk.IsRoundTable);
            }
            return base.GetParameter(parameter);
        }

    }

}
