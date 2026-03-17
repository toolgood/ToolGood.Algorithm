package toolgood.algorithm.operands;

import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public final class MyDate {
    public Integer Year;
    public Integer Month;
    public Integer Day;
    public int Hour;
    public int Minute;
    public int Second;

    private MyDate() { }

    public MyDate(Integer year, Integer month, Integer day, int hour, int minute, int second) {
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
    }

    public MyDate(Date dt) {
        Calendar cal = new GregorianCalendar();
        cal.setTime(dt);
        Year = cal.get(Calendar.YEAR);
        Month = cal.get(Calendar.MONTH) + 1;
        Day = cal.get(Calendar.DAY_OF_MONTH);
        Hour = cal.get(Calendar.HOUR_OF_DAY);
        Minute = cal.get(Calendar.MINUTE);
        Second = cal.get(Calendar.SECOND);
    }

    public static MyDate Parse(String txt) {
        String t = txt.trim();
        Matcher m = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$").matcher(t);
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
        m = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$").matcher(t);
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

        m = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Year = Integer.parseInt(m.group(1));
            date.Month = Integer.parseInt(m.group(2));
            date.Day = Integer.parseInt(m.group(3));
            date.Hour = Integer.parseInt(m.group(4));
            date.Minute = Integer.parseInt(m.group(5));
            return date;
        }
        m = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Year = Integer.parseInt(m.group(1));
            date.Month = Integer.parseInt(m.group(2));
            date.Day = Integer.parseInt(m.group(3));
            date.Hour = Integer.parseInt(m.group(4));
            date.Minute = Integer.parseInt(m.group(5));
            return date;
        }

        m = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Year = Integer.parseInt(m.group(1));
            date.Month = Integer.parseInt(m.group(2));
            date.Day = Integer.parseInt(m.group(3));
            return date;
        }
        m = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Year = Integer.parseInt(m.group(1));
            date.Month = Integer.parseInt(m.group(2));
            date.Day = Integer.parseInt(m.group(3));
            return date;
        }

        m = Pattern.compile("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Day = Integer.parseInt(m.group(1));
            date.Hour = Integer.parseInt(m.group(2));
            date.Minute = Integer.parseInt(m.group(3));
            date.Second = Integer.parseInt(m.group(4));
            return date;
        }
        m = Pattern.compile("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Day = Integer.parseInt(m.group(1));
            date.Hour = Integer.parseInt(m.group(2));
            date.Minute = Integer.parseInt(m.group(3));
            return date;
        }

        m = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Hour = Integer.parseInt(m.group(1));
            date.Minute = Integer.parseInt(m.group(2));
            date.Second = Integer.parseInt(m.group(3));
            return date;
        }
        m = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d)$").matcher(t);
        if (m.find()) {
            MyDate date = new MyDate();
            date.Hour = Integer.parseInt(m.group(1));
            date.Minute = Integer.parseInt(m.group(2));
            return date;
        }
        return null;
    }

    @Override
    public String toString() {
        StringBuilder stringBuffer = new StringBuilder(20);
        if (Year != null && Year > 0) {
            stringBuffer.append(Year);
            stringBuffer.append('-');
            stringBuffer.append(String.format("%02d", Month));
            stringBuffer.append('-');
            stringBuffer.append(String.format("%02d", Day));

            stringBuffer.append(' ');
            stringBuffer.append(String.format("%02d", Hour));
            stringBuffer.append(':');
            stringBuffer.append(String.format("%02d", Minute));
            stringBuffer.append(':');
            stringBuffer.append(String.format("%02d", Second));
        } else {
            if (Day != null && Day > 0) {
                stringBuffer.append(Day);
                stringBuffer.append(' ');
            }
            stringBuffer.append(String.format("%02d", Hour));
            stringBuffer.append(':');
            stringBuffer.append(String.format("%02d", Minute));
            stringBuffer.append(':');
            stringBuffer.append(String.format("%02d", Second));
        }
        return stringBuffer.toString();
    }

    String toString(String f) {
        if (Year == null || Year == 0) {
            return this.toString();
        }
        Calendar cal = new GregorianCalendar(Year, Month - 1, Day, Hour, Minute, Second);
        return String.format(f, cal);
    }

    public Date ToDateTime() {
        return new GregorianCalendar(Year != null ? Year : 0, Month != null ? Month - 1 : 0, Day != null ? Day : 0, Hour, Minute, Second).getTime();
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
        d += Hour * 10000;
        d += Minute * 100;
        d += Second;
        return d;
    }

    public MyDate AddYears(int year) {
        int t = (Year != null ? Year : 0) + year;
        return new MyDate(t, Month, Day, Hour, Minute, Second);
    }

    public MyDate AddMonths(int month) {
        int t = (Month != null ? Month : 0) + month;
        if (t >= 1 && t <= 12) {
            return new MyDate(Year, t, Day, Hour, Minute, Second);
        }
        Calendar cal = new GregorianCalendar(Year != null ? Year : 0, Month != null ? Month - 1 : 0, Day != null ? Day : 0, Hour, Minute, Second);
        cal.add(Calendar.MONTH, month);
        return new MyDate(cal.get(Calendar.YEAR), cal.get(Calendar.MONTH) + 1, cal.get(Calendar.DAY_OF_MONTH), cal.get(Calendar.HOUR_OF_DAY), cal.get(Calendar.MINUTE), cal.get(Calendar.SECOND));
    }

    public MyDate AddDays(int day) {
        if (Year != null && Year > 1900) {
            int d = (Day != null ? Day : 0) + day;
            if (d >= 1 && d <= 28) {
                return new MyDate(Year, Month, d, Hour, Minute, Second);
            }
            Calendar cal = new GregorianCalendar(Year, Month - 1, Day, Hour, Minute, Second);
            cal.add(Calendar.DAY_OF_MONTH, day);
            return new MyDate(cal.get(Calendar.YEAR), cal.get(Calendar.MONTH) + 1, cal.get(Calendar.DAY_OF_MONTH), cal.get(Calendar.HOUR_OF_DAY), cal.get(Calendar.MINUTE), cal.get(Calendar.SECOND));
        }
        int newDay = (Day != null ? Day : 0) + day;
        return new MyDate(null, null, newDay, Hour, Minute, Second);
    }

    public MyDate AddHours(int hour) {
        int t = Hour + hour;
        if (t >= 0 && t < 24) {
            return new MyDate(Year, Month, Day, t, Minute, Second);
        }
        if (Year != null && Year > 1900) {
            Calendar cal = new GregorianCalendar(Year, Month - 1, Day, Hour, Minute, Second);
            cal.add(Calendar.HOUR_OF_DAY, hour);
            return new MyDate(cal.get(Calendar.YEAR), cal.get(Calendar.MONTH) + 1, cal.get(Calendar.DAY_OF_MONTH), cal.get(Calendar.HOUR_OF_DAY), cal.get(Calendar.MINUTE), cal.get(Calendar.SECOND));
        }
        int newHour = Hour + hour;
        return new MyDate(null, null, Day, newHour, Minute, Second);
    }

    public MyDate AddMinutes(int minute) {
        int t = Minute + minute;
        if (t >= 0 && t <= 59) {
            return new MyDate(Year, Month, Day, Hour, t, Second);
        }
        if (Year != null && Year > 1900) {
            Calendar cal = new GregorianCalendar(Year, Month - 1, Day, Hour, Minute, Second);
            cal.add(Calendar.MINUTE, minute);
            return new MyDate(cal.get(Calendar.YEAR), cal.get(Calendar.MONTH) + 1, cal.get(Calendar.DAY_OF_MONTH), cal.get(Calendar.HOUR_OF_DAY), cal.get(Calendar.MINUTE), cal.get(Calendar.SECOND));
        }
        int newMinute = Minute + minute;
        return new MyDate(null, null, Day, Hour, newMinute, Second);
    }

    public MyDate AddSeconds(int second) {
        int t = Second + second;
        if (t >= 0 && t <= 59) {
            return new MyDate(Year, Month, Day, Hour, Minute, t);
        }
        if (Year != null && Year > 1900) {
            Calendar cal = new GregorianCalendar(Year, Month - 1, Day, Hour, Minute, Second);
            cal.add(Calendar.SECOND, second);
            return new MyDate(cal.get(Calendar.YEAR), cal.get(Calendar.MONTH) + 1, cal.get(Calendar.DAY_OF_MONTH), cal.get(Calendar.HOUR_OF_DAY), cal.get(Calendar.MINUTE), cal.get(Calendar.SECOND));
        }
        int newSecond = Second + second;
        return new MyDate(null, null, Day, Hour, Minute, newSecond);
    }
}
