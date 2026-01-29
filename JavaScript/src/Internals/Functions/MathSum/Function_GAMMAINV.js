import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_GAMMAINV extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter {1} is error!', 'GammaInv', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'GammaInv', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3.ToNumber('Function {0} parameter {1} is error!', 'GammaInv', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        const probability = args1.DoubleValue;
        const alpha = args2.DoubleValue;
        const beta = args3.DoubleValue;
        if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
            return Operand.error('Function {0} parameter is error!', 'GammaInv');
        }
        return Operand.Create(this.GammaInv(probability, alpha, beta));
    }

    GammaInv(probability, alpha, beta) {
        if (probability <= 0) {
            return 0;
        }
        if (probability >= 1) {
            return Infinity;
        }
        // 使用二分查找法计算Gamma分布的逆函数
        let low = 0;
        let high = 10000;
        let mid;
        let gammaValue;
        for (let i = 0; i < 100; i++) {
            mid = (low + high) / 2;
            gammaValue = this.GammaCDF(mid / beta, alpha);
            if (Math.abs(gammaValue - probability) < 1e-10) {
                return mid;
            }
            if (gammaValue < probability) {
                low = mid;
            } else {
                high = mid;
            }
        }
        return (low + high) / 2;
    }

    // 伽马累积分布函数
    GammaCDF(x, alpha) {
        if (x <= 0) {
            return 0;
        }
        // 使用不完全伽马函数计算累积分布函数
        return this.LowerIncompleteGamma(alpha, x) / this.Gamma(alpha);
    }

    // 下不完全伽马函数
    LowerIncompleteGamma(alpha, x) {
        if (x === 0) {
            return 0;
        }
        if (x < alpha + 1) {
            // 级数展开
            let sum = Math.pow(x, alpha) * Math.exp(-x) / alpha;
            let term = sum;
            for (let n = 1; n < 100; n++) {
                term *= x / (alpha + n);
                sum += term;
                if (Math.abs(term) < 1e-10) {
                    break;
                }
            }
            return sum;
        } else {
            // 连分数展开
            let b = x + 1 - alpha;
            let c = 1 / 1e-20;
            let d = 1 / b;
            let h = d;
            for (let n = 1; n < 100; n++) {
                const an = -n * (n - alpha);
                b += 2;
                d = an * d + b;
                if (Math.abs(d) < 1e-20) {
                    d = 1e-20;
                }
                c = b + an / c;
                if (Math.abs(c) < 1e-20) {
                    c = 1e-20;
                }
                d = 1 / d;
                const delta = d * c;
                h *= delta;
                if (Math.abs(delta - 1) < 1e-10) {
                    break;
                }
            }
            return this.Gamma(alpha) * Math.exp(-x) * Math.pow(x, alpha) * h;
        }
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
        this.addFunction(stringBuilder, 'GammaInv');
    }
}

export { Function_GAMMAINV };
