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
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_PRODUCT extends Function_N {
    public Function_PRODUCT(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Product";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
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

        BigDecimal d = BigDecimal.ONE;
        for (int i = 0; i < list.size(); i++) {
            BigDecimal a = list.get(i);
            d = d.multiply(a);
        }
        return Operand.Create(d);
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
