package mathtransformation

import (
	"regexp"
	"strings"
)

// 移植自 Internals/Functions/MathTransformation/Function_ROMAN.cs、Function_ARABIC.cs。

var romanRegex = regexp.MustCompile(`^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$`)

var romanValues = [][]int{
	{1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1},
	{1000, 950, 900, 500, 450, 400, 100, 95, 90, 50, 45, 40, 10, 9, 5, 4, 1},
	{1000, 990, 950, 900, 500, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1},
	{1000, 995, 990, 950, 900, 500, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1},
	{1000, 999, 995, 990, 950, 900, 500, 499, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1},
}

var romanSymbols = [][]string{
	{"M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"},
	{"M", "LM", "CM", "D", "LD", "CD", "C", "VC", "XC", "L", "VL", "XL", "X", "IX", "V", "IV", "I"},
	{"M", "XM", "LM", "CM", "D", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"},
	{"M", "VM", "XM", "LM", "CM", "D", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"},
	{"M", "IM", "VM", "XM", "LM", "CM", "D", "ID", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"},
}

// ArabicToRoman form 0: 经典形式；form 4: 最简化形式。
func arabicToRoman(num, form int) string {
	if num == 0 {
		return ""
	}
	var sb strings.Builder
	values := romanValues[form]
	symbols := romanSymbols[form]
	for i := 0; i < len(values); i++ {
		for num >= values[i] {
			sb.WriteString(symbols[i])
			num -= values[i]
		}
	}
	return sb.String()
}

func getRomanValue(c byte) int {
	switch c {
	case 'I':
		return 1
	case 'V':
		return 5
	case 'X':
		return 10
	case 'L':
		return 50
	case 'C':
		return 100
	case 'D':
		return 500
	case 'M':
		return 1000
	}
	return -1
}

func romanToArabic(roman string) int {
	result := 0
	prevValue := 0
	for i := len(roman) - 1; i >= 0; i-- {
		value := getRomanValue(roman[i])
		if value < 0 {
			return -1
		}
		if value < prevValue {
			result -= value
		} else {
			result += value
		}
		prevValue = value
	}
	return result
}

// ---------------------------------------------------------------- ROMAN

type FunctionROMAN struct{ BaseFunction }

func newFunctionROMAN(args []Function) Function {
	RequireArgs(args, "ROMAN", 1, 2)
	return &FunctionROMAN{BaseFunction: NewBase("ROMAN", OperandTypeText, args)}
}

func (f *FunctionROMAN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	numArg := f.GetNumber(1, engine, tp)
	if numArg.IsErrorOrNone() {
		return numArg
	}
	num, ok := tryGetInt(numArg)
	if !ok {
		return f.ParameterError(1)
	}
	// Excel：number 为负或大于 3999 时返回 #VALUE!，number 为 0 返回空文本
	if num < 0 || num > 3999 {
		return f.ParameterError(1)
	}

	form := 0
	if f.ArgCount() > 1 {
		formArg := f.EvalArg(1, engine, tp)
		if formArg.IsErrorOrNone() {
			return formArg
		}
		if formArg.IsBoolean() {
			// Excel：form 为 TRUE 时使用古典形式(0)，FALSE 时使用最简化形式(4)
			if formArg.BooleanValue() {
				form = 0
			} else {
				form = 4
			}
		} else {
			formNum := f.ConvertToNumber(formArg, 2)
			if formNum.IsErrorOrNone() {
				return formNum
			}
			v, ok := tryGetInt(formNum)
			if !ok {
				return f.ParameterError(2)
			}
			form = v
			// Excel：form 超出 0~4 范围返回 #VALUE!
			if form < 0 || form > 4 {
				return f.ParameterError(2)
			}
		}
	}
	return CreateString(arabicToRoman(num, form))
}

// ResultType ROMAN 固定输出文本。
func (f *FunctionROMAN) ResultType() OperandType { return OperandTypeText }

func (f *FunctionROMAN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ARABIC

type FunctionARABIC struct{ BaseFunction }

func newFunctionARABIC(args []Function) Function {
	RequireArgs(args, "ARABIC", 1, 1)
	return &FunctionARABIC{BaseFunction: NewBase("ARABIC", OperandTypeNumber, args)}
}

func (f *FunctionARABIC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	arg := f.GetText(1, engine, tp)
	if arg.IsErrorOrNone() {
		return arg
	}
	// 裁剪首尾空白后统一转大写，与 Excel 忽略空白/大小写的语义一致
	text := strings.ToUpper(strings.TrimSpace(arg.TextValue()))
	// Excel：空文本或不符合罗马数字语法时返回 #VALUE!
	if text == "" || !romanRegex.MatchString(text) {
		return f.ParameterError(1)
	}
	result := romanToArabic(text)
	if result < 0 {
		return f.ParameterError(1)
	}
	return CreateInt(result)
}

func (f *FunctionARABIC) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
}

func init() {
	RegisterFunction("ROMAN", newFunctionROMAN)
	RegisterFunction("ARABIC", newFunctionARABIC)
}
