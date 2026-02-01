package toolgood.algorithm.internals.functions.csharpsecurity;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.function.Function;

public class Function_MD5 extends Function_2 {
    public Function_MD5(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "MD5", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        try {
            java.nio.charset.Charset charset;
            if (func2 == null) {
                charset = java.nio.charset.StandardCharsets.UTF_8;
            } else {
                Operand args2 = func2.Evaluate(work, tempParameter);
                if (args2.IsNotText()) {
                    args2 = args2.ToText("Function '{0}' parameter {1} is error!", "MD5", 2);
                    if (args2.IsError()) {
                        return args2;
                    }
                }
                charset = java.nio.charset.Charset.forName(args2.TextValue());
            }
            byte[] buffer = args1.TextValue().getBytes(charset);
            String t = getMd5String(buffer);
            return Operand.Create(t);
        } catch (Exception ex) {
            return Operand.Error("Function '{0}' is error!{1}", "MD5", ex.getMessage());
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "MD5");
    }

    private String getMd5String(byte[] buffer) throws NoSuchAlgorithmException {
        MessageDigest md5 = MessageDigest.getInstance("MD5");
        byte[] retVal = md5.digest(buffer);
        return bytesToHex(retVal);
    }

    private String bytesToHex(byte[] bytes) {
        StringBuilder sb = new StringBuilder();
        for (byte b : bytes) {
            sb.append(String.format("%02x", b));
        }
        return sb.toString().toUpperCase();
    }
}
