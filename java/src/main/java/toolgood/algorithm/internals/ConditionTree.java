/**
 * 条件�?
 */
package toolgood.algorithm.internals;

import java.util.List;
import toolgood.algorithm.enums.ConditionTreeType;

public final class ConditionTree {
    /**
     * 子节�?
     */
    public List<ConditionTree> Nodes;
    /**
     * 开始位�?
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
     * 条件
     */
    public String ConditionString;

    /**
     * 出错信息
     */
    public String ErrorMessage;

 
}
