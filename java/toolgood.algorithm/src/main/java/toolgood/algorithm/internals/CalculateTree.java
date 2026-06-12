package toolgood.algorithm.internals;

import java.util.List;
import toolgood.algorithm.enums.CalculateTreeType;

/**
 * 计算树
 */
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

    public CalculateTree() {
    }
}
