import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_WEEKDAY extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "WeekDay", 1);
            if (args1.IsError) { return args1; }
        }

        let Type = 1;
        if (this.b !== null) {
            let args2 = this.b.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber(StringCache.Function_parameter_error, "WeekDay", 2);
                if (args2.IsError) { return args2; }
            }
            Type = args2.IntValue;
        }

        let t = args1.DateValue.ToDateTime().getDay(); // JavaScript中，0表示星期日，6表示星期�?
        if (Type == 1) {
            // 类型1：返�?-7�?表示星期日，7表示星期�?
            return Operand.Create(t + 1);
        } else if (Type == 2) {
            // 类型2：返�?-7�?表示星期一�?表示星期�?
            if (t == 0) return Operand.Create(7);
            return Operand.Create(t);
        }
        // 其他类型：返�?-6�?表示星期一�?表示星期�?
        if (t == 0) {
            return Operand.Create(6);
        }
        return Operand.Create(t - 1);
    }
}

export { Function_WEEKDAY };

