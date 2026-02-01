package toolgood.algorithm.Tests;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.MyParameter;

public class Cylinder extends AlgorithmEngine {
    private int _radius;
    private int _height;

    public Cylinder(int radius, int height) {
        _radius = radius;
        _height = height;
    }

    @Override
    protected Operand GetParameter(MyParameter parameter) {
        if (parameter.Name.equals("半径")) {
            return Operand.Create(_radius);
        }
        if (parameter.Name.equals("直径"))
        {
            return Operand.Create(_radius * 2);
        }
        if (parameter.Name.equals("高"))
        {
            return Operand.Create(_height);
        }
        return super.GetParameter(parameter);
    }

    @Override
    protected Operand ExecuteDiyFunction(String funcName, List<Operand> operands)
    {
        if (funcName.equals("求面积"))
        {
            if (operands.size() == 1)
            {
                int r =(int) operands.get(0).ToNumber
(null).NumberValue().intValue();
                return Operand.Create(r * r * Math.PI);
            }
        }
        return super.ExecuteDiyFunction(funcName, operands);
    }
}