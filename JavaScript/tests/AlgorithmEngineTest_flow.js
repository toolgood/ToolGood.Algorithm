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

// 测试用例
function testIf() {
  console.log('开始测试 If...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("if(1=1,1,2)", 0);
  assert.strictEqual(result, 1, "if(1=1,1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(1=1,1)", 0);
  assert.strictEqual(result, 1, "if(1=1,1) 应该是 1");
  
  result = engine.TryEvaluate("if(1=1，1)", 0);
  assert.strictEqual(result, 1, "if(1=1，1) 应该是 1");
  
  result = engine.TryEvaluate("if(3,1,2)", 0);
  assert.strictEqual(result, 1, "if(3,1,2) 应该是 1");
  
  result = engine.TryEvaluate("if('1',1,2)", 0);
  assert.strictEqual(result, 1, "if('1',1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(0,1,2)", 0);
  assert.strictEqual(result, 2, "if(0,1,2) 应该是 2");
  
  console.log('If 测试通过！');
}

function testIferror() {
  console.log('开始测试 iferror...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("iferror(1/0,1,2)", 0);
  assert.strictEqual(result, 1, "iferror(1/0,1,2) 应该是 1");
  
  result = engine.TryEvaluate("iferror(1-'rrr',1,2)", 0);
  assert.strictEqual(result, 1, "iferror(1-'rrr',1,2) 应该是 1");
  
  console.log('iferror 测试通过！');
}

function testIserror() {
  console.log('开始测试 iserror...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("iserror(1/0,1)", 0);
  assert.strictEqual(result, 1, "iserror(1/0,1) 应该是 1");
  
  result = engine.TryEvaluate("iserror(1-'rrr',1)", 0);
  assert.strictEqual(result, 1, "iserror(1-'rrr',1) 应该是 1");
  
  console.log('iserror 测试通过！');
}

function testIfnull() {
  console.log('开始测试 ifnull...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("isnull(null,1)", 0);
  assert.strictEqual(result, 1, "isnull(null,1) 应该是 1");
  
  result = engine.TryEvaluate("isnull(1,2)", 0);
  assert.strictEqual(result, 1, "isnull(1,2) 应该是 1");
  
  console.log('ifnull 测试通过！');
}

function testIsnullorerror() {
  console.log('开始测试 isnullorerror...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("isnullorerror(null,1)", 0);
  assert.strictEqual(result, 1, "isnullorerror(null,1) 应该是 1");
  
  result = engine.TryEvaluate("isnullorerror(1/0,1)", 0);
  assert.strictEqual(result, 1, "isnullorerror(1/0,1) 应该是 1");
  
  result = engine.TryEvaluate("isnullorerror(1,2)", 0);
  assert.strictEqual(result, 1, "isnullorerror(1,2) 应该是 1");
  
  console.log('isnullorerror 测试通过！');
}

function testISNUMBER() {
  console.log('开始测试 ISNUMBER...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("if(ISNUMBER(1),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISNUMBER(1),1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(ISNUMBER('e'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISNUMBER('e'),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISNUMBER('11'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISNUMBER('11'),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISNUMBER('2016-1-2'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISNUMBER('2016-1-2'),1,2) 应该是 2");
  
  console.log('ISNUMBER 测试通过！');
}

function testISTEXT() {
  console.log('开始测试 ISTEXT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("if(ISTEXT(1),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISTEXT(1),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISTEXT('e'),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISTEXT('e'),1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(ISTEXT('11'),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISTEXT('11'),1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(ISTEXT('2016-1-2'),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISTEXT('2016-1-2'),1,2) 应该是 1");
  
  console.log('ISTEXT 测试通过！');
}

function testISNONTEXT() {
  console.log('开始测试 ISNONTEXT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("if(ISNONTEXT(1),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISNONTEXT(1),1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(ISNONTEXT('e'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISNONTEXT('e'),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISNONTEXT('11'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISNONTEXT('11'),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISNONTEXT('2016-1-2'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISNONTEXT('2016-1-2'),1,2) 应该是 2");
  
  console.log('ISNONTEXT 测试通过！');
}

function testISLOGICAL() {
  console.log('开始测试 ISLOGICAL...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("if(ISLOGICAL(1),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISLOGICAL(1),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISLOGICAL('e'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISLOGICAL('e'),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISLOGICAL(true),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISLOGICAL(true),1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(ISLOGICAL(false),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISLOGICAL(false),1,2) 应该是 1");
  
  console.log('ISLOGICAL 测试通过！');
}

function testISEVEN() {
  console.log('开始测试 ISEVEN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("if(ISEVEN(1),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISEVEN(1),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISEVEN(2),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISEVEN(2),1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(ISEVEN('e'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISEVEN('e'),1,2) 应该是 2");
  
  console.log('ISEVEN 测试通过！');
}

function testISODD() {
  console.log('开始测试 ISODD...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("if(ISODD(1),1,2)", 0);
  assert.strictEqual(result, 1, "if(ISODD(1),1,2) 应该是 1");
  
  result = engine.TryEvaluate("if(ISODD(2),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISODD(2),1,2) 应该是 2");
  
  result = engine.TryEvaluate("if(ISODD('e'),1,2)", 0);
  assert.strictEqual(result, 2, "if(ISODD('e'),1,2) 应该是 2");
  
  console.log('ISODD 测试通过！');
}

function testAnd() {
  console.log('开始测试 And...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("and(true(),1=1)", false);
  assert.strictEqual(result, true, "and(true(),1=1) 应该是 true");
  
  result = engine.TryEvaluate("and(true(),1)", false);
  assert.strictEqual(result, true, "and(true(),1) 应该是 true");
  
  result = engine.TryEvaluate("and(true(),false(),1=1)", true);
  assert.strictEqual(result, false, "and(true(),false(),1=1) 应该是 false");
  
  result = engine.TryEvaluate("and(true(),false(),1)", true);
  assert.strictEqual(result, false, "and(true(),false(),1) 应该是 false");
  
  result = engine.TryEvaluate("and(true(),0)", true);
  assert.strictEqual(result, false, "and(true(),0) 应该是 false");
  
  console.log('And 测试通过！');
}

function testOr() {
  console.log('开始测试 Or...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("or(true(),1=1)", false);
  assert.strictEqual(result, true, "or(true(),1=1) 应该是 true");
  
  result = engine.TryEvaluate("or(true(),1)", false);
  assert.strictEqual(result, true, "or(true(),1) 应该是 true");
  
  result = engine.TryEvaluate("or(true(),false(),1=1)", false);
  assert.strictEqual(result, true, "or(true(),false(),1=1) 应该是 true");
  
  result = engine.TryEvaluate("or(true(),false(),1)", false);
  assert.strictEqual(result, true, "or(true(),false(),1) 应该是 true");
  
  result = engine.TryEvaluate("or(true(),0)", false);
  assert.strictEqual(result, true, "or(true(),0) 应该是 true");
  
  result = engine.TryEvaluate("or(false(),1=2)", true);
  assert.strictEqual(result, false, "or(false(),1=2) 应该是 false");
  
  console.log('Or 测试通过！');
}

function testNot() {
  console.log('开始测试 Not...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("not(true())", true);
  assert.strictEqual(result, false, "not(true()) 应该是 false");
  
  result = engine.TryEvaluate("not(false())", false);
  assert.strictEqual(result, true, "not(false()) 应该是 true");
  
  console.log('Not 测试通过！');
}

function testTrue() {
  console.log('开始测试 true...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("true()", false);
  assert.strictEqual(result, true, "true() 应该是 true");
  
  console.log('true 测试通过！');
}

function testFalse() {
  console.log('开始测试 false...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("false()", true);
  assert.strictEqual(result, false, "false() 应该是 false");
  
  console.log('false 测试通过！');
}

function testAndor() {
  console.log('开始测试 andor...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let result = engine.TryEvaluate("1=1 && 2==2 and 3=3", false);
  assert.strictEqual(result, true, "1=1 && 2==2 and 3=3 应该是 true");
  
  result = engine.TryEvaluate("1=1 && 2!=2", true);
  assert.strictEqual(result, false, "1=1 && 2!=2 应该是 false");
  
  result = engine.TryEvaluate("1=1 || 2!=2", false);
  assert.strictEqual(result, true, "1=1 || 2!=2 应该是 true");
  
  result = engine.TryEvaluate("1=1 and 2==2", false);
  assert.strictEqual(result, true, "1=1 and 2==2 应该是 true");
  
  result = engine.TryEvaluate("1=1 and 2!=2", true);
  assert.strictEqual(result, false, "1=1 and 2!=2 应该是 false");
  
  result = engine.TryEvaluate("1=1 or 2!=2", false);
  assert.strictEqual(result, true, "1=1 or 2!=2 应该是 true");
  
  console.log('andor 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testIf();
    testIferror();
    testIserror();
    testIfnull();
    testIsnullorerror();
    testISNUMBER();
    testISTEXT();
    testISNONTEXT();
    testISLOGICAL();
    testISEVEN();
    testISODD();
    testAnd();
    testOr();
    testNot();
    testTrue();
    testFalse();
    testAndor();
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
  testIf,
  testIferror,
  testIserror,
  testIfnull,
  testIsnullorerror,
  testISNUMBER,
  testISTEXT,
  testISNONTEXT,
  testISLOGICAL,
  testISEVEN,
  testISODD,
  testAnd,
  testOr,
  testNot,
  testTrue,
  testFalse,
  testAndor,
  runAllTests
};
