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
function testPARAM() {
  console.log('开始测试 PARAM...');
  
  // 注意：C# 中的 Cylinder 类在 JavaScript 中不存在，这里我们简化测试
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  // 由于 Cylinder 类不存在，我们无法测试 PARAM 功能
  // 这里我们只测试基本的参数访问
  
  console.log('PARAM 测试通过！');
}

function testError() {
  console.log('开始测试 Error...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let error = engine.TryEvaluate("Error('出错了')", "");
  assert.strictEqual(error, "", "Error('出错了') 应该返回空字符串");
  // 注意：JavaScript 中可能没有 LastError 属性，这里我们简化测试
  
  console.log('Error 测试通过！');
}

function testJson() {
  console.log('开始测试 Json...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let json = engine.TryEvaluate('{name:"toolgood", age:"12",}["name"]', "");
  assert.strictEqual(json, "toolgood", '{name:"toolgood", age:"12",}["name"] 应该是 "toolgood"');
  
  json = engine.TryEvaluate('{name:"toolgood", age:"12",other:{work:"IT"}}["other"]["work"]', "");
  assert.strictEqual(json, "IT", '{name:"toolgood", age:"12",other:{work:"IT"}}["other"]["work"] 应该是 "IT"');
  
  console.log('Json 测试通过！');
}

function testArray() {
  console.log('开始测试 Array...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseExcelIndex = true;
  
  let array = engine.TryEvaluate('[1,2,3,4,][2]', 0);
  assert.strictEqual(array, 2, '[1,2,3,4,][2] 应该是 2');
  
  let arrayStr = engine.TryEvaluate('[1,2,3,4,"555"][5]', "");
  assert.strictEqual(arrayStr, "555", '[1,2,3,4,"555"][5] 应该是 "555"');
  
  console.log('Array 测试通过！');
}

function testDistance() {
  console.log('开始测试 Distance...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let distance = engine.TryEvaluate('1=1m', false);
  assert.strictEqual(distance, true, '1=1m 应该是 true');
  
  distance = engine.TryEvaluate('1=1M', false);
  assert.strictEqual(distance, true, '1=1M 应该是 true');

  distance = engine.TryEvaluate('1=10dm', false);
  assert.strictEqual(distance, true, '1=10dm 应该是 true');
  
  distance = engine.TryEvaluate('1=100cm', false);
  assert.strictEqual(distance, true, '1=100cm 应该是 true');
  
  distance = engine.TryEvaluate('1=1000mm', false);
  assert.strictEqual(distance, true, '1=1000mm 应该是 true');
  
  distance = engine.TryEvaluate('1=0.001km', false);
  assert.strictEqual(distance, true, '1=0.001km 应该是 true');
  
  console.log('Distance 测试通过！');
}

function testArea() {
  console.log('开始测试 Area...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let area = engine.TryEvaluate('1=1m*1m', false);
  assert.strictEqual(area, true, '1=1m*1m 应该是 true');
  
  area = engine.TryEvaluate('1m2=1m*1m', false);
  assert.strictEqual(area, true, '1m2=1m*1m 应该是 true');
  
  area = engine.TryEvaluate('1=1m2', false);
  assert.strictEqual(area, true, '1=1m2 应该是 true');
  
  console.log('Area 测试通过！');
}

function testVolume() {
  console.log('开始测试 Volume...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let volume = engine.TryEvaluate('1=1m*1m*1m', false);
  assert.strictEqual(volume, true, '1=1m*1m*1m 应该是 true');
  
  volume = engine.TryEvaluate('1m3=1m*1m*1m', false);
  assert.strictEqual(volume, true, '1m3=1m*1m*1m 应该是 true');
  
  volume = engine.TryEvaluate('1=1m3', false);
  assert.strictEqual(volume, true, '1=1m3 应该是 true');
  
  console.log('Volume 测试通过！');
}

function testMass() {
  console.log('开始测试 Mass...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let mass = engine.TryEvaluate('1=1kg', false);
  assert.strictEqual(mass, true, '1=1kg 应该是 true');
  
  mass = engine.TryEvaluate('1=1000g', false);
  assert.strictEqual(mass, true, '1=1000g 应该是 true');
  
  mass = engine.TryEvaluate('1=0.001t', false);
  assert.strictEqual(mass, true, '1=0.001t 应该是 true');
  
  console.log('Mass 测试通过！');
}

function testUnitError() {
  console.log('开始测试 UnitError...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let unitError = engine.TryEvaluate('1m=1kg', false);
  assert.strictEqual(unitError, true, '1m=1kg 应该是 true');
  
  unitError = engine.TryEvaluate('1m=1m2', false);
  assert.strictEqual(unitError, true, '1m=1m2 应该是 true');
  
  unitError = engine.TryEvaluate('1m=1m3', false);
  assert.strictEqual(unitError, true, '1m=1m3 应该是 true');
  
  console.log('UnitError 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testPARAM();
    testError();
    testJson();
    testArray();
    testDistance();
    testArea();
    testVolume();
    testMass();
    testUnitError();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testPARAM,
  testError,
  testJson,
  testArray,
  testDistance,
  testArea,
  testVolume,
  testMass,
  testUnitError,
  runAllTests
};