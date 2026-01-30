import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';

class Function_WEEKNUM extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "WeekNum", 1);
            if (args1.IsError) { return args1; }
        }
        let startMyDate = args1.DateValue;

        // 计算当年第一天的星期几（0表示星期日，6表示星期六）
        let startDate = startMyDate.ToDateTime();
        let firstDayOfYear = new Date(startDate.getFullYear(), 0, 1);
        let firstDayWeekday = firstDayOfYear.getDay();
        
        // 计算日期在当年的第几天
        let dayOfYear = Math.ceil((startDate - firstDayOfYear) / (1000 * 60 * 60 * 24)) + 1;
        
        let days = dayOfYear + firstDayWeekday;
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "WeekNum", 2);
                if (args2.IsError) { return args2; }
            }
            if (args2.IntValue == 2) {
                days--;
            }
        }

        let week = Math.ceil(days / 7.0);
        return Operand.Create(week);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "WeekNum");
    }
}

export { Function_WEEKNUM };
