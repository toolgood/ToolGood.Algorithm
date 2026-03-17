package toolgood.algorithm.internals.functions.mathsum2;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;

public final class Function_HYPGEOMDIST extends Function_4 {
    public Function_HYPGEOMDIST(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "HypgeomDist";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) return args3;

        Operand args4 = GetNumber_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) return args4;

        int k = args1.IntValue();
        if (k < 0) {
            return ParameterError(1);
        }
        int draws = args2.IntValue();
        if (draws < 0) {
            return ParameterError(2);
        }
        int success = args3.IntValue();
        if (success < 0) {
            return ParameterError(3);
        }
        int population = args4.IntValue();
        if (population < 0) {
            return ParameterError(4);
        }
        if (k > draws) {
            return ParameterError(1);
        }
        if (success > population || draws > population) {
            return FunctionError();
        }
        return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
