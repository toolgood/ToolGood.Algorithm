package algorithm

import (
	"errors"
	"strings"
	"sync"
	"time"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/operands"
)

// AlgorithmEngineEx AlgorithmEngine 扩展类，增加自定义参数缓存功能
// （对应 C# 的 AlgorithmEngineEx）。
//
// C# 通过继承并重写 GetParameter 实现；Go 用组合 + GetParameterFunc 注入实现，
// 不修改 internals.AlgorithmEngine 的字段。
type AlgorithmEngineEx struct {
	*AlgorithmEngine

	// UseTempDict 是否把参数查询结果保存到临时字典（对应 C# 的 UseTempDict）。
	UseTempDict bool
	// GetParameterExFunc 自定义参数取值函数。
	//
	// 对应 C# 中可重写的 virtual GetParameterEx；为 nil 时使用 GetParameterEx 的默认实现。
	GetParameterExFunc func(parameter string) Operand

	mu       sync.RWMutex
	tempdict map[string]Operand
}

// NewAlgorithmEngineEx 创建扩展引擎（对应 C# 的 AlgorithmEngineEx(bool ignoreCase)）。
func NewAlgorithmEngineEx(ignoreCase bool) *AlgorithmEngineEx {
	e := &AlgorithmEngineEx{
		AlgorithmEngine: NewAlgorithmEngine(ignoreCase),
		tempdict:        map[string]Operand{},
	}
	e.AlgorithmEngine.GetParameterFunc = e.getParameter
	return e
}

// GetParameter 取参数值：先查临时字典，未命中时调用 GetParameterEx。
func (e *AlgorithmEngineEx) GetParameter(parameter string) Operand {
	key := e.mapKey(parameter)
	e.mu.RLock()
	operand, ok := e.tempdict[key]
	e.mu.RUnlock()
	if ok {
		return operand
	}

	result := e.GetParameterEx(parameter)
	if e.UseTempDict {
		e.mu.Lock()
		if _, exists := e.tempdict[key]; !exists { // 对应 ConcurrentDictionary.TryAdd
			e.tempdict[key] = result
		}
		e.mu.Unlock()
	}
	return result
}

// GetParameterEx 获取参数扩展方法：默认返回「参数缺失」错误。
func (e *AlgorithmEngineEx) GetParameterEx(parameter string) Operand {
	if e.GetParameterExFunc != nil {
		return e.GetParameterExFunc(parameter)
	}
	return Errorf("Parameter [{0}] is missing.", parameter)
}

// ClearParameters 清理参数。
func (e *AlgorithmEngineEx) ClearParameters() {
	e.mu.Lock()
	e.tempdict = map[string]Operand{}
	e.mu.Unlock()
}

// AddParameter 添加自定义参数。
//
// value 支持 Operand / bool / 各种整型 / float / Decimal / string / MyDate / time.Time /
// []Operand / []string / []float64 / []int / []bool 等（见 operands.Create）。
func (e *AlgorithmEngineEx) AddParameter(key string, value interface{}) {
	if t, ok := value.(time.Time); ok {
		// operands.Create 不支持 time.Time，对应 C# 的 AddParameter(string, DateTime) 重载。
		value = operands.NewMyDateFromTime(t)
	}
	e.setParameter(key, Create(value))
}

// AddParameterFromJson 从 JSON 对象字符串添加参数（对应 C# 的 AddParameterFromJson）。
func (e *AlgorithmEngineEx) AddParameterFromJson(json string) error {
	if strings.HasPrefix(json, "{") && strings.HasSuffix(json, "}") {
		jo, err := JsonMapperToObject(json)
		if err != nil {
			return err
		}
		if jo.IsObject() {
			for _, key := range jo.Keys() {
				e.setParameter(key, jsonToOperand(jo.GetChild(key)))
			}
			return nil
		}
	}
	return errors.New("Parameter is not json string.")
}

func (e *AlgorithmEngineEx) setParameter(key string, operand Operand) {
	e.mu.Lock()
	e.tempdict[e.mapKey(key)] = operand
	e.mu.Unlock()
}

// mapKey IgnoreCase 为 true 时把参数名转为大写，
// 对应 C# 的 ConcurrentDictionary(StringComparer.OrdinalIgnoreCase)。
func (e *AlgorithmEngineEx) mapKey(key string) string {
	if e.IgnoreCase {
		return strings.ToUpper(key)
	}
	return key
}

// getParameter 供 internals.AlgorithmEngine.GetParameterFunc 使用。
func (e *AlgorithmEngineEx) getParameter(parameter string) Operand {
	return e.GetParameter(parameter)
}

// jsonToOperand 把 JSON 节点转成操作数，分支顺序与 C# 的 switch 一致。
func jsonToOperand(v *JsonData) Operand {
	switch {
	case v.IsString():
		return CreateString(v.StringValue())
	case v.IsBoolean():
		return CreateBool(v.BooleanValue())
	case v.IsDouble():
		return CreateDecimal(v.NumberValue())
	case v.IsObject(), v.IsArray():
		return CreateJsonData(v)
	case v.IsNull():
		return Null
	}
	return CreateJsonData(v)
}
