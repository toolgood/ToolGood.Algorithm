package toolgood.algorithm;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;

import java.math.BigDecimal;
import java.math.MathContext;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.Date;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

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

    public MyDate(int year, int month, int day, int hour, int minute, int second) {
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
        Second = d.multiply(new BigDecimal(24)).subtract(new BigDecimal(Hour)).multiply(new BigDecimal(60)).subtract(new BigDecimal(Minute)).multiply(new BigDecimal(60)).intValue();
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

    public static MyDate parse(String txt) {
        String t = txt.trim();
        Matcher m = Pattern
                .compile("^(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d) ([01]\\d?|2[1234]):([012345]?\\d):([012345]?\\d)$")
                .matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Year = Integer.parseInt(m.group(1));
            date.Month = Integer.parseInt(m.group(2));
            date.Day = Integer.parseInt(m.group(3));
            date.Hour = Integer.parseInt(m.group(4));
            date.Minute = Integer.parseInt(m.group(5));
            date.Second = Integer.parseInt(m.group(6));
            return date;
        }
        m = Pattern.compile("(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d) ([01]\\d?|2[1234]):([012345]?\\d)").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Year = Integer.parseInt(m.group(1));
            date.Month = Integer.parseInt(m.group(2));
            date.Day = Integer.parseInt(m.group(3));
            date.Hour = Integer.parseInt(m.group(4));
            date.Minute = Integer.parseInt(m.group(5));
            return date;
        }
        m = Pattern.compile("(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d)").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Year = Integer.parseInt(m.group(1));
            date.Month = Integer.parseInt(m.group(2));
            date.Day = Integer.parseInt(m.group(3));
            return date;
        }
        m = Pattern.compile("^(\\d+) (2[1234]|[01]?\\d):([012345]?\\d):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Day = Integer.parseInt(m.group(1));
            date.Hour = Integer.parseInt(m.group(2));
            date.Minute = Integer.parseInt(m.group(3));
            date.Second = Integer.parseInt(m.group(4));
            return date;
        }
        m = Pattern.compile("^(2[1234]|[01]?\\d):([012345]?\\d):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Hour = Integer.parseInt(m.group(1));
            date.Minute = Integer.parseInt(m.group(2));
            date.Second = Integer.parseInt(m.group(3));
            return date;
        }
        m = Pattern.compile("^(2[1234]|[01]?\\d):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Hour = Integer.parseInt(m.group(1));
            date.Minute = Integer.parseInt(m.group(2));
            return date;
        }
        return null;
    }

    public static MyDate now() {
        return new MyDate(DateTime.now());
    }

    @Override
    public String toString() {
        StringBuffer stringBuffer = new StringBuffer();
        if (Year != null) {
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

            if (Second > 0 || Hour > 0 || Minute > 0) {
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
                if (Second > 0) {
                    if (Second < 10) {
                        stringBuffer.append("0");
                    }
                    stringBuffer.append(Second);
                }
            }
        } else {
            if (Day != null) {
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
            if (Second > 0) {
                if (Second < 10) {
                    stringBuffer.append("0");
                }
                stringBuffer.append(Second);
            }
        }

        return stringBuffer.toString();
    }

    public String toString(String f) {
        Date date;
        if (Year != null && Year > 1900) {
            date = new Date(Year, Month, Day, Hour, Minute, Second);
        }else if (Day != null){
            date = new Date(1900, 1, Day, Hour, Minute, Second);
        }else{
            date = new Date(1900, 1, 0, Hour, Minute, Second);
        }
        SimpleDateFormat sd = new SimpleDateFormat(f);
        return sd.format(date);
    }

    public DateTime ToDateTime() {
        return new DateTime(Year, Month, Day, Hour, Minute, Second, DateTimeZone.UTC);
    }

    public int DayOfWeek() {
        return new DateTime(Year, Month, Day, 0, 0, 0, DateTimeZone.UTC).dayOfWeek().get();
    }

    public int DayOfYear() {
        return new DateTime(Year, Month, Day, 0, 0, 0, DateTimeZone.UTC).getDayOfYear();
    }

    public MyDate AddYears(int d) {
        return new MyDate(ToDateTime().plusYears(d));
    }

    public MyDate AddMonths(int d) {
        return new MyDate(ToDateTime().plusMonths(d));
    }

    public MyDate AddDays(int d) {
        return new MyDate(ToDateTime().plusDays(d));
    }

    public MyDate AddHours(int d) {
        return new MyDate(ToDateTime().plusHours(d));
    }

    public MyDate AddMinutes(int d) {
        return new MyDate(ToDateTime().plusMinutes(d));
    }

    public MyDate AddSeconds(int d) {
        return new MyDate(ToDateTime().plusSeconds(d));
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
        return new MyDate(this.ToNumber().add(num.ToNumber()));
    }

    public MyDate SUB(MyDate num) {
        return new MyDate(this.ToNumber().subtract(num.ToNumber()));
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


}