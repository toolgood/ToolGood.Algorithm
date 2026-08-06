package toolgood.algorithm.internals.functions.csharpWeb;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

public final class Function_URLENCODE extends Function_1 {
	public Function_URLENCODE(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length != 1) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
		}
	}

	@Override
	public String Name() {
		return "UrlEncode";
	}

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetText_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) {
			return args1;
		}
		String s = args1.TextValue();
		String r = UrlEncode(s);
		return Operand.Create(r);
	}

	private static String UrlEncode(String s) {
		byte[] bytes = s.getBytes(StandardCharsets.UTF_8);
		StringBuilder sb = new StringBuilder(bytes.length);
		for (byte b : bytes) {
			int v = b & 0xFF;
			if ((v >= 'a' && v <= 'z') || (v >= 'A' && v <= 'Z') || (v >= '0' && v <= '9')
					|| v == '-' || v == '_' || v == '.' || v == '~') {
				sb.append((char) v);
			} else if (v == ' ') {
				sb.append('+');
			} else {
				sb.append('%').append(HEX[v >>> 4]).append(HEX[v & 0x0F]);
			}
		}
		return sb.toString();
	}

	private static final char[] HEX = "0123456789abcdef".toCharArray();

	@Override
	public OperandType GetResultType() {
		return OperandType.TEXT;
	}

	@Override
	public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
	}
}
