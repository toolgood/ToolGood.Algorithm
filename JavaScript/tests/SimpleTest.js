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
function testSimpleAlgorithmEngine() {
  console.log('开始测试 SimpleAlgorithmEngine...');
  
  const engine = new SimpleAlgorithmEngine();
  
  // 基本算术运算
  let c = engine.TryEvaluate('2+3', 0);
  assert.strictEqual(c, 5, '2+3 应该等于 5');
  console.log('✓ 2+3 = 5');
  
  c = engine.TryEvaluate('(2)+3', 0);
  assert.strictEqual(c, 5, '(2)+3 应该等于 5');
  console.log('✓ (2)+3 = 5');
  
  c = engine.TryEvaluate('2+3*2+10/2*4', 0);
  assert.strictEqual(c, 28, '2+3*2+10/2*4 应该等于 28');
  console.log('✓ 2+3*2+10/2*4 = 28');
  
  // 科学计数法
  c = engine.TryEvaluate('2.1e3 + 10', 0);
  assert.strictEqual(c, 2110, '2.1e3 + 10 应该等于 2110');
  console.log('✓ 2.1e3 + 10 = 2110');
  
  // 浮点数
  let d = engine.TryEvaluate('2.1e-3 + 10', 0.0);
  assert.strictEqual(d, 10.0021, '2.1e-3 + 10 应该等于 10.0021');
  console.log('✓ 2.1e-3 + 10 = 10.0021');
  
  // 常量
  let e = engine.TryEvaluate('Math.E', 0.0);
  assert.strictEqual(e, Math.E, 'e 应该等于 Math.E');
  console.log('✓ Math.E =', Math.E);
  
  e = engine.TryEvaluate('Math.PI', 0.0);
  assert.strictEqual(e, Math.PI, 'pi 应该等于 Math.PI');
  console.log('✓ Math.PI =', Math.PI);
  
  // 布尔值
  let b = engine.TryEvaluate('true', false);
  assert.strictEqual(b, true, 'true 应该等于 true');
  console.log('✓ true = true');
  
  b = engine.TryEvaluate('false', true);
  assert.strictEqual(b, false, 'false 应该等于 false');
  console.log('✓ false = false');
  
  // 条件表达式
  let b1 = engine.TryEvaluate('true ? 1 : 2', 0);
  assert.strictEqual(b1, 1, 'true ? 1 : 2 应该等于 1');
  console.log('✓ true ? 1 : 2 = 1');
  
  b1 = engine.TryEvaluate('false ? 1 : 2', 0);
  assert.strictEqual(b1, 2, 'false ? 1 : 2 应该等于 2');
  console.log('✓ false ? 1 : 2 = 2');
  
  // 字符串
  let s = engine.TryEvaluate('"aa" + "bb"', "");
  assert.strictEqual(s, "aabb", '"aa" + "bb" 应该等于 "aabb"');
  console.log('✓ "aa" + "bb" = "aabb"');
  
  // 数组
  let r = engine.TryEvaluate('[1,2,3,4].length', 0);
  assert.strictEqual(r, 4, '[1,2,3,4].length 应该等于 4');
  console.log('✓ [1,2,3,4].length = 4');
  
  // 比较运算
  let value = engine.TryEvaluate('1 > (-2)', false);
  assert.strictEqual(value, true, '1 > (-2) 应该等于 true');
  console.log('✓ 1 > (-2) = true');
  
  value = engine.TryEvaluate('-1 > -2', false);
  assert.strictEqual(value, true, '-1 > -2 应该等于 true');
  console.log('✓ -1 > -2 = true');
  
  let value3 = engine.TryEvaluate('-7 < -2', false);
  assert.strictEqual(value3, true, '-7 < -2 应该等于 true');
  console.log('✓ -7 < -2 = true');
  
  console.log('所有测试通过！');
}

// 执行测试
testSimpleAlgorithmEngine();
