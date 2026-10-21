package litjson

import (
	"errors"
	"fmt"
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"
)

// JsonType JSON 节点类型。
type JsonType int

const (
	// JsonTypeNone 未初始化。
	JsonTypeNone JsonType = iota
	// JsonTypeObject 对象。
	JsonTypeObject
	// JsonTypeArray 数组。
	JsonTypeArray
	// JsonTypeString 字符串。
	JsonTypeString
	// JsonTypeDouble 数字。
	JsonTypeDouble
	// JsonTypeBoolean 布尔。
	JsonTypeBoolean
	// JsonTypeNull 空。
	JsonTypeNull
)

// JsonException JSON 解析异常。
type JsonException struct{ Message string }

func (e *JsonException) Error() string { return e.Message }

// JsonData 轻量 JSON 节点，对应 LitJson 的 JsonData。
type JsonData struct {
	typ    JsonType
	value  interface{}
	keys   []string
	object map[string]*JsonData
}

// NewJsonData 由 Go 原生值创建 JsonData。
func NewJsonData(v interface{}) *JsonData {
	switch t := v.(type) {
	case nil:
		return &JsonData{typ: JsonTypeNull}
	case *JsonData:
		return t
	case bool:
		return &JsonData{typ: JsonTypeBoolean, value: t}
	case string:
		return &JsonData{typ: JsonTypeString, value: t}
	case int:
		return &JsonData{typ: JsonTypeDouble, value: DecFromInt(int64(t))}
	case int64:
		return &JsonData{typ: JsonTypeDouble, value: DecFromInt(t)}
	case float64:
		return &JsonData{typ: JsonTypeDouble, value: DecFromFloat(t)}
	case Decimal:
		return &JsonData{typ: JsonTypeDouble, value: t}
	case []*JsonData:
		return &JsonData{typ: JsonTypeArray, value: t}
	case []interface{}:
		list := make([]*JsonData, len(t))
		for i, item := range t {
			list[i] = NewJsonData(item)
		}
		return &JsonData{typ: JsonTypeArray, value: list}
	case map[string]interface{}:
		data := &JsonData{typ: JsonTypeObject, object: map[string]*JsonData{}}
		for k, item := range t {
			data.setKey(k, NewJsonData(item))
		}
		return data
	case map[string]*JsonData:
		data := &JsonData{typ: JsonTypeObject, object: map[string]*JsonData{}}
		for k, item := range t {
			data.setKey(k, item)
		}
		return data
	}
	return &JsonData{typ: JsonTypeNull}
}

// NewJsonObject 创建空对象。
func NewJsonObject() *JsonData {
	return &JsonData{typ: JsonTypeObject, object: map[string]*JsonData{}}
}

// NewJsonArray 创建空数组。
func NewJsonArray() *JsonData {
	return &JsonData{typ: JsonTypeArray, value: []*JsonData{}}
}

func (d *JsonData) setKey(key string, v *JsonData) {
	if d.object == nil {
		d.object = map[string]*JsonData{}
	}
	if _, ok := d.object[key]; !ok {
		d.keys = append(d.keys, key)
	}
	d.object[key] = v
}

// Set 设置对象成员（保持插入顺序）。
func (d *JsonData) Set(key string, v *JsonData) { d.setKey(key, v) }

// Add 追加数组元素。
func (d *JsonData) Add(v *JsonData) {
	if d.typ != JsonTypeArray {
		return
	}
	d.value = append(d.value.([]*JsonData), v)
}

// Type 返回 JSON 类型。
func (d *JsonData) Type() JsonType { return d.typ }

// IsNone 是否未初始化。
func (d *JsonData) IsNone() bool { return d.typ == JsonTypeNone }

// IsObject 是否对象。
func (d *JsonData) IsObject() bool { return d.typ == JsonTypeObject }

// IsArray 是否数组。
func (d *JsonData) IsArray() bool { return d.typ == JsonTypeArray }

// IsString 是否字符串。
func (d *JsonData) IsString() bool { return d.typ == JsonTypeString }

// IsDouble 是否数字。
func (d *JsonData) IsDouble() bool { return d.typ == JsonTypeDouble }

// IsBoolean 是否布尔。
func (d *JsonData) IsBoolean() bool { return d.typ == JsonTypeBoolean }

// IsNull 是否空。
func (d *JsonData) IsNull() bool { return d.typ == JsonTypeNull }

// InstArray 返回数组元素。
func (d *JsonData) InstArray() []*JsonData {
	if d.typ == JsonTypeArray {
		return d.value.([]*JsonData)
	}
	return nil
}

// InstObject 返回对象成员。
func (d *JsonData) InstObject() map[string]*JsonData {
	if d.typ == JsonTypeObject {
		return d.object
	}
	return map[string]*JsonData{}
}

