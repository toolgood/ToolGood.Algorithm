import assert from 'assert';
import { AlgorithmEngine } from '../src/AlgorithmEngine.js';

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

// 对齐 C# OperatorTest + MathBaseTest 百分比/边界值测试
function testArithmetic() {
  console.log('开始测试 arithmetic...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("1+(3*2+2)/2", 0);
  assert.strictEqual(t, 5, "1+(3*2+2)/2 应该等于 5");

  t = engine.TryEvaluate("(8-3)*(3+2)", 0);
  assert.strictEqual(t, 25, "(8-3)*(3+2) 应该等于 25");

  t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0);
  assert.strictEqual(t, 4, "(8-3)*(3+2) % 7 应该等于 4");

  let c = engine.TryEvaluate("2+3", 0);
  assert.strictEqual(c, 5, "2+3 应该等于 5");
  c = engine.TryEvaluate("(2)+3", 0);
  assert.strictEqual(c, 5, "(2)+3 应该等于 5");
  c = engine.TryEvaluate("2+3*2+10/2*4", 0);
  assert.strictEqual(c, 28, "2+3*2+10/2*4 应该等于 28");

  c = engine.TryEvaluate("2.1e3 + 10", 0);
  assert.strictEqual(c, 2110, "2.1e3 + 10 应该等于 2110");

  c = engine.TryEvaluate("2.1e+03 + 10", 0);
  assert.strictEqual(c, 2110, "2.1e+03 + 10 应该等于 2110");

  c = engine.TryEvaluate("2.1e+3 + 10", 0);
  assert.strictEqual(c, 2110, "2.1e+3 + 10 应该等于 2110");

  const d = engine.TryEvaluate("2.1e-3 + 10", 0.1);
  assert.strictEqual(d, 10.0021, "2.1e-3 + 10 应该等于 10.0021");

  console.log('arithmetic 测试通过！');
}

function testConnect() {
  console.log('开始测试 connect...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let s = engine.TryEvaluate("'aa'&'bb'", "");
  assert.strictEqual(s, "aabb", "'aa'&'bb' 应该等于 'aabb'");

  s = engine.TryEvaluate("'3'+2", "");
  assert.strictEqual(s, "5", "'3'+2 应该等于 '5'");

  console.log('connect 测试通过！');
}

function testPercentage() {
  console.log('开始测试 percentage...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("100%", 0.1);
  assert.strictEqual(t, 1.0, "100% 应该等于 1.0");

  t = engine.TryEvaluate("50%", 0.1);
  assert.strictEqual(t, 0.5, "50% 应该等于 0.5");

  t = engine.TryEvaluate("1%", 0.1);
  assert.strictEqual(t, 0.01, "1% 应该等于 0.01");

  t = engine.TryEvaluate("0%", 0.1);
  assert.strictEqual(t, 0.0, "0% 应该等于 0.0");

  t = engine.TryEvaluate("200%", 0.1);
  assert.strictEqual(t, 2.0, "200% 应该等于 2.0");

  t = engine.TryEvaluate("25.5%", 0.1);
  assert.strictEqual(t, 0.255, "25.5% 应该等于 0.255");

  t = engine.TryEvaluate("100*50%", 0.1);
  assert.strictEqual(t, 50.0, "100*50% 应该等于 50.0");

  t = engine.TryEvaluate("100+50%", 0.1);
  assert.strictEqual(t, 100.5, "100+50% 应该等于 100.5");

  console.log('percentage 测试通过！');
}

function testDivisionByZero() {
  console.log('开始测试 DivisionByZero...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("1/0", 0.0);
  assert.strictEqual(t, 0, "1/0 应该返回 0");

  t = engine.TryEvaluate("0/0", 0.0);
  assert.strictEqual(t, 0, "0/0 应该返回 0");

  console.log('DivisionByZero 测试通过！');
}

function testOverflow() {
  console.log('开始测试 Overflow...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  // C# 用 decimal 溢出(1e20*1e20)返回 0; JS 用 double 不会溢出,改用超过 double 最大值的表达式
  const t = engine.TryEvaluate("1e308*10", 0.0);
  assert.strictEqual(t, 0, "1e308*10 溢出应该返回 0");

  console.log('Overflow 测试通过！');
}

function testNullOperation() {
  console.log('开始测试 NullOperation...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("null+1", 0.0);
  assert.strictEqual(t, 0, "null+1 应该返回 0");

  t = engine.TryEvaluate("null*100", 0.0);
  assert.strictEqual(t, 0, "null*100 应该返回 0");

  t = engine.TryEvaluate("null-null", 0.0);
  assert.strictEqual(t, 0, "null-null 应该返回 0");

  const tbb2 = engine.TryEvaluate("'111'*null", 0);
  assert.strictEqual(tbb2, 0, "'111'*null 应该返回 0");

  console.log('NullOperation 测试通过！');
}

function testSquareRootNegative() {
  console.log('开始测试 SquareRootNegative...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  const t = engine.TryEvaluate("sqrt(-1)", 0.0);
  assert.strictEqual(t, 0, "sqrt(-1) 应该返回 0");

  console.log('SquareRootNegative 测试通过！');
}

function testLogNegative() {
  console.log('开始测试 LogNegative...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("log(-1)", 0.0);
  assert.strictEqual(t, 0, "log(-1) 应该返回 0");

  t = engine.TryEvaluate("ln(0)", 0.0);
  assert.strictEqual(t, 0, "ln(0) 应该返回 0");

  console.log('LogNegative 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testArithmetic();
    testConnect();
    testPercentage();
    testDivisionByZero();
    testOverflow();
    testNullOperation();
    testSquareRootNegative();
    testLogNegative();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testArithmetic,
  testConnect,
  testPercentage,
  testDivisionByZero,
  testOverflow,
  testNullOperation,
  testSquareRootNegative,
  testLogNegative,
  runAllTests
};
