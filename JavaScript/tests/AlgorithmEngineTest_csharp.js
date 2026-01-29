import assert from 'assert';
import { AlgorithmEngine } from '../src/AlgorithmEngine.js';
import { AlgorithmEngineEx } from '../src/AlgorithmEngineEx.js';

// 扩展 AlgorithmEngine 类以添加 TryEvaluate 方法
class AlgorithmEngineWithTryEvaluate extends AlgorithmEngine {
  TryEvaluate(exp, def) {
    const type = typeof def;
    switch (type) {
      case 'number':
        if (Number.isInteger(def)) {
          return this.TryEvaluate_Int32(exp, def);
        } else {
          return this.TryEvaluate_Double(exp, def);
        }
      case 'string':
        return this.TryEvaluate_String(exp, def);
      case 'boolean':
        return this.TryEvaluate_Boolean(exp, def);
      default:
        return def;
    }
  }
}

// 测试用例
function testUrlDecode() {
  console.log('开始测试 UrlDecode...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let urlDecode = engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", "");
  assert.strictEqual(urlDecode, "&=我中国人 >||", "UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c') 应该是 '&=我中国人 >||'");
  
  console.log('UrlDecode 测试通过！');
}

function testUrlEncode() {
  console.log('开始测试 UrlEncode...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let urlEncode = engine.TryEvaluate("UrlEncode('&=我中国人 >||')", "");
  assert.strictEqual(urlEncode, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c", "UrlEncode('&=我中国人 >||') 应该是 '%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c'");
  
  console.log('UrlEncode 测试通过！');
}

function testHtmlEncode() {
  console.log('开始测试 HtmlEncode...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let htmlEncode = engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", "");
  assert.strictEqual(htmlEncode, "&amp;=我中国人 &gt;||", "HtmlEncode('&=我中国人 >||') 应该是 '&amp;=我中国人 &gt;||'");
  
  console.log('HtmlEncode 测试通过！');
}

function testHtmlDecode() {
  console.log('开始测试 HtmlDecode...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let htmlDecode = engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", "");
  assert.strictEqual(htmlDecode, "&=我中国人 >||", "HtmlDecode('&amp;=我中国人 &gt;||') 应该是 '&=我中国人 >||'");
  
  console.log('HtmlDecode 测试通过！');
}

function testTextToBase64() {
  console.log('开始测试 TextToBase64...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let textToBase64 = engine.TryEvaluate("TextToBase64('&=我中国人 >||')", "");
  assert.strictEqual(textToBase64, "Jj3miJHkuK3lm73kurogPnx8", "TextToBase64('&=我中国人 >||') 应该是 'Jj3miJHkuK3lm73kurogPnx8'");
  
  textToBase64 = engine.TryEvaluate("TextToBase64('&=我中国人 >||','GBK')", "");
  assert.strictEqual(textToBase64, "Jj3O0tbQufrIyyA+fHw=", "TextToBase64('&=我中国人 >||','GBK') 应该是 'Jj3O0tbQufrIyyA+fHw='");
  
  console.log('TextToBase64 测试通过！');
}

function testTextToBase64Url() {
  console.log('开始测试 TextToBase64Url...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let textToBase64Url = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", "");
  assert.strictEqual(textToBase64Url, "Jj3miJHkuK3lm73kurogPnx8", "TextToBase64Url('&=我中国人 >||') 应该是 'Jj3miJHkuK3lm73kurogPnx8'");
  
  textToBase64Url = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||','GBK')", "");
  assert.strictEqual(textToBase64Url, "Jj3O0tbQufrIyyA-fHw", "TextToBase64Url('&=我中国人 >||','GBK') 应该是 'Jj3O0tbQufrIyyA-fHw'");
  
  console.log('TextToBase64Url 测试通过！');
}

function testBase64ToText() {
  console.log('开始测试 Base64ToText...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let base64ToText = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", "");
  assert.strictEqual(base64ToText, "&=我中国人 >||", "Base64ToText('Jj3miJHkuK3lm73kurogPnx8') 应该是 '&=我中国人 >||'");
  
  base64ToText = engine.TryEvaluate("Base64ToText('Jj3O0tbQufrIyyA+fHw=','GBK')", "");
  assert.strictEqual(base64ToText, "&=我中国人 >||", "Base64ToText('Jj3O0tbQufrIyyA+fHw=','GBK') 应该是 '&=我中国人 >||'");
  
  console.log('Base64ToText 测试通过！');
}

function testBase64UrlToText() {
  console.log('开始测试 Base64UrlToText...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let base64UrlToText = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", "");
  assert.strictEqual(base64UrlToText, "&=我中国人 >||", "Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8') 应该是 '&=我中国人 >||'");
  
  base64UrlToText = engine.TryEvaluate("Base64UrlToText('Jj3O0tbQufrIyyA-fHw','GBK')", "");
  assert.strictEqual(base64UrlToText, "&=我中国人 >||", "Base64UrlToText('Jj3O0tbQufrIyyA-fHw','GBK') 应该是 '&=我中国人 >||'");
  
  console.log('Base64UrlToText 测试通过！');
}

function testRegex() {
  console.log('开始测试 Regex...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseExcelIndex = false;
  
  let regex = engine.TryEvaluate("Regex('abcd','a.*c')", "");
  assert.strictEqual(regex, "abc", "Regex('abcd','a.*c') 应该是 'abc'");
  
  console.log('Regex 测试通过！');
}

function testIsRegex() {
  console.log('开始测试 IsRegex...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let isRegex = engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
  assert.strictEqual(isRegex, true, "IsRegex('abcd','a.*c') 应该是 true");
  
  isRegex = engine.TryEvaluate("IsRegex('abcd','da.*c')", true);
  assert.strictEqual(isRegex, false, "IsRegex('abcd','da.*c') 应该是 false");
  
  console.log('IsRegex 测试通过！');
}

function testGuid() {
  console.log('开始测试 Guid...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let guid = engine.TryEvaluate("Guid()", "");
  assert.ok(typeof guid === 'string', "Guid() 应该返回一个字符串");
  
  console.log('Guid 测试通过！');
}

function testHash() {
  console.log('开始测试 Hash...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let md5 = engine.TryEvaluate("Md5('&=我中国人 >||')", "");
  assert.strictEqual(md5, "2E1CEFBDFA3677725B7856E02D225819", "Md5('&=我中国人 >||') 应该是 '2E1CEFBDFA3677725B7856E02D225819'");
  
  let sha1 = engine.TryEvaluate("Sha1('&=我中国人 >||')", "");
  assert.strictEqual(sha1, "F2C250C58F3A40DC54B5A47F0F6B1187AD5AC2EE", "Sha1('&=我中国人 >||') 应该是 'F2C250C58F3A40DC54B5A47F0F6B1187AD5AC2EE'");
  
  let sha256 = engine.TryEvaluate("Sha256('&=我中国人 >||')", "");
  assert.strictEqual(sha256, "FA5BF04D13AEF750D62040E492479A16B6B10888D0B19923A1E7B9339990632A", "Sha256('&=我中国人 >||') 应该是 'FA5BF04D13AEF750D62040E492479A16B6B10888D0B19923A1E7B9339990632A'");
  
  let crc32 = engine.TryEvaluate("Crc32('&=我中国人 >||')", "");
  assert.strictEqual(crc32, "60649EFF", "Crc32('&=我中国人 >||') 应该是 '60649EFF'");
  
  console.log('Hash 测试通过！');
}

function testIndexOf() {
  console.log('开始测试 IndexOf...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  engine.UseExcelIndex = false;
  
  let indexOf = engine.TryEvaluate("IndexOf('abcd','cd')", -1);
  assert.strictEqual(indexOf, 2, "IndexOf('abcd','cd') 应该是 2");
  
  let lastIndexOf = engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
  assert.strictEqual(lastIndexOf, 2, "LastIndexOf('abcd','cd') 应该是 2");
  
  console.log('IndexOf 测试通过！');
}

function testSplit() {
  console.log('开始测试 Split...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let split = engine.TryEvaluate("Split('1,2,3,4',',')[3]", "");
  assert.strictEqual(split, "3", "Split('1,2,3,4',',')[3] 应该是 '3'");
  
  console.log('Split 测试通过！');
}

function testTrimStart() {
  console.log('开始测试 TrimStart...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let trimStart = engine.TryEvaluate("TrimStart(' 123 ')", "");
  assert.strictEqual(trimStart, "123 ", "TrimStart(' 123 ') 应该是 '123 '");
  
  trimStart = engine.TryEvaluate("TrimStart(' 123 ',' 1')", "");
  assert.strictEqual(trimStart, "23 ", "TrimStart(' 123 ',' 1') 应该是 '23 '");
  
  console.log('TrimStart 测试通过！');
}

function testTrimEnd() {
  console.log('开始测试 TrimEnd...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let trimEnd = engine.TryEvaluate("TrimEnd(' 123 ')", "");
  assert.strictEqual(trimEnd, " 123", "TrimEnd(' 123 ') 应该是 ' 123'");
  
  trimEnd = engine.TryEvaluate("TrimEnd(' 123 ','3 ')", "");
  assert.strictEqual(trimEnd, " 12", "TrimEnd(' 123 ','3 ') 应该是 ' 12'");
  
  console.log('TrimEnd 测试通过！');
}

function testJoin() {
  console.log('开始测试 Join...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let join = engine.TryEvaluate("Join(',',1,2,5,6)", "");
  assert.strictEqual(join, "1,2,5,6", "Join(',',1,2,5,6) 应该是 '1,2,5,6'");
  
  join = engine.TryEvaluate("Join(',',1,2,5,6,split('7,8,9',','))", "");
  assert.strictEqual(join, "1,2,5,6,7,8,9", "Join(',',1,2,5,6,split('7,8,9',',')) 应该是 '1,2,5,6,7,8,9'");
  
  console.log('Join 测试通过！');
}

function testSubstring() {
  console.log('开始测试 Substring...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let substring = engine.TryEvaluate("Substring('123456789',1,2)", "");
  assert.strictEqual(substring, "12", "Substring('123456789',1,2) 应该是 '12'");
  
  console.log('Substring 测试通过！');
}

function testStartsWith() {
  console.log('开始测试 StartsWith...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let startsWith = engine.TryEvaluate("StartsWith('123456789','12')", false);
  assert.strictEqual(startsWith, true, "StartsWith('123456789','12') 应该是 true");
  
  startsWith = engine.TryEvaluate("StartsWith('123456789','127')", true);
  assert.strictEqual(startsWith, false, "StartsWith('123456789','127') 应该是 false");
  
  console.log('StartsWith 测试通过！');
}

function testEndsWith() {
  console.log('开始测试 EndsWith...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let endsWith = engine.TryEvaluate("EndsWith('123456789','89')", false);
  assert.strictEqual(endsWith, true, "EndsWith('123456789','89') 应该是 true");
  
  endsWith = engine.TryEvaluate("EndsWith('123456789','127')", true);
  assert.strictEqual(endsWith, false, "EndsWith('123456789','127') 应该是 false");
  
  console.log('EndsWith 测试通过！');
}

function testIsNullOrEmpty() {
  console.log('开始测试 IsNullOrEmpty...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let isNullOrEmpty = engine.TryEvaluate("IsNullOrEmpty('')", false);
  assert.strictEqual(isNullOrEmpty, true, "IsNullOrEmpty('') 应该是 true");
  
  isNullOrEmpty = engine.TryEvaluate("IsNullOrEmpty(' ')", true);
  assert.strictEqual(isNullOrEmpty, false, "IsNullOrEmpty(' ') 应该是 false");
  
  console.log('IsNullOrEmpty 测试通过！');
}

function testIsNullOrWhiteSpace() {
  console.log('开始测试 IsNullOrWhiteSpace...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let isNullOrWhiteSpace = engine.TryEvaluate("IsNullOrWhiteSpace('')", false);
  assert.strictEqual(isNullOrWhiteSpace, true, "IsNullOrWhiteSpace('') 应该是 true");
  
  isNullOrWhiteSpace = engine.TryEvaluate("IsNullOrWhiteSpace('   ')", false);
  assert.strictEqual(isNullOrWhiteSpace, true, "IsNullOrWhiteSpace('   ') 应该是 true");
  
  isNullOrWhiteSpace = engine.TryEvaluate("IsNullOrWhiteSpace(' f ')", true);
  assert.strictEqual(isNullOrWhiteSpace, false, "IsNullOrWhiteSpace(' f ') 应该是 false");
  
  console.log('IsNullOrWhiteSpace 测试通过！');
}

function testRemoveStart() {
  console.log('开始测试 RemoveStart...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let removeStart = engine.TryEvaluate("RemoveStart('123456789','12')", "");
  assert.strictEqual(removeStart, "3456789", "RemoveStart('123456789','12') 应该是 '3456789'");
  
  removeStart = engine.TryEvaluate("RemoveStart('123456789','127')", "");
  assert.strictEqual(removeStart, "123456789", "RemoveStart('123456789','127') 应该是 '123456789'");
  
  console.log('RemoveStart 测试通过！');
}

function testRemoveEnd() {
  console.log('开始测试 RemoveEnd...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let removeEnd = engine.TryEvaluate("RemoveEnd('123456789','89')", "");
  assert.strictEqual(removeEnd, "1234567", "RemoveEnd('123456789','89') 应该是 '1234567'");
  
  removeEnd = engine.TryEvaluate("RemoveEnd('123456789','127')", "");
  assert.strictEqual(removeEnd, "123456789", "RemoveEnd('123456789','127') 应该是 '123456789'");
  
  console.log('RemoveEnd 测试通过！');
}

function testJson() {
  console.log('开始测试 Json...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let json = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", "");
  assert.strictEqual(json, "51", "json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age 应该是 '51'");
  
  json = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Birthday']", "");
  assert.strictEqual(json, "04/26/1564 00:00:00", "json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Birthday'] 应该是 '04/26/1564 00:00:00'");
  
  json = engine.TryEvaluate("json('[1,2,3]')[1]", "");
  assert.strictEqual(json, "1", "json('[1,2,3]')[1] 应该是 '1'");
  
  console.log('Json 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    testUrlDecode();
    testUrlEncode();
    testHtmlEncode();
    testHtmlDecode();
    testTextToBase64();
    testTextToBase64Url();
    testBase64ToText();
    testBase64UrlToText();
    testRegex();
    testIsRegex();
    testGuid();
    testHash();
    testIndexOf();
    testSplit();
    testTrimStart();
    testTrimEnd();
    testJoin();
    testSubstring();
    testStartsWith();
    testEndsWith();
    testIsNullOrEmpty();
    testIsNullOrWhiteSpace();
    testRemoveStart();
    testRemoveEnd();
    testJson();
    console.log('所有测试通过！');
  } catch (error) {
    console.error('测试失败:', error.message);
    process.exit(1);
  }
}

// 执行测试
if (import.meta.url === import.meta.resolve('./')) {
  runAllTests();
}

export {
  testUrlDecode,
  testUrlEncode,
  testHtmlEncode,
  testHtmlDecode,
  testTextToBase64,
  testTextToBase64Url,
  testBase64ToText,
  testBase64UrlToText,
  testRegex,
  testIsRegex,
  testGuid,
  testHash,
  testIndexOf,
  testSplit,
  testTrimStart,
  testTrimEnd,
  testJoin,
  testSubstring,
  testStartsWith,
  testEndsWith,
  testIsNullOrEmpty,
  testIsNullOrWhiteSpace,
  testRemoveStart,
  testRemoveEnd,
  testJson,
  runAllTests
};