package calculationlogic

import (
	"errors"
	"fmt"
	"strconv"
	"strings"
	"time"
)

// dateTimeExpFormat 日期字面量使用的格式（对应 C# 的 "yyyy-MM-dd HH:mm:ss"）。
const dateTimeExpFormat = "yyyy-MM-dd HH:mm:ss"

// FunctionCache 计算逻辑引擎所需的表达式缓存能力（对应 C# IFunctionCache 中被本引擎使用的部分）。
//
// 根包 algorithm 的 FunctionCache 只要实现这两个方法即可直接传入（Go 结构化接口）。
// NewCalculationLogicEngine 允许传 nil，此时退化为直接使用内部 AlgorithmEngine 的 Parse（无缓存），
// 且 Exp2（参数替换后的公式）无法分析，退化为原始公式。
type FunctionCache interface {
	// ParseWithCache 解析计算表达式，并使用缓存。
	ParseWithCache(exp string) (Function, error)
	// GetDiyNamesWithCache 获取自定义名称信息，并使用缓存。
	GetDiyNamesWithCache(exp string) *DiyNameInfo
}

// CalculationLogicEngine 计算逻辑引擎（对应 C# CalculationLogicEngine）。
//
// 按「场景 → 初始值 → 条件 → 赋值」的顺序累积参数，并可把每一步记录为 CalculationLogicInfo。
type CalculationLogicEngine struct {
	functionCache           FunctionCache
	useCalculationLogicInfo bool
	engine                  *AlgorithmEngine
	parameters              map[string]Operand
	initValueInfos          []*CalculationLogicInfo
	calculationLogicInfos   []*CalculationLogicInfo
}

// NewCalculationLogicEngine 创建计算逻辑引擎。
//
// useCalculationLogicInfo 为 false 时不记录 CalculationLogicInfo（对应 C# 关闭日志的用法）。
func NewCalculationLogicEngine(functionCache FunctionCache, useCalculationLogicInfo bool) *CalculationLogicEngine {
	e := &CalculationLogicEngine{
		functionCache:           functionCache,
		useCalculationLogicInfo: useCalculationLogicInfo,
		parameters:              map[string]Operand{},
	}
	e.engine = NewAlgorithmEngine(false)
	e.engine.GetParameterFunc = e.getParameter
	return e
}

// Engine 返回内部表达式引擎，便于上层调整 ExcelIndex / UseLocalTime / UseStrictMode 等选项。
func (e *CalculationLogicEngine) Engine() *AlgorithmEngine {
	return e.engine
}

// SetSceneName 设置场景名称。
func (e *CalculationLogicEngine) SetSceneName(scene string) {
	if e.useCalculationLogicInfo {
		e.calculationLogicInfos = append(e.calculationLogicInfos, &CalculationLogicInfo{
			LogicType: LogicTypeScene,
			Name:      scene,
		})
	}
}

// InitValue 初始化值。
//
// value 支持 string / bool / int / int64 / float64 / Decimal / *MyDate / time.Time / Operand，
// 对应 C# 的 InitValue 系列重载；layer 传 0、remark 传 "" 即对应 C# 省略这两个参数的重载。
func (e *CalculationLogicEngine) InitValue(key string, value interface{}, layer int, remark string) {
	operand, exp := operandOf(value, false)
	e.parameters[key] = operand
	if e.useCalculationLogicInfo {
		e.initValueInfos = append(e.initValueInfos, &CalculationLogicInfo{
			LogicType: LogicTypeInitValue,
			Name:      key,
			Exp:       exp,
			Layer:     layer,
			Remark:    remark,
		})
	}
}

// CheckCondition 检查条件。
//
// 条件结果必须是布尔值，否则返回错误（对应 C# 抛 FormatException）。
func (e *CalculationLogicEngine) CheckCondition(condition string, layer int, remark string) (bool, error) {
	operand, err := e.evaluate(condition)
	if err != nil {
		return false, err
	}
	operand = operand.ToBoolean("The condition must be a boolean value!")
	if operand.IsError() {
		if e.useCalculationLogicInfo {
			e.calculationLogicInfos = append(e.calculationLogicInfos, &CalculationLogicInfo{
				LogicType: LogicTypeCondition,
				Exp:       condition,
				Exp2:      e.expAnalysis(condition),
				Value:     operand,
				Layer:     layer,
				Remark:    remark,
			})
		}
		return false, errors.New(operand.ErrorMsg())
	}
	if e.useCalculationLogicInfo {
		e.calculationLogicInfos = append(e.calculationLogicInfos, &CalculationLogicInfo{
			LogicType: LogicTypeCondition,
			Exp:       condition,
			Exp2:      e.expAnalysis(condition),
			Value:     operand,
			Layer:     layer,
			Remark:    remark,
		})
	}
	return operand.BooleanValue(), nil
}

// SetFormula 用公式赋值。
func (e *CalculationLogicEngine) SetFormula(key string, exp string, layer int, remark string) error {
	operand, err := e.evaluate(exp)
	if err != nil {
		return err
	}
	if operand.IsError() {
		if e.useCalculationLogicInfo {
			e.calculationLogicInfos = append(e.calculationLogicInfos, &CalculationLogicInfo{
				LogicType: LogicTypeSetFormula,
				Name:      key,
				Exp:       exp,
				Exp2:      e.expAnalysis(exp),
				Value:     operand,
				Layer:     layer,
				Remark:    remark,
			})
		}
		return errors.New(operand.ErrorMsg())
	}
	e.parameters[key] = operand

	if e.useCalculationLogicInfo {
		e.calculationLogicInfos = append(e.calculationLogicInfos, &CalculationLogicInfo{
			LogicType: LogicTypeSetFormula,
			Name:      key,
			Exp:       exp,
			Exp2:      e.expAnalysis(exp),
			Value:     operand,
			Layer:     layer,
			Remark:    remark,
		})
	}
	return nil
}

