package toolgood.algorithm.internals.functions.operator;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_OR extends Function_2 {
    public Function_OR(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        // 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
        // 在excel内 AND(x,y) OR(x,y) 先报错，
        // 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
        // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotBoolean()) {
            args1 = args1.ToBoolean("Function '{0}' parameter {1} is error!", "OR", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args1.BooleanValue()) {
            Operand args2 = func2.Evaluate(work, tempParameter).ToBoolean("Function '{0}' parameter {1} is error!", "OR", 2);
            if (args2.IsError()) {
                return args2;
            }
            return Operand.TRUE;
        }
        return func2.Evaluate(work, tempParameter).ToBoolean("Function '{0}' parameter {1} is error!", "OR", 2);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) {
            stringBuilder.append('(');
        }
        func1.toString(stringBuilder, false);
        stringBuilder.append(" || ");
        func2.toString(stringBuilder, false);
        if (addBrackets) {
            stringBuilder.append(')');
        }
    }
}
