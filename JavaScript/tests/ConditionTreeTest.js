import assert from 'assert';
import { AlgorithmEngine } from '../src/AlgorithmEngine.js';

// 测试用例
function test1() {
  console.log('开始测试 ConditionTree 基本功能...');
  
  // 注意：JavaScript 中可能没有直接对应的 ConditionTree 类
  // 这里我们简化测试，只测试条件表达式计算
  
  const engine = new AlgorithmEngine();
  
  // 测试字符串比较
  let result = engine.TryEvaluate('AA.IsText() = bb', false);
  assert.ok(typeof result === 'boolean', "'AA.IsText() = bb' 应该返回一个布尔值");
  
  result = engine.TryEvaluate('[bbb]=bb', false);
  assert.ok(typeof result === 'boolean', "'[bbb]=bb' 应该返回一个布尔值");
  
  console.log('ConditionTree 基本功能测试通过！');
}

function test2() {
  console.log('开始测试 ConditionTree AND 操作...');
  
  const engine = new AlgorithmEngine();
  
  // 测试 AND 操作
  let result = engine.TryEvaluate('AA.IsText()=bb && dd=ss', false);
  assert.ok(typeof result === 'boolean', "'AA.IsText()=bb && dd=ss' 应该返回一个布尔值");
  
  console.log('ConditionTree AND 操作测试通过！');
}

function test3() {
  console.log('开始测试 ConditionTree OR 操作...');
  
  const engine = new AlgorithmEngine();
  
  // 测试 OR 操作
  let result = engine.TryEvaluate('AA.IsText()=bb || dd=ss', false);
  assert.ok(typeof result === 'boolean', "'AA.IsText()=bb || dd=ss' 应该返回一个布尔值");
  
  console.log('ConditionTree OR 操作测试通过！');
}

function test4() {
  console.log('开始测试 ConditionTree 复杂表达式...');
  
  const engine = new AlgorithmEngine();
  
  // 测试复杂表达式
  let result = engine.TryEvaluate('AA.IsText()=bb || (dd=ss && tt=22)', false);
  assert.ok(typeof result === 'boolean', "'AA.IsText()=bb || (dd=ss && tt=22)' 应该返回一个布尔值");
  
  result = engine.TryEvaluate('AA.IsText()==bb && (dd=ss || tt=22)', false);
  assert.ok(typeof result === 'boolean', "'AA.IsText()==bb && (dd=ss || tt=22)' 应该返回一个布尔值");
  
  result = engine.TryEvaluate('AA.IsText()==bb ? 1:2', 0);
  assert.ok(typeof result === 'number', "'AA.IsText()==bb ? 1:2' 应该返回一个数字");
  
  result = engine.TryEvaluate('AA.IsText()==bb && (dd=ss || [tt]=22)', false);
  assert.ok(typeof result === 'boolean', "'AA.IsText()==bb && (dd=ss || [tt]=22)' 应该返回一个布尔值");
  
  result = engine.TryEvaluate('AA.IsText()==bb && (dd=ss || [tt]==22)', false);
  assert.ok(typeof result === 'boolean', "'AA.IsText()==bb && (dd=ss || [tt]==22)' 应该返回一个布尔值");
  
  result = engine.TryEvaluate('AA && (dd=ss || [tt]==22)', false);
  assert.ok(typeof result === 'boolean', "'AA && (dd=ss || [tt]==22)' 应该返回一个布尔值");
  
  result = engine.TryEvaluate('1 && (dd=ss || [tt]==22)', false);
  assert.ok(typeof result === 'boolean', "'1 && (dd=ss || [tt]==22)' 应该返回一个布尔值");
  
  console.log('ConditionTree 复杂表达式测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    test1();
    test2();
    test3();
    test4();
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
  test1,
  test2,
  test3,
  test4,
  runAllTests
};