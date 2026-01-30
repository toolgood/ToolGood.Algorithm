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
function testMAX() {
  console.log('开始测试 MAX...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let max = engine.TryEvaluate("max(1,2,3,4,2,2,1,4)", 0.0);
  assert.strictEqual(max, 4.0, "max(1,2,3,4,2,2,1,4) 应该是 4.0");
  
  console.log('MAX 测试通过！');
}

function testMEDIAN() {
  console.log('开始测试 MEDIAN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let median = engine.TryEvaluate("MEDIAN(1,2,3,4,2,2,1,4)", 0.0);
  assert.strictEqual(median, 2.0, "MEDIAN(1,2,3,4,2,2,1,4) 应该是 2.0");
  
  console.log('MEDIAN 测试通过！');
}

function testMIN() {
  console.log('开始测试 MIN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let min = engine.TryEvaluate("MIN(1,2,3,4,2,2,1,4)", 0.0);
  assert.strictEqual(min, 1.0, "MIN(1,2,3,4,2,2,1,4) 应该是 1.0");
  
  console.log('MIN 测试通过！');
}

function testQUARTILE() {
  console.log('开始测试 QUARTILE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let quartile = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),0)", 0.0);
  assert.strictEqual(quartile, 1.0, "QUARTILE(array(1,2,3,4,2,2,1,4),0) 应该是 1.0");
  
  quartile = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),1)", 0.0);
  assert.strictEqual(quartile, 1.75, "QUARTILE(array(1,2,3,4,2,2,1,4),1) 应该是 1.75");
  
  quartile = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),2)", 0.0);
  assert.strictEqual(quartile, 2.0, "QUARTILE(array(1,2,3,4,2,2,1,4),2) 应该是 2.0");
  
  quartile = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),3)", 0.0);
  assert.strictEqual(quartile, 3.25, "QUARTILE(array(1,2,3,4,2,2,1,4),3) 应该是 3.25");
  
  quartile = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),4)", 0.0);
  assert.strictEqual(quartile, 4.0, "QUARTILE(array(1,2,3,4,2,2,1,4),4) 应该是 4.0");
  
  console.log('QUARTILE 测试通过！');
}

