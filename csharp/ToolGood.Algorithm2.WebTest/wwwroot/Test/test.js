
QUnit.module('test', function () {
    QUnit.test('Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));

        engine = new AlgorithmEngine();

        var c = engine.TryEvaluate("2+3", 0);
        assert.equal(5, c);
        c = engine.TryEvaluate("(2)+3", 0);
        assert.equal(5, c);
        c = engine.TryEvaluate("2+3*2+10/2*4", 0);
        assert.equal(28, c);

        c = engine.TryEvaluate("if(2+3*2+10/2*4,1", 0);
        assert.equal(0, c);

        c = engine.TryEvaluate("2.1e3 + 10", 0);
        assert.equal(2110, c);

        c = engine.TryEvaluate("2.1e+03 + 10", 0);
        assert.equal(2110, c);

        c = engine.TryEvaluate("2.1e+3 + 10", 0);
        assert.equal(2110, c);

        var d = engine.TryEvaluate("2.1e-3 + 10", 0.0);
        assert.equal(10.0021, d);


        var e = engine.TryEvaluate("e", 0.0);
        assert.equal(Math.round(Math.E, 10), Math.round(e, 10));
        e = engine.TryEvaluate("pi", 0.0);
        assert.equal(Math.round(Math.PI, 10), Math.round(e, 10));

        var b = engine.TryEvaluate("true", false);
        assert.equal(true, b);
        b = engine.TryEvaluate("false", true);
        assert.equal(false, b);

        var b1 = engine.TryEvaluate("if(true,1,2)", 0);
        assert.equal(1, b1);

        b1 = engine.TryEvaluate("if(false,1,2)", 0);
        assert.equal(2, b1);

        var b2 = engine.TryEvaluate("pi*4", 0.0);
        assert.equal(Math.round(Math.PI * 4, 10), Math.round(b2, 10));
        b2 = engine.TryEvaluate("e*4", 0.0);
        assert.equal(Math.round(Math.E * 4, 10), Math.round(b2, 10));

        var s = engine.TryEvaluate("'aa'&'bb'", "");
        assert.equal("aabb", s);

        s = engine.TryEvaluate("'3'+2", "");
        assert.equal("5", s);

        var r = engine.TryEvaluate("count(Array(1,2,3,4))", 0);
        assert.equal(4, r);


        r = engine.TryEvaluate("(1=1)*9+2", 0);
        assert.equal(11, r);;
        r = engine.TryEvaluate("(1=2)*9+2", 0);
        assert.equal(2, r);;

        var dt = engine.TryEvaluate("'2016-1-1'+1", new Date());
        assert.equal("2016-01-02 00:00:00", dt);
        dt = engine.TryEvaluate("'2016-1-1'+9*'1:0'", new Date());
        assert.equal("2016-01-01 09:00:00", dt);


        var value = engine.TryEvaluate("1 > (-2)", false);
        assert.equal(value, true);


        value = engine.TryEvaluate("(-1) > (-2）", false);
        assert.equal(value, true);


        value = engine.TryEvaluate("-1 > (-2)", false);
        assert.equal(value, true);

        value = engine.TryEvaluate("-1 > -2", false);
        assert.equal(value, true);

        var value2 = engine.TryEvaluate("-1 > -2", false);
        assert.equal(value2, true);


        var value3 = engine.TryEvaluate("-7 < -2", false);
        assert.equal(value3, true);

        var t1 = engine.TryEvaluate("-7 < -2 ?1 : 2", 0);
        assert.equal(t1, 1);
        t1 = engine.TryEvaluate("-7 < -2 ?1 ： 2", 0);
        assert.equal(t1, 1);

        t1 = engine.TryEvaluate("-7 < -2 ?1 ：2", 0);
        assert.equal(t1, 1);
        t1 = engine.TryEvaluate("-7 < -2 ？ 1 : 2", 0);
        assert.equal(t1, 1);
        t1 = engine.TryEvaluate("-7 < -2 ？1 : 2", 0);
        assert.equal(t1, 1);

        t1 = engine.TryEvaluate("-7 < -2 ？1 ： 2", 0);
        assert.equal(t1, 1);

        t1 = engine.TryEvaluate("(!(-7 < -2))？1：2", 0);
        assert.equal(t1, 2);
        t1 = engine.TryEvaluate("1>2？1：2", 0);
        assert.equal(t1, 2);

        t1 = engine.TryEvaluate("1！=2？1：2", 0);
        assert.equal(t1, 1);

        var t2 = engine.TryEvaluate("Ａsc('ａｂｃＡＢＣ１２３')", "");
        assert.equal(t2, "abcABC123");
    });

    QUnit.test('base_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));

        engine = new AlgorithmEngine();
        var t = engine.TryEvaluate("1+(3*2+2)/2", 0);
        assert.equal(5, t);

        t = engine.TryEvaluate("(8-3)*(3+2)", 0);
        assert.equal(25, t);

        t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0);
        assert.equal(4, t);

        var b = engine.TryEvaluate("1=1", false);
        assert.equal(true, b);

        b = engine.TryEvaluate("1=2", true);
        assert.equal(false, b);

        b = engine.TryEvaluate("1<>2", false);
        assert.equal(true, b);

        b = engine.TryEvaluate("1!=2", false);
        assert.equal(true, b);

        b = engine.TryEvaluate("1>2", true);
        assert.equal(false, b);

        b = engine.TryEvaluate("1<2", false);
        assert.equal(true, b);

        b = engine.TryEvaluate("1<=2", false);
        assert.equal(true, b);

        b = engine.TryEvaluate("1>=2", true);
        assert.equal(false, b);

        b = engine.TryEvaluate("'1'='1'", false);
        assert.equal(true, b);
        b = engine.TryEvaluate("'e'='e'", false);
        assert.equal(true, b);
        b = engine.TryEvaluate("'1'='2'", true);
        assert.equal(false, b);
        b = engine.TryEvaluate("'1'!='2'", false);
        assert.equal(true, b);
    });

    QUnit.test('base_test3', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));

        engine = new AlgorithmEngine();

        var c = engine.TryEvaluate("(2)+/*123456*/3", 0);
        assert.equal(5, c);

        c = engine.TryEvaluate("2+3//eee", 0);
        assert.equal(5, c);

        c = engine.TryEvaluate("(2)+/*123456*/3 ee22+22", 0);
        assert.equal(0, c);
    });

    QUnit.test('base_test4', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));

        engine = new AlgorithmEngine();

        var c = engine.TryEvaluate("'4dd'&'55' rr", "");
        assert.equal("", c);
    });

    QUnit.test('base_test5', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));

        engine = new AlgorithmEngine();

        var c = engine.TryEvaluate("'4dd'&'55'.left(1)", "");
        assert.equal("4dd5", c);
    });

    QUnit.test('Cylinder_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));

        c = new AlgorithmEngine();
        var data = { '半径': 3, '直径': 6, '高': 10 };

        var t = c.TryEvaluate("[半径]*[半径]*pi()", 0.0, data);      //圆底面积
        var t2 = c.TryEvaluate("半径*半径*pi()", 0.0, data);      //圆底面积

        var t4 = c.TryEvaluate("@半径*@半径*pi()", 0.0, data);      //圆底面积
        var t5 = c.TryEvaluate("#半径#*#半径#*pi()", 0.0, data);      //圆底面积
        var t6 = c.TryEvaluate("【半径】*【半径】*pi()", 0.0, data);      //圆底面积
        var t7 = c.TryEvaluate("【半径】*【半径】*pi（）", 0.0, data);      //圆底面积

        assert.equal(t, t2);
        //assert.equal(t, t3);
        assert.equal(t, t4);
        assert.equal(t, t5);
        assert.equal(t, t6);
        assert.equal(t, t7);



        t = c.TryEvaluate("[直径]*pi()", 0.0, data);            //圆的长
        t = c.TryEvaluate("[半径]*[半径]*pi()*[高]", 0.0, data); //圆的体积

        t = c.TryEvaluate("['半径']*[半径]*pi()*[高]", 0.0, data); //圆的体积

        //t = c.TryEvaluate("求面积（10）", 0.0, data); //圆的体积
        //assert.equal(10 * 10 * Math.PI, t, 10, data);


        var json = {'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'};

        var tt = c.TryEvaluate("['灰色']", "", json); //圆的体积
        assert.equal("L", tt);


        var tt2 = c.EvaluateFormula("'圆'-[半径]-高", '-', data);
        assert.equal("圆-3-10", tt2);
    });


    QUnit.test('Test5555', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));

        engine = new AlgorithmEngine();
        var data = { '半径': 3, '直径': 6, '高': 10 };


        var t = c.GetSimplifiedFormula("[半径]*[半径]*pi()", data); // 圆底面积
        assert.equal("3 * 3 * 3.14159265358979", t);

        var t2 = c.GetSimplifiedFormula("半径*if(半径>2,1,3)", data);
        assert.equal("3 * 1", t2);

        var t24 = c.GetSimplifiedFormula("半径*if(半径>2,1+4,3)", data);
        assert.equal("3 * 5", t24);
    });
});

 