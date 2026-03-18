/**
 * 参数类型类
 */
package toolgood.algorithm.internals;

import toolgood.algorithm.enums.OperandType;

public class ParameterType {
    /**
     * 参数名称
     */
    private String name;
    /**
     * 参数类型
     */
    private OperandType type;

    /**
     * 操作符，可为空
     */
    private String operator;

    /**
     * 操作值，可为空
     */
    private String value;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public OperandType getType() {
        return type;
    }

    public void setType(OperandType type) {
        this.type = type;
    }

    public String getOperator() {
        return operator;
    }

    public void setOperator(String operator) {
        this.operator = operator;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }
}
