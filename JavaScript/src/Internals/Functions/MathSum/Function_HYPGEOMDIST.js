import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_HYPGEOMDIST extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.toNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const args3 = this.func3.evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            args3.toNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 3);
            if (args3.isError) {
                return args3;
            }
        }
        const args4 = this.func4.evaluate(engine, tempParameter);
        if (args4.isNotNumber) {
            args4.toNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 4);
            if (args4.isError) {
                return args4;
            }
        }
        const k = Math.round(args1.doubleValue);
        const draws = Math.round(args2.doubleValue);
        const success = Math.round(args3.doubleValue);
        const population = Math.round(args4.doubleValue);
        if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
            return Operand.error('Function {0} parameter is error!', 'HypgeomDist');
        }
        return Operand.create(this.HypgeomDist(k, draws, success, population));
    }

    HypgeomDist(k, draws, success, population) {
        // 超几何分布的概率质量函数
        // HYPGEOMDIST(k, draws, success, population) = [C(success, k) * C(population - success, draws - k)] / C(population, draws)
        if (k < 0 || k > success || k > draws || draws - k > population - success) {
            return 0;
        }
        const numerator = this.Combination(success, k) * this.Combination(population - success, draws - k);
        const denominator = this.Combination(population, draws);
        if (denominator === 0) {
            return 0;
        }
        return numerator / denominator;
    }

    // 计算组合数 C(n, k)
    Combination(n, k) {
        if (k < 0 || k > n) {
            return 0;
        }
        if (k === 0 || k === n) {
            return 1;
        }
        k = Math.min(k, n - k); // 利用对称性
        let result = 1;
        for (let i = 1; i <= k; i++) {
            result *= (n - k + i) / i;
        }
        return Math.round(result);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'HypgeomDist');
    }
}

export { Function_HYPGEOMDIST };
