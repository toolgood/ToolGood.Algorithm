package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

public abstract class Function_2 extends Function_1 {
    protected FunctionBase func2;

    public Function_2(FunctionBase func1, FunctionBase func2) {
        super(func1);
        this.func2 = func2;
    }

    protected Function_2(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length >= 2) {
            this.func2 = funcs[1];
        }
    }

    @Override
    protected void AddFunction(StringBuilder stringBuilder, String functionName) {
        stringBuilder.append(functionName);
        stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        if (func2 != null) {
            stringBuilder.append(", ");
            func2.toString(stringBuilder, false);
        }
        stringBuilder.append(')');
    }

    protected Operand GetText_2(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsText()) return args2;
        return ConvertToText(args2, 2);
    }

    protected Operand GetNumber_2(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsNumber()) return args2;
        return ConvertToNumber(args2, 2);
    }

    protected Operand GetDate_2(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsDate()) return args2;
        return ConvertToDate(args2, 2);
    }

    protected Operand GetBoolean_2(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsBoolean()) return args2;
        return ConvertToBoolean(args2, 2);
    }

    protected Operand GetArray_2(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsArray()) return args2;
        return ConvertToArray(args2, 2);
    }
}
