import assert from 'assert';

// 导入所有测试文件
import { test, base_test, base_test2, base_test3, base_test4, base_test5, Cylinder_Test, TestVersion, Test_Json, runAllTests as runAlgorithmEngineTests } from './AlgorithmEngineTest.js';
import { testLookFloor, testLookCeiling, runAllTests as runVLookupTests } from './AlgorithmEngineTest_vlookup.js';
import { testPi, testAbs, testQUOTIENT, testMOD, testSIGN, testSQRT, testSUM, testTRUNC, testInt, testGCD, testLCM, testCombin, testPERMUT, testDegrees, testRADIANS, testCos, testCosh, testSin, testSinh, testTan, testTanh, testAcos, testAcosh, testAsin, testAsinh, testAtan, testAtanh, testAtan2, testROUND, testROUNDDOWN, testROUNDUP, testCEILING, testFLOOR, testEven, testOdd, testMROUND, testRand, testRANDBETWEEN, testFact, testFactdouble, testPOWER, testExp, testLn, testLog, testLog10, testMULTINOMIAL, testPRODUCT, testSQRTPI, testSUMSQ, testTransformation, runAllTests as runMathTests } from './AlgorithmEngineTest_math.js';
import { testASC, testJis, testCHAR, testCLEAN, testCode, testCONCATENATE, testEXACT, testFIND, testFIXED, testLEFT, testLEN, testLOWER, testMID, testPROPER, testREPLACE, testREPT, testRIGHT, testRMB, testSEARCH, testSUBSTITUTE, testT, testTEXT, testTRIM, testUPPER, testVALUE, runAllTests as runStringTests } from './AlgorithmEngineTest_string.js';
import { testIf, testIferror, testIserror, testIfnull, testIsnullorerror, testISNUMBER, testISTEXT, testISNONTEXT, testISLOGICAL, testISEVEN, testISODD, testAnd, testOr, testNot, testTrue, testFalse, testAndor, runAllTests as runFlowTests } from './AlgorithmEngineTest_flow.js';
import { testDATEVALUE, testTIMESTAMP, testTIMEVALUE, testDATE, testTime, testNow, testToday, testYear, testMonth, testDay, testHour, testMinute, testSecond, testWEEKDAY, testDATEDIF, testDAYS360, testEDATE, testEOMONTH, testNETWORKDAYS, testWORKDAY, testWEEKNUM, testAdd, runAllTests as runDateTimeTests } from './AlgorithmEngineTest_dateTime.js';
import { testUrlDecode, testUrlEncode, testHtmlEncode, testHtmlDecode, testTextToBase64, testTextToBase64Url, testBase64ToText, testBase64UrlToText, testRegex, testIsRegex, testGuid, testHash, testIndexOf, testSplit, testTrimStart, testTrimEnd, testJoin, testSubstring, testStartsWith, testEndsWith, testIsNullOrEmpty, testIsNullOrWhiteSpace, testRemoveStart, testRemoveEnd, testJson, runAllTests as runCSharpTests } from './AlgorithmEngineTest_csharp.js';
import { testMAX, testMEDIAN, testMIN, testQUARTILE, testMODE, testPERCENTILE, testPERCENTRANK, testAVERAGE, testGEOMEAN, testHARMEAN, testCOUNT, testAVEDEV, testSTDEV, testSTDEVP, testDEVSQ, testVAR, testVARP, testNORMSDIST, testNORMDIST, testNORMINV, testNORMSINV, testBETADIST, testBETAINV, testBINOMDIST, testEXPONDIST, testFDIST, testFINV, testGAMMADIST, testGAMMAINV, testGAMMALN, testHYPGEOMDIST, testLOGINV, testLOGNORMDIST, testNEGBINOMDIST, testPOISSON, testTDIST, testTINV, testWEIBULL, testFISHER, testFISHERINV, testLARGE, testSMALL, testCOVAR, testCOVARIANCES, testCOUNTIF, testSUMIF, testAVERAGEIF, runAllTests as runSumTests } from './AlgorithmEngineTest_sum.js';
import { testPARAM, testError, testJson as testJsonV35, testArray, testDistance, testArea, testVolume, testMass, testUnitError, runAllTests as runV35Tests } from './AlgorithmEngineTest_v3.5.js';
import { runAllTests as runSimpleTests } from './SimpleTest.js';
import { runAllTests as runAlgorithmEngineHelperTests } from './AlgorithmEngineHelperTest.js';
import { CalculateTreeTest } from './CalculateTreeTest.js';
import { ConditionTreeTest } from './ConditionTreeTest.js';
import { testIssues13, testIssues0, runAllTests as runIssuesTests } from './IssuesTest.js';
import { runAllTests as runFinancialTests } from './AlgorithmEngineTest_financial.js';
import { runAllTests as runStrictTests } from './AlgorithmEngineTest_strict.js';
import { runAllTests as runOperatorTests } from './AlgorithmEngineTest_operator.js';
import { runAllTests as runCacheTests } from './AlgorithmEngineTest_cache.js';
import { runAllTests as runUnitPrecisionTests } from './AlgorithmEngineTest_unitPrecision.js';
import { runAllTests as runMyDateTests } from './AlgorithmEngineTest_myDate.js';
import { runAllTests as runCalculationLogicTests } from './CalculationLogicTest.js';

