// DateTimes.cpp - 对应 csharp/.../Internals/Functions/DateTimes/
#include "DateTimes.h"

#include <algorithm>
#include <cstdint>
#include <ctime>
#include <string>
#include <vector>

#include "../../../AlgorithmEngine.h"
#include "../../../Decimal.h"
#include "../../../MyDate.h"
#include "../../../Operands/Operand.h"
#include "../../FunctionUtil.h"

namespace toolgood {

namespace {

const int64_t kSecondsPerDay = 86400;
const int64_t kMillisPerDay = 86400000;
/// 0001-01-01 到 1899-12-30 的毫秒数（.NET DoubleDateOffset）
const int64_t kOADateOffsetMillis = 59926435200000LL;
/// DateTime.MaxValue 的毫秒数（含 9999-12-31 23:59:59.999）
const int64_t kMaxDateTimeMillis = 315537897599999LL;
/// 1970-01-01 距 0001-01-01 的天数
const int64_t kEpochDayOffset = 719162LL;
/// 0001-01-01 / 9999-12-31 的 Unix 秒与毫秒
const int64_t kMinUnixSecond = -62135596800LL;
const int64_t kMaxUnixSecond = 253402300799LL;
const int64_t kMinUnixMillis = -62135596800000LL;
const int64_t kMaxUnixMillis = 253402300799999LL;

const Decimal& DecOADateMax() {
    static const Decimal v = Decimal::FromString("2958465.99999999");
    return v;
}

const Decimal& DecOADateMin() {
    static const Decimal v = Decimal::FromInt(-657435);
    return v;
}

const Decimal& DecUnixSecondMax() {
    static const Decimal v = Decimal::FromString("253402300800");
    return v;
}

const Decimal& DecUnixMilliMax() {
    static const Decimal v = Decimal::FromString("253402300800000");
    return v;
}

const Decimal& DecInt64Min() {
    static const Decimal v = Decimal::FromString("-9223372036854775808");
    return v;
}

const Decimal& DecInt64Max() {
    static const Decimal v = Decimal::FromString("9223372036854775807");
    return v;
}

/// 对应 C# 的 decimal -> int 转换：先向零截断，越界返回 false（OverflowException）。
bool TryIntValue(const Operand& op, int& out) {
    Decimal t = op.NumberValue().Trunc();
    if (!t.FitsInt32()) return false;
    out = t.Int();
    return true;
}

/// 对应 C# 的 decimal -> long 转换：先向零截断，越界返回 false。
bool TryLongValue(const Operand& op, int64_t& out) {
    Decimal t = op.NumberValue().Trunc();
    if (t.Less(DecInt64Min()) || t.Greater(DecInt64Max())) return false;
    out = t.Int64();
    return true;
}

int64_t FloorDiv(int64_t a, int64_t b) {
    int64_t q = a / b;
    if (a % b != 0 && ((a < 0) != (b < 0))) q--;
    return q;
}

bool ValidYear(int year) { return year >= 1 && year <= 9999; }

/// DateTime.DaysInMonth
int DaysInMonthOf(int year, int month) { return MyDate(year, month, 1, 0, 0, 0).DaysInMonth(); }

/// 某年某月某日距 1970-01-01 的天数（等价 DateTime.Date 的日序号）
int64_t DayNumber(int year, int month, int day) {
    return MyDate(year, month, day, 0, 0, 0).ToDays();
}

/// 完整的「日期 + 时间」秒数（等价 DateTime 的 Ticks 精度到秒）
int64_t DateTimeSeconds(const MyDate& d) { return d.ToDays() * kSecondsPerDay + d.ToSecondsOfDay(); }

bool IsWeekend(int dayOfWeek) { return dayOfWeek == 0 || dayOfWeek == 6; }

/// 对应 C# 的 decimal / double 混合截断：TotalDays 之类的天数差
double TotalDaysBetween(const MyDate& start, const MyDate& end) {
    return static_cast<double>(DateTimeSeconds(end) - DateTimeSeconds(start)) /
           static_cast<double>(kSecondsPerDay);
}

bool IsLeapYear(int y) { return (y % 4 == 0 && y % 100 != 0) || y % 400 == 0; }

std::string TrimSpace(const std::string& s) {
    size_t b = 0, e = s.size();
    auto isSpace = [](char c) {
        return c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\f' || c == '\v';
    };
    while (b < e && isSpace(s[b])) b++;
    while (e > b && isSpace(s[e - 1])) e--;
    return s.substr(b, e - b);
}

/// .NET DateTime.FromOADate：以 1899-12-30 为 0 的日数，毫秒四舍五入（远离零）。
bool FromOADate(const Decimal& value, MyDate& out) {
    const double d = value.Float64();
    // .NET: value >= 2958466.0 或 <= -657435.0 视为越界
    if (d >= 2958466.0 || d <= -657435.0) return false;
    int64_t millis = static_cast<int64_t>(d * static_cast<double>(kMillisPerDay) +
                                          (d >= 0 ? 0.5 : -0.5));
    // 负值需跳过 1900-02-29 这个不存在的日期（.NET 内部实现）
    if (millis < 0) millis -= (millis % kMillisPerDay) * 2;
    const int64_t total = millis + kOADateOffsetMillis;
    if (total < 0 || total > kMaxDateTimeMillis) return false;
    const int64_t days = FloorDiv(total, kMillisPerDay);
    const int64_t msOfDay = total - days * kMillisPerDay;
    const int64_t sec = (days - kEpochDayOffset) * kSecondsPerDay + msOfDay / 1000;
    out = MyDate::FromTimestamp(sec, false);
    return true;
}

/// 取本地时区相对 UTC 的偏移秒数（DST 未定）。用可表示的代理年份，
/// 避免 CRT 的 mktime/localtime 只能处理 1970~3000 年。
int64_t LocalOffsetSeconds(const MyDate& d) {
    if (d.Month < 1 || d.Month > 12 || d.Day < 1 || d.Day > 31) return 0;
    std::tm local{};
    local.tm_year = 2000 - 1900;
    local.tm_mon = d.Month - 1;
    local.tm_mday = d.Day;
    local.tm_hour = d.Hour;
    local.tm_min = d.Minute;
    local.tm_sec = d.Second;
    local.tm_isdst = -1;
    std::tm utc = local;
    utc.tm_isdst = 0;
    const std::time_t lt = std::mktime(&local);
#if defined(_WIN32)
    const std::time_t gt = _mkgmtime(&utc);
#else
    const std::time_t gt = timegm(&utc);
#endif
    if (lt == static_cast<std::time_t>(-1) || gt == static_cast<std::time_t>(-1)) return 0;
    return static_cast<int64_t>(lt) - static_cast<int64_t>(gt);
}

/// Unix 秒转 MyDate；useLocalTime 时转本地时间。
/// CRT 的 localtime 只覆盖 1970~3000 年，超出时按代理年份的时区偏移换算。
MyDate FromUnixSecondsValue(int64_t seconds, bool useLocalTime) {
    if (!useLocalTime) return MyDate::FromTimestamp(seconds, false);
    const MyDate local = MyDate::FromTimestamp(seconds, true);
    if (local.Year >= 1 && local.Year <= 9999 && local.Month >= 1 && local.Day >= 1) return local;
    const MyDate utc = MyDate::FromTimestamp(seconds, false);
    int64_t shifted = seconds - LocalOffsetSeconds(utc);
    // 换算后超出 DateTime 范围时夹取到边界（.NET 在边界处同样夹取）
    if (shifted < kMinUnixSecond) shifted = kMinUnixSecond;
    if (shifted > kMaxUnixSecond) shifted = kMaxUnixSecond;
    return MyDate::FromTimestamp(shifted, false);
}

/// MyDate 转 Unix 秒；useLocalTime 时按本地时间解释。
/// CRT 的 mktime 只覆盖 1970~3000 年，超出时按代理年份的时区偏移换算。
int64_t ToUnixSecondsValue(const MyDate& d, bool useLocalTime) {
    if (!useLocalTime) return d.ToTimestamp(false);
    const int64_t ts = d.ToTimestamp(true);
    if (ts != -1) return ts;
    return d.ToTimestamp(false) + LocalOffsetSeconds(d);
}

/// 由 Unix 毫秒构造 MyDate；越界返回 false（对应 DateTime 越界异常）。
bool FromUnixMillis(int64_t millis, bool useLocalTime, MyDate& out) {
    if (millis < kMinUnixMillis || millis > kMaxUnixMillis) return false;
    out = FromUnixSecondsValue(FloorDiv(millis, 1000), useLocalTime);
    return true;
}

/// 由 Unix 秒构造 MyDate；越界返回 false。
bool FromUnixSeconds(int64_t seconds, bool useLocalTime, MyDate& out) {
    if (seconds < kMinUnixSecond || seconds > kMaxUnixSecond) return false;
    out = FromUnixSecondsValue(seconds, useLocalTime);
    return true;
}

/// 解析非负整数（仅数字）；失败返回 false。
bool ParseInteger(const std::string& s, int64_t& value) {
    if (s.empty()) return false;
    int64_t v = 0;
    for (size_t i = 0; i < s.size(); i++) {
        if (s[i] < '0' || s[i] > '9') return false;
        v = v * 10 + (s[i] - '0');
        if (v > 1000000000LL) return false;
    }
    value = v;
    return true;
}

/// 对应 TimeSpan.TryParse 的常见格式：[-]d.hh:mm:ss[.fffffff]、[-]hh:mm[:ss[.fffffff]]。
bool ParseTimeSpan(const std::string& txt, MyDate& out) {
    std::string s = TrimSpace(txt);
    if (s.empty()) return false;
    bool negative = false;
    if (s[0] == '-') {
        negative = true;
        s.erase(0, 1);
    }
    if (s.empty()) return false;

    const size_t colon = s.find(':');
    if (colon == std::string::npos) {
        // 纯数字按「天」解释
        int64_t days = 0;
        if (!ParseInteger(s, days)) return false;
        if (negative) days = -days;
        out = MyDate(0, 0, static_cast<int>(days), 0, 0, 0);
        return true;
    }

    int64_t days = 0;
    bool hasExplicitDays = false;
    const size_t dot = s.find('.');
    if (dot != std::string::npos && dot > 0 && dot < colon) {
        if (!ParseInteger(s.substr(0, dot), days)) return false;
        hasExplicitDays = true;
        s = s.substr(dot + 1);
    }

    std::vector<std::string> segs;
    size_t start = 0;
    while (true) {
        const size_t p = s.find(':', start);
        if (p == std::string::npos) {
            segs.push_back(s.substr(start));
            break;
        }
        segs.push_back(s.substr(start, p - start));
        start = p + 1;
    }
    if (segs.size() != 2 && segs.size() != 3) return false;

    int64_t hour = 0, minute = 0, second = 0;
    if (!ParseInteger(segs[0], hour) || !ParseInteger(segs[1], minute)) return false;

    if (segs.size() == 3) {
        std::string secText = segs[2];
        const size_t sd = secText.find('.');
        if (sd != std::string::npos) secText = secText.substr(0, sd);
        if (!ParseInteger(secText, second)) return false;
        if (hour > 23 && !hasExplicitDays) {
            // .NET: "A:B:C" 中 A 超出 23 时按「日:时:分」解释
            days += hour;
            hour = minute;
            minute = second;
            second = 0;
        }
    }
    if (hour < 0 || hour > 23 || minute < 0 || minute > 59 || second < 0 || second > 59) {
        return false;
    }
    if (negative) {
        days = -days;
        hour = -hour;
        minute = -minute;
        second = -second;
    }
    out = MyDate(0, 0, static_cast<int>(days), static_cast<int>(hour), static_cast<int>(minute),
                 static_cast<int>(second));
    return true;
}

/// 大小写不敏感比较（对应 C# StringComparison.OrdinalIgnoreCase）
bool EqualsIgnoreCase(const std::string& a, const std::string& b) {
    if (a.size() != b.size()) return false;
    for (size_t i = 0; i < a.size(); i++) {
        char x = a[i], y = b[i];
        if (x >= 'a' && x <= 'z') x = static_cast<char>(x - 'a' + 'A');
        if (y >= 'a' && y <= 'z') y = static_cast<char>(y - 'a' + 'A');
        if (x != y) return false;
    }
    return true;
}

// ------------------------------------------------------------------ ADD*

/// ADDDAYS / ADDHOURS / ADDMINUTES / ADDMONTHS / ADDSECONDS / ADDYEARS 的公共骨架
class FunctionAddBase : public FunctionN {
public:
    explicit FunctionAddBase(std::vector<Function> args) : FunctionN(std::move(args)) {}

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

class Function_ADDDAYS : public FunctionAddBase {
public:
    explicit Function_ADDDAYS(std::vector<Function> args) : FunctionAddBase(std::move(args)) {
        RequireArgs(args_, "AddDays", 2, 2);
    }

