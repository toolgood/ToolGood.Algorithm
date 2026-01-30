import { AlgorithmEngineHelper } from '../src/AlgorithmEngineHelper.js';

function assert(condition, message) {
    if (!condition) {
        throw new Error(message || 'Assertion failed');
    }
}

function assertEquals(expected, actual, message) {
    if (expected !== actual) {
        throw new Error(message || `Expected ${expected}, got ${actual}`);
    }
}

function assertTrue(condition, message) {
    if (!condition) {
        throw new Error(message || 'Expected true, got false');
    }
}

function assertFalse(condition, message) {
    if (condition) {
        throw new Error(message || 'Expected false, got true');
    }
}

console.log('Running AlgorithmEngineHelper tests...');

// Test GetDiyNames function
try {
    console.log('Test 1: GetDiyNames basic functionality');
    const p = AlgorithmEngineHelper.GetDiyNames('dd');
    assertEquals('dd', p.Parameters[0].Name);
    console.log('✓ Test 1 passed');
} catch (error) {
    console.error('✗ Test 1 failed:', error.message);
}

try {
    console.log('Test 2: GetDiyNames repeated call');
    const p3 = AlgorithmEngineHelper.GetDiyNames('dd');
    assertEquals('dd', p3.Parameters[0].Name);
    console.log('✓ Test 2 passed');
} catch (error) {
    console.error('✗ Test 2 failed:', error.message);
}

try {
    console.log('Test 3: GetDiyNames with function and parameters');
    const p5 = AlgorithmEngineHelper.GetDiyNames('ddd(d1,22)');
    assertEquals('ddd', p5.Functions[0].Name);
    assertEquals('d1', p5.Parameters[0].Name);
    console.log('✓ Test 3 passed');
} catch (error) {
    console.error('✗ Test 3 failed:', error.message);
}

try {
    console.log('Test 4: GetDiyNames with Chinese character');
    const p6 = AlgorithmEngineHelper.GetDiyNames('长');
    assertEquals('长', p6.Parameters[0].Name);
    console.log('✓ Test 4 passed');
} catch (error) {
    console.error('✗ Test 4 failed:', error.message);
}

try {
    console.log('Test 5: GetDiyNames with expression');
    const p7 = AlgorithmEngineHelper.GetDiyNames('ddd+2');
    assertEquals('ddd', p7.Parameters[0].Name);
    console.log('✓ Test 5 passed');
} catch (error) {
    console.error('✗ Test 5 failed:', error.message);
}

try {
    console.log('Test 6: GetDiyNames with JSON and parameter');
    const p8 = AlgorithmEngineHelper.GetDiyNames(`{"A": 0.6,"B": 0.4,"C": 0.6,"E": 0.33,"F": 0.29,"Z": 0.15,
"EB": 0.7,"EE": 0.65,"EA": 0.85,"AB": 1.0,"BC": 1.0,"AA":1.0,
"EBC": 1.15,"BAB": 1.25,"BCB": 1.25,"BBC": 1.25,"CBB": 1.25,"EBA": 1.2,"AAA": 1.4}[瓦楞]`);
    assertEquals('瓦楞', p8.Parameters[0].Name);
    console.log('✓ Test 6 passed');
} catch (error) {
    console.error('✗ Test 6 failed:', error.message);
}

// Test IsKeywords function
try {
    console.log('Test 7: IsKeywords with false');
    const b = AlgorithmEngineHelper.IsKeywords('false');
    assertTrue(b);
    console.log('✓ Test 7 passed');
} catch (error) {
    console.error('✗ Test 7 failed:', error.message);
}

// Test UnitConversion function
try {
    console.log('Test 8: UnitConversion distance units');
    let b = AlgorithmEngineHelper.UnitConversion(1, '米', '千米', '测试');
    assertEquals(0.001, b);
    
    b = AlgorithmEngineHelper.UnitConversion(1, '米', '分米', '测试');
    assertEquals(10, b);
    
    b = AlgorithmEngineHelper.UnitConversion(1, '米', '厘米', '测试');
    assertEquals(100, b);
    
    b = AlgorithmEngineHelper.UnitConversion(1, '米', 'mm', '测试');
    assertEquals(1000, b);
    console.log('✓ Test 8 passed');
} catch (error) {
    console.error('✗ Test 8 failed:', error.message);
}

try {
    console.log('Test 9: UnitConversion area units');
    let b = AlgorithmEngineHelper.UnitConversion(1, 'm2', 'dm2', '测试');
    assertEquals(100, b);
    
    b = AlgorithmEngineHelper.UnitConversion(1, 'm2', 'cm2', '测试');
    assertEquals(10000, b);
    
    b = AlgorithmEngineHelper.UnitConversion(1, 'm2', 'mm2', '测试');
    assertEquals(1000000, b);
    console.log('✓ Test 9 passed');
} catch (error) {
    console.error('✗ Test 9 failed:', error.message);
}

try {
    console.log('Test 10: UnitConversion volume units');
    let b = AlgorithmEngineHelper.UnitConversion(1, 'm3', 'dm3', '测试');
    assertEquals(1000, b);
    
    b = AlgorithmEngineHelper.UnitConversion(1, 'm3', 'cm3', '测试');
    assertEquals(1000000, b);
    
    b = AlgorithmEngineHelper.UnitConversion(1, 'm3', 'mm3', '测试');
    assertEquals(1000000000, b);
    console.log('✓ Test 10 passed');
} catch (error) {
    console.error('✗ Test 10 failed:', error.message);
}

try {
    console.log('Test 11: UnitConversion mass units');
    let b = AlgorithmEngineHelper.UnitConversion(1, 't', 'kg', '测试');
    assertEquals(1000, b);
    
    b = AlgorithmEngineHelper.UnitConversion(1, 't', 'g', '测试');
    assertEquals(1000000, b);
    console.log('✓ Test 11 passed');
} catch (error) {
    console.error('✗ Test 11 failed:', error.message);
}

// Test CheckFormula function
try {
    console.log('Test 12: CheckFormula valid formula');
    let b = AlgorithmEngineHelper.CheckFormula('1+1');
    assertTrue(b);
    console.log('✓ Test 12 passed');
} catch (error) {
    console.error('✗ Test 12 failed:', error.message);
}

try {
    console.log('Test 13: CheckFormula invalid formula');
    let b = AlgorithmEngineHelper.CheckFormula('1+');
    assertFalse(b);
    console.log('✓ Test 13 passed');
} catch (error) {
    console.error('✗ Test 13 failed:', error.message);
}

try {
    console.log('Test 14: CheckFormula with invalid character');
    let b = AlgorithmEngineHelper.CheckFormula('@123+1');
    assertFalse(b);
    console.log('✓ Test 14 passed');
} catch (error) {
    console.error('✗ Test 14 failed:', error.message);
}

console.log('All tests completed!');
