package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.math.mathParser2.DEC2OCT_funContext;

public class AlgorithmEngineTest_dateTime {
 
    @Test
    public void DATEVALUE_Test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("DATEVALUE('2016-01-01')", DateTime.now());
        assertEquals(dt, new DateTime(2016, 1, 1,0,0,0,DateTimeZone.UTC));
    }
    @Test
    public void TIMEVALUE_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt = engine.TryEvaluate("TIMEVALUE('12:12:12')", DateTime.now());
        assertEquals(dt, new DateTime(2016, 1, 1, 12, 12, 12).toTimeOfDay());
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
        DateTime dt = engine.TryEvaluate("time(11,12,13)", DateTime.now().plusDays(1));
        assertEquals(dt, new DateTime(2016, 1, 1, 11, 12, 13).toTimeOfDay());
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

}