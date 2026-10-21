package algorithm

import (
	"errors"
	"fmt"
	"strings"
	"unicode"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/enums"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/operator"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/unitconversion"
)

// 本文件移植 C# 的 AlgorithmEngineHelper（静态工具类）。
// C# 的静态方法在 Go 中映射为包级函数；依赖 ANTLR 的 visitor
// （DiyNameVisitor / MathSplitVisitor / MathSplitVisitor2）用 Parser 生成的语法树等价实现。

// ParseFormula 编译公式（对应 C# AlgorithmEngineHelper.ParseFormula）。
func ParseFormula(exp string) (Function, error) {
	if strings.TrimSpace(exp) == "" {
		return nil, errors.New("Parameter exp invalid !")
	}
	return parseFunction(exp)
}

// CheckFormula 检查公式是否正确（对应 C# AlgorithmEngineHelper.CheckFormula）。
func CheckFormula(exp string) bool {
	if strings.TrimSpace(exp) == "" {
		return false
	}
	_, err := parseFunction(exp)
	return err == nil
}

// IsParameter 判断文本是否为一个参数名（对应 C# AlgorithmEngineHelper.IsParameter）。
func IsParameter(parameter string) bool {
	if strings.TrimSpace(parameter) == "" {
		return false
	}
	diy, err := GetDiyNames(parameter)
	if err != nil {
		return false
	}
	if len(diy.Functions) > 0 {
		return false
	}
	if len(diy.Parameters) == 1 {
		return diy.Parameters[0].Name == parameter
	}
	return false
}

// IsKeywords 判断文本是否为内置关键字 / 函数名。
//
// Go 附加方法：C# 的 AlgorithmEngineHelper 没有对应方法，等价能力由词法关键字表提供。
func IsKeywords(text string) bool {
	if text == "" {
		return false
	}
	_, ok := Keywords[strings.ToUpper(text)]
	return ok
}

// GetDiyNames 获取表达式中的自定义参数名与自定义函数名
// （对应 C# AlgorithmEngineHelper.GetDiyNames，C# 解析失败时抛 ArgumentException）。
func GetDiyNames(exp string) (*DiyNameInfo, error) {
	if strings.TrimSpace(exp) == "" {
		return nil, errors.New("Parameter exp invalid !")
	}
	node, err := parseAST(exp)
	if err != nil {
		return nil, err
	}

	parameters := []DiyNameKeyInfo{}
	functions := []DiyNameKeyInfo{}
	collectDiyNames(node, &parameters, &functions)
	return &DiyNameInfo{Parameters: parameters, Functions: functions}, nil
}

// ParseCondition 解析条件表达式为条件树（对应 C# AlgorithmEngineHelper.ParseCondition）。
func ParseCondition(condition string) (tree *ConditionTree) {
	tree = NewConditionTree()
	if strings.TrimSpace(condition) == "" {
		tree.Type = enums.ConditionTreeTypeError
		tree.ErrorMessage = "condition is null"
		return tree
	}
	defer func() {
		if r := recover(); r != nil {
			tree = NewConditionTree()
			tree.Type = enums.ConditionTreeTypeError
			tree.ErrorMessage = parseErrorMessage(r)
		}
	}()
	node := NewParser(condition).Parse()
	return buildConditionTree(node, condition, false)
}

// ParseCalculate 解析计算表达式为计算树（对应 C# AlgorithmEngineHelper.ParseCalculate）。
func ParseCalculate(exp string) (tree *CalculateTree) {
	tree = NewCalculateTree()
	if strings.TrimSpace(exp) == "" {
		tree.Type = enums.CalculateTreeTypeError
		tree.ErrorMessage = "exp is null"
		return tree
	}
	defer func() {
		if r := recover(); r != nil {
			tree = NewCalculateTree()
			tree.Type = enums.CalculateTreeTypeError
			tree.ErrorMessage = parseErrorMessage(r)
		}
	}()
	node := NewParser(exp).Parse()
	return buildCalculateTree(node, exp, false)
}

// Condition_And 组合两个节点为逻辑与（对应 C# AlgorithmEngineHelper.Condition_And）。
func Condition_And(left, right Function) Function {
	return operator.NewFunctionAnd2([]Function{left, right})
}

// Condition_Or 组合两个节点为逻辑或（对应 C# AlgorithmEngineHelper.Condition_Or）。
func Condition_Or(left, right Function) Function {
	return operator.NewFunctionOr2([]Function{left, right})
}

// ErrorFunction 生成错误节点（对应 C# AlgorithmEngineHelper.Error）。
//
// 名称带 Function 后缀：本包的 Error 已被操作数工厂占用（alias.go）。
func ErrorFunction(txt Function) Function {
	return internals.GetFunction("ERROR", []Function{txt})
}

