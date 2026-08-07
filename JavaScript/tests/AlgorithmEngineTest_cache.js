import assert from 'assert';
import { AlgorithmEngine } from '../src/AlgorithmEngine.js';

// 对齐 C# FunctionCacheTest
// JS 中对应的能力为 AlgorithmEngine.UseParseCache + Parse/ParseWithoutError
class EngineWithCache extends AlgorithmEngine {
  constructor() {
    super();
    this.UseParseCache = true;
  }
}

function testParseWithCacheSameFormulaReturnsSameInstance() {
  console.log('开始测试 ParseWithCache_SameFormula_ReturnsSameInstance...');

  const cache = new EngineWithCache();
  const r1 = cache.Parse("abs(1)");
  const r2 = cache.Parse("abs(1)");
  assert.ok(r1 != null, "Parse('abs(1)') 不应为空");
  assert.strictEqual(r1, r2, "同一公式解析结果应为同一实例");

  console.log('ParseWithCache_SameFormula_ReturnsSameInstance 测试通过！');
}

function testParseWithCacheDifferentFormulasReturnsDifferentInstances() {
  console.log('开始测试 ParseWithCache_DifferentFormulas_ReturnsDifferentInstances...');

  const cache = new EngineWithCache();
  const r1 = cache.Parse("abs(1)");
  const r2 = cache.Parse("abs(2)");
  assert.ok(r1 != null && r2 != null, "解析结果不应为空");
  assert.notStrictEqual(r1, r2, "不同公式解析结果应为不同实例");

  console.log('ParseWithCache_DifferentFormulas_ReturnsDifferentInstances 测试通过！');
}

function testParseWithCacheStringLeafReturnsFunctionBase() {
  console.log('开始测试 ParseWithCache_StringLeaf_ReturnsFunctionBase...');

  const cache = new EngineWithCache();
  // 修复: String 叶子节点也写入缓存(修复前直接返回不缓存)
  const r1 = cache.Parse("'abc'");
  const r2 = cache.Parse("'abc'");
  assert.ok(r1 != null, "Parse(\"'abc'\") 不应为空");
  assert.strictEqual(r1, r2, "字符串叶子节点同一公式解析结果应为同一实例");

  console.log('ParseWithCache_StringLeaf_ReturnsFunctionBase 测试通过！');
}

function testParseWithCacheNestedFunctionReturnsFunctionBase() {
  console.log('开始测试 ParseWithCache_NestedFunction_ReturnsFunctionBase...');

  const cache = new EngineWithCache();
  const r = cache.Parse("max(1,2,3)");
  assert.ok(r != null, "Parse('max(1,2,3)') 不应为空");

  console.log('ParseWithCache_NestedFunction_ReturnsFunctionBase 测试通过！');
}

function testParseWithCacheDiyExpressionReturnsFunctionBase() {
  console.log('开始测试 ParseWithCache_DiyExpression_ReturnsFunctionBase...');

  const cache = new EngineWithCache();
  const r = cache.Parse("myVar");
  assert.ok(r != null, "Parse('myVar') 不应为空");

  console.log('ParseWithCache_DiyExpression_ReturnsFunctionBase 测试通过！');
}

function testParseWithoutErrorWithCacheCachesResult() {
  console.log('开始测试 ParseWithoutError_WithCache...');

  const cache = new EngineWithCache();
  const r1 = cache.ParseWithoutError("1=1");
  const r2 = cache.ParseWithoutError("1=1");
  assert.ok(r1 != null, "ParseWithoutError('1=1') 不应为空");
  assert.strictEqual(r1, r2, "同一公式 ParseWithoutError 结果应为同一实例");

  console.log('ParseWithoutError_WithCache 测试通过！');
}

function testParseCacheInvalidFormulaReturnsNull() {
  console.log('开始测试 ParseCache_InvalidFormula_ReturnsNull...');

  const cache = new EngineWithCache();
  const r = cache.ParseWithoutError("1+");
  assert.ok(r == null, "ParseWithoutError('1+') 应为 null");
  assert.ok(cache.LastError != null, "无效公式应设置 LastError");

  console.log('ParseCache_InvalidFormula_ReturnsNull 测试通过！');
}

function testParseCacheCanEvaluate() {
  console.log('开始测试 ParseCache_CanEvaluate...');

  const cache = new EngineWithCache();
  const r = cache.Parse("abs(1)");
  assert.ok(r != null, "Parse('abs(1)') 不应为空");

  // 缓存实例仍可正常求值
  const e = new EngineWithCache();
  const t = e.TryEvaluate_Int("abs(-5)", 0);
  assert.strictEqual(t, 5, "abs(-5) 应该等于 5");

  console.log('ParseCache_CanEvaluate 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testParseWithCacheSameFormulaReturnsSameInstance();
    testParseWithCacheDifferentFormulasReturnsDifferentInstances();
    testParseWithCacheStringLeafReturnsFunctionBase();
    testParseWithCacheNestedFunctionReturnsFunctionBase();
    testParseWithCacheDiyExpressionReturnsFunctionBase();
    testParseWithoutErrorWithCacheCachesResult();
    testParseCacheInvalidFormulaReturnsNull();
    testParseCacheCanEvaluate();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testParseWithCacheSameFormulaReturnsSameInstance,
  testParseWithCacheDifferentFormulasReturnsDifferentInstances,
  testParseWithCacheStringLeafReturnsFunctionBase,
  testParseWithCacheNestedFunctionReturnsFunctionBase,
  testParseWithCacheDiyExpressionReturnsFunctionBase,
  testParseWithoutErrorWithCacheCachesResult,
  testParseCacheInvalidFormulaReturnsNull,
  testParseCacheCanEvaluate,
  runAllTests
};
