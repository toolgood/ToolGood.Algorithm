/**
 * Represents the base class for all function implementations that can be calculated by an algorithm engine.
 * <p>
 * This abstract class defines a contract for functions that can be Evaluated within the context
 * of an algorithm engine. Derived classes must implement the {@link #Evaluate} method to provide specific
 * function logic.
 */
package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

public abstract class FunctionBase {
    /**
     * 获取函数名称，默认返回类名（子类可覆盖）
     */
    public String Name() {
        return getClass().getSimpleName();
    }

    /**
     * 进行计算
     * 
     * @param work
     * @param tempParameter 临时参数，未找到返回null
     * @return
     */
    public abstract Operand Evaluate(AlgorithmEngine work, BiFunction<AlgorithmEngine, String, Operand> tempParameter);

    /**
     * 进行计算（无临时参数�?
     */
    public Operand Evaluate(AlgorithmEngine work) {
        return Evaluate(work, null);
    }

    /**
     * 获取结果类型（默认返�?NONE，子类可覆盖�?
     */
    public OperandType GetResultType() {
        return OperandType.NONE;
    }

    // region GetParameterTypes

    /**
     * 获取参数类型列表
     */
    public List<ParameterType> GetParameterTypes(AlgorithmEngine engine) {
        NoneEngine noneEngine = new NoneEngine(engine);
        List<ParameterType> result = new ArrayList<>();
        GetParameterTypes(noneEngine, result, OperandType.NONE, null, null);
        return result;
    }

    /**
     * 内部方法，获取参数类型（默认空实现，子类可覆盖）
     */
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
    }

    // endregion GetParameterTypes

    // region ConvertToXxx helpers

    /**
     * 转换为文本类�?
     */
    protected Operand ConvertToText(Operand arg, int paramIndex) {
        return arg.ToText("Function '%s' parameter %d is error!", Name(), paramIndex);
    }

    /**
     * 转换为布尔类�?
     */
    protected Operand ConvertToBoolean(Operand arg, int paramIndex) {
        return arg.ToBoolean("Function '%s' parameter %d is error!", Name(), paramIndex);
    }

    /**
     * 转换为数字类�?
     */
    protected Operand ConvertToNumber(Operand arg, int paramIndex) {
        return arg.ToNumber("Function '%s' parameter %d is error!", Name(), paramIndex);
    }

    /**
     * 转换为数组类�?
     */
    protected Operand ConvertToArray(Operand arg, int paramIndex) {
        return arg.ToArray("Function '%s' parameter %d is error!", Name(), paramIndex);
    }

    /**
     * 转换为日期类�?
     */
    protected Operand ConvertToDate(Operand arg, int paramIndex) {
        return arg.ToMyDate("Function '%s' parameter %d is error!", Name(), paramIndex);
    }

    // endregion

    // region Error helpers

    /**
     * 参数错误
     */
    protected Operand ParameterError(int paramIndex) {
        return Operand.Error("Function '%s' parameter %d is error!", Name(), paramIndex);
    }

    /**
     * 函数错误
     */
    protected Operand FunctionError() {
        return Operand.Error("Function '%s' parameter is error!", Name());
    }

    /**
     * 比较错误
     */
    protected Operand CompareError() {
        return Operand.Error("Function '%s' compare is error.", Name());
    }

    /**
     * 除零错误
     */
    protected Operand Div0Error() {
        return Operand.Error("Function '%s' Div 0 error!", Name());
    }

    // endregion

    //region TryEvaluate

    /**
     * 执行函数,如果异常，返回默认�?
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
     * 执行函数,如果异常，返回默认�?
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
     * 执行函数,如果异常，返回默认�?
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
     * 执行函数,如果异常，返回默认�?
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
            return (float) obj.DoubleValue();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
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
            return obj.DoubleValue();
        } catch (Exception ex) {
            work.SetLastError(ex.getMessage() + "\r\n" + getStackTrace(ex));
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
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
     * 执行函数,如果异常，返回默认�?
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
     * 执行函数,如果异常，返回默认值�?
     * 解决 def �?null 二义性问�?
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
     * @return 堆栈信息字符�?
     */
    private String getStackTrace(Exception ex) {
        StringBuilder sb = new StringBuilder();
        for (StackTraceElement element : ex.getStackTrace()) {
            sb.append(element.toString()).append("\r\n");
        }
        return sb.toString();
    }
}
