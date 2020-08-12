package toolgood.algorithm;

import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.Date;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;

public class MyDate {
    private MyDate(){}
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
    public static MyDate parse(String txt) {
        String t=txt.trim();
        Matcher m= Pattern.compile("^(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d) ([01]\\d?|2[1234]):([012345]?\\d):([012345]?\\d)$").matcher(t);
        if(m.find()){
            MyDate date=new MyDate();
            date.Year=Integer.parseInt(m.group(1));
            date.Month=Integer.parseInt(m.group(2));
            date.Day=Integer.parseInt(m.group(3));
            date.Hour=Integer.parseInt(m.group(4));
            date.Minute=Integer.parseInt(m.group(5));
            date.Second=Integer.parseInt(m.group(6));
            return date;
        }
        m= Pattern.compile("(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d) ([01]\\d?|2[1234]):([012345]?\\d)").matcher(t);
        if(m.find()){
            MyDate date=new MyDate();
            date.Year=Integer.parseInt(m.group(1));
            date.Month=Integer.parseInt(m.group(2));
            date.Day=Integer.parseInt(m.group(3));
            date.Hour=Integer.parseInt(m.group(4));
            date.Minute=Integer.parseInt(m.group(5));
            return date;
        }
        m= Pattern.compile("(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d)").matcher(t);
        if(m.find()){
            MyDate date=new MyDate();
            date.Year=Integer.parseInt(m.group(1));
            date.Month=Integer.parseInt(m.group(2));
            date.Day=Integer.parseInt(m.group(3));
            return date;
        }
        m= Pattern.compile("^(\\d+) (2[1234]|[01]?\\d):([012345]?\\d):([012345]?\\d)$").matcher(t);
        if(m.find()){
            MyDate date=new MyDate();
            date.Day=Integer.parseInt(m.group(1));
            date.Hour=Integer.parseInt(m.group(2));
            date.Minute=Integer.parseInt(m.group(3));
            date.Second=Integer.parseInt(m.group(4));
            return date;
        }
        m= Pattern.compile("^(2[1234]|[01]?\\d):([012345]?\\d):([012345]?\\d)$").matcher(t);
        if(m.find()){
            MyDate date=new MyDate();
            date.Hour=Integer.parseInt(m.group(1));
            date.Minute=Integer.parseInt(m.group(2));
            date.Second=Integer.parseInt(m.group(3));
            return date;
        }
        m= Pattern.compile("^(2[1234]|[01]?\\d):([012345]?\\d)$").matcher(t);
        if(m.find()){
            MyDate date=new MyDate();
            date.Hour=Integer.parseInt(m.group(1));
            date.Minute=Integer.parseInt(m.group(2));
            return date;
        }
        return null;
    }



    // public MyDate(TimeSpan dt) {
    //     Day = dt.Days;
    //     Hour = dt.Hours;
    //     Minute = dt.Minutes;
    //     Second = dt.Seconds;
    // }

    public MyDate(double num){
        int days=(int)num;
        if(days>365){
            LocalDate start = LocalDate.of(1900, 1, 1);
            start = start.plusDays(days-1);
            Year= start.getYear();
            Month=start.getMonthValue();
            Day=start.getDayOfMonth();
        }else{
            Day=days;
        }
        double d=num-days;
        Hour=(int)(d*24);
        Minute=(int)((d*24-Hour)*60.0); 
        Second=(int)(((d*24-Hour)*60.0-Minute)*60.0); 
    }

    public Integer Year; 
    public Integer Month;
    public Integer Day;
    public int Hour;
    public int Minute;
    public int Second;

    @Override
    public String toString() {
        StringBuffer stringBuffer=new StringBuffer();
        if (Year != null) {
            stringBuffer.append(Year);
            stringBuffer.append("-");
            if(Month<10){ stringBuffer.append("0"); }
            stringBuffer.append(Month);
            stringBuffer.append("-");
            if(Day<10){ stringBuffer.append("0"); }
            stringBuffer.append(Day);

            if (Second > 0 || Hour > 0 || Minute > 0) {
                stringBuffer.append(" ");
                if(Hour<10){ stringBuffer.append("0"); }
                stringBuffer.append(Hour);
                stringBuffer.append(":");
                if(Minute<10){ stringBuffer.append("0"); }
                stringBuffer.append(Minute);
                if(Second>0){
                    if(Second<10){ stringBuffer.append("0"); }
                    stringBuffer.append(Second);
                }
            }
        }else{
            if(Day!=null){
                stringBuffer.append(Day);
                stringBuffer.append(" ");
            }
            if(Hour<10){ stringBuffer.append("0"); }
            stringBuffer.append(Hour);
            stringBuffer.append(":");
            if(Minute<10){ stringBuffer.append("0"); }
            stringBuffer.append(Minute);
            if(Second>0){
                if(Second<10){ stringBuffer.append("0"); }
                stringBuffer.append(Second);
            }
        }
      
        return stringBuffer.toString();
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
        return new MyDate( ToDateTime().plusYears(d))  ;
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
        if (Year!=null && Year > 1900) {
            LocalDate start = LocalDate.of(Year, Month, Day);
            LocalDate end = LocalDate.of(1900, 1, 1);
            long days = ChronoUnit.DAYS.between(end,start)+1;
            return days + (Hour + (Minute + Second / 60.0) / 60) / 24;
        }
        if(Day!=null){
            return Day+ (Hour + (Minute + Second / 60.0) / 60) / 24;
        }
        return (Hour + (Minute + Second / 60.0) / 60) / 24;
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