// Keys 返回对象成员名（插入顺序）。
func (d *JsonData) Keys() []string { return d.keys }

// BooleanValue 返回布尔值。
func (d *JsonData) BooleanValue() bool {
	if b, ok := d.value.(bool); ok {
		return b
	}
	return false
}

// NumberValue 返回数字值。
func (d *JsonData) NumberValue() Decimal {
	if v, ok := d.value.(Decimal); ok {
		return v
	}
	return Decimal{}
}

// StringValue 返回字符串值。
func (d *JsonData) StringValue() string {
	if s, ok := d.value.(string); ok {
		return s
	}
	return ""
}

// Count 返回元素个数。
func (d *JsonData) Count() int {
	switch d.typ {
	case JsonTypeArray:
		return len(d.value.([]*JsonData))
	case JsonTypeObject:
		return len(d.object)
	}
	return 0
}

// GetChild 按键或下标取子节点。
func (d *JsonData) GetChild(key interface{}) *JsonData {
	switch k := key.(type) {
	case int:
		if d.typ == JsonTypeArray {
			list := d.value.([]*JsonData)
			if k >= 0 && k < len(list) {
				return list[k]
			}
		}
	case string:
		if d.typ == JsonTypeObject {
			return d.object[k]
		}
	}
	return nil
}

// ContainsKey 是否包含成员。
func (d *JsonData) ContainsKey(key string) bool {
	if d.typ != JsonTypeObject {
		return false
	}
	_, ok := d.object[key]
	return ok
}

func escapeJsonText(text string) string {
	var sb strings.Builder
	for _, ch := range text {
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
			if ch < 0x20 {
				sb.WriteString(fmt.Sprintf(`\u%04x`, ch))
			} else {
				sb.WriteRune(ch)
			}
		}
	}
	return sb.String()
}

// String 序列化为 JSON 文本。
func (d *JsonData) String() string {
	switch d.typ {
	case JsonTypeNull, JsonTypeNone:
		return "null"
	case JsonTypeBoolean:
		if d.BooleanValue() {
			return "true"
		}
		return "false"
	case JsonTypeString:
		return `"` + escapeJsonText(d.StringValue()) + `"`
	case JsonTypeDouble:
		return d.NumberValue().String()
	case JsonTypeArray:
		list := d.value.([]*JsonData)
		parts := make([]string, len(list))
		for i, v := range list {
			parts[i] = v.String()
		}
		return "[" + strings.Join(parts, ",") + "]"
	case JsonTypeObject:
		parts := make([]string, 0, len(d.keys))
		for _, k := range d.keys {
			parts = append(parts, `"`+escapeJsonText(k)+`":`+d.object[k].String())
		}
		return "{" + strings.Join(parts, ",") + "}"
	}
	return "null"
}

// ToJson 序列化为 JSON 文本。
func (d *JsonData) ToJson() string { return d.String() }

// ToInterface 转为 Go 原生值。
func (d *JsonData) ToInterface() interface{} {
	switch d.typ {
	case JsonTypeObject:
		m := make(map[string]interface{}, len(d.keys))
		for _, k := range d.keys {
			m[k] = d.object[k].ToInterface()
		}
		return m
	case JsonTypeArray:
		list := d.value.([]*JsonData)
		result := make([]interface{}, len(list))
		for i, v := range list {
			result[i] = v.ToInterface()
		}
		return result
	}
	return d.value
}

// JsonMapperToObject 解析 JSON 文本（比标准宽松：允许单引号、尾逗号）。
func JsonMapperToObject(text string) (*JsonData, error) {
	p := &jsonParser{text: text}
	v, err := p.parseValue()
	if err != nil {
		return nil, err
	}
	p.skipWS()
	if p.pos != len(p.text) {
		return nil, &JsonException{Message: fmt.Sprintf("additional text at position %d", p.pos)}
	}
	return v, nil
}

type jsonParser struct {
	text string
	pos  int
}

const jsonWhitespace = " \t\r\n\f"
const jsonNumberChars = "0123456789+-.eE"

func (p *jsonParser) errorf(format string, args ...interface{}) error {
	return &JsonException{Message: fmt.Sprintf(format, args...) + fmt.Sprintf(" at position %d", p.pos)}
}

func (p *jsonParser) skipWS() {
	for p.pos < len(p.text) && strings.IndexByte(jsonWhitespace, p.text[p.pos]) >= 0 {
		p.pos++
	}
}

func (p *jsonParser) peek() byte {
	if p.pos < len(p.text) {
		return p.text[p.pos]
	}
	return 0
}

