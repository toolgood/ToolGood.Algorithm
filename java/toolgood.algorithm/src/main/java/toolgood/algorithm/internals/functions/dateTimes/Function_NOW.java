package toolgood.algorithm.internals.functions.dateTimes;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_NOW extends Function_0 {
    @Override
    public String Name() {
        return "Now";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (engine.UseLocalTime) {
            return Operand.Create(DateTime.now());
        } else {
            return Operand.Create(DateTime.now(DateTimeZone.UTC));
        }
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Now()");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }
}
