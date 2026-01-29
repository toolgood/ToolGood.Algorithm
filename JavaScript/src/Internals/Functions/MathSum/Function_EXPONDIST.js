import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_EXPONDIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter {1} is error!', 'ExponDist', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'ExponDist', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotBoolean) {
            args3.ToBoolean('Function {0} parameter {1} is error!', 'ExponDist', 3);
            if (args3.IsError) {
                return args3;
            }
        }

        const n1 = args1.DoubleValue;
        if (n1 < 0.0) {
            return Operand.error('Function {0} parameter is error!', 'ExponDist');
        }
        return Operand.Create(this.ExponDist(n1, args2.DoubleValue, args3.BooleanValue));
    }

    ExponDist(x, lambda, cumulative) {
        if (x < 0 || lambda <= 0) {
            return 0;
        }
        if (cumulative) {
            // 累积分布函数
            return 1 - Math.exp(-lambda * x);
        } else {
            // 概率密度函数
            return lambda * Math.exp(-lambda * x);
        }
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'ExponDist');
    }
}

export { Function_EXPONDIST };
