import { Function_N } from '../Function_N.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_WORKDAY extends Function_N {
    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.z[0].Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "Workday", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.z[1].Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Workday", 2);
            if (args2.IsError) { return args2; }
        }

        let startMyDate = new Date(args1.DateValue.ToDateTime().getTime());
        let days = args2.IntValue;
        let list = new Set();
        for (let i = 2; i < this.z.length; i++) {
            let ar = this.z[i].Evaluate(engine, tempParameter);
            if (ar.IsNotDate) {
                let arDate = ar.ToMyDate(StringCache.Function_parameter_error, "Workday", i + 1);
                if (arDate.IsError) { return arDate; }
                // 将日期转换为YYYY-MM-DD格式以确保Set能够正确比较
                let dateStr = arDate.DateValue.ToDateTime().toISOString().split('T')[0];
                list.add(dateStr);
            } else {
                // 将日期转换为YYYY-MM-DD格式以确保Set能够正确比较
                let dateStr = ar.DateValue.ToDateTime().toISOString().split('T')[0];
                list.add(dateStr);
            }
        }

        while (days > 0) {
            startMyDate.setDate(startMyDate.getDate() + 1);
            let dayOfWeek = startMyDate.getDay();
            if (dayOfWeek === 6) continue; // 星期�?
            if (dayOfWeek === 0) continue; // 星期�?
            // 将当前日期转换为YYYY-MM-DD格式以确保Set能够正确比较
            let currentDateStr = startMyDate.toISOString().split('T')[0];
            if (list.has(currentDateStr)) continue;
            days--;
        }
        return Operand.Create(new MyDate(startMyDate));
    }
}

export { Function_WORKDAY };

