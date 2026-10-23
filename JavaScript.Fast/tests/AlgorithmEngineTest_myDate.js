import assert from 'assert';
import { MyDate } from '../src/Internals/MyDate.js';

// 对齐 C# MyDateTest(适配 JS API)

// 构造函数测试
function testConstructorParams() {
  console.log('开始测试 Constructor_Params...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);

  assert.strictEqual(2024, myDate.Year);
  assert.strictEqual(6, myDate.Month);
  assert.strictEqual(15, myDate.Day);
  assert.strictEqual(10, myDate.Hour);
  assert.strictEqual(30, myDate.Minute);
  assert.strictEqual(45, myDate.Second);

  console.log('Constructor_Params 测试通过！');
}

function testConstructorDateTime() {
  console.log('开始测试 Constructor_DateTime...');

  const dt = new Date(2024, 5, 15, 10, 30, 45);
  const myDate = new MyDate(dt);

  assert.strictEqual(2024, myDate.Year);
  assert.strictEqual(6, myDate.Month);
  assert.strictEqual(15, myDate.Day);
  assert.strictEqual(10, myDate.Hour);
  assert.strictEqual(30, myDate.Minute);
  assert.strictEqual(45, myDate.Second);

  console.log('Constructor_DateTime 测试通过！');
}

function testConstructorTimeSpan() {
  console.log('开始测试 Constructor_TimeSpan...');

  const myDate = new MyDate({ days: 1, hours: 10, minutes: 30, seconds: 45 });

  assert.strictEqual(null, myDate.Year);
  assert.strictEqual(null, myDate.Month);
  assert.strictEqual(1, myDate.Day);
  assert.strictEqual(10, myDate.Hour);
  assert.strictEqual(30, myDate.Minute);
  assert.strictEqual(45, myDate.Second);

  console.log('Constructor_TimeSpan 测试通过！');
}

// Parse 测试
function testParseDateTime() {
  console.log('开始测试 Parse_DateTime...');

  const myDate = MyDate.Parse("2024-06-15 10:30:45");
  assert.strictEqual(2024, myDate.Year);
  assert.strictEqual(6, myDate.Month);
  assert.strictEqual(15, myDate.Day);
  assert.strictEqual(10, myDate.Hour);
  assert.strictEqual(30, myDate.Minute);
  assert.strictEqual(45, myDate.Second);

  console.log('Parse_DateTime 测试通过！');
}

function testParseDateTimeSlash() {
  console.log('开始测试 Parse_DateTime_Slash...');

  const myDate = MyDate.Parse("2024/06/15 10:30:45");
  assert.strictEqual(2024, myDate.Year);
  assert.strictEqual(6, myDate.Month);
  assert.strictEqual(15, myDate.Day);
  assert.strictEqual(10, myDate.Hour);
  assert.strictEqual(30, myDate.Minute);
  assert.strictEqual(45, myDate.Second);

  console.log('Parse_DateTime_Slash 测试通过！');
}

function testParseDate() {
  console.log('开始测试 Parse_Date...');

  const myDate = MyDate.Parse("2024-06-15");
  assert.strictEqual(2024, myDate.Year);
  assert.strictEqual(6, myDate.Month);
  assert.strictEqual(15, myDate.Day);
  assert.strictEqual(0, myDate.Hour);
  assert.strictEqual(0, myDate.Minute);
  assert.strictEqual(0, myDate.Second);

  console.log('Parse_Date 测试通过！');
}

function testParseTime() {
  console.log('开始测试 Parse_Time...');

  const myDate = MyDate.Parse("10:30:45");
  assert.strictEqual(null, myDate.Year);
  assert.strictEqual(null, myDate.Month);
  assert.strictEqual(null, myDate.Day);
  assert.strictEqual(10, myDate.Hour);
  assert.strictEqual(30, myDate.Minute);
  assert.strictEqual(45, myDate.Second);

  console.log('Parse_Time 测试通过！');
}

function testParseTimeNoSeconds() {
  console.log('开始测试 Parse_Time_NoSeconds...');

  const myDate = MyDate.Parse("10:30");
  assert.strictEqual(null, myDate.Year);
  assert.strictEqual(null, myDate.Month);
  assert.strictEqual(null, myDate.Day);
  assert.strictEqual(10, myDate.Hour);
  assert.strictEqual(30, myDate.Minute);
  assert.strictEqual(0, myDate.Second);

  console.log('Parse_Time_NoSeconds 测试通过！');
}

function testParseInvalid() {
  console.log('开始测试 Parse_Invalid...');

  const myDate = MyDate.Parse("invalid date");
  assert.strictEqual(null, myDate);

  console.log('Parse_Invalid 测试通过！');
}

// ToDateTime 测试
function testToDateTime() {
  console.log('开始测试 ToDateTime...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const dt = myDate.ToDateTime(0); // UTC

  assert.strictEqual(2024, dt.getUTCFullYear());
  assert.strictEqual(6, dt.getUTCMonth() + 1);
  assert.strictEqual(15, dt.getUTCDate());
  assert.strictEqual(10, dt.getUTCHours());
  assert.strictEqual(30, dt.getUTCMinutes());
  assert.strictEqual(45, dt.getUTCSeconds());

  console.log('ToDateTime 测试通过！');
}

// ToTimeSpan 测试
function testToTimeSpan() {
  console.log('开始测试 ToTimeSpan...');

  const myDate = new MyDate(null, null, 1, 10, 30, 45);
  const ts = myDate.ToTimeSpan();

  assert.strictEqual(1, ts.days);
  assert.strictEqual(10, ts.hours);
  assert.strictEqual(30, ts.minutes);
  assert.strictEqual(45, ts.seconds);

  console.log('ToTimeSpan 测试通过！');
}

// AddYears 测试
function testAddYears() {
  console.log('开始测试 AddYears...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const result = myDate.AddYears(1);

  assert.strictEqual(2025, result.Year);
  assert.strictEqual(6, result.Month);
  assert.strictEqual(15, result.Day);

  console.log('AddYears 测试通过！');
}

function testAddYearsNegative() {
  console.log('开始测试 AddYears_Negative...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const result = myDate.AddYears(-1);

  assert.strictEqual(2023, result.Year);

  console.log('AddYears_Negative 测试通过！');
}

// AddMonths 测试
function testAddMonths() {
  console.log('开始测试 AddMonths...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const result = myDate.AddMonths(1);

  assert.strictEqual(7, result.Month);

  console.log('AddMonths 测试通过！');
}

function testAddMonthsOverflow() {
  console.log('开始测试 AddMonths_Overflow...');

  const myDate = new MyDate(2024, 11, 15, 10, 30, 45);
  const result = myDate.AddMonths(2);

  assert.strictEqual(2025, result.Year);
  assert.strictEqual(1, result.Month);

  console.log('AddMonths_Overflow 测试通过！');
}

// AddDays 测试
function testAddDays() {
  console.log('开始测试 AddDays...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const result = myDate.AddDays(5);

  assert.strictEqual(20, result.Day);

  console.log('AddDays 测试通过！');
}

function testAddDaysNegative() {
  console.log('开始测试 AddDays_Negative...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const result = myDate.AddDays(-5);

  assert.strictEqual(10, result.Day);

  console.log('AddDays_Negative 测试通过！');
}

// AddHours 测试
function testAddHours() {
  console.log('开始测试 AddHours...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const result = myDate.AddHours(5);

  assert.strictEqual(15, result.Hour);

  console.log('AddHours 测试通过！');
}

// AddMinutes 测试
function testAddMinutes() {
  console.log('开始测试 AddMinutes...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const result = myDate.AddMinutes(15);

  assert.strictEqual(45, result.Minute);

  console.log('AddMinutes 测试通过！');
}

// AddSeconds 测试
function testAddSeconds() {
  console.log('开始测试 AddSeconds...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const result = myDate.AddSeconds(10);

  assert.strictEqual(55, result.Second);

  console.log('AddSeconds 测试通过！');
}

// toString 测试
function testToString() {
  console.log('开始测试 ToString...');

  const myDate = new MyDate(2024, 6, 15, 10, 30, 45);
  const str = myDate.toString();

  assert.ok(str.includes("2024"), "toString 应包含 2024");
  assert.ok(str.includes("06"), "toString 应包含 06");
  assert.ok(str.includes("15"), "toString 应包含 15");

  const timeOnly = new MyDate(null, null, null, 10, 30, 45);
  const str2 = timeOnly.toString();
  assert.ok(str2.includes("10:30:45"), "时间字符串应包含 10:30:45");

  console.log('ToString 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testConstructorParams();
    testConstructorDateTime();
    testConstructorTimeSpan();
    testParseDateTime();
    testParseDateTimeSlash();
    testParseDate();
    testParseTime();
    testParseTimeNoSeconds();
    testParseInvalid();
    testToDateTime();
    testToTimeSpan();
    testAddYears();
    testAddYearsNegative();
    testAddMonths();
    testAddMonthsOverflow();
    testAddDays();
    testAddDaysNegative();
    testAddHours();
    testAddMinutes();
    testAddSeconds();
    testToString();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testConstructorParams,
  testConstructorDateTime,
  testConstructorTimeSpan,
  testParseDateTime,
  testParseDateTimeSlash,
  testParseDate,
  testParseTime,
  testParseTimeNoSeconds,
  testParseInvalid,
  testToDateTime,
  testToTimeSpan,
  testAddYears,
  testAddYearsNegative,
  testAddMonths,
  testAddMonthsOverflow,
  testAddDays,
  testAddDaysNegative,
  testAddHours,
  testAddMinutes,
  testAddSeconds,
  testToString,
  runAllTests
};
