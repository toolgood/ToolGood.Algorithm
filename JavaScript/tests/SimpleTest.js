import assert from 'assert';

// 简化版的 AlgorithmEngine 类
class SimpleAlgorithmEngine {
  TryEvaluate(exp, def) {
    try {
      // 简单的表达式求值
      const result = eval(exp);
      return typeof result === typeof def ? result : def;
    } catch (e) {
      return def;
    }
  }
}

// 测试用例
function testBasicArithmetic() {
  console.log('开始测试 基本算术运算...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let c = engine.TryEvaluate('2+3', 0);
  assert.strictEqual(c, 5, '2+3 应该等于 5');
  
  c = engine.TryEvaluate('(2)+3', 0);
  assert.strictEqual(c, 5, '(2)+3 应该等于 5');
  
  c = engine.TryEvaluate('2+3*2+10/2*4', 0);
  assert.strictEqual(c, 28, '2+3*2+10/2*4 应该等于 28');
  
  console.log('基本算术运算 测试通过！');
}

function testScientificNotation() {
  console.log('开始测试 科学计数法...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let c = engine.TryEvaluate('2.1e3 + 10', 0);
  assert.strictEqual(c, 2110, '2.1e3 + 10 应该等于 2110');
  
  console.log('科学计数法 测试通过！');
}

function testFloatingPoint() {
  console.log('开始测试 浮点数...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let d = engine.TryEvaluate('2.1e-3 + 10', 0.0);
  assert.strictEqual(d, 10.0021, '2.1e-3 + 10 应该等于 10.0021');
  
  console.log('浮点数 测试通过！');
}

function testConstants() {
  console.log('开始测试 常量...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let e = engine.TryEvaluate('Math.E', 0.0);
  assert.strictEqual(e, Math.E, 'e 应该等于 Math.E');
  
  e = engine.TryEvaluate('Math.PI', 0.0);
  assert.strictEqual(e, Math.PI, 'pi 应该等于 Math.PI');
  
  console.log('常量 测试通过！');
}

function testBoolean() {
  console.log('开始测试 布尔值...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let b = engine.TryEvaluate('true', false);
  assert.strictEqual(b, true, 'true 应该等于 true');
  
  b = engine.TryEvaluate('false', true);
  assert.strictEqual(b, false, 'false 应该等于 false');
  
  console.log('布尔值 测试通过！');
}

function testConditionalExpression() {
  console.log('开始测试 条件表达式...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let b1 = engine.TryEvaluate('true ? 1 : 2', 0);
  assert.strictEqual(b1, 1, 'true ? 1 : 2 应该等于 1');
  
  b1 = engine.TryEvaluate('false ? 1 : 2', 0);
  assert.strictEqual(b1, 2, 'false ? 1 : 2 应该等于 2');
  
  console.log('条件表达式 测试通过！');
}

function testString() {
  console.log('开始测试 字符串...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let s = engine.TryEvaluate('"aa" + "bb"', "");
  assert.strictEqual(s, "aabb", '"aa" + "bb" 应该等于 "aabb"');
  
  console.log('字符串 测试通过！');
}

function testArray() {
  console.log('开始测试 数组...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let r = engine.TryEvaluate('[1,2,3,4].length', 0);
  assert.strictEqual(r, 4, '[1,2,3,4].length 应该等于 4');
  
  console.log('数组 测试通过！');
}

function testComparison() {
  console.log('开始测试 比较运算...');
  
  const engine = new SimpleAlgorithmEngine();
  
  let value = engine.TryEvaluate('1 > (-2)', false);
  assert.strictEqual(value, true, '1 > (-2) 应该等于 true');
  
  value = engine.TryEvaluate('-1 > -2', false);
  assert.strictEqual(value, true, '-1 > -2 应该等于 true');
  
  let value3 = engine.TryEvaluate('-7 < -2', false);
  assert.strictEqual(value3, true, '-7 < -2 应该等于 true');
  
  console.log('比较运算 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testBasicArithmetic();
    testScientificNotation();
    testFloatingPoint();
    testConstants();
    testBoolean();
    testConditionalExpression();
    testString();
    testArray();
    testComparison();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testBasicArithmetic,
  testScientificNotation,
  testFloatingPoint,
  testConstants,
  testBoolean,
  testConditionalExpression,
  testString,
  testArray,
  testComparison,
  runAllTests
};
