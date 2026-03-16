package toolgood.algorithm.internals;

import toolgood.algorithm.enums.OperandType;

/**
 * 参数类型类
 */
public class ParameterType {
    /**
     * 参数名称
     */
    public String Name;

    /**
     * 参数类型
     */
    public OperandType Type;

    /**
     * 操作符，可为空
     */
    public String Operator;

    /**
     * 操作值，可为空
     */
    public String Value;
}
