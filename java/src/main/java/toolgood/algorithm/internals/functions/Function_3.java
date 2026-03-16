package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

public abstract class Function_3 extends Function_2 {
    protected FunctionBase func3;

    protected Function_3(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length >= 3) {
            this.func3 = funcs[2];
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
            if (func3 != null) {
                stringBuilder.append(", ");
                func3.toString(stringBuilder, false);
            }
        }
        stringBuilder.append(')');
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
}
