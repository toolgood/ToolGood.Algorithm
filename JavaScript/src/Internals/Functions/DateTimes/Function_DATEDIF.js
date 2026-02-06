import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_DATEDIF extends Function_3 {
    get Name() {
        return "DateDif";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.GetDate_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let args3 = this.GetText_3(engine, tempParameter);
        if (args3.IsError) { return args3; }
        let startMyDate = args1.DateValue;  // MyDate对象
        let endMyDate = args2.DateValue;    // MyDate对象
        let startDate = startMyDate.ToDateTime();  // Date对象
        let endDate = endMyDate.ToDateTime();      // Date对象
        let t = args3.TextValue.toLowerCase();

        if (t === 'y') {
            // y: Years
            let b = false;
            if (startDate.getMonth() < endDate.getMonth()) {
                b = true;
            } else if (startDate.getMonth() === endDate.getMonth()) {
                if (startDate.getDate() <= endDate.getDate()) b = true;
            }
            if (b) {
                return Operand.Create(endDate.getFullYear() - startDate.getFullYear());
            } else {
                return Operand.Create(endDate.getFullYear() - startDate.getFullYear() - 1);
            }
        } else if (t === 'm') {
            // m: Months
            let b = false;
            if (startDate.getDate() <= endDate.getDate()) b = true;
            if (b) {
                return Operand.Create((endDate.getFullYear() * 12 + endDate.getMonth() + 1) - (startDate.getFullYear() * 12 + startDate.getMonth() + 1));
            } else {
                return Operand.Create((endDate.getFullYear() * 12 + endDate.getMonth() + 1) - (startDate.getFullYear() * 12 + startDate.getMonth() + 1) - 1);
            }
        } else if (t === 'd') {
            // d: Days
            let diffTime = Math.abs(endDate.getTime() - startDate.getTime());
            let diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
            return Operand.Create(diffDays);
        } else if (t === 'yd') {
            // yd: Days of Year
            let getDayOfYear = (date) => {
                let firstDayOfYear = new Date(date.getFullYear(), 0, 1);
                return Math.ceil((date.getTime() - firstDayOfYear.getTime()) / (1000 * 60 * 60 * 24)) + 1;
            };
            let day = getDayOfYear(endDate) - getDayOfYear(startDate);
            if (endDate.getFullYear() > startDate.getFullYear() && day < 0) {
                let days = getDayOfYear(new Date(startDate.getFullYear(), 11, 31));
                day = days + day;
            }
            return Operand.Create(day);
        } else if (t === 'md') {
            // md: Days of Month
            let mo = endDate.getDate() - startDate.getDate();
            if (mo < 0) {
                let days;
                if (startDate.getMonth() === 11) {
                    days = new Date(startDate.getFullYear() + 1, 0, 0).getDate();
                } else {
                    days = new Date(startDate.getFullYear(), startDate.getMonth() + 1, 0).getDate();
                }
                mo += days;
            }
            return Operand.Create(mo);
        } else if (t === 'ym') {
            // ym: Months of Year
            let mo = (endDate.getMonth() + 1) - (startDate.getMonth() + 1);
            if (endDate.getDate() < startDate.getDate()) mo--;
            if (mo < 0) mo += 12;
            return Operand.Create(mo);
        }
        return this.ParameterError(3);
    }
}

export { Function_DATEDIF };

