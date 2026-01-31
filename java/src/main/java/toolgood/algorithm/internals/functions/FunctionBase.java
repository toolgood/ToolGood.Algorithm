/**
 * Represents the base class for all function implementations that can be calculated by an algorithm engine.
 * <p>
 * This abstract class defines a contract for functions that can be Evaluated within the context
 * of an algorithm engine. Derived classes must implement the {@link #Evaluate} method to provide specific
 * function logic.
 */
package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.function.Function;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;

public abstract class FunctionBase {
    /**
     * 进行计算
     * 
     * @param work
     * @param tempParameter 临时参数，未找到返回null
     * @return
     */
    public abstract Operand Evaluate(AlgorithmEngine work, Function<AlgorithmEngine, String, Operand> tempParameter);

    //region TryEvaluate

    /**
     * 执行函数,如果异常，返回默认值
     * 
     * @param work
     * @param def
     * @param tempParameter
     * @return
     */
    public short TryEvaluate(AlgorithmEngine work, short def, Function<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.isNumber()) {
                obj = obj.toNumber("It can't be converted to number!");
                if (obj.isError()) {
                    work.setLastError(obj.getErrorMsg());
                    return def;
                }
            }
            return (short) obj.getIntValue();
        } catch (Exception ex) {
            work.setLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
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
    public int TryEvaluate(AlgorithmEngine work, int def, Function<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.isNumber()) {
                obj = obj.toNumber("It can't be converted to number!");
                if (obj.isError()) {
                    work.setLastError(obj.getErrorMsg());
                    return def;
                }
            }
            return obj.getIntValue();
        } catch (Exception ex) {
            work.setLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
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
    public long TryEvaluate(AlgorithmEngine work, long def, Function<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.isNumber()) {
                obj = obj.toNumber("It can't be converted to number!");
                if (obj.isError()) {
                    work.setLastError(obj.getErrorMsg());
                    return def;
                }
            }
            return obj.getLongValue();
        } catch (Exception ex) {
            work.setLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
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
    public float TryEvaluate(AlgorithmEngine work, float def, Function<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.isNumber()) {
                obj = obj.toNumber("It can't be converted to number!");
                if (obj.isError()) {
                    work.setLastError(obj.getErrorMsg());
                    return def;
                }
            }
            return (float) obj.getDoubleValue();
        } catch (Exception ex) {
            work.setLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
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
    public double TryEvaluate(AlgorithmEngine work, double def, Function<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.isNumber()) {
                obj = obj.toNumber("It can't be converted to number!");
                if (obj.isError()) {
                    work.setLastError(obj.getErrorMsg());
                    return def;
                }
            }
            return obj.getDoubleValue();
        } catch (Exception ex) {
            work.setLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
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
    public String TryEvaluate(AlgorithmEngine work, String def, Function<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.isText()) {
                obj = obj.toText("It can't be converted to string!");
                if (obj.isError()) {
                    work.setLastError(obj.getErrorMsg());
                    return def;
                }
            }
            return obj.getTextValue();
        } catch (Exception ex) {
            work.setLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
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
    public boolean TryEvaluate(AlgorithmEngine work, boolean def, Function<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.isBoolean()) {
                obj = obj.toBoolean("It can't be converted to bool!");
                if (obj.isError()) {
                    work.setLastError(obj.getErrorMsg());
                    return def;
                }
            }
            return obj.getBooleanValue();
        } catch (Exception ex) {
            work.setLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
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
    public MyDate TryEvaluate_MyDate(AlgorithmEngine work, MyDate def, Function<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = this.Evaluate(work, tempParameter);
            if (!obj.isDate()) {
                obj = obj.toMyDate("It can't be converted to DateTime!");
                if (obj.isError()) {
                    work.setLastError(obj.getErrorMsg());
                    return def;
                }
            }
            return obj.getDateValue();
        } catch (Exception ex) {
            work.setLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
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
