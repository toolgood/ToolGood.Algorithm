QUnit.module('test-helper', function () {
    QUnit.test('test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var p = AlgorithmEngineHelper.GetDiyNames("[dd]");
        assert.equal("dd", p.Parameters[0].Name);
        p = AlgorithmEngineHelper.GetDiyNames("@dd");
        assert.equal("dd", p.Parameters[0].Name);
        p = AlgorithmEngineHelper.GetDiyNames("#dd#");
        assert.equal("dd", p.Parameters[0].Name);
        p = AlgorithmEngineHelper.GetDiyNames("dd");
        assert.equal("dd", p.Parameters[0].Name);

        // 注，这里的 ddd 是数组内有 ddd
        var p2 = AlgorithmEngineHelper.GetDiyNames("{ddd}");
        assert.equal("ddd", p2.Parameters[0].Name);

        var p3 = AlgorithmEngineHelper.GetDiyNames("【dd】");
        assert.equal("dd", p3.Parameters[0].Name);
        p3 = AlgorithmEngineHelper.GetDiyNames("【dd.1】");
        assert.equal("dd.1", p3.Parameters[0].Name);

        var p4 = AlgorithmEngineHelper.GetDiyNames("@ddd+2");
        assert.equal("ddd", p4.Parameters[0].Name);
        p4 = AlgorithmEngineHelper.GetDiyNames("ddd+2");
        assert.equal("ddd", p4.Parameters[0].Name);

        var p5 = AlgorithmEngineHelper.GetDiyNames("ddd(d1,22)");
        assert.equal("ddd", p5.Functions[0]);
        assert.equal("d1", p5.Parameters[0].Name);

        var p6 = AlgorithmEngineHelper.GetDiyNames("长");
        assert.equal("长", p6.Parameters[0].Name);

        var p7 = AlgorithmEngineHelper.GetDiyNames("#ddd#+2");
        assert.equal("ddd", p7.Parameters[0].Name);
    });
    QUnit.test('Test2', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var b = AlgorithmEngineHelper.IsKeywords("false");
        assert.equal(true, b);
    });
    QUnit.test('Test3', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var b = AlgorithmEngineHelper.UnitConversion(1, "米", "千米", "测试");
        assert.equal(0.001, b);
        b = AlgorithmEngineHelper.UnitConversion(1, "米", "分米", "测试");
        assert.equal(10, b);
        b = AlgorithmEngineHelper.UnitConversion(1, "米", "厘米", "测试");
        assert.equal(100, b);
        b = AlgorithmEngineHelper.UnitConversion(1, "米", "mm", "测试");
        assert.equal(1000, b);


        b = AlgorithmEngineHelper.UnitConversion(1, "m2", "dm2", "测试");
        assert.equal(100, b);
        b = AlgorithmEngineHelper.UnitConversion(1, "m2", "cm2", "测试");
        assert.equal(10000, b);
        b = AlgorithmEngineHelper.UnitConversion(1, "m2", "mm2", "测试");
        assert.equal(1000000, b);


        b = AlgorithmEngineHelper.UnitConversion(1, "m3", "dm3", "测试");
        assert.equal(1000, b);
        b = AlgorithmEngineHelper.UnitConversion(1, "m3", "cm3", "测试");
        assert.equal(1000000, b);
        b = AlgorithmEngineHelper.UnitConversion(1, "m3", "mm3", "测试");
        assert.equal(1000000000, b);


        b = AlgorithmEngineHelper.UnitConversion(1, "t", "kg", "测试");
        assert.equal(1000, b);
        b = AlgorithmEngineHelper.UnitConversion(1, "t", "g", "测试");
        assert.equal(1000000, b);
    });

}); 
 
 