import { Function_2 } from '../Function_2';
import { MyDate } from '../../MyDate';

class Function_TIMESTAMP extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args0 = this._arg1.evaluate(engine, tempParameter);
        if (args0.IsError) { return args0; }

        let type = 0; // 毫秒
        if (this._arg2 !== null) {
            let args2 = this._arg2.evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TimeStamp", 2);
                if (args2.IsError) { return args2; }
            }
            type = args2.IntValue;
        }

        let args1 = args0.ToMyDate("Function '{0}' parameter {1} is error!", "TimeStamp", 1);
        if (args1.IsError) { return args1; }

        // 转换为UTC时间戳
        const utcDate = new Date(args1.DateValue.getUTCFullYear(), args1.DateValue.getUTCMonth(), args1.DateValue.getUTCDate(), 
                                args1.DateValue.getUTCHours(), args1.DateValue.getUTCMinutes(), args1.DateValue.getUTCSeconds(), 
                                args1.DateValue.getUTCMilliseconds());

        if (type == 0) {
            // 毫秒时间戳
            const ms = utcDate.getTime();
            return engine.createOperand(ms);
        } else if (type == 1) {
            // 秒时间戳
            const s = utcDate.getTime() / 1000;
            return engine.createOperand(s);
        }
        return engine.createErrorOperand("Function '{0}' parameter is error!", "TimeStamp");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "TimeStamp");
    }
}

export { Function_TIMESTAMP };
