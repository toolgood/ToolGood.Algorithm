import assert from 'assert';
import { AlgorithmEngine } from '../src/AlgorithmEngine.js';
import { AlgorithmEngineEx } from '../src/AlgorithmEngineEx.js';

// 扩展 AlgorithmEngine 类以添加 TryEvaluate 方法
class AlgorithmEngineWithTryEvaluate extends AlgorithmEngine {
  TryEvaluate(exp, def) {
    const type = typeof def;
    switch (type) {
      case 'number':
        if (Number.isInteger(def)) {
          return this.TryEvaluate_Int(exp, def);
        } else {
          return this.TryEvaluate_Double(exp, def);
        }
      case 'string':
        return this.TryEvaluate_String(exp, def);
      case 'boolean':
        return this.TryEvaluate_Boolean(exp, def);
      default:
        return def;
    }
  }
}

// 测试用例
function testLookFloor() {
  console.log('开始测试 LookFloor...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let lookFloor = engine.TryEvaluate('LookFloor(2,[0,1,2,3,4])', 0);
  assert.strictEqual(lookFloor, 2, 'LookFloor(2,[0,1,2,3,4]) 应该是 2');
  
  lookFloor = engine.TryEvaluate('LookFloor(2.1,[0,1,2,3,4])', 0);
  assert.strictEqual(lookFloor, 2, 'LookFloor(2.1,[0,1,2,3,4]) 应该是 2');
  
  lookFloor = engine.TryEvaluate('LookFloor(-2.1,[0,1,2,3,4])', 0);
  assert.strictEqual(lookFloor, 0, 'LookFloor(-2.1,[0,1,2,3,4]) 应该是 0');
  
  lookFloor = engine.TryEvaluate('LookFloor(5,[0,1,2,3,4])', 0);
  assert.strictEqual(lookFloor, 4, 'LookFloor(5,[0,1,2,3,4]) 应该是 4');
  
  console.log('LookFloor 测试通过！');
}

function testLookCeiling() {
  console.log('开始测试 LookCeiling...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let lookCeiling = engine.TryEvaluate('LookCeiling(2,[0,1,2,3,4])', 0);
  assert.strictEqual(lookCeiling, 2, 'LookCeiling(2,[0,1,2,3,4]) 应该是 2');
  
  lookCeiling = engine.TryEvaluate('LookCeiling(2.1,[0,1,2,3,4])', 0);
  assert.strictEqual(lookCeiling, 3, 'LookCeiling(2.1,[0,1,2,3,4]) 应该是 3');
  
  lookCeiling = engine.TryEvaluate('LookCeiling(-2.1,[0,1,2,3,4])', 0);
  assert.strictEqual(lookCeiling, 0, 'LookCeiling(-2.1,[0,1,2,3,4]) 应该是 0');
  
  lookCeiling = engine.TryEvaluate('LookCeiling(5,[0,1,2,3,4])', 0);
  assert.strictEqual(lookCeiling, 4, 'LookCeiling(5,[0,1,2,3,4]) 应该是 4');
  
  console.log('LookCeiling 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testLookFloor();
    testLookCeiling();
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
runAllTests();

export {
  testLookFloor,
  testLookCeiling,
  runAllTests
};