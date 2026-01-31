package toolgood.algorithm.internals.functions.csharpsecurity;

import toolgood.algorithm.internals.Function_3;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.util.function.Function;

public class Function_HMACSHA1 extends Function_3 {
    public Function_HMACSHA1(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        try {
            java.nio.charset.Charset charset;
            if (func3 == null) {
                charset = java.nio.charset.StandardCharsets.UTF_8;
            } else {
                Operand args3 = func3.Evaluate(work, tempParameter);
                if (args3.IsNotText()) {
                    args3 = args3.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 3);
                    if (args3.IsError()) {
                        return args3;
                    }
                }
                charset = java.nio.charset.Charset.forName(args3.getTextValue());
            }
            byte[] buffer = args1.getTextValue().getBytes(charset);
            String secret = args2.getTextValue();
            String t = getHmacSha1String(buffer, secret);
            return Operand.Create(t);
        } catch (Exception ex) {
            return Operand.Error("Function '{0}' is error!{1}", "HmacSHA1", ex.getMessage());
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "HmacSHA1");
    }

    private String getHmacSha1String(byte[] buffer, String secret) throws NoSuchAlgorithmException, InvalidKeyException {
        byte[] keyByte = (secret != null ? secret : "").getBytes(java.nio.charset.StandardCharsets.UTF_8);
        Mac mac = Mac.getInstance("HmacSHA1");
        SecretKeySpec secretKeySpec = new SecretKeySpec(keyByte, "HmacSHA1");
        mac.init(secretKeySpec);
        byte[] hashmessage = mac.doFinal(buffer);
        return bytesToHex(hashmessage);
    }

    private String bytesToHex(byte[] bytes) {
        StringBuilder sb = new StringBuilder();
        for (byte b : bytes) {
            sb.append(String.format("%02x", b));
        }
        return sb.toString().toUpperCase();
    }
}
