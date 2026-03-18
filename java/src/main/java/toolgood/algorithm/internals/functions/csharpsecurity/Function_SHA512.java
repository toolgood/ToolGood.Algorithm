package toolgood.algorithm.internals.functions.csharpsecurity;

import java.lang.StringBuilder;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SHA512 extends Function_1 {
    public Function_SHA512(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "SHA512";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsError() || args1.IsNone()) {
            return args1;
        }
        byte[] buffer = args1.TextValue().getBytes(StandardCharsets.UTF_8);
        String t = getSha512String(buffer);
        return Operand.Create(t);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
    }

    private String getSha512String(byte[] buffer) throws Exception {
        MessageDigest sha512 = MessageDigest.getInstance("SHA-512");
        byte[] retVal = sha512.digest(buffer);
        return bytesToHex(retVal);
    }

    private String bytesToHex(byte[] bytes) {
        StringBuilder sb = new StringBuilder();
        for (byte b : bytes) {
            sb.append(String.format("%02X", b));
        }
        return sb.toString();
    }
}
