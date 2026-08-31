import assert from 'assert';
import { CalculationLogicEngine } from '../src/CalculationLogic/CalculationLogicEngine.js';
import { OperandType } from '../src/Enums/OperandType.js';

function createEngine(useCalculationLogicInfo = true) {
    return new CalculationLogicEngine(useCalculationLogicInfo);
}

// #region SetSceneName 测试

function testSetSceneName() {
    const logic = createEngine();
    logic.SetSceneName('场景1');
    assert.strictEqual(logic.ToInfoString(), '===== 场景1 =====\n');
}

function testSetSceneName_NoInfo() {
    const logic = createEngine(false);
    logic.SetSceneName('场景1');
    assert.strictEqual(logic.ToInfoString(), '');
}

// #endregion

// #region InitValue 测试

function testInitValue_Int() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    assert.strictEqual(logic.ToInfoString(), '[初始] a=1;b=2;\n');
}

function testInitValue_String() {
    const logic = createEngine();
    logic.InitValue('s', 'hello');
    assert.strictEqual(logic.ToInfoString(), '[初始] s=hello;\n');
}

function testInitValue_Decimal() {
    // JS 无 decimal 类型，使用 number 表示
    const logic = createEngine();
    logic.InitValue('d', 1.5);
    assert.strictEqual(logic.ToInfoString(), '[初始] d=1.5;\n');
}

function testInitValue_Double() {
    const logic = createEngine();
    logic.InitValue('dd', 1.5);
    assert.strictEqual(logic.ToInfoString(), '[初始] dd=1.5;\n');
}

function testInitValue_Bool() {
    const logic = createEngine();
    logic.InitValue('flag', true);
    assert.strictEqual(logic.ToInfoString(), '[初始] flag=True;\n');
}

function testInitValue_LayerRemark() {
    // InitValue 的层级与备注不参与信息输出
    const logic = createEngine();
    logic.InitValue('i', 100, 2, '备注');
    assert.strictEqual(logic.ToInfoString(), '[初始] i=100;\n');
}

function testInitValue_CanBeUsedInCondition() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    assert.strictEqual(logic.CheckCondition('a<b'), true);
    assert.strictEqual(logic.CheckCondition('a>b'), false);
}

// #endregion

// #region CheckCondition 测试

function testCheckCondition_True() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    assert.strictEqual(logic.CheckCondition('a<b'), true);
    assert.strictEqual(logic.ToInfoString(), '[初始] a=1;b=2;\n[成功] if a<b: // 1<2\n');
}

function testCheckCondition_False() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    assert.strictEqual(logic.CheckCondition('a>b'), false);
    assert.strictEqual(logic.ToInfoString(), '[初始] a=1;b=2;\n[失败] if a>b: // 1>2\n');
}

function testCheckCondition_LayerRemark() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    assert.strictEqual(logic.CheckCondition('a<b', 1, '备注'), true);
    assert.strictEqual(logic.ToInfoString(), '[初始] a=1;b=2;\n[成功]    if a<b: // 1<2 // 备注\n');
}

function testCheckCondition_StringCompare() {
    const logic = createEngine();
    logic.InitValue('s', 'abc');
    assert.strictEqual(logic.CheckCondition(`s='abc'`), true);
    assert.strictEqual(logic.CheckCondition(`s='ab'`), false);
    assert.strictEqual(logic.ToInfoString(), `[初始] s=abc;\n[成功] if s='abc': // "abc"='abc'\n[失败] if s='ab': // "abc"='ab'\n`);
}

function testCheckCondition_NonBoolean_Throws() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    let ex = null;
    try {
        logic.CheckCondition('a+b');
    } catch (e) {
        ex = e;
    }
    assert.ok(ex, '应该抛出异常');
    assert.strictEqual(ex.message, 'The condition must be a boolean value!');
    assert.strictEqual(logic.ToInfoString(), '[初始] a=1;b=2;\n[条件][错误] if a+b: // 1+2 = The condition must be a boolean value!\n');
}

