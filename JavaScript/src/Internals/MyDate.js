/**
 * 日期时间处理类
 */
export class MyDate {
    constructor(value, month, day, hour, minute, second) {
        if (month !== undefined && day !== undefined) {
            if (value === 0 && month === 0 && day === 0) {
                // 只有时间部分，按照 C# 逻辑：只计算时间部分的数值
                let totalSeconds = (hour || 0) * 3600 + (minute || 0) * 60 + (second || 0);
                this._value = totalSeconds / 86400;
            } else {
                // 构造函数重载：年、月、日、时、分、秒
                let date = new Date(value, month - 1, day, hour || 0, minute || 0, second || 0);
                // 按照 C# 逻辑：从 1900 年 1 月 1 日开始计算天数
                let start = new Date(1900, 0, 1);
                let diffTime = date - start;
                let diffDays = Math.floor(diffTime / 86400000) + 2;
                // 正确计算当天的时间部分
                let dayStart = new Date(value, month - 1, day, 0, 0, 0);
                let timePart = (date.getTime() - dayStart.getTime()) / 86400000;
                this._value = diffDays + timePart;
            }
        } else if (typeof value === 'number') {
            this._value = value;
        } else if (value instanceof Date) {
            // 按照 C# 逻辑：从 1900 年 1 月 1 日开始计算天数
            let start = new Date(1900, 0, 1);
            let diffTime = value - start;
            let diffDays = Math.floor(diffTime / 86400000) + 2;
            // 正确计算当天的时间部分
            let dayStart = new Date(value.getFullYear(), value.getMonth(), value.getDate(), 0, 0, 0);
            let timePart = (value.getTime() - dayStart.getTime()) / 86400000;
            this._value = diffDays + timePart;
        } else if (typeof value === 'string') {
            let date = new Date(value);
            // 按照 C# 逻辑：从 1900 年 1 月 1 日开始计算天数
            let start = new Date(1900, 0, 1);
            let diffTime = date - start;
            let diffDays = Math.floor(diffTime / 86400000) + 2;
            // 正确计算当天的时间部分
            let dayStart = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
            let timePart = (date.getTime() - dayStart.getTime()) / 86400000;
            this._value = diffDays + timePart;
        } else {
            this._value = 0;
        }
    }

    get Value() {
        return this._value;
    }

    set Value(value) {
        this._value = value;
    }

    /**
     * 转换为日期对象
     */
    ToDateTime() {
        if (this._value > 365) {
            // 按照 C# 逻辑：从 1900 年 1 月 1 日开始，减去 2 天偏移
            let start = new Date(1900, 0, 1);
            let days = Math.floor(this._value) - 2;
            let date = new Date(start);
            date.setDate(start.getDate() + days);
            // 添加小数部分作为时间
            let fractionalPart = this._value - Math.floor(this._value);
            let milliseconds = fractionalPart * 86400000;
            date.setTime(date.getTime() + milliseconds);
            return date;
        }
        return new Date(this._value * 86400000);
    }

    /**
     * 转换为字符串
     */
    toString() {
        let date = this.ToDateTime();
        return date.toISOString();
    }

    /**
     * 获取年
     */
    get Year() {
        return this.ToDateTime().getFullYear();
    }

    /**
     * 获取月
     */
    get Month() {
        return this.ToDateTime().getMonth() + 1;
    }

    /**
     * 获取日
     */
    get Day() {
        return this.ToDateTime().getDate();
    }

    /**
     * 获取时
     */
    get Hour() {
        return this.ToDateTime().getHours();
    }

    /**
     * 获取分
     */
    get Minute() {
        return this.ToDateTime().getMinutes();
    }

    /**
     * 获取秒
     */
    get Second() {
        return this.ToDateTime().getSeconds();
    }

    /**
     * 获取毫秒
     */
    get Millisecond() {
        return this.ToDateTime().getMilliseconds();
    }

    /**
     * 获取星期
     */
    get DayOfWeek() {
        return this.ToDateTime().getDay();
    }

    /**
     * 获取一年中的第几天
     */
    get DayOfYear() {
        let date = this.ToDateTime();
        let start = new Date(date.getFullYear(), 0, 0);
        let diff = date - start;
        let oneDay = 1000 * 60 * 60 * 24;
        return Math.floor(diff / oneDay);
    }

    /**
     * 加法运算
     */
    static Add(value, days) {
        return new MyDate(value.Value + days);
    }

    /**
     * 减法运算
     */
    static Subtract(value1, value2) {
        return value1.Value - value2.Value;
    }

    /**
     * 比较运算
     */
    static Compare(value1, value2) {
        if (value1.Value < value2.Value) return -1;
        if (value1.Value > value2.Value) return 1;
        return 0;
    }

    /**
     * 当前时间
     */
    static get Now() {
        return new MyDate(new Date());
    }

    /**
     * 今天
     */
    static get Today() {
        let date = new Date();
        date.setHours(0, 0, 0, 0);
        return new MyDate(date);
    }

    /**
     * 转换为数值
     */
    valueOf() {
        return this._value;
    }

    /**
     * 转换为日期
     */
    static FromDateTime(date) {
        return new MyDate(date);
    }

    /**
     * 转换为时间间隔
     */
    static FromTimeSpan(milliseconds) {
        return new MyDate(milliseconds / 86400000);
    }

    /**
     * 添加年
     */
    addYears(years) {
        let date = this.ToDateTime();
        date.setFullYear(date.getFullYear() + years);
        return new MyDate(date);
    }

    /**
     * 添加月
     */
    AddMonths(months) {
        let date = this.ToDateTime();
        date.setMonth(date.getMonth() + months);
        return new MyDate(date);
    }

    /**
     * 添加天
     */
    AddDays(days) {
        let date = this.ToDateTime();
        date.setDate(date.getDate() + days);
        return new MyDate(date);
    }

    /**
     * 获取年
     */
    year() {
        return this.Year;
    }

    /**
     * 获取月
     */
    Month() {
        return this.Month;
    }

    /**
     * 获取日
     */
    Day() {
        return this.Day;
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.MyDate = MyDate;
}