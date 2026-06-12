package toolgood.algorithm.internals.functions.mathSum2;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

final class Function_FISHERINV extends Function_1 {
    public Function_FISHERINV(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
        }
    }

    @Override
    public String Name() { return "FisherInv"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        double x = args1.NumberValue().doubleValue();
        double exp2x = Math.exp(2 * x);
        double n = (exp2x - 1) / (exp2x + 1);
        return Operand.Create(BigDecimal.valueOf(n));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
