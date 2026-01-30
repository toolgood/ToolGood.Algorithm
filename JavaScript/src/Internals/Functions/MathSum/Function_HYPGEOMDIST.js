import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_HYPGEOMDIST extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        let args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotNumber) {
            args4 = args4.ToNumber('Function {0} parameter {1} is error!', 'HypgeomDist', 4);
            if (args4.IsError) {
                return args4;
            }
        }
        let k = Math.round(args1.DoubleValue);
        let draws = Math.round(args2.DoubleValue);
        let success = Math.round(args3.DoubleValue);
        let population = Math.round(args4.DoubleValue);
        if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
            return Operand.error('Function {0} parameter is error!', 'HypgeomDist');
        }
        return Operand.Create(this.HypgeomDist(k, draws, success, population));
    }

    HypgeomDist(k, draws, success, population) {
        // 超几何分布的概率质量函数
        // HYPGEOMDIST(k, draws, success, population) = [C(success, k) * C(population - success, draws - k)] / C(population, draws)
        if (k < 0 || k > success || k > draws || draws - k > population - success) {
            return 0;
        }
        let numerator = this.Combination(success, k) * this.Combination(population - success, draws - k);
        let denominator = this.Combination(population, draws);
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
        this.AddFunction(stringBuilder, 'HypgeomDist');
    }
}

export { Function_HYPGEOMDIST };
