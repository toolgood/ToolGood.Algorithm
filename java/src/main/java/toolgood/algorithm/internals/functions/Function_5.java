package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

/**
 * 5参函数抽象基类，继承 Function_4
 */
public abstract class Function_5 extends Function_4 {
    protected FunctionBase func5;

    protected Function_5(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length >= 5) {
            this.func5 = funcs[4];
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
                    }
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

    // region Get_5 helpers

    protected Operand GetText_5(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args5 = func5.Evaluate(engine, tempParameter);
        if (args5.IsText()) return args5;
        return ConvertToText(args5, 5);
    }

    protected Operand GetNumber_5(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args5 = func5.Evaluate(engine, tempParameter);
        if (args5.IsNumber()) return args5;
        return ConvertToNumber(args5, 5);
    }

    protected Operand GetDate_5(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args5 = func5.Evaluate(engine, tempParameter);
        if (args5.IsDate()) return args5;
        return ConvertToDate(args5, 5);
    }

    protected Operand GetBoolean_5(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args5 = func5.Evaluate(engine, tempParameter);
        if (args5.IsBoolean()) return args5;
        return ConvertToBoolean(args5, 5);
    }

    protected Operand GetArray_5(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args5 = func5.Evaluate(engine, tempParameter);
        if (args5.IsArray()) return args5;
        return ConvertToArray(args5, 5);
    }

    // endregion
}
