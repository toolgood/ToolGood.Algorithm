package operands

import (
	"fmt"
	"strconv"
	"strings"
)

// Operand 表达式引擎中的动态类型值。
//
// 与 C# 的 Operand 抽象类、Java 的 Operand 抽象类一一对应。
// 取值方法（NumberValue 等）在类型不匹配时的行为与 C# 抛 NotImplementedException 一致：
// 会 panic，调用方应先用 Is*() 判断。引擎在公开入口处统一 recover，转成错误结果。
type Operand interface {
	IsErrorOrNone() bool
	IsNone() bool
	IsNull() bool
	IsNumber() bool
	IsText() bool
	IsBoolean() bool
	IsArray() bool
	IsDate() bool
	IsJson() bool
	IsArrayJson() bool
	IsError() bool
	ErrorMsg() string
	Type() OperandType

	NumberValue() Decimal
	DoubleValue() float64
	IntValue() int
	LongValue() int64
	TextValue() string
	BooleanValue() bool
	ArrayValue() []Operand
	JsonValue() *JsonData
	DateValue() *MyDate

	ToNumber(msg string) Operand
	ToBoolean(msg string) Operand
	ToText(msg string) Operand
	ToMyDate(msg string) Operand
	ToArray(msg string) Operand
	ToJson(msg string) Operand

	String() string
}

// ---------------------------------------------------------------- 默认实现

type operandBase struct{}

func (operandBase) IsErrorOrNone() bool { return false }
func (operandBase) IsNone() bool        { return false }
func (operandBase) IsNull() bool        { return false }
func (operandBase) IsNumber() bool      { return false }
func (operandBase) IsText() bool        { return false }
func (operandBase) IsBoolean() bool     { return false }
func (operandBase) IsArray() bool       { return false }
func (operandBase) IsDate() bool        { return false }
func (operandBase) IsJson() bool        { return false }
func (operandBase) IsArrayJson() bool   { return false }
func (operandBase) IsError() bool       { return false }
func (operandBase) ErrorMsg() string    { return "" }

func (operandBase) NumberValue() Decimal      { panic("operand has no number value") }
func (operandBase) DoubleValue() float64      { panic("operand has no double value") }
func (operandBase) IntValue() int             { panic("operand has no int value") }
func (operandBase) LongValue() int64          { panic("operand has no long value") }
func (operandBase) TextValue() string         { panic("operand has no text value") }
func (operandBase) BooleanValue() bool        { panic("operand has no boolean value") }
func (operandBase) ArrayValue() []Operand     { panic("operand has no array value") }
func (operandBase) JsonValue() *JsonData      { panic("operand has no json value") }
func (operandBase) DateValue() *MyDate        { panic("operand has no date value") }
func (b operandBase) ToNumber(msg string) Operand { return conversionError(msg, "Convert to number error!") }
func (b operandBase) ToBoolean(msg string) Operand {
	return conversionError(msg, "Convert to bool error!")
}
func (b operandBase) ToText(msg string) Operand { return conversionError(msg, "Convert to string error!") }
func (b operandBase) ToMyDate(msg string) Operand {
	return conversionError(msg, "Convert to date error!")
}
func (b operandBase) ToArray(msg string) Operand { return conversionError(msg, "Convert to array error!") }
func (b operandBase) ToJson(msg string) Operand  { return conversionError(msg, "Convert to json error!") }

func conversionError(msg, defaultMsg string) Operand {
	if msg != "" {
		return Error(msg)
	}
	return Error(defaultMsg)
}

// ---------------------------------------------------------------- 全局常量

var (
	// True 布尔真。
	True Operand = &OperandBoolean{value: true}
	// False 布尔假。
	False Operand = &OperandBoolean{value: false}
	// Null 空值。
	Null Operand = &OperandNull{}
	// None 未指定值。
	None Operand = &OperandNone{}
	// Zero 数字 0。
	Zero Operand
	// One 数字 1。
	One Operand
	// Version 版本号操作数。
	Version Operand = &OperandString{value: "ToolGood.Algorithm 6.3"}
)

const (
	intCacheOffset = 1000
	intCacheSize   = 2001
)

