import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NETWORKDAYS extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.funcs[0].Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "NetWorkdays", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.funcs[1].Evaluate(engine, tempParameter);
        if (args2.IsNotDate) {
            args2 = args2.ToMyDate(StringCache.Function_parameter_error, "NetWorkdays", 2);
            if (args2.IsError) { return args2; }
        }

        // 获取Date对象
        let startDate = args1.DateValue.ToDateTime();
        let endDate = args2.DateValue.ToDateTime();

        let startMyDate = new Date(startDate.getTime());
        let endMyDate = new Date(endDate.getTime());

        let list = new Set();
        for (let i = 2; i < this.funcs.length; i++) {
            let ar = this.funcs[i].Evaluate(engine, tempParameter);
            if (ar.IsNotDate) {
                let arDate = ar.ToMyDate(StringCache.Function_parameter_error, "NetWorkdays", i + 1);
                if (arDate.IsError) { return arDate; }
                // 将日期转换为YYYY-MM-DD格式以确保Set能够正确比较
                let dateObj = arDate.DateValue.ToDateTime();
                let dateStr = dateObj.toISOString().split('T')[0];
                list.add(dateStr);
            } else {
                // 将日期转换为YYYY-MM-DD格式以确保Set能够正确比较
                let dateObj = ar.DateValue.ToDateTime();
                let dateStr = dateObj.toISOString().split('T')[0];
                list.add(dateStr);
            }
        }

        let days = 0;
        while (startMyDate <= endMyDate) {
            let dayOfWeek = startMyDate.getDay();
            if (dayOfWeek !== 0 && dayOfWeek !== 6) { // 0是周日，6是周�?
                // 将当前日期转换为YYYY-MM-DD格式以确保Set能够正确比较
                let currentDateStr = startMyDate.toISOString().split('T')[0];
                if (!list.has(currentDateStr)) {
                    days++;
                }
            }
            startMyDate.setDate(startMyDate.getDate() + 1);
        }
        return Operand.Create(days);
    }
}

export { Function_NETWORKDAYS };

