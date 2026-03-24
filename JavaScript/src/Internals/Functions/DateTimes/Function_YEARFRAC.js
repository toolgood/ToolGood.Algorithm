import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_YEARFRAC extends Function_3 {
    get Name() {
        return "YEARFRAC";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let startDateArg = this.getDate_1(engine, tempParameter);
        if (startDateArg.IsError) { return startDateArg; }
        let startDate = startDateArg.DateValue.toDate();

        let endDateArg = this.getDate_2(engine, tempParameter);
        if (endDateArg.IsError) { return endDateArg; }
        let endDate = endDateArg.DateValue.toDate();

        let basis = 0;
        if (this.c != null) {
            let basisArg = this.getNumber_3(engine, tempParameter);
            if (basisArg.IsError) { return basisArg; }
            basis = Math.floor(basisArg.NumberValue);
            if (basis < 0 || basis > 4) {
                return this.parameterError(3);
            }
        }

        let result = this.calculateYearFrac(startDate, endDate, basis);
        return Operand.Create(result);
    }

    calculateYearFrac(startDate, endDate, basis) {
        if (startDate > endDate) {
            let temp = startDate;
            startDate = endDate;
            endDate = temp;
        }

        switch (basis) {
            case 0:
                return this.calculate30_360(startDate, endDate);
            case 1:
                return this.calculateActualActual(startDate, endDate);
            case 2:
                return (endDate - startDate) / (1000 * 60 * 60 * 24) / 360;
            case 3:
                return (endDate - startDate) / (1000 * 60 * 60 * 24) / 365;
            case 4:
                return this.calculate30_360E(startDate, endDate);
            default:
                return this.calculate30_360(startDate, endDate);
        }
    }

    calculate30_360(startDate, endDate) {
        let d1 = Math.min(30, startDate.getDate());
        let d2 = endDate.getDate();

        if (d1 === 30) d2 = Math.min(30, d2);

        return (360 * (endDate.getFullYear() - startDate.getFullYear()) + 30 * (endDate.getMonth() - startDate.getMonth()) + (d2 - d1)) / 360.0;
    }

    calculate30_360E(startDate, endDate) {
        let d1 = Math.min(30, startDate.getDate());
        let d2 = Math.min(30, endDate.getDate());

        return (360 * (endDate.getFullYear() - startDate.getFullYear()) + 30 * (endDate.getMonth() - startDate.getMonth()) + (d2 - d1)) / 360.0;
    }

    isLeapYear(year) {
        return (year % 4 === 0 && year % 100 !== 0) || (year % 400 === 0);
    }

    calculateActualActual(startDate, endDate) {
        let startYear = startDate.getFullYear();
        let endYear = endDate.getFullYear();

        if (startYear === endYear) {
            let daysInYear = this.isLeapYear(startYear) ? 366 : 365;
            return (endDate - startDate) / (1000 * 60 * 60 * 24) / daysInYear;
        }

        let result = 0;
        let daysInStartYear = this.isLeapYear(startYear) ? 366 : 365;
        let daysInEndYear = this.isLeapYear(endYear) ? 366 : 365;

        let endOfStartYear = new Date(startYear, 11, 31);
        let startOfEndYear = new Date(endYear, 0, 1);

        result += (endOfStartYear - startDate) / (1000 * 60 * 60 * 24) / daysInStartYear;
        result += (endDate - startOfEndYear) / (1000 * 60 * 60 * 24) / daysInEndYear;
        result += endYear - startYear - 1;

        return result;
    }

    getResultType() {
        return "NUMBER";
    }
}

export { Function_YEARFRAC };