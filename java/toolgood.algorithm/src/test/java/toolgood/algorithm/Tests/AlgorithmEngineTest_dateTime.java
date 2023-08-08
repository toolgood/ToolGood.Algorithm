package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.MyDate;

public class AlgorithmEngineTest_dateTime {
 
    @Test
    public void DATEVALUE_Test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("DATEVALUE('2016-01-01')", DateTime.now());
        assertEquals(dt, new DateTime(2016, 1, 1,0,0,0,DateTimeZone.UTC));

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", DateTime.now());
        assertEquals(dt, new DateTime(2023, 8, 5, 11, 28, 19,DateTimeZone.UTC));
        assertEquals(dt.toDateTime(DateTimeZone.getDefault()), new DateTime(2023, 8, 5, 19, 28, 19,DateTimeZone.getDefault()));

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", DateTime.now());
        assertEquals(dt, new DateTime(2023, 8, 5, 11, 28, 19,DateTimeZone.UTC));
        assertEquals(dt.toDateTime(DateTimeZone.getDefault()), new DateTime(2023, 8, 5, 19, 28, 19,DateTimeZone.getDefault()));

        engine.UseLocalTime=true;
        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", DateTime.now());
        assertEquals(dt, new DateTime(2023, 8, 5, 19, 28, 19,DateTimeZone.getDefault()));

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", DateTime.now());
        assertEquals(dt, new DateTime(2023, 8, 5, 19, 28, 19,DateTimeZone.getDefault()));

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899000')", DateTime.now());
        assertEquals(dt, new DateTime(2023, 8, 5, 19, 28, 19,DateTimeZone.getDefault()));

        // chinese time
        dt = engine.TryEvaluate("DATEVALUE('1691234899')", DateTime.now());
        assertEquals(dt, new DateTime(2023, 8, 5, 19, 28, 19,DateTimeZone.getDefault()));
    }

    @Test
    public void TIMESTAMP_Test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseLocalTime = true;

        // chinese time
        long dt = engine.TryEvaluate("TIMESTAMP('2016-01-01')", 0L);
        assertEquals(dt, 1451577600000L);

        dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',0)", 0L);
        assertEquals(dt, 1451577600000L);

        dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',1)", 0L);
        assertEquals(dt, 1451577600L);
    }

    @Test
    public void TIMEVALUE_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate("TIMEVALUE('11:12:13')", MyDate.now());
        assertEquals(dt.Hour,11);
        assertEquals(dt.Minute,12);
        assertEquals(dt.Second,13);
    }
    @Test
    public void DATE_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("DATE(2016,1,1)", DateTime.now());
        assertEquals(dt, new DateTime(2016, 1, 1,0,0,0,DateTimeZone.UTC));
    }
    @Test
    public void time_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate("time(11,12,13)", MyDate.now());
        assertEquals(dt.Hour,11);
        assertEquals(dt.Minute,12);
        assertEquals(dt.Second,13);
    }
    @Test
    public void Now_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("now()", DateTime.now().plusDays(1));
        DateTime dt2=DateTime.now();
        assertEquals(dt, new DateTime(dt2.getYear(),dt2.getMonthOfYear(),dt2.getDayOfMonth()
            ,dt2.getHourOfDay(),dt2.getMinuteOfHour(),dt2.getSecondOfMinute(),DateTimeZone.UTC));
    }
    @Test
    public void Today_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("Today()", DateTime.now());
        DateTime dt2=DateTime.now();
        assertEquals(dt, new DateTime(dt2.getYear(),dt2.getMonthOfYear(),dt2.getDayOfMonth(),0,0,0,DateTimeZone.UTC));
    }
    @Test
    public void Year_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("year(now())", 0);
        assertEquals(dt, DateTime.now().getYear());
    }
    @Test
    public void Month_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Month(now())", 0);
        assertEquals(dt, DateTime.now().getMonthOfYear());
    }
    @Test
    public void Day_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Day(now())", 0);
        assertEquals(dt, DateTime.now().getDayOfMonth());
    }
    @Test
    public void Hour_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Hour(now())", 0);
        assertEquals(dt, DateTime.now().getHourOfDay());
    }
    @Test
    public void Minute_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Minute(now())", 0);
        assertEquals(dt, DateTime.now().getMinuteOfHour());
    }
    @Test
    public void Second_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Second(now())", 0);
        assertEquals(dt, DateTime.now().getSecondOfMinute());
    }
    @Test
    public void WEEKDAY_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("WEEKDAY(date(2017,2,18),1)", 0);
        assertEquals(dt, 7);

        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7))", 0);
        assertEquals(dt, 7);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),1)", 0);
        assertEquals(dt, 7);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),2)", 0);
        assertEquals(dt, 6);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),3)", 0);
        assertEquals(dt, 5);

        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),1)", 0);
        assertEquals(dt, 1);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),2)", 0);
        assertEquals(dt, 7);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),3)", 0);
        assertEquals(dt, 6);

        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),1)", 0);
        assertEquals(dt, 2);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),2)", 0);
        assertEquals(dt, 1);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),3)", 0);
        assertEquals(dt, 0);

    }

    @Test
    public void DATEDIF_Test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0);
        assertEquals(dt, 41);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','m')", 0);
        assertEquals(dt, 503);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','d')", 0);
        assertEquals(dt, 15318);


        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','yd')", 0);
        assertEquals(dt, 342);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','ym')", 0);
        assertEquals(dt, 11);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','md')", 0);
        assertEquals(dt, 8);
    }
    @Test
    public void DAYS360_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("DAYS360('1975-1-30','2017-1-7')", 0);
        assertEquals(dt, 15097);
    }
    @Test
    public void EDATE_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("EDATE(\"2012-1-31\",32)", DateTime.now());
        assertEquals(dt,new DateTime(2014,9,30,0,0,0,DateTimeZone.UTC));
    }
    @Test
    public void EOMONTH_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("EOMONTH(\"2012-2-1\",32)", DateTime.now());
        assertEquals(dt, new DateTime(2014,10,31,0,0,0,DateTimeZone.UTC));
    }

    @Test
    public void NETWORKDAYS_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("NETWORKDAYS(\"2012-1-1\",\"2013-1-1\")", 0);
        assertEquals(dt, 262);
    }
    @Test
    public void WORKDAY_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("WORKDAY(\"2012-1-2\",145)", DateTime.now());
        assertEquals(dt,new DateTime(2012,07,23,0,0,0,DateTimeZone.UTC));
    }

    @Test
    public void WEEKNUM_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("WEEKNUM(\"2016-1-3\")", 0);
        assertEquals(dt, 2);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-2\")", 0);
        assertEquals(dt, 1);

        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-4\",2)", 0);
        assertEquals(dt, 2);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-3\",2)", 0);
        assertEquals(dt, 1);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-2\",2)", 0);
        assertEquals(dt, 1);
    }
    @Test
    public void ADD_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int   dt = engine.TryEvaluate("'2000-02-01'.addYears(1).year()", 0);
        assertEquals(dt, 2001);

        dt = engine.TryEvaluate("'2000-02-01'.AddMonths(1).Month()", 0);
        assertEquals(dt, 3);

        dt = engine.TryEvaluate("'2000-02-01'.AddDays(1).Day()", 0);
        assertEquals(dt, 2);

        dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddHours(1).Hour()", 0);
        assertEquals(dt, 13);

        dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddMinutes(1).Minute()", 0);
        assertEquals(dt, 6);

        dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 7);
    }
}