var intCache = func() []Operand {
	cache := make([]Operand, intCacheSize)
	for i := 0; i < intCacheSize; i++ {
		cache[i] = &OperandInt{value: i - intCacheOffset}
	}
	return cache
}()

func init() {
	Zero = intCache[intCacheOffset]
	One = intCache[1+intCacheOffset]
}

// ---------------------------------------------------------------- 工厂方法

// Create 由 Go 原生值创建 Operand。
func Create(v interface{}) Operand {
	switch t := v.(type) {
	case nil:
		return Null
	case Operand:
		return t
	case bool:
		return CreateBool(t)
	case int:
		return CreateInt(t)
	case int8:
		return CreateInt(int(t))
	case int16:
		return CreateInt(int(t))
	case int32:
		return CreateInt(int(t))
	case int64:
		return CreateInt64(t)
	case uint:
		return CreateInt64(int64(t))
	case uint8:
		return CreateInt(int(t))
	case uint16:
		return CreateInt(int(t))
	case uint32:
		return CreateInt64(int64(t))
	case uint64:
		return CreateUint64(t)
	case float32:
		return CreateFloat(float64(t))
	case float64:
		return CreateFloat(t)
	case Decimal:
		return CreateDecimal(t)
	case string:
		return CreateString(t)
	case *MyDate:
		return CreateMyDate(t)
	case MyDate:
		return CreateMyDate(&t)
	case *JsonData:
		return CreateJsonData(t)
	case []Operand:
		return CreateArray(t)
	case []string:
		arr := make([]Operand, len(t))
		for i, s := range t {
			arr[i] = CreateString(s)
		}
		return CreateArray(arr)
	case []float64:
		arr := make([]Operand, len(t))
		for i, f := range t {
			arr[i] = CreateFloat(f)
		}
		return CreateArray(arr)
	case []int:
		arr := make([]Operand, len(t))
		for i, n := range t {
			arr[i] = CreateInt(n)
		}
		return CreateArray(arr)
	case []bool:
		arr := make([]Operand, len(t))
		for i, b := range t {
			arr[i] = CreateBool(b)
		}
		return CreateArray(arr)
	}
	return Error(fmt.Sprintf("cannot convert %T to Operand", v))
}

// CreateBool 创建布尔操作数。
func CreateBool(v bool) Operand {
	if v {
		return True
	}
	return False
}

// CreateInt 创建整数操作数（-1000~1000 走缓存）。
func CreateInt(v int) Operand {
	if v >= -intCacheOffset && v <= intCacheOffset {
		return intCache[v+intCacheOffset]
	}
	return &OperandInt{value: v}
}

// CreateInt64 创建 64 位整数操作数。
func CreateInt64(v int64) Operand {
	if v >= -intCacheOffset && v <= intCacheOffset {
		return intCache[int(v)+intCacheOffset]
	}
	return &OperandDecimal{value: DecFromInt(v)}
}

// CreateUint64 创建无符号整数操作数。
func CreateUint64(v uint64) Operand {
	if v <= intCacheOffset {
		return intCache[int(v)+intCacheOffset]
	}
	return &OperandDecimal{value: DecFromUint(v)}
}

// CreateFloat 创建浮点操作数。
func CreateFloat(v float64) Operand {
	if IsNaNOrInf(v) {
		return Error("Convert to number error!")
	}
	if v == float64(int64(v)) && v >= -intCacheOffset && v <= intCacheOffset {
		return intCache[int(v)+intCacheOffset]
	}
	return &OperandDecimal{value: DecFromFloat(v)}
}

// CreateDecimal 创建十进制操作数。
func CreateDecimal(v Decimal) Operand {
	if v.IsInteger() && v.GreaterOrEqual(DecimalMax.Neg()) && v.LessOrEqual(DecimalMax) {
		i := v.Int64()
		if i >= -intCacheOffset && i <= intCacheOffset {
			return intCache[int(i)+intCacheOffset]
		}
	}
	return &OperandDecimal{value: v}
}

// CreateString 创建字符串操作数。
func CreateString(v string) Operand { return &OperandString{value: v} }

// CreateMyDate 创建日期操作数。
func CreateMyDate(v *MyDate) Operand { return &OperandMyDate{value: v} }

// CreateDate 由时间组件创建日期操作数。
func CreateDate(year, month, day, hour, minute, second int) Operand {
	return CreateMyDate(NewMyDate(year, month, day, hour, minute, second))
}

