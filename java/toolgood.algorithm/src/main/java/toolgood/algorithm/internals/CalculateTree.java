/**
 * 计算树
 */
package toolgood.algorithm.internals;

import java.util.ArrayList;
import java.util.List;
import toolgood.algorithm.enums.CalculateTreeType;

public final class CalculateTree {
    /**
     * 子节点
     */
    private List<CalculateTree> nodes;
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
    private CalculateTreeType type;

    /**
     * 条件
     */
    private String conditionString;

    /**
     * 出错信息
     */
    private String errorMessage;

    CalculateTree() {
    }

    public List<CalculateTree> getNodes() {
        return nodes;
    }

    void setNodes(List<CalculateTree> nodes) {
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

    public CalculateTreeType getType() {
        return type;
    }

    void setType(CalculateTreeType type) {
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
