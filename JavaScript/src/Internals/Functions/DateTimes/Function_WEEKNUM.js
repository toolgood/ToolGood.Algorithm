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
        let startMyDate = args1.DateValue.ToDateTime();

        let returnType = 1;
        if (this.b != null) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            returnType = args2.IntValue;
            if (returnType != 1 && returnType != 2 && returnType != 11 && returnType != 12 && returnType != 13
                && returnType != 14 && returnType != 15 && returnType != 16 && returnType != 17) {
                return this.parameterError(2);
            }
        }

        if (returnType == 21) {
            let jan4 = new Date(startMyDate.getFullYear(), 0, 4);
            let daysSinceJan4 = (startMyDate - jan4) / (1000 * 60 * 60 * 24);
            let dayOfWeekJan4 = jan4.getDay();
            let mondayOffset = dayOfWeekJan4 == 0 ? 6 : dayOfWeekJan4 - 1;
            let weekStart = new Date(jan4.getTime() - mondayOffset * 24 * 60 * 60 * 1000);
            let weekNumber = Math.ceil((daysSinceJan4 + mondayOffset) / 7) + 1;
            return Operand.Create(weekNumber);
        }

        let jan1 = new Date(startMyDate.getFullYear(), 0, 1);
        let dayOfYear = startMyDate.getDayOfYear ? startMyDate.getDayOfYear() : Math.ceil((startMyDate - jan1) / (1000 * 60 * 60 * 24)) + 1;
        let dayOfWeekJan1 = jan1.getDay();

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