    std::string Name() const override { return "AddDays"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int n = 0;
        if (!TryIntValue(args2, n)) return ParameterError(2);
        return Operand::Create(args1.DateValue().AddDays(n));
    }
};

class Function_ADDHOURS : public FunctionAddBase {
public:
    explicit Function_ADDHOURS(std::vector<Function> args) : FunctionAddBase(std::move(args)) {
        RequireArgs(args_, "AddHours", 2, 2);
    }

    std::string Name() const override { return "AddHours"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int n = 0;
        if (!TryIntValue(args2, n)) return ParameterError(2);
        return Operand::Create(args1.DateValue().AddHours(n));
    }
};

class Function_ADDMINUTES : public FunctionAddBase {
public:
    explicit Function_ADDMINUTES(std::vector<Function> args) : FunctionAddBase(std::move(args)) {
        RequireArgs(args_, "AddMinutes", 2, 2);
    }

    std::string Name() const override { return "AddMinutes"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int n = 0;
        if (!TryIntValue(args2, n)) return ParameterError(2);
        return Operand::Create(args1.DateValue().AddMinutes(n));
    }
};

class Function_ADDMONTHS : public FunctionAddBase {
public:
    explicit Function_ADDMONTHS(std::vector<Function> args) : FunctionAddBase(std::move(args)) {
        RequireArgs(args_, "AddMonths", 2, 2);
    }

