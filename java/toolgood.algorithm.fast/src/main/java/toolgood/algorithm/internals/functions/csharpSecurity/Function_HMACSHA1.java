package toolgood.algorithm.internals.functions.csharpSecurity;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.function.BiFunction;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_HMACSHA1 extends Function_2 {
	public Function_HMACSHA1(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length != 2) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
		}
	}

	@Override
	public String Name() {
		return "HmacSHA1";
	}

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetText_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) {
			return args1;
		}

		Operand args2 = GetText_2(engine, tempParameter);
		if (args2.IsErrorOrNone()) {
			return args2;
		}
		String t = GetHmacSha1String(args1.TextValue().getBytes(StandardCharsets.UTF_8), args2.TextValue());
		return Operand.Create(t);
	}

	private String GetHmacSha1String(byte[] buffer, String secret) {
		try {
			byte[] keyByte = (secret != null ? secret : "").getBytes(StandardCharsets.UTF_8);
			SecretKeySpec keySpec = new SecretKeySpec(keyByte, "HmacSHA1");
			Mac hmacSha1 = Mac.getInstance("HmacSHA1");
			hmacSha1.init(keySpec);
			byte[] hashMessage = hmacSha1.doFinal(buffer);
			return bytesToHex(hashMessage);
		} catch (Exception e) {
			throw new RuntimeException(e);
		}
	}

	private static String bytesToHex(byte[] bytes) {
		StringBuilder sb = new StringBuilder();
		for (byte b : bytes) {
			sb.append(String.format("%02X", b));
		}
		return sb.toString();
	}

	@Override
	public OperandType GetResultType() {
		return OperandType.TEXT;
	}

	@Override
	public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
	}
}
