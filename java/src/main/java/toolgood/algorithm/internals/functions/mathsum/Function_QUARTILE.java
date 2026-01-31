package toolgood.algorithm.internals.functions.mathsum;

import java.util.List;
import java.util.function.Function;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.OperandType;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.mathnet.ExcelFunctions;

public class Function_QUARTILE extends Function_2 {
    public Function_QUARTILE(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(Object work, Function<Object, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.getOperandType() != OperandType.Array) {
            args1 = args1.toArray("Function '{0}' parameter {1} is error!", "Quartile", 1);
            if (args1.isError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.getOperandType() != OperandType.Number) {
            args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "Quartile", 2);
            if (args2.isError()) {
                return args2;
            }
        }

        List<Double> list = new java.util.ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args1, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 1);
        }

        int quant = args2.getIntValue();
        if (quant < 0 || quant > 4) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 2);
        }
        double[] array = list.stream().mapToDouble(Double::doubleValue).toArray();
        return Operand.Create(ExcelFunctions.Quartile(array, quant));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "Quartile");
    }
}