// CreateJsonData 创建 JSON 操作数。
func CreateJsonData(v *JsonData) Operand { return &OperandJson{value: v} }

// CreateArray 创建数组操作数。
func CreateArray(v []Operand) Operand { return &OperandArray{value: v} }

// CreateJson 解析 JSON 文本为操作数。
func CreateJson(txt string) Operand {
	if (strings.HasPrefix(txt, "{") && strings.HasSuffix(txt, "}")) ||
		(strings.HasPrefix(txt, "[") && strings.HasSuffix(txt, "]")) {
		json, err := JsonMapperToObject(txt)
		if err == nil {
			return CreateJsonData(json)
		}
	}
	return Error("Convert to json error!")
}

// Error 创建错误操作数。
func Error(msg string) Operand { return &OperandError{msg: msg} }

// Errorf 创建带格式参数的错误操作数（{0} {1} 形式）。
func Errorf(msg string, args ...interface{}) Operand {
	return &OperandError{msg: msg, args: args}
}

// ---------------------------------------------------------------- 整数

// OperandInt 整数操作数。
type OperandInt struct {
	operandBase
	value int
}

// Type 返回操作数类型。
func (o *OperandInt) Type() OperandType { return OperandTypeNumber }
// IsNumber 是否数字。
func (o *OperandInt) IsNumber() bool    { return true }
// IntValue 返回 int 值。
func (o *OperandInt) IntValue() int     { return o.value }
// LongValue 返回 int64 值。
func (o *OperandInt) LongValue() int64  { return int64(o.value) }
// DoubleValue 返回 float64 值。
func (o *OperandInt) DoubleValue() float64 { return float64(o.value) }
// NumberValue 返回 Decimal 值。
func (o *OperandInt) NumberValue() Decimal { return DecFromInt(int64(o.value)) }
// ToNumber 返回自身。
func (o *OperandInt) ToNumber(msg string) Operand { return o }
// ToBoolean 0/1 转布尔。
func (o *OperandInt) ToBoolean(msg string) Operand {
	switch o.value {
	case 0:
		return False
	case 1:
		return True
	}
	if msg == "" {
		msg = "Convert number to bool error!"
	}
	return Error(msg)
}
// ToText 转文本。
func (o *OperandInt) ToText(msg string) Operand {
	return &OperandString{value: strconv.Itoa(o.value)}
}
// ToMyDate 按 Excel 序列值转日期。
func (o *OperandInt) ToMyDate(msg string) Operand {
	return &OperandMyDate{value: NewMyDateFromNumber(DecFromInt(int64(o.value)))}
}
func (o *OperandInt) String() string { return strconv.Itoa(o.value) }

// ---------------------------------------------------------------- 十进制

// OperandDecimal 十进制操作数。
type OperandDecimal struct {
	operandBase
	value Decimal
}

// Type 返回操作数类型。
func (o *OperandDecimal) Type() OperandType { return OperandTypeNumber }
// IsNumber 是否数字。
func (o *OperandDecimal) IsNumber() bool           { return true }
// IntValue 返回 int 值。
func (o *OperandDecimal) IntValue() int            { return int(o.value.Trunc().Int64()) }
// LongValue 返回 int64 值。
func (o *OperandDecimal) LongValue() int64         { return o.value.Trunc().Int64() }
// DoubleValue 返回 float64 值。
func (o *OperandDecimal) DoubleValue() float64     { return o.value.Float64() }
// NumberValue 返回 Decimal 值。
func (o *OperandDecimal) NumberValue() Decimal     { return o.value }
// ToNumber 返回自身。
func (o *OperandDecimal) ToNumber(msg string) Operand { return o }
// ToBoolean 0/1 转布尔。
func (o *OperandDecimal) ToBoolean(msg string) Operand {
	if o.value.IsZero() {
		return False
	}
	if o.value.Equal(DecimalOne) {
		return True
	}
	if msg == "" {
		msg = "Convert number to bool error!"
	}
	return Error(msg)
}
// ToText 转文本。
func (o *OperandDecimal) ToText(msg string) Operand {
	return &OperandString{value: o.value.PlainStrip()}
}
// ToMyDate 按 Excel 序列值转日期。
func (o *OperandDecimal) ToMyDate(msg string) Operand {
	return &OperandMyDate{value: NewMyDateFromNumber(o.value)}
}
func (o *OperandDecimal) String() string { return o.value.String() }

