package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_VAR extends Function_N {
    public Function_VAR(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() { return "Var"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        BigDecimal mean = BigDecimal.ZERO;
        BigDecimal m2 = BigDecimal.ZERO;
        int count = 0;

        for (int i = 0; i < args.size(); i++) {
            Operand item = args.get(i);
            if (item.IsArray()) {
                List<Operand> array = item.ArrayValue();
                for (int j = 0; j < array.size(); j++) {
                    Operand elem = array.get(j);
                    if (elem.IsNumber()) {
                        count++;
                        BigDecimal delta = elem.NumberValue().subtract(mean);
                        mean = mean.add(delta.divide(BigDecimal.valueOf(count), RoundingMode.HALF_UP));
                        BigDecimal delta2 = elem.NumberValue().subtract(mean);
                        m2 = m2.add(delta.multiply(delta2));
                    } else if (elem.IsArray() || elem.IsJson()) {
                        List<BigDecimal> list = new ArrayList<>();
                        if (!FunctionUtil.FlattenToNumberList(elem, list)) { return FunctionError(); }
                        for (int k = 0; k < list.size(); k++) {
                            count++;
                            BigDecimal delta = list.get(k).subtract(mean);
                            mean = mean.add(delta.divide(BigDecimal.valueOf(count), RoundingMode.HALF_UP));
                            BigDecimal delta2 = list.get(k).subtract(mean);
                            m2 = m2.add(delta.multiply(delta2));
                        }
                    } else {
                        Operand converted = elem.ToNumber(null);
                        if (converted.IsError()) { return FunctionError(); }
                        count++;
                        BigDecimal delta = converted.NumberValue().subtract(mean);
                        mean = mean.add(delta.divide(BigDecimal.valueOf(count), RoundingMode.HALF_UP));
                        BigDecimal delta2 = converted.NumberValue().subtract(mean);
                        m2 = m2.add(delta.multiply(delta2));
                    }
                }
            } else if (item.IsNumber()) {
                count++;
                BigDecimal delta = item.NumberValue().subtract(mean);
                mean = mean.add(delta.divide(BigDecimal.valueOf(count), RoundingMode.HALF_UP));
                BigDecimal delta2 = item.NumberValue().subtract(mean);
                m2 = m2.add(delta.multiply(delta2));
            } else if (item.IsJson()) {
                List<BigDecimal> list = new ArrayList<>();
                if (!FunctionUtil.FlattenToNumberList(item, list)) { return FunctionError(); }
                for (int k = 0; k < list.size(); k++) {
                    count++;
                    BigDecimal delta = list.get(k).subtract(mean);
                    mean = mean.add(delta.divide(BigDecimal.valueOf(count), RoundingMode.HALF_UP));
                    BigDecimal delta2 = list.get(k).subtract(mean);
                    m2 = m2.add(delta.multiply(delta2));
                }
            } else {
                Operand converted = item.ToNumber(null);
                if (converted.IsError()) { return FunctionError(); }
                count++;
                BigDecimal delta = converted.NumberValue().subtract(mean);
                mean = mean.add(delta.divide(BigDecimal.valueOf(count), RoundingMode.HALF_UP));
                BigDecimal delta2 = converted.NumberValue().subtract(mean);
                m2 = m2.add(delta.multiply(delta2));
            }
        }

        if (count <= 1) { return FunctionError(); }
        return Operand.Create(m2.divide(BigDecimal.valueOf(count - 1), RoundingMode.HALF_UP));
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
