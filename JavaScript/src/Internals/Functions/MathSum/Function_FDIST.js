import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_FDIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            args1.ToNumber('Function {0} parameter {1} is error!', 'FDist', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'FDist', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            args3.ToNumber('Function {0} parameter {1} is error!', 'FDist', 3);
            if (args3.isError) {
                return args3;
            }
        }

        const x = args1.doubleValue;
        const degreesFreedom = Math.round(args2.doubleValue);
        const degreesFreedom2 = Math.round(args3.doubleValue);
        if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
            return Operand.error('Function {0} parameter is error!', 'FDist');
        }
        return Operand.Create(this.FDist(x, degreesFreedom, degreesFreedom2));
    }

    FDist(x, degreesFreedom1, degreesFreedom2) {
        if (x < 0) {
            return 0;
        }
        if (x === 0) {
            return 0;
        }
        // F分布的累积分布函数可以用贝塔分布来计算
        // F = (v2/v1) * (X/Y)，其中X和Y是独立的卡方变量
        // 因此，P(F <= x) = P(Beta <= (v1*x)/(v1*x + v2))，其中Beta ~ Beta(v1/2, v2/2)
        const betaArg = (degreesFreedom1 * x) / (degreesFreedom1 * x + degreesFreedom2);
        return this.BetaCDF(betaArg, degreesFreedom1 / 2, degreesFreedom2 / 2);
    }

    // 贝塔分布的累积分布函数
    BetaCDF(x, alpha, beta) {
        // 这里使用一个简化的实现，实际应用中可能需要更精确的计算
        // 对于小值的alpha和beta，可以使用近似计算
        if (x <= 0) {
            return 0;
        }
        if (x >= 1) {
            return 1;
        }
        // 这里使用伽马函数的近似计算
        const gammaAlpha = this.Gamma(alpha);
        const gammaBeta = this.Gamma(beta);
        const gammaAlphaBeta = this.Gamma(alpha + beta);
        const betaFunction = gammaAlphaBeta / (gammaAlpha * gammaBeta);
        // 简化的积分近似
        let sum = 0;
        let term = Math.pow(x, alpha) * Math.pow(1 - x, beta - 1);
        sum += term;
        for (let k = 1; k < 100; k++) {
            term *= (alpha + k - 1) * x / (k * (beta + k - 1) * (1 - x));
            if (Math.abs(term) < 1e-10) {
                break;
            }
            sum += term;
        }
        return sum / betaFunction;
    }

    // 伽马函数的近似计算
    Gamma(z) {
        // 使用斯特林公式近似
        if (z < 0.5) {
            return Math.PI / (Math.sin(Math.PI * z) * this.Gamma(1 - z));
        }
        z -= 1;
        let t = z + 5.5;
        let s = 1 + 1/(t) * (0.99999999999980993 + 
            676.5203681218851/(t+1) - 
            1259.1392167224028/(t+2) + 
            771.32342877765313/(t+3) - 
            176.61502916214059/(t+4) + 
            12.507343278686905/(t+5) - 
            0.13857109526572012/(t+6) + 
            9.9843695780195716e-6/(t+7) + 
            1.5056327351493116e-7/(t+8));
        return Math.sqrt(2 * Math.PI) * Math.pow(t, z + 0.5) * Math.exp(-t) * s;
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'FDist');
    }
}

export { Function_FDIST };