// ---------------------------------------------------------------- 字符串

// OperandString 字符串操作数。
type OperandString struct {
	operandBase
	value string
}

// Type 返回操作数类型。
func (o *OperandString) Type() OperandType { return OperandTypeText }
// IsText 是否文本。
func (o *OperandString) IsText() bool      { return true }
// TextValue 返回字符串值。
func (o *OperandString) TextValue() string { return o.value }
// ToNumber 解析数字。
func (o *OperandString) ToNumber(msg string) Operand {
	text := strings.TrimSpace(o.value)
	if i, err := strconv.Atoi(text); err == nil {
		return CreateInt(i)
	}
	if d, ok := TryParseDecimal(text); ok {
		return CreateDecimal(d)
	}
	if msg == "" {
		return Error("Convert string to number error!")
	}
	return Error(msg)
}
// ToText 返回自身。
func (o *OperandString) ToText(msg string) Operand { return o }
// ToBoolean 解析布尔。
func (o *OperandString) ToBoolean(msg string) Operand {
	text := strings.TrimSpace(o.value)
	switch text {
	case "true", "TRUE", "True":
		return True
	case "false", "FALSE", "False":
		return False
	case "1", "是", "有":
		return True
	case "0", "否", "不是", "无", "没有":
		return False
	}
	if msg == "" {
		return Error("Convert string to bool error!")
	}
	return Error(msg)
}
// ToMyDate 解析日期。
func (o *OperandString) ToMyDate(msg string) Operand {
	if d := ParseMyDate(o.value); d != nil {
		return CreateMyDate(d)
	}
	if msg == "" {
		return Error("Convert string to date error!")
	}
	return Error(msg)
}
// ToArray 字符串不能转数组。
func (o *OperandString) ToArray(msg string) Operand {
	if msg == "" {
		msg = "Convert string to array error!"
	}
	return Error(msg)
}
// ToJson 解析 JSON 文本。
func (o *OperandString) ToJson(msg string) Operand {
	text := strings.TrimSpace(o.value)
	if (strings.HasPrefix(text, "{") && strings.HasSuffix(text, "}")) ||
		(strings.HasPrefix(text, "[") && strings.HasSuffix(text, "]")) {
		if json, err := JsonMapperToObject(text); err == nil {
			return CreateJsonData(json)
		}
	}
	if msg == "" {
		return Error("Convert string to json error!")
	}
	return Error(msg)
}
func (o *OperandString) String() string { return quoteText(o.value) }

func quoteText(value string) string {
	if value == "" {
		return `""`
	}
	needsEscape := false
	for _, ch := range value {
		if ch == '"' || ch == '\\' || ch < 32 {
			needsEscape = true
			break
		}
	}
	if !needsEscape {
		return `"` + value + `"`
	}
	var sb strings.Builder
	sb.WriteByte('"')
	for _, ch := range value {
		switch ch {
		case '"':
			sb.WriteString(`\"`)
		case '\\':
			sb.WriteString(`\\`)
		case '\n':
			sb.WriteString(`\n`)
		case '\r':
			sb.WriteString(`\r`)
		case '\t':
			sb.WriteString(`\t`)
		case '\b':
			sb.WriteString(`\b`)
		case '\f':
			sb.WriteString(`\f`)
		default:
			if ch < 32 {
				sb.WriteString(fmt.Sprintf(`\u%04x`, ch))
			} else {
				sb.WriteRune(ch)
			}
		}
	}
	sb.WriteByte('"')
	return sb.String()
}

// ---------------------------------------------------------------- 布尔

// OperandBoolean 布尔操作数。
type OperandBoolean struct {
	operandBase
	value bool
}

