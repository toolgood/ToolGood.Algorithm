import assert from 'assert';
import { AlgorithmEngineHelper } from '../src/AlgorithmEngineHelper.js';

// 测试 GetDiyNames 函数
function testGetDiyNamesBasic() {
    console.log('开始测试 GetDiyNames 基本功能...');
    
    const p = AlgorithmEngineHelper.GetDiyNames('dd');
    assert.strictEqual('dd', p.Parameters[0].Name, "GetDiyNames('dd') 应该返回包含参数 'dd' 的对象");
    
    console.log('GetDiyNames 基本功能 测试通过！');
}

function testGetDiyNamesRepeated() {
    console.log('开始测试 GetDiyNames 重复调用...');
    
    const p3 = AlgorithmEngineHelper.GetDiyNames('dd');
    assert.strictEqual('dd', p3.Parameters[0].Name, "GetDiyNames('dd') 重复调用应该返回相同结果");
    
    console.log('GetDiyNames 重复调用 测试通过！');
}

function testGetDiyNamesWithFunction() {
    console.log('开始测试 GetDiyNames 带函数和参数...');
    
    const p5 = AlgorithmEngineHelper.GetDiyNames('ddd(d1,22)');
    assert.strictEqual('ddd', p5.Functions[0].Name, "GetDiyNames('ddd(d1,22)') 应该返回包含函数 'ddd' 的对象");
    assert.strictEqual('d1', p5.Parameters[0].Name, "GetDiyNames('ddd(d1,22)') 应该返回包含参数 'd1' 的对象");
    
    console.log('GetDiyNames 带函数和参数 测试通过！');
}

function testGetDiyNamesWithChinese() {
    console.log('开始测试 GetDiyNames 带中文字符...');
    
    const p6 = AlgorithmEngineHelper.GetDiyNames('长');
    assert.strictEqual('长', p6.Parameters[0].Name, "GetDiyNames('长') 应该返回包含参数 '长' 的对象");
    
    console.log('GetDiyNames 带中文字符 测试通过！');
}

function testGetDiyNamesWithExpression() {
    console.log('开始测试 GetDiyNames 带表达式...');
    
    const p7 = AlgorithmEngineHelper.GetDiyNames('ddd+2');
    assert.strictEqual('ddd', p7.Parameters[0].Name, "GetDiyNames('ddd+2') 应该返回包含参数 'ddd' 的对象");
    
    console.log('GetDiyNames 带表达式 测试通过！');
}

function testGetDiyNamesWithJson() {
    console.log('开始测试 GetDiyNames 带 JSON 和参数...');
    
    const p8 = AlgorithmEngineHelper.GetDiyNames(`{"A": 0.6,"B": 0.4,"C": 0.6,"E": 0.33,"F": 0.29,"Z": 0.15,
"EB": 0.7,"EE": 0.65,"EA": 0.85,"AB": 1.0,"BC": 1.0,"AA":1.0,
"EBC": 1.15,"BAB": 1.25,"BCB": 1.25,"BBC": 1.25,"CBB": 1.25,"EBA": 1.2,"AAA": 1.4}[瓦楞]`);
    assert.strictEqual('瓦楞', p8.Parameters[0].Name, "GetDiyNames 带 JSON 和参数 应该返回包含参数 '瓦楞' 的对象");
    
    console.log('GetDiyNames 带 JSON 和参数 测试通过！');
}

// 测试 IsParameter 函数
function testIsParameter() {
    console.log('开始测试 IsParameter...');
    
    let b = AlgorithmEngineHelper.IsParameter('false');
    assert.strictEqual(b, false, "IsParameter('false') 应该返回 false");

    b = AlgorithmEngineHelper.IsParameter("f11");
    assert.strictEqual(b, true, "IsParameter('f11') 应该返回 true");

    b = AlgorithmEngineHelper.IsParameter("f11+1");
    assert.strictEqual(b, false, "IsParameter('f11+1') 应该返回 false");

    b = AlgorithmEngineHelper.IsParameter("f11++");
    assert.strictEqual(b, false, "IsParameter('f11++') 应该返回 false");
    
    console.log('IsParameter 测试通过！');
}

// 测试 UnitConversion 函数
function testUnitConversionDistance() {
    console.log('开始测试 UnitConversion 距离单位...');
    
    let b = AlgorithmEngineHelper.UnitConversion(1, '米', '千米', '测试');
    assert.strictEqual(b, 0.001, "UnitConversion(1, '米', '千米', '测试') 应该返回 0.001");
    
    b = AlgorithmEngineHelper.UnitConversion(1, '米', '分米', '测试');
    assert.strictEqual(b, 10, "UnitConversion(1, '米', '分米', '测试') 应该返回 10");
    
    b = AlgorithmEngineHelper.UnitConversion(1, '米', '厘米', '测试');
    assert.strictEqual(b, 100, "UnitConversion(1, '米', '厘米', '测试') 应该返回 100");
    
    b = AlgorithmEngineHelper.UnitConversion(1, '米', 'mm', '测试');
    assert.strictEqual(b, 1000, "UnitConversion(1, '米', 'mm', '测试') 应该返回 1000");
    
    console.log('UnitConversion 距离单位 测试通过！');
}

