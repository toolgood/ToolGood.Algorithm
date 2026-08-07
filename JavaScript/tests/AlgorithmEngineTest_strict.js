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

// 对齐 C# StrictModeTest
// 二元 && 运算符
function testAndStrictShouldErrorWhenRightIsError() {
  console.log('开始测试 and_strict_should_error_when_right_is_error...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  const t = engine.TryEvaluate("(1>2) && ERROR('test')", true);
  // 严格模式下，即使左边为 false，右边错误也会传播
  assert.strictEqual(t, true, "严格模式下 (1>2) && ERROR('test') 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "严格模式下右边错误应传播到 LastError");

  console.log('and_strict_should_error_when_right_is_error 测试通过！');
}

function testAndStrictAllValid() {
  console.log('开始测试 and_strict_all_valid...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  const t = engine.TryEvaluate("2>1 && 3>2", false);
  assert.strictEqual(t, true, "2>1 && 3>2 应该等于 true");
  assert.ok(engine.LastError === null, "严格模式全有效时 LastError 应为 null");

  console.log('and_strict_all_valid 测试通过！');
}

function testAndNotStrictShortCircuit() {
  console.log('开始测试 and_not_strict_short_circuit...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：1>2 为 false，短路不执行右边
  const t = engine.TryEvaluate("(1>2) && ERROR('test')", true);
  assert.strictEqual(t, false, "非严格模式 (1>2) && ERROR('test') 应短路返回 false");
  assert.ok(engine.LastError === null, "非严格模式短路时 LastError 应为 null");

  console.log('and_not_strict_short_circuit 测试通过！');
}

function testAndNotStrictErrorOnLeft() {
  console.log('开始测试 and_not_strict_error_on_left...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：左边错误无法短路，应报错
  const t = engine.TryEvaluate("ERROR('left') && (2>1)", true);
  assert.strictEqual(t, true, "非严格模式 ERROR('left') && (2>1) 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "非严格模式左边错误应报错");

  console.log('and_not_strict_error_on_left 测试通过！');
}

function testAndNotStrictContinueWhenLeftTrue() {
  console.log('开始测试 and_not_strict_continue_when_left_true...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：左边为 true，继续执行右边
  const t = engine.TryEvaluate("(2>1) && ERROR('right')", true);
  assert.strictEqual(t, true, "非严格模式 (2>1) && ERROR('right') 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "非严格模式右边错误应报错");

  console.log('and_not_strict_continue_when_left_true 测试通过！');
}

function testAndNotStrictAllValid() {
  console.log('开始测试 and_not_strict_all_valid...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  const t = engine.TryEvaluate("2>1 && 3>2", false);
  assert.strictEqual(t, true, "2>1 && 3>2 应该等于 true");
  assert.ok(engine.LastError === null, "非严格模式全有效时 LastError 应为 null");

  console.log('and_not_strict_all_valid 测试通过！');
}

// 二元 || 运算符
function testOrStrictShouldErrorWhenRightIsError() {
  console.log('开始测试 or_strict_should_error_when_right_is_error...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  // 严格模式下，即使左边为 true，右边错误也会传播
  const t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
  assert.strictEqual(t, false, "严格模式下 (2>1) || ERROR('test') 返回值应为默认值 false");
  assert.ok(engine.LastError != null, "严格模式下右边错误应传播到 LastError");

  console.log('or_strict_should_error_when_right_is_error 测试通过！');
}

function testOrStrictAllValid() {
  console.log('开始测试 or_strict_all_valid...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  const t = engine.TryEvaluate("1>2 || 3>2", false);
  assert.strictEqual(t, true, "1>2 || 3>2 应该等于 true");
  assert.ok(engine.LastError === null, "严格模式全有效时 LastError 应为 null");

  console.log('or_strict_all_valid 测试通过！');
}

function testOrNotStrictShortCircuit() {
  console.log('开始测试 or_not_strict_short_circuit...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：2>1 为 true，短路不执行右边
  const t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
  assert.strictEqual(t, true, "非严格模式 (2>1) || ERROR('test') 应短路返回 true");
  assert.ok(engine.LastError === null, "非严格模式短路时 LastError 应为 null");

  console.log('or_not_strict_short_circuit 测试通过！');
}

function testOrNotStrictErrorOnLeft() {
  console.log('开始测试 or_not_strict_error_on_left...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：左边错误无法短路，应报错
  const t = engine.TryEvaluate("ERROR('left') || (2>1)", false);
  assert.strictEqual(t, false, "非严格模式 ERROR('left') || (2>1) 返回值应为默认值 false");
  assert.ok(engine.LastError != null, "非严格模式左边错误应报错");

  console.log('or_not_strict_error_on_left 测试通过！');
}

function testOrNotStrictContinueWhenLeftFalse() {
  console.log('开始测试 or_not_strict_continue_when_left_false...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：左边为 false，继续执行右边
  const t = engine.TryEvaluate("(1>2) || ERROR('right')", true);
  assert.strictEqual(t, true, "非严格模式 (1>2) || ERROR('right') 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "非严格模式右边错误应报错");

  console.log('or_not_strict_continue_when_left_false 测试通过！');
}

function testOrNotStrictAllValid() {
  console.log('开始测试 or_not_strict_all_valid...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  const t = engine.TryEvaluate("1>2 || 3>2", false);
  assert.strictEqual(t, true, "1>2 || 3>2 应该等于 true");
  assert.ok(engine.LastError === null, "非严格模式全有效时 LastError 应为 null");

  console.log('or_not_strict_all_valid 测试通过！');
}

// n 元 AND() 函数
function testAndNStrictShouldErrorWhenAnyParamIsError() {
  console.log('开始测试 and_n_strict_should_error_when_any_param_is_error...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  const t = engine.TryEvaluate("AND(true(), true(), ERROR('test'))", true);
  assert.strictEqual(t, true, "严格模式下 AND(true(), true(), ERROR('test')) 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "严格模式下 AND 任一参数错误应传播");

  console.log('and_n_strict_should_error_when_any_param_is_error 测试通过！');
}

function testAndNStrictAllValid() {
  console.log('开始测试 and_n_strict_all_valid...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  const t = engine.TryEvaluate("AND(true(), 1=1, 3>2)", false);
  assert.strictEqual(t, true, "AND(true(), 1=1, 3>2) 应该等于 true");
  assert.ok(engine.LastError === null, "严格模式全有效时 LastError 应为 null");

  console.log('and_n_strict_all_valid 测试通过！');
}

function testAndNNotStrictShortCircuit() {
  console.log('开始测试 and_n_not_strict_short_circuit...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：遇到 false 就短路
  const t = engine.TryEvaluate("AND(true(), false(), ERROR('test'))", true);
  assert.strictEqual(t, false, "非严格模式 AND(true(), false(), ERROR('test')) 应短路返回 false");
  assert.ok(engine.LastError === null, "非严格模式短路时 LastError 应为 null");

  console.log('and_n_not_strict_short_circuit 测试通过！');
}

function testAndNNotStrictErrorBeforeShortCircuit() {
  console.log('开始测试 and_n_not_strict_error_before_short_circuit...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：错误在短路条件之前，应报错
  const t = engine.TryEvaluate("AND(ERROR('first'), false(), true())", true);
  assert.strictEqual(t, true, "非严格模式 AND(ERROR('first'), false(), true()) 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "非严格模式短路前的错误应报错");

  console.log('and_n_not_strict_error_before_short_circuit 测试通过！');
}

function testAndNNotStrictAllValid() {
  console.log('开始测试 and_n_not_strict_all_valid...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  const t = engine.TryEvaluate("AND(true(), 1=1, 3>2)", false);
  assert.strictEqual(t, true, "AND(true(), 1=1, 3>2) 应该等于 true");
  assert.ok(engine.LastError === null, "非严格模式全有效时 LastError 应为 null");

  console.log('and_n_not_strict_all_valid 测试通过！');
}

// n 元 OR() 函数
function testOrNStrictShouldErrorWhenAnyParamIsError() {
  console.log('开始测试 or_n_strict_should_error_when_any_param_is_error...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  const t = engine.TryEvaluate("OR(false(), false(), ERROR('test'))", true);
  assert.strictEqual(t, true, "严格模式下 OR(false(), false(), ERROR('test')) 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "严格模式下 OR 任一参数错误应传播");

  console.log('or_n_strict_should_error_when_any_param_is_error 测试通过！');
}

function testOrNStrictAllValid() {
  console.log('开始测试 or_n_strict_all_valid...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  const t = engine.TryEvaluate("OR(false(), 1=2, 3>2)", false);
  assert.strictEqual(t, true, "OR(false(), 1=2, 3>2) 应该等于 true");
  assert.ok(engine.LastError === null, "严格模式全有效时 LastError 应为 null");

  console.log('or_n_strict_all_valid 测试通过！');
}

function testOrNNotStrictShortCircuit() {
  console.log('开始测试 or_n_not_strict_short_circuit...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：遇到 true 就短路
  const t = engine.TryEvaluate("OR(false(), true(), ERROR('test'))", false);
  assert.strictEqual(t, true, "非严格模式 OR(false(), true(), ERROR('test')) 应短路返回 true");
  assert.ok(engine.LastError === null, "非严格模式短路时 LastError 应为 null");

  console.log('or_n_not_strict_short_circuit 测试通过！');
}

function testOrNNotStrictErrorBeforeShortCircuit() {
  console.log('开始测试 or_n_not_strict_error_before_short_circuit...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // 非严格模式：错误在短路条件之前，应报错
  const t = engine.TryEvaluate("OR(ERROR('first'), true(), false())", true);
  assert.strictEqual(t, true, "非严格模式 OR(ERROR('first'), true(), false()) 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "非严格模式短路前的错误应报错");

  console.log('or_n_not_strict_error_before_short_circuit 测试通过！');
}

function testOrNNotStrictAllValid() {
  console.log('开始测试 or_n_not_strict_all_valid...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  const t = engine.TryEvaluate("OR(false(), 1=2, 3>2)", false);
  assert.strictEqual(t, true, "OR(false(), 1=2, 3>2) 应该等于 true");
  assert.ok(engine.LastError === null, "非严格模式全有效时 LastError 应为 null");

  console.log('or_n_not_strict_all_valid 测试通过！');
}

// 链式短路
function testChainedAndNotStrictShortCircuit() {
  console.log('开始测试 chained_and_not_strict_short_circuit...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // (1>2) && ... 短路，跳过 ERROR
  const t = engine.TryEvaluate("(1>2) && ERROR('a') && ERROR('b')", true);
  assert.strictEqual(t, false, "非严格模式 (1>2) && ERROR('a') && ERROR('b') 应短路返回 false");
  assert.ok(engine.LastError === null, "非严格模式链式短路时 LastError 应为 null");

  console.log('chained_and_not_strict_short_circuit 测试通过！');
}

function testChainedOrNotStrictShortCircuit() {
  console.log('开始测试 chained_or_not_strict_short_circuit...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = false;
  // (2>1) || ... 短路，跳过 ERROR
  const t = engine.TryEvaluate("(2>1) || ERROR('a') || ERROR('b')", false);
  assert.strictEqual(t, true, "非严格模式 (2>1) || ERROR('a') || ERROR('b') 应短路返回 true");
  assert.ok(engine.LastError === null, "非严格模式链式短路时 LastError 应为 null");

  console.log('chained_or_not_strict_short_circuit 测试通过！');
}

function testChainedAndStrictAllEvaluated() {
  console.log('开始测试 chained_and_strict_all_evaluated...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  // 严格模式：所有条件都要求值，遇到错误报错
  const t = engine.TryEvaluate("(1>2) && (3>2) && ERROR('test')", true);
  assert.strictEqual(t, true, "严格模式下 (1>2) && (3>2) && ERROR('test') 返回值应为默认值 true");
  assert.ok(engine.LastError != null, "严格模式链式求值遇到错误应报错");

  console.log('chained_and_strict_all_evaluated 测试通过！');
}

function testChainedOrStrictAllEvaluated() {
  console.log('开始测试 chained_or_strict_all_evaluated...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseStrictMode = true;
  // 严格模式：所有条件都要求值，遇到错误报错
  const t = engine.TryEvaluate("(2>1) || (3>2) || ERROR('test')", false);
  assert.strictEqual(t, false, "严格模式下 (2>1) || (3>2) || ERROR('test') 返回值应为默认值 false");
  assert.ok(engine.LastError != null, "严格模式链式求值遇到错误应报错");

  console.log('chained_or_strict_all_evaluated 测试通过！');
}

// 默认行为
function testDefaultModeIsStrict() {
  console.log('开始测试 default_mode_is_strict...');

  const engine = new AlgorithmEngineWithTryEvaluate();
  // 默认 UseStrictMode = true
  const t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
  assert.strictEqual(t, false, "默认严格模式下 (2>1) || ERROR('test') 返回值应为默认值 false");
  assert.ok(engine.LastError != null, "默认严格模式下右边错误应传播到 LastError");

  console.log('default_mode_is_strict 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    // 二元 && 运算符
    testAndStrictShouldErrorWhenRightIsError();
    testAndStrictAllValid();
    testAndNotStrictShortCircuit();
    testAndNotStrictErrorOnLeft();
    testAndNotStrictContinueWhenLeftTrue();
    testAndNotStrictAllValid();
    // 二元 || 运算符
    testOrStrictShouldErrorWhenRightIsError();
    testOrStrictAllValid();
    testOrNotStrictShortCircuit();
    testOrNotStrictErrorOnLeft();
    testOrNotStrictContinueWhenLeftFalse();
    testOrNotStrictAllValid();
    // n 元 AND() 函数
    testAndNStrictShouldErrorWhenAnyParamIsError();
    testAndNStrictAllValid();
    testAndNNotStrictShortCircuit();
    testAndNNotStrictErrorBeforeShortCircuit();
    testAndNNotStrictAllValid();
    // n 元 OR() 函数
    testOrNStrictShouldErrorWhenAnyParamIsError();
    testOrNStrictAllValid();
    testOrNNotStrictShortCircuit();
    testOrNNotStrictErrorBeforeShortCircuit();
    testOrNNotStrictAllValid();
    // 链式短路
    testChainedAndNotStrictShortCircuit();
    testChainedOrNotStrictShortCircuit();
    testChainedAndStrictAllEvaluated();
    testChainedOrStrictAllEvaluated();
    // 默认行为
    testDefaultModeIsStrict();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testAndStrictShouldErrorWhenRightIsError,
  testAndStrictAllValid,
  testAndNotStrictShortCircuit,
  testAndNotStrictErrorOnLeft,
  testAndNotStrictContinueWhenLeftTrue,
  testAndNotStrictAllValid,
  testOrStrictShouldErrorWhenRightIsError,
  testOrStrictAllValid,
  testOrNotStrictShortCircuit,
  testOrNotStrictErrorOnLeft,
  testOrNotStrictContinueWhenLeftFalse,
  testOrNotStrictAllValid,
  testAndNStrictShouldErrorWhenAnyParamIsError,
  testAndNStrictAllValid,
  testAndNNotStrictShortCircuit,
  testAndNNotStrictErrorBeforeShortCircuit,
  testAndNNotStrictAllValid,
  testOrNStrictShouldErrorWhenAnyParamIsError,
  testOrNStrictAllValid,
  testOrNNotStrictShortCircuit,
  testOrNNotStrictErrorBeforeShortCircuit,
  testOrNNotStrictAllValid,
  testChainedAndNotStrictShortCircuit,
  testChainedOrNotStrictShortCircuit,
  testChainedAndStrictAllEvaluated,
  testChainedOrStrictAllEvaluated,
  testDefaultModeIsStrict,
  runAllTests
};
