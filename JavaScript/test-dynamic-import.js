// 使用动态导入测试 ES6 模块
async function testDynamicImport() {
  console.log('开始测试动态导入 ES6 模块...');
  
  try {
    // 动态导入 ES6 模块
    const { AlgorithmEngine } = await import('./src/AlgorithmEngine.js');
    const { AlgorithmEngineEx } = await import('./src/AlgorithmEngineEx.js');
    
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
    console.log('2+3 =', c);
    
    c = engine.TryEvaluate('(2)+3', 0);
    console.log('(2)+3 =', c);
    
    c = engine.TryEvaluate('2+3*2+10/2*4', 0);
    console.log('2+3*2+10/2*4 =', c);
    
    console.log('动态导入测试成功！');
  } catch (error) {
    console.error('动态导入测试失败:', error.message);
    console.error(error.stack);
  }
}

// 执行测试
testDynamicImport();
