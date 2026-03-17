package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;

public abstract class Function_6 extends Function_5 {
    protected FunctionBase func6;

    protected Function_6(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length >= 6) {
            this.func6 = funcs[5];
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
                if (func4 != null) {
                    stringBuilder.append(", ");
                    func4.toString(stringBuilder, false);
                    if (func5 != null) {
                        stringBuilder.append(", ");
                        func5.toString(stringBuilder, false);
                        if (func6 != null) {
                            stringBuilder.append(", ");
                            func6.toString(stringBuilder, false);
                        }
                    }
                }
            }
        }
        stringBuilder.append(')');
    }

    protected Operand GetText_6(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args6 = func6.Evaluate(engine, tempParameter);
        if (args6.IsText()) return args6;
        return ConvertToText(args6, 6);
    }

    protected Operand GetNumber_6(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args6 = func6.Evaluate(engine, tempParameter);
        if (args6.IsNumber()) return args6;
        return ConvertToNumber(args6, 6);
    }

    protected Operand GetDate_6(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args6 = func6.Evaluate(engine, tempParameter);
        if (args6.IsDate()) return args6;
        return ConvertToDate(args6, 6);
    }

    protected Operand GetBoolean_6(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args6 = func6.Evaluate(engine, tempParameter);
        if (args6.IsBoolean()) return args6;
        return ConvertToBoolean(args6, 6);
    }

    protected Operand GetArray_6(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args6 = func6.Evaluate(engine, tempParameter);
        if (args6.IsArray()) return args6;
        return ConvertToArray(args6, 6);
    }
}
