package toolgood.algorithm.internals.functions.compare;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_NE extends Function_2 {
    public Function_NE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "!=";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsError() || args1.IsNone()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsError() || args2.IsNone()) {
            return args2;
        }

        if (args1.Type() == args2.Type()) {
            if (args1.IsNumber()) {
                return Operand.Create(args1.NumberValue().compareTo(args2.NumberValue()) != 0);
            } else if (args1.IsText()) {
                return Operand.Create(!args1.TextValue().equals(args2.TextValue()));
            } else if (args1.IsBoolean()) {
                return Operand.Create(args1.BooleanValue() != args2.BooleanValue());
            } else if (args1.IsDate()) {
                return Operand.Create(args1.DateValue().ToLong() != args2.DateValue().ToLong());
            } else if (args1.IsNull()) {
                return Operand.False;
            }
            return CompareError();
        } else if (args1.IsNull() || args2.IsNull()) {
            return Operand.True;
        } else if (args1.IsDate() || args2.IsDate() || args1.IsJson() || args2.IsJson()
                || args1.IsArray() || args2.IsArray() || args1.IsArrayJson() || args2.IsArrayJson()) {
            return CompareError();
        }
        args1 = ConvertToNumber(args1, 1);
        if (args1.IsError() || args1.IsNone()) {
            return args1;
        }
        args2 = ConvertToNumber(args2, 2);
        if (args2.IsError() || args2.IsNone()) {
            return args2;
        }

        return Operand.Create(args1.NumberValue().compareTo(args2.NumberValue()) != 0);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        stringBuilder.append(" != ");
        func2.toString(stringBuilder, false);
        if (addBrackets) stringBuilder.append(')');
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
        OperandType t1 = func1.GetResultType();
        OperandType t2 = func2.GetResultType();
        if (t1 == OperandType.NONE) {
            try {
                Operand p = noneEngine.Evaluate(func2).ToText(null);
                if (t2 != OperandType.ERROR && !(p.IsError() || p.IsNone())) {
                    func1.GetParameterTypes(noneEngine, result, t2, Name(), p.TextValue());
                    func2.GetParameterTypes(noneEngine, result, t2, null, null);
                    return;
                }
            } catch (Exception e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        } else if (t2 == OperandType.NONE) {
            try {
                Operand p = noneEngine.Evaluate(func1).ToText(null);
                if (t1 != OperandType.ERROR && !(p.IsError() || p.IsNone())) {
                    func2.GetParameterTypes(noneEngine, result, t1, Name(), p.TextValue());
                    func1.GetParameterTypes(noneEngine, result, t1, null, null);
                    return;
                }
            } catch (Exception e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        }
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.NONE, null, null);
    }
}
