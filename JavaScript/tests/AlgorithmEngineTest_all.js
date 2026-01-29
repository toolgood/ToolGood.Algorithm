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

// 扩展 AlgorithmEngineEx 类以添加 TryEvaluate 方法
class AlgorithmEngineExWithTryEvaluate extends AlgorithmEngineEx {
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

// Cylinder 类
class Cylinder extends AlgorithmEngineExWithTryEvaluate {
  constructor(radius, height) {
    super();
    this.radius = radius;
    this.height = height;
  }

  GetParameter(parameter) {
    switch (parameter.toLowerCase()) {
      case '半径':
      case 'radius':
        return super.TryEvaluate(this.radius, 0);
      case '高':
      case 'height':
        return super.TryEvaluate(this.height, 0);
      case '直径':
      case 'diameter':
        return super.TryEvaluate(this.radius * 2, 0);
      default:
        return super.GetParameter(parameter);
    }
  }

  ExecuteDiyFunction(parameter, args) {
    if (parameter === '求面积') {
      const r = args[0].ToNumber('');
      if (!r.IsError) {
        return super.TryEvaluate(r * r * Math.PI, 0);
      }
    }
    return super.ExecuteDiyFunction(parameter, args);
  }
}

// 测试用例
function testDateTime() {
  console.log('开始测试 DateTime 函数...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  // DATEVALUE_Test
  let dt = engine.TryEvaluate("DATEVALUE('2016-01-01')", new Date(0));
  assert.strictEqual(dt.getFullYear(), 2016, "DATEVALUE('2016-01-01') 年份应该是 2016");
  assert.strictEqual(dt.getMonth() + 1, 1, "DATEVALUE('2016-01-01') 月份应该是 1");
  assert.strictEqual(dt.getDate(), 1, "DATEVALUE('2016-01-01') 日期应该是 1");
  
  dt = engine.TryEvaluate("DATEVALUE（'2016-01-01'）", new Date(0));
  assert.strictEqual(dt.getFullYear(), 2016, "DATEVALUE（'2016-01-01'） 年份应该是 2016");
  assert.strictEqual(dt.getMonth() + 1, 1, "DATEVALUE（'2016-01-01'） 月份应该是 1");
  assert.strictEqual(dt.getDate(), 1, "DATEVALUE（'2016-01-01'） 日期应该是 1");
  
  dt = engine.TryEvaluate("DATEVALUE('2016/01/01')", new Date(0));
  assert.strictEqual(dt.getFullYear(), 2016, "DATEVALUE('2016/01/01') 年份应该是 2016");
  assert.strictEqual(dt.getMonth() + 1, 1, "DATEVALUE('2016/01/01') 月份应该是 1");
  assert.strictEqual(dt.getDate(), 1, "DATEVALUE('2016/01/01') 日期应该是 1");
  
  // TIMESTAMP_Test
  engine.UseLocalTime = true;
  let timestamp = engine.TryEvaluate("TIMESTAMP('2016-01-01')", 0);
  assert.strictEqual(timestamp, 1451577600000, "TIMESTAMP('2016-01-01') 应该是 1451577600000");
  
  timestamp = engine.TryEvaluate("TIMESTAMP('2016/01/01')", 0);
  assert.strictEqual(timestamp, 1451577600000, "TIMESTAMP('2016/01/01') 应该是 1451577600000");
  
  timestamp = engine.TryEvaluate("TIMESTAMP('2016-01-01',0)", 0);
  assert.strictEqual(timestamp, 1451577600000, "TIMESTAMP('2016-01-01',0) 应该是 1451577600000");
  
  timestamp = engine.TryEvaluate("TIMESTAMP('2016-01-01',1)", 0);
  assert.strictEqual(timestamp, 1451577600, "TIMESTAMP('2016-01-01',1) 应该是 1451577600");
  
  // TIMEVALUE_test
  let time = engine.TryEvaluate("TIMEVALUE('12:12:12')", 0);
  assert.strictEqual(time, 0.5084722222222222, "TIMEVALUE('12:12:12') 应该是 0.5084722222222222");
  
  // DATE_test
  dt = engine.TryEvaluate("DATE(2016,1,1)", new Date(0));
  assert.strictEqual(dt.getFullYear(), 2016, "DATE(2016,1,1) 年份应该是 2016");
  assert.strictEqual(dt.getMonth() + 1, 1, "DATE(2016,1,1) 月份应该是 1");
  assert.strictEqual(dt.getDate(), 1, "DATE(2016,1,1) 日期应该是 1");
  
  // time_test
  time = engine.TryEvaluate("time(11,12,13)", 0);
  assert.strictEqual(time, 0.4668634259259259, "time(11,12,13) 应该是 0.4668634259259259");
  
  // Now_test
  dt = engine.TryEvaluate("now()", new Date(0));
  assert.ok(dt instanceof Date, "now() 应该返回一个日期对象");
  
  // Today_test
  dt = engine.TryEvaluate("Today()", new Date(0));
  assert.ok(dt instanceof Date, "Today() 应该返回一个日期对象");
  
  // Year_test
  let year = engine.TryEvaluate("year(now())", 0);
  assert.ok(typeof year === 'number', "year(now()) 应该返回一个数字");
  
  // Month_test
  let month = engine.TryEvaluate("Month(now())", 0);
  assert.ok(typeof month === 'number', "Month(now()) 应该返回一个数字");
  
  // Day_test
  let day = engine.TryEvaluate("Day(now())", 0);
  assert.ok(typeof day === 'number', "Day(now()) 应该返回一个数字");
  
  // Hour_test
  let hour = engine.TryEvaluate("Hour(now())", 0);
  assert.ok(typeof hour === 'number', "Hour(now()) 应该返回一个数字");
  
  // Minute_test
  let minute = engine.TryEvaluate("Minute(now())", 0);
  assert.ok(typeof minute === 'number', "Minute(now()) 应该返回一个数字");
  
  // Second_test
  let second = engine.TryEvaluate("Second(now())", 0);
  assert.ok(typeof second === 'number', "Second(now()) 应该返回一个数字");
  
  // WEEKDAY_test
  let weekday = engine.TryEvaluate("WEEKDAY(date(2017,1,7))", 0);
  assert.strictEqual(weekday, 7, "WEEKDAY(date(2017,1,7)) 应该是 7");
  
  weekday = engine.TryEvaluate("WEEKDAY(date(2017,1,7),1)", 0);
  assert.strictEqual(weekday, 7, "WEEKDAY(date(2017,1,7),1) 应该是 7");
  
  weekday = engine.TryEvaluate("WEEKDAY(date(2017,1,7),2)", 0);
  assert.strictEqual(weekday, 6, "WEEKDAY(date(2017,1,7),2) 应该是 6");
  
  weekday = engine.TryEvaluate("WEEKDAY(date(2017,1,7),3)", 0);
  assert.strictEqual(weekday, 5, "WEEKDAY(date(2017,1,7),3) 应该是 5");
  
  // DATEDIF_Test
  let datedif = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0);
  assert.strictEqual(datedif, 41, "DATEDIF('1975-1-30','2017-1-7','y') 应该是 41");
  
  datedif = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','m')", 0);
  assert.strictEqual(datedif, 503, "DATEDIF('1975-1-30','2017-1-7','m') 应该是 503");
  
  datedif = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','d')", 0);
  assert.strictEqual(datedif, 15318, "DATEDIF('1975-1-30','2017-1-7','d') 应该是 15318");
  
  // DAYS360_test
  let days360 = engine.TryEvaluate("DAYS360('1975-1-30','2017-1-7')", 0);
  assert.strictEqual(days360, 15097, "DAYS360('1975-1-30','2017-1-7') 应该是 15097");
  
  // EDATE_test
  dt = engine.TryEvaluate("EDATE(\"2012-1-31\",32)", new Date(0));
  assert.strictEqual(dt.getFullYear(), 2014, "EDATE(\"2012-1-31\",32) 年份应该是 2014");
  assert.strictEqual(dt.getMonth() + 1, 9, "EDATE(\"2012-1-31\",32) 月份应该是 9");
  assert.strictEqual(dt.getDate(), 30, "EDATE(\"2012-1-31\",32) 日期应该是 30");
  
  // EOMONTH_test
  dt = engine.TryEvaluate("EOMONTH(\"2012-2-1\",32)", new Date(0));
  assert.strictEqual(dt.getFullYear(), 2014, "EOMONTH(\"2012-2-1\",32) 年份应该是 2014");
  assert.strictEqual(dt.getMonth() + 1, 10, "EOMONTH(\"2012-2-1\",32) 月份应该是 10");
  assert.strictEqual(dt.getDate(), 31, "EOMONTH(\"2012-2-1\",32) 日期应该是 31");
  
  // NETWORKDAYS_test
  let networkdays = engine.TryEvaluate("NETWORKDAYS(\"2012-1-1\",\"2013-1-1\")", 0);
  assert.strictEqual(networkdays, 262, "NETWORKDAYS(\"2012-1-1\",\"2013-1-1\") 应该是 262");
  
  // WORKDAY_test
  dt = engine.TryEvaluate("WORKDAY(\"2012-1-2\",145)", new Date(0));
  assert.strictEqual(dt.getFullYear(), 2012, "WORKDAY(\"2012-1-2\",145) 年份应该是 2012");
  assert.strictEqual(dt.getMonth() + 1, 7, "WORKDAY(\"2012-1-2\",145) 月份应该是 7");
  assert.strictEqual(dt.getDate(), 23, "WORKDAY(\"2012-1-2\",145) 日期应该是 23");
  
  // WEEKNUM_test
  let weeknum = engine.TryEvaluate("WEEKNUM(\"2016-1-3\")", 0);
  assert.strictEqual(weeknum, 2, "WEEKNUM(\"2016-1-3\") 应该是 2");
  
  weeknum = engine.TryEvaluate("WEEKNUM(\"2016-1-2\")", 0);
  assert.strictEqual(weeknum, 1, "WEEKNUM(\"2016-1-2\") 应该是 1");
  
  // Add_test
  let addResult = engine.TryEvaluate("'2000-02-01'.addYears(1).year()", 0);
  assert.strictEqual(addResult, 2001, "'2000-02-01'.addYears(1).year() 应该是 2001");
  
  addResult = engine.TryEvaluate("'2000/02/01'.addYears(1).year()", 0);
  assert.strictEqual(addResult, 2001, "'2000/02/01'.addYears(1).year() 应该是 2001");
  
  addResult = engine.TryEvaluate("'2000-02-01'.AddMonths(1).Month()", 0);
  assert.strictEqual(addResult, 3, "'2000-02-01'.AddMonths(1).Month() 应该是 3");
  
  addResult = engine.TryEvaluate("'2000-02-01'.AddDays(1).Day()", 0);
  assert.strictEqual(addResult, 2, "'2000-02-01'.AddDays(1).Day() 应该是 2");
  
  console.log('DateTime 函数测试通过！');
}

function testMath() {
  console.log('开始测试 Math 函数...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  // Pi_test
  let pi = engine.TryEvaluate("pi()", 0);
  assert.strictEqual(Math.round(pi * 1000000000) / 1000000000, 3.141592654, "pi() 应该约等于 3.141592654");
  
  // abs_test
  let abs = engine.TryEvaluate("abs(-1.2)", 0);
  assert.strictEqual(abs, 1.2, "abs(-1.2) 应该是 1.2");
  
  // QUOTIENT_test
  let quotient = engine.TryEvaluate("QUOTIENT(7,3)", 0);
  assert.strictEqual(quotient, 2, "QUOTIENT(7,3) 应该是 2");
  
  // MOD_test
  let mod = engine.TryEvaluate("MOD(7,3)", 0);
  assert.strictEqual(mod, 1, "MOD(7,3) 应该是 1");
  
  // SIGN_test
  let sign = engine.TryEvaluate("SIGN(0)", 0);
  assert.strictEqual(sign, 0, "SIGN(0) 应该是 0");
  
  sign = engine.TryEvaluate("SIGN(9)", 0);
  assert.strictEqual(sign, 1, "SIGN(9) 应该是 1");
  
  sign = engine.TryEvaluate("SIGN(-9)", 0);
  assert.strictEqual(sign, -1, "SIGN(-9) 应该是 -1");
  
  // SQRT_test
  let sqrt = engine.TryEvaluate("SQRT(9)", 0);
  assert.strictEqual(sqrt, 3, "SQRT(9) 应该是 3");
  
  // SUM_test
  let sum = engine.TryEvaluate("SUM(1,2,3,4)", 0);
  assert.strictEqual(sum, 10, "SUM(1,2,3,4) 应该是 10");
  
  // TRUNC_test
  let trunc = engine.TryEvaluate("TRUNC(9.222)", 0);
  assert.strictEqual(trunc, 9, "TRUNC(9.222) 应该是 9");
  
  trunc = engine.TryEvaluate("TRUNC(-9.222)", 0);
  assert.strictEqual(trunc, -9, "TRUNC(-9.222) 应该是 -9");
  
  // int_test
  let intResult = engine.TryEvaluate("int(9.222)", 0);
  assert.strictEqual(intResult, 9, "int(9.222) 应该是 9");
  
  intResult = engine.TryEvaluate("int(-9.222)", 0);
  assert.strictEqual(intResult, -9, "int(-9.222) 应该是 -9");
  
  // GCD_test
  let gcd = engine.TryEvaluate("GCD(3,5,7)", 0);
  assert.strictEqual(gcd, 1, "GCD(3,5,7) 应该是 1");
  
  gcd = engine.TryEvaluate("GCD(30,21)", 0);
  assert.strictEqual(gcd, 3, "GCD(30,21) 应该是 3");
  
  // LCM_test
  let lcm = engine.TryEvaluate("LCM(3,5,7)", 0);
  assert.strictEqual(lcm, 105, "LCM(3,5,7) 应该是 105");
  
  // combin_test
  let combin = engine.TryEvaluate("combin(10,2)", 0);
  assert.strictEqual(combin, 45, "combin(10,2) 应该是 45");
  
  // PERMUT_test
  let permut = engine.TryEvaluate("PERMUT(10,2)", 0);
  assert.strictEqual(permut, 90, "PERMUT(10,2) 应该是 90");
  
  // degrees_test
  let degrees = engine.TryEvaluate("degrees(pi())", 0);
  assert.strictEqual(degrees, 180, "degrees(pi()) 应该是 180");
  
  // RADIANS_test
  let radians = engine.TryEvaluate("RADIANS(180)", 0);
  assert.strictEqual(radians, Math.PI, "RADIANS(180) 应该是 Math.PI");
  
  // cos_test
  let cos = engine.TryEvaluate("cos(1)", 0);
  assert.strictEqual(Math.round(cos * 1000000) / 1000000, Math.round(0.540302306 * 1000000) / 1000000, "cos(1) 应该约等于 0.540302306");
  
  // ROUND_test
  let round = engine.TryEvaluate("ROUND(4.333,2)", 0);
  assert.strictEqual(round, 4.33, "ROUND(4.333,2) 应该是 4.33");
  
  // ROUNDDOWN_test
  let rounddown = engine.TryEvaluate("ROUNDDOWN(4.333,2)", 0);
  assert.strictEqual(rounddown, 4.33, "ROUNDDOWN(4.333,2) 应该是 4.33");
  
  rounddown = engine.TryEvaluate("ROUNDDOWN(-3.141592, 3)", 0);
  assert.strictEqual(rounddown, -3.141, "ROUNDDOWN(-3.141592, 3) 应该是 -3.141");
  
  // ROUNDUP_test
  let roundup = engine.TryEvaluate("ROUNDUP(4.333,2)", 0);
  assert.strictEqual(roundup, 4.34, "ROUNDUP(4.333,2) 应该是 4.34");
  
  roundup = engine.TryEvaluate("ROUNDUP(-3.141592, 3)", 0);
  assert.strictEqual(roundup, -3.142, "ROUNDUP(-3.141592, 3) 应该是 -3.142");
  
  // CEILING_test
  let ceiling = engine.TryEvaluate("CEILING(4.333,0.1)", 0);
  assert.strictEqual(ceiling, 4.4, "CEILING(4.333,0.1) 应该是 4.4");
  
  ceiling = engine.TryEvaluate("CEILING(4.333)", 0);
  assert.strictEqual(ceiling, 5, "CEILING(4.333) 应该是 5");
  
  // FLOOR_test
  let floor = engine.TryEvaluate("FLOOR(4.363,0.1)", 0);
  assert.strictEqual(floor, 4.3, "FLOOR(4.363,0.1) 应该是 4.3");
  
  floor = engine.TryEvaluate("FLOOR(4.333)", 0);
  assert.strictEqual(floor, 4, "FLOOR(4.333) 应该是 4");
  
  // Rand_test
  let rand = engine.TryEvaluate("RAND()", 0);
  assert.ok(rand > 0 && rand <= 1, "RAND() 应该在 (0, 1] 之间");
  
  // RANDBETWEEN_test
  let randbetween = engine.TryEvaluate("RANDBETWEEN(2,99)", 0);
  assert.ok(randbetween >= 2 && randbetween <= 99, "RANDBETWEEN(2,99) 应该在 [2, 99] 之间");
  
  // fact_test
  let fact = engine.TryEvaluate("fact(6)", 0);
  assert.strictEqual(fact, 720, "fact(6) 应该是 720");
  
  // POWER_test
  let power = engine.TryEvaluate("POWER(10,2)", 0);
  assert.strictEqual(power, 100, "POWER(10,2) 应该是 100");
  
  // exp_test
  let exp = engine.TryEvaluate("exp(2)", 0);
  assert.strictEqual(Math.round(exp * 1000000) / 1000000, Math.round(7.389056099 * 1000000) / 1000000, "exp(2) 应该约等于 7.389056099");
  
  // ln_test
  let ln = engine.TryEvaluate("ln(4)", 0);
  assert.strictEqual(Math.round(ln * 1000000) / 1000000, Math.round(1.386294361 * 1000000) / 1000000, "ln(4) 应该约等于 1.386294361");
  
  // log_test
  let log = engine.TryEvaluate("log(10)", 0);
  assert.strictEqual(Math.round(log * 1000000) / 1000000, Math.round(1.0 * 1000000) / 1000000, "log(10) 应该约等于 1.0");
  
  log = engine.TryEvaluate("log(8,2)", 0);
  assert.strictEqual(Math.round(log * 1000000) / 1000000, Math.round(3.0 * 1000000) / 1000000, "log(8,2) 应该约等于 3.0");
  
  // log10_test
  let log10 = engine.TryEvaluate("log10(10)", 0);
  assert.strictEqual(Math.round(log10 * 1000000) / 1000000, Math.round(1.0 * 1000000) / 1000000, "log10(10) 应该约等于 1.0");
  
  // PRODUCT_test
  let product = engine.TryEvaluate("PRODUCT(1,2,3,4)", 0);
  assert.strictEqual(product, 24, "PRODUCT(1,2,3,4) 应该是 24");
  
  // SUMSQ_test
  let sumsq = engine.TryEvaluate("SUMSQ(1,2)", 0);
  assert.strictEqual(sumsq, 5, "SUMSQ(1,2) 应该是 5");
  
  // transformation_test
  let bin2dec = engine.TryEvaluate("BIN2DEC(10101)", 0);
  assert.strictEqual(bin2dec, 21, "BIN2DEC(10101) 应该是 21");
  
  let oct2dec = engine.TryEvaluate("OCT2DEC(12456)", 0);
  assert.strictEqual(oct2dec, 5422, "OCT2DEC(12456) 应该是 5422");
  
  let hex2dec = engine.TryEvaluate("HEX2DEC('213adf')", 0);
  assert.strictEqual(hex2dec, 2177759, "HEX2DEC('213adf') 应该是 2177759");
  
  console.log('Math 函数测试通过！');
}

function testString() {
  console.log('开始测试 String 函数...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  // ASC_test
  let asc = engine.TryEvaluate("asc('ａｂｃＡＢＣ１２３')", "");
  assert.strictEqual(asc, "abcABC123", "asc('ａｂｃＡＢＣ１２３') 应该是 'abcABC123'");
  
  // Jis_test
  let jis = engine.TryEvaluate("jis('abcABC123')", "");
  assert.strictEqual(jis, "ａｂｃＡＢＣ１２３", "jis('abcABC123') 应该是 'ａｂｃＡＢＣ１２３'");
  
  jis = engine.TryEvaluate("WIDECHAR('abcABC123')", "");
  assert.strictEqual(jis, "ａｂｃＡＢＣ１２３", "WIDECHAR('abcABC123') 应该是 'ａｂｃＡＢＣ１２３'");
  
  // CHAR_test
  let char = engine.TryEvaluate("char(49)", "");
  assert.strictEqual(char, "1", "char(49) 应该是 '1'");
  
  // CLEAN_test
  let clean = engine.TryEvaluate("clean('\r112\t')", "");
  assert.strictEqual(clean, "112", "clean('\\r112\\t') 应该是 '112'");
  
  // code_test
  let code = engine.TryEvaluate("code('1')", 0);
  assert.strictEqual(code, 49, "code('1') 应该是 49");
  
  // CONCATENATE_test
  let concatenate = engine.TryEvaluate("CONCATENATE('tt','33')", "");
  assert.strictEqual(concatenate, "tt33", "CONCATENATE('tt','33') 应该是 'tt33'");
  
  // EXACT_test
  let exact = engine.TryEvaluate("EXACT('tt','33')", true);
  assert.strictEqual(exact, false, "EXACT('tt','33') 应该是 false");
  
  exact = engine.TryEvaluate("EXACT('tt','tt')", false);
  assert.strictEqual(exact, true, "EXACT('tt','tt') 应该是 true");
  
  // FIND_test
  let find = engine.TryEvaluate("FIND(\"11\",\"12221122\")", 0);
  assert.strictEqual(find, 5, "FIND(\"11\",\"12221122\") 应该是 5");
  
  // FIXED_test
  let fixed = engine.TryEvaluate("FIXED(4567.89,1)", "");
  assert.strictEqual(fixed, "4,567.9", "FIXED(4567.89,1) 应该是 '4,567.9'");
  
  // LEFT_test
  let left = engine.TryEvaluate("LEFT('123222',3)", "");
  assert.strictEqual(left, "123", "LEFT('123222',3) 应该是 '123'");
  
  // LEN_test
  let len = engine.TryEvaluate("LEN('123222')", 0);
  assert.strictEqual(len, 6, "LEN('123222') 应该是 6");
  
  // LOWER_test
  let lower = engine.TryEvaluate("LOWER('ABC')", "");
  assert.strictEqual(lower, "abc", "LOWER('ABC') 应该是 'abc'");
  
  // MID_test
  let mid = engine.TryEvaluate("MID('ABCDEF',2,3)", "");
  assert.strictEqual(mid, "BCD", "MID('ABCDEF',2,3) 应该是 'BCD'");
  
  // PROPER_test
  let proper = engine.TryEvaluate("PROPER('abc abc')", "");
  assert.strictEqual(proper, "Abc Abc", "PROPER('abc abc') 应该是 'Abc Abc'");
  
  // REPLACE_test
  let replace = engine.TryEvaluate("REPLACE(\"abccd\",2,3,\"2\")", "");
  assert.strictEqual(replace, "a2d", "REPLACE(\"abccd\",2,3,\"2\") 应该是 'a2d'");
  
  // REPT_test
  let rept = engine.TryEvaluate("REPT(\"q\",3)", "");
  assert.strictEqual(rept, "qqq", "REPT(\"q\",3) 应该是 'qqq'");
  
  // RIGHT_test
  let right = engine.TryEvaluate("RIGHT(\"123q\",3)", "");
  assert.strictEqual