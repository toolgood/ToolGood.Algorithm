import assert from 'assert';
import { AlgorithmEngineHelper } from '../src/AlgorithmEngineHelper.js';
import { CalculateTreeType } from '../src/Enums/CalculateTreeType.js';

/**
 * 计算树测试
 */
export class CalculateTreeTest {
    static Test1() {
        console.log('开始测试 CalculateTreeTest.Test1');
        
        let txt = "A1+1";
        let t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.Add, `Expected Add, got ${t1.Type}`);
        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "A1+1", `Expected "A1+1", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(t1.conditionString, "A1+1", `Expected "A1+1", got "${t1.conditionString}"`);

        txt = "A1-(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.Sub, `Expected Sub, got ${t1.Type}`);
        assert.strictEqual(t1.nodes[1].conditionString, "1+1", `Expected "1+1", got "${t1.nodes[1].conditionString}"`);

        txt = "A1*(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.Mul, `Expected Mul, got ${t1.Type}`);

        txt = "A1/(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.Div, `Expected Div, got ${t1.Type}`);

        txt = "A1%(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.Mod, `Expected Mod, got ${t1.Type}`);

        txt = "A1&(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.Connect, `Expected Connect, got ${t1.Type}`);

        txt = "A1(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.String, `Expected String, got ${t1.Type}`);

        txt = "A1(1+1)-";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.Error, `Expected Error, got ${t1.Type}`);

        txt = "-1+(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.Add, `Expected Add, got ${t1.Type}`);

        txt = "-1";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(t1.Type, CalculateTreeType.String, `Expected String, got ${t1.Type}`);

        console.log('CalculateTreeTest.Test1 测试通过');
    }

    static TestError() {
        console.log('开始测试 CalculateTreeTest.TestError');
        
        let txt = "";
        let tree = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(tree.Type, CalculateTreeType.Error, `Expected Error, got ${tree.Type}`);
        assert.strictEqual(tree.ErrorMessage, "exp is null", `Expected "exp is null", got "${tree.ErrorMessage}"`);

        console.log('CalculateTreeTest.TestError 测试通过');
    }

    static TestComplexExpressions() {
        console.log('开始测试 CalculateTreeTest.TestComplexExpressions');
        
        let txt = "A1+B1*C1";
        let tree = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(tree.Type, CalculateTreeType.Add, `Expected Add, got ${tree.Type}`);

        txt = "A1*(B1+C1)/D1";
        tree = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(tree.Type, CalculateTreeType.Div, `Expected Div, got ${tree.Type}`);

        txt = "A1+B1-C1*D1/E1";
        tree = AlgorithmEngineHelper.ParseCalculate(txt);
        assert.strictEqual(tree.Type, CalculateTreeType.Sub, `Expected Sub, got ${tree.Type}`);

        console.log('CalculateTreeTest.TestComplexExpressions 测试通过');
    }

    static RunAllTests() {
        console.log('开始运行所有 CalculateTree 测试');
        this.Test1();
        this.TestError();
        this.TestComplexExpressions();
        console.log('所有 CalculateTree 测试运行完成');
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.CalculateTreeTest = CalculateTreeTest;
}

// 执行测试
CalculateTreeTest.RunAllTests();
