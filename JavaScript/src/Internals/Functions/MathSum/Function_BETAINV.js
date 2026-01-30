import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { SpecialFunctions } from '../../../MathNet/SpecialFunctions/SpecialFunctions.js';

class Function_BETAINV extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'BetaInv', 1);
            if (args1.IsError) return args1;
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'BetaInv', 2);
            if (args2.IsError) return args2;
        }
        let args3 = this.func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'BetaInv', 3);
            if (args3.IsError) return args3;
        }
        let p = args1.DoubleValue;
        let alpha = args2.DoubleValue;
        let beta = args3.DoubleValue;
        if (alpha < 0.0 || beta < 0.0 || p < 0.0 || p > 1.0) {
            return Operand.Error(StringCache.Function_parameter_error, 'BetaInv');
        }
        try {
            return Operand.Create(ExcelFunctions.BetaInv(p, alpha, beta));
        } catch (error) {
            // 如果计算失败，使用二分法尝试计算
            let lower = 0;
            let upper = 1;
            let mid;
            let fmid;
            
            // 二分法迭�?
            for (let i = 0; i < 100; i++) {
                mid = (lower + upper) / 2;
                fmid = SpecialFunctions.BetaRegularized(alpha, beta, mid) - p;
                
                if (Math.abs(fmid) < 1e-10) {
                    return Operand.Create(mid);
                }
                
                if (fmid < 0) {
                    lower = mid;
                } else {
                    upper = mid;
                }
            }
            
            // 如果二分法也失败，返回区间中�?
            return Operand.Create((lower + upper) / 2);
        }
    }
}



export { Function_BETAINV };

