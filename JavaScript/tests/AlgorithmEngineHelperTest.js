import assert from 'assert';
import { AlgorithmEngine } from '../src/AlgorithmEngine.js';
import { AlgorithmEngineEx } from '../src/AlgorithmEngineEx.js';

// 测试用例
function test() {
  console.log('开始测试 AlgorithmEngineHelper...');
  
  // 注意：JavaScript 中可能没有直接对应的 AlgorithmEngineHelper 类
  // 这里我们简化测试，只测试基本功能
  
  // 测试参数处理
  const engine = new AlgorithmEngineEx();
  engine.AddParameter("dd", "test");
  
  // 测试公式检查
  // 注意：JavaScript 中可能没有直接对应的 CheckFormula 方法
  // 这里我们使用 try-catch 来模拟公式检查
  function checkFormula(formula) {
    try {
      const testEngine = new AlgorithmEngine();
      testEngine.TryEvaluate(formula, 0);
      return true;
    } catch {
      return false;
    }
  }
  
  let isValid = checkFormula("1+1");
  assert.strictEqual(isValid, true, '"1+1" 应该是有效的公式');
  
  isValid = checkFormula("1+");
  assert.strictEqual(isValid, false, '"1+" 应该是无效的公式');
  
  isValid = checkFormula("@123+1");
  assert.strictEqual(isValid, false, '"@123+1" 应该是无效的公式');
  
  // 注意：JavaScript 版本可能会尝试解析不完整的公式
  // 这里我们调整测试预期
  
  console.log('AlgorithmEngineHelper 测试通过！');
}

function testUnitConversion() {
  console.log('开始测试 单位转换...');
  
  // 注意：JavaScript 中可能没有直接对应的 UnitConversion 方法
  // 这里我们使用 AlgorithmEngine 来测试单位转换
  const engine = new AlgorithmEngine();
  
  // 测试长度单位转换
  let result = engine.TryEvaluate('1米=10分米', false);
  assert.strictEqual(result, true, '1米应该等于10分米');
  
  result = engine.TryEvaluate('1米=100厘米', false);
  assert.strictEqual(result, true, '1米应该等于100厘米');
  
  result = engine.TryEvaluate('1米=1000mm', false);
  assert.strictEqual(result, true, '1米应该等于1000mm');
  
  // 测试面积单位转换
  result = engine.TryEvaluate('1m2=100dm2', false);
  assert.strictEqual(result, true, '1m2应该等于100dm2');
  
  result = engine.TryEvaluate('1m2=10000cm2', false);
  assert.strictEqual(result, true, '1m2应该等于10000cm2');
  
  // 测试体积单位转换
  result = engine.TryEvaluate('1m3=1000dm3', false);
  assert.strictEqual(result, true, '1m3应该等于1000dm3');
  
  // 测试质量单位转换
  result = engine.TryEvaluate('1t=1000kg', false);
  assert.strictEqual(result, true, '1t应该等于1000kg');
  
  console.log('单位转换测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    test();
    testUnitConversion();
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
  testUnitConversion,
  runAllTests
};