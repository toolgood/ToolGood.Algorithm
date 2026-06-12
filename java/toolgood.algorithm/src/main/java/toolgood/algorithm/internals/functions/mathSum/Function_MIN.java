package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

final class Function_MIN extends Function_N {
    public Function_MIN(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() { return "Min"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        boolean found = false;
        BigDecimal min = null;

        for (int i = 0; i < args.size(); i++) {
            Operand item = args.get(i);
            if (item.IsArray()) {
                List<Operand> array = item.ArrayValue();
                for (int j = 0; j < array.size(); j++) {
                    Operand elem = array.get(j);
                    if (elem.IsNumber()) {
                        if (!found || elem.NumberValue().compareTo(min) < 0) {
                            min = elem.NumberValue();
                            found = true;
                        }
                    } else if (elem.IsArray() || elem.IsJson()) {
                        List<BigDecimal> list = new ArrayList<>();
                        if (FunctionUtil.FlattenToList(elem, list)) {
                            for (int k = 0; k < list.size(); k++) {
                                if (!found || list.get(k).compareTo(min) < 0) {
                                    min = list.get(k);
                                    found = true;
                                }
                            }
                        } else {
                            return FunctionError();
                        }
                    } else {
                        Operand converted = elem.ToNumber(null);
                        if (converted.IsError()) { return FunctionError(); }
                        if (!found || converted.NumberValue().compareTo(min) < 0) {
                            min = converted.NumberValue();
                            found = true;
                        }
                    }
                }
            } else if (item.IsNumber()) {
                if (!found || item.NumberValue().compareTo(min) < 0) {
                    min = item.NumberValue();
                    found = true;
                }
            } else if (item.IsJson()) {
                List<BigDecimal> list = new ArrayList<>();
                if (!FunctionUtil.FlattenToList(item, list)) { return FunctionError(); }
                for (int k = 0; k < list.size(); k++) {
                    if (!found || list.get(k).compareTo(min) < 0) {
                        min = list.get(k);
                        found = true;
                    }
                }
            } else {
                Operand converted = item.ToNumber(null);
                if (converted.IsError()) { return FunctionError(); }
                if (!found || converted.NumberValue().compareTo(min) < 0) {
                    min = converted.NumberValue();
                    found = true;
                }
            }
        }

        if (!found) { return FunctionError(); }
        return Operand.Create(min);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        if (funcs.length == 1) {
            funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        } else if (funcs.length == 2) {
            OperandType t1 = funcs[0].GetResultType();
            OperandType t2 = funcs[1].GetResultType();
            if (t1 == OperandType.NONE && t2 == OperandType.NUMBER) {
                Operand p = noneEngine.Evaluate(funcs[1]).ToText(null);
                if (t2 != OperandType.ERROR && !p.IsErrorOrNone()) {
                    funcs[0].GetParameterTypes(noneEngine, result, t2, Name(), p.TextValue());
                    funcs[1].GetParameterTypes(noneEngine, result, t2);
                    return;
                }
            } else if (t1 == OperandType.NUMBER && t2 == OperandType.NONE) {
                Operand p = noneEngine.Evaluate(funcs[0]).ToText(null);
                if (t1 != OperandType.ERROR && !p.IsErrorOrNone()) {
                    funcs[1].GetParameterTypes(noneEngine, result, t1, Name(), p.TextValue());
                    funcs[0].GetParameterTypes(noneEngine, result, t1);
                    return;
                }
            }
            funcs[0].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
            funcs[1].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        } else {
            for (int i = 0; i < funcs.length; i++) {
                funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
            }
        }
    }
}
