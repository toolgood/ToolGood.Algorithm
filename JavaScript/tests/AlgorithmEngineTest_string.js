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
function testASC() {
  console.log('开始测试 ASC...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let asc = engine.TryEvaluate("asc('ａｂｃＡＢＣ１２３')", "");
  assert.strictEqual(asc, "abcABC123", "asc('ａｂｃＡＢＣ１２３') 应该是 'abcABC123'");
  
  console.log('ASC 测试通过！');
}

function testJis() {
  console.log('开始测试 Jis...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let jis = engine.TryEvaluate("jis('abcABC123')", "");
  assert.strictEqual(jis, "ａｂｃＡＢＣ１２３", "jis('abcABC123') 应该是 'ａｂｃＡＢＣ１２３'");
  
  jis = engine.TryEvaluate("WIDECHAR('abcABC123')", "");
  assert.strictEqual(jis, "ａｂｃＡＢＣ１２３", "WIDECHAR('abcABC123') 应该是 'ａｂｃＡＢＣ１２３'");
  
  console.log('Jis 测试通过！');
}

function testCHAR() {
  console.log('开始测试 CHAR...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let char = engine.TryEvaluate("char(49)", "");
  assert.strictEqual(char, "1", "char(49) 应该是 '1'");
  
  console.log('CHAR 测试通过！');
}

function testCLEAN() {
  console.log('开始测试 CLEAN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let clean = engine.TryEvaluate("clean('\\r112\\t')", "");
  assert.strictEqual(clean, "112", "clean('\\r112\\t') 应该是 '112'");
  
  console.log('CLEAN 测试通过！');
}

function testCode() {
  console.log('开始测试 code...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let code = engine.TryEvaluate("code('1')", 0);
  assert.strictEqual(code, 49, "code('1') 应该是 49");
  
  console.log('code 测试通过！');
}

function testCONCATENATE() {
  console.log('开始测试 CONCATENATE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let concatenate = engine.TryEvaluate("CONCATENATE('tt','33')", "");
  assert.strictEqual(concatenate, "tt33", "CONCATENATE('tt','33') 应该是 'tt33'");
  
  console.log('CONCATENATE 测试通过！');
}

function testEXACT() {
  console.log('开始测试 EXACT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let exact = engine.TryEvaluate("EXACT('tt','33')", true);
  assert.strictEqual(exact, false, "EXACT('tt','33') 应该是 false");
  
  exact = engine.TryEvaluate("EXACT('tt','tt')", false);
  assert.strictEqual(exact, true, "EXACT('tt','tt') 应该是 true");
  
  exact = engine.TryEvaluate("EXACT('33',33)", false);
  assert.strictEqual(exact, true, "EXACT('33',33) 应该是 true");
  
  exact = engine.TryEvaluate("EXACT('331.1',331.1)", false);
  assert.strictEqual(exact, true, "EXACT('331.1',331.1) 应该是 true");
  
  exact = engine.TryEvaluate("EXACT('TRUE',TRUE())", false);
  assert.strictEqual(exact, true, "EXACT('TRUE',TRUE()) 应该是 true");
  
  exact = engine.TryEvaluate("EXACT('1',TRUE())", true);
  assert.strictEqual(exact, false, "EXACT('1',TRUE()) 应该是 false");
  
  console.log('EXACT 测试通过！');
}

function testFIND() {
  console.log('开始测试 FIND...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let find = engine.TryEvaluate("FIND(\"11\",\"12221122\")", 0);
  assert.strictEqual(find, 5, "FIND(\"11\",\"12221122\") 应该是 5");
  
  console.log('FIND 测试通过！');
}

function testFIXED() {
  console.log('开始测试 FIXED...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let fixed = engine.TryEvaluate("FIXED(4567.89,1)", "");
  assert.strictEqual(fixed, "4,567.9", "FIXED(4567.89,1) 应该是 '4,567.9'");
  
  fixed = engine.TryEvaluate("FIXED(-4567.89, 1, TRUE())", "");
  assert.strictEqual(fixed, "-4567.9", "FIXED(-4567.89, 1, TRUE()) 应该是 '-4567.9'");
  
  fixed = engine.TryEvaluate("FIXED(77.888)", "");
  assert.strictEqual(fixed, "77.89", "FIXED(77.888) 应该是 '77.89'");
  
  console.log('FIXED 测试通过！');
}

function testLEFT() {
  console.log('开始测试 LEFT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let left = engine.TryEvaluate("LEFT('123222',3)", "");
  assert.strictEqual(left, "123", "LEFT('123222',3) 应该是 '123'");
  
  console.log('LEFT 测试通过！');
}

function testLEN() {
  console.log('开始测试 LEN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let len = engine.TryEvaluate("LEN('123222')", 0);
  assert.strictEqual(len, 6, "LEN('123222') 应该是 6");
  
  console.log('LEN 测试通过！');
}

function testLOWER() {
  console.log('开始测试 LOWER...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let lower = engine.TryEvaluate("LOWER('ABC')", "");
  assert.strictEqual(lower, "abc", "LOWER('ABC') 应该是 'abc'");
  
  console.log('LOWER 测试通过！');
}

function testMID() {
  console.log('开始测试 MID...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let mid = engine.TryEvaluate("MID('ABCDEF',2,3)", "");
  assert.strictEqual(mid, "BCD", "MID('ABCDEF',2,3) 应该是 'BCD'");
  
  console.log('MID 测试通过！');
}

function testPROPER() {
  console.log('开始测试 PROPER...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let proper = engine.TryEvaluate("PROPER('abc abc')", "");
  assert.strictEqual(proper, "Abc Abc", "PROPER('abc abc') 应该是 'Abc Abc'");
  
  console.log('PROPER 测试通过！');
}

function testREPLACE() {
  console.log('开始测试 REPLACE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let replace = engine.TryEvaluate("REPLACE(\"abccd\",2,3,\"2\")", "");
  assert.strictEqual(replace, "a2d", "REPLACE(\"abccd\",2,3,\"2\") 应该是 'a2d'");
  
  let replace1 = engine.TryEvaluate("REPLACE(\"abccd\",'bc',\"2\")", "");
  assert.strictEqual(replace1, "a2cd", "REPLACE(\"abccd\",'bc',\"2\") 应该是 'a2cd'");
  
  console.log('REPLACE 测试通过！');
}

function testREPT() {
  console.log('开始测试 REPT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let rept = engine.TryEvaluate("REPT(\"q\",3)", "");
  assert.strictEqual(rept, "qqq", "REPT(\"q\",3) 应该是 'qqq'");
  
  console.log('REPT 测试通过！');
}

function testRIGHT() {
  console.log('开始测试 RIGHT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let right = engine.TryEvaluate("RIGHT(\"123q\",3)", "");
  assert.strictEqual(right, "23q", "RIGHT(\"123q\",3) 应该是 '23q'");
  
  console.log('RIGHT 测试通过！');
}

function testRMB() {
  console.log('开始测试 RMB...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let rmb = engine.TryEvaluate("rmb(12.3)", "");
  assert.strictEqual(rmb, "壹拾贰元叁角", "rmb(12.3) 应该是 '壹拾贰元叁角'");
  
  console.log('RMB 测试通过！');
}

function testSEARCH() {
  console.log('开始测试 SEARCH...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let search = engine.TryEvaluate("SEARCH(\"aa\",\"abbAaddd\")", 0);
  assert.strictEqual(search, 4, "SEARCH(\"aa\",\"abbAaddd\") 应该是 4");
  
  console.log('SEARCH 测试通过！');
}

function testSUBSTITUTE() {
  console.log('开始测试 SUBSTITUTE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let substitute = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\")", "");
  assert.strictEqual(substitute, "1212cc", "SUBSTITUTE(\"ababcc\",\"ab\",\"12\") 应该是 '1212cc'");
  
  substitute = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\",2)", "");
  assert.strictEqual(substitute, "ab12cc", "SUBSTITUTE(\"ababcc\",\"ab\",\"12\",2) 应该是 'ab12cc'");
  
  substitute = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"123\",\"1111111111111111111111\")", "");
  assert.strictEqual(substitute, "1111111111111111111111456789", "SUBSTITUTE(\"123456789\",\"123\",\"1111111111111111111111\") 应该是 '1111111111111111111111456789'");
  
  substitute = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"1239\",\"1111111111111111111111\")", "");
  assert.strictEqual(substitute, "123456789", "SUBSTITUTE(\"123456789\",\"1239\",\"1111111111111111111111\") 应该是 '123456789'");
  
  substitute = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"9\",\"1111111111111111111111\")", "");
  assert.strictEqual(substitute, "123456781111111111111111111111", "SUBSTITUTE(\"123456789\",\"9\",\"1111111111111111111111\") 应该是 '123456781111111111111111111111'");
  
  console.log('SUBSTITUTE 测试通过！');
}

function testT() {
  console.log('开始测试 T...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let t = engine.TryEvaluate("T(12)", "");
  assert.strictEqual(t, "", "T(12) 应该是 ''");
  
  t = engine.TryEvaluate("T('123')", "");
  assert.strictEqual(t, "123", "T('123') 应该是 '123'");
  
  console.log('T 测试通过！');
}

function testTEXT() {
  console.log('开始测试 TEXT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let text = engine.TryEvaluate("TEXT(123,\"0.00\")", "");
  assert.strictEqual(text, "123.00", "TEXT(123,\"0.00\") 应该是 '123.00'");
  
  console.log('TEXT 测试通过！');
}

function testTRIM() {
  console.log('开始测试 TRIM...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let trim = engine.TryEvaluate("TRIM(\" 123 123 \")", "");
  assert.strictEqual(trim, "123 123", "TRIM(\" 123 123 \") 应该是 '123 123'");
  
  console.log('TRIM 测试通过！');
}

function testUPPER() {
  console.log('开始测试 UPPER...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let upper = engine.TryEvaluate("UPPER(\"abc\")", "");
  assert.strictEqual(upper, "ABC", "UPPER(\"abc\") 应该是 'ABC'");
  
  console.log('UPPER 测试通过！');
}

function testVALUE() {
  console.log('开始测试 VALUE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let value = engine.TryEvaluate("VALUE(\"123\")", 0);
  assert.strictEqual(value, 123, "VALUE(\"123\") 应该是 123");
  
  console.log('VALUE 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testASC();
    testJis();
    testCHAR();
    testCLEAN();
    testCode();
    testCONCATENATE();
    testEXACT();
    testFIND();
    testFIXED();
    testLEFT();
    testLEN();
    testLOWER();
    testMID();
    testPROPER();
    testREPLACE();
    testREPT();
    testRIGHT();
    testRMB();
    testSEARCH();
    testSUBSTITUTE();
    testT();
    testTEXT();
    testTRIM();
    testUPPER();
    testVALUE();
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
  testASC,
  testJis,
  testCHAR,
  testCLEAN,
  testCode,
  testCONCATENATE,
  testEXACT,
  testFIND,
  testFIXED,
  testLEFT,
  testLEN,
  testLOWER,
  testMID,
  testPROPER,
  testREPLACE,
  testREPT,
  testRIGHT,
  testRMB,
  testSEARCH,
  testSUBSTITUTE,
  testT,
  testTEXT,
  testTRIM,
  testUPPER,
  testVALUE,
  runAllTests
};
