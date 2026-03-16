package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

/**
 * NPER: 基于固定利率及等额分期付款方式，返回某项投资（或贷款）的期数
 * NPER(rate, pmt, pv, [fv], [type])
 */
public class Function_NPER extends Function_5 {
    public Function_NPER(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsError()) return rateArg;
        double rate = rateArg.DoubleValue();

        Operand pmtArg = GetNumber_2(engine, tempParameter);
        if (pmtArg.IsError()) return pmtArg;
        double pmt = pmtArg.DoubleValue();

        Operand pvArg = GetNumber_3(engine, tempParameter);
        if (pvArg.IsError()) return pvArg;
        double pv = pvArg.DoubleValue();

        double fv = 0;
        if (func4 != null) {
            Operand fvArg = GetNumber_4(engine, tempParameter);
            if (fvArg.IsError()) return fvArg;
            fv = fvArg.DoubleValue();
        }

        int type = 0;
        if (func5 != null) {
            Operand typeArg = GetNumber_5(engine, tempParameter);
            if (typeArg.IsError()) return typeArg;
            type = typeArg.IntValue();
        }

        if (rate == 0) {
            if (pmt == 0) return Div0Error();
            return Operand.Create(-(pv + fv) / pmt);
        }
        if (rate == -1) {
            return Div0Error();
        }

        double factor = (type == 1) ? pmt * (1 + rate) : pmt;
        double nper = Math.log((-fv * rate + factor) / (pv * rate + factor)) / Math.log(1 + rate);
        return Operand.Create(nper);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "NPER");
    }
}
