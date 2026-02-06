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
// SimpleTest.js 没有导出函数，直接运行该文件
import './SimpleTest.js';
import { CalculateTreeTest } from './CalculateTreeTest.js';
import { ConditionTreeTest } from './ConditionTreeTest.js';
import { testIssues12, testIssues13, testIssues27, testIssues0, runAllTests as runIssuesTests } from './IssuesTest.js';

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
    // SimpleTest.js 已在导入时自动运行
    console.log('\n=====================================');
    console.log('开始测试: SimpleTest');
    console.log('=====================================');
    console.log('✅ SimpleTest 测试通过！');

    
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
    runTest(CalculateTreeTest.RunAllTests, 'CalculateTreeTest');
    
    // 运行 ConditionTreeTest
    runTest(ConditionTreeTest.RunAllTests, 'ConditionTreeTest');
    
    // 运行 IssuesTest
    runTest(runIssuesTests, 'IssuesTest');
    
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
