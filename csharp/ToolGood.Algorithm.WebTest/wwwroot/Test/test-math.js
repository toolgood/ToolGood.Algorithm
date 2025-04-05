QUnit.module('test-math', function () {
    QUnit.test('Pi_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 50));

        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("pi()", 0.0);
        assert.equal(Math.round(3.141592654,9), Math.round(t, 9));
    });
    QUnit.test('abs_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("abs(-1.2)", 0.0);
        assert.equal(1.2, t);
    });
    QUnit.test('QUOTIENT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("QUOTIENT(7,3)", 0.0);
        assert.equal(2.0, t);
    });
    QUnit.test('MOD_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("MOD(7,3)", 0.0);
        assert.equal(1.0, t);
    });
    QUnit.test('SIGN_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("SIGN(0)", 0);
        assert.equal(0, t);
        t = engine.TryEvaluate("SIGN(9)", 0);
        assert.equal(1, t);
        t = engine.TryEvaluate("SIGN(-9)", 0);
        assert.equal(-1, t);
    });
    QUnit.test('SQRT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("SQRT(9)", 0.0);
        assert.equal(3.0, t);
    });
    QUnit.test('SUM_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("SUM(1,2,3,4)", 0.0);
        assert.equal(10.0, t);
    });
    QUnit.test('TRUNC_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("TRUNC(9.222)", 0.0);
        assert.equal(9.0, t);
        t = engine.TryEvaluate("TRUNC(-9.222)", 0.0);
        assert.equal(-9.0, t);
    });
    QUnit.test('int_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("int(9.222)", 0.0);
        assert.equal(9.0, t);
        t = engine.TryEvaluate("int(-9.222)", 0.0);
        assert.equal(-9.0, t);
    });
    QUnit.test('GCD_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("GCD(3,5,7)", 0.0);
        assert.equal(1.0, t);
        t = engine.TryEvaluate("GCD(30,21)", 0.0);
        assert.equal(3.0, t);
    });
    QUnit.test('LCM_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("LCM(3,5,7)", 0.0);
        assert.equal(105.0, t);
    });
    QUnit.test('combin_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("combin(10,2)", 0.0);
        assert.equal(45.0, t);
    });
    QUnit.test('PERMUT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("PERMUT(10,2)", 0.0);
        assert.equal(90.0, t);
    });
    QUnit.test('degrees_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 50));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("degrees(pi())", 0.0);
        assert.equal(180.0, t);
    });
    QUnit.test('RADIANS_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("RADIANS(180)", 0.0);
        assert.equal(Math.round(Math.PI, 9), Math.round(t,9));
    });
    QUnit.test('cos_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("cos(1)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(0.540302306, 6), t);
    });
    QUnit.test('cosh_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("cosh(1)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.543080635, 6), t);
    });
    QUnit.test('sin_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("sin(1)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(0.841470985, 6), t);
    });
    QUnit.test('sinh_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("sinh(1)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.175201194, 6), t);
    });
    QUnit.test('tan_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("tan(1)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.557407725, 6), t);
    });
    QUnit.test('tanh_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("tanh(1)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(0.761594156, 6), t);
    });
    QUnit.test('acos_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("acos(0.5)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.047197551, 6), t);
    });
    QUnit.test('acosh_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("acosh(1.5)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(0.96242365, 6), t);
    });
    QUnit.test('asin_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("asin(0.5)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(0.523598776, 6), t);
    });
    QUnit.test('asinh_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("asinh(1.5)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.194763217, 6), t);
    });
    QUnit.test('atan_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("atan(1)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(0.785398163, 6), t);
    });
    QUnit.test('atanh_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("atanh(0.5)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(0.549306144, 6), t);
    });
    QUnit.test('atan2_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("atan2(1,2)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.107148718, 6), t);
    });
    QUnit.test('ROUND_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("ROUND(4.333,2)", 0.0);
        assert.equal(4.33, t);
    });
    QUnit.test('ROUNDDOWN_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("ROUNDDOWN(4.333,2)", 0.0);
        assert.equal(4.33, t);

        t = engine.TryEvaluate("ROUNDDOWN(-3.141592, 3)", 0.0);
        assert.equal(-3.141, t);
    });
    QUnit.test('ROUNDUP_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("ROUNDUP(4.333,2)", 0.0);
        assert.equal(4.34, t);

        t = engine.TryEvaluate("ROUNDUP(-3.141592, 3)", 0.0);
        assert.equal(-3.142, t);
    });
    QUnit.test('CEILING_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("CEILING(4.333,0.1)", 0.0);
        assert.equal(4.4, t);

        t = engine.TryEvaluate("CEILING(4.333)", 0.0);
        assert.equal(5, t);
    });
    QUnit.test('FLOOR_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("FLOOR(4.363,0.1)", 0.0);
        assert.equal(4.3, t);

        t = engine.TryEvaluate("FLOOR(4.333)", 0.0);
        assert.equal(4, t);
    });
    QUnit.test('even_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("even(4.363)", 0.0);
        assert.equal(6.0, t);
    });
    QUnit.test('odd_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("odd(4.363)", 0.0);
        assert.equal(5, t);
    });
    QUnit.test('MROUND_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("MROUND(4.363,2)", 0.0);
        assert.equal(4, t);
        t = engine.TryEvaluate("MROUND(5.363,2)", 0.0);
        assert.equal(6, t);
    });
    QUnit.test('Rand_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("RAND()", 0.0);
        assert.equal(t > 0, true);
        assert.equal(t < 1, true);
    });
    QUnit.test('RANDBETWEEN_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("RANDBETWEEN(2,99)", 0.0);
        assert.equal(t > 2, true);
        assert.equal(t < 99, true);
    });
    QUnit.test('fact_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("fact(6)", 0.0);
        assert.equal(720.0, t);
        t = engine.TryEvaluate("fact(3)", 0.0);
        assert.equal(6.0, t);
    });
    QUnit.test('factdouble_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("factdouble(10)", 0.0);
        assert.equal(3840.0, t);
    });
    QUnit.test('POWER_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("POWER(10,2)", 0.0);
        assert.equal(100.0, t);
    });
    QUnit.test('exp_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("exp(2)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(7.389056099, 6), t);
    });
    QUnit.test('ln_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("ln(4)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.386294361, 6), t);
    });
    QUnit.test('log_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("log(10)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.0, 6), t);

        t = engine.TryEvaluate("log(8,2)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(3.0, 6), t);
    });
    QUnit.test('log10_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("log10(10)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(1.0, 6), t);
    });
    QUnit.test('MULTINOMIAL_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("MULTINOMIAL(1,2,3)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(60.0, 6), t);
        t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(12600.0, 6), t);
    });
    QUnit.test('PRODUCT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("PRODUCT(1,2,3,4)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(24.0, 6), t);
    });
    QUnit.test('SQRTPI_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("SQRTPI(3)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(3.069980124, 6), t);
    });
    QUnit.test('SUMSQ_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("SUMSQ(1,2)", 0.0);
        t = Math.round(t, 6);
        assert.equal(Math.round(5.0, 6), t);
    });
    QUnit.test('transformation_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var num = engine.TryEvaluate("BIN2DEC(10101)", 0);
        assert.equal(num, 21);
        num = engine.TryEvaluate("OCT2DEC(12456)", 0);
        assert.equal(num, 5422);
        num = engine.TryEvaluate("HEX2DEC('213adf')", 0);
        assert.equal(num, 2177759);

        var t = engine.TryEvaluate("DEC2BIN(10)", "");
        assert.equal(t, "1010");
        t = engine.TryEvaluate("OCT2BIN('721')", "");
        assert.equal(t, "111010001");
        t = engine.TryEvaluate("HEX2BIN('fa')", "");
        assert.equal(t, "11111010");
        t = engine.TryEvaluate("'fa'.HEX2BIN()", "");
        assert.equal(t, "11111010");

        t = engine.TryEvaluate("BIN2OCT(10)", "");
        assert.equal(t, "2");
        t = engine.TryEvaluate("DEC2OCT('75')", "");
        assert.equal(t, "113");
        t = engine.TryEvaluate("HEX2OCT('f5')", "");
        assert.equal(t, "365");

        t = engine.TryEvaluate("BIN2HEX(101010100)", "");
        assert.equal(t, "154");
        t = engine.TryEvaluate("OCT2HEX(75212)", "");
        assert.equal(t, "7A8A");
        t = engine.TryEvaluate("DEC2HEX(952)", "");
        assert.equal(t, "3B8");
    });



}); 
  