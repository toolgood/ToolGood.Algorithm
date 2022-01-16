package toolgood.algorithm.Test2;

import toolgood.algorithm.AlgorithmEngineEx;
import toolgood.algorithm.ConditionCache;
import toolgood.algorithm.Operand;

public class PriceAlgorithm extends AlgorithmEngineEx {
    private Desk _disk;

    public PriceAlgorithm(ConditionCache multiConditionCache, Desk desk) {
        super(multiConditionCache);
        _disk = desk;
    }

    @Override
    protected Operand GetParameter(String parameter) {
        if (parameter.equals("长")) {
            return Operand.Create(_disk.Length);
        }
        if (parameter.equals("宽")) {
            return Operand.Create(_disk.Width);
        }
        if (parameter.equals("高")) {
            return Operand.Create(_disk.Heigth);
        }
        if (parameter.equals("半径")) {
            return Operand.Create(_disk.Radius);
        }
        if (parameter.equals("方桌")) {
            return Operand.Create(_disk.IsRoundTable == false);
        }
        if (parameter.equals("圆桌")) {
            return Operand.Create(_disk.IsRoundTable);
        }
        return super.GetParameter(parameter);
    }
}
