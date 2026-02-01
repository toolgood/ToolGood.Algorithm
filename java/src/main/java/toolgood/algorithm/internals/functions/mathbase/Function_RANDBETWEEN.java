package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

import java.util.Random;

public class Function_RANDBETWEEN extends Function_2 {
    public Function_RANDBETWEEN(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RandBetween", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RandBetween", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Random rand = new Random();
        return Operand.Create(rand.nextDouble() * (args2.DoubleValue() - args1.DoubleValue()) + args1.DoubleValue());
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "RandBetween");
    }
}