function testUnitConversionArea() {
    console.log('开始测试 UnitConversion 面积单位...');
    
    let b = AlgorithmEngineHelper.UnitConversion(1, 'm2', 'dm2', '测试');
    assert.strictEqual(b, 100, "UnitConversion(1, 'm2', 'dm2', '测试') 应该返回 100");
    
    b = AlgorithmEngineHelper.UnitConversion(1, 'm2', 'cm2', '测试');
    assert.strictEqual(b, 10000, "UnitConversion(1, 'm2', 'cm2', '测试') 应该返回 10000");
    
    b = AlgorithmEngineHelper.UnitConversion(1, 'm2', 'mm2', '测试');
    assert.strictEqual(b, 1000000, "UnitConversion(1, 'm2', 'mm2', '测试') 应该返回 1000000");
    
    console.log('UnitConversion 面积单位 测试通过！');
}

function testUnitConversionVolume() {
    console.log('开始测试 UnitConversion 体积单位...');
    
    let b = AlgorithmEngineHelper.UnitConversion(1, 'm3', 'dm3', '测试');
    assert.strictEqual(b, 1000, "UnitConversion(1, 'm3', 'dm3', '测试') 应该返回 1000");
    
    b = AlgorithmEngineHelper.UnitConversion(1, 'm3', 'cm3', '测试');
    assert.strictEqual(b, 1000000, "UnitConversion(1, 'm3', 'cm3', '测试') 应该返回 1000000");
    
    b = AlgorithmEngineHelper.UnitConversion(1, 'm3', 'mm3', '测试');
    assert.strictEqual(b, 1000000000, "UnitConversion(1, 'm3', 'mm3', '测试') 应该返回 1000000000");
    
    console.log('UnitConversion 体积单位 测试通过！');
}

function testUnitConversionMass() {
    console.log('开始测试 UnitConversion 质量单位...');
    
    let b = AlgorithmEngineHelper.UnitConversion(1, 't', 'kg', '测试');
    assert.strictEqual(b, 1000, "UnitConversion(1, 't', 'kg', '测试') 应该返回 1000");
    
    b = AlgorithmEngineHelper.UnitConversion(1, 't', 'g', '测试');
    assert.strictEqual(b, 1000000, "UnitConversion(1, 't', 'g', '测试') 应该返回 1000000");
    
    console.log('UnitConversion 质量单位 测试通过！');
}

// 测试 CheckFormula 函数
function testCheckFormulaValid() {
    console.log('开始测试 CheckFormula 有效公式...');
    
    let b = AlgorithmEngineHelper.CheckFormula('1+1');
    assert.strictEqual(b, true, "CheckFormula('1+1') 应该返回 true");
    
    console.log('CheckFormula 有效公式 测试通过！');
}

function testCheckFormulaInvalid() {
    console.log('开始测试 CheckFormula 无效公式...');
    
    let b = AlgorithmEngineHelper.CheckFormula('1+');
    assert.strictEqual(b, false, "CheckFormula('1+') 应该返回 false");
    
    console.log('CheckFormula 无效公式 测试通过！');
}

function testCheckFormulaInvalidChar() {
    console.log('开始测试 CheckFormula 带无效字符...');
    
    let b = AlgorithmEngineHelper.CheckFormula('@123+1');
    assert.strictEqual(b, false, "CheckFormula('@123+1') 应该返回 false");
    
    console.log('CheckFormula 带无效字符 测试通过！');
}

// 运行所有测试
function runAllTests() {
    try {
        testGetDiyNamesBasic();
        testGetDiyNamesRepeated();
        testGetDiyNamesWithFunction();
        testGetDiyNamesWithChinese();
        testGetDiyNamesWithExpression();
        testGetDiyNamesWithJson();
        testIsParameter();
        testUnitConversionDistance();
        testUnitConversionArea();
        testUnitConversionVolume();
        testUnitConversionMass();
        testCheckFormulaValid();
        testCheckFormulaInvalid();
        testCheckFormulaInvalidChar();
        console.log('所有测试通过！');
    } catch (error) {
        console.error('测试失败:', error.message);
        process.exit(1);
    }
}

// 执行测试
runAllTests();

export {
    testGetDiyNamesBasic,
    testGetDiyNamesRepeated,
    testGetDiyNamesWithFunction,
    testGetDiyNamesWithChinese,
    testGetDiyNamesWithExpression,
    testGetDiyNamesWithJson,
    testIsParameter,
    testUnitConversionDistance,
    testUnitConversionArea,
    testUnitConversionVolume,
    testUnitConversionMass,
    testCheckFormulaValid,
    testCheckFormulaInvalid,
    testCheckFormulaInvalidChar,
    runAllTests
};
