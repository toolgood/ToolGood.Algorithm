import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NETWORKDAYS extends Function_N {
    get Name() {
        return "NetworkDays";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetDate(engine, tempParameter, 0);
        if (args1.IsError) { return args1; }

        let args2 = this.GetDate(engine, tempParameter, 1);
        if (args2.IsError) { return args2; }

        // 获取Date对象
        let startDate = args1.DateValue.ToDateTime();
        let endDate = args2.DateValue.ToDateTime();

        let startMyDate = new Date(startDate.getTime());
        let endMyDate = new Date(endDate.getTime());

        let list = new Set();
        for (let i = 2; i < this.z.length; i++) {
            let ar = this.GetDate(engine, tempParameter, i);
            if (ar.IsError) { return ar; }
            // 将日期转换为YYYY-MM-DD格式以确保Set能够正确比较
            let dateObj = ar.DateValue.ToDateTime();
            let dateStr = dateObj.toISOString().split('T')[0];
            list.add(dateStr);
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

