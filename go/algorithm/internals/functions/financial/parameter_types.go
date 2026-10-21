package financial

// init 声明各函数的参数期望类型（对应 C# Function_XXX.GetParameterTypes 重写）。
func init() {
	for _, name := range []string{
		"PMT", "PV", "FV", "IPMT", "PPMT", "NPER", "RATE", "NPV", "MIRR", "XNPV",
		"SLN", "SYD", "DB", "DDB",
	} {
		RegisterParameterTypes(name, OperandTypeNumber)
	}
	RegisterParameterTypes("IRR", OperandTypeArray, OperandTypeNumber)
	RegisterParameterTypes("XIRR", OperandTypeArray, OperandTypeArray, OperandTypeNumber)
}
