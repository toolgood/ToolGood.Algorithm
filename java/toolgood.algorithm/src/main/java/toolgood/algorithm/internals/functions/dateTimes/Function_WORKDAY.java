package toolgood.algorithm.internals.functions.dateTimes;

import java.util.HashSet;
import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_WORKDAY extends Function_N {
    public Function_WORKDAY(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Workday";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate(engine, tempParameter, 0);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetNumber(engine, tempParameter, 1);
        if (args2.IsErrorOrNone()) { return args2; }

        DateTime startMyDate = args1.DateValue().ToDateTime();
        int days = args2.IntValue();
        HashSet<DateTime> list = new HashSet<DateTime>();
        for (int i = 2; i < funcs.length; i++) {
            Operand ar = GetDate(engine, tempParameter, i);
            if (ar.IsErrorOrNone()) { return ar; }
            list.add(ar.DateValue().ToDateTime());
        }
        if (days > 0) {
            // 先逐天对齐到周一（最多 6 天），期间消耗工作日
            while (startMyDate.getDayOfWeek() != 1 && days > 0) {
                startMyDate = startMyDate.plusDays(1);
                if (startMyDate.getDayOfWeek() == 6 || startMyDate.getDayOfWeek() == 7) continue;
                if (list.contains(startMyDate)) continue;
                days--;
            }
            if (days == 0) return Operand.Create(startMyDate);

            // 整周粗跳：起点已是周一，每 5 个工作日 = 7 天
            DateTime afterJump = startMyDate.plusDays((days / 5) * 7);
            int extra = 0;
            for (DateTime h : list) {
                if (h.isAfter(startMyDate) && !h.isAfter(afterJump)
                        && h.getDayOfWeek() != 6 && h.getDayOfWeek() != 7) {
                    extra++;
                }
            }
            startMyDate = afterJump;
            days = days % 5 + extra;
            while (days > 0) {
                startMyDate = startMyDate.plusDays(1);
                if (startMyDate.getDayOfWeek() == 6 || startMyDate.getDayOfWeek() == 7) continue;
                if (list.contains(startMyDate)) continue;
                days--;
            }
        } else if (days < 0) {
            // 先逐天对齐到周五（最多 6 天），期间消耗工作日
            while (startMyDate.getDayOfWeek() != 5 && days < 0) {
                startMyDate = startMyDate.plusDays(-1);
                if (startMyDate.getDayOfWeek() == 6 || startMyDate.getDayOfWeek() == 7) continue;
                if (list.contains(startMyDate)) continue;
                days++;
            }
            if (days == 0) return Operand.Create(startMyDate);

            // 整周粗跳：起点已是周五，每 5 个工作日 = 7 天
            DateTime afterJump = startMyDate.plusDays((-days / 5) * -7);
            int extra = 0;
            for (DateTime h : list) {
                if (!h.isBefore(afterJump) && h.isBefore(startMyDate)
                        && h.getDayOfWeek() != 6 && h.getDayOfWeek() != 7) {
                    extra++;
                }
            }
            startMyDate = afterJump;
            days = -((-days) % 5) - extra;
            while (days < 0) {
                startMyDate = startMyDate.plusDays(-1);
                if (startMyDate.getDayOfWeek() == 6 || startMyDate.getDayOfWeek() == 7) continue;
                if (list.contains(startMyDate)) continue;
                days++;
            }
        }
        return Operand.Create(startMyDate);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.DATE);
        funcs[1].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        for (int i = 2; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.DATE);
        }
    }
}
