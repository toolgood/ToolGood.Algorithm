package toolgood.algorithm.internals;

import java.util.List;
import toolgood.algorithm.enums.ConditionTreeType;

/**
 * 条件树
 */
public final class ConditionTree {
    /**
     * 子节点
     */
    public List<ConditionTree> Nodes;

    /**
     * 开始位置
     */
    public int Start;

    /**
     * 结束位置
     */
    public int End;

    /**
     * 类型
     */
    public ConditionTreeType Type;

    /**
     * 文本
     */
    public String Text;

    /**
     * 外面是否有括号
     */
    public boolean HasBracket;

    /**
     * 出错信息
     */
    public String ErrorMessage;

    public ConditionTree() {
        Type = ConditionTreeType.String;
    }
}
