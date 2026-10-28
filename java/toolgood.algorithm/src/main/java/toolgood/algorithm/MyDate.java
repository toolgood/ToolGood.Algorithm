package toolgood.algorithm;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;

import java.math.BigDecimal;
import java.math.MathContext;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.Date;

public class MyDate {
    public Integer Year;
    public Integer Month;
    public Integer Day;
    public int Hour;
    public int Minute;
    public int Second;

    // public MyDate(TimeSpan dt) {
    // Day = dt.Days;
    // Hour = dt.Hours;
    // Minute = dt.Minutes;
    // Second = dt.Seconds;
    // }

    private MyDate() {
    }

    public MyDate(MyDate dt) {
        Year = dt.Year;
        Month = dt.Month;
        Day = dt.Day;
        Hour = dt.Hour;
        Minute = dt.Minute;
        Second = dt.Second;
    }

    public MyDate(Integer year, Integer month, Integer day, Integer hour, Integer minute, Integer second) {
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
    }

    public MyDate(Date dt) {
        DateTime dTime = new DateTime(dt);
        Year = dTime.getYear();
        Month = dTime.getMonthOfYear();
        Day = dTime.getDayOfMonth();
        Hour = dTime.getHourOfDay();
        Minute = dTime.getMinuteOfHour();
        Second = dTime.getSecondOfMinute();
    }

    public MyDate(DateTime dTime) {
        Year = dTime.getYear();
        Month = dTime.getMonthOfYear();
        Day = dTime.getDayOfMonth();
        Hour = dTime.getHourOfDay();
        Minute = dTime.getMinuteOfHour();
        Second = dTime.getSecondOfMinute();
    }

    public MyDate(BigDecimal num) {
        int days = num.intValue();
        if (days > 365) {
            LocalDate start = LocalDate.of(1900, 1, 1);
            start = start.plusDays(days - 2);
            Year = start.getYear();
            Month = start.getMonthValue();
            Day = start.getDayOfMonth();
        } else {
            Day = days;
        }
        BigDecimal d = num.subtract(new BigDecimal(days));
        Hour = d.multiply(new BigDecimal(24)).intValue();
        Minute = d.multiply(new BigDecimal(24)).subtract(new BigDecimal(Hour)).multiply(new BigDecimal(60)).intValue();
        Second = d.multiply(new BigDecimal(24)).subtract(new BigDecimal(Hour)).multiply(new BigDecimal(60))
                .subtract(new BigDecimal(Minute)).multiply(new BigDecimal(60)).intValue();
        // 防止秒数出错
        if (Second == 60) {
            Second = 0;
            Minute = Minute + 1;
            if (Minute == 60) {
                Minute = 0;
                Hour = Hour + 1;
            }
        }
    }

    public MyDate(double num) {
        int days = (int) num;
        if (days > 365) {
            LocalDate start = LocalDate.of(1900, 1, 1);
            start = start.plusDays(days - 2);
            Year = start.getYear();
            Month = start.getMonthValue();
            Day = start.getDayOfMonth();
        } else {
            Day = days;
        }
        double d = num - days;
        Hour = (int) (d * 24);
        Minute = (int) ((d * 24 - Hour) * 60.0);
        Second = (int) Math.round(((d * 24 - Hour) * 60.0 - Minute) * 60.0);
        // 防止秒数出错
        if (Second == 60) {
            Second = 0;
            Minute = Minute + 1;
            if (Minute == 60) {
                Minute = 0;
                Hour = Hour + 1;
            }
        }
    }

