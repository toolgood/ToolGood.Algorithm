/**
 * Represents the base class for all function implementations that can be calculated by an algorithm engine.
 * <p>
 * This abstract class defines a contract for functions that can be Evaluated within the context
 * of an algorithm engine. Derived classes must implement the {@link #Evaluate} method to provide specific
 * function logic.
 */
package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.function.BiFunction;

import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

public abstract class FunctionBase {
    /**
     * 进行计算
     * 
     * @param work
     * @param tempParameter 临时参数，未找到返回null
     * @return
     */
    public abstract Operand Evaluate(AlgorithmEngine work, BiFunction<AlgorithmEngine, String, Operand> tempParameter);

    //region TryEvaluate

    /**
     * 执行函数,如果异常，返回默认值
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public short TryEvaluate(AlgorithmEngine work, short def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.IsNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    work.SetLastError(obj.ErrorMsg());
                    return def;
                }
            }
            return (short) obj.IntValue();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认值
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public int TryEvaluate(AlgorithmEngine work, int def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.IsNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    work.SetLastError(obj.ErrorMsg());
                    return def;
                }
            }
            return obj.IntValue();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认值
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public long TryEvaluate(AlgorithmEngine work, long def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.IsNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    work.SetLastError(obj.ErrorMsg());
                    return def;
                }
            }
            return obj.LongValue();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认值
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public float TryEvaluate(AlgorithmEngine work, float def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.IsNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    work.SetLastError(obj.ErrorMsg());
                    return def;
                }
            }
            return (float) obj.DoubleValue
();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认值
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public double TryEvaluate(AlgorithmEngine work, double def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.IsNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    work.SetLastError(obj.ErrorMsg());
                    return def;
                }
            }
            return obj.DoubleValue
();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认值
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public String TryEvaluate(AlgorithmEngine work, String def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.IsText()) {
                obj = obj.ToText("It can't be converted to string!");
                if (obj.IsError()) {
                    work.SetLastError(obj.ErrorMsg());
                    return def;
                }
            }
            return obj.TextValue();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认值
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public boolean TryEvaluate(AlgorithmEngine work, boolean def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.IsBoolean()) {
                obj = obj.ToBoolean("It can't be converted to bool!");
                if (obj.IsError()) {
                    work.SetLastError(obj.ErrorMsg());
                    return def;
                }
            }
            return obj.BooleanValue();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认值。
     * 解决 def 为 null 二义性问题
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public MyDate TryEvaluate_MyDate(AlgorithmEngine work, MyDate def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.IsDate()) {
                obj = obj.ToMyDate("It can't be converted to DateTime!");
                if (obj.IsError()) {
                    work.SetLastError(obj.ErrorMsg());
                    return def;
                }
            }
            return obj.DateValue();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    //endregion TryEvaluate

    /**
     * Returns a string that represents the current object.
     * 
     * @return A string that represents the current object.
     */
    @Override
    public String toString() {
        StringBuilder stringBuilder = new StringBuilder();
        toString(stringBuilder, false);
        return stringBuilder.toString();
    }

    /**
     * Appends a string representation of the current object to the specified StringBuilder, optionally including
     * brackets.
     * 
     * @param stringBuilder The StringBuilder to which the string representation will be appended. Cannot be null.
     * @param addBrackets true to enclose the string representation in brackets; otherwise, false.
     */
    public abstract void toString(StringBuilder stringBuilder, boolean addBrackets);

    /**
     * 获取异常堆栈信息
     * 
     * @param ex 异常
     * @return 堆栈信息字符串
     */
    private String getStackTrace(Exception ex) {
        StringBuilder sb = new StringBuilder();
        for (StackTraceElement element : ex.getStackTrace()) {
            sb.append(element.toString()).append("\r\n");
        }
        return sb.toString();
    }
}
