package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

/**
 * PMT: 基于固定利率及等额分期付款方式，返回贷款的每期付款额
 * PMT(rate, nper, pv, [fv], [type])
 */
public class Function_PMT extends Function_5 {
    public Function_PMT(FunctionBase[] funcs) {
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

        Operand pvArg = GetNumber_3(engine, tempParameter);
        if (pvArg.IsError()) return pvArg;
        double pv = pvArg.DoubleValue();

        if (nper == 0) return Div0Error();

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
            return Operand.Create(-(pv + fv) / nper);
        }

        double factor = Math.pow(1 + rate, nper);
        double pmt = -(pv * factor + fv) * rate / (factor - 1);
        if (type == 1) {
            pmt = pmt / (1 + rate);
        }

        return Operand.Create(pmt);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "PMT");
    }
}
