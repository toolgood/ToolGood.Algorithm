package internals

import (
	"errors"
	"fmt"
	"strings"
	"time"
)

// AlgorithmEngine 表达式引擎，对应 C# / Java 的 AlgorithmEngine。
type AlgorithmEngine struct {
	// ExcelIndex 1 表示 Excel 下标（从 1 开始），0 表示从 0 开始。
	ExcelIndex int
	// IgnoreCase 参数名是否忽略大小写。
	IgnoreCase bool
	// UseLocalTime 日期计算是否使用本地时间。
	UseLocalTime bool
	// UseStrictMode 严格模式：逻辑运算不短路。
	UseStrictMode bool
	// DistanceUnit 默认长度单位。
	DistanceUnit DistanceUnitType
	// AreaUnit 默认面积单位。
	AreaUnit AreaUnitType
	// VolumeUnit 默认体积单位。
	VolumeUnit VolumeUnitType
	// MassUnit 默认质量单位。
	MassUnit MassUnitType
	// LastError 最近一次错误信息。
	LastError string
	// UseParseCache 是否缓存解析结果。
	UseParseCache bool

	// GetParameterFunc 自定义参数取值函数。
	GetParameterFunc func(name string) Operand
	// ExecuteDiyFunctionFunc 自定义函数执行器。
	ExecuteDiyFunctionFunc func(name string, args []Operand) Operand

	parseCache map[string]Function
	noneMode   bool
}

// NewAlgorithmEngine 创建引擎实例。
func NewAlgorithmEngine(ignoreCase bool) *AlgorithmEngine {
	return &AlgorithmEngine{
		ExcelIndex:    1,
		IgnoreCase:    ignoreCase,
		UseLocalTime:  true,
		UseStrictMode: true,
		DistanceUnit:  DistanceUnitTypeM,
		AreaUnit:      AreaUnitTypeM2,
		VolumeUnit:    VolumeUnitTypeM3,
		MassUnit:      MassUnitTypeKG,
		parseCache:    map[string]Function{},
	}
}

// SetUseExcelIndex 设置下标是否从 1 开始。
func (e *AlgorithmEngine) SetUseExcelIndex(value bool) {
	if value {
		e.ExcelIndex = 1
	} else {
		e.ExcelIndex = 0
	}
}

// GetParameter 取参数值；可通过 GetParameterFunc 覆盖。
func (e *AlgorithmEngine) GetParameter(parameter string) Operand {
	if e.noneMode {
		return Null
	}
	if e.GetParameterFunc != nil {
		return e.GetParameterFunc(parameter)
	}
	return Errorf("Parameter [{0}] is missing.", parameter)
}

// ExecuteDiyFunction 执行自定义函数；可通过 ExecuteDiyFunctionFunc 覆盖。
func (e *AlgorithmEngine) ExecuteDiyFunction(name string, args []Operand) Operand {
	if e.noneMode {
		return Null
	}
	if e.ExecuteDiyFunctionFunc != nil {
		return e.ExecuteDiyFunctionFunc(name, args)
	}
	return Errorf("DiyFunction [{0}] is missing.", name)
}

// Parse 解析表达式，失败时返回错误。
var ParseExpression func(text string) (Function, error)

func parseExpression(text string) (Function, error) {
	if ParseExpression == nil {
		return nil, errors.New("parser not registered")
	}
	return ParseExpression(text)
}

func (e *AlgorithmEngine) Parse(exp string) (fn Function, err error) {
	e.LastError = ""
	defer func() {
		if r := recover(); r != nil {
			e.LastError = fmt.Sprint(r)
			err = errors.New(e.LastError)
			fn = nil
		}
	}()
	trimmed := strings.TrimSpace(exp)
	if trimmed == "" {
		e.LastError = "Parameter exp invalid !"
		return nil, errors.New(e.LastError)
	}
	if e.UseParseCache {
		if cached, ok := e.parseCache[trimmed]; ok {
			return cached, nil
		}
		fn, err = parseExpression(trimmed)
		if err != nil {
			e.LastError = err.Error()
			return nil, err
		}
		e.parseCache[trimmed] = fn
		return fn, nil
	}
	fn, err = parseExpression(trimmed)
	if err != nil {
		e.LastError = err.Error()
		return nil, err
	}
	return fn, nil
}

