/**
 * 日期时间处理类
 */
export class MyDate {
    constructor(value, month, day, hour, minute, second) {
        if (month !== undefined && day !== undefined) {
            // 构造函数重载：年、月、日、时、分、秒
            let date = new Date(value, month - 1, day, hour || 0, minute || 0, second || 0);
            this._value = date.getTime() / 86400000;
        } else if (typeof value === 'number') {
            this._value = value;
        } else if (value instanceof Date) {
            this._value = value.getTime() / 86400000;
        } else if (typeof value === 'string') {
            let date = new Date(value);
            this._value = date.getTime() / 86400000;
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