func (p *jsonParser) parseValue() (*JsonData, error) {
	p.skipWS()
	ch := p.peek()
	switch {
	case ch == 0:
		return nil, p.errorf("unexpected end of json")
	case ch == '{':
		return p.parseObject()
	case ch == '[':
		return p.parseArray()
	case ch == '\'' || ch == '"':
		s, err := p.parseString()
		if err != nil {
			return nil, err
		}
		return &JsonData{typ: JsonTypeString, value: s}, nil
	case ch == '-' || (ch >= '0' && ch <= '9'):
		return p.parseNumber()
	case strings.HasPrefix(p.text[p.pos:], "true"):
		p.pos += 4
		return &JsonData{typ: JsonTypeBoolean, value: true}, nil
	case strings.HasPrefix(p.text[p.pos:], "false"):
		p.pos += 5
		return &JsonData{typ: JsonTypeBoolean, value: false}, nil
	case strings.HasPrefix(p.text[p.pos:], "null"):
		p.pos += 4
		return &JsonData{typ: JsonTypeNull}, nil
	}
	return nil, p.errorf("unexpected character '%c'", ch)
}

func (p *jsonParser) parseObject() (*JsonData, error) {
	p.pos++
	data := NewJsonObject()
	for {
		p.skipWS()
		if p.peek() == '}' {
			p.pos++
			return data, nil
		}
		var key string
		if c := p.peek(); c == '\'' || c == '"' {
			s, err := p.parseString()
			if err != nil {
				return nil, err
			}
			key = s
		} else {
			start := p.pos
			for p.pos < len(p.text) {
				c := p.text[p.pos]
				if c == ':' || c == ',' || c == '}' || strings.IndexByte(jsonWhitespace, c) >= 0 {
					break
				}
				p.pos++
			}
			key = p.text[start:p.pos]
		}
		p.skipWS()
		if p.peek() != ':' {
			return nil, p.errorf("expected ':'")
		}
		p.pos++
		v, err := p.parseValue()
		if err != nil {
			return nil, err
		}
		data.setKey(key, v)
		p.skipWS()
		if p.peek() == ',' {
			p.pos++
			continue
		}
		if p.peek() == '}' {
			p.pos++
			return data, nil
		}
		return nil, p.errorf("expected ',' or '}'")
	}
}

func (p *jsonParser) parseArray() (*JsonData, error) {
	p.pos++
	list := []*JsonData{}
	for {
		p.skipWS()
		if p.peek() == ']' {
			p.pos++
			return &JsonData{typ: JsonTypeArray, value: list}, nil
		}
		v, err := p.parseValue()
		if err != nil {
			return nil, err
		}
		list = append(list, v)
		p.skipWS()
		if p.peek() == ',' {
			p.pos++
			continue
		}
		if p.peek() == ']' {
			p.pos++
			return &JsonData{typ: JsonTypeArray, value: list}, nil
		}
		return nil, p.errorf("expected ',' or ']'")
	}
}

func (p *jsonParser) parseString() (string, error) {
	quote := p.text[p.pos]
	p.pos++
	var sb strings.Builder
	for p.pos < len(p.text) {
		ch := p.text[p.pos]
		if ch == '\\' {
			p.pos++
			if p.pos >= len(p.text) {
				return "", p.errorf("unterminated escape")
			}
			esc := p.text[p.pos]
			switch esc {
			case 'n':
				sb.WriteByte('\n')
			case 'r':
				sb.WriteByte('\r')
			case 't':
				sb.WriteByte('\t')
			case 'b':
				sb.WriteByte('\b')
			case 'f':
				sb.WriteByte('\f')
			case 'u':
				if p.pos+5 > len(p.text) {
					return "", p.errorf("invalid unicode escape")
				}
				var code int
				if _, err := fmt.Sscanf(p.text[p.pos+1:p.pos+5], "%04x", &code); err != nil {
					return "", p.errorf("invalid unicode escape")
				}
				sb.WriteRune(rune(code))
				p.pos += 5
				continue
			default:
				sb.WriteByte(esc)
			}
			p.pos++
			continue
		}
		if ch == quote {
			p.pos++
			return sb.String(), nil
		}
		sb.WriteByte(ch)
		p.pos++
	}
	return "", p.errorf("unterminated string")
}

func (p *jsonParser) parseNumber() (*JsonData, error) {
	start := p.pos
	for p.pos < len(p.text) && strings.IndexByte(jsonNumberChars, p.text[p.pos]) >= 0 {
		p.pos++
	}
	text := p.text[start:p.pos]
	d, err := DecFromString(text)
	if err != nil {
		return nil, p.errorf("invalid number '%s'", text)
	}
	if d.Abs().Greater(decimal.DecimalMax) {
		return nil, p.errorf("Number '%s' is invalid or out of range", text)
	}
	return &JsonData{typ: JsonTypeDouble, value: d}, nil
}

// ErrJsonParse JSON 解析错误。
var ErrJsonParse = errors.New("json parse error")
