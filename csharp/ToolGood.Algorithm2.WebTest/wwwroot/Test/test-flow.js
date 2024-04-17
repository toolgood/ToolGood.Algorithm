QUnit.module('test-flow', function () {
    QUnit.test('If_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("if(1=1,1,2)", 0);
        assert.equal(1, t);
        t = engine.TryEvaluate("if(1=1,1)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("if(1=1，1)", 0);
        assert.equal(1, t);


        t = engine.TryEvaluate("if(3,1,2)", 0);
        assert.equal(1, t);
        t = engine.TryEvaluate("if('1',1,2)", 0);
        assert.equal(1, t);
        t = engine.TryEvaluate("if(0,1,2)", 0);
        assert.equal(2, t);
    });
    QUnit.test('iferror_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("iferror(1/0,1,2)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("iferror(1-'rrr',1,2)", 0);
        assert.equal(1, t);
    });
    QUnit.test('iferror_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("iserror(1/0,1)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("iserror(1-'rrr',1)", 0);
        assert.equal(1, t);
    });
    QUnit.test('ifnull_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("isnull(null,1)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("isnull(1,2)", 0);
        assert.equal(1, t);
    });
    QUnit.test('isnullorerror_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("isnullorerror(null,1)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("isnullorerror(1/0,1)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("isnullorerror(1,2)", 0);
        assert.equal(1, t);
    });
    QUnit.test('ISNUMBER_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("if(ISNUMBER(1),1,2)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("if(ISNUMBER('e'),1,2)", 0);
        assert.equal(2, t);
        t = engine.TryEvaluate("if(ISNUMBER('11'),1,2)", 0);
        assert.equal(2, t);

        t = engine.TryEvaluate("if(ISNUMBER('2016-1-2'),1,2)", 0);
        assert.equal(2, t);
    });
    QUnit.test('ISTEXT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("if(ISTEXT(1),1,2)", 0);
        assert.equal(2, t);

        t = engine.TryEvaluate("if(ISTEXT('e'),1,2)", 0);
        assert.equal(1, t);
        t = engine.TryEvaluate("if(ISTEXT('11'),1,2)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("if(ISTEXT('2016-1-2'),1,2)", 0);
        assert.equal(1, t);
    });

    QUnit.test('ISNONTEXT_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("if(ISNONTEXT(1),1,2)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("if(ISNONTEXT('e'),1,2)", 0);
        assert.equal(2, t);
        t = engine.TryEvaluate("if(ISNONTEXT('11'),1,2)", 0);
        assert.equal(2, t);

        t = engine.TryEvaluate("if(ISNONTEXT('2016-1-2'),1,2)", 0);
        assert.equal(2, t);
    });

    QUnit.test('ISLOGICAL_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("if(ISLOGICAL(1),1,2)", 0);
        assert.equal(2, t);

        t = engine.TryEvaluate("if(ISLOGICAL('e'),1,2)", 0);
        assert.equal(2, t);

        t = engine.TryEvaluate("if(ISLOGICAL(true),1,2)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("if(ISLOGICAL(false),1,2)", 0);
        assert.equal(1, t);

    });

    QUnit.test('ISEVEN_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("if(ISEVEN(1),1,2)", 0);
        assert.equal(2, t);

        t = engine.TryEvaluate("if(ISEVEN(2),1,2)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("if(ISEVEN('e'),1,2)", 0);
        assert.equal(2, t);
    });

    QUnit.test('ISODD_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("if(ISODD(1),1,2)", 0);
        assert.equal(1, t);

        t = engine.TryEvaluate("if(ISODD(2),1,2)", 0);
        assert.equal(2, t);

        t = engine.TryEvaluate("if(ISODD('e'),1,2)", 0);
        assert.equal(2, t);
    });
    QUnit.test('And_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("and(true(),1=1)", false);
        assert.equal(true, t);
        t = engine.TryEvaluate("and(true(),1)", false);
        assert.equal(true, t);

        t = engine.TryEvaluate("and(true(),false(),1=1)", true);
        assert.equal(false, t);
        t = engine.TryEvaluate("and(true(),false(),1)", true);
        assert.equal(false, t);

        t = engine.TryEvaluate("and(true(),0)", true);
        assert.equal(false, t);
    });
    QUnit.test('Or_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("or(true(),1=1)", false);
        assert.equal(true, t);
        t = engine.TryEvaluate("or(true(),1)", false);
        assert.equal(true, t);

        t = engine.TryEvaluate("or(true(),false(),1=1)", false);
        assert.equal(true, t);
        t = engine.TryEvaluate("or(true(),false(),1)", false);
        assert.equal(true, t);

        t = engine.TryEvaluate("or(true(),0)", false);
        assert.equal(true, t);

        t = engine.TryEvaluate("or(false(),1=2)", true);
        assert.equal(false, t);
    });
    QUnit.test('Not_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("not(true())", true);
        assert.equal(false, t);
        t = engine.TryEvaluate("not(false())", false);
        assert.equal(true, t);
    });
    QUnit.test('true_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("true()", false);
        assert.equal(true, t);
    });
    QUnit.test('false_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("false()", true);
        assert.equal(false, t);
    });
    QUnit.test('andor_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("1=1 && 2==2 and 3=3", false);
        assert.equal(true, t);

        t = engine.TryEvaluate("1=1 && 2!=2", true);
        assert.equal(false, t);

        t = engine.TryEvaluate("1=1 && 2!=2", true);
        assert.equal(false, t);

        t = engine.TryEvaluate("1=1 || 2!=2", false);
        assert.equal(true, t);


        t = engine.TryEvaluate("1=1 and 2==2", false);
        assert.equal(true, t);

        t = engine.TryEvaluate("1=1 and 2!=2", true);
        assert.equal(false, t);

        t = engine.TryEvaluate("1=1 and 2!=2", true);
        assert.equal(false, t);

        t = engine.TryEvaluate("1=1 or 2!=2", false);
        assert.equal(true, t);
    });

});
       

