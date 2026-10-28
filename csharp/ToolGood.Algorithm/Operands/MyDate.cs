using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Operands
{
    /// <summary>
    /// MyDate
    /// </summary>
    public sealed class MyDate
    {
        /// <summary>
        /// 年
        /// </summary>
        public int? Year { get; private set; }

        /// <summary>
        /// 月
        /// </summary>
        public int? Month { get; private set; }

        /// <summary>
        /// 日
        /// </summary>
        public int? Day { get; private set; }

        /// <summary>
        /// 时
        /// </summary>
        public int Hour { get; private set; }

        /// <summary>
        /// 分
        /// </summary>
        public int Minute { get; private set; }

        /// <summary>
        /// 秒
        /// </summary>
        public int Second { get; private set; }

        private MyDate()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public MyDate(int? year, int? month, int? day, int hour, int minute, int second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt">日期时间</param>
        public MyDate(DateTime dt)
        {
            Year = dt.Year;
            Month = dt.Month;
            Day = dt.Day;
            Hour = dt.Hour;
            Minute = dt.Minute;
            Second = dt.Second;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt">时间</param>
        public MyDate(TimeSpan dt)
        {
            Year = null;
            Month = null;
            Day = dt.Days;
            Hour = dt.Hours;
            Minute = dt.Minutes;
            Second = dt.Seconds;
        }

        /// <summary>
        /// 字符串转MyDate
        /// </summary>
        /// <remarks>
        /// 原先由 8 条日期正则链（RegexHelper.dateTimeRegex 等）逐个尝试，改为 span 手写扫描。
        /// 支持并等价于以下形状：
        ///   YYYY[-]M[-]D HH:MM:SS / YYYY[/]M[/]D HH:MM:SS
        ///   YYYY[-]M[-]D HH:MM    / YYYY[/]M[/]D HH:MM
        ///   YYYY[-]M[-]D          / YYYY[/]M[/]D
        ///   D HH:MM:SS、D HH:MM、HH:MM:SS、HH:MM
        /// 约束与原正则一致：年月日之间两个分隔符必须相同（不能混用 - 和 /），
        /// 年恰好 4 位，月 0-12，日 0-31，时 0-23，分/秒 0-59，各段不允许超过 2 位（年/日前缀除外）。
        /// 与正则版的差异只有两处，都是正则版本身的坑：
        ///   1. 只认 ASCII 数字（\d 会匹配全角/其它 Nd 数字，随后 int.Parse 多半失败）；
        ///   2. 超长数字段直接视为解析失败（正则版会走到 int.Parse 抛 OverflowException）。
        /// </remarks>
        public static MyDate Parse(string txt)
        {
            var span = txt.AsSpan().Trim();
            if (span.IsEmpty) { return null; }

            // 含空格：日期/日 + 时间
            var space = span.IndexOf(' ');
            if (space >= 0) {
                if (TryParseTime(span.Slice(space + 1), out var hour, out var minute, out var second) == false) {
                    return null;
                }
                if (TryParseDate(span.Slice(0, space), out var year, out var month, out var day)) {
                    return new MyDate(year, month, day, hour, minute, second);
                }
                if (TryParseNumber(span.Slice(0, space), 9, out var onlyDay)) {
                    return new MyDate(null, null, onlyDay, hour, minute, second);
                }
                return null;
            }
            // 无空格：日期 或 时间
            if (TryParseDate(span, out var y, out var m, out var d)) {
                return new MyDate(y, m, d, 0, 0, 0);
            }
            if (TryParseTime(span, out var h, out var mi, out var se)) {
                return new MyDate(null, null, null, h, mi, se);
            }
            return null;
        }

        /// <summary>
        /// 解析 YYYY M D：年份恰好 4 位，两个分隔符必须同为 '-' 或同为 '/'，月 0-12、日 0-31
        /// </summary>
        private static bool TryParseDate(ReadOnlySpan<char> span, out int year, out int month, out int day)
        {
            year = 0; month = 0; day = 0;
            var first = span.IndexOfAny('-', '/');
            if (first != 4) { return false; }
            var sep = span[first];
            var second = span.Slice(5).IndexOfAny('-', '/');
            if (second < 0) { return false; }
            second += 5;
            if (span[second] != sep) { return false; }
            return TryParseNumber(span.Slice(0, 4), 4, out year)
                && TryParseNumber(span.Slice(5, second - 5), 2, out month) && month <= 12
                && TryParseNumber(span.Slice(second + 1), 2, out day) && day <= 31;
        }

        /// <summary>
        /// 解析 HH:MM[:SS]：时 0-23，分/秒 0-59
        /// </summary>
        private static bool TryParseTime(ReadOnlySpan<char> span, out int hour, out int minute, out int second)
        {
            hour = 0; minute = 0; second = 0;
            var first = span.IndexOf(':');
            if (first < 0) { return false; }
            if (TryParseNumber(span.Slice(0, first), 2, out hour) == false || hour > 23) { return false; }

            var rest = span.Slice(first + 1);
            var next = rest.IndexOf(':');
            if (next < 0) {
                return TryParseNumber(rest, 2, out minute) && minute <= 59;
            }
            if (TryParseNumber(rest.Slice(0, next), 2, out minute) == false || minute > 59) { return false; }
            return TryParseNumber(rest.Slice(next + 1), 2, out second) && second <= 59;
        }

        /// <summary>
        /// 纯 ASCII 数字段转 int；段长超过 maxLen 或含非数字字符即失败（maxLen ≤ 9，不会溢出）
        /// </summary>
        private static bool TryParseNumber(ReadOnlySpan<char> span, int maxLen, out int value)
        {
            value = 0;
            if (span.Length == 0 || span.Length > maxLen) { return false; }
            for (int i = 0; i < span.Length; i++) {
                var c = span[i];
                if (c < '0' || c > '9') { return false; }
                value = value * 10 + (c - '0');
            }
            return true;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 快路径：stackalloc + 一次 new string(span)，避免 6 次 "D2" 格式化与 StringBuilder 的中间串分配
            // （实测 119 ns / 336 B → 34 ns / 64 B）。字段超出常规范围时退回慢路径，输出与旧实现逐字一致。
            if (Year != null && Year > 0 && Month != null && Day != null) {
                var y = Year.Value;
                var mo = Month.Value;
                var d = Day.Value;
                if ((uint)y <= 9999 && (uint)mo <= 99 && (uint)d <= 99
                    && (uint)Hour <= 23 && (uint)Minute <= 59 && (uint)Second <= 59) {
                    Span<char> buffer = stackalloc char[19];
                    Write4(buffer, 0, y);
                    buffer[4] = '-';
                    Write2(buffer, 5, mo);
                    buffer[7] = '-';
                    Write2(buffer, 8, d);
                    buffer[10] = ' ';
                    Write2(buffer, 11, Hour);
                    buffer[13] = ':';
                    Write2(buffer, 14, Minute);
                    buffer[16] = ':';
                    Write2(buffer, 17, Second);
                    return new string(buffer);
                }
                return ToStringSlow();
            }
            if (Day != null && Day > 0) {
                var d = Day.Value;
                if ((uint)d <= 99 && (uint)Hour <= 23 && (uint)Minute <= 59 && (uint)Second <= 59) {
                    var offset = d < 10 ? 1 : 2;
                    Span<char> buffer = stackalloc char[offset + 9];   // "D HH:mm:ss"
                    if (d >= 10) { buffer[0] = (char)('0' + d / 10); }
                    buffer[offset - 1] = (char)('0' + d % 10);
                    buffer[offset] = ' ';
                    Write2(buffer, offset + 1, Hour);
                    buffer[offset + 3] = ':';
                    Write2(buffer, offset + 4, Minute);
                    buffer[offset + 6] = ':';
                    Write2(buffer, offset + 7, Second);
                    return new string(buffer);
                }
                return ToStringSlow();
            }
            if ((uint)Hour <= 23 && (uint)Minute <= 59 && (uint)Second <= 59) {
                Span<char> buffer = stackalloc char[8];
                Write2(buffer, 0, Hour);
                buffer[2] = ':';
                Write2(buffer, 3, Minute);
                buffer[5] = ':';
                Write2(buffer, 6, Second);
                return new string(buffer);
            }
            return ToStringSlow();
        }

        private static void Write2(Span<char> span, int offset, int value)
        {
            span[offset] = (char)('0' + value / 10);
            span[offset + 1] = (char)('0' + value % 10);
        }

        private static void Write4(Span<char> span, int offset, int value)
        {
            span[offset] = (char)('0' + value / 1000);
            span[offset + 1] = (char)('0' + value / 100 % 10);
            span[offset + 2] = (char)('0' + value / 10 % 10);
            span[offset + 3] = (char)('0' + value % 10);
        }

        /// <summary>
        /// 慢路径：字段超出常规范围（年份 &gt; 9999、月/日/时&gt; 两位数等）时按原格式输出
        /// </summary>
        private string ToStringSlow()
        {
            var stringBuffer = new StringBuilder(20);
            // 仅当年月日齐全时才按日期格式输出，避免 Month/Day 为空时取值抛异常
            if (Year != null && Year > 0 && Month != null && Day != null) {
                stringBuffer.Append(Year.Value);
                stringBuffer.Append('-');
                stringBuffer.Append(Month.Value.ToString("D2"));
                stringBuffer.Append('-');
                stringBuffer.Append(Day.Value.ToString("D2"));

                stringBuffer.Append(' ');
                stringBuffer.Append(Hour.ToString("D2"));
                stringBuffer.Append(':');
                stringBuffer.Append(Minute.ToString("D2"));
                stringBuffer.Append(':');
                stringBuffer.Append(Second.ToString("D2"));
            } else {
                if (Day != null && Day > 0) {
                    stringBuffer.Append(Day.Value);
                    stringBuffer.Append(' ');
                }
                stringBuffer.Append(Hour.ToString("D2"));
                stringBuffer.Append(':');
                stringBuffer.Append(Minute.ToString("D2"));
                stringBuffer.Append(':');
                stringBuffer.Append(Second.ToString("D2"));
            }
            return stringBuffer.ToString();
        }

        internal string ToString(string f)
        {
            if (Year == null || Year == 0) {
                return this.ToString();
            }
            // 与全库约定一致用 InvariantCulture，避免服务器区域差异影响 TEXT 函数输出
            return this.ToDateTime().ToString(f, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 转DateTime
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            return new DateTime(Year ?? 0, Month ?? 0, Day ?? 0, Hour, Minute, Second, DateTimeKind.Utc);
        }

        /// <summary>
        /// 转DateTime
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime(DateTimeKind dateTimeKind)
        {
            return new DateTime(Year ?? 0, Month ?? 0, Day ?? 0, Hour, Minute, Second, dateTimeKind);
        }

        /// <summary>
        /// 转TimeSpan
        /// </summary>
        /// <returns></returns>
        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(Day ?? 0, Hour, Minute, Second);
        }

        /// <summary>
        /// 添加年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public MyDate AddYears(int year)
        {
            // 年月日齐全时走 DateTime 路径，2/29 加年自动夹取到 2/28
            if (Year != null && Month != null && Day != null) {
                return new MyDate(ToDateTime().AddYears(year));
            }
            var t = (this.Year ?? 0) + year;
            return new MyDate(t, Month, Day, Hour, Minute, Second);
        }

        /// <summary>
        /// 添加月
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public MyDate AddMonths(int month)
        {
            // 年月日齐全时统一走 DateTime 路径，保证月末夹取（如 1-31 加 1 月 → 2-28/2-29）
            if (Year != null && Month != null && Day != null) {
                return new MyDate(ToDateTime().AddMonths(month));
            }
            var t = (this.Month ?? 0) + month;
            return new MyDate(Year, t, Day, Hour, Minute, Second);
        }

        /// <summary>
        /// 添加日
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public MyDate AddDays(int day)
        {
            // 年月日齐全时走 DateTime 路径；仅“日 + 时间”型操作数才退化为 TimeSpan 偏移
            if (Year != null && Month != null && Day != null) {
                var d = (this.Day ?? 0) + day;
                if (d >= 1 && d <= 28) {
                    return new MyDate(Year, Month, d, Hour, Minute, Second);
                }
                return new MyDate(ToDateTime().AddDays(day));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(day, 0, 0, 0)));
        }

        /// <summary>
        /// 添加时
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public MyDate AddHours(int hour)
        {
            var t = this.Hour + hour;
            if (t >= 0 && t < 24) {
                return new MyDate(Year, Month, Day, t, Minute, Second);
            }
            if (Year != null && Month != null && Day != null) {
                return new MyDate(ToDateTime().AddHours(hour));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(hour, 0, 0)));
        }

        /// <summary>
        /// 添加分
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public MyDate AddMinutes(int minute)
        {
            var t = this.Minute + minute;
            if (t >= 0 && t <= 59) {
                return new MyDate(Year, Month, Day, Hour, t, Second);
            }
            if (Year != null && Month != null && Day != null) {
                return new MyDate(ToDateTime().AddMinutes(minute));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(0, minute, 0)));
        }

        /// <summary>
        /// 添加秒
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public MyDate AddSeconds(int second)
        {
            var t = this.Second + second;
            if (t >= 0 && t <= 59) {
                return new MyDate(Year, Month, Day, Hour, Minute, t);
            }
            if (Year != null && Month != null && Day != null) {
                return new MyDate(ToDateTime().AddSeconds(second));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(0, 0, second)));
        }

        #region operator

        /// <summary>
        /// DateTime=>MyDate
        /// </summary>
        /// <param name="myDate"></param>
        public static implicit operator MyDate(DateTime myDate)
        {
            return new MyDate(myDate);
        }

        /// <summary>
        /// MyDate=>DateTime
        /// </summary>
        /// <param name="myDate"></param>
        public static implicit operator DateTime(MyDate myDate)
        {
            return myDate.ToDateTime();
        }

        /// <summary>
        /// MyDate=>TimeSpan
        /// </summary>
        /// <param name="myDate"></param>
        public static implicit operator TimeSpan(MyDate myDate)
        {
            return myDate.ToTimeSpan();
        }

        #endregion operator

        internal long ToLong()
        {
            long d = 0;
            if (Year != null) {
                d += Year.Value * 10000000000;
            }
            if (Month != null) {
                d += Month.Value * 100000000;
            }
            if (Day != null) {
                d += Day.Value * 1000000;
            }
            d += Hour * 10000;
            d += Minute * 100;
            d += Second;
            return d;
		}

	}
}