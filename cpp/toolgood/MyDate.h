// MyDate.h - 对应 csharp/ToolGood.Algorithm/Operands/MyDate.cs
// 年/月/日为 0 表示未设置（对应 C# 的 int? 为 null）。
#pragma once

#include <cstdint>
#include <string>

namespace toolgood {

/// 日期/时间值（可只含日期、只含时间或含"日+时间"）。
class MyDate {
public:
    int Year = 0;
    int Month = 0;
    int Day = 0;
    int Hour = 0;
    int Minute = 0;
    int Second = 0;

    MyDate() = default;
    MyDate(int year, int month, int day, int hour, int minute, int second)
        : Year(year), Month(month), Day(day), Hour(hour), Minute(minute), Second(second) {}
    explicit MyDate(int64_t timestamp, bool useLocalTime);

    /// 解析文本为 MyDate，失败返回 false。
    static bool TryParse(const std::string& txt, MyDate& out);
    static MyDate Parse(const std::string& txt) {
        MyDate d;
        TryParse(txt, d);
        return d;
    }

    /// 由 epoch 秒构造。
    static MyDate FromTimestamp(int64_t seconds, bool useLocalTime);
    /// 转为 epoch 秒。
    int64_t ToTimestamp(bool useLocalTime) const;
    /// 当前时间。
    static MyDate Now(bool useLocalTime);
    /// 今天（时间部分为 0）。
    static MyDate Today(bool useLocalTime);

    std::string ToString() const;
    /// 按 .NET 格式串输出（支持 yyyy/yy/MM/M/dd/d/HH/H/mm/m/ss/s 等）。
    std::string ToString(const std::string& format) const;

    /// 转天数（自 1970-01-01）与当日秒数。
    int64_t ToDays() const;
    int32_t ToSecondsOfDay() const;
    /// 转 "yyyyMMddHHmmss" 形式的长整数（对应 ToLong）。
    int64_t ToLong() const;

    MyDate AddYears(int years) const;
    MyDate AddMonths(int months) const;
    MyDate AddDays(int days) const;
    MyDate AddHours(int hours) const;
    MyDate AddMinutes(int minutes) const;
    MyDate AddSeconds(int seconds) const;

    /// 星期几：0=周日 .. 6=周六
    int DayOfWeek() const;
    /// 一年中的第几天（1 起）
    int DayOfYear() const;
    /// 该月天数
    int DaysInMonth() const;

    /// 仅含时间部分时转为"秒"，用于时间运算（对应 ToTimeSpan）。
    int64_t ToTimeSpanSeconds() const;

    bool HasDate() const { return Year > 0 && Month > 0 && Day > 0; }
};

/// 格式化 epoch 秒（本地/UTC），对应 MyDate.ToString(format) 的底层实现。
std::string FormatDateTime(const MyDate& dt, const std::string& pattern);

}  // namespace toolgood
