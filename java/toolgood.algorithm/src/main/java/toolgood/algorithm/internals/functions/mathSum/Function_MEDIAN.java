package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_MEDIAN extends Function_N {

    public Function_MEDIAN(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "Median";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        List<BigDecimal> list = new ArrayList<>();
        if (FunctionUtil.FlattenToBigDecimalList(args, list) == false) { return FunctionError(); }
        if (list.size() == 0) { return FunctionError(); }

        int n = list.size();
        if (n == 1) return Operand.Create(list.get(0));

        int mid = n / 2;
        if (n % 2 == 0) {
            return Operand.Create(BigDecimal.valueOf(
                    (QuickSelect(list, mid - 1) + QuickSelect(list, mid)) / 2));
        }
        return Operand.Create(BigDecimal.valueOf(QuickSelect(list, mid)));
    }

    private static double QuickSelect(List<BigDecimal> arr, int k) {
        int left = 0, right = arr.size() - 1;
        while (left < right) {
            int pivot = SelectPivot(arr, left, right);
            int partition = Partition(arr, left, right, pivot);
            if (partition == k) {
                return arr.get(k).doubleValue();
            }
            if (k < partition) {
                right = partition - 1;
            } else {
                left = partition + 1;
            }
        }
        return arr.get(left).doubleValue();
    }

    private static int SelectPivot(List<BigDecimal> arr, int left, int right) {
        int mid = left + (right - left) / 2;
        double a = arr.get(left).doubleValue();
        double b = arr.get(mid).doubleValue();
        double c = arr.get(right).doubleValue();
        if (a < b) {
            if (b < c) return mid;
            if (a < c) return right;
            return left;
        }
        if (a < c) return left;
        if (b < c) return right;
        return mid;
    }

    private static int Partition(List<BigDecimal> arr, int left, int right, int pivotIndex) {
        double pivot = arr.get(pivotIndex).doubleValue();
        BigDecimal temp = arr.get(pivotIndex);
        arr.set(pivotIndex, arr.get(right));
        arr.set(right, temp);
        int storeIndex = left;
        for (int i = left; i < right; i++) {
            if (arr.get(i).doubleValue() < pivot) {
                temp = arr.get(storeIndex);
                arr.set(storeIndex, arr.get(i));
                arr.set(i, temp);
                storeIndex++;
            }
        }
        temp = arr.get(storeIndex);
        arr.set(storeIndex, arr.get(right));
        arr.set(right, temp);
        return storeIndex;
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
