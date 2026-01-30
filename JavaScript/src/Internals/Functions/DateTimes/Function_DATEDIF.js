import { Function_3 } from '../Function_3.js';
import { MyDate } from '../../MyDate.js';

class Function_DATEDIF extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "DateDif", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotDate) {
            args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "DateDif", 2);
            if (args2.IsError) { return args2; }
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotText) {
            args3 = args3.ToText("Function '{0}' parameter {1} is error!", "DateDif", 3);
            if (args3.IsError) { return args3; }
        }
        let startMyDate = args1.DateValue;
        let endMyDate = args2.DateValue;
        let t = args3.TextValue.toLowerCase();

        if (t === 'y') {
            // y: Years
            let b = false;
            if (startMyDate.getMonth() < endMyDate.getMonth()) {
                b = true;
            } else if (startMyDate.getMonth() === endMyDate.getMonth()) {
                if (startMyDate.getDate() <= endMyDate.getDate()) b = true;
            }
            if (b) {
                return Operand.Create(endMyDate.getFullYear() - startMyDate.getFullYear());
            } else {
                return Operand.Create(endMyDate.getFullYear() - startMyDate.getFullYear() - 1);
            }
        } else if (t === 'm') {
            // m: Months
            let b = false;
            if (startMyDate.getDate() <= endMyDate.getDate()) b = true;
            if (b) {
                return Operand.Create((endMyDate.getFullYear() * 12 + endMyDate.getMonth() + 1) - (startMyDate.getFullYear() * 12 + startMyDate.getMonth() + 1));
            } else {
                return Operand.Create((endMyDate.getFullYear() * 12 + endMyDate.getMonth() + 1) - (startMyDate.getFullYear() * 12 + startMyDate.getMonth() + 1) - 1);
            }
        } else if (t === 'd') {
            // d: Days
            let diffTime = Math.abs(endMyDate - startMyDate);
            let diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
            return Operand.Create(diffDays);
        } else if (t === 'yd') {
            // yd: Days of Year
            let getDayOfYear = (date) => {
                let firstDayOfYear = new Date(date.getFullYear(), 0, 1);
                return Math.ceil((date - firstDayOfYear) / (1000 * 60 * 60 * 24)) + 1;
            };
            let day = getDayOfYear(endMyDate) - getDayOfYear(startMyDate);
            if (endMyDate.getFullYear() > startMyDate.getFullYear() && day < 0) {
                let days = getDayOfYear(new Date(startMyDate.getFullYear(), 11, 31));
                day = days + day;
            }
            return Operand.Create(day);
        } else if (t === 'md') {
            // md: Days of Month
            let mo = endMyDate.getDate() - startMyDate.getDate();
            if (mo < 0) {
                let days;
                if (startMyDate.getMonth() === 11) {
                    days = new Date(startMyDate.getFullYear() + 1, 0, 0).getDate();
                } else {
                    days = new Date(startMyDate.getFullYear(), startMyDate.getMonth() + 1, 0).getDate();
                }
                mo += days;
            }
            return Operand.Create(mo);
        } else if (t === 'ym') {
            // ym: Months of Year
            let mo = (endMyDate.getMonth() + 1) - (startMyDate.getMonth() + 1);
            if (endMyDate.getDate() < startMyDate.getDate()) mo--;
            if (mo < 0) mo += 12;
            return Operand.Create(mo);
        }
        return Operand.Error("Function '{0}' parameter {1} is error!", "DateDif", 3);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "DateDif");
    }
}

export { Function_DATEDIF };
