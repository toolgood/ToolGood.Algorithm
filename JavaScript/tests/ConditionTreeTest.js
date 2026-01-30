import assert from 'assert';
import { AlgorithmEngineHelper } from '../src/AlgorithmEngineHelper.js';
import { ConditionTreeType } from '../src/Enums/ConditionTreeType.js';

/**
 * 条件树测试
 */
export class ConditionTreeTest {
    static Test1() {
        console.log('开始测试 ConditionTreeTest.Test1');
        
        let txt = "AA.IsText() = bb";
        let t1 = AlgorithmEngineHelper.ParseCondition(txt);
        assert.strictEqual(t1.Type, ConditionTreeType.String, `Expected String, got ${t1.Type}`);
        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA.IsText() = bb", `Expected "AA.IsText() = bb", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(t1.conditionString, "AA.IsText()=bb", `Expected "AA.IsText()=bb", got "${t1.conditionString}"`);

        txt = "[bbb]=bb";
        t1 = AlgorithmEngineHelper.ParseCondition(txt);
        assert.strictEqual(t1.Type, ConditionTreeType.String, `Expected String, got ${t1.Type}`);
        assert.strictEqual(txt.substring(t1.start, t1.end + 1), txt, `Expected "${txt}", got "${txt.substring(t1.start, t1.end + 1)}"`);

        console.log('ConditionTreeTest.Test1 测试通过');
    }

