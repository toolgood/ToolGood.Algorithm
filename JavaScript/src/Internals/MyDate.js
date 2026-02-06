/**
 * 日期时间处理类
 */
export class MyDate {
    constructor(year, month, day, hour, minute, second) {
        if (year instanceof Date) {
            // 构造函数重载：接受Date对象
            this.Year = year.getFullYear();
            this.Month = year.getMonth() + 1;
            this.Day = year.getDate();
            this.Hour = year.getHours();
            this.Minute = year.getMinutes();
            this.Second = year.getSeconds();
        } else if (typeof year === 'number' && month === undefined) {
            // 构造函数重载：接受数字（像Excel日期值）
            let num = year;
            let days = Math.floor(num);
            if (days > 365) {
                // 从1900年1月1日开始计算
                let start = new Date(1900, 0, 1);
                let targetDate = new Date(start);
                targetDate.setDate(targetDate.getDate() + days - 2);
                this.Year = targetDate.getFullYear();
                this.Month = targetDate.getMonth() + 1;
                this.Day = targetDate.getDate();
            } else {
                this.Year = null;
                this.Month = null;
                this.Day = days;
            }
            let d = num - days;
            this.Hour = Math.floor(d * 24);
            this.Minute = Math.floor((d * 24 - this.Hour) * 60);
            this.Second = Math.round(((d * 24 - this.Hour) * 60 - this.Minute) * 60);
            if (this.Second === 60) {
                this.Second = 0;
                this.Minute++;
                if (this.Minute === 60) {
                    this.Minute = 0;
                    this.Hour++;
                }
            }
        } else if (typeof year === 'object' && year !== null && 'days' in year) {
            // 构造函数重载：接受时间跨度对象
            let timeSpan = year;
            this.Year = null;
            this.Month = null;
            this.Day = timeSpan.days || 0;
            this.Hour = timeSpan.hours || 0;
            this.Minute = timeSpan.minutes || 0;
            this.Second = timeSpan.seconds || 0;
        } else {
            // 构造函数重载：接受年、月、日、时、分、秒
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Hour = hour || 0;
            this.Minute = minute || 0;
            this.Second = second || 0;
        }
    }

    /**
     * 转换为字符串
     */
    toString(format) {
        if (format) {
            // 这里可以实现格式化功能，暂时返回默认格式
            return this.toString();
        }
        if (this.Year != null && this.Year > 0) {
            return `${this.Year}-${this.Month.toString().padStart(2, '0')}-${this.Day.toString().padStart(2, '0')} ${this.Hour.toString().padStart(2, '0')}:${this.Minute.toString().padStart(2, '0')}:${this.Second.toString().padStart(2, '0')}`;
        } else {
            let result = '';
            if (this.Day != null && this.Day > 0) {
                result += `${this.Day} `;
            }
            result += `${this.Hour.toString().padStart(2, '0')}:${this.Minute.toString().padStart(2, '0')}:${this.Second.toString().padStart(2, '0')}`;
            return result;
        }
    }

    /**
     * 转换为Date对象
     */
    ToDateTime(dateTimeKind) {
        if (this.Year != null && this.Year > 0) {
            if (dateTimeKind === 0) {
                // UTC时间
                return new Date(Date.UTC(this.Year, this.Month - 1, this.Day, this.Hour, this.Minute, this.Second));
            } else {
                // 本地时间
                return new Date(this.Year, this.Month - 1, this.Day, this.Hour, this.Minute, this.Second);
            }
        }
        // 只有时间部分，返回从1970年1月1日开始的时间
        return new Date(1970, 0, 1 + (this.Day || 0), this.Hour, this.Minute, this.Second);
    }

    /**
     * 转换为时间跨度对象
     */
    ToTimeSpan() {
        return {
            days: this.Day || 0,
            hours: this.Hour,
            minutes: this.Minute,
            seconds: this.Second
        };
    }

    /**
     * 添加年
     */
    AddYears(year) {
        let newYear = (this.Year || 0) + year;
        return new MyDate(newYear, this.Month, this.Day, this.Hour, this.Minute, this.Second);
    }

    /**
     * 添加月
     */
    AddMonths(month) {
        let newMonth = (this.Month || 0) + month;
        if (newMonth >= 1 && newMonth <= 12) {
            return new MyDate(this.Year, newMonth, this.Day, this.Hour, this.Minute, this.Second);
        }
        // 如果月份超出范围，使用Date对象计算
        let date = this.ToDateTime();
        date.setMonth(date.getMonth() + month);
        return new MyDate(date);
    }

    /**
     * 添加日
     */
    AddDays(day) {
        if (this.Year != null && this.Year > 1900) {
            let newDay = (this.Day || 0) + day;
            if (newDay >= 1 && newDay <= 28) {
                return new MyDate(this.Year, this.Month, newDay, this.Hour, this.Minute, this.Second);
            }
            // 如果日期超出范围，使用Date对象计算
            let date = this.ToDateTime();
            date.setDate(date.getDate() + day);
            return new MyDate(date);
        }
        // 只有时间部分，直接添加天数
        return new MyDate(null, null, (this.Day || 0) + day, this.Hour, this.Minute, this.Second);
    }

