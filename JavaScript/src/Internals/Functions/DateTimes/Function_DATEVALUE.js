import { Function_N } from '../Function_N.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_DATEVALUE extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }
        if (args[0].IsDate) { return args[0]; }
        let Type = 0;
        if (args.length == 2) {
            let args2 = args[1].ToNumber(StringCache.Function_parameter_error, "DateValue", 2);
            if (args2.IsError) { return args2; }
            Type = args2.IntValue;
        }
        if (Type == 0) {
            if (args[0].IsText) {
                let parsedDate = new Date(args[0].TextValue);
                if (!isNaN(parsedDate.getTime())) {
                    return Operand.Create(new MyDate(parsedDate));
                }
            }
            let args1 = args[0].ToNumber(StringCache.Function_parameter_error, "DateValue", 1);
            if (args1.LongValue <= 2958465) { // 9999-12-31 日时间在excel的数字为 2958465
                return args1.ToMyDate();
            }
            if (args1.LongValue <= 253402232399) { // 9999-12-31 12:59:59 日时间 转 时间截 为 253402232399，
                let time = new Date(Date.UTC(1970, 0, 1, 0, 0, 0) + args1.LongValue * 1000);
                if (engine.UseLocalTime) { return Operand.Create(new MyDate(new Date(time.getTime() + time.getTimezoneOffset() * 60000))); }
                return Operand.Create(new MyDate(time));
            }
            // 注：时间截 253402232399 ms 转时间 为 1978-01-12 05:30:32
            let time2 = new Date(Date.UTC(1970, 0, 1, 0, 0, 0) + args1.LongValue);
            if (engine.UseLocalTime) { return Operand.Create(new MyDate(new Date(time2.getTime() + time2.getTimezoneOffset() * 60000))); }
            return Operand.Create(new MyDate(time2));
        } else if (Type == 1) {
            let args1 = args[0].ToText(StringCache.Function_parameter_error, "DateValue", 1);
            if (args1.IsError) { return args1; }
            let parsedDate = new Date(args1.TextValue);
            if (!isNaN(parsedDate.getTime())) {
                return Operand.Create(new MyDate(parsedDate));
            }
        } else if (Type == 2) {
            return args[0].ToNumber(StringCache.Function_parameter_error, "DateValue").ToMyDate();
        } else if (Type == 3) {
            let args1 = args[0].ToNumber(StringCache.Function_parameter_error, "DateValue", 1);
            let time = new Date(Date.UTC(1970, 0, 1, 0, 0, 0) + args1.LongValue);
            if (engine.UseLocalTime) { return Operand.Create(new MyDate(new Date(time.getTime() + time.getTimezoneOffset() * 60000))); }
            return Operand.Create(new MyDate(time));
        } else if (Type == 4) {
            let args1 = args[0].ToNumber(StringCache.Function_parameter_error, "DateValue", 1);
            let time = new Date(Date.UTC(1970, 0, 1, 0, 0, 0) + args1.LongValue * 1000);
            if (engine.UseLocalTime) { return Operand.Create(new MyDate(new Date(time.getTime() + time.getTimezoneOffset() * 60000))); }
            return Operand.Create(new MyDate(time));
        }
        return Operand.Error(StringCache.Function_parameter_error, "DateValue");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "DateValue");
    }
}

export { Function_DATEVALUE };
