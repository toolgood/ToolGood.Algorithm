import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';

class Function_WEEKDAY extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "WeekDay", 1);
            if (args1.IsError) { return args1; }
        }

        let type = 1;
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "WeekDay", 2);
                if (args2.IsError) { return args2; }
            }
            type = args2.IntValue;
        }

        const t = args1.DateValue.getDay(); // JavaScript中，0表示星期日，6表示星期六
        if (type == 1) {
            // 类型1：返回1-7，1表示星期日，7表示星期六
            return engine.createOperand(t + 1);
        } else if (type == 2) {
            // 类型2：返回1-7，1表示星期一，7表示星期日
            if (t == 0) return engine.createOperand(7);
            return engine.createOperand(t);
        }
        // 其他类型：返回0-6，0表示星期一，6表示星期日
        if (t == 0) {
            return engine.createOperand(6);
        }
        return engine.createOperand(t - 1);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "WeekDay");
    }
}

export { Function_WEEKDAY };
