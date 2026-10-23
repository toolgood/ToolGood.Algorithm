import assert from 'assert';
import { AlgorithmEngineHelper } from '../src/AlgorithmEngineHelper.js';

// 对齐 C# UnitConversionPrecisionTest
// 精确分数系数防回归测试。
// 这些系数由 6 位近似值重构为精确分数(如 short ton = 50000/45359237),
// 反向换算误差应远小于 1e-9。若有人改回近似值,本测试将失败。
function assertClose(src, tar, value, expected) {
  const result = AlgorithmEngineHelper.UnitConversion(value, src, tar, '精度测试');
  const rel = Math.abs(result - expected) / Math.max(Math.abs(expected), 1e-30);
  assert.ok(rel < 1e-9, `${src} -> ${tar} 相对误差 ${rel} 应小于 1e-9, 实际值 ${result}, 期望值 ${expected}`);
}

// 质量
function testMassShortTonExact() {
  console.log('开始测试 Mass_ShortTon_Exact...');

  assertClose("short ton", "kg", 1, 907.18474);
  assertClose("kg", "short ton", 1, 1.0 / 907.18474);
  assertClose("short ton", "lb", 1, 2000);

  console.log('Mass_ShortTon_Exact 测试通过！');
}

function testMassLongTonExact() {
  console.log('开始测试 Mass_LongTon_Exact...');

  assertClose("long ton", "kg", 1, 1016.0469088);
  assertClose("kg", "long ton", 1, 1.0 / 1016.0469088);
  assertClose("long ton", "lb", 1, 2240);

  console.log('Mass_LongTon_Exact 测试通过！');
}

// 面积
function testAreaMuExact() {
  console.log('开始测试 Area_Mu_Exact...');

  assertClose("亩", "m²", 1, 666.6666666666667);
  assertClose("m²", "亩", 1, 0.0015);

  console.log('Area_Mu_Exact 测试通过！');
}

// 体积
function testVolumeCubicFootExact() {
  console.log('开始测试 Volume_CubicFoot_Exact...');

  assertClose("ft³", "l", 1, 28.316846592);
  assertClose("l", "ft³", 1, 0.03531466672148859);
  assertClose("ft³", "in³", 1, 1728);

  console.log('Volume_CubicFoot_Exact 测试通过！');
}

function testVolumeCubicInchExact() {
  console.log('开始测试 Volume_CubicInch_Exact...');

  assertClose("in³", "l", 1, 0.016387064);
  assertClose("l", "in³", 1, 61.0237440947323);

  console.log('Volume_CubicInch_Exact 测试通过！');
}

function testVolumeImperialPintExact() {
  console.log('开始测试 Volume_ImperialPint_Exact...');

  assertClose("imperial pint", "l", 1, 0.56826125);
  assertClose("l", "imperial pint", 1, 1.7597539863927023);

  console.log('Volume_ImperialPint_Exact 测试通过！');
}

function testVolumeImperialGallonExact() {
  console.log('开始测试 Volume_ImperialGallon_Exact...');

  assertClose("imperial gallon", "l", 1, 4.54609);
  assertClose("l", "imperial gallon", 1, 0.2199692482990878);
  assertClose("imperial gallon", "imperial pint", 1, 8);
  assertClose("imperial gallon", "imperial quart", 1, 4);

  console.log('Volume_ImperialGallon_Exact 测试通过！');
}

function testVolumeImperialQuartExact() {
  console.log('开始测试 Volume_ImperialQuart_Exact...');

  assertClose("imperial quart", "l", 1, 1.1365225);
  assertClose("l", "imperial quart", 1, 0.87987699319635115);

  console.log('Volume_ImperialQuart_Exact 测试通过！');
}

function testVolumeUSPintExact() {
  console.log('开始测试 Volume_USPint_Exact...');

  assertClose("US pint", "l", 1, 0.473176473);
  assertClose("l", "US pint", 1, 2.1133764188651873);

  console.log('Volume_USPint_Exact 测试通过！');
}

function testVolumeUSGallonExact() {
  console.log('开始测试 Volume_USGallon_Exact...');

  assertClose("US gallon", "l", 1, 3.785411784);
  assertClose("l", "US gallon", 1, 0.2641720523581484);
  assertClose("US gallon", "US quart", 1, 4);

  console.log('Volume_USGallon_Exact 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testMassShortTonExact();
    testMassLongTonExact();
    testAreaMuExact();
    testVolumeCubicFootExact();
    testVolumeCubicInchExact();
    testVolumeImperialPintExact();
    testVolumeImperialGallonExact();
    testVolumeImperialQuartExact();
    testVolumeUSPintExact();
    testVolumeUSGallonExact();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
runAllTests();

export {
  testMassShortTonExact,
  testMassLongTonExact,
  testAreaMuExact,
  testVolumeCubicFootExact,
  testVolumeCubicInchExact,
  testVolumeImperialPintExact,
  testVolumeImperialGallonExact,
  testVolumeImperialQuartExact,
  testVolumeUSPintExact,
  testVolumeUSGallonExact,
  runAllTests
};
