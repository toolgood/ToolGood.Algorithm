Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
        (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
            RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}
QUnit.module('test-datetime', function () {
    QUnit.test('DATEVALUE_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("DATEVALUE('2016-01-01')",new Date());
        assert.equal(dt, "2016-01-01 00:00:00");
        dt = engine.TryEvaluate("DATEVALUE（'2016-01-01'）", new Date());
        assert.equal(dt, "2016-01-01 00:00:00");

        // chinese time  中国8时区
        dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", new Date());
        assert.equal(dt, "2023-08-05 11:28:19");// new Date(2023, 8, 5, 19, 28, 19));
         

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", new Date());
        assert.equal(dt, "2023-08-05 11:28:19");// new Date(2023, 8, 5, 19, 28, 19));


        engine.UseLocalTime = true;
        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", new Date());
        assert.equal(dt, "2023-08-05 19:28:19");// new Date(2023, 8, 5, 19, 28, 19));

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", new Date());
        assert.equal(dt, "2023-08-05 19:28:19");// new Date(2023, 8, 5, 19, 28, 19));

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899000')", new Date());
        assert.equal(dt, "2023-08-05 19:28:19");// new Date(2023, 8, 5, 19, 28, 19));

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899')", new Date());
        assert.equal(dt, "2023-08-05 19:28:19");// new Date(2023, 8, 5, 19, 28, 19));

    });


    QUnit.test('TIMESTAMP_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        engine.UseLocalTime = true;

        // chinese time
        var dt = engine.TryEvaluate("TIMESTAMP('2016-01-01')", 0);
        assert.equal(dt, 1451577600000);

        dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',0)", 0);
        assert.equal(dt, 1451577600000);

        dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',1)", 0);
        assert.equal(dt, 1451577600);
    });

    QUnit.test('TIMEVALUE_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("TIMEVALUE('12:12:12')", "");
        assert.equal(dt, "12:12:12");
    });

    QUnit.test('DATE_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("DATE(2016,1,1)", new Date());
        assert.equal(dt, "2016-01-01 00:00:00");
    });

    QUnit.test('time_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("time(11,12,13)", "");
        assert.equal(dt, "11:12:13");

    });

    QUnit.test('Now_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("now()", new Date());
        assert.equal(new Date(dt).format('yyyy-MM-dd hh:mm:ss'), new Date().format('yyyy-MM-dd hh:mm:ss'));
    });

    QUnit.test('Today_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Today()", new Date());
        assert.equal(new Date(dt).format('yyyy-MM-dd hh:mm:ss'), new Date().format('yyyy-MM-dd 00:00:00'));
    });

    QUnit.test('year_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("year(now())", 0);
        assert.equal(dt, new Date().getFullYear());
    });

    QUnit.test('Month_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Month(now())", 0);
        assert.equal(dt, new Date().getMonth()+1);
    });

    QUnit.test('Day_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Day(now())", 0);
        assert.equal(dt, new Date().getDate());
    });

    QUnit.test('Hour_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Hour(now())", 0);
        assert.equal(dt, new Date().getHours());
    });

    QUnit.test('Minute_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Minute(now())", 0);
        assert.equal(dt, new Date().getMinutes());
    });

    QUnit.test('Second_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Second(now())", 0);
        assert.equal(dt, new Date().getSeconds());
    });

    QUnit.test('WEEKDAY_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7))", 0);
        assert.equal(dt, 7);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),1)", 0);
        assert.equal(dt, 7);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),2)", 0);
        assert.equal(dt, 6);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),3)", 0);
        assert.equal(dt, 5);

        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),1)", 0);
        assert.equal(dt, 1);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),2)", 0);
        assert.equal(dt, 7);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),3)", 0);
        assert.equal(dt, 6);

        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),1)", 0);
        assert.equal(dt, 2);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),2)", 0);
        assert.equal(dt, 1);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),3)", 0);
        assert.equal(dt, 0);
    });

    QUnit.test('DATEDIF_Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0);
        assert.equal(dt, 41);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','m')", 0);
        assert.equal(dt, 503);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','d')", 0);
        assert.equal(dt, 15318);


        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','yd')", 0);
        assert.equal(dt, 342);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','ym')", 0);
        assert.equal(dt, 11);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','md')", 0);
        assert.equal(dt, 8);
    });
    QUnit.test('DAYS360_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("DAYS360('1975-1-30','2017-1-7')", 0);
        assert.equal(dt, 15097);
    });
    QUnit.test('EDATE_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("EDATE(\"2012-1-31\",32)", new Date());
        assert.equal(dt, "2014-09-30 00:00:00");
    });
    QUnit.test('EOMONTH_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("EOMONTH(\"2012-2-1\",32)", new Date());
        assert.equal(dt, "2014-10-31 00:00:00");
    });
    QUnit.test('NETWORKDAYS_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("NETWORKDAYS(\"2012-1-1\",\"2013-1-1\")", 0);
        assert.equal(dt, 262);
    });
    QUnit.test('WORKDAY_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("WORKDAY(\"2012-1-2\",145)", new Date());
        assert.equal(dt, "2012-07-23 00:00:00");
    });
    QUnit.test('WEEKNUM_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("WEEKNUM(\"2016-1-3\")", 0);
        assert.equal(dt, 2);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-2\")", 0);
        assert.equal(dt, 1);

        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-4\",2)", 0);
        assert.equal(dt, 2);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-3\",2)", 0);
        assert.equal(dt, 1);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-2\",2)", 0);
        assert.equal(dt, 1);
    });
    QUnit.test('Add_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("'2000-02-01'.addYears(1).year()", 0);
        assert.equal(dt, 2001);

        dt = engine.TryEvaluate("'2000-02-01'.AddMonths(1).Month()", 0);
        assert.equal(dt, 3);

        dt = engine.TryEvaluate("'2000-02-01'.AddDays(1).Day()", 0);
        assert.equal(dt, 2);

        dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddHours(1).Hour()", 0);
        assert.equal(dt, 13);

        dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddMinutes(1).Minute()", 0);
        assert.equal(dt, 6);

        dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddSeconds(1).Second()", 0);
        assert.equal(dt, 7);

        dt = engine.TryEvaluate("'2000-02-01 10:05:06'.AddSeconds(1).Second()", 0);
        assert.equal(dt, 7);

        dt = engine.TryEvaluate("'2000-02-01 20:05:06'.AddSeconds(1).Second()", 0);
        assert.equal(dt, 7);

        dt = engine.TryEvaluate("'2000-02-01 9:05:06'.AddSeconds(1).Second()", 0);
        assert.equal(dt, 7);

        dt = engine.TryEvaluate("'7:05:06'.AddSeconds(1).Second()", 0);
        assert.equal(dt, 7);

        dt = engine.TryEvaluate("'10:05:06'.AddSeconds(1).Second()", 0);
        assert.equal(dt, 7);

        dt = engine.TryEvaluate("'20:05:06'.AddSeconds(1).Second()", 0);
        assert.equal(dt, 7);

        // 错误 
        dt = engine.TryEvaluate("'2000-02-01 24:05:06'.AddSeconds(1).Second()", 0);
        assert.equal(dt, 0);
    });

});
   

    