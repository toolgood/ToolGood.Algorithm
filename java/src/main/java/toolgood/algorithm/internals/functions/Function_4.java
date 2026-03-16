package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

public abstract class Function_4 extends FunctionBase {
    protected FunctionBase func1;
    protected FunctionBase func2;
    protected FunctionBase func3;
    protected FunctionBase func4;

    protected Function_4(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        this.func1 = func1;
        this.func2 = func2;
        this.func3 = func3;
        this.func4 = func4;
    }

    /** FunctionBase[] 数组构造器，供 Function_5/6 调用 */
    protected Function_4(FunctionBase[] funcs) {
        if (funcs.length >= 1) this.func1 = funcs[0];
        if (funcs.length >= 2) this.func2 = funcs[1];
        if (funcs.length >= 3) this.func3 = funcs[2];
        if (funcs.length >= 4) this.func4 = funcs[3];
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
                if (func4 != null) {
                    stringBuilder.append(", ");
                    func4.toString(stringBuilder, false);
                }
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

    // region Get_4 helpers

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

    // endregion
}