// ParseWithoutError 解析表达式，失败时返回 nil。
func (e *AlgorithmEngine) ParseWithoutError(exp string) Function {
	fn, err := e.Parse(exp)
	if err != nil {
		return nil
	}
	return fn
}

// Evaluate 对节点求值。
func (e *AlgorithmEngine) Evaluate(fn Function) Operand {
	tp := TempParameter(func(name string) Operand { return e.GetParameter(name) })
	return fn.Evaluate(e, tp)
}

// EvaluateSafe 对节点求值，内部 panic 转换为错误操作数。
func (e *AlgorithmEngine) EvaluateSafe(fn Function) (result Operand) {
	defer func() {
		if r := recover(); r != nil {
			e.LastError = fmt.Sprint(r)
			result = Error(e.LastError)
		}
	}()
	return e.Evaluate(fn)
}

// evaluateExp 解析并求值，失败时返回 nil 与 false。
func (e *AlgorithmEngine) evaluateExp(exp string) (Operand, bool) {
	fn := e.ParseWithoutError(exp)
	if fn == nil {
		return nil, false
	}
	return e.EvaluateSafe(fn), true
}

// TryEvaluate 求值并按 def 的类型转换（对应 TryEvaluate(string, object)）。
//
// def 可为 bool / int / int64 / float64 / Decimal / string / *MyDate / time.Time / Operand / nil。
func (e *AlgorithmEngine) TryEvaluate(exp string, def interface{}) interface{} {
	obj, ok := e.evaluateExp(exp)
	if !ok {
		return def
	}
	if obj.IsError() {
		e.LastError = obj.ErrorMsg()
		return def
	}
	switch def.(type) {
	case nil:
		if obj.IsNone() {
			return nil
		}
		return obj
	case bool:
		converted := obj
		if !obj.IsBoolean() {
			converted = obj.ToBoolean("It can't be converted to bool!")
		}
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
			return def
		}
		return converted.BooleanValue()
	case int:
		converted := obj
		if !obj.IsNumber() {
			converted = obj.ToNumber("It can't be converted to number!")
		}
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
			return def
		}
		return converted.IntValue()
	case int64:
		converted := obj
		if !obj.IsNumber() {
			converted = obj.ToNumber("It can't be converted to number!")
		}
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
			return def
		}
		return converted.LongValue()
	case float64:
		converted := obj
		if !obj.IsNumber() {
			converted = obj.ToNumber("It can't be converted to number!")
		}
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
			return def
		}
		return converted.DoubleValue()
	case Decimal:
		converted := obj
		if !obj.IsNumber() {
			converted = obj.ToNumber("It can't be converted to number!")
		}
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
			return def
		}
		return converted.NumberValue()
	case string:
		converted := obj
		if !obj.IsText() {
			converted = obj.ToText("It can't be converted to string!")
		}
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
			return def
		}
		return converted.TextValue()
	case *MyDate:
		converted := obj
		if !obj.IsDate() {
			converted = obj.ToMyDate("It can't be converted to DateTime!")
		}
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
			return def
		}
		return converted.DateValue()
	case time.Time:
		converted := obj
		if !obj.IsDate() {
			converted = obj.ToMyDate("It can't be converted to DateTime!")
		}
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
			return def
		}
		return converted.DateValue().ToDateTime()
	}
	converted := obj
	if !obj.IsDate() {
		converted = obj.ToMyDate("It can't be converted to DateTime!")
	}
	if converted.IsError() {
		e.LastError = converted.ErrorMsg()
		return def
	}
	return converted.DateValue()
}

// tryTyped 通用「解析 + 求值 + 转换 + 提取」流程。
func (e *AlgorithmEngine) tryTyped(exp string, convert func(Operand) Operand, extract func(Operand) interface{}) (interface{}, bool) {
	fn := e.ParseWithoutError(exp)
	if fn == nil {
		return nil, false
	}
	obj := e.EvaluateSafe(fn)
	converted := convert(obj)
	if converted.IsError() || converted.IsNone() {
		if converted.IsError() {
			e.LastError = converted.ErrorMsg()
		}
		return nil, false
	}
	return extract(converted), true
}

