package algorithm

import (
	"errors"
	"sync"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/enums"
)

// IFunctionCache 函数缓存接口。
//
// 对应 C# 的 IFunctionCache，缓存计算表达式、条件表达式与自定义名称的解析结果。
type IFunctionCache interface {
	// GetDiyNamesWithCache 获取自定义名称信息，并使用缓存。
	GetDiyNamesWithCache(exp string) *DiyNameInfo
	// ParseWithCache 解析计算表达式，并使用缓存。
	ParseWithCache(funExp string) (Function, error)
	// ParseConditionWithCache 解析条件表达式，并使用缓存。
	ParseConditionWithCache(funExp string) (Function, error)
}

// functionCacheEntry 缓存项。
//
// C# 用 ConcurrentDictionary<string, FunctionBase> 直接缓存节点，解析失败时异常不会被缓存；
// Go 的 API 返回 error，因此这里把错误一起缓存（对应「解析失败缓存错误」的要求）。
type functionCacheEntry struct {
	fn  Function
	err error
}

// FunctionCache 函数缓存。
//
// 对应 C# 的 FunctionCache（ConcurrentDictionary 实现），Go 用互斥锁保护普通 map。
type FunctionCache struct {
	// engine 构造时传入的引擎，仅作保留（C# 的 FunctionCache 不依赖引擎，解析使用全局函数注册表）。
	engine *AlgorithmEngine

	mu             sync.RWMutex
	calculateCache map[string]functionCacheEntry
	conditionCache map[string]functionCacheEntry
	diyNameCache   map[string]*DiyNameInfo
}

// 编译期确认 FunctionCache 实现 IFunctionCache。
var _ IFunctionCache = (*FunctionCache)(nil)

// NewFunctionCache 创建函数缓存。
//
// 对应 C# 的 FunctionCache 构造函数；engine 可为 nil。
func NewFunctionCache(engine *AlgorithmEngine) *FunctionCache {
	return &FunctionCache{
		engine:         engine,
		calculateCache: map[string]functionCacheEntry{},
		conditionCache: map[string]functionCacheEntry{},
		diyNameCache:   map[string]*DiyNameInfo{},
	}
}

// Engine 返回构造时传入的引擎（Go 附加：C# 的 FunctionCache 不持有引擎）。
func (c *FunctionCache) Engine() *AlgorithmEngine { return c.engine }

// GetDiyNamesWithCache 获取自定义名称信息，并使用缓存。
//
// C# 在解析失败时抛出异常；Go 版本返回 nil 并缓存该失败结果
// （calculationlogic.FunctionCache 的使用方已按 nil 处理）。
func (c *FunctionCache) GetDiyNamesWithCache(exp string) *DiyNameInfo {
	c.mu.RLock()
	info, ok := c.diyNameCache[exp]
	c.mu.RUnlock()
	if ok {
		return info
	}

	info, err := GetDiyNames(exp)
	if err != nil {
		info = nil
	}

	c.mu.Lock()
	c.diyNameCache[exp] = info
	c.mu.Unlock()
	return info
}

// ParseWithCache 解析计算表达式，并使用缓存。
func (c *FunctionCache) ParseWithCache(exp string) (Function, error) {
	if entry, ok := c.lookup(c.calculateCache, exp); ok {
		return entry.fn, entry.err
	}

	tree := c.ParseCalculateTree(exp)
	fn, err := c.createCalculate(tree, []rune(exp))

	c.store(c.calculateCache, exp, fn, err)
	return fn, err
}

// ParseConditionWithCache 解析条件表达式，并使用缓存。
func (c *FunctionCache) ParseConditionWithCache(exp string) (Function, error) {
	if entry, ok := c.lookup(c.conditionCache, exp); ok {
		return entry.fn, entry.err
	}

	tree := c.ParseConditionTree(exp)
	fn, err := c.createCondition(tree, []rune(exp))

	c.store(c.conditionCache, exp, fn, err)
	return fn, err
}

