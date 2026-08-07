import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_YEARFRAC extends Function_N {
    get Name() {
        return "YEARFRAC";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        const startDateArg = this.getDate(engine, tempParameter, 0);
        if (startDateArg.IsError) return startDateArg;
        const startDate = startDateArg.DateValue.ToDateTime();

        const endDateArg = this.getDate(engine, tempParameter, 1);
        if (endDateArg.IsError) return endDateArg;
        const endDate = endDateArg.DateValue.ToDateTime();

        let basis = 0;
        if (this.z.length > 2) {
            const basisArg = this.getNumber(engine, tempParameter, 2);
            if (basisArg.IsError) return basisArg;
            basis = Math.floor(basisArg.NumberValue);
            if (basis < 0 || basis > 4) {
                return this.parameterError(3);
            }
        }

        const result = this.calculateYearFrac(startDate, endDate, basis);
        return Operand.Create(result);
    }

    calculateYearFrac(startDate, endDate, basis) {
        if (startDate > endDate) {
            const temp = startDate;
            startDate = endDate;
            endDate = temp;
        }

        switch (basis) {
            case 0: // US (NASD) 30/360
                return this.calculate30_360(startDate, endDate);
            case 1: // Actual/actual
                return this.calculateActualActual(startDate, endDate);
            case 2: // Actual/360
                return (endDate - startDate) / (1000 * 60 * 60 * 24) / 360;
            case 3: // Actual/365
                return (endDate - startDate) / (1000 * 60 * 60 * 24) / 365;
            case 4: // European 30/360
                return this.calculate30_360E(startDate, endDate);
            default:
                return this.calculate30_360(startDate, endDate);
        }
    }

    calculate30_360(startDate, endDate) {
        // Excel basis 0 (US NASD 30/360)，微软 OpenSpecs 规则链（与 C# 一致）：
        // 1. 两日期日号均为 31 时, 都改为 30
        // 2. 否则 date1 为 31, 改为 30
        // 3. 否则 date1 为 30 且 date2 为 31, date2 改为 30
        // 4. 否则两日期均为各自年份 2 月最后一天, 都改为 30
        // 5. 否则 date1 为 2 月最后一天, 改为 30
        let d1 = startDate.getDate();
        let d2 = endDate.getDate();
        let d1FebLast = startDate.getMonth() === 1 && startDate.getDate() === this.daysInMonth(startDate.getFullYear(), 1);
        let d2FebLast = endDate.getMonth() === 1 && endDate.getDate() === this.daysInMonth(endDate.getFullYear(), 1);

        if (d1 === 31 && d2 === 31) {
            d1 = 30;
            d2 = 30;
        } else if (d1 === 31) {
            d1 = 30;
        } else if (d1 === 30 && d2 === 31) {
            d2 = 30;
        } else if (d1FebLast && d2FebLast) {
            d1 = 30;
            d2 = 30;
        } else if (d1FebLast) {
            d1 = 30;
        }

        return (360 * (endDate.getFullYear() - startDate.getFullYear()) +
            30 * (endDate.getMonth() - startDate.getMonth()) +
            (d2 - d1)) / 360;
    }

    daysInMonth(year, monthIndex) {
        return new Date(year, monthIndex + 1, 0).getDate();
    }

    calculate30_360E(startDate, endDate) {
        const d1 = Math.min(30, startDate.getDate());
        const d2 = Math.min(30, endDate.getDate());

        return (360 * (endDate.getFullYear() - startDate.getFullYear()) +
            30 * (endDate.getMonth() - startDate.getMonth()) +
            (d2 - d1)) / 360;
    }

    calculateActualActual(startDate, endDate) {
        const startYear = startDate.getFullYear();
        const endYear = endDate.getFullYear();

        if (startYear === endYear) {
            const daysInYear = this.isLeapYear(startYear) ? 366 : 365;
            return (endDate - startDate) / (1000 * 60 * 60 * 24) / daysInYear;
        }

        let result = 0;
        const daysInStartYear = this.isLeapYear(startYear) ? 366 : 365;
        const daysInEndYear = this.isLeapYear(endYear) ? 366 : 365;

        const endOfStartYear = new Date(startYear, 11, 31);
        const startOfEndYear = new Date(endYear, 0, 1);

        result += (endOfStartYear - startDate) / (1000 * 60 * 60 * 24) / daysInStartYear;
        result += (endDate - startOfEndYear) / (1000 * 60 * 60 * 24) / daysInEndYear;
        result += endYear - startYear - 1;

        return result;
    }

    isLeapYear(year) {
        return (year % 4 === 0 && year % 100 !== 0) || (year % 400 === 0);
    }
}

export { Function_YEARFRAC };