// SetValue 设置值。
//
// value 支持的类型与 InitValue 相同。
func (e *CalculationLogicEngine) SetValue(key string, value interface{}, layer int, remark string) {
	operand, exp := operandOf(value, true)
	e.parameters[key] = operand
	if e.useCalculationLogicInfo {
		e.calculationLogicInfos = append(e.calculationLogicInfos, &CalculationLogicInfo{
			LogicType: LogicTypeSetValue,
			Name:      key,
			Exp:       exp,
			Layer:     layer,
			Remark:    remark,
		})
	}
}

// GetValue 获取值。
func (e *CalculationLogicEngine) GetValue(key string) Operand {
	return e.getParameter(key)
}

// BlankLine 添加空行。
func (e *CalculationLogicEngine) BlankLine() {
	if e.useCalculationLogicInfo {
		e.calculationLogicInfos = append(e.calculationLogicInfos, &CalculationLogicInfo{
			LogicType: LogicTypeBlankLine,
		})
	}
}

// ToInfoString 转换为信息字符串。
func (e *CalculationLogicEngine) ToInfoString() string {
	if !e.useCalculationLogicInfo {
		return ""
	}
	var sb strings.Builder
	if len(e.initValueInfos) > 0 {
		sb.WriteString("[初始] ")
		for _, item := range e.initValueInfos {
			sb.WriteString(item.ToInfoString())
		}
		sb.WriteString("\n")
	}
	for _, item := range e.calculationLogicInfos {
		sb.WriteString(item.ToInfoString())
		sb.WriteString("\n")
	}
	return sb.String()
}

// getParameter 参数取值：未定义的参数返回错误操作数（对应 C# AlgorithmEngineEx.GetParameterEx）。
func (e *CalculationLogicEngine) getParameter(name string) Operand {
	if operand, ok := e.parameters[name]; ok {
		return operand
	}
	return Errorf("Parameter [{0}] is missing.", name)
}

// parse 解析表达式：优先走 FunctionCache，未提供缓存时退回引擎自带解析。
func (e *CalculationLogicEngine) parse(exp string) (Function, error) {
	if e.functionCache != nil {
		return e.functionCache.ParseWithCache(exp)
	}
	return e.engine.Parse(exp)
}

// evaluate 解析并求值，求值期间的 panic 转为错误操作数。
func (e *CalculationLogicEngine) evaluate(exp string) (Operand, error) {
	fn, err := e.parse(exp)
	if err != nil {
		return nil, err
	}
	return e.engine.EvaluateSafe(fn), nil
}

// expAnalysis 把公式中的参数名替换为当前值（对应 C# ExpAnalysis）。
func (e *CalculationLogicEngine) expAnalysis(exp string) string {
	if e.functionCache == nil {
		return exp
	}
	diyNameInfo := e.functionCache.GetDiyNamesWithCache(exp)
	if diyNameInfo == nil {
		return exp
	}
	var sb strings.Builder
	index := 0
	for _, parameter := range diyNameInfo.Parameters {
		start, end := parameter.Start, parameter.End
		if start > len(exp) || end >= len(exp) || end < start {
			// 名称位置越界（缓存实现给出的位置与 exp 不一致），跳过该参数。
			continue
		}
		if index < start {
			sb.WriteString(exp[index:start])
		}
		sb.WriteString(e.getOperand(parameter.Name))
		index = end + 1
	}
	if index < len(exp) {
		sb.WriteString(exp[index:])
	}
	return sb.String()
}

// getOperand 把参数值格式化为公式文本（对应 C# GetOperand）。
func (e *CalculationLogicEngine) getOperand(name string) string {
	operand := e.getParameter(name)
	if operand.IsError() {
		message := strings.ReplaceAll(operand.ErrorMsg(), "\\", "\\\\")
		message = strings.ReplaceAll(message, "\"", "\\\"")
		return "error(\"" + message + "\")"
	}
	if operand.Type() == OperandTypeText {
		return operand.String()
	}
	return operand.ToText("").TextValue()
}

// operandOf 把 Go 原生值转换为 Operand，并生成日志中记录的表达式文本。
//
// quotedText 对应 C# SetValue(string) 把文本包成 "..."（并转义双引号）的行为，
// InitValue(string) 则直接使用原始文本。
func operandOf(value interface{}, quotedText bool) (Operand, string) {
	switch v := value.(type) {
	case Operand:
		return v, v.String()
	case string:
		if quotedText {
			return CreateString(v), "\"" + strings.ReplaceAll(v, "\"", "\\\"") + "\""
		}
		return CreateString(v), v
	case bool:
		if v {
			return CreateBool(v), "True"
		}
		return CreateBool(v), "False"
	case int:
		return CreateInt(v), strconv.Itoa(v)
	case int64:
		return CreateInt64(v), strconv.FormatInt(v, 10)
	case float64:
		return CreateFloat(v), strconv.FormatFloat(v, 'g', -1, 64)
	case Decimal:
		return CreateDecimal(v), v.String()
	case *MyDate:
		return CreateMyDate(v), "\"" + v.ToString(dateTimeExpFormat) + "\""
	case time.Time:
		date := NewMyDateFromTime(v)
		return CreateMyDate(date), "\"" + date.ToString(dateTimeExpFormat) + "\""
	}
	panic(fmt.Sprintf("Unsupported value type %T.", value))
}
