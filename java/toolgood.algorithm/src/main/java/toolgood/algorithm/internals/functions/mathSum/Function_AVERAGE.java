package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.math.MathContext;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_AVERAGE extends Function_N {
    public Function_AVERAGE(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() { return "Average"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        BigDecimal sum = BigDecimal.ZERO;
        int count = 0;

        for (int i = 0; i < args.size(); i++) {
            Operand item = args.get(i);
            if (item.IsArray()) {
                List<Operand> array = item.ArrayValue();
                for (int j = 0; j < array.size(); j++) {
                    Operand elem = array.get(j);
                    if (elem.IsNumber()) {
                        sum = sum.add(elem.NumberValue());
                        count++;
                    } else if (elem.IsArray() || elem.IsJson()) {
                        List<BigDecimal> list = new ArrayList<>();
                        if (FunctionUtil.FlattenToNumberList(elem, list)) {
                            for (int k = 0; k < list.size(); k++) {
                                sum = sum.add(list.get(k));
                            }
                            count += list.size();
                        } else {
                            return FunctionError();
                        }
                    } else {
                        Operand converted = elem.ToNumber(null);
                        if (converted.IsError()) { return FunctionError(); }
                        sum = sum.add(converted.NumberValue());
                        count++;
                    }
                }
            } else if (item.IsNumber()) {
                sum = sum.add(item.NumberValue());
                count++;
            } else if (item.IsJson()) {
                List<BigDecimal> list = new ArrayList<>();
                if (!FunctionUtil.FlattenToNumberList(item, list)) { return FunctionError(); }
                for (int k = 0; k < list.size(); k++) {
                    sum = sum.add(list.get(k));
                }
                count += list.size();
            } else {
                Operand converted = item.ToNumber(null);
                if (converted.IsError()) { return FunctionError(); }
                sum = sum.add(converted.NumberValue());
                count++;
            }
        }

        if (count == 0) { return Div0Error(); }
        return Operand.Create(sum.divide(BigDecimal.valueOf(count), MathContext.DECIMAL128));
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
