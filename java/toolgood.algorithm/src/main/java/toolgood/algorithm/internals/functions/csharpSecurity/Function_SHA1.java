package toolgood.algorithm.internals.functions.csharpSecurity;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

public final class Function_SHA1 extends Function_1 {
	public Function_SHA1(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length != 1) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
		}
	}

	@Override
	public String Name() {
		return "SHA1";
	}

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetText_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) {
			return args1;
		}
		String t = GetSha1String(args1.TextValue().getBytes(StandardCharsets.UTF_8));
		return Operand.Create(t);
	}

	private String GetSha1String(byte[] buffer) {
		try {
			MessageDigest sha1 = MessageDigest.getInstance("SHA-1");
			byte[] retVal = sha1.digest(buffer);
			return bytesToHex(retVal);
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
	}
}
