// 移植自 csharp/ToolGood.Algorithm.Test/Helpers/MyDateTest.cs
//
// 类型对应：
//   new DateTime(y,M,d,h,m,s) / new MyDate(dt) -> MyDate(y, M, d, h, m, s)
//   new MyDate(ts)（TimeSpan）                 -> MyDate(0, 0, day, h, m, s)
//   new MyDate(null, null, d, h, m, s)         -> MyDate(0, 0, d, h, m, s)
//   int? Year/Month/Day 为 null                -> 0 表示未设置（MyDate 字段是 int）
//   MyDate.Parse(s) 失败返回 null              -> MyDate::TryParse 返回 false
//   myDate.ToDateTime() / DateTimeKind          -> C++ 无对应，直接取 MyDate 字段
//   myDate.ToTimeSpan()                        -> ToTimeSpanSeconds() 再拆成日/时/分/秒
#include "cpptest.h"

#include <cstdint>
#include <string>

using namespace toolgood;

namespace {

bool Contains(const std::string& s, const std::string& part) {
    return s.find(part) != std::string::npos;
}

// C# TimeSpan 的 Days/Hours/Minutes/Seconds 属性（对应 ToTimeSpanSeconds 的拆分）
int TsDays(int64_t secs) { return static_cast<int>(secs / 86400); }
int TsHours(int64_t secs) { return static_cast<int>((secs % 86400) / 3600); }
int TsMinutes(int64_t secs) { return static_cast<int>((secs % 3600) / 60); }
int TsSeconds(int64_t secs) { return static_cast<int>(secs % 60); }

// ---------------- 构造函数 ----------------

TEST(MyDate, Constructor_DateTime_Test) {
    MyDate dt(2024, 6, 15, 10, 30, 45);
    MyDate myDate = dt;

    Assert::AreEqual(2024, myDate.Year);
    Assert::AreEqual(6, myDate.Month);
    Assert::AreEqual(15, myDate.Day);
    Assert::AreEqual(10, myDate.Hour);
    Assert::AreEqual(30, myDate.Minute);
    Assert::AreEqual(45, myDate.Second);
}

TEST(MyDate, Constructor_TimeSpan_Test) {
    // new TimeSpan(1, 10, 30, 45) = 1 天 10 时 30 分 45 秒；年月为 null -> 0
    MyDate myDate(0, 0, 1, 10, 30, 45);

    Assert::AreEqual(0, myDate.Year);
    Assert::AreEqual(0, myDate.Month);
    Assert::AreEqual(1, myDate.Day);
    Assert::AreEqual(10, myDate.Hour);
    Assert::AreEqual(30, myDate.Minute);
    Assert::AreEqual(45, myDate.Second);
}

TEST(MyDate, Constructor_Params_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);

    Assert::AreEqual(2024, myDate.Year);
    Assert::AreEqual(6, myDate.Month);
    Assert::AreEqual(15, myDate.Day);
    Assert::AreEqual(10, myDate.Hour);
    Assert::AreEqual(30, myDate.Minute);
    Assert::AreEqual(45, myDate.Second);
}

// ---------------- Parse ----------------

TEST(MyDate, Parse_DateTime_Test) {
    MyDate myDate = MyDate::Parse("2024-06-15 10:30:45");
    Assert::AreEqual(2024, myDate.Year);
    Assert::AreEqual(6, myDate.Month);
    Assert::AreEqual(15, myDate.Day);
    Assert::AreEqual(10, myDate.Hour);
    Assert::AreEqual(30, myDate.Minute);
    Assert::AreEqual(45, myDate.Second);
}

TEST(MyDate, Parse_DateTime_Slash_Test) {
    MyDate myDate = MyDate::Parse("2024/06/15 10:30:45");
    Assert::AreEqual(2024, myDate.Year);
    Assert::AreEqual(6, myDate.Month);
    Assert::AreEqual(15, myDate.Day);
    Assert::AreEqual(10, myDate.Hour);
    Assert::AreEqual(30, myDate.Minute);
    Assert::AreEqual(45, myDate.Second);
}

TEST(MyDate, Parse_Date_Test) {
    MyDate myDate = MyDate::Parse("2024-06-15");
    Assert::AreEqual(2024, myDate.Year);
    Assert::AreEqual(6, myDate.Month);
    Assert::AreEqual(15, myDate.Day);
    Assert::AreEqual(0, myDate.Hour);
    Assert::AreEqual(0, myDate.Minute);
    Assert::AreEqual(0, myDate.Second);
}

TEST(MyDate, Parse_Time_Test) {
    MyDate myDate = MyDate::Parse("10:30:45");
    Assert::AreEqual(0, myDate.Year);
    Assert::AreEqual(0, myDate.Month);
    Assert::AreEqual(0, myDate.Day);
    Assert::AreEqual(10, myDate.Hour);
    Assert::AreEqual(30, myDate.Minute);
    Assert::AreEqual(45, myDate.Second);
}

TEST(MyDate, Parse_Time_NoSeconds_Test) {
    MyDate myDate = MyDate::Parse("10:30");
    Assert::AreEqual(0, myDate.Year);
    Assert::AreEqual(0, myDate.Month);
    Assert::AreEqual(0, myDate.Day);
    Assert::AreEqual(10, myDate.Hour);
    Assert::AreEqual(30, myDate.Minute);
    Assert::AreEqual(0, myDate.Second);
}

TEST(MyDate, Parse_Invalid_Test) {
    // C# 的 MyDate.Parse 失败返回 null；C++ 用 TryParse 的 false 表达同一语义
    MyDate myDate;
    Assert::IsFalse(MyDate::TryParse("invalid date", myDate));
}

