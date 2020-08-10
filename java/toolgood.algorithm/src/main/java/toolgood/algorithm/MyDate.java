package toolgood.algorithm;

import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.util.Date;
import java.util.TimeSpan;
/// <summary>
/// Date
/// </summary>
public class MyDate {
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <param name="hour">时</param>
    /// <param name="minute">分</param>
    /// <param name="second">秒</param>
    public MyDate(int year, int month, int day, int hour, int minute, int second) {
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
    public MyDate(Date dt) {
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
    public MyDate(TimeSpan dt) {
        Day = dt.Days;
        Hour = dt.Hours;
        Minute = dt.Minutes;
        Second = dt.Seconds;
    }

    /// <summary>
    /// 年
    /// </summary>
    public Integer Year; 
    /// <summary>
    /// 月
    /// </summary>
    public Integer Month;
    /// <summary>
    /// 日
    /// </summary>
    public Integer Day;
    /// <summary>
    /// 时
    /// </summary>
    public int Hour;
    /// <summary>
    /// 分
    /// </summary>
    public int Minute;
    /// <summary>
    /// 秒
    /// </summary>
    public int Second;

    public String toString() {
        if (Year != null) {
            SimpleDateFormat fmt=new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
            Date date= fmt.parse(Year+"-"+Month+"-"+Day+" "+Hour+":"+Minute+":"+Second);
            if (Second > 0) {
                return new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(date);
            } else if (Hour > 0 || Minute > 0) {
                return new SimpleDateFormat("yyyy-MM-dd HH:mm").format(date);
            } else {
                return new SimpleDateFormat("yyyy-MM-dd").format(date);
            }
        }
        double sp= Day+ (Hour + (Minute + Second / 60.0) / 60) / 24;
        TimeSpan ts = new TimeSpan(timeSpan);

        if (Day != null) {
            return new SimpleDateFormat("d HH:mm:ss").format(ts);
        }
        if (Second == 0) {
            return new SimpleDateFormat("HH:mm").format(ts);
        }
        return new SimpleDateFormat("HH:mm:ss").format(ts);
    }

    public string ToString(string f) {
        if (Year == null) {
            return ((DateTime) this).ToString(f);
        }
        return ((DateTime) this).ToString(f);
    }

    //  public static implicit operator

    // Date(DateTime date) {
    //     return new Date(date);
    // }

    // public static implicit operator

    // DateTime(Date date)
    //     {
    //         return new DateTime(date.Year ?? 0, date.Month ?? 0, date.Day ?? 0, date.Hour, date.Minute, date.Second);
    //     }

    // public static implicit operator

    // TimeSpan(Date date)
    //     {
    //         return new TimeSpan(date.Day ?? 0, date.Hour, date.Minute, date.Second);
    //     }

    // public static implicit operator

    // Date(double days) {
    //     var dt = DateTime.MinValue.AddDays((int) days);
    //     if (dt.Year > 1900) {
    //         dt = dt.AddSeconds((days - (int) days) * 24 * 60 * 60);
    //         return new Date(dt);
    //     }
    //     var day = (int) days;
    //     days = (days - day) * 24;
    //     var hour = (int) days;
    //     days = (days - hour) * 60;
    //     var minute = (int) days * 24;
    //     days = (days - minute) * 60;
    //     var second = (int) days;
    //     return new Date(new TimeSpan(day, hour, minute, second));
    // }

    public double ToNumber(){
        if (Year > 1900) {
            LocalDate start = LocalDate.of(Year, Month-1, Day);
            LocalDate end = LocalDate.of(1900, 1, 1);
            long days = ChronoUnit.DAYS.between(start, end)+1;
            return days + (Hour + (Minute + Second / 60.0) / 60) / 24;
        }
        return Day+ (Hour + (Minute + Second / 60.0) / 60) / 24;
    }

    // public static implicit operator double(
    // Date date)
    // {
    //         if (date.Year > 1900) {
    //             var dt = new DateTime((date.Year ?? 0), (date.Month ?? 0), (date.Day ?? 0), date.Hour, date.Minute, date.Second);
    //             double days = (double)(dt - DateTime.MinValue).TotalDays;
    //             days += (date.Hour + (date.Minute + date.Second / 60.0) / 60) / 24;
    //             return days;
    //         }
    //         return (date.Day ?? 0) + (date.Hour + (date.Minute + date.Second / 60.0) / 60) / 24;
    //     }

    // public static Date operator+(
    // Date date, Date num)
    // {
    //     return (Date) ((double) date + (double) num);
    // }
    // public static Date operator-(
    // Date date, Date num)
    // {
    //     return (Date) ((double) date - (double) num);
    // }
    // public static Date operator+(
    // Date date,
    // double num)
    // {
    //     return (Date) ((double) date + (double) num);
    // }
    // public static Date operator-(
    // Date date,
    // double num)
    // {
    //     return (Date) ((double) date - (double) num);
    // }
    // public static Date operator*(
    // Date date,
    // double num)
    // {
    //     return (Date) ((double) date * (double) num);
    // }
    // public static Date operator/(
    // Date date,
    // double num)
    // {
    //     return (Date) ((double) date / (double) num);
    // }
}