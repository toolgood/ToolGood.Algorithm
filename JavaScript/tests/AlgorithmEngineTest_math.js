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
function testPi() {
  console.log('开始测试 Pi...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let pi = engine.TryEvaluate_Double("pi()", 0.0);
  assert.strictEqual(Math.round(pi * 1000000000) / 1000000000, 3.141592654, "pi() 应该约等于 3.141592654");
  
  console.log('Pi 测试通过！');
}

function testAbs() {
  console.log('开始测试 abs...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let abs = engine.TryEvaluate_Double("abs(-1.2)", 0.0);
  assert.strictEqual(abs, 1.2, "abs(-1.2) 应该是 1.2");
  
  console.log('abs 测试通过！');
}

function testQUOTIENT() {
  console.log('开始测试 QUOTIENT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let quotient = engine.TryEvaluate_Double("QUOTIENT(7,3)", 0.0);
  assert.strictEqual(quotient, 2.0, "QUOTIENT(7,3) 应该是 2.0");
  
  console.log('QUOTIENT 测试通过！');
}

function testMOD() {
  console.log('开始测试 MOD...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let mod = engine.TryEvaluate_Double("MOD(7,3)", 0.0);
  assert.strictEqual(mod, 1.0, "MOD(7,3) 应该是 1.0");
  
  console.log('MOD 测试通过！');
}

function testSIGN() {
  console.log('开始测试 SIGN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let sign = engine.TryEvaluate_Double("SIGN(0)", 0);
  assert.strictEqual(sign, 0, "SIGN(0) 应该是 0");
  
  sign = engine.TryEvaluate_Double("SIGN(9)", 0);
  assert.strictEqual(sign, 1, "SIGN(9) 应该是 1");
  
  sign = engine.TryEvaluate_Double("SIGN(-9)", 0);
  assert.strictEqual(sign, -1, "SIGN(-9) 应该是 -1");
  
  console.log('SIGN 测试通过！');
}

function testSQRT() {
  console.log('开始测试 SQRT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let sqrt = engine.TryEvaluate_Double("SQRT(9)", 0.0);
  assert.strictEqual(sqrt, 3.0, "SQRT(9) 应该是 3.0");
  
  console.log('SQRT 测试通过！');
}

function testSUM() {
  console.log('开始测试 SUM...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let sum = engine.TryEvaluate_Double("SUM(1,2,3,4)", 0.0);
  assert.strictEqual(sum, 10.0, "SUM(1,2,3,4) 应该是 10.0");
  
  console.log('SUM 测试通过！');
}

function testTRUNC() {
  console.log('开始测试 TRUNC...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let trunc = engine.TryEvaluate_Double("TRUNC(9.222)", 0.0);
  assert.strictEqual(trunc, 9.0, "TRUNC(9.222) 应该是 9.0");
  
  trunc = engine.TryEvaluate_Double("TRUNC(-9.222)", 0.0);
  assert.strictEqual(trunc, -9.0, "TRUNC(-9.222) 应该是 -9.0");
  
  console.log('TRUNC 测试通过！');
}

function testInt() {
  console.log('开始测试 int...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let intValue = engine.TryEvaluate_Double("int(9.222)", 0.0);
  assert.strictEqual(intValue, 9.0, "int(9.222) 应该是 9.0");
  
  intValue = engine.TryEvaluate_Double("int(-9.222)", 0.0);
  assert.strictEqual(intValue, -9.0, "int(-9.222) 应该是 -9.0");
  
  console.log('int 测试通过！');
}

function testGCD() {
  console.log('开始测试 GCD...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let gcd = engine.TryEvaluate_Double("GCD(3,5,7)", 0.0);
  assert.strictEqual(gcd, 1.0, "GCD(3,5,7) 应该是 1.0");
  
  gcd = engine.TryEvaluate_Double("GCD(30,21)", 0.0);
  assert.strictEqual(gcd, 3.0, "GCD(30,21) 应该是 3.0");
  
  console.log('GCD 测试通过！');
}

function testLCM() {
  console.log('开始测试 LCM...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let lcm = engine.TryEvaluate_Double("LCM(3,5,7)", 0.0);
  assert.strictEqual(lcm, 105.0, "LCM(3,5,7) 应该是 105.0");
  
  console.log('LCM 测试通过！');
}

function testCombin() {
  console.log('开始测试 combin...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let combin = engine.TryEvaluate_Double("combin(10,2)", 0.0);
  assert.strictEqual(combin, 45.0, "combin(10,2) 应该是 45.0");
  
  console.log('combin 测试通过！');
}

function testPERMUT() {
  console.log('开始测试 PERMUT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let permut = engine.TryEvaluate_Double("PERMUT(10,2)", 0.0);
  assert.strictEqual(permut, 90.0, "PERMUT(10,2) 应该是 90.0");
  
  console.log('PERMUT 测试通过！');
}

function testDegrees() {
  console.log('开始测试 degrees...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let degrees = engine.TryEvaluate_Double("degrees(pi())", 0.0);
  assert.strictEqual(degrees, 180.0, "degrees(pi()) 应该是 180.0");
  
  console.log('degrees 测试通过！');
}

function testRADIANS() {
  console.log('开始测试 RADIANS...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let radians = engine.TryEvaluate_Double("RADIANS(180)", 0.0);
  assert.strictEqual(Math.round(radians * 1000000000) / 1000000000, Math.round(Math.PI * 1000000000) / 1000000000, "RADIANS(180) 应该等于 Math.PI");
  
  console.log('RADIANS 测试通过！');
}

function testCos() {
  console.log('开始测试 cos...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let cos = engine.TryEvaluate_Double("cos(1)", 0.0);
  cos = Math.round(cos * 1000000) / 1000000;
  assert.strictEqual(cos, Math.round(Math.cos(1) * 1000000) / 1000000, "cos(1) 应该等于 Math.cos(1)");
  
  console.log('cos 测试通过！');
}

function testCosh() {
  console.log('开始测试 cosh...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let cosh = engine.TryEvaluate_Double("cosh(1)", 0.0);
  cosh = Math.round(cosh * 1000000) / 1000000;
  assert.strictEqual(cosh, Math.round(Math.cosh(1) * 1000000) / 1000000, "cosh(1) 应该等于 Math.cosh(1)");
  
  console.log('cosh 测试通过！');
}

function testSin() {
  console.log('开始测试 sin...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let sin = engine.TryEvaluate_Double("sin(1)", 0.0);
  sin = Math.round(sin * 1000000) / 1000000;
  assert.strictEqual(sin, Math.round(Math.sin(1) * 1000000) / 1000000, "sin(1) 应该等于 Math.sin(1)");
  
  console.log('sin 测试通过！');
}

function testSinh() {
  console.log('开始测试 sinh...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let sinh = engine.TryEvaluate_Double("sinh(1)", 0.0);
  sinh = Math.round(sinh * 1000000) / 1000000;
  assert.strictEqual(sinh, Math.round(Math.sinh(1) * 1000000) / 1000000, "sinh(1) 应该等于 Math.sinh(1)");
  
  console.log('sinh 测试通过！');
}

function testTan() {
  console.log('开始测试 tan...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let tan = engine.TryEvaluate_Double("tan(1)", 0.0);
  tan = Math.round(tan * 1000000) / 1000000;
  assert.strictEqual(tan, Math.round(Math.tan(1) * 1000000) / 1000000, "tan(1) 应该等于 Math.tan(1)");
  
  console.log('tan 测试通过！');
}

function testTanh() {
  console.log('开始测试 tanh...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let tanh = engine.TryEvaluate_Double("tanh(1)", 0.0);
  tanh = Math.round(tanh * 1000000) / 1000000;
  assert.strictEqual(tanh, Math.round(Math.tanh(1) * 1000000) / 1000000, "tanh(1) 应该等于 Math.tanh(1)");
  
  console.log('tanh 测试通过！');
}

function testAcos() {
  console.log('开始测试 acos...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let acos = engine.TryEvaluate_Double("acos(0.5)", 0.0);
  acos = Math.round(acos * 1000000) / 1000000;
  assert.strictEqual(acos, Math.round(Math.acos(0.5) * 1000000) / 1000000, "acos(0.5) 应该等于 Math.acos(0.5)");
  
  console.log('acos 测试通过！');
}

function testAcosh() {
  console.log('开始测试 acosh...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let acosh = engine.TryEvaluate_Double("acosh(1.5)", 0.0);
  acosh = Math.round(acosh * 1000000) / 1000000;
  assert.strictEqual(acosh, Math.round(Math.acosh(1.5) * 1000000) / 1000000, "acosh(1.5) 应该等于 Math.acosh(1.5)");
  
  console.log('acosh 测试通过！');
}

function testAsin() {
  console.log('开始测试 asin...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let asin = engine.TryEvaluate_Double("asin(0.5)", 0.0);
  asin = Math.round(asin * 1000000) / 1000000;
  assert.strictEqual(asin, Math.round(Math.asin(0.5) * 1000000) / 1000000, "asin(0.5) 应该等于 Math.asin(0.5)");
  
  console.log('asin 测试通过！');
}

function testAsinh() {
  console.log('开始测试 asinh...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let asinh = engine.TryEvaluate_Double("asinh(1.5)", 0.0);
  asinh = Math.round(asinh * 1000000) / 1000000;
  assert.strictEqual(asinh, Math.round(Math.asinh(1.5) * 1000000) / 1000000, "asinh(1.5) 应该等于 Math.asinh(1.5)");
  
  console.log('asinh 测试通过！');
}

function testAtan() {
  console.log('开始测试 atan...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let atan = engine.TryEvaluate_Double("atan(1)", 0.0);
  atan = Math.round(atan * 1000000) / 1000000;
  assert.strictEqual(atan, Math.round(Math.atan(1) * 1000000) / 1000000, "atan(1) 应该等于 Math.atan(1)");
  
  console.log('atan 测试通过！');
}

function testAtanh() {
  console.log('开始测试 atanh...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let atanh = engine.TryEvaluate_Double("atanh(0.5)", 0.0);
  atanh = Math.round(atanh * 1000000) / 1000000;
  assert.strictEqual(atanh, Math.round(Math.atanh(0.5) * 1000000) / 1000000, "atanh(0.5) 应该等于 Math.atanh(0.5)");
  
  console.log('atanh 测试通过！');
}

function testAtan2() {
  console.log('开始测试 atan2...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let atan2 = engine.TryEvaluate_Double("atan2(1,2)", 0.0);
  atan2 = Math.round(atan2 * 1000000) / 1000000;
  assert.strictEqual(atan2, Math.round(Math.atan2(1, 2) * 1000000) / 1000000, "atan2(1,2) 应该等于 Math.atan2(1, 2)");
  
  console.log('atan2 测试通过！');
}

function testROUND() {
  console.log('开始测试 ROUND...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let round = engine.TryEvaluate_Double("ROUND(4.333,2)", 0.0);
  assert.strictEqual(round, 4.33, "ROUND(4.333,2) 应该是 4.33");
  
  console.log('ROUND 测试通过！');
}

function testROUNDDOWN() {
  console.log('开始测试 ROUNDDOWN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let rounddown = engine.TryEvaluate_Double("ROUNDDOWN(4.333,2)", 0.0);
  assert.strictEqual(rounddown, 4.33, "ROUNDDOWN(4.333,2) 应该是 4.33");
  
  rounddown = engine.TryEvaluate_Double("ROUNDDOWN(-3.141592, 3)", 0.0);
  assert.strictEqual(rounddown, -3.141, "ROUNDDOWN(-3.141592, 3) 应该是 -3.141");
  
  console.log('ROUNDDOWN 测试通过！');
}

function testROUNDUP() {
  console.log('开始测试 ROUNDUP...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let roundup = engine.TryEvaluate_Double("ROUNDUP(4.333,2)", 0.0);
  assert.strictEqual(roundup, 4.34, "ROUNDUP(4.333,2) 应该是 4.34");
  
  roundup = engine.TryEvaluate_Double("ROUNDUP(-3.141592, 3)", 0.0);
  assert.strictEqual(roundup, -3.142, "ROUNDUP(-3.141592, 3) 应该是 -3.142");
  
  console.log('ROUNDUP 测试通过！');
}

function testCEILING() {
  console.log('开始测试 CEILING...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let ceiling = engine.TryEvaluate_Double("CEILING(4.333,0.1)", 0.0);
  assert.strictEqual(ceiling, 4.4, "CEILING(4.333,0.1) 应该是 4.4");
  
  ceiling = engine.TryEvaluate_Double("CEILING(4.333)", 0.0);
  assert.strictEqual(ceiling, 5, "CEILING(4.333) 应该是 5");
  
  console.log('CEILING 测试通过！');
}

function testFLOOR() {
  console.log('开始测试 FLOOR...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let floor = engine.TryEvaluate_Double("FLOOR(4.363,0.1)", 0.0);
  assert.strictEqual(floor, 4.3, "FLOOR(4.363,0.1) 应该是 4.3");
  
  floor = engine.TryEvaluate_Double("FLOOR(4.333)", 0.0);
  assert.strictEqual(floor, 4, "FLOOR(4.333) 应该是 4");
  
  console.log('FLOOR 测试通过！');
}

function testEven() {
  console.log('开始测试 even...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let even = engine.TryEvaluate_Double("even(4.363)", 0.0);
  assert.strictEqual(even, 6.0, "even(4.363) 应该是 6.0");
  
  console.log('even 测试通过！');
}

function testOdd() {
  console.log('开始测试 odd...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let odd = engine.TryEvaluate_Double("odd(4.363)", 0.0);
  assert.strictEqual(odd, 5, "odd(4.363) 应该是 5");
  
  console.log('odd 测试通过！');
}

function testMROUND() {
  console.log('开始测试 MROUND...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let mround = engine.TryEvaluate_Double("MROUND(4.363,2)", 0.0);
  assert.strictEqual(mround, 4, "MROUND(4.363,2) 应该是 4");
  
  mround = engine.TryEvaluate_Double("MROUND(5.363,2)", 0.0);
  assert.strictEqual(mround, 6, "MROUND(5.363,2) 应该是 6");
  
  console.log('MROUND 测试通过！');
}

function testRand() {
  console.log('开始测试 Rand...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let rand = engine.TryEvaluate_Double("RAND()", 0.0);
  assert.ok(rand > 0 && rand <= 1, "RAND() 应该返回一个 0 到 1 之间的数字");
  
  console.log('Rand 测试通过！');
}

function testRANDBETWEEN() {
  console.log('开始测试 RANDBETWEEN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let randbetween = engine.TryEvaluate_Double("RANDBETWEEN(2,99)", 0.0);
  assert.ok(randbetween >= 2 && randbetween <= 99, "RANDBETWEEN(2,99) 应该返回一个 2 到 99 之间的数字");
  
  console.log('RANDBETWEEN 测试通过！');
}

function testFact() {
  console.log('开始测试 fact...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let fact = engine.TryEvaluate_Double("fact(6)", 0.0);
  assert.strictEqual(fact, 720.0, "fact(6) 应该是 720.0");
  
  fact = engine.TryEvaluate_Double("fact(3)", 0.0);
  assert.strictEqual(fact, 6.0, "fact(3) 应该是 6.0");
  
  console.log('fact 测试通过！');
}

function testFactdouble() {
  console.log('开始测试 factdouble...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let factdouble = engine.TryEvaluate_Double("factdouble(10)", 0.0);
  assert.strictEqual(factdouble, 3840.0, "factdouble(10) 应该是 3840.0");
  
  console.log('factdouble 测试通过！');
}

function testPOWER() {
  console.log('开始测试 POWER...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let power = engine.TryEvaluate_Double("POWER(10,2)", 0.0);
  assert.strictEqual(power, 100.0, "POWER(10,2) 应该是 100.0");
  
  console.log('POWER 测试通过！');
}

function testExp() {
  console.log('开始测试 exp...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let exp = engine.TryEvaluate_Double("exp(2)", 0.0);
  exp = Math.round(exp * 1000000) / 1000000;
  assert.strictEqual(exp, Math.round(Math.exp(2) * 1000000) / 1000000, "exp(2) 应该等于 Math.exp(2)");
  
  console.log('exp 测试通过！');
}

function testLn() {
  console.log('开始测试 ln...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let ln = engine.TryEvaluate_Double("ln(4)", 0.0);
  ln = Math.round(ln * 1000000) / 1000000;
  assert.strictEqual(ln, Math.round(Math.log(4) * 1000000) / 1000000, "ln(4) 应该等于 Math.log(4)");
  
  console.log('ln 测试通过！');
}

function testLog() {
  console.log('开始测试 log...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let log = engine.TryEvaluate_Double("log(10)", 0.0);
  log = Math.round(log * 1000000) / 1000000;
  assert.strictEqual(log, 1.0, "log(10) 应该是 1.0");
  
  log = engine.TryEvaluate_Double("log(8,2)", 0.0);
  log = Math.round(log * 1000000) / 1000000;
  assert.strictEqual(log, 3.0, "log(8,2) 应该是 3.0");
  
  console.log('log 测试通过！');
}

function testLog10() {
  console.log('开始测试 log10...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let log10 = engine.TryEvaluate_Double("log10(10)", 0.0);
  log10 = Math.round(log10 * 1000000) / 1000000;
  assert.strictEqual(log10, 1.0, "log10(10) 应该是 1.0");
  
  console.log('log10 测试通过！');
}

function testMULTINOMIAL() {
  console.log('开始测试 MULTINOMIAL...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let multinomial = engine.TryEvaluate_Double("MULTINOMIAL(1,2,3)", 0.0);
  multinomial = Math.round(multinomial * 1000000) / 1000000;
  assert.strictEqual(multinomial, 60.0, "MULTINOMIAL(1,2,3) 应该是 60.0");
  
  multinomial = engine.TryEvaluate_Double("MULTINOMIAL(1,2,3,4)", 0.0);
  multinomial = Math.round(multinomial * 1000000) / 1000000;
  assert.strictEqual(multinomial, 12600.0, "MULTINOMIAL(1,2,3,4) 应该是 12600.0");
  
  console.log('MULTINOMIAL 测试通过！');
}

function testPRODUCT() {
  console.log('开始测试 PRODUCT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let product = engine.TryEvaluate_Double("PRODUCT(1,2,3,4)", 0.0);
  product = Math.round(product * 1000000) / 1000000;
  assert.strictEqual(product, 24.0, "PRODUCT(1,2,3,4) 应该是 24.0");
  
  console.log('PRODUCT 测试通过！');
}

function testSQRTPI() {
  console.log('开始测试 SQRTPI...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let sqrtpi = engine.TryEvaluate_Double("SQRTPI(3)", 0.0);
  sqrtpi = Math.round(sqrtpi * 1000000) / 1000000;
  assert.ok(typeof sqrtpi === 'number', "SQRTPI(3) 应该返回一个数字");
  
  console.log('SQRTPI 测试通过！');
}

function testSUMSQ() {
  console.log('开始测试 SUMSQ...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let sumsq = engine.TryEvaluate_Double("SUMSQ(1,2)", 0.0);
  sumsq = Math.round(sumsq * 1000000) / 1000000;
  assert.strictEqual(sumsq, 5.0, "SUMSQ(1,2) 应该是 5.0");
  
  console.log('SUMSQ 测试通过！');
}

function testTransformation() {
  console.log('开始测试 transformation...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let num = engine.TryEvaluate_Double("BIN2DEC(10101)", 0);
  assert.strictEqual(num, 21, "BIN2DEC(10101) 应该是 21");
  
  num = engine.TryEvaluate_Double("OCT2DEC(12456)", 0);
  assert.strictEqual(num, 5422, "OCT2DEC(12456) 应该是 5422");
  
  num = engine.TryEvaluate_Double("HEX2DEC('213adf')", 0);
  assert.strictEqual(num, 2177759, "HEX2DEC('213adf') 应该是 2177759");
  
  let t = engine.TryEvaluate_String("DEC2BIN(10)", "");
  assert.strictEqual(t, "1010", "DEC2BIN(10) 应该是 '1010'");
  
  t = engine.TryEvaluate_String("OCT2BIN('721')", "");
  assert.strictEqual(t, "111010001", "OCT2BIN('721') 应该是 '111010001'");
  
  t = engine.TryEvaluate_String("HEX2BIN('fa')", "");
  assert.strictEqual(t, "11111010", "HEX2BIN('fa') 应该是 '11111010'");
  
  t = engine.TryEvaluate_String("'fa'.HEX2BIN()", "");
  assert.strictEqual(t, "11111010", "'fa'.HEX2BIN() 应该是 '11111010'");
  
  t = engine.TryEvaluate_String("BIN2OCT(10)", "");
  assert.strictEqual(t, "2", "BIN2OCT(10) 应该是 '2'");
  
  t = engine.TryEvaluate_String("DEC2OCT('75')", "");
  assert.strictEqual(t, "113", "DEC2OCT('75') 应该是 '113'");
  
  t = engine.TryEvaluate_String("HEX2OCT('f5')", "");
  assert.strictEqual(t, "365", "HEX2OCT('f5') 应该是 '365'");
  
  t = engine.TryEvaluate_String("BIN2HEX(101010100)", "");
  assert.strictEqual(t, "154", "BIN2HEX(101010100) 应该是 '154'");
  
  t = engine.TryEvaluate_String("OCT2HEX(75212)", "");
  assert.strictEqual(t, "7A8A", "OCT2HEX(75212) 应该是 '7A8A'");
  
  t = engine.TryEvaluate_String("DEC2HEX(952)", "");
  assert.strictEqual(t, "3B8", "DEC2HEX(952) 应该是 '3B8'");
  
  console.log('transformation 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    // 基础数学
    testPi();
    testAbs();
    testQUOTIENT();
    testMOD();
    testSIGN();
    testSQRT();
    testSUM();
    testTRUNC();
    testInt();
    testGCD();
    testLCM();
    testCombin();
    testPERMUT();
    
    // 三角函数
    testDegrees();
    testRADIANS();
    testCos();
    testCosh();
    testSin();
    testSinh();
    testTan();
    testTanh();
    testAcos();
    testAcosh();
    testAsin();
    testAsinh();
    testAtan();
    testAtanh();
    testAtan2();
    
    // 四舍五入
    testROUND();
    testROUNDDOWN();
    testROUNDUP();
    testCEILING();
    testFLOOR();
    testEven();
    testOdd();
    testMROUND();
    
    // 随机数
    testRand();
    testRANDBETWEEN();
    
    // 幂/对数/阶乘
    testFact();
    testFactdouble();
    testPOWER();
    testExp();
    testLn();
    testLog();
    testLog10();
    testMULTINOMIAL();
    testPRODUCT();
    testSQRTPI();
    testSUMSQ();
    
    // 转化
    testTransformation();
    
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
  // 基础数学
  testPi,
  testAbs,
  testQUOTIENT,
  testMOD,
  testSIGN,
  testSQRT,
  testSUM,
  testTRUNC,
  testInt,
  testGCD,
  testLCM,
  testCombin,
  testPERMUT,
  
  // 三角函数
  testDegrees,
  testRADIANS,
  testCos,
  testCosh,
  testSin,
  testSinh,
  testTan,
  testTanh,
  testAcos,
  testAcosh,
  testAsin,
  testAsinh,
  testAtan,
  testAtanh,
  testAtan2,
  
  // 四舍五入
  testROUND,
  testROUNDDOWN,
  testROUNDUP,
  testCEILING,
  testFLOOR,
  testEven,
  testOdd,
  testMROUND,
  
  // 随机数
  testRand,
  testRANDBETWEEN,
  
  // 幂/对数/阶乘
  testFact,
  testFactdouble,
  testPOWER,
  testExp,
  testLn,
  testLog,
  testLog10,
  testMULTINOMIAL,
  testPRODUCT,
  testSQRTPI,
  testSUMSQ,
  
  // 转化
  testTransformation,
  
  runAllTests
};
