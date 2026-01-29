import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_NORMDIST extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter {1} is error!', 'NormDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'NormDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3.ToNumber('Function {0} parameter {1} is error!', 'NormDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        const args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotBoolean) {
            args4.ToBoolean('Function {0} parameter {1} is error!', 'NormDist', 4);
            if (args4.IsError) {
                return args4;
            }
        }

        const num = args1.DoubleValue;
        const avg = args2.DoubleValue;
        const STDEV = args3.DoubleValue;
        const b = args4.BooleanValue;
        return Operand.Create(this.NormDist(num, avg, STDEV, b));
    }

    NormDist(x, mean, standardDev, cumulative) {
        if (standardDev <= 0) {
            return 0;
        }
        if (cumulative) {
            // 正态分布的累积分布函数
            return this.NORMSDIST((x - mean) / standardDev);
        } else {
            // 正态分布的概率密度函数
            return this.NORMDENSITY(x, mean, standardDev);
        }
    }

    // 正态分布的概率密度函数
    NORMDENSITY(x, mean, standardDev) {
        const exponent = -0.5 * Math.pow((x - mean) / standardDev, 2);
        return Math.exp(exponent) / (standardDev * Math.sqrt(2 * Math.PI));
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
        this.addFunction(stringBuilder, 'NormDist');
    }
}

export { Function_NORMDIST };