    /**
     * 添加时
     */
    AddHours(hour) {
        let newHour = this.Hour + hour;
        if (newHour >= 0 && newHour < 24) {
            return new MyDate(this.Year, this.Month, this.Day, newHour, this.Minute, this.Second);
        }
        if (this.Year != null && this.Year > 1900) {
            // 使用Date对象计算
            let date = this.ToDateTime();
            date.setHours(date.getHours() + hour);
            return new MyDate(date);
        }
        // 只有时间部分，计算总小时数
        let totalHours = this.Hour + hour;
        let days = Math.floor(totalHours / 24);
        let remainingHours = totalHours % 24;
        if (remainingHours < 0) {
            remainingHours += 24;
            days--;
        }
        return new MyDate(null, null, (this.Day || 0) + days, remainingHours, this.Minute, this.Second);
    }

    /**
     * 添加分
     */
    AddMinutes(minute) {
        let newMinute = this.Minute + minute;
        if (newMinute >= 0 && newMinute <= 59) {
            return new MyDate(this.Year, this.Month, this.Day, this.Hour, newMinute, this.Second);
        }
        if (this.Year != null && this.Year > 1900) {
            // 使用Date对象计算
            let date = this.ToDateTime();
            date.setMinutes(date.getMinutes() + minute);
            return new MyDate(date);
        }
        // 只有时间部分，计算总分钟数
        let totalMinutes = this.Minute + minute;
        let hours = Math.floor(totalMinutes / 60);
        let remainingMinutes = totalMinutes % 60;
        if (remainingMinutes < 0) {
            remainingMinutes += 60;
            hours--;
        }
        return this.AddHours(hours).AddMinutes(remainingMinutes);
    }

    /**
     * 添加秒
     */
    AddSeconds(second) {
        let newSecond = this.Second + second;
        if (newSecond >= 0 && newSecond <= 59) {
            return new MyDate(this.Year, this.Month, this.Day, this.Hour, this.Minute, newSecond);
        }
        if (this.Year != null && this.Year > 1900) {
            // 使用Date对象计算
            let date = this.ToDateTime();
            date.setSeconds(date.getSeconds() + second);
            return new MyDate(date);
        }
        // 只有时间部分，计算总秒数
        let totalSeconds = this.Second + second;
        let minutes = Math.floor(totalSeconds / 60);
        let remainingSeconds = totalSeconds % 60;
        if (remainingSeconds < 0) {
            remainingSeconds += 60;
            minutes--;
        }
        return this.AddMinutes(minutes).AddSeconds(remainingSeconds);
    }

    /**
     * 转换为数字（像Excel日期值）
     */
    valueOf() {
        if (this.Year != null && this.Year > 1900) {
            let dt = new Date(this.Year, this.Month - 1, this.Day, this.Hour, this.Minute, this.Second);
            let start = new Date(1900, 0, 1);
            let diffTime = dt.getTime() - start.getTime();
            let days = Math.floor(diffTime / (1000 * 60 * 60 * 24) + 2);
            days += (this.Hour + (this.Minute + this.Second / 60) / 60) / 24;
            return days;
        }
        return (this.Day || 0) + (this.Hour + (this.Minute + this.Second / 60) / 60) / 24;
    }

    /**
     * 转换为长整型数字，格式：年月日时分秒
     */
    ToLong() {
        let d = 0;
        if (this.Year != null) {
            d += this.Year * 10000000000;
        }
        if (this.Month != null) {
            d += this.Month * 100000000;
        }
        if (this.Day != null) {
            d += this.Day * 1000000;
        }
        d += this.Hour * 10000;
        d += this.Minute * 100;
        d += this.Second;
        return d;
    }

    /**
     * 静态方法：解析字符串为MyDate对象
     */
    static Parse(txt) {
        let t = txt.trim();
        
        // 匹配年月日 时分秒格式
        let dateTimeRegex = /^(\d{4})[-/](\d{1,2})[-/](\d{1,2})\s+(\d{1,2}):(\d{1,2}):(\d{1,2})$/;
        let match = t.match(dateTimeRegex);
        if (match) {
            return new MyDate(
                parseInt(match[1]),
                parseInt(match[2]),
                parseInt(match[3]),
                parseInt(match[4]),
                parseInt(match[5]),
                parseInt(match[6])
            );
        }
        
        // 匹配年月日 时分格式
        let dateTimeRegex2 = /^(\d{4})[-/](\d{1,2})[-/](\d{1,2})\s+(\d{1,2}):(\d{1,2})$/;
        match = t.match(dateTimeRegex2);
        if (match) {
            return new MyDate(
                parseInt(match[1]),
                parseInt(match[2]),
                parseInt(match[3]),
                parseInt(match[4]),
                parseInt(match[5]),
                0
            );
        }
        
        // 匹配年月日格式
        let dateRegex = /^(\d{4})[-/](\d{1,2})[-/](\d{1,2})$/;
        match = t.match(dateRegex);
        if (match) {
            return new MyDate(
                parseInt(match[1]),
                parseInt(match[2]),
                parseInt(match[3]),
                0,
                0,
                0
            );
        }
        
        // 匹配时分秒格式
        let timeRegex = /^(\d{1,2}):(\d{1,2}):(\d{1,2})$/;
        match = t.match(timeRegex);
        if (match) {
            return new MyDate(
                null,
                null,
                null,
                parseInt(match[1]),
                parseInt(match[2]),
                parseInt(match[3])
            );
        }
        
        // 匹配时分格式
        let timeRegex2 = /^(\d{1,2}):(\d{1,2})$/;
        match = t.match(timeRegex2);
        if (match) {
            return new MyDate(
                null,
                null,
                null,
                parseInt(match[1]),
                parseInt(match[2]),
                0
            );
        }
        
        return null;
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.MyDate = MyDate;
}
