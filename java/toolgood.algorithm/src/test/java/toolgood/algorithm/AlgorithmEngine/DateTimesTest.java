package toolgood.algorithm.AlgorithmEngine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.MyDate;

public class DateTimesTest {

    @Test
    public void DATEVALUE_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("DATEVALUE('2016-01-01')", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2016, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(1, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATEVALUE（'2016-01-01'）", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2016, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(1, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATEVALUE('2016/01/01')", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2016, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(1, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATEVALUE('1691234899000',0)", MyDate.now());
        assertEquals(2023, (int)dt.Year);
        assertEquals(8, (int)dt.Month);
        assertEquals(5, (int)dt.Day);
        assertEquals(19, (int)dt.Hour);
        assertEquals(28, (int)dt.Minute);
        assertEquals(19, (int)dt.Second);

        dt = engine.TryEvaluate_MyDate("DATEVALUE('1691234899',0)", MyDate.now());
        assertEquals(2023, (int)dt.Year);
        assertEquals(8, (int)dt.Month);
        assertEquals(5, (int)dt.Day);
        assertEquals(19, (int)dt.Hour);
        assertEquals(28, (int)dt.Minute);
        assertEquals(19, (int)dt.Second);

        engine.UseLocalTime = true;
        dt = engine.TryEvaluate_MyDate("DATEVALUE('1691234899000',0)", MyDate.now());
        assertEquals(2023, (int)dt.Year);
        assertEquals(8, (int)dt.Month);
        assertEquals(5, (int)dt.Day);
        assertEquals(19, (int)dt.Hour);
        assertEquals(28, (int)dt.Minute);
        assertEquals(19, (int)dt.Second);

        dt = engine.TryEvaluate_MyDate("DATEVALUE('1691234899',0)", MyDate.now());
        assertEquals(2023, (int)dt.Year);
        assertEquals(8, (int)dt.Month);
        assertEquals(5, (int)dt.Day);
        assertEquals(19, (int)dt.Hour);
        assertEquals(28, (int)dt.Minute);
        assertEquals(19, (int)dt.Second);

        dt = engine.TryEvaluate_MyDate("DATEVALUE('1691234899000')", MyDate.now());
        assertEquals(2023, (int)dt.Year);
        assertEquals(8, (int)dt.Month);
        assertEquals(5, (int)dt.Day);
        assertEquals(19, (int)dt.Hour);
        assertEquals(28, (int)dt.Minute);
        assertEquals(19, (int)dt.Second);

        dt = engine.TryEvaluate_MyDate("DATEVALUE('1691234899')", MyDate.now());
        assertEquals(2023, (int)dt.Year);
        assertEquals(8, (int)dt.Month);
        assertEquals(5, (int)dt.Day);
        assertEquals(19, (int)dt.Hour);
        assertEquals(28, (int)dt.Minute);
        assertEquals(19, (int)dt.Second);
    }

    @Test
    public void TIMESTAMP_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseLocalTime = true;

        long dt = engine.TryEvaluate("TIMESTAMP('2016-01-01')", 0L);
        assertEquals(1451577600000L, dt);

        dt = engine.TryEvaluate("TIMESTAMP('2016/01/01')", 0L);
        assertEquals(1451577600000L, dt);

        dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',0)", 0L);
        assertEquals(1451577600000L, dt);

        dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',1)", 0L);
        assertEquals(1451577600L, dt);
    }

    @Test
    public void TIMEVALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("TIMEVALUE('12:12:12')", new MyDate(0, 0, 0, 0, 0, 0));
        assertEquals(12, (int)dt.Hour);
        assertEquals(12, (int)dt.Minute);
        assertEquals(12, (int)dt.Second);
    }

    @Test
    public void DATE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("DATE(2016,1,1)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2016, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(1, (int)dt.Day);
    }

