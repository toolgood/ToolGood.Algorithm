import { Function_N } from '../Function_N.js';
import { MyDate } from '../../MyDate.js';

class Function_DATEVALUE extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this._args) {
            const aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }
        if (args[0].IsDate) { return args[0]; }
        let type = 0;
        if (args.length == 2) {
            const args2 = args[1].ToNumber("Function '{0}' parameter {1} is error!", "DateValue", 2);
            if (args2.IsError) { return args2; }
            type = args2.IntValue;
        }
        if (type == 0) {
            if (args[0].IsText) {
                const parsedDate = new Date(args[0].TextValue);
                if (!isNaN(parsedDate.getTime())) {
                    return engine.createOperand(new MyDate(parsedDate));
                }
            }
            const args1 = args[0].ToNumber("Function '{0}' parameter {1} is error!", "DateValue", 1);
            if (args1.LongValue <= 2958465) { // 9999-12-31 日时间在excel的数字为 2958465
                return args1.ToMyDate();
            }
            if (args1.LongValue <= 253402232399) { // 9999-12-31 12:59:59 日时间 转 时间截 为 253402232399，
                const time = new Date(Date.UTC(1970, 0, 1, 0, 0, 0) + args1.LongValue * 1000);
                if (engine.UseLocalTime) { return engine.createOperand(new MyDate(new Date(time.getTime() + time.getTimezoneOffset() * 60000))); }
                return engine.createOperand(new MyDate(time));
            }
            // 注：时间截 253402232399 ms 转时间 为 1978-01-12 05:30:32
            const time2 = new Date(Date.UTC(1970, 0, 1, 0, 0, 0) + args1.LongValue);
            if (engine.UseLocalTime) { return engine.createOperand(new MyDate(new Date(time2.getTime() + time2.getTimezoneOffset() * 60000))); }
            return engine.createOperand(new MyDate(time2));
        } else if (type == 1) {
            const args1 = args[0].ToText("Function '{0}' parameter {1} is error!", "DateValue", 1);
            if (args1.IsError) { return args1; }
            const parsedDate = new Date(args1.TextValue);
            if (!isNaN(parsedDate.getTime())) {
                return engine.createOperand(new MyDate(parsedDate));
            }
        } else if (type == 2) {
            return args[0].ToNumber("Function '{0}' parameter is error!", "DateValue").ToMyDate();
        } else if (type == 3) {
            const args1 = args[0].ToNumber("Function '{0}' parameter {1} is error!", "DateValue", 1);
            const time = new Date(Date.UTC(1970, 0, 1, 0, 0, 0) + args1.LongValue);
            if (engine.UseLocalTime) { return engine.createOperand(new MyDate(new Date(time.getTime() + time.getTimezoneOffset() * 60000))); }
            return engine.createOperand(new MyDate(time));
        } else if (type == 4) {
            const args1 = args[0].ToNumber("Function '{0}' parameter {1} is error!", "DateValue", 1);
            const time = new Date(Date.UTC(1970, 0, 1, 0, 0, 0) + args1.LongValue * 1000);
            if (engine.UseLocalTime) { return engine.createOperand(new MyDate(new Date(time.getTime() + time.getTimezoneOffset() * 60000))); }
            return engine.createOperand(new MyDate(time));
        }
        return engine.createErrorOperand("Function '{0}' parameter is error!", "DateValue");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "DateValue");
    }
}

export { Function_DATEVALUE };