    std::string Name() const override { return "AddMonths"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int n = 0;
        if (!TryIntValue(args2, n)) return ParameterError(2);
        return Operand::Create(args1.DateValue().AddMonths(n));
    }
};

class Function_ADDSECONDS : public FunctionAddBase {
public:
    explicit Function_ADDSECONDS(std::vector<Function> args) : FunctionAddBase(std::move(args)) {
        RequireArgs(args_, "AddSeconds", 2, 2);
    }

    std::string Name() const override { return "AddSeconds"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int n = 0;
        if (!TryIntValue(args2, n)) return ParameterError(2);
        return Operand::Create(args1.DateValue().AddSeconds(n));
    }
};

class Function_ADDYEARS : public FunctionAddBase {
public:
    explicit Function_ADDYEARS(std::vector<Function> args) : FunctionAddBase(std::move(args)) {
        RequireArgs(args_, "AddYears", 2, 2);
    }

    std::string Name() const override { return "AddYears"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int n = 0;
        if (!TryIntValue(args2, n)) return ParameterError(2);
        return Operand::Create(args1.DateValue().AddYears(n));
    }
};

// ------------------------------------------------------------------ DATE

class Function_DATE : public FunctionN {
public:
    explicit Function_DATE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Date", 3, 6);
    }

    std::string Name() const override { return "Date"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        if (args_.size() >= 4) {
            args_[3]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
        if (args_.size() >= 5) {
            args_[4]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
        if (args_.size() >= 6) {
            args_[5]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        int year = 0, month = 0, day = 0;
        if (!TryIntValue(args1, year)) return ParameterError(1);
        if (!TryIntValue(args2, month)) return ParameterError(2);
        if (!TryIntValue(args3, day)) return ParameterError(3);

        // new DateTime(year, 1, 1).AddMonths(month - 1).AddDays(day - 1)
        if (!ValidYear(year)) return ParameterError(1);
        MyDate baseDate = MyDate(year, 1, 1, 0, 0, 0).AddMonths(month - 1);
        if (!ValidYear(baseDate.Year)) return ParameterError(1);
        baseDate = baseDate.AddDays(day - 1);
        if (!ValidYear(baseDate.Year)) return ParameterError(1);

        int hour = 0, minute = 0, second = 0;
        if (ArgCount() >= 4) {
            auto args4 = GetNumber(4, engine, tp);
            if (args4.IsErrorOrNone()) return args4;
            if (!TryIntValue(args4, hour)) return ParameterError(4);
            if (hour < 0 || hour > 23) return ParameterError(4);
        }
        if (ArgCount() >= 5) {
            auto args5 = GetNumber(5, engine, tp);
            if (args5.IsErrorOrNone()) return args5;
            if (!TryIntValue(args5, minute)) return ParameterError(5);
            if (minute < 0 || minute > 59) return ParameterError(5);
        }
        if (ArgCount() >= 6) {
            auto args6 = GetNumber(6, engine, tp);
            if (args6.IsErrorOrNone()) return args6;
            if (!TryIntValue(args6, second)) return ParameterError(6);
            if (second < 0 || second > 59) return ParameterError(6);
        }

        MyDate d(baseDate.Year, baseDate.Month, baseDate.Day, hour, minute, second);
        return Operand::Create(d);
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

// ------------------------------------------------------------------ DATEDIF

class Function_DATEDIF : public FunctionN {
public:
    explicit Function_DATEDIF(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "DateDif", 3, 3);
    }

    std::string Name() const override { return "DateDif"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[2]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetDate(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        auto args3 = GetText(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        const MyDate startMyDate = args1.DateValue();
        const MyDate endMyDate = args2.DateValue();
        // C# 中 ToDateTime() 对「时间型」操作数抛越界异常，此处按参数错误处理
        if (!startMyDate.HasDate()) return ParameterError(1);
        if (!endMyDate.HasDate()) return ParameterError(2);
        const std::string t = args3.TextValue();

        if (EqualsIgnoreCase(t, "Y")) {
            bool b = false;
            if (startMyDate.Month < endMyDate.Month) {
                b = true;
            } else if (startMyDate.Month == endMyDate.Month) {
                if (startMyDate.Day <= endMyDate.Day) b = true;
            }
            if (b) return Operand::Create(endMyDate.Year - startMyDate.Year);
            return Operand::Create(endMyDate.Year - startMyDate.Year - 1);
        } else if (EqualsIgnoreCase(t, "M")) {
            const int months = endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 -
                               startMyDate.Month;
            if (startMyDate.Day <= endMyDate.Day) return Operand::Create(months);
            return Operand::Create(months - 1);
        } else if (EqualsIgnoreCase(t, "D")) {
            const int64_t days =
                (DateTimeSeconds(endMyDate) - DateTimeSeconds(startMyDate)) / kSecondsPerDay;
            return Operand::Create(static_cast<int>(days));
        } else if (EqualsIgnoreCase(t, "YD")) {
            // Excel: 将 end 的月日置于 start 年构造候选日期, 若候选早于 start 则加一年, 再求天数差
            int candDay = endMyDate.Day;
            const int maxDay = DaysInMonthOf(startMyDate.Year, endMyDate.Month);
            if (candDay > maxDay) candDay = maxDay;
            MyDate cand(startMyDate.Year, endMyDate.Month, candDay, 0, 0, 0);
            if (DateTimeSeconds(cand) < DateTimeSeconds(startMyDate)) {
                // start 年为 9999 时无法再加一年（超出 DateTime 上限），按超范围处理
                if (startMyDate.Year == 9999) return ParameterError(3);
                cand = cand.AddYears(1);
            }
            const int64_t day =
                (DateTimeSeconds(cand) - DateTimeSeconds(startMyDate)) / kSecondsPerDay;
            return Operand::Create(static_cast<int>(day));
        } else if (EqualsIgnoreCase(t, "MD")) {
            int mo = endMyDate.Day - startMyDate.Day;
            if (mo < 0) {
                // 取 start 所在月的天数
                mo += DaysInMonthOf(startMyDate.Year, startMyDate.Month);
            }
            return Operand::Create(mo);
        } else if (EqualsIgnoreCase(t, "YM")) {
            int mo = endMyDate.Month - startMyDate.Month;
            if (endMyDate.Day < startMyDate.Day) mo--;
            if (mo < 0) mo += 12;
            return Operand::Create(mo);
        }
        return ParameterError(3);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ DATEVALUE

class Function_DATEVALUE : public FunctionN {
public:
    explicit Function_DATEVALUE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "DateValue", 1, 2);
    }

    std::string Name() const override { return "DateValue"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NONE);
        if (args_.size() >= 2) {
            args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = EvalArg(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        int type = 0;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            if (!TryIntValue(args2, type)) return ParameterError(2);
        }
        // 先校验第 2 参数，避免非法 type 被 args1 是日期的短路逻辑静默忽略
        if (type < 0 || type > 4) return ParameterError(2);
        if (args1.IsDate()) return args1;

        if (type == 0) {
            if (args1.IsText()) {
                MyDate d;
                if (MyDate::TryParse(args1.TextValue(), d)) return Operand::Create(d);
            }
            auto arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) return arg1;
            const Decimal v = arg1.NumberValue();
            // Excel/OLE 序列号：以 1899-12-30 为 0 的日数
            if (v.LessOrEqual(DecOADateMax())) {
                if (v.LessOrEqual(DecOADateMin())) return ParameterError(1);
                MyDate d;
                if (!FromOADate(v, d)) return ParameterError(1);
                return Operand::Create(d);
            }
            // Unix 时间戳（秒）
            if (v.Less(DecUnixSecondMax())) {
                int64_t n = 0;
                if (!TryLongValue(arg1, n)) return ParameterError(1);
                MyDate d;
                if (!FromUnixSeconds(n, engine.UseLocalTime, d)) return ParameterError(1);
                return Operand::Create(d);
            }
            // Unix 时间戳（毫秒）
            if (v.GreaterOrEqual(DecUnixMilliMax())) return ParameterError(1);
            int64_t n = 0;
            if (!TryLongValue(arg1, n)) return ParameterError(1);
            MyDate d;
            if (!FromUnixMillis(n, engine.UseLocalTime, d)) return ParameterError(1);
            return Operand::Create(d);
        } else if (type == 1) {
            auto arg1 = ConvertToText(args1, 1);
            if (arg1.IsErrorOrNone()) return arg1;
            MyDate d;
            if (MyDate::TryParse(arg1.TextValue(), d)) return Operand::Create(d);
            return ParameterError(1);
        } else if (type == 2) {
            auto arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) return arg1;
            const Decimal v = arg1.NumberValue();
            if (v.LessOrEqual(DecOADateMin()) || v.Greater(DecOADateMax())) return ParameterError(1);
            MyDate d;
            if (!FromOADate(v, d)) return ParameterError(1);
            return Operand::Create(d);
        } else if (type == 3) {
            auto arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) return arg1;
            int64_t n = 0;
            if (!TryLongValue(arg1, n)) return ParameterError(1);
            MyDate d;
            if (!FromUnixMillis(n, engine.UseLocalTime, d)) return ParameterError(1);
            return Operand::Create(d);
        } else if (type == 4) {
            auto arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) return arg1;
            int64_t n = 0;
            if (!TryLongValue(arg1, n)) return ParameterError(1);
            MyDate d;
            if (!FromUnixSeconds(n, engine.UseLocalTime, d)) return ParameterError(1);
            return Operand::Create(d);
        }
        return ParameterError(2);
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

// ------------------------------------------------------------------ DAY / MONTH / YEAR

class Function_DAY : public FunctionN {
public:
    explicit Function_DAY(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Day", 1, 1);
    }

    std::string Name() const override { return "Day"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const MyDate d = args1.DateValue();
        if (d.Day == 0) return ParameterError(1);
        return Operand::Create(d.Day);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_MONTH : public FunctionN {
public:
    explicit Function_MONTH(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Month", 1, 1);
    }

    std::string Name() const override { return "Month"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const MyDate d = args1.DateValue();
        if (d.Month == 0) return ParameterError(1);
        return Operand::Create(d.Month);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_YEAR : public FunctionN {
public:
    explicit Function_YEAR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Year", 1, 1);
    }

    std::string Name() const override { return "Year"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const MyDate d = args1.DateValue();
        if (d.Year == 0) return ParameterError(1);
        return Operand::Create(d.Year);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_HOUR : public FunctionN {
public:
    explicit Function_HOUR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Hour", 1, 1);
    }

    std::string Name() const override { return "Hour"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(args1.DateValue().Hour);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_MINUTE : public FunctionN {
public:
    explicit Function_MINUTE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Minute", 1, 1);
    }

    std::string Name() const override { return "Minute"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(args1.DateValue().Minute);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_SECOND : public FunctionN {
public:
    explicit Function_SECOND(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Second", 1, 1);
    }

    std::string Name() const override { return "Second"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(args1.DateValue().Second);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ DAYS

class Function_DAYS : public FunctionN {
public:
    explicit Function_DAYS(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Days", 2, 2);
    }

    std::string Name() const override { return "Days"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::DATE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetDate(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        const MyDate endDate = args1.DateValue();
        const MyDate startDate = args2.DateValue();
        if (!endDate.HasDate()) return ParameterError(1);
        if (!startDate.HasDate()) return ParameterError(2);
        const int64_t days =
            DayNumber(endDate.Year, endDate.Month, endDate.Day) -
            DayNumber(startDate.Year, startDate.Month, startDate.Day);
        return Operand::Create(static_cast<int>(days));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ DAYS360

class Function_DAYS360 : public FunctionN {
public:
    explicit Function_DAYS360(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Days360", 2, 3);
    }

    std::string Name() const override { return "Days360"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        if (args_.size() >= 3) {
            args_[2]->GetParameterTypes(noneEngine, result, OperandType::BOOLEAN);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetDate(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const MyDate startMyDate = args1.DateValue();
        const MyDate endMyDate = args2.DateValue();
        if (!startMyDate.HasDate()) return ParameterError(1);
        if (!endMyDate.HasDate()) return ParameterError(2);

        bool method = false;
        if (ArgCount() >= 3) {
            auto args3 = GetBoolean(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            method = args3.BooleanValue();
        }

        int days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30 -
                   startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
        if (method) {
            // 欧洲方法: 仅将 31 日按 30 日计算, 日号仍需正常参与天数差
            days += std::min(endMyDate.Day, 30) - std::min(startMyDate.Day, 30);
        } else {
            // US (NASD) 方法: start 若为月末(含 2 月最后一天)则调整为 30 日,
            // end 若为月末, 依据调整后的 startDay 决定按 31 日(下月1日)或 30 日计算
            int startDay = startMyDate.Day;
            int endDay = endMyDate.Day;
            if (startDay == DaysInMonthOf(startMyDate.Year, startMyDate.Month)) {
                startDay = 30;
            }
            if (endDay == DaysInMonthOf(endMyDate.Year, endMyDate.Month)) {
                endDay = startDay < 30 ? 31 : 30;
            }
            days += endDay - startDay;
        }
        return Operand::Create(days);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ EDATE / EOMONTH

class Function_EDATE : public FunctionN {
public:
    explicit Function_EDATE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "EDate", 2, 2);
    }

    std::string Name() const override { return "EDate"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int n = 0;
        if (!TryIntValue(args2, n)) return ParameterError(2);
        return Operand::Create(args1.DateValue().AddMonths(n));
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

class Function_EOMONTH : public FunctionN {
public:
    explicit Function_EOMONTH(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "EOMonth", 2, 2);
    }

    std::string Name() const override { return "EOMonth"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int n = 0;
        if (!TryIntValue(args2, n)) return ParameterError(2);

        const MyDate src = args1.DateValue();
        if (!src.HasDate()) return ParameterError(1);
        MyDate dt = src.AddMonths(n + 1);
        if (!ValidYear(dt.Year)) return ParameterError(2);
        dt = MyDate(dt.Year, dt.Month, 1, 0, 0, 0).AddDays(-1);
        return Operand::Create(dt);
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

// ------------------------------------------------------------------ NETWORKDAYS

class Function_NETWORKDAYS : public FunctionN {
public:
    explicit Function_NETWORKDAYS(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "NetworkDays", 2, -1);
    }

    std::string Name() const override { return "NetworkDays"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        // C# 对 funcs 全部按 DATE 处理（含可选节假日）
        for (auto& f : args_) {
            f->GetParameterTypes(noneEngine, result, OperandType::DATE);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetDate(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const MyDate startRaw = args1.DateValue();
        const MyDate endRaw = args2.DateValue();
        if (!startRaw.HasDate()) return ParameterError(1);
        if (!endRaw.HasDate()) return ParameterError(2);

        int64_t startDay = DayNumber(startRaw.Year, startRaw.Month, startRaw.Day);
        int64_t endDay = DayNumber(endRaw.Year, endRaw.Month, endRaw.Day);

        // 对应 C# 的 HashSet<DateTime>（取 .Date 后去重）
        std::vector<int64_t> holidays;
        for (size_t i = 3; i <= ArgCount(); i++) {
            auto ar = GetDate(static_cast<int>(i), engine, tp);
            if (ar.IsErrorOrNone()) return ar;
            const MyDate hd = ar.DateValue();
            if (!hd.HasDate()) return ParameterError(static_cast<int>(i));
            const int64_t key = DayNumber(hd.Year, hd.Month, hd.Day);
            if (std::find(holidays.begin(), holidays.end(), key) == holidays.end()) {
                holidays.push_back(key);
            }
        }

        bool negative = false;
        if (startDay > endDay) {
            std::swap(startDay, endDay);
            negative = true;
        }
        // 数学统计工作日:完整周数 × 5 + 余数逐天,再减去落在工作日的节假日
        const int64_t totalDays = endDay - startDay + 1;
        const int64_t fullWeeks = totalDays / 7;
        const int64_t remainder = totalDays % 7;
        int64_t days = fullWeeks * 5;
        for (int64_t i = 0; i < remainder; i++) {
            const MyDate d = MyDate::FromTimestamp((startDay + i) * kSecondsPerDay, false);
            if (!IsWeekend(d.DayOfWeek())) days++;
        }
        for (int64_t h : holidays) {
            if (h >= startDay && h <= endDay) {
                const MyDate d = MyDate::FromTimestamp(h * kSecondsPerDay, false);
                if (!IsWeekend(d.DayOfWeek())) days--;
            }
        }
        return Operand::Create(static_cast<int>(negative ? -days : days));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ NOW / TODAY

class Function_NOW : public FunctionN {
public:
    explicit Function_NOW(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Now", 0, 0);
    }

    std::string Name() const override { return "Now"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* /*tp*/) override {
        return Operand::Create(MyDate::Now(engine.UseLocalTime));
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

class Function_TODAY : public FunctionN {
public:
    explicit Function_TODAY(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Today", 0, 0);
    }

    std::string Name() const override { return "Today"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* /*tp*/) override {
        return Operand::Create(MyDate::Today(engine.UseLocalTime));
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

// ------------------------------------------------------------------ TIME

class Function_TIME : public FunctionN {
public:
    explicit Function_TIME(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Time", 2, 3);
    }

    std::string Name() const override { return "Time"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        if (args_.size() >= 3) {
            args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        int hour = 0, minute = 0;
        if (!TryIntValue(args1, hour)) return ParameterError(1);
        if (!TryIntValue(args2, minute)) return ParameterError(2);
        if (hour < 0 || hour > 23) return ParameterError(1);
        if (minute < 0 || minute > 59) return ParameterError(2);

        int second = 0;
        if (ArgCount() >= 3) {
            auto args3 = GetNumber(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            if (!TryIntValue(args3, second)) return ParameterError(3);
            if (second < 0 || second > 59) return ParameterError(3);
        }
        return Operand::Create(MyDate(0, 0, 0, hour, minute, second));
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

// ------------------------------------------------------------------ TIMESTAMP

class Function_TIMESTAMP : public FunctionN {
public:
    explicit Function_TIMESTAMP(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Timestamp", 1, 2);
    }

    std::string Name() const override { return "Timestamp"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        if (args_.size() >= 2) {
            args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        int type = 0;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            type = args2.IntValue();
        }
        auto args0 = GetDate(1, engine, tp);
        if (args0.IsErrorOrNone()) return args0;

        const MyDate d = args0.DateValue();
        if (!d.HasDate()) return ParameterError(1);
        // UseLocalTime 时按本地时间解释，否则按 UTC
        const int64_t seconds = ToUnixSecondsValue(d, engine.UseLocalTime);
        if (type == 0) {
            return Operand::Create(static_cast<double>(seconds * 1000));
        } else if (type == 1) {
            return Operand::Create(static_cast<double>(seconds));
        }
        return ParameterError(2);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ TIMEVALUE

class Function_TIMEVALUE : public FunctionN {
public:
    explicit Function_TIMEVALUE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "TimeValue", 1, 1);
    }

    std::string Name() const override { return "TimeValue"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        MyDate d;
        if (ParseTimeSpan(args1.TextValue(), d)) return Operand::Create(d);
        return ParameterError(1);
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

// ------------------------------------------------------------------ WEEKDAY

class Function_WEEKDAY : public FunctionN {
public:
    explicit Function_WEEKDAY(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Weekday", 1, 2);
    }

    std::string Name() const override { return "Weekday"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        if (args_.size() >= 2) {
            args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        int type = 1;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            if (!TryIntValue(args2, type)) return ParameterError(2);
            if (type != 1 && type != 2 && type != 3 && (type < 11 || type > 17)) {
                return ParameterError(2);
            }
        }

        const MyDate d = args1.DateValue();
        if (!d.HasDate()) return ParameterError(1);
        const int dayOfWeek = d.DayOfWeek();  // 0=周日 .. 6=周六

        if (type == 1 || type == 17) {
            return Operand::Create(dayOfWeek + 1);
        } else if (type == 2 || type == 11) {
            if (dayOfWeek == 0) return Operand::Create(7);
            return Operand::Create(dayOfWeek);
        } else if (type == 3) {
            if (dayOfWeek == 0) return Operand::Create(6);
            return Operand::Create(dayOfWeek - 1);
        } else if (type == 12) {
            static const int mapping[7] = {6, 7, 1, 2, 3, 4, 5};
            return Operand::Create(mapping[dayOfWeek]);
        } else if (type == 13) {
            static const int mapping[7] = {5, 6, 7, 1, 2, 3, 4};
            return Operand::Create(mapping[dayOfWeek]);
        } else if (type == 14) {
            static const int mapping[7] = {4, 5, 6, 7, 1, 2, 3};
            return Operand::Create(mapping[dayOfWeek]);
        } else if (type == 15) {
            static const int mapping[7] = {3, 4, 5, 6, 7, 1, 2};
            return Operand::Create(mapping[dayOfWeek]);
        } else if (type == 16) {
            static const int mapping[7] = {2, 3, 4, 5, 6, 7, 1};
            return Operand::Create(mapping[dayOfWeek]);
        }
        return ParameterError(2);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ WEEKNUM

class Function_WEEKNUM : public FunctionN {
public:
    explicit Function_WEEKNUM(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Weeknum", 1, 2);
    }

    std::string Name() const override { return "Weeknum"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        if (args_.size() >= 2) {
            args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        int returnType = 1;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            if (!TryIntValue(args2, returnType)) return ParameterError(2);
            if (returnType != 1 && returnType != 2 && returnType != 11 && returnType != 12 &&
                returnType != 13 && returnType != 14 && returnType != 15 && returnType != 16 &&
                returnType != 17 && returnType != 21) {
                return ParameterError(2);
            }
        }

        const MyDate startMyDate = args1.DateValue();
        if (!startMyDate.HasDate()) return ParameterError(1);

        if (returnType == 21) {
            // ISO 8601: 第 1 周是包含当年第一个周四的周
            int isoDow = startMyDate.DayOfWeek();  // 0=周日...6=周六
            if (isoDow == 0) isoDow = 7;           // 转为 1=周一...7=周日
            const MyDate thursday = startMyDate.AddDays(4 - isoDow);
            const int weekNumber = (thursday.DayOfYear() - 1) / 7 + 1;
            return Operand::Create(weekNumber);
        }

        const MyDate jan1(startMyDate.Year, 1, 1, 0, 0, 0);
        const int dayOfYear = startMyDate.DayOfYear();
        const int dayOfWeekJan1 = jan1.DayOfWeek();

        int weekStartDay = 0;
        if (returnType == 1 || returnType == 17) {
            weekStartDay = 0;
        } else if (returnType == 2 || returnType == 11) {
            weekStartDay = 1;
        } else if (returnType == 12) {
            weekStartDay = 2;
        } else if (returnType == 13) {
            weekStartDay = 3;
        } else if (returnType == 14) {
            weekStartDay = 4;
        } else if (returnType == 15) {
            weekStartDay = 5;
        } else {
            weekStartDay = 6;
        }

        const int daysUntilWeekStart = (dayOfWeekJan1 - weekStartDay + 7) % 7;
        const int adjustedDayOfYear = dayOfYear + daysUntilWeekStart;
        // 对应 (int)Math.Ceiling(adjustedDayOfYear / 7.0m)
        const int week = (adjustedDayOfYear + 6) / 7;
        return Operand::Create(week);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ WORKDAY

class Function_WORKDAY : public FunctionN {
public:
    explicit Function_WORKDAY(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Workday", 2, -1);
    }

    std::string Name() const override { return "Workday"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        for (size_t i = 2; i < args_.size(); i++) {
            args_[i]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetDate(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        int days = 0;
        if (!TryIntValue(args2, days)) return ParameterError(2);

        // 归一到当天零点；时间型操作数（年月日为空）按参数错误处理
        const MyDate startRaw = args1.DateValue();
        if (!startRaw.HasDate()) return ParameterError(1);
        MyDate start(startRaw.Year, startRaw.Month, startRaw.Day, 0, 0, 0);

        std::vector<int64_t> holidays;
        for (size_t i = 3; i <= ArgCount(); i++) {
            auto ar = GetDate(static_cast<int>(i), engine, tp);
            if (ar.IsErrorOrNone()) return ar;
            const MyDate hd = ar.DateValue();
            if (!hd.HasDate()) return ParameterError(static_cast<int>(i));
            const int64_t key = DayNumber(hd.Year, hd.Month, hd.Day);
            if (std::find(holidays.begin(), holidays.end(), key) == holidays.end()) {
                holidays.push_back(key);
            }
        }

        auto containsHoliday = [&holidays](const MyDate& d) {
            const int64_t key = DayNumber(d.Year, d.Month, d.Day);
            return std::find(holidays.begin(), holidays.end(), key) != holidays.end();
        };
        // 推移仅由起点与天数参与，越界时按第 2 参报错（对应 C# 的越界异常）
        auto shift = [&start](int n) -> bool {
            MyDate next = start.AddDays(n);
            if (!ValidYear(next.Year)) return false;
            start = next;
            return true;
        };

        if (days > 0) {
            // 先逐天对齐到周一（最多 6 天），期间消耗工作日
            while (start.DayOfWeek() != 1 && days > 0) {
                if (!shift(1)) return ParameterError(2);
                if (IsWeekend(start.DayOfWeek())) continue;
                if (containsHoliday(start)) continue;
                days--;
            }
            if (days == 0) return Operand::Create(start);

            // 整周粗跳：起点已是周一，每 5 个工作日 = 7 天
            MyDate afterJump = start.AddDays((days / 5) * 7);
            if (!ValidYear(afterJump.Year)) return ParameterError(2);
            const int64_t fromDay = DayNumber(start.Year, start.Month, start.Day);
            const int64_t toDay = DayNumber(afterJump.Year, afterJump.Month, afterJump.Day);
            int extra = 0;
            for (int64_t h : holidays) {
                if (h > fromDay && h <= toDay) {
                    const MyDate hd = MyDate::FromTimestamp(h * kSecondsPerDay, false);
                    if (!IsWeekend(hd.DayOfWeek())) extra++;
                }
            }
            start = afterJump;
            days = days % 5 + extra;
            while (days > 0) {
                if (!shift(1)) return ParameterError(2);
                if (IsWeekend(start.DayOfWeek())) continue;
                if (containsHoliday(start)) continue;
                days--;
            }
        } else if (days < 0) {
            // 先逐天对齐到周五（最多 6 天），期间消耗工作日
            while (start.DayOfWeek() != 5 && days < 0) {
                if (!shift(-1)) return ParameterError(2);
                if (IsWeekend(start.DayOfWeek())) continue;
                if (containsHoliday(start)) continue;
                days++;
            }
            if (days == 0) return Operand::Create(start);

            // 整周粗跳：起点已是周五，每 5 个工作日 = 7 天
            MyDate afterJump = start.AddDays((-days / 5) * -7);
            if (!ValidYear(afterJump.Year)) return ParameterError(2);
            const int64_t fromDay = DayNumber(start.Year, start.Month, start.Day);
            const int64_t toDay = DayNumber(afterJump.Year, afterJump.Month, afterJump.Day);
            int extra = 0;
            for (int64_t h : holidays) {
                if (h >= toDay && h < fromDay) {
                    const MyDate hd = MyDate::FromTimestamp(h * kSecondsPerDay, false);
                    if (!IsWeekend(hd.DayOfWeek())) extra++;
                }
            }
            start = afterJump;
            days = -((-days) % 5) - extra;
            while (days < 0) {
                if (!shift(-1)) return ParameterError(2);
                if (IsWeekend(start.DayOfWeek())) continue;
                if (containsHoliday(start)) continue;
                days++;
            }
        }
        return Operand::Create(start);
    }

    OperandType GetResultType() const override { return OperandType::DATE; }
};

// ------------------------------------------------------------------ YEARFRAC

Decimal Calculate30_360(const MyDate& startDate, const MyDate& endDate) {
    int d1 = startDate.Day;
    int d2 = endDate.Day;
    const bool d1FebLast = startDate.Month == 2 && startDate.Day == DaysInMonthOf(startDate.Year, 2);
    const bool d2FebLast = endDate.Month == 2 && endDate.Day == DaysInMonthOf(endDate.Year, 2);

    if (d1 == 31 && d2 == 31) {
        d1 = 30;
        d2 = 30;
    } else if (d1 == 31) {
        d1 = 30;
    } else if (d1 == 30 && d2 == 31) {
        d2 = 30;
    } else if (d1FebLast && d2FebLast) {
        d1 = 30;
        d2 = 30;
    } else if (d1FebLast) {
        d1 = 30;
    }

    const int days = 360 * (endDate.Year - startDate.Year) +
                     30 * (endDate.Month - startDate.Month) + (d2 - d1);
    return Decimal::FromInt(days).Div(Decimal::FromInt(360));
}

Decimal Calculate30_360E(const MyDate& startDate, const MyDate& endDate) {
    const int d1 = std::min(30, startDate.Day);
    const int d2 = std::min(30, endDate.Day);
    const int days = 360 * (endDate.Year - startDate.Year) +
                     30 * (endDate.Month - startDate.Month) + (d2 - d1);
    return Decimal::FromInt(days).Div(Decimal::FromInt(360));
}

Decimal CalculateActualActual(const MyDate& startDate, const MyDate& endDate) {
    const int startYear = startDate.Year;
    const int endYear = endDate.Year;

    if (startYear == endYear) {
        const int daysInYear = IsLeapYear(startYear) ? 366 : 365;
        return Decimal::FromDouble(TotalDaysBetween(startDate, endDate))
            .Div(Decimal::FromInt(daysInYear));
    }

    const int daysInStartYear = IsLeapYear(startYear) ? 366 : 365;
    const int daysInEndYear = IsLeapYear(endYear) ? 366 : 365;

    Decimal result = Decimal::FromDouble(TotalDaysBetween(
                         startDate, MyDate(startYear, 12, 31, 0, 0, 0)))
                         .Div(Decimal::FromInt(daysInStartYear));
    result = result.Add(Decimal::FromDouble(TotalDaysBetween(
                            MyDate(endYear, 1, 1, 0, 0, 0), endDate))
                            .Div(Decimal::FromInt(daysInEndYear)));
    result = result.Add(Decimal::FromInt(endYear - startYear - 1));
    return result;
}

Decimal CalculateYearFrac(const MyDate& startArg, const MyDate& endArg, int basis) {
    MyDate startDate = startArg;
    MyDate endDate = endArg;
    if (DateTimeSeconds(startDate) > DateTimeSeconds(endDate)) std::swap(startDate, endDate);

    switch (basis) {
        case 0:
            return Calculate30_360(startDate, endDate);
        case 1:
            return CalculateActualActual(startDate, endDate);
        case 2:
            return Decimal::FromDouble(TotalDaysBetween(startDate, endDate))
                .Div(Decimal::FromInt(360));
        case 3:
            return Decimal::FromDouble(TotalDaysBetween(startDate, endDate))
                .Div(Decimal::FromInt(365));
        case 4:
            return Calculate30_360E(startDate, endDate);
        default:
            return Calculate30_360(startDate, endDate);
    }
}

class Function_YEARFRAC : public FunctionN {
public:
    explicit Function_YEARFRAC(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "YearFrac", 2, 3);
    }

    std::string Name() const override { return "YearFrac"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::DATE);
        if (args_.size() >= 3) {
            args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto startArg = GetDate(1, engine, tp);
        if (startArg.IsErrorOrNone()) return startArg;
        const MyDate startDate = startArg.DateValue();
        if (!startDate.HasDate()) return ParameterError(1);

        auto endArg = GetDate(2, engine, tp);
        if (endArg.IsErrorOrNone()) return endArg;
        const MyDate endDate = endArg.DateValue();
        if (!endDate.HasDate()) return ParameterError(2);

        int basis = 0;
        if (ArgCount() >= 3) {
            auto basisArg = GetNumber(3, engine, tp);
            if (basisArg.IsErrorOrNone()) return basisArg;
            if (!TryIntValue(basisArg, basis)) return ParameterError(3);
            if (basis < 0 || basis > 4) return ParameterError(3);
        }

        return Operand::Create(CalculateYearFrac(startDate, endDate, basis));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

}  // namespace

Function NewFunction_ADDDAYS(std::vector<Function> args) {
    return std::make_shared<Function_ADDDAYS>(std::move(args));
}
Function NewFunction_ADDHOURS(std::vector<Function> args) {
    return std::make_shared<Function_ADDHOURS>(std::move(args));
}
Function NewFunction_ADDMINUTES(std::vector<Function> args) {
    return std::make_shared<Function_ADDMINUTES>(std::move(args));
}
Function NewFunction_ADDMONTHS(std::vector<Function> args) {
    return std::make_shared<Function_ADDMONTHS>(std::move(args));
}
Function NewFunction_ADDSECONDS(std::vector<Function> args) {
    return std::make_shared<Function_ADDSECONDS>(std::move(args));
}
Function NewFunction_ADDYEARS(std::vector<Function> args) {
    return std::make_shared<Function_ADDYEARS>(std::move(args));
}
Function NewFunction_DATE(std::vector<Function> args) {
    return std::make_shared<Function_DATE>(std::move(args));
}
Function NewFunction_DATEDIF(std::vector<Function> args) {
    return std::make_shared<Function_DATEDIF>(std::move(args));
}
Function NewFunction_DATEVALUE(std::vector<Function> args) {
    return std::make_shared<Function_DATEVALUE>(std::move(args));
}
Function NewFunction_DAY(std::vector<Function> args) {
    return std::make_shared<Function_DAY>(std::move(args));
}
Function NewFunction_DAYS(std::vector<Function> args) {
    return std::make_shared<Function_DAYS>(std::move(args));
}
Function NewFunction_DAYS360(std::vector<Function> args) {
    return std::make_shared<Function_DAYS360>(std::move(args));
}
Function NewFunction_EDATE(std::vector<Function> args) {
    return std::make_shared<Function_EDATE>(std::move(args));
}
Function NewFunction_EOMONTH(std::vector<Function> args) {
    return std::make_shared<Function_EOMONTH>(std::move(args));
}
Function NewFunction_HOUR(std::vector<Function> args) {
    return std::make_shared<Function_HOUR>(std::move(args));
}
Function NewFunction_MINUTE(std::vector<Function> args) {
    return std::make_shared<Function_MINUTE>(std::move(args));
}
Function NewFunction_MONTH(std::vector<Function> args) {
    return std::make_shared<Function_MONTH>(std::move(args));
}
Function NewFunction_NETWORKDAYS(std::vector<Function> args) {
    return std::make_shared<Function_NETWORKDAYS>(std::move(args));
}
Function NewFunction_NOW(std::vector<Function> args) {
    return std::make_shared<Function_NOW>(std::move(args));
}
Function NewFunction_SECOND(std::vector<Function> args) {
    return std::make_shared<Function_SECOND>(std::move(args));
}
Function NewFunction_TIME(std::vector<Function> args) {
    return std::make_shared<Function_TIME>(std::move(args));
}
Function NewFunction_TIMESTAMP(std::vector<Function> args) {
    return std::make_shared<Function_TIMESTAMP>(std::move(args));
}
Function NewFunction_TIMEVALUE(std::vector<Function> args) {
    return std::make_shared<Function_TIMEVALUE>(std::move(args));
}
Function NewFunction_TODAY(std::vector<Function> args) {
    return std::make_shared<Function_TODAY>(std::move(args));
}
Function NewFunction_WEEKDAY(std::vector<Function> args) {
    return std::make_shared<Function_WEEKDAY>(std::move(args));
}
Function NewFunction_WEEKNUM(std::vector<Function> args) {
    return std::make_shared<Function_WEEKNUM>(std::move(args));
}
Function NewFunction_WORKDAY(std::vector<Function> args) {
    return std::make_shared<Function_WORKDAY>(std::move(args));
}
Function NewFunction_YEAR(std::vector<Function> args) {
    return std::make_shared<Function_YEAR>(std::move(args));
}
Function NewFunction_YEARFRAC(std::vector<Function> args) {
    return std::make_shared<Function_YEARFRAC>(std::move(args));
}

}  // namespace toolgood
