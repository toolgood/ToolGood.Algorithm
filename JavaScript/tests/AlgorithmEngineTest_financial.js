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

// 扩展 AlgorithmEngineEx 以添加 TryEvaluate 方法（用于带自定义参数的测试）
class AlgorithmEngineExWithTryEvaluate extends AlgorithmEngineEx {
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

// 对齐 C# Math.Round(value, digits)
function roundTo(value, digits) {
  const f = Math.pow(10, digits);
  return Math.round(value * f) / f;
}

// 测试用例
function testPMT() {
  console.log('开始测试 PMT...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("PMT(0.08/12, 10, 10000)", 0.1);
  assert.strictEqual(roundTo(t, 4), roundTo(-1037.0321, 4), "PMT(0.08/12, 10, 10000) 应该约等于 -1037.0321");

  console.log('PMT 测试通过！');
}

function testPPMT() {
  console.log('开始测试 PPMT...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("PPMT(0.08/12, 1, 10, 10000)", 0.1);
  assert.strictEqual(roundTo(t, 4), roundTo(-970.3654, 4), "PPMT(0.08/12, 1, 10, 10000) 应该约等于 -970.3654");

  console.log('PPMT 测试通过！');
}

function testIPMT() {
  console.log('开始测试 IPMT...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("IPMT(0.08/12, 1, 10, 10000)", 0.1);
  assert.strictEqual(roundTo(t, 4), roundTo(-66.6667, 4), "IPMT(0.08/12, 1, 10, 10000) 应该约等于 -66.6667");

  console.log('IPMT 测试通过！');
}

function testPV() {
  console.log('开始测试 PV...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("PV(0.08/12, 10, -1000)", 0.1);
  assert.strictEqual(roundTo(t, 2), roundTo(9642.90, 2), "PV(0.08/12, 10, -1000) 应该约等于 9642.90");

  console.log('PV 测试通过！');
}

function testFV() {
  console.log('开始测试 FV...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("FV(0.08/12, 10, -1000)", 0.1);
  assert.strictEqual(roundTo(t, 2), roundTo(10305.40, 2), "FV(0.08/12, 10, -1000) 应该约等于 10305.40");

  console.log('FV 测试通过！');
}

function testNPER() {
  console.log('开始测试 NPER...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("NPER(0.08/12, -1000, 10000)", 0.1);
  assert.strictEqual(roundTo(t, 2), roundTo(10.38, 2), "NPER(0.08/12, -1000, 10000) 应该约等于 10.38");

  console.log('NPER 测试通过！');
}

function testRATE() {
  console.log('开始测试 RATE...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("RATE(12,-100,400,0,0,0.1)", 0.1);
  assert.strictEqual(roundTo(t, 4), roundTo(0.2289, 4), "RATE(12,-100,400,0,0,0.1) 应该约等于 0.2289");

  console.log('RATE 测试通过！');
}

function testNPV() {
  console.log('开始测试 NPV...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("NPV(0.1, -10000, 3000, 4200, 6800)", 0.1);
  assert.strictEqual(roundTo(t, 2), roundTo(1188.44, 2), "NPV(0.1, -10000, 3000, 4200, 6800) 应该约等于 1188.44");

  console.log('NPV 测试通过！');
}

function testIRR() {
  console.log('开始测试 IRR...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("IRR(array(-70000, 12000, 15000, 18000, 21000, 26000))", 0.1);
  assert.strictEqual(roundTo(t, 4), roundTo(0.0866, 4), "IRR(array(-70000, 12000, 15000, 18000, 21000, 26000)) 应该约等于 0.0866");

  console.log('IRR 测试通过！');
}

function testMIRR() {
  console.log('开始测试 MIRR...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("MIRR(array(-70000, 12000, 15000, 18000, 21000, 26000), 0.1, 0.12)", 0.1);
  assert.strictEqual(roundTo(t, 4), roundTo(0.0987, 4), "MIRR(array(-70000, 12000, 15000, 18000, 21000, 26000), 0.1, 0.12) 应该约等于 0.0987");

  console.log('MIRR 测试通过！');
}

function testSLN() {
  console.log('开始测试 SLN...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("SLN(30000, 7500, 10)", 0.1);
  assert.strictEqual(t, 2250.0, "SLN(30000, 7500, 10) 应该等于 2250");

  console.log('SLN 测试通过！');
}

function testSYD() {
  console.log('开始测试 SYD...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("SYD(30000, 7500, 10, 1)", 0.1);
  assert.strictEqual(roundTo(t, 2), roundTo(4090.91, 2), "SYD(30000, 7500, 10, 1) 应该约等于 4090.91");

  console.log('SYD 测试通过！');
}

function testDDB() {
  console.log('开始测试 DDB...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("DDB(2400, 300, 10, 2)", 0.1);
  assert.strictEqual(roundTo(t, 2), roundTo(384.0, 2), "DDB(2400, 300, 10, 2) 应该约等于 384.0");

  console.log('DDB 测试通过！');
}

function testDB() {
  console.log('开始测试 DB...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  let t = engine.TryEvaluate("DB(1000000, 100000, 6, 1)", 0.1);
  assert.strictEqual(roundTo(t, 2), roundTo(319000.0, 2), "DB(1000000, 100000, 6, 1) 应该约等于 319000.0");

  console.log('DB 测试通过！');
}

function testDB_month() {
  console.log('开始测试 DB_month...');

  const engine = new AlgorithmEngineWithTryEvaluate();

  // Excel: month<12 时折旧跨越 life+1 个期间(第 1 年部分月 + life-1 个整年 + 最后部分月)
  // 修复前 period>life 直接报错, 现在允许 period=life+1
  let t = engine.TryEvaluate("DB(1000000, 100000, 6, 7, 7)", 0.1);
  assert.ok(engine.LastError === null && t > 0, "DB(1000000, 100000, 6, 7, 7) 应该成功且结果大于 0");

  // 超过总期间数仍报错
  t = engine.TryEvaluate("DB(1000000, 100000, 6, 8, 7)", 0.1);
  assert.ok(engine.LastError != null, "DB(1000000, 100000, 6, 8, 7) 应该报错");

  // month=12 时总期间数等于 life, 最后一年折旧为整年
  t = engine.TryEvaluate("DB(1000000, 100000, 6, 6, 12)", 0.1);
  assert.ok(engine.LastError === null && t > 0, "DB(1000000, 100000, 6, 6, 12) 应该成功且结果大于 0");

  // period=1 首期按部分月折旧
  t = engine.TryEvaluate("DB(1000000, 100000, 6, 1, 7)", 0.1);
  assert.ok(engine.LastError === null && t > 0, "DB(1000000, 100000, 6, 1, 7) 应该成功且结果大于 0");

  console.log('DB_month 测试通过！');
}

function testXNPV() {
  console.log('开始测试 XNPV...');

  const engine = new AlgorithmEngineExWithTryEvaluate();
  engine.AddParameter('values', [-10000, 2750, 4250, 3250, 2750]);
  engine.AddParameter('dates2', ['2008-1-1', '2008-3-1', '2008-10-30', '2009-2-15', '2009-4-1']);
  let t = engine.TryEvaluate("XNPV(0.09, values, dates2)", 0.1);
  assert.strictEqual(roundTo(t, 2), roundTo(2086.65, 2), "XNPV(0.09, values, dates2) 应该约等于 2086.65");

  console.log('XNPV 测试通过！');
}

function testXIRR() {
  console.log('开始测试 XIRR...');

  const engine = new AlgorithmEngineExWithTryEvaluate();
  engine.AddParameter('values', [-10000, 2750, 4250, 3250, 2750]);
  engine.AddParameter('dates2', ['2008-1-1', '2008-3-1', '2008-10-30', '2009-2-15', '2009-4-1']);
  let t = engine.TryEvaluate("XIRR(values, dates2)", 0.1);
  assert.strictEqual(roundTo(t, 4), roundTo(0.3734, 4), "XIRR(values, dates2) 应该约等于 0.3734");

  console.log('XIRR 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testPMT();
    testPPMT();
    testIPMT();
    testPV();
    testFV();
    testNPER();
    testRATE();
    testNPV();
    testIRR();
    testMIRR();
    testSLN();
    testSYD();
    testDDB();
    testDB();
    testDB_month();
    testXNPV();
    testXIRR();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testPMT,
  testPPMT,
  testIPMT,
  testPV,
  testFV,
  testNPER,
  testRATE,
  testNPV,
  testIRR,
  testMIRR,
  testSLN,
  testSYD,
  testDDB,
  testDB,
  testDB_month,
  testXNPV,
  testXIRR,
  runAllTests
};