// 测试执行函数
function runTest(testFunction, testName) {
  console.log(`\n=====================================`);
  console.log(`开始测试: ${testName}`);
  console.log(`=====================================`);
  try {
    testFunction();
    console.log(`✅ ${testName} 测试通过！`);
  } catch (error) {
    console.error(`❌ ${testName} 测试失败:`, error.message);
    console.error(error.stack);
    throw error; // 抛出错误以停止测试
  }
}

// 运行所有测试
function runAllTests() {
  console.log('开始运行所有测试...');
  console.log('=====================================');
  
  try {
    // 运行 SimpleTest
    runTest(runSimpleTests, 'SimpleTest');
    
    // 运行 AlgorithmEngineHelperTest
    runTest(runAlgorithmEngineHelperTests, 'AlgorithmEngineHelperTest');
    
    // 运行 AlgorithmEngineTest
    runTest(runAlgorithmEngineTests, 'AlgorithmEngineTest');
    
    // 运行 AlgorithmEngineTest_vlookup
    runTest(runVLookupTests, 'AlgorithmEngineTest_vlookup');
    
    // 运行 AlgorithmEngineTest_math
    runTest(runMathTests, 'AlgorithmEngineTest_math');
    
    // 运行 AlgorithmEngineTest_string
    runTest(runStringTests, 'AlgorithmEngineTest_string');
    
    // 运行 AlgorithmEngineTest_flow
    runTest(runFlowTests, 'AlgorithmEngineTest_flow');
    
    // 运行 AlgorithmEngineTest_dateTime
    runTest(runDateTimeTests, 'AlgorithmEngineTest_dateTime');
    
    // 运行 AlgorithmEngineTest_csharp
    runTest(runCSharpTests, 'AlgorithmEngineTest_csharp');
    
    // 运行 AlgorithmEngineTest_sum
    runTest(runSumTests, 'AlgorithmEngineTest_sum');
    
    // 运行 AlgorithmEngineTest_v3.5
    runTest(runV35Tests, 'AlgorithmEngineTest_v3.5');
    
    // 运行 CalculateTreeTest
    runTest(() => CalculateTreeTest.RunAllTests(), 'CalculateTreeTest');
    
    // 运行 ConditionTreeTest
    runTest(() => ConditionTreeTest.RunAllTests(), 'ConditionTreeTest');
    
    // 运行 IssuesTest
    runTest(runIssuesTests, 'IssuesTest');
    
    // 运行 AlgorithmEngineTest_financial
    runTest(runFinancialTests, 'AlgorithmEngineTest_financial');
    
    // 运行 AlgorithmEngineTest_strict
    runTest(runStrictTests, 'AlgorithmEngineTest_strict');
    
    // 运行 AlgorithmEngineTest_operator
    runTest(runOperatorTests, 'AlgorithmEngineTest_operator');
    
    // 运行 AlgorithmEngineTest_cache
    runTest(runCacheTests, 'AlgorithmEngineTest_cache');
    
    // 运行 AlgorithmEngineTest_unitPrecision
    runTest(runUnitPrecisionTests, 'AlgorithmEngineTest_unitPrecision');
    
    // 运行 AlgorithmEngineTest_myDate
    runTest(runMyDateTests, 'AlgorithmEngineTest_myDate');
    
    // 运行 CalculationLogicTest
    runTest(runCalculationLogicTests, 'CalculationLogicTest');
    
    console.log('\n=====================================');
    console.log('🎉 所有测试通过！');
    console.log('=====================================');
  } catch (error) {
    console.error('\n=====================================');
    console.error('❌ 测试执行失败，已停止');
    console.error('=====================================');
    process.exit(1);
  }
}

// 执行测试
if (import.meta.url === import.meta.resolve('./')) {
  runAllTests();
}

// 导出函数
 export {
  runAllTests
};
