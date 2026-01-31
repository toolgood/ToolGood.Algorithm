/**
 * 条件树
 */
package toolgood.algorithm.internals;

import java.util.ArrayList;
import java.util.List;
import toolgood.algorithm.enums.ConditionTreeType;

public final class ConditionTree {
    /**
     * 子节点
     */
    private List<ConditionTree> nodes;

    /**
     * 开始位置
     */
    private int start;

    /**
     * 结束位置
     */
    private int end;

    /**
     * 类型
     */
    private ConditionTreeType type;

    /**
     * 条件
     */
    private String conditionString;

    /**
     * 出错信息
     */
    private String errorMessage;

    ConditionTree() {
    }

    public List<ConditionTree> getNodes() {
        return nodes;
    }

    void setNodes(List<ConditionTree> nodes) {
        this.nodes = nodes;
    }

    public int getStart() {
        return start;
    }

    void setStart(int start) {
        this.start = start;
    }

    public int getEnd() {
        return end;
    }

    void setEnd(int end) {
        this.end = end;
    }

    public ConditionTreeType getType() {
        return type;
    }

    void setType(ConditionTreeType type) {
        this.type = type;
    }

    public String getConditionString() {
        return conditionString;
    }

    void setConditionString(String conditionString) {
        this.conditionString = conditionString;
    }

    public String getErrorMessage() {
        return errorMessage;
    }

    void setErrorMessage(String errorMessage) {
        this.errorMessage = errorMessage;
    }
}
