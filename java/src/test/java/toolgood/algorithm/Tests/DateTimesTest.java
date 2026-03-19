package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.temporal.ChronoUnit;

import static org.junit.Assert.assertEquals;

public class DateTimesTest {

    @Test
    void DATEVALUE_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("DATEVALUE('2016-01-01')", "");
        assertEquals(dt, LocalDate.of(2016, 1, 1));
        
        dt = engine.TryEvaluate("DATEVALUE('2016/01/01')", "");
        assertEquals(dt, LocalDate.of(2016, 1, 1));

        dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", 0);
        assertEquals(dt, LocalDateTime.of(2023, 8, 5, 19, 28, 19));

        dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", 0);
        assertEquals(dt, LocalDateTime.of(2023, 8, 5, 19, 28, 19));

        engine.UseLocalTime = true;
        dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", 0);
        assertEquals(dt, LocalDateTime.of(2023, 8, 5, 19, 28, 19));

        dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", 0);
        assertEquals(dt, LocalDateTime.of(2023, 8, 5, 19, 28, 19));

        dt = engine.TryEvaluate("DATEVALUE('1691234899000')", "");
        assertEquals(dt, LocalDateTime.of(2023, 8, 5, 19, 28, 19));

        dt = engine.TryEvaluate("DATEVALUE('1691234899')", "");
        assertEquals(dt, LocalDateTime.of(2023, 8, 5, 19, 28, 19));
    }

    @Test
    void TIMESTAMP_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseLocalTime = true;

        Object dt = engine.TryEvaluate("TIMESTAMP('2016-01-01')", 0L);
        assertEquals(dt, 1451577600000L);

        dt = engine.TryEvaluate("TIMESTAMP('2016/01/01')", 0L);
        assertEquals(dt, 1451577600000L);

        dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',0)", 0L);
        assertEquals(dt, 1451577600000L);

        dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',1)", 0L);
        assertEquals(dt, 1451577600L);
    }

    @Test
    void TIMEVALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("TIMEVALUE('12:12:12')", "");
        assertEquals(dt, LocalTime.of(12, 12, 12));
    }

    @Test
    void DATE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("DATE(2016,1,1)", "");
        assertEquals(dt, LocalDate.of(2016, 1, 1));
    }

    @Test
    void time_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("time(11,12,13)", "");
        assertEquals(dt, LocalTime.of(11, 12, 13));
    }

    @Test
    void Now_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("now()", "");
        LocalDateTime now = LocalDateTime.now();
        LocalDateTime result = (LocalDateTime) dt;
        assertEquals(result.getYear(), now.getYear());
        assertEquals(result.getMonth(), now.getMonth());
        assertEquals(result.getDayOfMonth(), now.getDayOfMonth());
        assertEquals(result.getHour(), now.getHour());
        assertEquals(result.getMinute(), now.getMinute());
    }

    @Test
    void Today_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("Today()", "");
        LocalDate today = LocalDate.now();
        assertEquals(dt, today);
    }

    @Test
    void Year_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("year(now())", 0);
        int year = LocalDateTime.now().getYear();
        assertEquals(dt, year);
    }

    @Test
    void Month_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("Month(now())", 0);
        int month = LocalDateTime.now().getMonthValue();
        assertEquals(dt, month);
    }

    @Test
    void Day_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("Day(now())", 0);
        int day = LocalDateTime.now().getDayOfMonth();
        assertEquals(dt, day);
    }

    @Test
    void Hour_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("Hour(now())", 0);
        int hour = LocalDateTime.now().getHour();
        assertEquals(dt, hour);
    }

    @Test
    void Minute_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("Minute(now())", 0);
        int minute = LocalDateTime.now().getMinute();
        assertEquals(dt, minute);
    }

    @Test
    void Second_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("Second(now())", 0);
        int second = LocalDateTime.now().getSecond();
        assertEquals(dt, second);
    }

    @Test
    void WEEKDAY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7))", 0);
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
    void DATEDIF_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0);
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
    void DAYS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("DAYS('2017-1-7','1975-1-30')", 0);
        assertEquals(dt, 15318);

        dt = engine.TryEvaluate("DAYS('2016-3-1','2016-2-28')", 0);
        assertEquals(dt, 2);

        dt = engine.TryEvaluate("DAYS('2016-2-28','2016-3-1')", 0);
        assertEquals(dt, -2);
    }

    @Test
    void DAYS360_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("DAYS360('1975-1-30','2017-1-7')", 0);
        assertEquals(dt, 15097);
    }

    @Test
    void EDATE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("EDATE('2012-1-31',32)", "");
        assertEquals(dt, LocalDate.of(2014, 9, 30));
    }

    @Test
    void EOMONTH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("EOMONTH('2012-2-1',32)", "");
        assertEquals(dt, LocalDate.of(2014, 10, 31));
    }

    @Test
    void NETWORKDAYS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("NETWORKDAYS('2012-1-1','2013-1-1')", 0);
        assertEquals(dt, 262);
    }

    @Test
    void WORKDAY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("WORKDAY('2012-1-2',145)", "");
        assertEquals(dt, LocalDate.of(2012, 7, 23));
    }

    @Test
    void WEEKNUM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("WEEKNUM('2016-1-3')", 0);
        assertEquals(dt, 2);
        dt = engine.TryEvaluate("WEEKNUM('2016-1-2')", 0);
        assertEquals(dt, 1);

        dt = engine.TryEvaluate("WEEKNUM('2016-1-4',2)", 0);
        assertEquals(dt, 2);
        dt = engine.TryEvaluate("WEEKNUM('2016-1-3',2)", 0);
        assertEquals(dt, 1);
        dt = engine.TryEvaluate("WEEKNUM('2016-1-2',2)", 0);
        assertEquals(dt, 1);
    }

    @Test
    void YEARFRAC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1')", 0.0);
        assertEquals(Math.round((Double) t * 1000) / 1000.0, Math.round(0.5 * 1000) / 1000.0);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2013-1-1')", 0.0);
        assertEquals(Math.round((Double) t * 1000) / 1000.0, Math.round(1.0 * 1000) / 1000.0);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 0)", 0.0);
        assertEquals(Math.round((Double) t * 1000) / 1000.0, Math.round(0.5 * 1000) / 1000.0);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 1)", 0.0);
        assertEquals(Math.round((Double) t * 1000) / 1000.0, Math.round(0.49726776 * 1000) / 1000.0);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 2)", 0.0);
        assertEquals(Math.round((Double) t * 1000) / 1000.0, Math.round(0.505555556 * 1000) / 1000.0);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 3)", 0.0);
        assertEquals(Math.round((Double) t * 1000) / 1000.0, Math.round(0.498630137 * 1000) / 1000.0);

        t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 4)", 0.0);
        assertEquals(Math.round((Double) t * 1000) / 1000.0, Math.round(0.5 * 1000) / 1000.0);
    }

    @Test
    void AddYears_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-02-01'.addYears(1).year()", 0);
        assertEquals(dt, 2001);

        dt = engine.TryEvaluate("'2000/02/01'.addYears(1).year()", 0);
        assertEquals(dt, 2001);
    }

    @Test
    void AddMonths_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-02-01'.AddMonths(1).Month()", 0);
        assertEquals(dt, 3);
    }

    @Test
    void AddDays_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-02-01'.AddDays(1).Day()", 0);
        assertEquals(dt, 2);
    }

    @Test
    void AddHours_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddHours(1).Hour()", 0);
        assertEquals(dt, 13);
    }

    @Test
    void AddMinutes_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddMinutes(1).Minute()", 0);
        assertEquals(dt, 6);
    }

    @Test
    void AddSeconds_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 7);

        dt = engine.TryEvaluate("'2000-02-01 10:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 7);

        dt = engine.TryEvaluate("'2000-02-01 20:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 7);

        dt = engine.TryEvaluate("'2000-02-01 9:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 7);

        dt = engine.TryEvaluate("'7:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 7);

        dt = engine.TryEvaluate("'10:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 7);

        dt = engine.TryEvaluate("'20:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 7);

        dt = engine.TryEvaluate("'2000-02-01 24:05:06'.AddSeconds(1).Second()", 0);
        assertEquals(dt, 0);
    }

    // 方法式调用测试
    @Test
    void MethodStyle_DATEVALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2016-01-01'.DATEVALUE()", "");
        assertEquals(dt, LocalDate.of(2016, 1, 1));
    }

    @Test
    void MethodStyle_TIMEVALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'12:12:12'.TIMEVALUE()", "");
        assertEquals(dt, LocalTime.of(12, 12, 12));
    }

    @Test
    void MethodStyle_YEAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2016-05-15'.YEAR()", 0);
        assertEquals(dt, 2016);
    }

    @Test
    void MethodStyle_MONTH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2016-05-15'.MONTH()", 0);
        assertEquals(dt, 5);
    }

    @Test
    void MethodStyle_DAY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2016-05-15'.DAY()", 0);
        assertEquals(dt, 15);
    }

    @Test
    void MethodStyle_HOUR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'12:30:45'.HOUR()", 0);
        assertEquals(dt, 12);
    }

    @Test
    void MethodStyle_MINUTE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'12:30:45'.MINUTE()", 0);
        assertEquals(dt, 30);
    }

    @Test
    void MethodStyle_SECOND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'12:30:45'.SECOND()", 0);
        assertEquals(dt, 45);
    }

    @Test
    void MethodStyle_ADDYEARS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-01-01'.ADDYEARS(1).YEAR()", 0);
        assertEquals(dt, 2001);
    }

    @Test
    void MethodStyle_ADDMONTHS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-01-15'.ADDMONTHS(2).MONTH()", 0);
        assertEquals(dt, 3);
    }

    @Test
    void MethodStyle_ADDDAYS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-01-01'.ADDDAYS(10).DAY()", 0);
        assertEquals(dt, 11);
    }

    @Test
    void MethodStyle_ADDHOURS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-01-01 10:00:00'.ADDHOURS(5).HOUR()", 0);
        assertEquals(dt, 15);
    }

    @Test
    void MethodStyle_ADDMINUTES_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-01-01 10:30:00'.ADDMINUTES(45).MINUTE()", 0);
        assertEquals(dt, 15);
    }

    @Test
    void MethodStyle_ADDSECONDS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2000-01-01 10:00:30'.ADDSECONDS(45).SECOND()", 0);
        assertEquals(dt, 15);
    }

    @Test
    void MethodStyle_TIMESTAMP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseLocalTime = true;
        Object dt = engine.TryEvaluate("'2016-01-01'.TIMESTAMP()", 0L);
        assertEquals(dt, 1451577600000L);

        dt = engine.TryEvaluate("'2016-01-01'.TIMESTAMP(1)", 0L);
        assertEquals(dt, 1451577600L);
    }

    // 边界值测试
    @Test
    void LeapYear_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("DATE(2020,2,29)", "");
        assertEquals(dt, LocalDate.of(2020, 2, 29));

        dt = engine.TryEvaluate("DATE(2024,2,29)", "");
        assertEquals(dt, LocalDate.of(2024, 2, 29));

        dt = engine.TryEvaluate("DATE(2019,2,28)", "");
        assertEquals(dt, LocalDate.of(2019, 2, 28));

        Object year = engine.TryEvaluate("'2020-02-29'.YEAR()", 0);
        assertEquals(year, 2020);
        Object month = engine.TryEvaluate("'2020-02-29'.MONTH()", 0);
        assertEquals(month, 2);
        Object day = engine.TryEvaluate("'2020-02-29'.DAY()", 0);
        assertEquals(day, 29);
    }

    @Test
    void LeapYear_AddDays_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("'2020-02-28'.ADDDAYS(1).DAY()", 0);
        assertEquals(dt, 29);

        dt = engine.TryEvaluate("'2020-03-01'.ADDDAYS(-1).DAY()", 0);
        assertEquals(dt, 29);
    }

    @Test
    void MonthBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("DATE(2020,1,31).ADDMONTHS(1).MONTH()", 0);
        assertEquals(dt, 2);

        dt = engine.TryEvaluate("DATE(2020,3,31).ADDMONTHS(-1).MONTH()", 0);
        assertEquals(dt, 2);
    }

    @Test
    void YearBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).YEAR()", 0);
        assertEquals(dt, 2021);
        dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).MONTH()", 0);
        assertEquals(dt, 1);
        dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).DAY()", 0);
        assertEquals(dt, 1);

        dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).YEAR()", 0);
        assertEquals(dt, 2020);
        dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).MONTH()", 0);
        assertEquals(dt, 12);
        dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).DAY()", 0);
        assertEquals(dt, 31);
    }
}