// Type 返回操作数类型。
func (o *OperandBoolean) Type() OperandType { return OperandTypeBoolean }
// IsBoolean 是否布尔。
func (o *OperandBoolean) IsBoolean() bool   { return true }
// BooleanValue 返回布尔值。
func (o *OperandBoolean) BooleanValue() bool { return o.value }
// ToNumber 转为 1/0。
func (o *OperandBoolean) ToNumber(msg string) Operand {
	if o.value {
		return One
	}
	return Zero
}
// ToBoolean 返回自身。
func (o *OperandBoolean) ToBoolean(msg string) Operand { return o }
// ToText 转文本。
func (o *OperandBoolean) ToText(msg string) Operand {
	if o.value {
		return &OperandString{value: "TRUE"}
	}
	return &OperandString{value: "FALSE"}
}
// ToMyDate 布尔不能转日期。
func (o *OperandBoolean) ToMyDate(msg string) Operand {
	if msg == "" {
		msg = "Convert bool to date error!"
	}
	return Error(msg)
}
// ToArray 布尔不能转数组。
func (o *OperandBoolean) ToArray(msg string) Operand {
	if msg == "" {
		msg = "Convert bool to array error!"
	}
	return Error(msg)
}
// ToJson 布尔不能转 JSON。
func (o *OperandBoolean) ToJson(msg string) Operand {
	if msg == "" {
		msg = "Convert bool to json error!"
	}
	return Error(msg)
}
func (o *OperandBoolean) String() string {
	if o.value {
		return "true"
	}
	return "false"
}

// ---------------------------------------------------------------- 数组

// OperandArray 数组操作数。
type OperandArray struct {
	operandBase
	value []Operand
}

// Type 返回操作数类型。
func (o *OperandArray) Type() OperandType { return OperandTypeArray }
// IsArray 是否数组。
func (o *OperandArray) IsArray() bool     { return true }
// ArrayValue 返回数组值。
func (o *OperandArray) ArrayValue() []Operand { return o.value }
// ToNumber 数组不能转数字。
func (o *OperandArray) ToNumber(msg string) Operand {
	if msg == "" {
		msg = "Convert array to number error!"
	}
	return Error(msg)
}
// ToBoolean 数组不能转布尔。
func (o *OperandArray) ToBoolean(msg string) Operand {
	if msg == "" {
		msg = "Convert array to bool error!"
	}
	return Error(msg)
}
// ToText 转文本。
func (o *OperandArray) ToText(msg string) Operand {
	return &OperandString{value: o.String()}
}
// ToArray 返回自身。
func (o *OperandArray) ToArray(msg string) Operand { return o }
// ToJson 转 JSON。
func (o *OperandArray) ToJson(msg string) Operand {
	if json, err := JsonMapperToObject(o.String()); err == nil {
		return CreateJsonData(json)
	}
	if msg == "" {
		msg = "Convert array to json error!"
	}
	return Error(msg)
}
func (o *OperandArray) String() string {
	var sb strings.Builder
	sb.WriteByte('[')
	for i, v := range o.value {
		if i > 0 {
			sb.WriteByte(',')
		}
		sb.WriteString(v.String())
	}
	sb.WriteByte(']')
	return sb.String()
}

// ---------------------------------------------------------------- 日期

// OperandMyDate 日期操作数。
type OperandMyDate struct {
	operandBase
	value *MyDate
}

// Type 返回操作数类型。
func (o *OperandMyDate) Type() OperandType { return OperandTypeDate }
// IsDate 是否日期。
func (o *OperandMyDate) IsDate() bool      { return true }
// DateValue 返回日期值。
func (o *OperandMyDate) DateValue() *MyDate { return o.value }
// ToNumber 转 Excel 序列值。
func (o *OperandMyDate) ToNumber(msg string) Operand {
	return CreateDecimal(o.value.ToNumber())
}
// ToBoolean 非零为真。
func (o *OperandMyDate) ToBoolean(msg string) Operand {
	if !o.value.ToNumber().IsZero() {
		return True
	}
	return False
}
// ToText 转文本。
func (o *OperandMyDate) ToText(msg string) Operand {
	return &OperandString{value: o.value.String()}
}
// ToMyDate 返回自身。
func (o *OperandMyDate) ToMyDate(msg string) Operand { return o }
// ToArray 日期不能转数组。
func (o *OperandMyDate) ToArray(msg string) Operand {
	if msg == "" {
		msg = "Convert date to array error!"
	}
	return Error(msg)
}
// ToJson 日期不能转 JSON。
func (o *OperandMyDate) ToJson(msg string) Operand {
	if msg == "" {
		msg = "Convert date to json error!"
	}
	return Error(msg)
}
func (o *OperandMyDate) String() string { return `"` + o.value.String() + `"` }

