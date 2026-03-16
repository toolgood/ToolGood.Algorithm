package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

public abstract class Function_3 extends FunctionBase {
    protected FunctionBase func1;
    protected FunctionBase func2;
    protected FunctionBase func3;

    protected Function_3(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        this.func1 = func1;
        this.func2 = func2;
        this.func3 = func3;
    }

    protected void AddFunction(StringBuilder stringBuilder, String functionName) {
        stringBuilder.append(functionName);
        stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        if (func2 != null) {
            stringBuilder.append(", ");
            func2.toString(stringBuilder, false);
            if (func3 != null) {
                stringBuilder.append(", ");
                func3.toString(stringBuilder, false);
            }
        }
        stringBuilder.append(')');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
    }

    // region Get_1/2/3 helpers

    protected Operand GetText_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsText()) return args1;
        return ConvertToText(args1, 1);
    }

    protected Operand GetNumber_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsNumber()) return args1;
        return ConvertToNumber(args1, 1);
    }

    protected Operand GetDate_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsDate()) return args1;
        return ConvertToDate(args1, 1);
    }

    protected Operand GetBoolean_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsBoolean()) return args1;
        return ConvertToBoolean(args1, 1);
    }

    protected Operand GetArray_1(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsArray()) return args1;
        return ConvertToArray(args1, 1);
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

    protected Operand GetText_3(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args3 = func3.Evaluate(engine, tempParameter);
        if (args3.IsText()) return args3;
        return ConvertToText(args3, 3);
    }

    protected Operand GetNumber_3(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args3 = func3.Evaluate(engine, tempParameter);
        if (args3.IsNumber()) return args3;
        return ConvertToNumber(args3, 3);
    }

    protected Operand GetDate_3(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args3 = func3.Evaluate(engine, tempParameter);
        if (args3.IsDate()) return args3;
        return ConvertToDate(args3, 3);
    }

    protected Operand GetBoolean_3(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args3 = func3.Evaluate(engine, tempParameter);
        if (args3.IsBoolean()) return args3;
        return ConvertToBoolean(args3, 3);
    }

    protected Operand GetArray_3(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args3 = func3.Evaluate(engine, tempParameter);
        if (args3.IsArray()) return args3;
        return ConvertToArray(args3, 3);
    }

    // endregion
}
