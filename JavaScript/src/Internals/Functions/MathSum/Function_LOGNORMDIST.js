import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_LOGNORMDIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function {0} parameter {1} is error!', 'LognormDist', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.toNumber('Function {0} parameter {1} is error!', 'LognormDist', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const args3 = this.func3.evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            args3.toNumber('Function {0} parameter {1} is error!', 'LognormDist', 3);
            if (args3.isError) {
                return args3;
            }
        }

        const n3 = args3.doubleValue;
        if (n3 < 0.0) {
            return Operand.error('Function {0} parameter is error!', 'LognormDist');
        }
        return Operand.create(this.LognormDist(args1.doubleValue, args2.doubleValue, n3));
    }

    LognormDist(x, mean, stdDev) {
        if (x <= 0) {
            return 0;
        }
        // 对数正态分布的累积分布函数
        // LOGNORMDIST(x, mean, stdDev) = NORMSDIST((ln(x) - mean) / stdDev)
        const z = (Math.log(x) - mean) / stdDev;
        return this.NORMSDIST(z);
    }

    // 标准正态分布的累积分布函数（使用近似计算）
    NORMSDIST(z) {
        // 使用A&S公式（26.2.17）近似计算标准正态分布的累积分布函数
        const t = 1.0 / (1.0 + 0.2316419 * Math.abs(z));
        const d = 0.3989423 * Math.exp(-z * z / 2);
        const probability = d * t * (0.3193815 + t * (-0.3565638 + t * (1.781478 + t * (-1.821256 + t * 1.330274))));
        if (z > 0) {
            return 1.0 - probability;
        }
        return probability;
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'LognormDist');
    }
}

export { Function_LOGNORMDIST };
