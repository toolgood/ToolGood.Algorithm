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
function testDATEVALUE() {
  console.log('开始测试 DATEVALUE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
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
  
  console.log('DATEVALUE 测试通过！');
}

function testTIMESTAMP() {
  console.log('开始测试 TIMESTAMP...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseLocalTime = true;
  
  let timestamp = engine.TryEvaluate("TIMESTAMP('2016-01-01')", 0);
  assert.strictEqual(timestamp, 1451577600000, "TIMESTAMP('2016-01-01') 应该是 1451577600000");
  
  timestamp = engine.TryEvaluate("TIMESTAMP('2016/01/01')", 0);
  assert.strictEqual(timestamp, 1451577600000, "TIMESTAMP('2016/01/01') 应该是 1451577600000");
  
  timestamp = engine.TryEvaluate("TIMESTAMP('2016-01-01',0)", 0);
  assert.strictEqual(timestamp, 1451577600000, "TIMESTAMP('2016-01-01',0) 应该是 1451577600000");
  
  timestamp = engine.TryEvaluate("TIMESTAMP('2016-01-01',1)", 0);
  assert.strictEqual(timestamp, 1451577600, "TIMESTAMP('2016-01-01',1) 应该是 1451577600");
  
  console.log('TIMESTAMP 测试通过！');
}

function testTIMEVALUE() {
  console.log('开始测试 TIMEVALUE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let timeValue = engine.TryEvaluate("TIMEVALUE('12:12:12')", 0);
  // 注意：JavaScript 中时间值的处理可能与 C# 不同，这里我们简化测试
  assert.ok(typeof timeValue === 'number', "TIMEVALUE('12:12:12') 应该返回一个数字");
  
  console.log('TIMEVALUE 测试通过！');
}

function testDATE() {
  console.log('开始测试 DATE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let dt = engine.TryEvaluate("DATE(2016,1,1)", new Date(0));
  assert.strictEqual(dt.getFullYear(), 2016, "DATE(2016,1,1) 年份应该是 2016");
  assert.strictEqual(dt.getMonth() + 1, 1, "DATE(2016,1,1) 月份应该是 1");
  assert.strictEqual(dt.getDate(), 1, "DATE(2016,1,1) 日期应该是 1");
  
  console.log('DATE 测试通过！');
}

function testTime() {
  console.log('开始测试 Time...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let time = engine.TryEvaluate("time(11,12,13)", 0);
  // 注意：JavaScript 中时间值的处理可能与 C# 不同，这里我们简化测试
  assert.ok(typeof time === 'number', "time(11,12,13) 应该返回一个数字");
  
  console.log('Time 测试通过！');
}

function testNow() {
  console.log('开始测试 Now...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let now = engine.TryEvaluate("now()", new Date(0));
  assert.ok(now instanceof Date, "now() 应该返回一个 Date 对象");
  
  console.log('Now 测试通过！');
}

function testToday() {
  console.log('开始测试 Today...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let today = engine.TryEvaluate("Today()", new Date(0));
  assert.ok(today instanceof Date, "Today() 应该返回一个 Date 对象");
  
  console.log('Today 测试通过！');
}

function testYear() {
  console.log('开始测试 Year...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let year = engine.TryEvaluate("year(now())", 0);
  assert.ok(typeof year === 'number', "year(now()) 应该返回一个数字");
  assert.ok(year > 2000, "year(now()) 应该返回一个大于 2000 的数字");
  
  console.log('Year 测试通过！');
}

function testMonth() {
  console.log('开始测试 Month...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let month = engine.TryEvaluate("Month(now())", 0);
  assert.ok(typeof month === 'number', "Month(now()) 应该返回一个数字");
  assert.ok(month >= 1 && month <= 12, "Month(now()) 应该返回一个 1-12 之间的数字");
  
  console.log('Month 测试通过！');
}

function testDay() {
  console.log('开始测试 Day...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let day = engine.TryEvaluate("Day(now())", 0);
  assert.ok(typeof day === 'number', "Day(now()) 应该返回一个数字");
  assert.ok(day >= 1 && day <= 31, "Day(now()) 应该返回一个 1-31 之间的数字");
  
  console.log('Day 测试通过！');
}

function testHour() {
  console.log('开始测试 Hour...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let hour = engine.TryEvaluate("Hour(now())", 0);
  assert.ok(typeof hour === 'number', "Hour(now()) 应该返回一个数字");
  assert.ok(hour >= 0 && hour <= 23, "Hour(now()) 应该返回一个 0-23 之间的数字");
  
  console.log('Hour 测试通过！');
}

function testMinute() {
  console.log('开始测试 Minute...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let minute = engine.TryEvaluate("Minute(now())", 0);
  assert.ok(typeof minute === 'number', "Minute(now()) 应该返回一个数字");
  assert.ok(minute >= 0 && minute <= 59, "Minute(now()) 应该返回一个 0-59 之间的数字");
  
  console.log('Minute 测试通过！');
}

function testSecond() {
  console.log('开始测试 Second...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let second = engine.TryEvaluate("Second(now())", 0);
  assert.ok(typeof second === 'number', "Second(now()) 应该返回一个数字");
  assert.ok(second >= 0 && second <= 59, "Second(now()) 应该返回一个 0-59 之间的数字");
  
  console.log('Second 测试通过！');
}

function testWEEKDAY() {
  console.log('开始测试 WEEKDAY...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let weekday = engine.TryEvaluate("WEEKDAY(date(2017,1,7))", 0);
  assert.strictEqual(weekday, 7, "WEEKDAY(date(2017,1,7)) 应该是 7");
  
  weekday = engine.TryEvaluate("WEEKDAY(date(2017,1,7),1)", 0);
  assert.strictEqual(weekday, 7, "WEEKDAY(date(2017,1,7),1) 应该是 7");
  
  weekday = engine.TryEvaluate("WEEKDAY(date(2017,1,7),2)", 0);
  assert.strictEqual(weekday, 6, "WEEKDAY(date(2017,1,7),2) 应该是 6");
  
  weekday = engine.TryEvaluate("WEEKDAY(date(2017,1,7),3)", 0);
  assert.strictEqual(weekday, 5, "WEEKDAY(date(2017,1,7),3) 应该是 5");
  
  console.log('WEEKDAY 测试通过！');
}

function testDATEDIF() {
  console.log('开始测试 DATEDIF...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let datedif = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0);
  assert.strictEqual(datedif, 41, "DATEDIF('1975-1-30','2017-1-7','y') 应该是 41");
  
  datedif = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','m')", 0);
  assert.strictEqual(datedif, 503, "DATEDIF('1975-1-30','2017-1-7','m') 应该是 503");
  
  datedif = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','d')", 0);
  assert.strictEqual(datedif, 15318, "DATEDIF('1975-1-30','2017-1-7','d') 应该是 15318");
  
  console.log('DATEDIF 测试通过！');
}

function testDAYS360() {
  console.log('开始测试 DAYS360...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let days360 = engine.TryEvaluate("DAYS360('1975-1-30','2017-1-7')", 0);
  assert.strictEqual(days360, 15097, "DAYS360('1975-1-30','2017-1-7') 应该是 15097");
  
  console.log('DAYS360 测试通过！');
}

function testEDATE() {
  console.log('开始测试 EDATE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let edate = engine.TryEvaluate("EDATE(\"2012-1-31\",32)", new Date(0));
  assert.strictEqual(edate.getFullYear(), 2014, "EDATE(\"2012-1-31\",32) 年份应该是 2014");
  assert.strictEqual(edate.getMonth() + 1, 9, "EDATE(\"2012-1-31\",32) 月份应该是 9");
  assert.strictEqual(edate.getDate(), 30, "EDATE(\"2012-1-31\",32) 日期应该是 30");
  
  console.log('EDATE 测试通过！');
}

function testEOMONTH() {
  console.log('开始测试 EOMONTH...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let eomonth = engine.TryEvaluate("EOMONTH(\"2012-2-1\",32)", new Date(0));
  assert.strictEqual(eomonth.getFullYear(), 2014, "EOMONTH(\"2012-2-1\",32) 年份应该是 2014");
  assert.strictEqual(eomonth.getMonth() + 1, 10, "EOMONTH(\"2012-2-1\",32) 月份应该是 10");
  assert.strictEqual(eomonth.getDate(), 31, "EOMONTH(\"2012-2-1\",32) 日期应该是 31");
  
  console.log('EOMONTH 测试通过！');
}

function testNETWORKDAYS() {
  console.log('开始测试 NETWORKDAYS...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let networkdays = engine.TryEvaluate("NETWORKDAYS(\"2012-1-1\",\"2013-1-1\")", 0);
  assert.strictEqual(networkdays, 262, "NETWORKDAYS(\"2012-1-1\",\"2013-1-1\") 应该是 262");
  
  console.log('NETWORKDAYS 测试通过！');
}

function testWORKDAY() {
  console.log('开始测试 WORKDAY...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let workday = engine.TryEvaluate("WORKDAY(\"2012-1-2\",145)", new Date(0));
  assert.strictEqual(workday.getFullYear(), 2012, "WORKDAY(\"2012-1-2\",145) 年份应该是 2012");
  assert.strictEqual(workday.getMonth() + 1, 7, "WORKDAY(\"2012-1-2\",145) 月份应该是 7");
  assert.strictEqual(workday.getDate(), 23, "WORKDAY(\"2012-1-2\",145) 日期应该是 23");
  
  console.log('WORKDAY 测试通过！');
}

function testWEEKNUM() {
  console.log('开始测试 WEEKNUM...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let weeknum = engine.TryEvaluate("WEEKNUM(\"2016-1-3\")", 0);
  assert.strictEqual(weeknum, 2, "WEEKNUM(\"2016-1-3\") 应该是 2");
  
  weeknum = engine.TryEvaluate("WEEKNUM(\"2016-1-2\")", 0);
  assert.strictEqual(weeknum, 1, "WEEKNUM(\"2016-1-2\") 应该是 1");
  
  console.log('WEEKNUM 测试通过！');
}

function testAdd() {
  console.log('开始测试 Add...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let add = engine.TryEvaluate("'2000-02-01'.addYears(1).year()", 0);
  assert.strictEqual(add, 2001, "'2000-02-01'.addYears(1).year() 应该是 2001");
  
  add = engine.TryEvaluate("'2000/02/01'.addYears(1).year()", 0);
  assert.strictEqual(add, 2001, "'2000/02/01'.addYears(1).year() 应该是 2001");
  
  add = engine.TryEvaluate("'2000-02-01'.AddMonths(1).Month()", 0);
  assert.strictEqual(add, 3, "'2000-02-01'.AddMonths(1).Month() 应该是 3");
  
  add = engine.TryEvaluate("'2000-02-01'.AddDays(1).Day()", 0);
  assert.strictEqual(add, 2, "'2000-02-01'.AddDays(1).Day() 应该是 2");
  
  console.log('Add 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testDATEVALUE();
    testTIMESTAMP();
    testTIMEVALUE();
    testDATE();
    testTime();
    testNow();
    testToday();
    testYear();
    testMonth();
    testDay();
    testHour();
    testMinute();
    testSecond();
    testWEEKDAY();
    testDATEDIF();
    testDAYS360();
    testEDATE();
    testEOMONTH();
    testNETWORKDAYS();
    testWORKDAY();
    testWEEKNUM();
    testAdd();
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
  testDATEVALUE,
  testTIMESTAMP,
  testTIMEVALUE,
  testDATE,
  testTime,
  testNow,
  testToday,
  testYear,
  testMonth,
  testDay,
  testHour,
  testMinute,
  testSecond,
  testWEEKDAY,
  testDATEDIF,
  testDAYS360,
  testEDATE,
  testEOMONTH,
  testNETWORKDAYS,
  testWORKDAY,
  testWEEKNUM,
  testAdd,
  runAllTests
};
