import { Function_3 } from '../Function_3';
import { MyDate } from '../../MyDate';

class Function_DATEDIF extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "DateDif", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.IsNotDate) {
            args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "DateDif", 2);
            if (args2.IsError) { return args2; }
        }
        let args3 = this._arg3.evaluate(engine, tempParameter);
        if (args3.IsNotText) {
            args3 = args3.ToText("Function '{0}' parameter {1} is error!", "DateDif", 3);
            if (args3.IsError) { return args3; }
        }
        const startMyDate = args1.DateValue;
        const endMyDate = args2.DateValue;
        const t = args3.TextValue.toLowerCase();

        if (t === 'y') {
            // y: Years
            let b = false;
            if (startMyDate.getMonth() < endMyDate.getMonth()) {
                b = true;
            } else if (startMyDate.getMonth() === endMyDate.getMonth()) {
                if (startMyDate.getDate() <= endMyDate.getDate()) b = true;
            }
            if (b) {
                return engine.createOperand(endMyDate.getFullYear() - startMyDate.getFullYear());
            } else {
                return engine.createOperand(endMyDate.getFullYear() - startMyDate.getFullYear() - 1);
            }
        } else if (t === 'm') {
            // m: Months
            let b = false;
            if (startMyDate.getDate() <= endMyDate.getDate()) b = true;
            if (b) {
                return engine.createOperand((endMyDate.getFullYear() * 12 + endMyDate.getMonth() + 1) - (startMyDate.getFullYear() * 12 + startMyDate.getMonth() + 1));
            } else {
                return engine.createOperand((endMyDate.getFullYear() * 12 + endMyDate.getMonth() + 1) - (startMyDate.getFullYear() * 12 + startMyDate.getMonth() + 1) - 1);
            }
        } else if (t === 'd') {
            // d: Days
            const diffTime = Math.abs(endMyDate - startMyDate);
            const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
            return engine.createOperand(diffDays);
        } else if (t === 'yd') {
            // yd: Days of Year
            const getDayOfYear = (date) => {
                const firstDayOfYear = new Date(date.getFullYear(), 0, 1);
                return Math.ceil((date - firstDayOfYear) / (1000 * 60 * 60 * 24)) + 1;
            };
            let day = getDayOfYear(endMyDate) - getDayOfYear(startMyDate);
            if (endMyDate.getFullYear() > startMyDate.getFullYear() && day < 0) {
                const days = getDayOfYear(new Date(startMyDate.getFullYear(), 11, 31));
                day = days + day;
            }
            return engine.createOperand(day);
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
            return engine.createOperand(mo);
        } else if (t === 'ym') {
            // ym: Months of Year
            let mo = (endMyDate.getMonth() + 1) - (startMyDate.getMonth() + 1);
            if (endMyDate.getDate() < startMyDate.getDate()) mo--;
            if (mo < 0) mo += 12;
            return engine.createOperand(mo);
        }
        return engine.createErrorOperand("Function '{0}' parameter {1} is error!", "DateDif", 3);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "DateDif");
    }
}

export { Function_DATEDIF };