// TryEvaluateMyDate 求值并转 MyDate。
func (e *AlgorithmEngine) TryEvaluateMyDate(exp string, def *MyDate) *MyDate {
	v, ok := e.tryTyped(exp,
		func(o Operand) Operand {
			if o.IsDate() {
				return o
			}
			return o.ToMyDate("It can't be converted to DateTime!")
		},
		func(o Operand) interface{} { return o.DateValue() })
	if !ok {
		return def
	}
	return v.(*MyDate)
}

// TryEvaluateText 求值并转字符串。
func (e *AlgorithmEngine) TryEvaluateText(exp string, def string) string {
	v, ok := e.tryTyped(exp,
		func(o Operand) Operand {
			if o.IsText() {
				return o
			}
			return o.ToText("It can't be converted to string!")
		},
		func(o Operand) interface{} { return o.TextValue() })
	if !ok {
		return def
	}
	return v.(string)
}

// TryEvaluateInt 求值并转 int。
func (e *AlgorithmEngine) TryEvaluateInt(exp string, def int) int {
	v, ok := e.tryTyped(exp,
		func(o Operand) Operand {
			if o.IsNumber() {
				return o
			}
			return o.ToNumber("It can't be converted to number!")
		},
		func(o Operand) interface{} { return o.IntValue() })
	if !ok {
		return def
	}
	return v.(int)
}

// TryEvaluateBool 求值并转 bool。
func (e *AlgorithmEngine) TryEvaluateBool(exp string, def bool) bool {
	v, ok := e.tryTyped(exp,
		func(o Operand) Operand {
			if o.IsBoolean() {
				return o
			}
			return o.ToBoolean("It can't be converted to bool!")
		},
		func(o Operand) interface{} { return o.BooleanValue() })
	if !ok {
		return def
	}
	return v.(bool)
}

// TryEvaluateNumber 求值并转 Decimal。
func (e *AlgorithmEngine) TryEvaluateNumber(exp string, def Decimal) Decimal {
	v, ok := e.tryTyped(exp,
		func(o Operand) Operand {
			if o.IsNumber() {
				return o
			}
			return o.ToNumber("It can't be converted to number!")
		},
		func(o Operand) interface{} { return o.NumberValue() })
	if !ok {
		return def
	}
	return v.(Decimal)
}

// TryEvaluateDouble 求值并转 float64。
func (e *AlgorithmEngine) TryEvaluateDouble(exp string, def float64) float64 {
	v, ok := e.tryTyped(exp,
		func(o Operand) Operand {
			if o.IsNumber() {
				return o
			}
			return o.ToNumber("It can't be converted to number!")
		},
		func(o Operand) interface{} { return o.DoubleValue() })
	if !ok {
		return def
	}
	return v.(float64)
}

// TryEvaluateDateTime 求值并转 time.Time。
func (e *AlgorithmEngine) TryEvaluateDateTime(exp string, def time.Time) time.Time {
	v, ok := e.tryTyped(exp,
		func(o Operand) Operand {
			if o.IsDate() {
				return o
			}
			return o.ToMyDate("It can't be converted to DateTime!")
		},
		func(o Operand) interface{} { return o.DateValue().ToDateTime() })
	if !ok {
		return def
	}
	return v.(time.Time)
}

// NewNoneEngine 创建「全部参数为空」的引擎，用于推导参数类型。
func NewNoneEngine(engine *AlgorithmEngine) *AlgorithmEngine {
	none := &AlgorithmEngine{
		ExcelIndex:    engine.ExcelIndex,
		IgnoreCase:    engine.IgnoreCase,
		UseLocalTime:  engine.UseLocalTime,
		UseStrictMode: engine.UseStrictMode,
		DistanceUnit:  engine.DistanceUnit,
		AreaUnit:      engine.AreaUnit,
		VolumeUnit:    engine.VolumeUnit,
		MassUnit:      engine.MassUnit,
		LastError:     engine.LastError,
		parseCache:    map[string]Function{},
		noneMode:      true,
	}
	return none
}
