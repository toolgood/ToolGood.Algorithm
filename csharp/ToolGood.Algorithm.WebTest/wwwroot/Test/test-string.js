QUnit.module('test-string', function () {
    QUnit.test('asc_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("asc('ａｂｃＡＢＣ１２３')", "");
        assert.equal(t, "abcABC123");
    });

    QUnit.test('Jis_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("jis('abcABC123')", "");
        assert.equal(t, "ａｂｃＡＢＣ１２３");
        t = engine.TryEvaluate("WIDECHAR('abcABC123')", "");
        assert.equal(t, "ａｂｃＡＢＣ１２３");
    });
    QUnit.test('CHAR_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("char(49)", "");
        assert.equal(t, "1");
    });
    QUnit.test('CLEAN_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("clean('\\r112\\t')", "");
        assert.equal(t, "112");
    });
    QUnit.test('code_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("code('1')", 0);
        assert.equal(t, 49);
    });
    QUnit.test('CONCATENATE_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("CONCATENATE('tt','33')", "");
        assert.equal(t, "tt33");
    });
    QUnit.test('EXACT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("EXACT('tt','33')", true);
        assert.equal(t, false);
        t = engine.TryEvaluate("EXACT('tt','tt')", false);
        assert.equal(t, true);

        t = engine.TryEvaluate("EXACT('33',33)", false);
        assert.equal(t, true);
        t = engine.TryEvaluate("EXACT('331.1',331.1)", false);
        assert.equal(t, true);
        t = engine.TryEvaluate("EXACT('TRUE',TRUE())", false);
        assert.equal(t, true);
        t = engine.TryEvaluate("EXACT('1',TRUE())", true);
        assert.equal(t, false);
    });
    QUnit.test('FIND_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("FIND(\"11\",\"12221122\")", 0);
        assert.equal(t, 5);
    });
    QUnit.test('FIXED_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("FIXED(4567.89,1)", "");
        assert.equal(t, "4,567.9");
        //t = engine.TryEvaluate(" FIXED(4567.89,-1)", "");//iserror
        //assert.equal(t, "4,570.0");
        t = engine.TryEvaluate("FIXED(-4567.89, 1, TRUE())", "");
        assert.equal(t, "-4567.9");
        t = engine.TryEvaluate("FIXED(77.888)", "");
        assert.equal(t, "77.89");
    });
    QUnit.test('LEFT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("LEFT('123222',3)", "");
        assert.equal(t, "123");
    });
    QUnit.test('LEN_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("LEN('123222')", 0);
        assert.equal(t, 6);
    });
    QUnit.test('LOWER_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("LOWER('ABC')", "");
        assert.equal(t, "abc");
    });
    QUnit.test('MID_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("MID('ABCDEF',2,3)", "");
        assert.equal(t, "BCD");
    });
    QUnit.test('PROPER_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("PROPER('abc abc')", "");
        assert.equal(t, "Abc Abc");
    });
    QUnit.test('REPLACE_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("REPLACE(\"abccd\",2,3,\"2\")", "");
        assert.equal(t, "a2d");

        var t1 = engine.TryEvaluate("REPLACE(\"abccd\",'bc',\"2\")", "");
        assert.equal(t1, "a2cd");
    });
    QUnit.test('REPT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("REPT(\"q\",3)", "");
        assert.equal(t, "qqq");
    });
    QUnit.test('RIGHT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("RIGHT(\"123q\",3)", "");
        assert.equal(t, "23q");
    });
    QUnit.test('RMB_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("rmb(12.3)", "");
        assert.equal(t, "壹拾贰元叁角");
    });
    QUnit.test('SEARCH_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("SEARCH(\"aa\",\"abbAaddd\")", 0);
        assert.equal(t, 4);
    });
    QUnit.test('SUBSTITUTE_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\")", "");
        assert.equal(t, "1212cc");
        t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\",2)", "");
        assert.equal(t, "ab12cc");
    });
    QUnit.test('T_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("T(12)", "");
        assert.equal(t, "");
        t = engine.TryEvaluate("T('123')", "");
        assert.equal(t, "123");
    });
    QUnit.test('TEXT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("TEXT(123,\"0.00\")", "");
        assert.equal(t, "123.00");
    });
    QUnit.test('TRIM_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("TRIM(\" 123 123 \")", "");
        assert.equal(t, "123 123");
    });
    QUnit.test('UPPER_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("UPPER(\"abc\")", "");
        assert.equal(t, "ABC");
    });
    QUnit.test('VALUE_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("VALUE(\"123\")", 0);
        assert.equal(t, 123);
    });

}); 
 
  