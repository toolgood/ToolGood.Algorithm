package toolgood.algorithm.helpers;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.MyDate;

public class MyDateTest {

    // region 构造函数测试

    @Test
    public void Constructor_DateTime_Test() {
        MyDate dt = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate myDate = new MyDate(dt);

        assertEquals(2024, (int) myDate.Year);
        assertEquals(6, (int) myDate.Month);
        assertEquals(15, (int) myDate.Day);
        assertEquals(10, myDate.Hour);
        assertEquals(30, myDate.Minute);
        assertEquals(45, myDate.Second);
    }

    @Test
    public void Constructor_TimeSpan_Test() {
        MyDate ts = new MyDate(null, null, 1, 10, 30, 45);
        MyDate myDate = new MyDate(ts);

        assertNull(myDate.Year);
        assertNull(myDate.Month);
        assertEquals(1, (int) myDate.Day);
        assertEquals(10, myDate.Hour);
        assertEquals(30, myDate.Minute);
        assertEquals(45, myDate.Second);
    }

    @Test
    public void Constructor_Params_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);

        assertEquals(2024, (int) myDate.Year);
        assertEquals(6, (int) myDate.Month);
        assertEquals(15, (int) myDate.Day);
        assertEquals(10, myDate.Hour);
        assertEquals(30, myDate.Minute);
        assertEquals(45, myDate.Second);
    }

    // endregion

    // region Parse 测试

    @Test
    public void Parse_DateTime_Test() {
        MyDate myDate = MyDate.parse("2024-06-15 10:30:45");
        assertEquals(2024, (int) myDate.Year);
        assertEquals(6, (int) myDate.Month);
        assertEquals(15, (int) myDate.Day);
        assertEquals(10, myDate.Hour);
        assertEquals(30, myDate.Minute);
        assertEquals(45, myDate.Second);
    }

    @Test
    public void Parse_DateTime_Slash_Test() {
        MyDate myDate = MyDate.parse("2024/06/15 10:30:45");
        assertEquals(2024, (int) myDate.Year);
        assertEquals(6, (int) myDate.Month);
        assertEquals(15, (int) myDate.Day);
        assertEquals(10, myDate.Hour);
        assertEquals(30, myDate.Minute);
        assertEquals(45, myDate.Second);
    }

    @Test
    public void Parse_Date_Test() {
        MyDate myDate = MyDate.parse("2024-06-15");
        assertEquals(2024, (int) myDate.Year);
        assertEquals(6, (int) myDate.Month);
        assertEquals(15, (int) myDate.Day);
        assertEquals(0, myDate.Hour);
        assertEquals(0, myDate.Minute);
        assertEquals(0, myDate.Second);
    }

    @Test
    public void Parse_Time_Test() {
        MyDate myDate = MyDate.parse("10:30:45");
        assertNull(myDate.Year);
        assertNull(myDate.Month);
        assertNull(myDate.Day);
        assertEquals(10, myDate.Hour);
        assertEquals(30, myDate.Minute);
        assertEquals(45, myDate.Second);
    }

    @Test
    public void Parse_Time_NoSeconds_Test() {
        MyDate myDate = MyDate.parse("10:30");
        assertNull(myDate.Year);
        assertNull(myDate.Month);
        assertNull(myDate.Day);
        assertEquals(10, myDate.Hour);
        assertEquals(30, myDate.Minute);
        assertEquals(0, myDate.Second);
    }

    @Test
    public void Parse_Invalid_Test() {
        MyDate myDate = MyDate.parse("invalid date");
        assertNull(myDate);
    }

    // endregion

    // region ToDateTime 测试

    @Test
    public void ToDateTime_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        DateTime dt = myDate.ToDateTime();

        assertEquals(2024, dt.getYear());
        assertEquals(6, dt.getMonthOfYear());
        assertEquals(15, dt.getDayOfMonth());
        assertEquals(10, dt.getHourOfDay());
        assertEquals(30, dt.getMinuteOfHour());
        assertEquals(45, dt.getSecondOfMinute());
        assertEquals(DateTimeZone.UTC, dt.getZone());
    }

    @Test
    public void ToDateTime_Kind_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        DateTime dt = myDate.ToDateTime(DateTimeZone.forOffsetHours(8));

        assertEquals(DateTimeZone.forOffsetHours(8), dt.getZone());
    }

    // endregion

    // region AddYears 测试

    @Test
    public void AddYears_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddYears(1);

        assertEquals(2025, (int) result.Year);
        assertEquals(6, (int) result.Month);
        assertEquals(15, (int) result.Day);
    }

    @Test
    public void AddYears_Negative_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddYears(-1);

        assertEquals(2023, (int) result.Year);
    }

    // endregion

    // region AddMonths 测试

    @Test
    public void AddMonths_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddMonths(1);

        assertEquals(7, (int) result.Month);
    }

    @Test
    public void AddMonths_Overflow_Test() {
        MyDate myDate = new MyDate(2024, 11, 15, 10, 30, 45);
        MyDate result = myDate.AddMonths(2);

        assertEquals(2025, (int) result.Year);
        assertEquals(1, (int) result.Month);
    }

    // endregion

    // region AddDays 测试

    @Test
    public void AddDays_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddDays(5);

        assertEquals(20, (int) result.Day);
    }

    @Test
    public void AddDays_Negative_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddDays(-5);

        assertEquals(10, (int) result.Day);
    }

    // endregion

    // region AddHours 测试

    @Test
    public void AddHours_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddHours(5);

        assertEquals(15, result.Hour);
    }

    @Test
    public void AddHours_NoOverflow_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddHours(2);

        assertEquals(12, result.Hour);
    }

    // endregion

    // region AddMinutes 测试

    @Test
    public void AddMinutes_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddMinutes(15);

        assertEquals(45, result.Minute);
    }

    @Test
    public void AddMinutes_NoOverflow_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddMinutes(10);

        assertEquals(40, result.Minute);
    }

    // endregion

    // region AddSeconds 测试

    @Test
    public void AddSeconds_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddSeconds(10);

        assertEquals(55, result.Second);
    }

    @Test
    public void AddSeconds_NoOverflow_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        MyDate result = myDate.AddSeconds(5);

        assertEquals(50, result.Second);
    }

    // endregion

    // region ToString 测试

    @Test
    public void ToString_DateTime_Test() {
        MyDate myDate = new MyDate(2024, 6, 15, 10, 30, 45);
        String str = myDate.toString();

        assertTrue(str.contains("2024"));
        assertTrue(str.contains("06"));
        assertTrue(str.contains("15"));
    }

    @Test
    public void ToString_TimeOnly_Test() {
        MyDate myDate = new MyDate(null, null, null, 10, 30, 45);
        String str = myDate.toString();

        assertTrue(str.contains("10:30:45"));
    }

    // endregion
}
