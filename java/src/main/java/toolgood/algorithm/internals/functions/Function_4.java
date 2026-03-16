package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

public abstract class Function_4 extends Function_3 {
    protected FunctionBase func4;

    protected Function_4(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length >= 4) {
            this.func4 = funcs[3];
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
                }
            }
        }
        stringBuilder.append(')');
    }

    protected Operand GetText_4(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args4 = func4.Evaluate(engine, tempParameter);
        if (args4.IsText()) return args4;
        return ConvertToText(args4, 4);
    }

    protected Operand GetNumber_4(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args4 = func4.Evaluate(engine, tempParameter);
        if (args4.IsNumber()) return args4;
        return ConvertToNumber(args4, 4);
    }

    protected Operand GetDate_4(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args4 = func4.Evaluate(engine, tempParameter);
        if (args4.IsDate()) return args4;
        return ConvertToDate(args4, 4);
    }

    protected Operand GetBoolean_4(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args4 = func4.Evaluate(engine, tempParameter);
        if (args4.IsBoolean()) return args4;
        return ConvertToBoolean(args4, 4);
    }

    protected Operand GetArray_4(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args4 = func4.Evaluate(engine, tempParameter);
        if (args4.IsArray()) return args4;
        return ConvertToArray(args4, 4);
    }
}
