import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_EDATE extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "EDate", 1);
            if (args1.IsError) { return args1; }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "EDate", 2);
            if (args2.IsError) { return args2; }
        // 获取开始日期的Date对象
        let startDate = args1.DateValue.ToDateTime();
        // 创建新的日期对象
        let date = new Date(startDate.getFullYear(), startDate.getMonth(), 1); // 先设置为月份的第一�?
        // 添加月份
        date.setMonth(date.getMonth() + args2.IntValue);
        // 计算该月的天�?
        let daysInMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
        // 调整日期：如果原日期是月份的最后一天，则新日期也设置为月份的最后一�?
        let day = Math.min(startDate.getDate(), daysInMonth);
        date.setDate(day);
        return Operand.Create(new MyDate(date));
    }
}

export { Function_EDATE };

