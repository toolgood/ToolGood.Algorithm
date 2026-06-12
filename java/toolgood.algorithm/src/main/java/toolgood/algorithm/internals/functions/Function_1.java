package toolgood.algorithm.internals.functions;

import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;

abstract class Function_1 extends FunctionBase {
    protected FunctionBase func1;

    protected Function_1(FunctionBase func1) {
        this.func1 = func1;
    }

    protected Function_1(FunctionBase[] funcs) {
        this.func1 = funcs[0];
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, Name());
    }

    protected void AddFunction(StringBuilder stringBuilder, String functionName) {
        stringBuilder.append(functionName);
        stringBuilder.append('(');
        func1.ToString(stringBuilder, false);
        stringBuilder.append(')');
    }

    protected Operand GetText_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsText())
            return args1;
        return ConvertToText(args1, 1);
    }

    protected Operand GetNumber_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsNumber())
            return args1;
        return ConvertToNumber(args1, 1);
    }

    protected Operand GetDate_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsDate())
            return args1;
        return ConvertToDate(args1, 1);
    }

    protected Operand GetBoolean_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsBoolean())
            return args1;
        return ConvertToBoolean(args1, 1);
    }

    protected Operand GetArray_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsArray())
            return args1;
        return ConvertToArray(args1, 1);
    }

}
