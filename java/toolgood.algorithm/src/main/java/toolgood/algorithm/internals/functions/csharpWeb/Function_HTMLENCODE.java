package toolgood.algorithm.internals.functions.csharpWeb;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Enums.OperandType;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

final class Function_HTMLENCODE extends Function_1 {
	public Function_HTMLENCODE(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length != 1) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
		}
	}

	@Override
	public String Name() {
		return "HtmlEncode";
	}

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetText_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) {
			return args1;
		}
		String s = args1.TextValue();
		String r = HtmlEncode(s);
		return Operand.Create(r);
	}

	private static String HtmlEncode(String s) {
		if (s == null || s.isEmpty()) {
			return s;
		}
		return s.replace("&", "&amp;")
				.replace("<", "&lt;")
				.replace(">", "&gt;")
				.replace("\"", "&quot;")
				.replace("'", "&#39;");
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
