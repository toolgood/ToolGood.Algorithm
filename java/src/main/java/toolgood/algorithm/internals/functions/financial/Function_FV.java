package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

/**
 * FV: 基于固定利率及等额分期付款方式，返回某项投资的未来�?
 * FV(rate, nper, pmt, [pv], [type])
 */
public class Function_FV extends Function_5 {
    public Function_FV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsError()) return rateArg;
        double rate = rateArg.DoubleValue();

        Operand nperArg = GetNumber_2(engine, tempParameter);
        if (nperArg.IsError()) return nperArg;
        double nper = nperArg.DoubleValue();

        Operand pmtArg = GetNumber_3(engine, tempParameter);
        if (pmtArg.IsError()) return pmtArg;
        double pmt = pmtArg.DoubleValue();

        double pv = 0;
        if (func4 != null) {
            Operand pvArg = GetNumber_4(engine, tempParameter);
            if (pvArg.IsError()) return pvArg;
            pv = pvArg.DoubleValue();
        }

        int type = 0;
        if (func5 != null) {
            Operand typeArg = GetNumber_5(engine, tempParameter);
            if (typeArg.IsError()) return typeArg;
            type = typeArg.IntValue();
            if (type != 0 && type != 1) return ParameterError(5);
        }

        if (rate == 0) {
            return Operand.Create(-pmt * nper - pv);
        }

        double factor = Math.pow(1 + rate, nper);
        double fv;
        if (type == 1) {
            fv = -pv * factor - pmt * (1 + rate) * (factor - 1) / rate;
        } else {
            fv = -pv * factor - pmt * (factor - 1) / rate;
        }

        return Operand.Create(fv);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "FV");
    }
}