function testCheckCondition_Error_Throws() {
    const logic = createEngine();
    let ex = null;
    try {
        logic.CheckCondition('1/0=0');
    } catch (e) {
        ex = e;
    }
    assert.ok(ex, '应该抛出异常');
    assert.strictEqual(ex.message, `Function '/' Div 0 error!`);
    assert.strictEqual(logic.ToInfoString(), `[条件][错误] if 1/0=0: // 1/0=0 = Function '/' Div 0 error!\n`);
}

// #endregion

// #region SetFormula 测试

function testSetFormula() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    logic.SetFormula('x', 'a*3+b', 1, '备注');
    assert.strictEqual(logic.ToInfoString(), '[初始] a=1;b=2;\n[赋值]    x = a*3+b = 1*3+2 = 5 // 备注\n');
    // 公式结果应已写入参数
    assert.strictEqual(logic.CheckCondition('x=5'), true);
}

function testSetFormula_StringConcat() {
    const logic = createEngine();
    logic.InitValue('s', 'hello');
    logic.SetFormula('y', `s&'!'`);
    assert.strictEqual(logic.ToInfoString(), `[初始] s=hello;\n[赋值] y = s&'!' = "hello"&'!' = hello!\n`);
    assert.strictEqual(logic.CheckCondition(`y='hello!'`), true);
}

function testSetFormula_TextAndNumber() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.SetFormula('z', `'text'&a`);
    assert.strictEqual(logic.ToInfoString(), `[初始] a=1;\n[赋值] z = 'text'&a = 'text'&1 = text1\n`);
}

function testSetFormula_Error_Throws() {
    const logic = createEngine();
    let ex = null;
    try {
        logic.SetFormula('x', '1/0');
    } catch (e) {
        ex = e;
    }
    assert.ok(ex, '应该抛出异常');
    assert.strictEqual(ex.message, `Function '/' Div 0 error!`);
    assert.strictEqual(logic.ToInfoString(), `[赋值][错误] x = 1/0 = 1/0 // Function '/' Div 0 error!\n`);
}

// #endregion

// #region SetValue 测试

function testSetValue_String() {
    const logic = createEngine();
    logic.SetValue('s', '文本');
    assert.strictEqual(logic.ToInfoString(), '[赋值] s = "文本"\n');
    assert.strictEqual(logic.CheckCondition(`s='文本'`), true);
}

function testSetValue_StringEscape() {
    const logic = createEngine();
    logic.SetValue('sq', 'a"b');
    assert.strictEqual(logic.ToInfoString(), '[赋值] sq = "a\\"b"\n');
}

function testSetValue_Decimal() {
    // JS 无 decimal 类型，使用 number 表示
    const logic = createEngine();
    logic.SetValue('d', 1.5);
    assert.strictEqual(logic.ToInfoString(), '[赋值] d = 1.5\n');
}

function testSetValue_Double() {
    const logic = createEngine();
    logic.SetValue('dd', 1.5);
    assert.strictEqual(logic.ToInfoString(), '[赋值] dd = 1.5\n');
}

function testSetValue_Bool() {
    const logic = createEngine();
    logic.SetValue('flag', true);
    assert.strictEqual(logic.ToInfoString(), '[赋值] flag = True\n');
}

function testSetValue_LayerRemark() {
    const logic = createEngine();
    logic.SetValue('i', 100, 1, '备注');
    assert.strictEqual(logic.ToInfoString(), '[赋值]    i = 100 // 备注\n');
}

// #endregion

// #region GetValue 测试

function testGetValue_InitValue_Int() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    const operand = logic.GetValue('a');
    assert.strictEqual(operand.IsNumber, true);
    assert.strictEqual(operand.Type, OperandType.NUMBER);
    assert.strictEqual(operand.NumberValue, 1);
}

