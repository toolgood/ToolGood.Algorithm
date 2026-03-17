package toolgood.algorithm.internals.functions.csharp;

import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_JOIN extends Function_N {
    public Function_JOIN(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Join";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }
        Operand args1 = args.get(0);
        if (args1.IsJson()) {
            Operand o = args1.ToArray(null);
            if (!o.IsErrorOrNone()) {
                args1 = o;
            }
        }
        if (args1.IsArray()) {
            List<String> list = new ArrayList<>(args1.ArrayValue().size());
            boolean o = FunctionUtil.FlattenToList(args1, list);
            if (!o) {
                return ParameterError(1);
            }

            Operand args2 = ConvertToText(args.get(1), 2);
            if (args2.IsErrorOrNone()) {
                return args2;
            }

            return Operand.Create(String.join(args2.TextValue(), list));
        } else {
            args1 = ConvertToText(args1, 1);
            if (args1.IsErrorOrNone()) {
                return args1;
            }

            List<String> list = new ArrayList<>(args.size());
            for (int i = 1; i < args.size(); i++) {
                boolean o = FunctionUtil.FlattenToList(args.get(i), list);
                if (!o) {
                    return ParameterError(i + 1);
                }
            }
            return Operand.Create(String.join(args1.TextValue(), list));
        }
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (FunctionBase item : funcs) {
            item.GetParameterTypes(noneEngine, result, OperandType.NONE, null, null);
        }
    }
}
