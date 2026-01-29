const assert = require('assert');
const { ToolGoodAlgorithm } = require('./dist/toolgood.algorithm.js');

// 测试构建后的文件
function testDistFile() {
  console.log('开始测试构建后的文件...');
  
  // 获取 AlgorithmEngine 类
  const AlgorithmEngine = ToolGoodAlgorithm.AlgorithmEngine;
  const AlgorithmEngineEx = ToolGoodAlgorithm.AlgorithmEngineEx;
  
  // 扩展 AlgorithmEngine 类以添加 TryEvaluate 方法
  class AlgorithmEngineWithTryEvaluate extends AlgorithmEngine {
    TryEvaluate(exp, def) {
      const type = typeof def;
      switch (type) {
        case 'number':
          if (Number.isInteger(def)) {
            return this.TryEvaluate_Int32(exp, def);
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
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
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
  
  console.log('构建后的文件测试通过！');
}

// 执行测试
testDistFile();