// ---------------- ToDateTime ----------------
// C++ 的 MyDate 没有 ToDateTime()（也没有 DateTimeKind），
// 日期时间字段原样保留，故直接断言 MyDate 的字段。

TEST(MyDate, ToDateTime_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate dt = myDate;

    Assert::AreEqual(2024, dt.Year);
    Assert::AreEqual(6, dt.Month);
    Assert::AreEqual(15, dt.Day);
    Assert::AreEqual(10, dt.Hour);
    Assert::AreEqual(30, dt.Minute);
    Assert::AreEqual(45, dt.Second);
    // C# 断言 dt.Kind == DateTimeKind.Utc：C++ 无 Kind 概念，未翻译
}

TEST(MyDate, ToDateTime_Kind_Test) {
    // C# 断言 ToDateTime(DateTimeKind.Local).Kind == Local；
    // C++ 的 MyDate 不携带 Kind，仅保留构造结果的字段断言。
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate dt = myDate;

    Assert::AreEqual(2024, dt.Year);
    Assert::AreEqual(6, dt.Month);
    Assert::AreEqual(15, dt.Day);
}

// ---------------- ToTimeSpan ----------------

TEST(MyDate, ToTimeSpan_Test) {
    MyDate myDate(0, 0, 1, 10, 30, 45);
    int64_t ts = myDate.ToTimeSpanSeconds();

    Assert::AreEqual(1, TsDays(ts));
    Assert::AreEqual(10, TsHours(ts));
    Assert::AreEqual(30, TsMinutes(ts));
    Assert::AreEqual(45, TsSeconds(ts));
}

// ---------------- AddYears ----------------

TEST(MyDate, AddYears_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddYears(1);

    Assert::AreEqual(2025, result.Year);
    Assert::AreEqual(6, result.Month);
    Assert::AreEqual(15, result.Day);
}

TEST(MyDate, AddYears_Negative_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddYears(-1);

    Assert::AreEqual(2023, result.Year);
}

// ---------------- AddMonths ----------------

TEST(MyDate, AddMonths_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddMonths(1);

    Assert::AreEqual(7, result.Month);
}

TEST(MyDate, AddMonths_Overflow_Test) {
    MyDate myDate(2024, 11, 15, 10, 30, 45);
    MyDate result = myDate.AddMonths(2);

    Assert::AreEqual(2025, result.Year);
    Assert::AreEqual(1, result.Month);
}

// ---------------- AddDays ----------------

TEST(MyDate, AddDays_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddDays(5);

    Assert::AreEqual(20, result.Day);
}

TEST(MyDate, AddDays_Negative_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddDays(-5);

    Assert::AreEqual(10, result.Day);
}

// ---------------- AddHours ----------------

TEST(MyDate, AddHours_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddHours(5);

    Assert::AreEqual(15, result.Hour);
}

TEST(MyDate, AddHours_NoOverflow_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddHours(2);

    Assert::AreEqual(12, result.Hour);
}

// ---------------- AddMinutes ----------------

TEST(MyDate, AddMinutes_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddMinutes(15);

    Assert::AreEqual(45, result.Minute);
}

TEST(MyDate, AddMinutes_NoOverflow_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddMinutes(10);

    Assert::AreEqual(40, result.Minute);
}

// ---------------- AddSeconds ----------------

TEST(MyDate, AddSeconds_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddSeconds(10);

    Assert::AreEqual(55, result.Second);
}

TEST(MyDate, AddSeconds_NoOverflow_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate result = myDate.AddSeconds(5);

    Assert::AreEqual(50, result.Second);
}

// ---------------- ToString ----------------

TEST(MyDate, ToString_DateTime_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    std::string str = myDate.ToString();

    Assert::IsTrue(Contains(str, "2024"));
    Assert::IsTrue(Contains(str, "06"));
    Assert::IsTrue(Contains(str, "15"));
}

TEST(MyDate, ToString_TimeOnly_Test) {
    MyDate myDate(0, 0, 0, 10, 30, 45);
    std::string str = myDate.ToString();

    Assert::IsTrue(Contains(str, "10:30:45"));
}

// ---------------- 隐式转换 ----------------
// C++ 没有隐式转换（DateTime/TimeSpan 类型本身也不存在），
// 直接以 MyDate 构造表达相同的值语义。

TEST(MyDate, Implicit_DateTime_To_MyDate_Test) {
    MyDate dt(2024, 6, 15, 10, 30, 45);
    MyDate myDate = dt;

    Assert::AreEqual(2024, myDate.Year);
    Assert::AreEqual(6, myDate.Month);
    Assert::AreEqual(15, myDate.Day);
}

TEST(MyDate, Implicit_MyDate_To_DateTime_Test) {
    MyDate myDate(2024, 6, 15, 10, 30, 45);
    MyDate dt = myDate;

    Assert::AreEqual(2024, dt.Year);
    Assert::AreEqual(6, dt.Month);
    Assert::AreEqual(15, dt.Day);
}

TEST(MyDate, Implicit_MyDate_To_TimeSpan_Test) {
    MyDate myDate(0, 0, 1, 10, 30, 45);
    int64_t ts = myDate.ToTimeSpanSeconds();

    Assert::AreEqual(1, TsDays(ts));
    Assert::AreEqual(10, TsHours(ts));
    Assert::AreEqual(30, TsMinutes(ts));
    Assert::AreEqual(45, TsSeconds(ts));
}

}  // namespace