    /**
     * 解析日期文本，失败返回 null。支持以下形状：
     * <pre>
     *   YYYY[-]M[-]D HH:MM:SS / YYYY[/]M[/]D HH:MM:SS
     *   YYYY[-]M[-]D HH:MM    / YYYY[/]M[/]D HH:MM
     *   YYYY[-]M[-]D          / YYYY[/]M[/]D
     *   D HH:MM:SS、HH:MM:SS、HH:MM
     * </pre>
     * 原先每次调用都要 Pattern.compile 9 条正则（编译状态机的开销是匹配本身的十倍以上），
     * 改为手写扫描：年恰 4 位、两个日期分隔符必须相同、月 0-12、日 0-31、时 0-23、分/秒 0-59、
     * 除年以外各段不超过 2 位。纯数字“日”只接受带秒的 D HH:MM:SS。
     */
    public static MyDate parse(String txt) {
        String t = txt.trim();
        int len = t.length();
        if (len == 0) {
            return null;
        }
        int space = t.indexOf(' ');
        if (space >= 0) {
            int[] hms = new int[3];
            if (!parseTime(t, space + 1, len, hms)) {
                return null;
            }
            int[] ymd = new int[3];
            if (parseDate(t, 0, space, ymd)) {
                return new MyDate(ymd[0], ymd[1], ymd[2], hms[0], hms[1], hms[2] < 0 ? 0 : hms[2]);
            }
            // 纯数字“日”只接受 D HH:MM:SS（不带秒的写法不在支持范围）
            if (hms[2] < 0) {
                return null;
            }
            int day = parseNumber(t, 0, space, 9);
            if (day < 0) {
                return null;
            }
            MyDate date = new MyDate();
            date.Day = day;
            date.Hour = hms[0];
            date.Minute = hms[1];
            date.Second = hms[2];
            return date;
        }
        int[] ymd = new int[3];
        if (parseDate(t, 0, len, ymd)) {
            return new MyDate(ymd[0], ymd[1], ymd[2], 0, 0, 0);
        }
        int[] hms = new int[3];
        if (parseTime(t, 0, len, hms)) {
            MyDate date = new MyDate();
            date.Hour = hms[0];
            date.Minute = hms[1];
            date.Second = hms[2] < 0 ? 0 : hms[2];
            return date;
        }
        return null;
    }

    /**
     * 解析 [from,to) 内的 YYYY-M-D 或 YYYY/M/D，年份必须 4 位、两个分隔符必须相同。
     */
    private static boolean parseDate(String t, int from, int to, int[] ymd) {
        int first = indexOfDateSeparator(t, from, to);
        if (first != from + 4) {
            return false;
        }
        char sep = t.charAt(first);
        int second = indexOfDateSeparator(t, first + 1, to);
        if (second < 0 || t.charAt(second) != sep) {
            return false;
        }
        int year = parseNumber(t, from, first, 4);
        int month = parseNumber(t, first + 1, second, 2);
        int day = parseNumber(t, second + 1, to, 2);
        if (year < 0 || month < 0 || day < 0 || month > 12 || day > 31) {
            return false;
        }
        ymd[0] = year;
        ymd[1] = month;
        ymd[2] = day;
        return true;
    }

    private static int indexOfDateSeparator(String t, int from, int to) {
        for (int i = from; i < to; i++) {
            char c = t.charAt(i);
            if (c == '-' || c == '/') {
                return i;
            }
        }
        return -1;
    }

    /**
     * 解析 [from,to) 内的 H:M 或 H:M:S。hms[2] 为 -1 表示没有秒段。
     */
    private static boolean parseTime(String t, int from, int to, int[] hms) {
        int first = t.indexOf(':', from);
        if (first < 0 || first >= to) {
            return false;
        }
        int hour = parseNumber(t, from, first, 2);
        if (hour < 0 || hour > 23) {
            return false;
        }
        int second = t.indexOf(':', first + 1);
        if (second < 0 || second >= to) {
            int minute = parseNumber(t, first + 1, to, 2);
            if (minute < 0 || minute > 59) {
                return false;
            }
            hms[0] = hour;
            hms[1] = minute;
            hms[2] = -1;
            return true;
        }
        int minute = parseNumber(t, first + 1, second, 2);
        int sec = parseNumber(t, second + 1, to, 2);
        if (minute < 0 || minute > 59 || sec < 0 || sec > 59) {
            return false;
        }
        hms[0] = hour;
        hms[1] = minute;
        hms[2] = sec;
        return true;
    }

