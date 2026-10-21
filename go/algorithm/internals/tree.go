package internals

import "strings"

// ParameterType 参数类型描述（对应 C# 的 ParameterType 结构体）。
type ParameterType struct {
	Name     string
	Type     OperandType
	Operator string
	Value    string
}

// NewParameterType 创建参数类型描述。
func NewParameterType(name string, t OperandType, operator, value string) ParameterType {
	return ParameterType{Name: name, Type: t, Operator: operator, Value: value}
}

// DiyNameKeyInfo 自定义名称的位置信息。
type DiyNameKeyInfo struct {
	Name  string
	Start int
	End   int
}

func (d DiyNameKeyInfo) String() string { return d.Name }

// DiyNameInfo 自定义参数 / 方法名称集合。
type DiyNameInfo struct {
	Parameters []DiyNameKeyInfo
	Functions  []DiyNameKeyInfo
}

// NewDiyNameInfo 创建 DiyNameInfo。
func NewDiyNameInfo(parameters, functions []DiyNameKeyInfo) *DiyNameInfo {
	if parameters == nil {
		parameters = []DiyNameKeyInfo{}
	}
	if functions == nil {
		functions = []DiyNameKeyInfo{}
	}
	return &DiyNameInfo{Parameters: parameters, Functions: functions}
}

// CalculateTree 计算树节点。
type CalculateTree struct {
	Nodes        []*CalculateTree
	Start        int
	End          int
	Type         CalculateTreeType
	HasBracket   bool
	ErrorMessage string

	source   string
	text     string
	hasText  bool
}

// NewCalculateTree 创建计算树节点。
func NewCalculateTree() *CalculateTree {
	return &CalculateTree{Type: CalculateTreeTypeString, Nodes: []*CalculateTree{}}
}

// SetSource 记录原始表达式（用于 Text 懒加载）。
func (t *CalculateTree) SetSource(source string) { t.source = source }

// Text 返回 [Start, End] 区间的原始文本。
func (t *CalculateTree) Text() string {
	if !t.hasText && t.source != "" && t.End >= t.Start {
		end := t.End + 1
		if end > len(t.source) {
			end = len(t.source)
		}
		if t.Start <= len(t.source) {
			t.text = t.source[t.Start:end]
		}
		t.hasText = true
	}
	return t.text
}

// ConditionTree 条件树节点。
type ConditionTree struct {
	Nodes        []*ConditionTree
	Start        int
	End          int
	Type         ConditionTreeType
	HasBracket   bool
	ErrorMessage string

	source  string
	text    string
	hasText bool
}

// NewConditionTree 创建条件树节点。
func NewConditionTree() *ConditionTree {
	return &ConditionTree{Type: ConditionTreeTypeString, Nodes: []*ConditionTree{}}
}

// SetSource 记录原始表达式。
func (t *ConditionTree) SetSource(source string) { t.source = source }

// Text 返回 [Start, End] 区间的原始文本。
func (t *ConditionTree) Text() string {
	if !t.hasText && t.source != "" && t.End >= t.Start {
		end := t.End + 1
		if end > len(t.source) {
			end = len(t.source)
		}
		if t.Start <= len(t.source) {
			t.text = t.source[t.Start:end]
		}
		t.hasText = true
	}
	return t.text
}

// writeJoin 将子节点用分隔符连接写入。
func writeJoin(sb *strings.Builder, sep string, count int, write func(i int)) {
	for i := 0; i < count; i++ {
		if i > 0 {
			sb.WriteString(sep)
		}
		write(i)
	}
}