// ---------------------------------------------------------------- 错误

// OperandError 错误操作数。
type OperandError struct {
	operandBase
	msg  string
	args []interface{}
}

// Type 返回操作数类型。
func (o *OperandError) Type() OperandType { return OperandTypeError }
// IsError 是否错误。
func (o *OperandError) IsError() bool     { return true }
// IsErrorOrNone 是否错误或未指定。
func (o *OperandError) IsErrorOrNone() bool { return true }
// ErrorMsg 返回错误信息。
func (o *OperandError) ErrorMsg() string {
	if len(o.args) == 0 {
		return o.msg
	}
	return formatMessage(o.msg, o.args)
}
// ToNumber 返回自身。
func (o *OperandError) ToNumber(msg string) Operand { return o }
// ToBoolean 返回自身。
func (o *OperandError) ToBoolean(msg string) Operand { return o }
// ToText 返回自身。
func (o *OperandError) ToText(msg string) Operand { return o }
// ToMyDate 返回自身。
func (o *OperandError) ToMyDate(msg string) Operand { return o }
// ToArray 返回自身。
func (o *OperandError) ToArray(msg string) Operand { return o }
// ToJson 返回自身。
func (o *OperandError) ToJson(msg string) Operand { return o }
func (o *OperandError) String() string            { return "ERROR: " + o.ErrorMsg() }

func formatMessage(message string, args []interface{}) string {
	out := message
	for i, v := range args {
		out = strings.ReplaceAll(out, "{"+strconv.Itoa(i)+"}", fmt.Sprint(v))
	}
	return out
}

// ---------------------------------------------------------------- JSON

// OperandJson JSON 操作数。
type OperandJson struct {
	operandBase
	value *JsonData
}

// Type 返回操作数类型。
func (o *OperandJson) Type() OperandType { return OperandTypeJson }
// IsJson 是否 JSON。
func (o *OperandJson) IsJson() bool      { return true }
// JsonValue 返回 JSON 值。
func (o *OperandJson) JsonValue() *JsonData { return o.value }
// ToJson 返回自身。
func (o *OperandJson) ToJson(msg string) Operand { return o }
// ToText 转文本。
func (o *OperandJson) ToText(msg string) Operand {
	return &OperandString{value: o.value.String()}
}
// ToArray JSON 数组转 Operand 数组。
func (o *OperandJson) ToArray(msg string) Operand {
	if o.value.IsArray() {
		list := o.value.InstArray()
		result := make([]Operand, len(list))
		for i, v := range list {
			result[i] = jsonToOperand(v)
		}
		return CreateArray(result)
	}
	if msg == "" {
		msg = "Convert json to array error!"
	}
	return Error(msg)
}
// ToBoolean JSON 不能转布尔。
func (o *OperandJson) ToBoolean(msg string) Operand {
	if msg == "" {
		msg = "Convert json to bool error!"
	}
	return Error(msg)
}
// ToNumber JSON 不能转数字。
func (o *OperandJson) ToNumber(msg string) Operand {
	if msg == "" {
		msg = "Convert json to number error!"
	}
	return Error(msg)
}
// ToMyDate JSON 不能转日期。
func (o *OperandJson) ToMyDate(msg string) Operand {
	if msg == "" {
		msg = "Convert json to date error!"
	}
	return Error(msg)
}
func (o *OperandJson) String() string { return o.value.String() }

// ---------------------------------------------------------------- Null / None

// OperandNull 空值操作数。
type OperandNull struct{ operandBase }

// Type 返回操作数类型。
func (o *OperandNull) Type() OperandType { return OperandTypeNull }
// IsNull 是否空值。
func (o *OperandNull) IsNull() bool      { return true }
func (o *OperandNull) String() string    { return "null" }

// OperandNone 未指定值操作数。
type OperandNone struct{ operandBase }

// Type 返回操作数类型。
func (o *OperandNone) Type() OperandType { return OperandTypeNone }
// IsNone 是否未指定。
func (o *OperandNone) IsNone() bool      { return true }
// IsErrorOrNone 错误或未指定。
func (o *OperandNone) IsErrorOrNone() bool { return true }
func (o *OperandNone) String() string      { return "NONE" }

