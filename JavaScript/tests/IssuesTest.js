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
        if (def instanceof Date) {
          return this.TryEvaluate_DateTime(exp, def);
        }
        return def;
    }
  }
}
 
function testIssues13() {
  console.log('开始测试 Issues 13...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  // 测试 DAYS360 函数
  let result = engine.TryEvaluate('days360(date(2020,5,31),date(2023,12,15))', 0);
  assert.strictEqual(result, 1275, "'days360(date(2020,5,31),date(2023,12,15))' 应该返回 1275");
  
  console.log('Issues 13 测试通过！');
}

 
function testIssues0() {
  console.log('开始测试 Issues 0...');
  
  const engine = new AlgorithmEngineEx();
  engine.AddParameter("瓦楞", "BC");
  
  // 测试 JSON 操作和参数使用
  let result = engine.TryEvaluate_Double(`{"A": 0.6,"B": 0.4,"C": 0.6,"E": 0.33,"F": 0.29,"Z": 0.15
,"EB": 0.7,"EE": 0.65,"EA": 0.85,"AB": 1.0,"BC": 1.0,"AA":1.0
,"EBC": 1.15,"BAB": 1.25,"BCB": 1.25,"BBC": 1.25,"CBB": 1.25,"EBA": 1.2,"AAA": 1.4}[瓦楞]`, 0);
  assert.strictEqual(result, 1.0, "JSON 操作应该返回 1.0");
  
  console.log('Issues 0 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testIssues13();
    testIssues0();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testIssues13,
  testIssues0,
  runAllTests
};