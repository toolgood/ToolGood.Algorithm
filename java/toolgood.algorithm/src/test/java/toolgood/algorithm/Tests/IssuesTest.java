package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;

import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.MyDate;

public class IssuesTest {

    @Test
    public void issues_12() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("Year(44406)=2021", false);
        assertEquals(dt, true);
        dt = engine.TryEvaluate("MONTH(44406)=7", false);
        assertEquals(dt, true);
        dt = engine.TryEvaluate("DAY(44406)=29", false);
        assertEquals(dt, true);

        int num = engine.TryEvaluate("date(2011,2,2)", 0);
        assertEquals(num, 40576);
    }

    public void issues_13() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int dt = engine.TryEvaluate("days360(date(2020,5,31),date(2023,12,15))", 0);
        assertEquals(dt, 1275);

    }

    @Test
      public void issues_27()
      {
          AlgorithmEngine engine = new AlgorithmEngine();
          MyDate dt = engine.TryEvaluate("DATE(2024, 8, 1) + TIME(8, 0, 0)", MyDate.now());
          assertEquals(dt.toString(),  "2024-08-01 08:00:00");
      }

}