// CombineCalculate 拼接计算表达式（对应 C# AlgorithmEngineHelper.CombineCalculate）。
//
// calcType 用 enums.CombineCalculateType（alias.go 未导出该类型别名）。
func CombineCalculate(left Function, calcType enums.CombineCalculateType, right Function) Function {
	var name string
	switch calcType {
	case enums.CombineCalculateTypeAdd:
		name = "ADD"
	case enums.CombineCalculateTypeSub:
		name = "SUB"
	case enums.CombineCalculateTypeMul:
		name = "MUL"
	case enums.CombineCalculateTypeDiv:
		name = "DIV"
	case enums.CombineCalculateTypeMod:
		name = "MOD"
	case enums.CombineCalculateTypeConnect:
		name = "CONNECT"
	case enums.CombineCalculateTypeAnd:
		return Condition_And(left, right)
	case enums.CombineCalculateTypeOr:
		return Condition_Or(left, right)
	case enums.CombineCalculateTypeOpGt:
		name = "GT"
	case enums.CombineCalculateTypeOpLt:
		name = "LT"
	case enums.CombineCalculateTypeOpGe:
		name = "GE"
	case enums.CombineCalculateTypeOpLe:
		name = "LE"
	case enums.CombineCalculateTypeOpEq:
		name = "EQ"
	default: // 含 CombineCalculateTypeOpNe
		name = "NE"
	}
	return internals.GetFunction(name, []Function{left, right})
}

// SplitFormula 按顶层分隔符切分公式（对应 internals.CharUtil.SplitFormula）。
func SplitFormula(formula string, splitChars string) []string {
	return internals.SplitFormula(formula, splitChars)
}

// UnitConversion 单位转换（对应 C# AlgorithmEngineHelper.UnitConversion）。
//
// name 对应 C# 的可选参数 name = null（错误信息中显示的项目名），可省略。
func UnitConversion(src Decimal, oldSrcUnit, oldTarUnit string, name ...string) (Decimal, error) {
	if strings.TrimSpace(oldSrcUnit) == "" || strings.TrimSpace(oldTarUnit) == "" {
		return src, nil
	}
	if oldSrcUnit == oldTarUnit {
		return src, nil
	}
	if result, ok := tryConvertUnit(src, oldSrcUnit, oldTarUnit); ok {
		return result, nil
	}

	oldSrcUnit = stripUnitChars(oldSrcUnit)
	oldTarUnit = stripUnitChars(oldTarUnit)
	if result, ok := tryConvertUnit(src, oldSrcUnit, oldTarUnit); ok {
		return result, nil
	}

	itemName := ""
	if len(name) > 0 {
		itemName = name[0]
	}
	if itemName == "" {
		return DecimalZero, fmt.Errorf("The input item has different units and cannot be converted from [%s] to [%s]", oldSrcUnit, oldTarUnit)
	}
	return DecimalZero, fmt.Errorf("The input item [%s] has different units and cannot be converted from [%s] to [%s]", itemName, oldSrcUnit, oldTarUnit)
}

// GetParameterTypes 解析公式并推导其中的参数类型
// （对应 C# FunctionBase.GetParameterTypes(AlgorithmEngine)，exp 为公式文本）。
//
// 解析失败时返回 nil（C# 版本抛异常）。
func GetParameterTypes(exp string, engine *AlgorithmEngine) []ParameterType {
	fn, err := ParseFormula(exp)
	if err != nil {
		return nil
	}
	if engine == nil {
		engine = NewAlgorithmEngine(false)
	}
	noneEngine := NewNoneEngine(engine)
	result := []ParameterType{}
	fn.GetParameterTypes(noneEngine, &result, OperandTypeNone)
	return result
}

// ------------------------------------------------------------------ 内部实现

var calculateTreeTypes = map[string]enums.CalculateTreeType{
	"OPMUL": enums.CalculateTreeTypeMul,
	"OPDIV": enums.CalculateTreeTypeDiv,
	"OPMOD": enums.CalculateTreeTypeMod,
	"OPADD": enums.CalculateTreeTypeAdd,
	"OPSUB": enums.CalculateTreeTypeSub,
	"OPCAT": enums.CalculateTreeTypeConnect,
	"OPAND": enums.CalculateTreeTypeAnd,
	"OPOR":  enums.CalculateTreeTypeOr,
	"OPGT":  enums.CalculateTreeTypeOpGt,
	"OPLT":  enums.CalculateTreeTypeOpLt,
	"OPGE":  enums.CalculateTreeTypeOpGe,
	"OPLE":  enums.CalculateTreeTypeOpLe,
	"OPEQ":  enums.CalculateTreeTypeOpEq,
	"OPNE":  enums.CalculateTreeTypeOpNe,
}