    @Test
    public void time_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("time(11,12,13)", new MyDate(0, 0, 0, 0, 0, 0));
        assertEquals(11, (int)dt.Hour);
        assertEquals(12, (int)dt.Minute);
        assertEquals(13, (int)dt.Second);
    }

    @Test
    public void Now_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("now()", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(MyDate.now().toString(), dt.toString());
    }

    @Test
    public void Today_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("Today()", new MyDate(1900, 1, 1, 0, 0, 0));
        MyDate today = MyDate.now();
        assertEquals(today.Year, dt.Year);
        assertEquals(today.Month, dt.Month);
        assertEquals(today.Day, dt.Day);
    }

    @Test
    public void Year_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("year(now())", 0);
        assertEquals((int)MyDate.now().Year, dt);
    }

    @Test
    public void Month_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Month(now())", 0);
        assertEquals((int)MyDate.now().Month, dt);
    }

    @Test
    public void Day_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Day(now())", 0);
        assertEquals((int)MyDate.now().Day, dt);
    }

    @Test
    public void Hour_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Hour(now())", 0);
        assertEquals(MyDate.now().Hour, dt);
    }

    @Test
    public void Minute_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Minute(now())", 0);
        assertEquals(MyDate.now().Minute, dt);
    }

    @Test
    public void Second_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("Second(now())", 0);
        assertEquals(MyDate.now().Second, dt);
    }

    @Test
    public void WEEKDAY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7))", 0);
        assertEquals(7, dt);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),1)", 0);
        assertEquals(7, dt);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),2)", 0);
        assertEquals(6, dt);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),3)", 0);
        assertEquals(5, dt);

        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),1)", 0);
        assertEquals(1, dt);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),2)", 0);
        assertEquals(7, dt);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),3)", 0);
        assertEquals(6, dt);

        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),1)", 0);
        assertEquals(2, dt);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),2)", 0);
        assertEquals(1, dt);
        dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),3)", 0);
        assertEquals(0, dt);
    }

    @Test
    public void WEEKDAY_ExcelCompatible_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();

        // 2017年1月1日是周日，1月7日是周六，1月2日是周一
        // type 1: 周日=1, 周六=7
        // type 2: 周一=1, 周日=7
        // type 3: 周一=0, 周日=6

        // 测试所有 type (1-3, 11-17)
        // 2017-1-2 (周一)
        assertEquals(2, engine.TryEvaluate("WEEKDAY(date(2017,1,2),1)", 0));  // type 1: 周一=2
        assertEquals(1, engine.TryEvaluate("WEEKDAY(date(2017,1,2),2)", 0));  // type 2: 周一=1
        assertEquals(0, engine.TryEvaluate("WEEKDAY(date(2017,1,2),3)", 0));  // type 3: 周一=0
        assertEquals(1, engine.TryEvaluate("WEEKDAY(date(2017,1,2),11)", 0)); // type 11: 周一=1
        assertEquals(7, engine.TryEvaluate("WEEKDAY(date(2017,1,2),12)", 0)); // type 12: 周二=1, 周一=7
        assertEquals(6, engine.TryEvaluate("WEEKDAY(date(2017,1,2),13)", 0)); // type 13: 周三=1, 周一=6
        assertEquals(5, engine.TryEvaluate("WEEKDAY(date(2017,1,2),14)", 0)); // type 14: 周四=1, 周一=5
        assertEquals(4, engine.TryEvaluate("WEEKDAY(date(2017,1,2),15)", 0)); // type 15: 周五=1, 周一=4
        assertEquals(3, engine.TryEvaluate("WEEKDAY(date(2017,1,2),16)", 0)); // type 16: 周六=1, 周一=3
        assertEquals(2, engine.TryEvaluate("WEEKDAY(date(2017,1,2),17)", 0)); // type 17: 周日=1, 周一=2

        // 2017-1-7 (周六)
        assertEquals(7, engine.TryEvaluate("WEEKDAY(date(2017,1,7),1)", 0));  // type 1: 周六=7
        assertEquals(6, engine.TryEvaluate("WEEKDAY(date(2017,1,7),2)", 0));  // type 2: 周六=6
        assertEquals(5, engine.TryEvaluate("WEEKDAY(date(2017,1,7),3)", 0));  // type 3: 周六=5
        assertEquals(6, engine.TryEvaluate("WEEKDAY(date(2017,1,7),11)", 0)); // type 11: 周六=6
        assertEquals(5, engine.TryEvaluate("WEEKDAY(date(2017,1,7),12)", 0)); // type 12: 周六=5
        assertEquals(4, engine.TryEvaluate("WEEKDAY(date(2017,1,7),13)", 0)); // type 13: 周六=4
        assertEquals(3, engine.TryEvaluate("WEEKDAY(date(2017,1,7),14)", 0)); // type 14: 周六=3
        assertEquals(2, engine.TryEvaluate("WEEKDAY(date(2017,1,7),15)", 0)); // type 15: 周六=2
        assertEquals(1, engine.TryEvaluate("WEEKDAY(date(2017,1,7),16)", 0)); // type 16: 周六=1
        assertEquals(7, engine.TryEvaluate("WEEKDAY(date(2017,1,7),17)", 0)); // type 17: 周六=7

        // 2017-1-8 (周日)
        assertEquals(1, engine.TryEvaluate("WEEKDAY(date(2017,1,8),1)", 0));  // type 1: 周日=1
        assertEquals(7, engine.TryEvaluate("WEEKDAY(date(2017,1,8),2)", 0));  // type 2: 周日=7
        assertEquals(6, engine.TryEvaluate("WEEKDAY(date(2017,1,8),3)", 0));  // type 3: 周日=6
        assertEquals(7, engine.TryEvaluate("WEEKDAY(date(2017,1,8),11)", 0)); // type 11: 周日=7
        assertEquals(6, engine.TryEvaluate("WEEKDAY(date(2017,1,8),12)", 0)); // type 12: 周日=6
        assertEquals(5, engine.TryEvaluate("WEEKDAY(date(2017,1,8),13)", 0)); // type 13: 周日=5
        assertEquals(4, engine.TryEvaluate("WEEKDAY(date(2017,1,8),14)", 0)); // type 14: 周日=4
        assertEquals(3, engine.TryEvaluate("WEEKDAY(date(2017,1,8),15)", 0)); // type 15: 周日=3
        assertEquals(2, engine.TryEvaluate("WEEKDAY(date(2017,1,8),16)", 0)); // type 16: 周日=2
        assertEquals(1, engine.TryEvaluate("WEEKDAY(date(2017,1,8),17)", 0)); // type 17: 周日=1
    }

    @Test
    public void DATEDIF_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0);
        assertEquals(41, dt);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','m')", 0);
        assertEquals(503, dt);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','d')", 0);
        assertEquals(15318, dt);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','yd')", 0);
        assertEquals(342, dt);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','ym')", 0);
        assertEquals(11, dt);

        dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','md')", 0);
        assertEquals(8, dt);
    }

    @Test
    public void DAYS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("DAYS('2017-1-7','1975-1-30')", 0);
        assertEquals(15318, dt);

        dt = engine.TryEvaluate("DAYS('2016-3-1','2016-2-28')", 0);
        assertEquals(2, dt);

        dt = engine.TryEvaluate("DAYS('2016-2-28','2016-3-1')", 0);
        assertEquals(-2, dt);
    }

    @Test
    public void DAYS360_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("DAYS360('1975-1-30','2017-1-7')", 0);
        assertEquals(15097, dt);
    }

    @Test
    public void EDATE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("EDATE(\"2012-1-31\",32)", new MyDate(1900, 1, 1, 0, 0, 0));
        MyDate expected = MyDate.parse("2014-09-30");
        assertEquals(expected.Year, dt.Year);
        assertEquals(expected.Month, dt.Month);
        assertEquals(expected.Day, dt.Day);
    }

    @Test
    public void EOMONTH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("EOMONTH(\"2012-2-1\",32)", new MyDate(1900, 1, 1, 0, 0, 0));
        MyDate expected = MyDate.parse("2014-10-31");
        assertEquals(expected.Year, dt.Year);
        assertEquals(expected.Month, dt.Month);
        assertEquals(expected.Day, dt.Day);
    }

    @Test
    public void NETWORKDAYS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("NETWORKDAYS(\"2012-1-1\",\"2013-1-1\")", 0);
        assertEquals(262, dt);
    }

    @Test
    public void WORKDAY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("WORKDAY(\"2012-1-2\",145)", new MyDate(1900, 1, 1, 0, 0, 0));
        MyDate expected = MyDate.parse("2012-07-23");
        assertEquals(expected.Year, dt.Year);
        assertEquals(expected.Month, dt.Month);
        assertEquals(expected.Day, dt.Day);
    }

    @Test
    public void WORKDAY_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("WORKDAY(\"2024-01-15\", 5)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(22, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("WORKDAY(\"2024-01-15\", -5)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(8, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("WORKDAY(\"2024-01-15\", 0)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(15, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("WORKDAY(\"2024-01-12\", 1)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(15, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("WORKDAY(\"2024-01-15\", -1)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(12, (int)dt.Day);
    }

    @Test
    public void DATE_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("DATE(2024, 1, 15)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(15, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2024, 13, 1)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2025, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(1, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2024, 1, 32)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(2, (int)dt.Month);
        assertEquals(1, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2024, 0, 1)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2023, (int)dt.Year);
        assertEquals(12, (int)dt.Month);
        assertEquals(1, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2024, 2, 30)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(3, (int)dt.Month);
        assertEquals(1, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2024, -1, 1)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2023, (int)dt.Year);
        assertEquals(11, (int)dt.Month);
        assertEquals(1, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2024, 12, 0)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(11, (int)dt.Month);
        assertEquals(30, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2024, 1, -1)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2023, (int)dt.Year);
        assertEquals(12, (int)dt.Month);
        assertEquals(30, (int)dt.Day);
    }

    @Test
    public void WEEKNUM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("WEEKNUM(\"2016-1-3\")", 0);
        assertEquals(2, dt);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-2\")", 0);
        assertEquals(1, dt);

        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-4\",2)", 0);
        assertEquals(2, dt);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-3\",2)", 0);
        assertEquals(1, dt);
        dt = engine.TryEvaluate("WEEKNUM(\"2016-1-2\",2)", 0);
        assertEquals(1, dt);
    }

    @Test
    public void WEEKNUM_ExcelCompatible_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();

        // 2016年1月1日是周五
        // type 1: 周日作为第一天
        // type 2: 周一作为第一天
        // type 11: 周一作为第一天 (同type 2)
        // type 12-16: 不同起始日
        // type 17: 周日作为第一天 (同type 1)
        // type 21: ISO 8601，周一作为第一天

        // 2016-1-1 (周五)
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),1)", 0));   // type 1: 周五在第1周
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),2)", 0));  // type 2: 周一为起始日
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),11)", 0));// type 11: 同type 2
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),12)", 0)); // type 12: 周二为起始日
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),13)", 0)); // type 13: 周三为起始日
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),14)", 0)); // type 14: 周四为起始日
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),15)", 0)); // type 15: 周五为起始日
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),16)", 0));  // type 16: 周六为起始日
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,1),17)", 0));  // type 17: 同type 1

        // 2016-1-3 (周日)
        assertEquals(2, engine.TryEvaluate("WEEKNUM(date(2016,1,3),1)", 0));   // type 1: 周日为新一周开始
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,3),2)", 0));   // type 2: 周一为新一周开始
        assertEquals(1, engine.TryEvaluate("WEEKNUM(date(2016,1,3),11)", 0));  // type 11: 同type 2
        assertEquals(2, engine.TryEvaluate("WEEKNUM(date(2016,1,3),16)", 0));  // type 16: 周六为起始日
        assertEquals(2, engine.TryEvaluate("WEEKNUM(date(2016,1,3),17)", 0));  // type 17: 同type 1

        // 2016-1-4 (周一)
        assertEquals(2, engine.TryEvaluate("WEEKNUM(date(2016,1,4),1)", 0));  // type 1
        assertEquals(2, engine.TryEvaluate("WEEKNUM(date(2016,1,4),2)", 0));  // type 2: 周一在第2周
        assertEquals(2, engine.TryEvaluate("WEEKNUM(date(2016,1,4),11)", 0)); // type 11: 同type 2
        assertEquals(0, engine.TryEvaluate("WEEKNUM(date(2016,1,4),21)", 0)); // type 21: ISO 8601

        // 测试 ISO 8601 (type 21)
        // 2016年1月4日是ISO 8601标准的第1周第一天
        assertEquals(0, engine.TryEvaluate("WEEKNUM(date(2016,1,4),21)", 0));
        assertEquals(0, engine.TryEvaluate("WEEKNUM(date(2016,1,3),21)", 0)); // 2016-1-3是2015年的第53周

        // 2021年1月1日是周五，ISO 8601第53周
        assertEquals(0, engine.TryEvaluate("WEEKNUM(date(2021,1,1),21)", 0));
        // 2021年1月4日是周一，ISO 8601第1周
        assertEquals(0, engine.TryEvaluate("WEEKNUM(date(2021,1,4),21)", 0));
    }

    @Test
    public void YEARFRAC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1')", 0.0);
        assertEquals(0.5, t, 1e-3);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2013-1-1')", 0.0);
        assertEquals(1.0, t, 1e-3);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 0)", 0.0);
        assertEquals(0.5, t, 1e-3);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 1)", 0.0);
        assertEquals(0.49726776, t, 1e-3);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 2)", 0.0);
        assertEquals(0.505555556, t, 1e-3);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 3)", 0.0);
        assertEquals(0.498630137, t, 1e-3);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 4)", 0.0);
        assertEquals(0.5, t, 1e-3);
    }

    @Test
    public void AddYears_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-02-01'.addYears(1).year()", 0);
        assertEquals(2001, dt);

        dt = engine.TryEvaluate("'2000/02/01'.addYears(1).year()", 0);
        assertEquals(2001, dt);
    }

    @Test
    public void AddMonths_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-02-01'.AddMonths(1).Month()", 0);
        assertEquals(3, dt);
    }

    @Test
    public void AddDays_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-02-01'.AddDays(1).Day()", 0);
        assertEquals(2, dt);
    }

    @Test
    public void AddHours_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddHours(1).Hour()", 0);
        assertEquals(13, dt);
    }

    @Test
    public void AddMinutes_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddMinutes(1).Minute()", 0);
        assertEquals(6, dt);
    }

    @Test
    public void AddSeconds_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(7, dt);

        dt = engine.TryEvaluate("'2000-02-01 10:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(7, dt);

        dt = engine.TryEvaluate("'2000-02-01 20:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(7, dt);

        dt = engine.TryEvaluate("'2000-02-01 9:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(7, dt);

        dt = engine.TryEvaluate("'7:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(7, dt);

        dt = engine.TryEvaluate("'10:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(7, dt);

        dt = engine.TryEvaluate("'20:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(7, dt);

        dt = engine.TryEvaluate("'2000-02-01 24:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(0, dt);
    }

    // 方法式调用测试

    @Test
    public void MethodStyle_DATEVALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("'2016-01-01'.DATEVALUE()", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2016, (int)dt.Year);
        assertEquals(1, (int)dt.Month);
        assertEquals(1, (int)dt.Day);
    }

    @Test
    public void MethodStyle_TIMEVALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("'12:12:12'.TIMEVALUE()", new MyDate(0, 0, 0, 0, 0, 0));
        assertEquals(12, (int)dt.Hour);
        assertEquals(12, (int)dt.Minute);
        assertEquals(12, (int)dt.Second);
    }

    @Test
    public void MethodStyle_YEAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2016-05-15'.YEAR()", 0);
        assertEquals(2016, dt);
    }

    @Test
    public void MethodStyle_MONTH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2016-05-15'.MONTH()", 0);
        assertEquals(5, dt);
    }

    @Test
    public void MethodStyle_DAY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2016-05-15'.DAY()", 0);
        assertEquals(15, dt);
    }

    @Test
    public void MethodStyle_HOUR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'12:30:45'.HOUR()", 0);
        assertEquals(12, dt);
    }

    @Test
    public void MethodStyle_MINUTE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'12:30:45'.MINUTE()", 0);
        assertEquals(30, dt);
    }

    @Test
    public void MethodStyle_SECOND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'12:30:45'.SECOND()", 0);
        assertEquals(45, dt);
    }

    @Test
    public void MethodStyle_ADDYEARS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-01-01'.ADDYEARS(1).YEAR()", 0);
        assertEquals(2001, dt);
    }

    @Test
    public void MethodStyle_ADDMONTHS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-01-15'.ADDMONTHS(2).MONTH()", 0);
        assertEquals(3, dt);
    }

    @Test
    public void MethodStyle_ADDDAYS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-01-01'.ADDDAYS(10).DAY()", 0);
        assertEquals(11, dt);
    }

    @Test
    public void MethodStyle_ADDHOURS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-01-01 10:00:00'.ADDHOURS(5).HOUR()", 0);
        assertEquals(15, dt);
    }

    @Test
    public void MethodStyle_ADDMINUTES_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-01-01 10:30:00'.ADDMINUTES(45).MINUTE()", 0);
        assertEquals(15, dt);
    }

    @Test
    public void MethodStyle_ADDSECONDS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2000-01-01 10:00:30'.ADDSECONDS(45).SECOND()", 0);
        assertEquals(15, dt);
    }

    @Test
    public void MethodStyle_TIMESTAMP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseLocalTime = true;
        long dt = engine.TryEvaluate("'2016-01-01'.TIMESTAMP()", 0L);
        assertEquals(1451577600000L, dt);

        dt = engine.TryEvaluate("'2016-01-01'.TIMESTAMP(1)", 0L);
        assertEquals(1451577600L, dt);
    }

    // 边界值测试

    @Test
    public void LeapYear_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt = engine.TryEvaluate_MyDate("DATE(2020,2,29)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2020, (int)dt.Year);
        assertEquals(2, (int)dt.Month);
        assertEquals(29, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2024,2,29)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2024, (int)dt.Year);
        assertEquals(2, (int)dt.Month);
        assertEquals(29, (int)dt.Day);

        dt = engine.TryEvaluate_MyDate("DATE(2019,2,28)", new MyDate(1900, 1, 1, 0, 0, 0));
        assertEquals(2019, (int)dt.Year);
        assertEquals(2, (int)dt.Month);
        assertEquals(28, (int)dt.Day);

        int year = engine.TryEvaluate("'2020-02-29'.YEAR()", 0);
        assertEquals(2020, year);
        int month = engine.TryEvaluate("'2020-02-29'.MONTH()", 0);
        assertEquals(2, month);
        int day = engine.TryEvaluate("'2020-02-29'.DAY()", 0);
        assertEquals(29, day);
    }

    @Test
    public void LeapYear_AddDays_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("'2020-02-28'.ADDDAYS(1).DAY()", 0);
        assertEquals(29, dt);

        dt = engine.TryEvaluate("'2020-03-01'.ADDDAYS(-1).DAY()", 0);
        assertEquals(29, dt);
    }

    @Test
    public void MonthBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("DATE(2020,1,31).ADDMONTHS(1).MONTH()", 0);
        assertEquals(2, dt);

        dt = engine.TryEvaluate("DATE(2020,3,31).ADDMONTHS(-1).MONTH()", 0);
        assertEquals(2, dt);
    }

    @Test
    public void YearBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).YEAR()", 0);
        assertEquals(2021, dt);
        dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).MONTH()", 0);
        assertEquals(1, dt);
        dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).DAY()", 0);
        assertEquals(1, dt);

        dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).YEAR()", 0);
        assertEquals(2020, dt);
        dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).MONTH()", 0);
        assertEquals(12, dt);
        dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).DAY()", 0);
        assertEquals(31, dt);
    }
}