function testGetValue_InitValue_String() {
    const logic = createEngine();
    logic.InitValue('s', 'hello');
    const operand = logic.GetValue('s');
    assert.strictEqual(operand.IsText, true);
    assert.strictEqual(operand.Type, OperandType.TEXT);
    assert.strictEqual(operand.TextValue, 'hello');
}

function testGetValue_InitValue_Decimal() {
    // JS 无 decimal 类型，使用 number 表示
    const logic = createEngine();
    logic.InitValue('d', 1.5);
    const operand = logic.GetValue('d');
    assert.strictEqual(operand.IsNumber, true);
    assert.strictEqual(operand.NumberValue, 1.5);
}

function testGetValue_InitValue_Double() {
    const logic = createEngine();
    logic.InitValue('dd', 1.5);
    const operand = logic.GetValue('dd');
    assert.strictEqual(operand.IsNumber, true);
    assert.strictEqual(operand.NumberValue, 1.5);
}

function testGetValue_InitValue_Bool() {
    const logic = createEngine();
    logic.InitValue('flag', true);
    const operand = logic.GetValue('flag');
    assert.strictEqual(operand.IsBoolean, true);
    assert.strictEqual(operand.Type, OperandType.BOOLEAN);
    assert.strictEqual(operand.BooleanValue, true);
}

function testGetValue_SetValue() {
    const logic = createEngine();
    logic.SetValue('v', 'text');
    const operand = logic.GetValue('v');
    assert.strictEqual(operand.IsText, true);
    assert.strictEqual(operand.TextValue, 'text');
}

function testGetValue_SetFormula() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    logic.SetFormula('x', 'a*3+b');
    const operand = logic.GetValue('x');
    assert.strictEqual(operand.IsNumber, true);
    assert.strictEqual(operand.NumberValue, 5);
}

function testGetValue_AfterSetFormulaOverwrite() {
    const logic = createEngine();
    logic.InitValue('a', 1);
    logic.SetFormula('a', 'a+10');
    const operand = logic.GetValue('a');
    assert.strictEqual(operand.IsNumber, true);
    assert.strictEqual(operand.NumberValue, 11);
}

function testGetValue_NotFound_ReturnsError() {
    const logic = createEngine();
    const operand = logic.GetValue('missing');
    assert.strictEqual(operand.IsError, true);
    assert.strictEqual(operand.ErrorMsg, 'Parameter [missing] is missing.');
}

function testGetValue_NoInfo() {
    // 关闭信息记录不影响取值
    const logic = createEngine(false);
    logic.InitValue('a', 1);
    assert.strictEqual(logic.GetValue('a').NumberValue, 1);
}

// #endregion

// #region BlankLine 测试

function testBlankLine() {
    const logic = createEngine();
    logic.BlankLine();
    assert.strictEqual(logic.ToInfoString(), '\n');

    logic.BlankLine();
    logic.BlankLine();
    assert.strictEqual(logic.ToInfoString(), '\n\n\n');
}

// #endregion

// #region ToInfoString 测试

function testToInfoString_Empty() {
    const logic = createEngine();
    assert.strictEqual(logic.ToInfoString(), '');
}

function testToInfoString_Disabled() {
    // 关闭信息记录后，所有操作都不产生信息
    const logic = createEngine(false);
    logic.SetSceneName('场景1');
    logic.InitValue('a', 1);
    logic.CheckCondition('a>0');
    logic.SetValue('b', 2);
    logic.SetFormula('c', 'a+1');
    logic.BlankLine();
    assert.strictEqual(logic.ToInfoString(), '');
}

