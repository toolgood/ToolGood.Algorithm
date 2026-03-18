package toolgood.algorithm.internals.functions.csharpsecurity;

import java.lang.StringBuilder;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.function.BiFunction;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_HMACSHA256 extends Function_2 {
    public Function_HMACSHA256(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "HmacSHA256";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsError() || args1.IsNone()) {
            return args1;
        }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsError() || args2.IsNone()) {
            return args2;
        }

        byte[] buffer = args1.TextValue().getBytes(StandardCharsets.UTF_8);
        String t = getHmacSha256String(buffer, args2.TextValue());
        return Operand.Create(t);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
    }

    private String getHmacSha256String(byte[] buffer, String secret) throws Exception {
        byte[] keyByte = secret != null ? secret.getBytes(StandardCharsets.UTF_8) : new byte[0];
        SecretKeySpec keySpec = new SecretKeySpec(keyByte, "HmacSHA256");
        Mac mac = Mac.getInstance("HmacSHA256");
        mac.init(keySpec);
        byte[] hashMessage = mac.doFinal(buffer);
        return bytesToHex(hashMessage);
    }

    private String bytesToHex(byte[] bytes) {
        StringBuilder sb = new StringBuilder();
        for (byte b : bytes) {
            sb.append(String.format("%02X", b));
        }
        return sb.toString();
    }
}
