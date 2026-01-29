import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_GAMMALN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter is error!', 'GammaLn');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(this.GAMMALN(args1.DoubleValue));
    }

    GAMMALN(x) {
        if (x <= 0) {
            return Infinity;
        }
        // 使用斯特林公式近似计算伽马函数的自然对数
        if (x < 0.5) {
            return Math.log(Math.PI) - Math.log(Math.sin(Math.PI * x)) - this.GAMMALN(1 - x);
        }
        x -= 1;
        const t = x + 5.5;
        const s = 1 + 1/(t) * (0.99999999999980993 + 
            676.5203681218851/(t+1) - 
            1259.1392167224028/(t+2) + 
            771.32342877765313/(t+3) - 
            176.61502916214059/(t+4) + 
            12.507343278686905/(t+5) - 
            0.13857109526572012/(t+6) + 
            9.9843695780195716e-6/(t+7) + 
            1.5056327351493116e-7/(t+8));
        return Math.log(2 * Math.PI) / 2 + (x + 0.5) * Math.log(t) - t + Math.log(s);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'GammaLn');
    }
}

export { Function_GAMMALN };
