import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';

class Function_TIMESTAMP extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args0 = this.func1.Evaluate(engine, tempParameter);
        if (args0.IsError) { return args0; }

        let Type = 0; // 毫秒
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TimeStamp", 2);
                if (args2.IsError) { return args2; }
            }
            Type = args2.IntValue;
        }

        let args1 = args0.ToMyDate("Function '{0}' parameter {1} is error!", "TimeStamp", 1);
        if (args1.IsError) { return args1; }

        // 转换为UTC时间戳
        let utcDate = new Date(args1.DateValue.getUTCFullYear(), args1.DateValue.getUTCMonth(), args1.DateValue.getUTCDate(), 
                                args1.DateValue.getUTCHours(), args1.DateValue.getUTCMinutes(), args1.DateValue.getUTCSeconds(), 
                                args1.DateValue.getUTCMilliseconds());

        if (Type == 0) {
            // 毫秒时间戳
            let ms = utcDate.getTime();
            return Operand.Create(ms);
        } else if (Type == 1) {
            // 秒时间戳
            let s = utcDate.getTime() / 1000;
            return Operand.Create(s);
        }
        return engine.createErrorOperand("Function '{0}' parameter is error!", "TimeStamp");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "TimeStamp");
    }
}

export { Function_TIMESTAMP };
