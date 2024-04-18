QUnit.module('test-csharp', function () {
    QUnit.test('UrlDecode', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", null);
        assert.equal(dt, "&=我中国人 >||");
    });
    QUnit.test('UrlEncode', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("UrlEncode('&=我中国人 >||')", null);
        assert.equal(dt, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c");
    });
    QUnit.test('HtmlEncode', async function (assert) {
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", null);
        assert.equal(dt, "&amp;=我中国人 &gt;||");
    });
    QUnit.test('HtmlDecode', async function (assert) {
        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", null);
        assert.equal(dt, "&=我中国人 >||");
    });

    QUnit.test('TextToBase64', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var r = engine.TryEvaluate("TextToBase64('&=我中国人 >||')", "");
        assert.equal(r, "Jj3miJHkuK3lm73kurogPnx8");

        r = engine.TryEvaluate("TextToBase64('&=我中国人 >||','GBK')", "");
        assert.equal(r, "Jj3O0tbQufrIyyA+fHw=");
    });

    QUnit.test('TextToBase64Url', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", null);
        assert.equal(r, "Jj3miJHkuK3lm73kurogPnx8");

        r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||','GBK')", null);
        assert.equal(r, "Jj3O0tbQufrIyyA-fHw");
    });
    QUnit.test('Base64ToText', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var r = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", null);
        assert.equal(r, "&=我中国人 >||");

        r = engine.TryEvaluate("Base64ToText('Jj3O0tbQufrIyyA+fHw=','GBK')", null);
        assert.equal(r, "&=我中国人 >||");
    });
    QUnit.test('Base64UrlToText', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var r = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", null);
        assert.equal(r, "&=我中国人 >||");

        r = engine.TryEvaluate("Base64UrlToText('Jj3O0tbQufrIyyA-fHw','GBK')", null);
        assert.equal(r, "&=我中国人 >||");
    });
    QUnit.test('Regex_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        engine.UseExcelIndex = false;
        var r = engine.TryEvaluate("Regex('abcd','a.*c')", null);
        assert.equal(r, "abc");
    });
    QUnit.test('IsRegex_test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var r = engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
        assert.equal(r, true);
        r = engine.TryEvaluate("IsRegex('abcd','da.*c')", true);
        assert.equal(r, false);
    });
    QUnit.test('Guid', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var r = engine.TryEvaluate("Guid()", "");
        assert.notEqual(r, null);

    });
    QUnit.test('Md5', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));

        var engine = new AlgorithmEngine();
        //var dt2 = engine.TryEvaluate("Md5('&=我中国人 >||')", null);
        //assert.equal(dt2, "2E1CEFBDFA3677725B7856E02D225819"); //不支持Md5
   

        var  dt = engine.TryEvaluate("Md5('&=我中国人 >||','GGG')", null);
        assert.equal(dt, null);
        dt = engine.TryEvaluate("Sha1('&=我中国人 >||')", null);
        assert.equal(dt, "F2C250C58F3A40DC54B5A47F0F6B1187AD5AC2EE");
        dt = engine.TryEvaluate("Sha256('&=我中国人 >||')", null);
        assert.equal(dt, "FA5BF04D13AEF750D62040E492479A16B6B10888D0B19923A1E7B9339990632A");
        dt = engine.TryEvaluate("Sha512('&=我中国人 >||')", null);
        assert.equal(dt, "FFEAC98C39D76CD86A3AB8ECEF16BE23166F68E1A3C5C9809A8AD2CE417170465286E4CF6FFA17924613CD7477533B9109A5DD504A2462F9DB693D56AD365C14");
 
        dt = engine.TryEvaluate("Crc32('&=我中国人 >||')", null);
        assert.equal(dt, "60649EFF");
        //dt = engine.TryEvaluate("HmacMd5('&=我中国人 >||','12')", null);
        //assert.equal(dt, "CF3923196E21B1E270FD72B089B092BB"); //不支持HmacMd5
 

        dt = engine.TryEvaluate("HmacSha1('&=我中国人 >||','12')", null);
        assert.equal(dt, "EB4D4FC2AA5637060FD12004DF845801D8902105");
        dt = engine.TryEvaluate("HmacSha256('&=我中国人 >||','12')", null);
        assert.equal(dt, "3E25E0D14039E8258BBBBD15F7E3B91BB497A8966C12E1DEA3D651BF03CB4B97");
        dt = engine.TryEvaluate("HmacSha512('&=我中国人 >||','12')", null);
        assert.equal(dt, "4E9CE785C46569965C7C712A841EC7382C64D918D49F992EDB3504BED9C3A5EFBB1C8F712968F6B904417E07F9D72E707FCF148D55A4D3EDF1A9866B7BAC2049");
    });
    QUnit.test('IndexOf', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        engine.UseExcelIndex = false;
        var dt = engine.TryEvaluate("IndexOf('abcd','cd')", -1);
        assert.equal(dt, 2);
        dt = engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
        assert.equal(dt, 2);
    });

    QUnit.test('Split', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Split('1,2,3,4',',')[3]", null);
        assert.equal(dt, "3");
    });

    QUnit.test('TrimStart', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("TrimStart(' 123 ')", null);
        assert.equal(dt, "123 ");

        dt = engine.TryEvaluate("TrimStart(' 123 ',' 1')", null);
        assert.equal(dt, "23 ");
    });

    QUnit.test('TrimEnd', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("TrimEnd(' 123 ')", null);
        assert.equal(dt, " 123");

        dt = engine.TryEvaluate("TrimEnd(' 123 ','3 ')", null);
        assert.equal(dt, " 12");
    });

    QUnit.test('Join', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Join(',',1,2,5,6)", null);
        assert.equal(dt, "1,2,5,6");
        dt = engine.TryEvaluate("Join(',',1,2,5,6,split('7,8,9',','))", null);
        assert.equal(dt, "1,2,5,6,7,8,9");
    });

    QUnit.test('Substring', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("Substring('123456789',1,2)", null);
        assert.equal(dt, "12");
    });
    QUnit.test('StartsWith', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("StartsWith('123456789','12')", false);
        assert.equal(dt, true);
        dt = engine.TryEvaluate("StartsWith('123456789','127')", true);
        assert.equal(dt, false);
    });
    QUnit.test('EndsWith', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("EndsWith('123456789','89')", false);
        assert.equal(dt, true);
        dt = engine.TryEvaluate("EndsWith('123456789','127')", true);
        assert.equal(dt, false);
    });
    QUnit.test('IsNullOrEmpty', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("IsNullOrEmpty('')", false);
        assert.equal(dt, true);
        dt = engine.TryEvaluate("IsNullOrEmpty(' ')", true);
        assert.equal(dt, false);
    });
    QUnit.test('IsNullOrWhiteSpace', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("IsNullOrWhiteSpace('')", false);
        assert.equal(dt, true);
        dt = engine.TryEvaluate("IsNullOrWhiteSpace('   ')", false);
        assert.equal(dt, true);
        dt = engine.TryEvaluate("IsNullOrWhiteSpace(' f ')", true);
        assert.equal(dt, false);
    });
    QUnit.test('RemoveStart', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("RemoveStart('123456789','12')", null);
        assert.equal(dt, "3456789");
        dt = engine.TryEvaluate("RemoveStart('123456789','127')", null);
        assert.equal(dt, "123456789");
    });
    QUnit.test('RemoveEnd', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("RemoveEnd('123456789','89')", null);
        assert.equal(dt, "1234567");
        dt = engine.TryEvaluate("RemoveEnd('123456789','127')", null);
        assert.equal(dt, "123456789");
    });
    QUnit.test('Json', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 250));

        var engine = new AlgorithmEngine();
        var dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", null);
        assert.equal(dt, "51");
        dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Birthday]", null);
        assert.equal(dt, "04/26/1564 00:00:00");

        dt = engine.TryEvaluate("json('[1,2,3]')[1]", null);
        assert.equal(dt, "1");

        dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Name].Trim()", null);
        assert.equal(dt, "William Shakespeare");


        dt = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null);
        assert.equal(dt, "ill");

        dt = engine.TryEvaluate("json('12346{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null);
        assert.equal(dt, null);

        dt = engine.TryEvaluate("json('[1,2,3,4,5,6]')[1].Trim()", null);
        assert.equal(dt, "1");

        dt = engine.TryEvaluate("json('[1,2,3,4,5,6]22')[1].Trim()", null);
        assert.equal(dt, null);

        dt = engine.TryEvaluate("json('22[1,2,3,4,5,6]')[1].Trim()", null);
        assert.equal(dt, null);
    });


});
  
 
  