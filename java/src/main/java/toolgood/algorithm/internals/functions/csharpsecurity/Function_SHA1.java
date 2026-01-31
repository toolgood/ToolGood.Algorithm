package toolgood.algorithm.internals.functions.csharpsecurity;

import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.function.Function;

public class Function_SHA1 extends Function_2 {
    public Function_SHA1(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "SHA1", 1);
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
                    args2 = args2.ToText("Function '{0}' parameter {1} is error!", "SHA1", 2);
                    if (args2.IsError()) {
                        return args2;
                    }
                }
                charset = java.nio.charset.Charset.forName(args2.getTextValue());
            }
            byte[] buffer = args1.getTextValue().getBytes(charset);
            String t = getSha1String(buffer);
            return Operand.Create(t);
        } catch (Exception ex) {
            return Operand.Error("Function '{0}' is error!{1}", "SHA1", ex.getMessage());
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "SHA1");
    }

    private String getSha1String(byte[] buffer) throws NoSuchAlgorithmException {
        MessageDigest sha1 = MessageDigest.getInstance("SHA-1");
        byte[] retVal = sha1.digest(buffer);
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