// ---------------------------------------------------------------- KeyValue

// KeyValue 键值对。
type KeyValue struct {
	Key   string
	Value Operand
}

// NewKeyValue 创建键值对。
func NewKeyValue(key string, value Operand) *KeyValue {
	if value == nil {
		value = Null
	}
	return &KeyValue{Key: key, Value: value}
}

// OperandKeyValue 单个键值对操作数。
type OperandKeyValue struct {
	operandBase
	value *KeyValue
}

// Type 返回操作数类型。
func (o *OperandKeyValue) Type() OperandType { return OperandTypeArrayJson }
// IsArrayJson 是否键值集合。
func (o *OperandKeyValue) IsArrayJson() bool { return true }
// Value 返回键值对。
func (o *OperandKeyValue) Value() *KeyValue  { return o.value }

// OperandKeyValueList 键值对列表操作数。
type OperandKeyValueList struct {
	operandBase
	list []*KeyValue
}

// NewOperandKeyValueList 创建键值对列表。
func NewOperandKeyValueList() *OperandKeyValueList {
	return &OperandKeyValueList{}
}

// Type 返回操作数类型。
func (o *OperandKeyValueList) Type() OperandType { return OperandTypeArrayJson }
// IsArrayJson 是否键值集合。
func (o *OperandKeyValueList) IsArrayJson() bool { return true }
// ArrayValue 返回值的数组。
func (o *OperandKeyValueList) ArrayValue() []Operand {
	result := make([]Operand, len(o.list))
	for i, kv := range o.list {
		result[i] = kv.Value
	}
	return result
}
// ToText 转文本。
func (o *OperandKeyValueList) ToText(msg string) Operand {
	return &OperandString{value: o.String()}
}
// ToArray 转数组。
func (o *OperandKeyValueList) ToArray(msg string) Operand {
	return CreateArray(o.ArrayValue())
}
// ToJson 转 JSON。
func (o *OperandKeyValueList) ToJson(msg string) Operand {
	if json, err := JsonMapperToObject(o.String()); err == nil {
		return CreateJsonData(json)
	}
	if msg == "" {
		msg = "Convert to json error!"
	}
	return Error(msg)
}
// AddValue 追加键值对。
func (o *OperandKeyValueList) AddValue(kv *KeyValue) { o.list = append(o.list, kv) }
// GetValue 按 key 查找。
func (o *OperandKeyValueList) GetValue(key string) Operand {
	for _, kv := range o.list {
		if kv.Key == key {
			return kv.Value
		}
	}
	return nil
}
// ContainsKey 是否包含 key。
func (o *OperandKeyValueList) ContainsKey(value Operand) bool {
	text := value.TextValue()
	for _, kv := range o.list {
		if kv.Key == text {
			return true
		}
	}
	return false
}
// ContainsValue 是否包含值。
func (o *OperandKeyValueList) ContainsValue(value Operand) bool {
	text := value.TextValue()
	for _, kv := range o.list {
		op := kv.Value
		switch {
		case op.IsText():
			if op.TextValue() == text {
				return true
			}
		case op.IsNumber():
			if Plain(op.NumberValue()) == text {
				return true
			}
		case op.IsBoolean():
			if strings.ToLower(strconv.FormatBool(op.BooleanValue())) == strings.ToLower(text) {
				return true
			}
		}
	}
	return false
}
func (o *OperandKeyValueList) String() string {
	var sb strings.Builder
	sb.WriteByte('{')
	for i, kv := range o.list {
		if i > 0 {
			sb.WriteByte(',')
		}
		sb.WriteString(quoteText(kv.Key))
		sb.WriteByte(':')
		sb.WriteString(kv.Value.String())
	}
	sb.WriteByte('}')
	return sb.String()
}

func jsonToOperand(data *JsonData) Operand {
	switch {
	case data.IsString():
		return CreateString(data.StringValue())
	case data.IsBoolean():
		return CreateBool(data.BooleanValue())
	case data.IsDouble():
		return CreateDecimal(data.NumberValue())
	case data.IsNull():
		return Null
	}
	return CreateJsonData(data)
}
