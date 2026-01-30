import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { SpecialFunctions } from '../../../MathNet/SpecialFunctions/SpecialFunctions.js';

class Function_FINV extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function {0} parameter {1} is error!', 'FInv', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber('Function {0} parameter {1} is error!', 'FInv', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber('Function {0} parameter {1} is error!', 'FInv', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        let p = args1.DoubleValue;
        let degreesFreedom = Math.round(args2.DoubleValue);
        let degreesFreedom2 = Math.round(args3.DoubleValue);
        if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0 || p < 0.0 || p > 1.0) {
            return Operand.error('Function {0} parameter is error!', 'FInv');
        }
        try {
            return Operand.Create(ExcelFunctions.FInv(p, degreesFreedom, degreesFreedom2));
        } catch (error) {
            // 如果计算失败，使用二分法尝试计算
            let lower = 0;
            let upper = 1000;
            let mid;
            let fmid;
            const targetP = 1 - p;
            
            // 二分法迭代
            for (let i = 0; i < 100; i++) {
                mid = (lower + upper) / 2;
                fmid = SpecialFunctions.BetaRegularized(degreesFreedom / 2, degreesFreedom2 / 2, degreesFreedom * mid / (degreesFreedom * mid + degreesFreedom2)) - targetP;
                
                if (Math.abs(fmid) < 1e-10) {
                    return Operand.Create(mid);
                }
                
                if (fmid < 0) {
                    lower = mid;
                } else {
                    upper = mid;
                }
            }
            
            // 如果二分法也失败，返回区间中点
            return Operand.Create((lower + upper) / 2);
        }
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'FInv');
    }
}

export { Function_FINV };
