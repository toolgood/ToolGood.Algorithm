package toolgood.algorithm.internals.functions.csharpWeb;

import java.nio.charset.StandardCharsets;
import java.util.Base64;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Enums.OperandType;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

final class Function_BASE64URLTOTEXT extends Function_1 {
	public Function_BASE64URLTOTEXT(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length != 1) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
		}
	}

	@Override
	public String Name() {
		return "Base64UrlToText";
	}

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetText_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) {
			return args1;
		}
		try {
			String base64Url = args1.TextValue().replace('-', '+').replace('_', '/');
			int padding = 4 - (base64Url.length() % 4);
			if (padding < 4) {
				StringBuilder sb = new StringBuilder(base64Url);
				for (int i = 0; i < padding; i++) {
					sb.append('=');
				}
				base64Url = sb.toString();
			}
			byte[] decoded = Base64.getDecoder().decode(base64Url);
			String t = new String(decoded, StandardCharsets.UTF_8);
			return Operand.Create(t);
		} catch (Exception e) {
			return ParameterError(1);
		}
	}

	@Override
	public OperandType GetResultType() {
		return OperandType.TEXT;
	}

	@Override
	void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
	}
}
