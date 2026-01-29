import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_LOGNORMDIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter {1} is error!', 'LognormDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'LognormDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3.ToNumber('Function {0} parameter {1} is error!', 'LognormDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }

        let n3 = args3.DoubleValue;
        if (n3 < 0.0) {
            return Operand.error('Function {0} parameter is error!', 'LognormDist');
        }
        return Operand.Create(this.LognormDist(args1.DoubleValue, args2.DoubleValue, n3));
    }

    LognormDist(x, mean, stdDev) {
        if (x <= 0) {
            return 0;
        }
        // 对数正态分布的累积分布函数
        // LOGNORMDIST(x, mean, stdDev) = NORMSDIST((ln(x) - mean) / stdDev)
        let z = (Math.log(x) - mean) / stdDev;
        return this.NORMSDIST(z);
    }

    // 标准正态分布的累积分布函数（使用近似计算）
    NORMSDIST(z) {
        // 使用A&S公式（26.2.17）近似计算标准正态分布的累积分布函数
        let t = 1.0 / (1.0 + 0.2316419 * Math.abs(z));
        let d = 0.3989423 * Math.exp(-z * z / 2);
        let probability = d * t * (0.3193815 + t * (-0.3565638 + t * (1.781478 + t * (-1.821256 + t * 1.330274))));
        if (z > 0) {
            return 1.0 - probability;
        }
        return probability;
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'LognormDist');
    }
}

export { Function_LOGNORMDIST };
