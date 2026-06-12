package toolgood.algorithm.internals.functions.dateTimes;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.Function_0;

final class Function_TODAY extends Function_0 {
    @Override
    public String Name() {
        return "Today";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        DateTime now;
        if (engine.UseLocalTime) {
            now = DateTime.now();
        } else {
            now = DateTime.now(DateTimeZone.UTC);
        }
        return Operand.Create(new MyDate(now.getYear(), now.getMonthOfYear(), now.getDayOfMonth(), 0, 0, 0));
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Today()");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }
}
