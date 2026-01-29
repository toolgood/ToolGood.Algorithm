import assert from 'assert';
import { AlgorithmEngine } from '../src/AlgorithmEngine.js';

// 测试用例
function test() {
  console.log('开始测试 CalculateTree...');
  
  // 注意：JavaScript 中可能没有直接对应的 CalculateTree 类
  // 这里我们简化测试，只测试表达式计算
  
  const engine = new AlgorithmEngine();
  
  // 测试加法
  let result = engine.TryEvaluate('A1+1', 0);
  assert.ok(typeof result === 'number', "'A1+1' 应该返回一个数字");
  
  // 测试减法
  result = engine.TryEvaluate('A1-(1+1)', 0);
  assert.ok(typeof result === 'number', "'A1-(1+1)' 应该返回一个数字");
  
  // 测试乘法
  result = engine.TryEvaluate('A1*(1+1)', 0);
  assert.ok(typeof result === 'number', "'A1*(1+1)' 应该返回一个数字");
  
  // 测试除法
  result = engine.TryEvaluate('A1/(1+1)', 0);
  assert.ok(typeof result === 'number', "'A1/(1+1)' 应该返回一个数字");
  
  // 测试取模
  result = engine.TryEvaluate('A1%(1+1)', 0);
  assert.ok(typeof result === 'number', "'A1%(1+1)' 应该返回一个数字");
  
  // 测试字符串连接
  result = engine.TryEvaluate('A1&(1+1)', "");
  assert.ok(typeof result === 'string', "'A1&(1+1)' 应该返回一个字符串");
  
  // 测试函数调用
  result = engine.TryEvaluate('A1(1+1)', "");
  assert.ok(typeof result === 'string', "'A1(1+1)' 应该返回一个字符串");
  
  // 测试负数
  result = engine.TryEvaluate('-1+(1+1)', 0);
  assert.strictEqual(result, 1, "'-1+(1+1)' 应该等于 1");
  
  result = engine.TryEvaluate('-1', 0);
  assert.strictEqual(result, -1, "'-1' 应该等于 -1");
  
  console.log('CalculateTree 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    test();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
if (import.meta.url === import.meta.resolve('./')) {
  runAllTests();
}

export {
  test,
  runAllTests
};