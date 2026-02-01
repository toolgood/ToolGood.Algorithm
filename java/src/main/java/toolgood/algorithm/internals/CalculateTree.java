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
    public List<CalculateTree> Nodes;
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
    public CalculateTreeType Type;

    /**
     * 条件
     */
    public String ConditionString;

    /**
     * 出错信息
     */
    public String ErrorMessage;

  
  
}
