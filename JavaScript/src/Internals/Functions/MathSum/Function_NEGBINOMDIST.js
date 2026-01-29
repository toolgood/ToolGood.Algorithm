import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_NEGBINOMDIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter {1} is error!', 'NegbinomDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'NegbinomDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3.ToNumber('Function {0} parameter {1} is error!', 'NegbinomDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        const k = Math.round(args1.DoubleValue);
        const r = args2.DoubleValue;
        const p = args3.DoubleValue;

        if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
            return Operand.error('Function {0} parameter is error!', 'NegbinomDist');
        }
        return Operand.Create(this.NegbinomDist(k, r, p));
    }

    NegbinomDist(k, r, p) {
        if (k < 0) {
            return 0;
        }
        if (p === 0) {
            return k === 0 ? 1 : 0;
        }
        if (p === 1) {
            return 0;
        }
        // 负二项分布的概率质量函数
        // NEGBINOMDIST(k, r, p) = C(k + r - 1, r - 1) * p^r * (1 - p)^k
        const combination = this.Combination(k + r - 1, r - 1);
        return combination * Math.pow(p, r) * Math.pow(1 - p, k);
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
        return result;
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'NegbinomDist');
    }
}

export { Function_NEGBINOMDIST };
