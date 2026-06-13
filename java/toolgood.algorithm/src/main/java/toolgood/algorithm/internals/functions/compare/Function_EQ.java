package toolgood.algorithm.internals.functions.compare;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_EQ extends Function_2 {

    public Function_EQ(FunctionBase[] funcs) {
        super(funcs);
    }

    public Function_EQ(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "==";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        if (args1.Type() == args2.Type()) {
            if (args1.IsNumber()) {
                return Operand.Create(args1.NumberValue().compareTo(args2.NumberValue()) == 0);
            } else if (args1.IsText()) {
                return Operand.Create(args1.TextValue().equals(args2.TextValue()));
            } else if (args1.IsBoolean()) {
                return Operand.Create(args1.BooleanValue() == args2.BooleanValue());
            } else if (args1.IsDate()) {
                return Operand.Create(args1.DateValue().ToLong() == args2.DateValue().ToLong());
            } else if (args1.IsNull()) {
                return Operand.True;
            }
            return CompareError();
        } else if (args1.IsNull() || args2.IsNull()) {
            return Operand.False;
        } else if (args1.IsDate() || args2.IsDate() || args1.IsJson() || args2.IsJson() || args1.IsArray() || args2.IsArray() || args1.IsArrayJson() || args2.IsArrayJson()) {
            return CompareError();
        }
        args1 = ConvertToNumber(args1, 1);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        args2 = ConvertToNumber(args2, 2);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        return Operand.Create(args1.NumberValue().compareTo(args2.NumberValue()) == 0);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets)
            stringBuilder.append('(');
        func1.ToString(stringBuilder, false);
        stringBuilder.append(" == ");
        func2.ToString(stringBuilder, false);
        if (addBrackets)
            stringBuilder.append(')');
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        OperandType t1 = func1.GetResultType();
        OperandType t2 = func2.GetResultType();
        if (t1 == OperandType.NONE) {
            Operand p = func2.Evaluate(noneEngine, null).ToText(null);
            if (t2 != OperandType.ERROR && !p.IsErrorOrNone()) {
                func1.GetParameterTypes(noneEngine, result, t2, Name(), p.TextValue());
                func2.GetParameterTypes(noneEngine, result, t2);
                return;
            }
        } else if (t2 == OperandType.NONE) {
            Operand p = func1.Evaluate(noneEngine, null).ToText(null);
            if (t1 != OperandType.ERROR && !p.IsErrorOrNone()) {
                func2.GetParameterTypes(noneEngine, result, t1, Name(), p.TextValue());
                func1.GetParameterTypes(noneEngine, result, t1);
                return;
            }
        }
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
        func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
    }

}
