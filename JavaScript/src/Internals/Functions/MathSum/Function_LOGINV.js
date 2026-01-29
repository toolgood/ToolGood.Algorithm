import { Function_3 } from '../Function_3';
import { Operand } from '../../Operand';

class Function_LOGINV extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function {0} parameter {1} is error!', 'LogInv', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.toNumber('Function {0} parameter {1} is error!', 'LogInv', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const args3 = this.func3.evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            args3.toNumber('Function {0} parameter {1} is error!', 'LogInv', 3);
            if (args3.isError) {
                return args3;
            }
        }

        const n3 = args3.doubleValue;
        if (n3 < 0.0) {
            return Operand.error('Function {0} parameter is error!', 'LogInv');
        }
        return Operand.create(this.LogInv(args1.doubleValue, args2.doubleValue, n3));
    }

    LogInv(p, mean, stdDev) {
        if (p <= 0 || p >= 1) {
            return 0;
        }
        // 对数正态分布的逆函数
        // LOGINV(p, mean, stdDev) = exp(mean + stdDev * NORMINV(p))
        const normInvValue = this.NORMINV(p);
        return Math.exp(mean + stdDev * normInvValue);
    }

    // 标准正态分布的逆函数（使用近似计算）
    NORMINV(p) {
        if (p <= 0) {
            return -Infinity;
        }
        if (p >= 1) {
            return Infinity;
        }
        if (p === 0.5) {
            return 0;
        }
        // 使用Beasley-Springer-Moro算法近似计算
        const q = p - 0.5;
        let r = q * q;
        let x = q * (((((-39689.01 * r + 220946.0) * r - 275928.6) * r + 138357.7) * r - 30664.52 * r + 2.506628) / (((((-54476.09 * r + 161585.8) * r - 155698.9) * r + 668013.1) * r - 13285.21) * r + 1.0));
        // 改进近似
        const e = 0.5 * Math.pow(x, 2) - Math.log(p);
        let u = e / (1.0 + 0.5 * Math.pow(x, 2));
        x = x - u * (1.0 + 0.5 * x * u) / (1.0 + u * (1.0 + (2.0 / 3.0) * u));
        return x;
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'LogInv');
    }
}

export { Function_LOGINV };
