package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;

public abstract class Function_N extends FunctionBase {
    protected FunctionBase[] funcs;

    protected Function_N(FunctionBase[] funcs) {
        this.funcs = funcs;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, Name());
    }

    protected void AddFunction(StringBuilder stringBuilder, String functionName) {
        stringBuilder.append(functionName);
        stringBuilder.append('(');
        for (int i = 0; i < funcs.length; i++) {
            if (i > 0) {
                stringBuilder.append(", ");
            }
            funcs[i].toString(stringBuilder, false);
        }
        stringBuilder.append(')');
    }

    protected Operand TryEvaluateAll(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter, List<Operand> args) throws Exception {
        for (int i = 0; i < funcs.length; i++) {
            Operand aa = funcs[i].Evaluate(engine, tempParameter);
            if (aa.IsError() || aa.IsNone()) {
                return aa;
            }
            args.add(aa);
        }
        return null;
    }

    protected Operand GetText(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter, int idx) throws Exception {
        Operand args1 = funcs[idx].Evaluate(engine, tempParameter);
        if (args1.IsText()) return args1;
        return ConvertToText(args1, idx);
    }

    protected Operand GetNumber(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter, int idx) throws Exception {
        Operand args1 = funcs[idx].Evaluate(engine, tempParameter);
        if (args1.IsNumber()) return args1;
        return ConvertToNumber(args1, idx);
    }

    protected Operand GetDate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter, int idx) throws Exception {
        Operand args1 = funcs[idx].Evaluate(engine, tempParameter);
        if (args1.IsDate()) return args1;
        return ConvertToDate(args1, idx);
    }

    protected Operand GetBoolean(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter, int idx) throws Exception {
        Operand args1 = funcs[idx].Evaluate(engine, tempParameter);
        if (args1.IsBoolean()) return args1;
        return ConvertToBoolean(args1, idx);
    }

    protected Operand GetArray(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter, int idx) throws Exception {
        Operand args1 = funcs[idx].Evaluate(engine, tempParameter);
        if (args1.IsArray()) return args1;
        return ConvertToArray(args1, idx);
    }
}
