import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';


class Function_WEEKNUM extends Function_2 {
    get Name() {
        return "Weeknum";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let returnType = 1;
        if (this.b != null) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            returnType = args2.IntValue;
            if (returnType != 1 && returnType != 2 && returnType != 11 && returnType != 12 && returnType != 13
                && returnType != 14 && returnType != 15 && returnType != 16 && returnType != 17 && returnType != 21) {
                return this.parameterError(2);
            }
        }

        let startDate = args1.DateValue.ToDateTime();
        let year = startDate.getFullYear();

        if (returnType == 21) {
            // ISO 8601: 第1周是包含当年第一个周四的周
            let isoDow = startDate.getDay(); // 0=周日...6=周六
            isoDow = isoDow == 0 ? 7 : isoDow; // 转为 1=周一...7=周日
            let thursday = new Date(startDate.getTime());
            thursday.setDate(thursday.getDate() + (4 - isoDow)); // 本周的周四
            let thursdayDayOfYear = Math.floor((thursday - new Date(year, 0, 0)) / (1000 * 60 * 60 * 24));
            return Operand.Create(Math.floor((thursdayDayOfYear - 1) / 7) + 1);
        }

        // 当年第几天（1基）
        let dayOfYear = Math.floor((startDate - new Date(year, 0, 0)) / (1000 * 60 * 60 * 24));
        let dayOfWeekJan1 = new Date(year, 0, 1).getDay(); // 0=周日...6=周六

        let weekStartDay;
        if (returnType == 1 || returnType == 17) {
            weekStartDay = 0;
        } else if (returnType == 2 || returnType == 11) {
            weekStartDay = 1;
        } else if (returnType == 12) {
            weekStartDay = 2;
        } else if (returnType == 13) {
            weekStartDay = 3;
        } else if (returnType == 14) {
            weekStartDay = 4;
        } else if (returnType == 15) {
            weekStartDay = 5;
        } else {
            weekStartDay = 6;
        }

        let daysUntilWeekStart = (dayOfWeekJan1 - weekStartDay + 7) % 7;
        let adjustedDayOfYear = dayOfYear + daysUntilWeekStart;
        let week = Math.ceil(adjustedDayOfYear / 7.0);

        return Operand.Create(week);
    }
}

export { Function_WEEKNUM };