function testToInfoString_FullFlow() {
    const logic = createEngine();
    logic.SetSceneName('场景1');
    logic.InitValue('a', 1);
    logic.InitValue('b', 2);
    if (logic.CheckCondition('a>b')) {
        logic.SetValue('c', 3, 1);
        logic.SetFormula('c', 'a*5+b', 1);
    } else if (logic.CheckCondition('a<b')) {
        logic.SetValue('c', 4, 1);
        logic.SetFormula('c', 'a*3+b', 1);
    }
    logic.BlankLine();
    logic.SetValue('e', 5);

    const expected = '[初始] a=1;b=2;\n'
        + '===== 场景1 =====\n'
        + '[失败] if a>b: // 1>2\n'
        + '[成功] if a<b: // 1<2\n'
        + '[赋值]    c = 4\n'
        + '[赋值]    c = a*3+b = 1*3+2 = 5\n'
        + '\n'
        + '[赋值] e = 5\n';
    assert.strictEqual(logic.ToInfoString(), expected);
}

// #endregion

// 运行所有测试
function runAllTests() {
    try {
        testSetSceneName();
        testSetSceneName_NoInfo();
        testInitValue_Int();
        testInitValue_String();
        testInitValue_Decimal();
        testInitValue_Double();
        testInitValue_Bool();
        testInitValue_LayerRemark();
        testInitValue_CanBeUsedInCondition();
        testCheckCondition_True();
        testCheckCondition_False();
        testCheckCondition_LayerRemark();
        testCheckCondition_StringCompare();
        testCheckCondition_NonBoolean_Throws();
        testCheckCondition_Error_Throws();
        testSetFormula();
        testSetFormula_StringConcat();
        testSetFormula_TextAndNumber();
        testSetFormula_Error_Throws();
        testSetValue_String();
        testSetValue_StringEscape();
        testSetValue_Decimal();
        testSetValue_Double();
        testSetValue_Bool();
        testSetValue_LayerRemark();
        testGetValue_InitValue_Int();
        testGetValue_InitValue_String();
        testGetValue_InitValue_Decimal();
        testGetValue_InitValue_Double();
        testGetValue_InitValue_Bool();
        testGetValue_SetValue();
        testGetValue_SetFormula();
        testGetValue_AfterSetFormulaOverwrite();
        testGetValue_NotFound_ReturnsError();
        testGetValue_NoInfo();
        testBlankLine();
        testToInfoString_Empty();
        testToInfoString_Disabled();
        testToInfoString_FullFlow();
        console.log('所有 CalculationLogic 测试通过！');
    } catch (error) {
        console.error('测试失败:', error.message);
        process.exit(1);
    }
}

// 执行测试
runAllTests();

export {
    testSetSceneName,
    testSetSceneName_NoInfo,
    testInitValue_Int,
    testInitValue_String,
    testInitValue_Decimal,
    testInitValue_Double,
    testInitValue_Bool,
    testInitValue_LayerRemark,
    testInitValue_CanBeUsedInCondition,
    testCheckCondition_True,
    testCheckCondition_False,
    testCheckCondition_LayerRemark,
    testCheckCondition_StringCompare,
    testCheckCondition_NonBoolean_Throws,
    testCheckCondition_Error_Throws,
    testSetFormula,
    testSetFormula_StringConcat,
    testSetFormula_TextAndNumber,
    testSetFormula_Error_Throws,
    testSetValue_String,
    testSetValue_StringEscape,
    testSetValue_Decimal,
    testSetValue_Double,
    testSetValue_Bool,
    testSetValue_LayerRemark,
    testGetValue_InitValue_Int,
    testGetValue_InitValue_String,
    testGetValue_InitValue_Decimal,
    testGetValue_InitValue_Double,
    testGetValue_InitValue_Bool,
    testGetValue_SetValue,
    testGetValue_SetFormula,
    testGetValue_AfterSetFormulaOverwrite,
    testGetValue_NotFound_ReturnsError,
    testGetValue_NoInfo,
    testBlankLine,
    testToInfoString_Empty,
    testToInfoString_Disabled,
    testToInfoString_FullFlow,
    runAllTests
};