    /**
     * 解析 [from,to) 内的纯 ASCII 数字，长度超过 maxLen 或含非数字字符返回 -1。
     */
    private static int parseNumber(String t, int from, int to, int maxLen) {
        int len = to - from;
        if (len <= 0 || len > maxLen) {
            return -1;
        }
        int value = 0;
        for (int i = from; i < to; i++) {
            char c = t.charAt(i);
            if (c < '0' || c > '9') {
                return -1;
            }
            value = value * 10 + (c - '0');
        }
        return value;
    }

    public static MyDate now() {
        return new MyDate(DateTime.now());
    }

    @Override
    public String toString() {
        StringBuffer stringBuffer = new StringBuffer();
        if (Year != null && Year > 0) {
            stringBuffer.append(Year);
            stringBuffer.append("-");
            if (Month < 10) {
                stringBuffer.append("0");
            }
            stringBuffer.append(Month);
            stringBuffer.append("-");
            if (Day < 10) {
                stringBuffer.append("0");
            }
            stringBuffer.append(Day);

            stringBuffer.append(" ");
            if (Hour < 10) {
                stringBuffer.append("0");
            }
            stringBuffer.append(Hour);
            stringBuffer.append(":");
            if (Minute < 10) {
                stringBuffer.append("0");
            }
            stringBuffer.append(Minute);
            stringBuffer.append(":");
            if (Second < 10) {
                stringBuffer.append("0");
            }
            stringBuffer.append(Second);
        } else {
            if (Day != null && Day > 0) {
                stringBuffer.append(Day);
                stringBuffer.append(" ");
            }
            if (Hour < 10) {
                stringBuffer.append("0");
            }
            stringBuffer.append(Hour);
            stringBuffer.append(":");
            if (Minute < 10) {
                stringBuffer.append("0");
            }
            stringBuffer.append(Minute);
            stringBuffer.append(":");
            if (Second < 10) {
                stringBuffer.append("0");
            }
            stringBuffer.append(Second);
        }

        return stringBuffer.toString();
    }

    public String toString(String f) {
        if (Year == null || Year == 0) {
            return this.toString();
        }
        return this.ToDateTime().toString(f);
    }

    public DateTime ToDateTime() {
        return new DateTime(Year == null ? 0 : Year, Month == null ? 0 : Month, Day == null ? 0 : Day,
                Hour, Minute, Second, DateTimeZone.UTC);
    }

    public DateTime ToDateTime(DateTimeZone zoo) {
        return new DateTime(Year == null ? 0 : Year, Month == null ? 0 : Month, Day == null ? 0 : Day,
                Hour, Minute, Second, zoo);
    }

    public int DayOfWeek() {
        return ToDateTime().dayOfWeek().get();
    }

    public int DayOfYear() {
        return ToDateTime().getDayOfYear();
    }

    public MyDate AddYears(int d) {
        int t = (Year == null ? 0 : Year) + d;
        return new MyDate(t, Month, Day, Hour, Minute, Second);
    }

    public MyDate AddMonths(int d) {
        int t = (Month == null ? 0 : Month) + d;
        if (t >= 1 && t <= 12) {
            return new MyDate(Year, t, Day, Hour, Minute, Second);
        }
        return new MyDate(ToDateTime().plusMonths(d));
    }

    public MyDate AddDays(int d) {
        if (Year != null && Year > 1900) {
            int t = (Day == null ? 0 : Day) + d;
            if (t >= 1 && t <= 28) {
                return new MyDate(Year, Month, t, Hour, Minute, Second);
            }
            return new MyDate(ToDateTime().plusDays(d));
        }
        return addTimeSpan(d * 86400);
    }

    public MyDate AddHours(int d) {
        int t = this.Hour + d;
        if (Year != null && t >= 0 && t < 24) {
            return new MyDate(Year, Month, Day, t, Minute, Second);
        }
        if (Year != null && Year > 1900) {
            return new MyDate(ToDateTime().plusHours(d));
        }
        return addTimeSpan(d * 3600);
    }