    static Test2() {
        console.log('开始测试 ConditionTreeTest.Test2');
        
        let txt = "AA.IsText()=bb && dd=ss";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.And, `Expected And, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA.IsText()=bb", `Expected "AA.IsText()=bb", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t2.start, t2.end + 1), "dd=ss", `Expected "dd=ss", got "${txt.substring(t2.start, t2.end + 1)}"`);

        console.log('ConditionTreeTest.Test2 测试通过');
    }

    static Test3() {
        console.log('开始测试 ConditionTreeTest.Test3');
        
        let txt = "AA.IsText()=bb || dd=ss";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.Or, `Expected Or, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA.IsText()=bb", `Expected "AA.IsText()=bb", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t2.start, t2.end + 1), "dd=ss", `Expected "dd=ss", got "${txt.substring(t2.start, t2.end + 1)}"`);

        console.log('ConditionTreeTest.Test3 测试通过');
    }

    static Test4() {
        console.log('开始测试 ConditionTreeTest.Test4');
        
        let txt = "AA.IsText()=bb || (dd=ss && tt=22)";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.Or, `Expected Or, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        let t3 = t2.nodes[0];
        let t4 = t2.nodes[1];
        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA.IsText()=bb", `Expected "AA.IsText()=bb", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t3.start, t3.end + 1), "dd=ss", `Expected "dd=ss", got "${txt.substring(t3.start, t3.end + 1)}"`);
        assert.strictEqual(txt.substring(t4.start, t4.end + 1), "tt=22", `Expected "tt=22", got "${txt.substring(t4.start, t4.end + 1)}"`);

        console.log('ConditionTreeTest.Test4 测试通过');
    }

    static Test5() {
        console.log('开始测试 ConditionTreeTest.Test5');
        
        let txt = "AA.IsText()=bb || AND(dd=ss , tt=22)";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.Or, `Expected Or, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA.IsText()=bb", `Expected "AA.IsText()=bb", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t2.start, t2.end + 1), "AND(dd=ss , tt=22)", `Expected "AND(dd=ss , tt=22)", got "${txt.substring(t2.start, t2.end + 1)}"`);

        console.log('ConditionTreeTest.Test5 测试通过');
    }

    static Test6() {
        console.log('开始测试 ConditionTreeTest.Test6');
        
        let txt = "AA.IsText()==bb && (dd=ss || tt=22)";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.And, `Expected And, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        assert.strictEqual(t2.Type, ConditionTreeType.Or, `Expected Or, got ${t2.Type}`);
        let t3 = t2.nodes[0];
        let t4 = t2.nodes[1];

        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA.IsText()==bb", `Expected "AA.IsText()==bb", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t3.start, t3.end + 1), "dd=ss", `Expected "dd=ss", got "${txt.substring(t3.start, t3.end + 1)}"`);
        assert.strictEqual(txt.substring(t4.start, t4.end + 1), "tt=22", `Expected "tt=22", got "${txt.substring(t4.start, t4.end + 1)}"`);

        console.log('ConditionTreeTest.Test6 测试通过');
    }

    static Test7() {
        console.log('开始测试 ConditionTreeTest.Test7');
        
        let txt = "AA.IsText()==bb ? 1:2";
        let t1 = AlgorithmEngineHelper.ParseCondition(txt);
        assert.strictEqual(t1.Type, ConditionTreeType.String, `Expected String, got ${t1.Type}`);
        assert.strictEqual(txt.substring(t1.start, t1.end + 1), txt, `Expected "${txt}", got "${txt.substring(t1.start, t1.end + 1)}"`);

        console.log('ConditionTreeTest.Test7 测试通过');
    }

    static Test8() {
        console.log('开始测试 ConditionTreeTest.Test8');
        
        let txt = "AA.IsText()==bb && (dd=ss || [tt]=22)";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.And, `Expected And, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        assert.strictEqual(t2.Type, ConditionTreeType.Or, `Expected Or, got ${t2.Type}`);
        let t3 = t2.nodes[0];
        let t4 = t2.nodes[1];

        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA.IsText()==bb", `Expected "AA.IsText()==bb", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t3.start, t3.end + 1), "dd=ss", `Expected "dd=ss", got "${txt.substring(t3.start, t3.end + 1)}"`);
        assert.strictEqual(txt.substring(t4.start, t4.end + 1), "[tt]=22", `Expected "[tt]=22", got "${txt.substring(t4.start, t4.end + 1)}"`);

        console.log('ConditionTreeTest.Test8 测试通过');
    }

    static Test9() {
        console.log('开始测试 ConditionTreeTest.Test9');
        
        let txt = "AA.IsText()==bb && (dd=ss || [tt]==22)";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.And, `Expected And, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        assert.strictEqual(t2.Type, ConditionTreeType.Or, `Expected Or, got ${t2.Type}`);
        let t3 = t2.nodes[0];
        let t4 = t2.nodes[1];

        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA.IsText()==bb", `Expected "AA.IsText()==bb", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t3.start, t3.end + 1), "dd=ss", `Expected "dd=ss", got "${txt.substring(t3.start, t3.end + 1)}"`);
        assert.strictEqual(txt.substring(t4.start, t4.end + 1), "[tt]==22", `Expected "[tt]==22", got "${txt.substring(t4.start, t4.end + 1)}"`);

        console.log('ConditionTreeTest.Test9 测试通过');
    }

    static Test10() {
        console.log('开始测试 ConditionTreeTest.Test10');
        
        let txt = "AA && (dd=ss || [tt]==22)";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.And, `Expected And, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        assert.strictEqual(t2.Type, ConditionTreeType.Or, `Expected Or, got ${t2.Type}`);
        let t3 = t2.nodes[0];
        let t4 = t2.nodes[1];

        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "AA", `Expected "AA", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t3.start, t3.end + 1), "dd=ss", `Expected "dd=ss", got "${txt.substring(t3.start, t3.end + 1)}"`);
        assert.strictEqual(txt.substring(t4.start, t4.end + 1), "[tt]==22", `Expected "[tt]==22", got "${txt.substring(t4.start, t4.end + 1)}"`);

        console.log('ConditionTreeTest.Test10 测试通过');
    }

    static Test11() {
        console.log('开始测试 ConditionTreeTest.Test11');
        
        let txt = "1 && (dd=ss || [tt]==22)";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);

        assert.strictEqual(tree.Type, ConditionTreeType.And, `Expected And, got ${tree.Type}`);
        let t1 = tree.nodes[0];
        let t2 = tree.nodes[1];
        assert.strictEqual(t2.Type, ConditionTreeType.Or, `Expected Or, got ${t2.Type}`);
        let t3 = t2.nodes[0];
        let t4 = t2.nodes[1];

        assert.strictEqual(txt.substring(t1.start, t1.end + 1), "1", `Expected "1", got "${txt.substring(t1.start, t1.end + 1)}"`);
        assert.strictEqual(txt.substring(t3.start, t3.end + 1), "dd=ss", `Expected "dd=ss", got "${txt.substring(t3.start, t3.end + 1)}"`);
        assert.strictEqual(txt.substring(t4.start, t4.end + 1), "[tt]==22", `Expected "[tt]==22", got "${txt.substring(t4.start, t4.end + 1)}"`);

        console.log('ConditionTreeTest.Test11 测试通过');
    }

    static TestError() {
        console.log('开始测试 ConditionTreeTest.TestError');
        
        let txt = "";
        let tree = AlgorithmEngineHelper.ParseCondition(txt);
        assert.strictEqual(tree.Type, ConditionTreeType.Error, `Expected Error, got ${tree.Type}`);
        assert.strictEqual(tree.ErrorMessage, "condition is null", `Expected "condition is null", got "${tree.ErrorMessage}"`);

        console.log('ConditionTreeTest.TestError 测试通过');
    }

    static RunAllTests() {
        console.log('开始运行所有 ConditionTree 测试');
        this.Test1();
        this.Test2();
        this.Test3();
        this.Test4();
        this.Test5();
        this.Test6();
        this.Test7();
        this.Test8();
        this.Test9();
        this.Test10();
        this.Test11();
        this.TestError();
        console.log('所有 ConditionTree 测试运行完成');
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.ConditionTreeTest = ConditionTreeTest;
}

// 执行测试
ConditionTreeTest.RunAllTests();
