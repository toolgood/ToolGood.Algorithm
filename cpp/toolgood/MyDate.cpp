#include "MyDate.h"

#include <cstdio>
#include <cstdlib>
#include <ctime>
#include <string>
#include <vector>

namespace toolgood {

namespace {

const int64_t kSecondsPerDay = 86400;

// Howard Hinnant 的 civil 日历算法
int64_t DaysFromCivil(int y, int m, int d) {
    y -= (m <= 2);
    const int era = (y >= 0 ? y : y - 399) / 400;
    const unsigned yoe = static_cast<unsigned>(y - era * 400);
    const unsigned doy = static_cast<unsigned>((153 * (m + (m > 2 ? -3 : 9)) + 2) / 5 + d - 1);
    const unsigned doe = yoe * 365 + yoe / 4 - yoe / 100 + doy;
    return static_cast<int64_t>(era) * 146097 + static_cast<int64_t>(doe) - 719468;
}

void CivilFromDays(int64_t z, int& y, int& m, int& d) {
    z += 719468;
    const int64_t era = (z >= 0 ? z : z - 146096) / 146097;
    const unsigned doe = static_cast<unsigned>(z - era * 146097);
    const unsigned yoe = (doe - doe / 1460 + doe / 36524 - doe / 146096) / 365;
    y = static_cast<int>(yoe) + static_cast<int>(era) * 400;
    const unsigned doy = doe - (365 * yoe + yoe / 4 - yoe / 100);
    const unsigned mp = (5 * doy + 2) / 153;
    d = static_cast<int>(doy - (153 * mp + 2) / 5 + 1);
    m = static_cast<int>(mp + (mp < 10 ? 3 : -9));
    y += (m <= 2);
}

bool IsLeap(int y) { return (y % 4 == 0 && y % 100 != 0) || y % 400 == 0; }

int MonthDays(int y, int m) {
    static const int kDays[13] = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
    if (m < 1 || m > 12) return 0;
    if (m == 2 && IsLeap(y)) return 29;
    return kDays[m];
}

std::vector<std::string> SplitSpace(const std::string& s) {
    std::vector<std::string> out;
    size_t i = 0;
    while (i < s.size()) {
        while (i < s.size() && (s[i] == ' ' || s[i] == '\t')) i++;
        size_t st = i;
        while (i < s.size() && s[i] != ' ' && s[i] != '\t') i++;
        if (i > st) out.push_back(s.substr(st, i - st));
    }
    return out;
}

/// 解析全部由数字组成的串；非数字或空返回 false。
bool ParseInts(const std::string& s, std::vector<int>& out) {
    if (s.empty()) return false;
    int cur = -1;
    for (char c : s) {
        if (c >= '0' && c <= '9') {
            if (cur < 0) cur = c - '0';
            else cur = cur * 10 + (c - '0');
            if (cur > 1000000) return false;
        } else if (c == ':' || c == '-' || c == '/') {
            if (cur < 0) return false;
            out.push_back(cur);
            cur = -1;
        } else {
            return false;
        }
    }
    if (cur < 0) return false;
    out.push_back(cur);
    return true;
}

}  // namespace

int MyDate::DaysInMonth() const {
    if (Month < 1 || Month > 12) return 0;
    return MonthDays(Year, Month);
}

int64_t MyDate::ToDays() const { return DaysFromCivil(Year, Month, Day); }

int32_t MyDate::ToSecondsOfDay() const { return Hour * 3600 + Minute * 60 + Second; }

int64_t MyDate::ToTimeSpanSeconds() const {
    return static_cast<int64_t>(Day) * kSecondsPerDay + ToSecondsOfDay();
}

MyDate::MyDate(int64_t timestamp, bool useLocalTime) { *this = FromTimestamp(timestamp, useLocalTime); }

MyDate MyDate::FromTimestamp(int64_t seconds, bool useLocalTime) {
    int64_t days = seconds / kSecondsPerDay;
    int64_t rem = seconds % kSecondsPerDay;
    if (rem < 0) {
        rem += kSecondsPerDay;
        days -= 1;
    }
    if (useLocalTime) {
        std::time_t t = static_cast<std::time_t>(seconds);
        std::tm tmv{};
#if defined(_WIN32)
        localtime_s(&tmv, &t);
#else
        localtime_r(&t, &tmv);
#endif
        return MyDate(tmv.tm_year + 1900, tmv.tm_mon + 1, tmv.tm_mday, tmv.tm_hour, tmv.tm_min,
                      tmv.tm_sec);
    }
    int y = 0, m = 0, d = 0;
    CivilFromDays(days, y, m, d);
    return MyDate(y, m, d, static_cast<int>(rem / 3600), static_cast<int>((rem % 3600) / 60),
                  static_cast<int>(rem % 60));
}

int64_t MyDate::ToTimestamp(bool useLocalTime) const {
    const int y = Year > 0 ? Year : 1970;
    const int m = Month > 0 ? Month : 1;
    const int d = Day > 0 ? Day : 1;
    if (useLocalTime) {
        std::tm tmv{};
        tmv.tm_year = y - 1900;
        tmv.tm_mon = m - 1;
        tmv.tm_mday = d;
        tmv.tm_hour = Hour;
        tmv.tm_min = Minute;
        tmv.tm_sec = Second;
        tmv.tm_isdst = -1;
        return static_cast<int64_t>(std::mktime(&tmv));
    }
    return DaysFromCivil(y, m, d) * kSecondsPerDay + ToSecondsOfDay();
}

MyDate MyDate::Now(bool useLocalTime) {
    std::time_t t = std::time(nullptr);
    return FromTimestamp(static_cast<int64_t>(t), useLocalTime);
}

MyDate MyDate::Today(bool useLocalTime) {
    MyDate d = Now(useLocalTime);
    d.Hour = 0;
    d.Minute = 0;
    d.Second = 0;
    return d;
}

bool MyDate::TryParse(const std::string& txt, MyDate& out) {
    // 去首尾空白
    size_t b = 0, e = txt.size();
    while (b < e && (txt[b] == ' ' || txt[b] == '\t')) b++;
    while (e > b && (txt[e - 1] == ' ' || txt[e - 1] == '\t')) e--;
    if (b == e) return false;
    std::string t = txt.substr(b, e - b);

    std::vector<std::string> parts = SplitSpace(t);
    if (parts.empty() || parts.size() > 2) return false;

    std::vector<int> nums;
    if (parts.size() == 2) {
        // 日期部分 + 时间部分
        nums.clear();
        if (!ParseInts(parts[0], nums)) return false;
        std::vector<int> tnums;
        if (!ParseInts(parts[1], tnums)) return false;
        if (tnums.size() != 3 && tnums.size() != 2) return false;
        MyDate d;
        d.Hour = tnums[0];
        d.Minute = tnums[1];
        d.Second = tnums.size() > 2 ? tnums[2] : 0;
        if (d.Hour > 23 || d.Minute > 59 || d.Second > 59) return false;
        if (nums.size() == 3) {
            d.Year = nums[0];
            d.Month = nums[1];
            d.Day = nums[2];
            if (d.Month > 12 || d.Day > 31) return false;
        } else if (nums.size() == 1) {
            d.Day = nums[0];
        } else {
            return false;
        }
        out = d;
        return true;
    }

    // 单段：日期 或 时间
    nums.clear();
    if (!ParseInts(parts[0], nums)) return false;
    MyDate d;
    if (nums.size() == 3 && parts[0].find(':') != std::string::npos) {
        // 时分秒
        d.Hour = nums[0];
        d.Minute = nums[1];
        d.Second = nums[2];
        if (d.Hour > 23 || d.Minute > 59 || d.Second > 59) return false;
        out = d;
        return true;
    }
    if (nums.size() == 2 && parts[0].find(':') != std::string::npos) {
        d.Hour = nums[0];
        d.Minute = nums[1];
        if (d.Hour > 23 || d.Minute > 59) return false;
        out = d;
        return true;
    }
    if (nums.size() == 3) {
        d.Year = nums[0];
        d.Month = nums[1];
        d.Day = nums[2];
        if (d.Month > 12 || d.Day > 31) return false;
        out = d;
        return true;
    }
    return false;
}

std::string MyDate::ToString() const {
    char buf[64];
    if (Year > 0 && Month > 0 && Day > 0) {
        std::snprintf(buf, sizeof(buf), "%d-%02d-%02d %02d:%02d:%02d", Year, Month, Day, Hour,
                      Minute, Second);
        return buf;
    }
    if (Day > 0) {
        std::snprintf(buf, sizeof(buf), "%d %02d:%02d:%02d", Day, Hour, Minute, Second);
        return buf;
    }
    std::snprintf(buf, sizeof(buf), "%02d:%02d:%02d", Hour, Minute, Second);
    return buf;
}

std::string MyDate::ToString(const std::string& format) const {
    if (Year <= 0) return ToString();
    return FormatDateTime(*this, format);
}

std::string FormatDateTime(const MyDate& dt, const std::string& pattern) {
    std::string out;
    char buf[16];
    const size_t n = pattern.size();
    for (size_t i = 0; i < n;) {
        char c = pattern[i];
        size_t run = 1;
        while (i + run < n && pattern[i + run] == c) run++;
        switch (c) {
            case 'y':
                std::snprintf(buf, sizeof(buf), run == 2 ? "%02d" : "%04d",
                              run == 2 ? dt.Year % 100 : dt.Year);
                out += buf;
                break;
            case 'M':
                std::snprintf(buf, sizeof(buf), run >= 2 ? "%02d" : "%d", dt.Month);
                out += buf;
                break;
            case 'd':
                std::snprintf(buf, sizeof(buf), run >= 2 ? "%02d" : "%d", dt.Day);
                out += buf;
                break;
            case 'H':
                std::snprintf(buf, sizeof(buf), run >= 2 ? "%02d" : "%d", dt.Hour);
                out += buf;
                break;
            case 'h': {
                int h = dt.Hour % 12;
                if (h == 0) h = 12;
                std::snprintf(buf, sizeof(buf), run >= 2 ? "%02d" : "%d", h);
                out += buf;
                break;
            }
            case 'm':
                std::snprintf(buf, sizeof(buf), run >= 2 ? "%02d" : "%d", dt.Minute);
                out += buf;
                break;
            case 's':
                std::snprintf(buf, sizeof(buf), run >= 2 ? "%02d" : "%d", dt.Second);
                out += buf;
                break;
            default:
                out.append(run, c);
                break;
        }
        i += run;
    }
    return out;
}

int64_t MyDate::ToLong() const {
    int64_t d = 0;
    if (Year > 0) d += static_cast<int64_t>(Year) * 10000000000LL;
    if (Month > 0) d += static_cast<int64_t>(Month) * 100000000LL;
    if (Day > 0) d += static_cast<int64_t>(Day) * 1000000LL;
    d += static_cast<int64_t>(Hour) * 10000;
    d += static_cast<int64_t>(Minute) * 100;
    d += Second;
    return d;
}

int MyDate::DayOfWeek() const {
    // 1970-01-01 是周四（4）
    int64_t days = DaysFromCivil(Year, Month, Day);
    int w = static_cast<int>((days + 4) % 7);
    if (w < 0) w += 7;
    return w;
}

int MyDate::DayOfYear() const {
    int64_t d = DaysFromCivil(Year, Month, Day) - DaysFromCivil(Year, 1, 1) + 1;
    return static_cast<int>(d);
}

MyDate MyDate::AddYears(int years) const {
    if (HasDate()) {
        int y = Year + years;
        int d = Day;
        if (Month == 2 && d == 29 && !IsLeap(y)) d = 28;
        return MyDate(y, Month, d, Hour, Minute, Second);
    }
    return MyDate(Year + years, Month, Day, Hour, Minute, Second);
}

MyDate MyDate::AddMonths(int months) const {
    if (HasDate()) {
        int total = (Year * 12 + (Month - 1)) + months;
        int y = total / 12;
        int m = total % 12;
        if (m < 0) {
            m += 12;
            y -= 1;
        }
        m += 1;
        int d = Day;
        int md = MonthDays(y, m);
        if (d > md) d = md;
        return MyDate(y, m, d, Hour, Minute, Second);
    }
    return MyDate(Year, Month + months, Day, Hour, Minute, Second);
}

MyDate MyDate::AddDays(int days) const {
    if (HasDate()) {
        int d = Day + days;
        if (d >= 1 && d <= 28) return MyDate(Year, Month, d, Hour, Minute, Second);
        int64_t total = DaysFromCivil(Year, Month, Day) + days;
        int y = 0, m = 0, dd = 0;
        CivilFromDays(total, y, m, dd);
        return MyDate(y, m, dd, Hour, Minute, Second);
    }
    int64_t total = ToTimeSpanSeconds() + static_cast<int64_t>(days) * kSecondsPerDay;
    int64_t dayPart = total / kSecondsPerDay;
    int64_t rem = total % kSecondsPerDay;
    if (rem < 0) {
        rem += kSecondsPerDay;
        dayPart -= 1;
    }
    return MyDate(Year, Month, static_cast<int>(dayPart), static_cast<int>(rem / 3600),
                  static_cast<int>((rem % 3600) / 60), static_cast<int>(rem % 60));
}

MyDate MyDate::AddHours(int hours) const {
    int t = Hour + hours;
    if (t >= 0 && t < 24) return MyDate(Year, Month, Day, t, Minute, Second);
    if (HasDate()) {
        int64_t secs = DaysFromCivil(Year, Month, Day) * kSecondsPerDay + ToSecondsOfDay() +
                       static_cast<int64_t>(hours) * 3600;
        MyDate d = MyDate(secs, false);
        return d;
    }
    int64_t total = ToTimeSpanSeconds() + static_cast<int64_t>(hours) * 3600;
    int64_t dayPart = total / kSecondsPerDay;
    int64_t rem = total % kSecondsPerDay;
    if (rem < 0) {
        rem += kSecondsPerDay;
        dayPart -= 1;
    }
    return MyDate(Year, Month, static_cast<int>(dayPart), static_cast<int>(rem / 3600),
                  static_cast<int>((rem % 3600) / 60), static_cast<int>(rem % 60));
}

MyDate MyDate::AddMinutes(int minutes) const {
    int t = Minute + minutes;
    if (t >= 0 && t <= 59) return MyDate(Year, Month, Day, Hour, t, Second);
    if (HasDate()) {
        int64_t secs = DaysFromCivil(Year, Month, Day) * kSecondsPerDay + ToSecondsOfDay() +
                       static_cast<int64_t>(minutes) * 60;
        return MyDate(secs, false);
    }
    int64_t total = ToTimeSpanSeconds() + static_cast<int64_t>(minutes) * 60;
    int64_t dayPart = total / kSecondsPerDay;
    int64_t rem = total % kSecondsPerDay;
    if (rem < 0) {
        rem += kSecondsPerDay;
        dayPart -= 1;
    }
    return MyDate(Year, Month, static_cast<int>(dayPart), static_cast<int>(rem / 3600),
                  static_cast<int>((rem % 3600) / 60), static_cast<int>(rem % 60));
}

MyDate MyDate::AddSeconds(int seconds) const {
    int t = Second + seconds;
    if (t >= 0 && t <= 59) return MyDate(Year, Month, Day, Hour, Minute, t);
    if (HasDate()) {
        int64_t secs = DaysFromCivil(Year, Month, Day) * kSecondsPerDay + ToSecondsOfDay() + seconds;
        return MyDate(secs, false);
    }
    int64_t total = ToTimeSpanSeconds() + seconds;
    int64_t dayPart = total / kSecondsPerDay;
    int64_t rem = total % kSecondsPerDay;
    if (rem < 0) {
        rem += kSecondsPerDay;
        dayPart -= 1;
    }
    return MyDate(Year, Month, static_cast<int>(dayPart), static_cast<int>(rem / 3600),
                  static_cast<int>((rem % 3600) / 60), static_cast<int>(rem % 60));
}

}  // namespace toolgood