    public MyDate AddMinutes(int d) {
        int t = this.Minute + d;
        if (Year != null && t >= 0 && t <= 59) {
            return new MyDate(Year, Month, Day, Hour, t, Second);
        }
        if (Year != null && Year > 1900) {
            return new MyDate(ToDateTime().plusMinutes(d));
        }
        return addTimeSpan(d * 60);
    }

    public MyDate AddSeconds(int d) {
        int t = this.Second + d;
        if (Year != null && t >= 0 && t <= 59) {
            return new MyDate(Year, Month, Day, Hour, Minute, t);
        }
        if (Year != null && Year > 1900) {
            return new MyDate(ToDateTime().plusSeconds(d));
        }
        return addTimeSpan(d);
    }

    private MyDate addTimeSpan(int totalSeconds) {
        int total = Hour * 3600 + Minute * 60 + Second + totalSeconds;
        int newSecond = total % 60;
        if (newSecond < 0) { newSecond += 60; }
        int totalMinutes = (total - newSecond) / 60;
        int newMinute = totalMinutes % 60;
        if (newMinute < 0) { newMinute += 60; }
        int totalHours = (totalMinutes - newMinute) / 60;
        int newHour = totalHours % 24;
        if (newHour < 0) { newHour += 24; }
        int newDay = (totalHours - newHour) / 24;
        MyDate result = new MyDate();
        result.Day = newDay;
        result.Hour = newHour;
        result.Minute = newMinute;
        result.Second = newSecond;
        return result;
    }

    public BigDecimal ToNumber() {
        BigDecimal result = new BigDecimal(Second).divide(new BigDecimal(60), MathContext.DECIMAL32);
        result = result.add(new BigDecimal(Minute)).divide(new BigDecimal(60), MathContext.DECIMAL32);
        result = result.add(new BigDecimal(Hour)).divide(new BigDecimal(24), MathContext.DECIMAL32);

        if (Year != null && Year > 1900) {
            LocalDate start = LocalDate.of(Year, Month, Day);
            LocalDate end = LocalDate.of(1900, 1, 1);
            long days = ChronoUnit.DAYS.between(end, start) + 2;
            return result.add(new BigDecimal(days));
        }
        if (Day != null) {
            return result.add(new BigDecimal(Day));
        }
        return result;
    }

    public MyDate ADD(MyDate num) {
        DateTime dt = this.ToDateTime();
        if (num.Year != null) {
            dt = dt.plusYears(num.Year);
        }
        if (num.Month != null) {
            dt = dt.plusMonths(num.Month);
        }
        if (num.Day != null) {
            dt = dt.plusDays(num.Day);
        }
        dt = dt.plusHours(num.Hour);
        dt = dt.plusMinutes(num.Minute);
        dt = dt.plusSeconds(num.Second);
        return new MyDate(dt);
    }

    public MyDate SUB(MyDate num) {
        DateTime dt = this.ToDateTime();
        if (num.Year != null) {
            dt = dt.plusYears(-num.Year);
        }
        if (num.Month != null) {
            dt = dt.plusMonths(-num.Month);
        }
        if (num.Day != null) {
            dt = dt.plusDays(-num.Day);
        }
        dt = dt.plusHours(-num.Hour);
        dt = dt.plusMinutes(-num.Minute);
        dt = dt.plusSeconds(-num.Second);
        return new MyDate(dt);
    }

    public MyDate ADD(BigDecimal num) {
        return new MyDate(this.ToNumber().add(num));
    }

    public MyDate SUB(BigDecimal num) {
        return new MyDate(this.ToNumber().subtract(num));
    }

    public MyDate MUL(BigDecimal num) {
        return new MyDate(this.ToNumber().multiply(num));
    }

    public MyDate DIV(BigDecimal num) {
        return new MyDate(this.ToNumber().divide(num, MathContext.DECIMAL32));
    }

    public long ToLong() {
        long d = 0;
        if (Year != null) {
            d += Year * 10000000000L;
        }
        if (Month != null) {
            d += Month * 100000000L;
        }
        if (Day != null) {
            d += Day * 1000000L;
        }
        d += Hour * 10000L;
        d += Minute * 100L;
        d += Second;
        return d;
    }

}