// buildCalculateTree 等价于 C# 的 MathSplitVisitor2。
func buildCalculateTree(node *astNode, source string, hasBracket bool) *CalculateTree {
	if node.kind == "bracket" {
		return buildCalculateTree(node.children[0], source, true)
	}

	tree := NewCalculateTree()
	tree.Start = node.start
	tree.End = node.end
	tree.HasBracket = hasBracket
	tree.SetSource(source)
	if node.kind == "binary" {
		if treeType, ok := calculateTreeTypes[node.op]; ok {
			tree.Type = treeType
			tree.Nodes = []*CalculateTree{
				buildCalculateTree(node.children[0], source, false),
				buildCalculateTree(node.children[1], source, false),
			}
		}
	}
	return tree
}

// buildConditionTree 等价于 C# 的 MathSplitVisitor。
func buildConditionTree(node *astNode, source string, hasBracket bool) *ConditionTree {
	if node.kind == "bracket" {
		return buildConditionTree(node.children[0], source, true)
	}

	tree := NewConditionTree()
	tree.Start = node.start
	tree.End = node.end
	tree.HasBracket = hasBracket
	tree.SetSource(source)
	if node.kind == "binary" && (node.op == "OPAND" || node.op == "OPOR") {
		if node.op == "OPAND" {
			tree.Type = enums.ConditionTreeTypeAnd
		} else {
			tree.Type = enums.ConditionTreeTypeOr
		}
		tree.Nodes = []*ConditionTree{
			buildConditionTree(node.children[0], source, false),
			buildConditionTree(node.children[1], source, false),
		}
	}
	return tree
}

// collectDiyNames 等价于 C# 的 DiyNameVisitor：
//   - PARAMETER_fun -> Parameters
//   - GetJsonValue_fun 中 `expr[PARAMETER]` 的 PARAMETER -> Parameters（先于被访问的 expr）
//   - Function_fun 中 token 为 PARAMETER 的函数名 -> Functions
func collectDiyNames(node *astNode, parameters, functions *[]DiyNameKeyInfo) {
	if node.kind == "index" && node.keyIsParameter {
		key := node.children[1]
		*parameters = append(*parameters, DiyNameKeyInfo{Name: key.text, Start: key.start, End: key.end})
		collectDiyNames(node.children[0], parameters, functions)
		return
	}
	if (node.kind == "call" || node.kind == "method") && node.keyword == "PARAMETER" {
		*functions = append(*functions, DiyNameKeyInfo{Name: node.text, Start: node.start, End: node.end})
	} else if node.kind == "param" {
		*parameters = append(*parameters, DiyNameKeyInfo{Name: node.text, Start: node.start, End: node.end})
	}

	for _, child := range node.children {
		collectDiyNames(child, parameters, functions)
	}
}

// parseAST 解析表达式为语法树，panic 转为 error。
func parseAST(exp string) (node *astNode, err error) {
	defer func() {
		if r := recover(); r != nil {
			node = nil
			err = errors.New(parseErrorMessage(r))
		}
	}()
	return NewParser(exp).Parse(), nil
}

// parseErrorMessage 把 panic 值转成错误文本（与 parseFunction 的处理一致）。
func parseErrorMessage(r interface{}) string {
	switch e := r.(type) {
	case *ParseError:
		return e.Message
	case error:
		return e.Error()
	}
	return fmt.Sprint(r)
}

// stripUnitChars 去掉单位文本中的空白、括号与尖括号
// （对应 C# 的 unitRegex = [\s\(\)（）\[\]<>]）。
func stripUnitChars(unit string) string {
	return strings.Map(func(c rune) rune {
		if unicode.IsSpace(c) {
			return -1
		}
		switch c {
		case '(', ')', '（', '）', '[', ']', '<', '>':
			return -1
		}
		return c
	}, unit)
}

// tryConvertUnit 依次尝试长度 / 质量 / 面积 / 体积转换（顺序与 C# 一致）。
func tryConvertUnit(src Decimal, srcUnit, tarUnit string) (Decimal, bool) {
	if result, ok := unitconversion.TryConvertDistance(srcUnit, tarUnit, src); ok {
		return result, true
	}
	if result, ok := unitconversion.TryConvertMass(srcUnit, tarUnit, src); ok {
		return result, true
	}
	if result, ok := unitconversion.TryConvertArea(srcUnit, tarUnit, src); ok {
		return result, true
	}
	if result, ok := unitconversion.TryConvertVolume(srcUnit, tarUnit, src); ok {
		return result, true
	}
	return DecimalZero, false
}
