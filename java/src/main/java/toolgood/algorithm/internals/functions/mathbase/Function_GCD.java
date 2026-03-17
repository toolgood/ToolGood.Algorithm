package toolgood.algorithm.internals.functions.mathbase;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.functions.FunctionUtil;

public final class Function_GCD extends Function_N {
    public Function_GCD(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Gcd";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        List<Operand> args = new ArrayList<>(funcs.length);
        for (int i = 0; i < funcs.length; i++) {
            Operand aa = GetNumber(engine, tempParameter, i);
            if (aa.IsErrorOrNone()) {
                return aa;
            }
            args.add(aa);
        }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList_BigDecimal(args, list);
        if (o == false) {
            return FunctionError();
        }

        for (int i = 0; i < list.size(); i++) {
            if (list.get(i).compareTo(BigDecimal.ZERO) < 0) {
                return ParameterError(i + 1);
            }
        }

        return Operand.Create(FunctionUtil.GetGcd(list));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
