package toolgood.algorithm.Tests;

import java.util.List;
import toolgood.algorithm.AlgorithmEngineEx;
import toolgood.algorithm.Operand;

public class Cylinder extends AlgorithmEngineEx {
    private double _radius;
    private double _height;

    public Cylinder(double radius, double height) {
        _radius = radius;
        _height = height;
    }
    @Override
    public Operand GetParameterEx(String parameter) {
        if(parameter.equals("半径")) {
            return Operand.Create(_radius);
        }
        if(parameter.equals("直径")) {
            return Operand.Create(_radius * 2);
        }
        if(parameter.equals("高")) {
            return Operand.Create(_height);
        }
        return super.GetParameter(parameter);
    }

    @Override
    public Operand ExecuteDiyFunction(String funcName, List<Operand> operands) {
        if (funcName.equals("求面积")) {
            if (operands.size() == 1) {
                double r = operands.get(0).ToNumber("").DoubleValue();
                return Operand.Create(r * r * Math.PI);
            }
        }
        return super.ExecuteDiyFunction(funcName, operands);
    }
}