// ParseCalculateTree 解析计算表达式为计算树。
//
// Go 附加方法：C# 中这一步由 FunctionCache 内部的 AlgorithmEngineHelper.ParseCalculate 完成。
func (c *FunctionCache) ParseCalculateTree(exp string) *CalculateTree { return ParseCalculate(exp) }

// ParseConditionTree 解析条件表达式为条件树。
//
// Go 附加方法：C# 中这一步由 FunctionCache 内部的 AlgorithmEngineHelper.ParseCondition 完成。
func (c *FunctionCache) ParseConditionTree(condition string) *ConditionTree {
	return ParseCondition(condition)
}

// createCalculate 由计算树构建节点（对应 C# 的 FunctionCache.CreateCalculate）。
func (c *FunctionCache) createCalculate(tree *CalculateTree, source []rune) (Function, error) {
	if tree.Type == enums.CalculateTreeTypeString {
		// 仅叶子节点需要字符串内容；中间节点不再以 Text(子串)作为缓存 key，
		// 避免 Substring 产生 O(n²) 子串驻留（与 C# 一致）。
		text := treeText(source, tree.Start, tree.End)
		if entry, ok := c.lookup(c.calculateCache, text); ok {
			return entry.fn, entry.err
		}
		fn, err := ParseFormula(text)
		c.store(c.calculateCache, text, fn, err)
		return fn, err
	}
	if tree.Type == enums.CalculateTreeTypeError {
		return nil, errors.New(tree.ErrorMessage)
	}
	if len(tree.Nodes) < 2 {
		return nil, errors.New("calculate tree node is invalid")
	}

	left, err := c.createCalculate(tree.Nodes[0], source)
	if err != nil {
		return nil, err
	}
	right, err := c.createCalculate(tree.Nodes[1], source)
	if err != nil {
		return nil, err
	}
	// CalculateTreeType 与 CombineCalculateType 的数值一一对应（1..13），
	// 对应 C# 的 (CombineCalculateType)(byte)tree.Type。
	return CombineCalculate(left, enums.CombineCalculateType(tree.Type), right), nil
}

// createCondition 由条件树构建节点（对应 C# 的 FunctionCache.CreateCondition）。
func (c *FunctionCache) createCondition(tree *ConditionTree, source []rune) (Function, error) {
	if tree.Type == enums.ConditionTreeTypeString {
		// 直接走计算树路径，避免对同一 key 重入缓存。
		text := treeText(source, tree.Start, tree.End)
		return c.createCalculate(ParseCalculate(text), []rune(text))
	}
	if len(tree.Nodes) < 2 {
		return nil, errors.New(tree.ErrorMessage)
	}

	left, err := c.createCondition(tree.Nodes[0], source)
	if err != nil {
		return nil, err
	}
	right, err := c.createCondition(tree.Nodes[1], source)
	if err != nil {
		return nil, err
	}
	switch tree.Type {
	case enums.ConditionTreeTypeAnd:
		return Condition_And(left, right), nil
	case enums.ConditionTreeTypeOr:
		return Condition_Or(left, right), nil
	}
	return nil, errors.New(tree.ErrorMessage)
}

func (c *FunctionCache) lookup(m map[string]functionCacheEntry, key string) (functionCacheEntry, bool) {
	c.mu.RLock()
	defer c.mu.RUnlock()
	entry, ok := m[key]
	return entry, ok
}

func (c *FunctionCache) store(m map[string]functionCacheEntry, key string, fn Function, err error) {
	c.mu.Lock()
	defer c.mu.Unlock()
	m[key] = functionCacheEntry{fn: fn, err: err}
}

// treeText 按 rune 下标截取 [start, end] 文本。
//
// internals.CalculateTree.Text() 用 rune 下标做字节切片，非 ASCII 表达式会截取错位，
// 因此缓存键统一走本函数（C# 使用 char 下标，行为与 rune 下标一致）。
func treeText(source []rune, start, end int) string {
	if start < 0 {
		start = 0
	}
	if end >= len(source) {
		end = len(source) - 1
	}
	if end < start {
		return ""
	}
	return string(source[start : end+1])
}
