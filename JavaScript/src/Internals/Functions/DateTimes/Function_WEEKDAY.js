import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_WEEKDAY extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate(StringCache.Function_parameter_error2, "WeekDay", 1);
            if (args1.IsError) { return args1; }
        }

        let Type = 1;
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber(StringCache.Function_parameter_error2, "WeekDay", 2);
                if (args2.IsError) { return args2; }
            }
            Type = args2.IntValue;
        }

        let t = args1.DateValue.ToDateTime().getDay(); // JavaScript中，0表示星期日，6表示星期六
        if (Type == 1) {
            // 类型1：返回1-7，1表示星期日，7表示星期六
            return Operand.Create(t + 1);
        } else if (Type == 2) {
            // 类型2：返回1-7，1表示星期一，7表示星期日
            if (t == 0) return Operand.Create(7);
            return Operand.Create(t);
        }
        // 其他类型：返回0-6，0表示星期一，6表示星期日
        if (t == 0) {
            return Operand.Create(6);
        }
        return Operand.Create(t - 1);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "WeekDay");
    }
}

export { Function_WEEKDAY };
