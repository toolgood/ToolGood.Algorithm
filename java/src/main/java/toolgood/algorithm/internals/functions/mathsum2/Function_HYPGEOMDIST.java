package toolgood.algorithm.internals.functions.mathsum2;

import java.math.BigDecimal;
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
    public Function_HYPGEOMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    public Function_HYPGEOMDIST(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "HypgeomDist";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }
        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) {
            return args3;
        }
        Operand args4 = GetNumber_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) {
            return args4;
        }

        int sampleS = args1.IntValue();
        int numberSample = args2.IntValue();
        int populationS = args3.IntValue();
        int numberPop = args4.IntValue();

        if (sampleS < 0 || numberSample < 0 || populationS < 0 || numberPop < 0 ||
            sampleS > numberSample || sampleS > populationS || numberSample > numberPop || populationS > numberPop) {
            return Operand.Error("Function '{0}' parameter is error!", "HypgeomDist");
        }
        return Operand.Create(ExcelFunctions.HypgeomDist(sampleS, numberSample, populationS, numberPop));
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
