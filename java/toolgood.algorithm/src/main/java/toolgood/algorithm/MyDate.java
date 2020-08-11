package toolgood.algorithm;

import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.Date;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;

/// <summary>
/// Date
/// </summary>
public class MyDate {
    public MyDate(int year, int month, int day, int hour, int minute, int second) {
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
    }
    public MyDate(Date dt) {
        DateTime dTime=new DateTime(dt);
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

    // public MyDate(TimeSpan dt) {
    //     Day = dt.Days;
    //     Hour = dt.Hours;
    //     Minute = dt.Minutes;
    //     Second = dt.Seconds;
    // }

    public MyDate(double d){
        
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

    public String toString(String f) {
        return null;
        // if (Year == null) {
        //     return ((DateTime) this).ToString(f);
        // }
        // return ((DateTime) this).ToString(f);
    }
    public DateTime ToDateTime(){
        return new DateTime(Year,Month,Day,Hour,Minute,Second,DateTimeZone.UTC);
    }

    public int DayOfWeek(){
        return  new DateTime(Year,Month,Day,0,0,0,DateTimeZone.UTC).dayOfWeek().get();
    }
    public int DayOfYear(){
        return  new DateTime(Year,Month,Day,0,0,0,DateTimeZone.UTC).getDayOfYear();
    }
    public MyDate AddYears(int d){
        return new MyDate( ToDateTime().plusDays(d))  ;
    }
    public MyDate AddMonths(int d){
        return new MyDate( ToDateTime().plusMonths(d))  ;
    }
    public MyDate AddDays(int d){
        return new MyDate( ToDateTime().plusDays(d))  ;
    }
    public MyDate AddHours(int d){
        return new MyDate( ToDateTime().plusHours(d))  ;
    } 
    public MyDate AddMinutes(int d){
        return new MyDate( ToDateTime().plusMinutes(d))  ;
    }
    public MyDate AddSeconds(int d){
        return new MyDate( ToDateTime().plusSeconds(d))  ;
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

    public MyDate ADD(MyDate num){
        return new MyDate(this.ToNumber()+num.ToNumber());
    }
    public MyDate ADD(Double num){
        return new MyDate(this.ToNumber()+num);
    }
    public MyDate SUB(MyDate num){
        return new MyDate(this.ToNumber()-num.ToNumber());
    }
    public MyDate SUB(Double num){
        return new MyDate(this.ToNumber()-num);
    }
    public MyDate MUL(Double num){
        return new MyDate(this.ToNumber()*num);
    }
    public MyDate DIV(Double num){
        return new MyDate(this.ToNumber()/num);
    }

}