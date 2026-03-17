package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_GEOMEAN extends Function_N {
    public Function_GEOMEAN(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "GeoMean";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList_BigDecimal(args, list);
        if (o == false) {
            return ParameterError(1);
        }
        if (list.size() == 0) {
            return ParameterError(1);
        }
        BigDecimal product = BigDecimal.ONE;
        for (int i = 0; i < list.size(); i++) {
            if (list.get(i).compareTo(BigDecimal.ZERO) <= 0) {
                return ParameterError(1);
            }
            product = product.multiply(list.get(i));
        }
        BigDecimal geoMean = MathEx.Pow(product, BigDecimal.ONE.divide(new BigDecimal(list.size()), java.math.MathContext.DECIMAL128));
        return Operand.Create(geoMean);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        if (funcs.length == 1) {
            funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        } else {
            for (int i = 0; i < funcs.length; i++) {
                funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
            }
        }
    }
}
