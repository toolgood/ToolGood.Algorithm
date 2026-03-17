package toolgood.algorithm.internals.functions.string;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_EXACT extends Function_2 {
    public Function_EXACT(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Exact";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        return Operand.Create(args1.TextValue().equals(args2.TextValue()));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        OperandType t1 = func1.GetResultType();
        OperandType t2 = func2.GetResultType();
        if (t1 == OperandType.NONE) {
            try {
                Operand p = noneEngine.Evaluate(func2).ToText("");
                if (t2 != OperandType.ERROR && p.IsErrorOrNone() == false) {
                    func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, "==", p.TextValue());
                    func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
                    return;
                }
            } catch (Exception e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        } else if (t2 == OperandType.NONE) {
            try {
                Operand p = noneEngine.Evaluate(func1).ToText("");
                if (t1 != OperandType.ERROR && p.IsErrorOrNone() == false) {
                    func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, "==", p.TextValue());
                    func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
                    return;
                }
            } catch (Exception e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        }
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
    }
}
