/**
 * 日期时间处理类
 */
export class MyDate {
    constructor(value) {
        if (typeof value === 'number') {
            this._value = value;
        } else if (value instanceof Date) {
            this._value = value.getTime() / 86400000;
        } else if (typeof value === 'string') {
            const date = new Date(value);
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
        const date = this.ToDateTime();
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
        const date = this.ToDateTime();
        const start = new Date(date.getFullYear(), 0, 0);
        const diff = date - start;
        const oneDay = 1000 * 60 * 60 * 24;
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
        const date = new Date();
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
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.MyDate = MyDate;
}