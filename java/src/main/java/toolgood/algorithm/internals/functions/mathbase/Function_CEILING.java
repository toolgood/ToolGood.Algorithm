package toolgood.algorithm.internals.functions.mathbase;

import java.lang.StringBuilder;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_CEILING extends Function_2 {
    public Function_CEILING(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber()) {
            args1 = args1.toNumber("Function '{0}' parameter {1} is error!", "Ceiling", 1);
            if (args1.isError()) {
                return args1;
            }
        }

        if (func2 == null) {
            return Operand.create(Math.ceil(args1.getNumberValue()));
        }

        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "Ceiling", 2);
            if (args2.isError()) {
                return args2;
            }
        }
        double b = args2.getNumberValue();
        if (b == 0) {
            return Operand.create(0);
        }
        if (b < 0) {
            return Operand.error("Function '{0}' parameter {1} is error!", "Ceiling", 2);
        }

        double a = args1.getNumberValue();
        double d = Math.ceil(a / b) * b;
        return Operand.create(d);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Ceiling");
    }
}
