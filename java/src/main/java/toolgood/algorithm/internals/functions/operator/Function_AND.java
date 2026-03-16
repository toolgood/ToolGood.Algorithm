package toolgood.algorithm.internals.functions.operator;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_AND extends Function_2 {
    public Function_AND(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        // 程序 && and || or �?excel�? AND(x,y) OR(x,y) 有区�?
        // 在excel�?AND(x,y) OR(x,y) 先报错，
        // 在程序中�?& and  有true 直接返回true 就不会检测下一个会不会报错
        // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotBoolean()) {
            args1 = args1.ToBoolean();
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args1.BooleanValue() == false) {
            Operand args2 = func2.Evaluate(work, tempParameter).ToBoolean();
            if (args2.IsError()) {
                return args2;
            }
            return Operand.False;
        }
        return func2.Evaluate(work, tempParameter).ToBoolean();
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) {
            stringBuilder.append('(');
        }
        func1.toString(stringBuilder, false);
        stringBuilder.append(" && ");
        func2.toString(stringBuilder, false);
        if (addBrackets) {
            stringBuilder.append(')');
        }
    }
}