function testMODE() {
  console.log('开始测试 MODE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let mode = engine.TryEvaluate("MODE(1,2,3,4,2,2,1,4)", 0.0);
  assert.strictEqual(mode, 2.0, "MODE(1,2,3,4,2,2,1,4) 应该是 2.0");
  
  console.log('MODE 测试通过！');
}

function testPERCENTILE() {
  console.log('开始测试 PERCENTILE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let percentile = engine.TryEvaluate("PERCENTILE(array(1,2,3,4,2,2,1,4),0.4)", 0.0);
  assert.strictEqual(percentile, 2.0, "PERCENTILE(array(1,2,3,4,2,2,1,4),0.4) 应该是 2.0");
  
  console.log('PERCENTILE 测试通过！');
}

function testPERCENTRANK() {
  console.log('开始测试 PERCENTRANK...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let percentrank = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3)", 0.0);
  assert.strictEqual(percentrank, 0.714, "PERCENTRANK(array(1,2,3,4,2,2,1,4),3) 应该是 0.714");
  
  console.log('PERCENTRANK 测试通过！');
}

function testAVERAGE() {
  console.log('开始测试 AVERAGE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let average = engine.TryEvaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0);
  assert.strictEqual(average, 2.375, "AVERAGE(1,2,3,4,2,2,1,4) 应该是 2.375");
  
  console.log('AVERAGE 测试通过！');
}

function testGEOMEAN() {
  console.log('开始测试 GEOMEAN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let geomean = engine.TryEvaluate("GEOMEAN(1,2,3,4)", 0.0);
  geomean = Math.round(geomean * 1000000) / 1000000;
  assert.strictEqual(geomean, Math.round(2.213363839 * 1000000) / 1000000, "GEOMEAN(1,2,3,4) 应该约等于 2.213363839");
  
  console.log('GEOMEAN 测试通过！');
}

function testHARMEAN() {
  console.log('开始测试 HARMEAN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let harmean = engine.TryEvaluate("HARMEAN(1,2,3,4)", 0.0);
  harmean = Math.round(harmean * 1000000) / 1000000;
  assert.strictEqual(harmean, Math.round(1.92 * 1000000) / 1000000, "HARMEAN(1,2,3,4) 应该约等于 1.92");
  
  console.log('HARMEAN 测试通过！');
}

function testCOUNT() {
  console.log('开始测试 COUNT...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let count = engine.TryEvaluate("COUNT(1,2,3,4,2,2,1,4)", 0.0);
  assert.strictEqual(count, 8.0, "COUNT(1,2,3,4,2,2,1,4) 应该是 8.0");
  
  console.log('COUNT 测试通过！');
}

function testAVEDEV() {
  console.log('开始测试 AVEDEV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let avedev = engine.TryEvaluate("AVEDEV(1,2,3,4,2,2,1,4)", 0.0);
  assert.strictEqual(avedev, 0.96875, "AVEDEV(1,2,3,4,2,2,1,4) 应该是 0.96875");
  
  console.log('AVEDEV 测试通过！');
}

function testSTDEV() {
  console.log('开始测试 STDEV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let stdev = engine.TryEvaluate("STDEV(1,2,3,4,2,2,1,4)", 0.0);
  stdev = Math.round(stdev * 1000000) / 1000000;
  assert.strictEqual(stdev, Math.round(1.187734939 * 1000000) / 1000000, "STDEV(1,2,3,4,2,2,1,4) 应该约等于 1.187734939");
  
  console.log('STDEV 测试通过！');
}

function testSTDEVP() {
  console.log('开始测试 STDEVP...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let stdevp = engine.TryEvaluate("STDEVP(1,2,3,4,2,2,1,4)", 0.0);
  stdevp = Math.round(stdevp * 1000000) / 1000000;
  assert.strictEqual(stdevp, Math.round(1.111024302 * 1000000) / 1000000, "STDEVP(1,2,3,4,2,2,1,4) 应该约等于 1.111024302");
  
  console.log('STDEVP 测试通过！');
}

function testDEVSQ() {
  console.log('开始测试 DEVSQ...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let devsq = engine.TryEvaluate("DEVSQ(1,2,3,4,2,2,1,4)", 0.0);
  devsq = Math.round(devsq * 1000000) / 1000000;
  assert.strictEqual(devsq, Math.round(9.875 * 1000000) / 1000000, "DEVSQ(1,2,3,4,2,2,1,4) 应该约等于 9.875");
  
  console.log('DEVSQ 测试通过！');
}

function testVAR() {
  console.log('开始测试 VAR...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let varValue = engine.TryEvaluate("VAR(1,2,3,4,2,2,1,4)", 0.0);
  varValue = Math.round(varValue * 1000000) / 1000000;
  assert.strictEqual(varValue, Math.round(1.410714286 * 1000000) / 1000000, "VAR(1,2,3,4,2,2,1,4) 应该约等于 1.410714286");
  
  console.log('VAR 测试通过！');
}

function testVARP() {
  console.log('开始测试 VARP...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let varp = engine.TryEvaluate("VARP(1,2,3,4,2,2,1,4)", 0.0);
  varp = Math.round(varp * 1000000) / 1000000;
  assert.strictEqual(varp, Math.round(1.234375 * 1000000) / 1000000, "VARP(1,2,3,4,2,2,1,4) 应该约等于 1.234375");
  
  console.log('VARP 测试通过！');
}

function testNORMSDIST() {
  console.log('开始测试 NORMSDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let normsdist = engine.TryEvaluate("NORMSDIST(1)", 0.0);
  normsdist = Math.round(normsdist * 1000000) / 1000000;
  assert.strictEqual(normsdist, Math.round(0.841344746 * 1000000) / 1000000, "NORMSDIST(1) 应该约等于 0.841344746");
  
  console.log('NORMSDIST 测试通过！');
}

function testNORMDIST() {
  console.log('开始测试 NORMDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let normdist = engine.TryEvaluate("NORMDIST(3,8,4,1)", 0.0);
  normdist = Math.round(normdist * 1000000) / 1000000;
  assert.strictEqual(normdist, Math.round(0.105649774 * 1000000) / 1000000, "NORMDIST(3,8,4,1) 应该约等于 0.105649774");
  
  normdist = engine.TryEvaluate("NORMDIST(3,8,4,0)", 0.0);
  normdist = Math.round(normdist * 1000000) / 1000000;
  assert.strictEqual(normdist, Math.round(0.045662271 * 1000000) / 1000000, "NORMDIST(3,8,4,0) 应该约等于 0.045662271");
  
  console.log('NORMDIST 测试通过！');
}

function testNORMINV() {
  console.log('开始测试 NORMINV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let norminv = engine.TryEvaluate("NORMINV(0.8,8,3)", 0.0);
  norminv = Math.round(norminv * 1000000) / 1000000;
  assert.strictEqual(norminv, Math.round(10.5248637 * 1000000) / 1000000, "NORMINV(0.8,8,3) 应该约等于 10.5248637");
  
  console.log('NORMINV 测试通过！');
}

function testNORMSINV() {
  console.log('开始测试 NORMSINV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let normsinv = engine.TryEvaluate("NORMSINV(0.3)", 0.0);
  normsinv = Math.round(normsinv * 1000000) / 1000000;
  assert.strictEqual(normsinv, Math.round(-0.524400513 * 1000000) / 1000000, "NORMSINV(0.3) 应该约等于 -0.524400513");
  
  console.log('NORMSINV 测试通过！');
}

function testBETADIST() {
  console.log('开始测试 BETADIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let betadist = engine.TryEvaluate("BETADIST(0.5,11,22)", 0.0);
  betadist = Math.round(betadist * 1000000) / 1000000;
  assert.strictEqual(betadist, Math.round(0.97494877 * 1000000) / 1000000, "BETADIST(0.5,11,22) 应该约等于 0.97494877");
  
  console.log('BETADIST 测试通过！');
}

function testBETAINV() {
  console.log('开始测试 BETAINV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let betainv = engine.TryEvaluate("BETAINV(0.5,23,45)", 0.0);
  betainv = Math.round(betainv * 1000000) / 1000000;
  assert.strictEqual(betainv, Math.round(0.336640759 * 1000000) / 1000000, "BETAINV(0.5,23,45) 应该约等于 0.336640759");
  
  console.log('BETAINV 测试通过！');
}

function testBINOMDIST() {
  console.log('开始测试 BINOMDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let binomdist = engine.TryEvaluate("BINOMDIST(12,45,0.5,0)", 0.0);
  binomdist = Math.round(binomdist * 1000000) / 1000000;
  assert.strictEqual(binomdist, Math.round(0.000817409 * 1000000) / 1000000, "BINOMDIST(12,45,0.5,0) 应该约等于 0.000817409");
  
  binomdist = engine.TryEvaluate("BINOMDIST(12,45,0.5,1)", 0.0);
  binomdist = Math.round(binomdist * 1000000) / 1000000;
  assert.strictEqual(binomdist, Math.round(0.00122945 * 1000000) / 1000000, "BINOMDIST(12,45,0.5,1) 应该约等于 0.00122945");
  
  console.log('BINOMDIST 测试通过！');
}

function testEXPONDIST() {
  console.log('开始测试 EXPONDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let expondist = engine.TryEvaluate("EXPONDIST(3,1,0)", 0.0);
  expondist = Math.round(expondist * 1000000) / 1000000;
  assert.strictEqual(expondist, Math.round(0.049787068 * 1000000) / 1000000, "EXPONDIST(3,1,0) 应该约等于 0.049787068");
  
  expondist = engine.TryEvaluate("EXPONDIST(3,1,1)", 0.0);
  expondist = Math.round(expondist * 1000000) / 1000000;
  assert.strictEqual(expondist, Math.round(0.950212932 * 1000000) / 1000000, "EXPONDIST(3,1,1) 应该约等于 0.950212932");
  
  console.log('EXPONDIST 测试通过！');
}

function testFDIST() {
  console.log('开始测试 FDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let fdist = engine.TryEvaluate("FDIST(0.4,2,3)", 0.0);
  fdist = Math.round(fdist * 1000000) / 1000000;
  assert.strictEqual(fdist, Math.round(0.701465776 * 1000000) / 1000000, "FDIST(0.4,2,3) 应该约等于 0.701465776");
  
  console.log('FDIST 测试通过！');
}

function testFINV() {
  console.log('开始测试 FINV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let finv = engine.TryEvaluate("FINV(0.7,2,3)", 0.0);
  finv = Math.round(finv * 1000000) / 1000000;
  assert.strictEqual(finv, Math.round(0.402651432 * 1000000) / 1000000, "FINV(0.7,2,3) 应该约等于 0.402651432");
  
  console.log('FINV 测试通过！');
}

function testGAMMADIST() {
  console.log('开始测试 GAMMADIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let gammadist = engine.TryEvaluate("GAMMADIST(0.5,3,4,0)", 0.0);
  gammadist = Math.round(gammadist * 1000000) / 1000000;
  assert.strictEqual(gammadist, Math.round(0.001723627 * 1000000) / 1000000, "GAMMADIST(0.5,3,4,0) 应该约等于 0.001723627");
  
  gammadist = engine.TryEvaluate("GAMMADIST(0.5,3,4,1)", 0.0);
  gammadist = Math.round(gammadist * 1000000) / 1000000;
  assert.strictEqual(gammadist, Math.round(0.000296478 * 1000000) / 1000000, "GAMMADIST(0.5,3,4,1) 应该约等于 0.000296478");
  
  console.log('GAMMADIST 测试通过！');
}

function testGAMMAINV() {
  console.log('开始测试 GAMMAINV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let gammainv = engine.TryEvaluate("GAMMAINV(0.2,3,4)", 0.0);
  gammainv = Math.round(gammainv * 1000000) / 1000000;
  assert.strictEqual(gammainv, Math.round(6.140176811 * 1000000) / 1000000, "GAMMAINV(0.2,3,4) 应该约等于 6.140176811");
  
  console.log('GAMMAINV 测试通过！');
}

function testGAMMALN() {
  console.log('开始测试 GAMMALN...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let gammaln = engine.TryEvaluate("GAMMALN(4)", 0.0);
  gammaln = Math.round(gammaln * 1000000) / 1000000;
  assert.strictEqual(gammaln, Math.round(1.791759469 * 1000000) / 1000000, "GAMMALN(4) 应该约等于 1.791759469");
  
  console.log('GAMMALN 测试通过！');
}

function testHYPGEOMDIST() {
  console.log('开始测试 HYPGEOMDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let hypgeomdist = engine.TryEvaluate("HYPGEOMDIST(23,45,45,100)", 0.0);
  hypgeomdist = Math.round(hypgeomdist * 1000000) / 1000000;
  assert.strictEqual(hypgeomdist, Math.round(0.08715016 * 1000000) / 1000000, "HYPGEOMDIST(23,45,45,100) 应该约等于 0.08715016");
  
  console.log('HYPGEOMDIST 测试通过！');
}

function testLOGINV() {
  console.log('开始测试 LOGINV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let loginv = engine.TryEvaluate("LOGINV(0.1,45,33)", 0.0);
  loginv = Math.round(loginv * 1000000) / 1000000;
  assert.strictEqual(loginv, Math.round(15.01122624 * 1000000) / 1000000, "LOGINV(0.1,45,33) 应该约等于 15.01122624");
  
  console.log('LOGINV 测试通过！');
}

function testLOGNORMDIST() {
  console.log('开始测试 LOGNORMDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let lognormdist = engine.TryEvaluate("LOGNORMDIST(15,23,45)", 0.0);
  lognormdist = Math.round(lognormdist * 1000000) / 1000000;
  assert.strictEqual(lognormdist, Math.round(0.326019201 * 1000000) / 1000000, "LOGNORMDIST(15,23,45) 应该约等于 0.326019201");
  
  console.log('LOGNORMDIST 测试通过！');
}

function testNEGBINOMDIST() {
  console.log('开始测试 NEGBINOMDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let negbinomdist = engine.TryEvaluate("NEGBINOMDIST(23,45,0.7)", 0.0);
  negbinomdist = Math.round(negbinomdist * 1000000) / 1000000;
  assert.strictEqual(negbinomdist, Math.round(0.053463314 * 1000000) / 1000000, "NEGBINOMDIST(23,45,0.7) 应该约等于 0.053463314");
  
  console.log('NEGBINOMDIST 测试通过！');
}

function testPOISSON() {
  console.log('开始测试 POISSON...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let poisson = engine.TryEvaluate("POISSON(23,23,0)", 0.0);
  poisson = Math.round(poisson * 1000000) / 1000000;
  assert.strictEqual(poisson, Math.round(0.082884384 * 1000000) / 1000000, "POISSON(23,23,0) 应该约等于 0.082884384");
  
  poisson = engine.TryEvaluate("POISSON(23,23,1)", 0.0);
  poisson = Math.round(poisson * 1000000) / 1000000;
  assert.strictEqual(poisson, Math.round(0.555149936 * 1000000) / 1000000, "POISSON(23,23,1) 应该约等于 0.555149936");
  
  console.log('POISSON 测试通过！');
}

function testTDIST() {
  console.log('开始测试 TDIST...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let tdist = engine.TryEvaluate("TDIST(1.2,24,1)", 0.0);
  tdist = Math.round(tdist * 1000000) / 1000000;
  assert.strictEqual(tdist, Math.round(0.120925677 * 1000000) / 1000000, "TDIST(1.2,24,1) 应该约等于 0.120925677");
  
  tdist = engine.TryEvaluate("TDIST(1.2,24,2)", 0.0);
  tdist = Math.round(tdist * 1000000) / 1000000;
  assert.strictEqual(tdist, Math.round(0.241851353 * 1000000) / 1000000, "TDIST(1.2,24,2) 应该约等于 0.241851353");
  
  console.log('TDIST 测试通过！');
}

function testTINV() {
  console.log('开始测试 TINV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let tinv = engine.TryEvaluate("TINV(0.12,23)", 0.0);
  tinv = Math.round(tinv * 1000000) / 1000000;
  assert.strictEqual(tinv, Math.round(1.614756561 * 1000000) / 1000000, "TINV(0.12,23) 应该约等于 1.614756561");
  
  console.log('TINV 测试通过！');
}

function testWEIBULL() {
  console.log('开始测试 WEIBULL...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let weibull = engine.TryEvaluate("WEIBULL(1,2,3,1)", 0.0);
  weibull = Math.round(weibull * 1000000) / 1000000;
  assert.strictEqual(weibull, Math.round(0.105160683 * 1000000) / 1000000, "WEIBULL(1,2,3,1) 应该约等于 0.105160683");
  
  weibull = engine.TryEvaluate("WEIBULL(1,2,3,0)", 0.0);
  weibull = Math.round(weibull * 1000000) / 1000000;
  assert.strictEqual(weibull, Math.round(0.198853182 * 1000000) / 1000000, "WEIBULL(1,2,3,0) 应该约等于 0.198853182");
  
  console.log('WEIBULL 测试通过！');
}

function testFISHER() {
  console.log('开始测试 FISHER...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let fisher = engine.TryEvaluate("FISHER(0.68)", 0.0);
  fisher = Math.round(fisher * 1000000) / 1000000;
  assert.strictEqual(fisher, Math.round(0.8291140383 * 1000000) / 1000000, "FISHER(0.68) 应该约等于 0.8291140383");
  
  console.log('FISHER 测试通过！');
}

function testFISHERINV() {
  console.log('开始测试 FISHERINV...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let fisherinv = engine.TryEvaluate("FISHERINV(0.6)", 0.0);
  fisherinv = Math.round(fisherinv * 1000000) / 1000000;
  assert.strictEqual(fisherinv, Math.round(0.537049567 * 1000000) / 1000000, "FISHERINV(0.6) 应该约等于 0.537049567");
  
  console.log('FISHERINV 测试通过！');
}

function testLARGE() {
  console.log('开始测试 LARGE...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let large = engine.TryEvaluate("LARGE(array(1,2,3,4,2,2,1,4),3)", 0.0);
  large = Math.round(large * 1000000) / 1000000;
  assert.strictEqual(large, Math.round(3.0 * 1000000) / 1000000, "LARGE(array(1,2,3,4,2,2,1,4),3) 应该是 3.0");
  
  console.log('LARGE 测试通过！');
}

function testSMALL() {
  console.log('开始测试 SMALL...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let small = engine.TryEvaluate("SMALL(array(1,2,3,4,2,2,1,4),3)", 0.0);
  small = Math.round(small * 1000000) / 1000000;
  assert.strictEqual(small, Math.round(2.0 * 1000000) / 1000000, "SMALL(array(1,2,3,4,2,2,1,4),3) 应该是 2.0");
  
  console.log('SMALL 测试通过！');
}

function testCOVAR() {
  console.log('开始测试 COVAR...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let covar = engine.TryEvaluate("COVAR(array(3,7,6,11),array(5,15,13,9))", 0.0);
  covar = Math.round(covar * 1000000) / 1000000;
  assert.strictEqual(covar, Math.round(3.375 * 1000000) / 1000000, "COVAR(array(3,7,6,11),array(5,15,13,9)) 应该约等于 3.375");
  
  let covarP = engine.TryEvaluate("COVARIANCE.P(array(3,7,6,11),array(5,15,13,9))", 0.0);
  covarP = Math.round(covarP * 1000000) / 1000000;
  assert.strictEqual(covarP, Math.round(3.375 * 1000000) / 1000000, "COVARIANCE.P(array(3,7,6,11),array(5,15,13,9)) 应该约等于 3.375");
  
  console.log('COVAR 测试通过！');
}

function testCOVARIANCES() {
  console.log('开始测试 COVARIANCES...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let covariances = engine.TryEvaluate("COVARIANCE.S(array(3,7,6,11),array(5,15,13,9))", 0.0);
  covariances = Math.round(covariances * 1000000) / 1000000;
  assert.strictEqual(covariances, Math.round(4.5 * 1000000) / 1000000, "COVARIANCE.S(array(3,7,6,11),array(5,15,13,9)) 应该约等于 4.5");
  
  console.log('COVARIANCES 测试通过！');
}

function testCOUNTIF() {
  console.log('开始测试 COUNTIF...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let countif = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
  assert.strictEqual(countif, 6.0, "COUNTIF(array(1,2,3,4,2,2,1,4),'>1') 应该是 6.0");
  
  countif = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>=1')", 0.0);
  assert.strictEqual(countif, 8.0, "COUNTIF(array(1,2,3,4,2,2,1,4),'>=1') 应该是 8.0");
  
  countif = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'!=1')", 0.0);
  assert.strictEqual(countif, 6.0, "COUNTIF(array(1,2,3,4,2,2,1,4),'!=1') 应该是 6.0");
  
  countif = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'=1')", 0.0);
  assert.strictEqual(countif, 2.0, "COUNTIF(array(1,2,3,4,2,2,1,4),'=1') 应该是 2.0");
  
  countif = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<=1')", 0.0);
  assert.strictEqual(countif, 2.0, "COUNTIF(array(1,2,3,4,2,2,1,4),'<=1') 应该是 2.0");
  
  countif = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<'2')", 0.0);
  assert.strictEqual(countif, 2.0, "COUNTIF(array(1,2,3,4,2,2,1,4),'<'2') 应该是 2.0");
  
  console.log('COUNTIF 测试通过！');
}

function testSUMIF() {
  console.log('开始测试 SUMIF...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let sumif = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
  assert.strictEqual(sumif, 17, "SUMIF(array(1,2,3,4,2,2,1,4),'>1') 应该是 17");
  
  sumif = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
  assert.strictEqual(sumif, 6, "SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1)) 应该是 6");
  
  sumif = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>=2',array(1,1,1,1,1,1,1,1))", 0.0);
  assert.strictEqual(sumif, 6, "SUMIF(array(1,2,3,4,2,2,1,4),'>=2',array(1,1,1,1,1,1,1,1)) 应该是 6");
  
  sumif = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'<1',array(1,1,1,1,1,1,1,1))", 0.0);
  assert.strictEqual(sumif, 0, "SUMIF(array(1,2,3,4,2,2,1,4),'<1',array(1,1,1,1,1,1,1,1)) 应该是 0");
  
  sumif = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'==1',array(1,1,1,1,1,1,1,1))", 0.0);
  assert.strictEqual(sumif, 2, "SUMIF(array(1,2,3,4,2,2,1,4),'==1',array(1,1,1,1,1,1,1,1)) 应该是 2");
  
  console.log('SUMIF 测试通过！');
}

function testAVERAGEIF() {
  console.log('开始测试 AVERAGEIF...');
  
  const engine = new AlgorithmEngineWithTryEvaluate();
  
  let averageif = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
  assert.strictEqual(averageif, 2.833333333, "AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1') 应该是 2.833333333");
  
  averageif = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
  assert.strictEqual(averageif, 1, "AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1)) 应该是 1");
  
  console.log('AVERAGEIF 测试通过！');
}

// 运行所有测试
function runAllTests() {
  try {
    // 简单统计
    testMAX();
    testMEDIAN();
    testMIN();
    testQUARTILE();
    testMODE();
    testPERCENTILE();
    testPERCENTRANK();
    testAVERAGE();
    testGEOMEAN();
    testHARMEAN();
    testCOUNT();
    testAVEDEV();
    testSTDEV();
    testSTDEVP();
    testDEVSQ();
    testVAR();
    testVARP();
    
    // 统计
    testNORMSDIST();
    testNORMDIST();
    testNORMINV();
    testNORMSINV();
    testBETADIST();
    testBETAINV();
    testBINOMDIST();
    testEXPONDIST();
    testFDIST();
    testFINV();
    testGAMMADIST();
    testGAMMAINV();
    testGAMMALN();
    testHYPGEOMDIST();
    testLOGINV();
    testLOGNORMDIST();
    testNEGBINOMDIST();
    testPOISSON();
    testTDIST();
    testTINV();
    testWEIBULL();
    testFISHER();
    testFISHERINV();
    testLARGE();
    testSMALL();
    testCOVAR();
    testCOVARIANCES();
    
    // if
    testCOUNTIF();
    testSUMIF();
    testAVERAGEIF();
    
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
runAllTests();

export {
  // 简单统计
  testMAX,
  testMEDIAN,
  testMIN,
  testQUARTILE,
  testMODE,
  testPERCENTILE,
  testPERCENTRANK,
  testAVERAGE,
  testGEOMEAN,
  testHARMEAN,
  testCOUNT,
  testAVEDEV,
  testSTDEV,
  testSTDEVP,
  testDEVSQ,
  testVAR,
  testVARP,
  
  // 统计
  testNORMSDIST,
  testNORMDIST,
  testNORMINV,
  testNORMSINV,
  testBETADIST,
  testBETAINV,
  testBINOMDIST,
  testEXPONDIST,
  testFDIST,
  testFINV,
  testGAMMADIST,
  testGAMMAINV,
  testGAMMALN,
  testHYPGEOMDIST,
  testLOGINV,
  testLOGNORMDIST,
  testNEGBINOMDIST,
  testPOISSON,
  testTDIST,
  testTINV,
  testWEIBULL,
  testFISHER,
  testFISHERINV,
  testLARGE,
  testSMALL,
  testCOVAR,
  testCOVARIANCES,
  
  // if
  testCOUNTIF,
  testSUMIF,
  testAVERAGEIF,
  
  runAllTests
};
