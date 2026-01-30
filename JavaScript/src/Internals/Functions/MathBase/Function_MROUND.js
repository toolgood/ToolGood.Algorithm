import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_MROUND extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "MRound", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "MRound", 2);
            if (args2.IsError) { return args2; }
        }
        let a = args2.NumberValue;
        if (a <= 0) {
            return Operand.Error(StringCache.Function_parameter_error, "MRound", 2);
        }

        let b = args1.NumberValue;
        // 四舍五入到最接近的倍数
        let r = Math.round(b / a) * a;
        return Operand.Create(r);
    }
}

export { Function_MROUND };

