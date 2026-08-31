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
     * 外面是否有括号
     */
    public boolean HasBracket;

    /**
     * 出错信息
     */
    public String ErrorMessage;

    private String _source;
    private String _text;

    /**
     * 文本（懒加载，返回原始表达式中 [Start, End] 区间的子串）
     */
    public String getText() {
        if (_text == null && _source != null && End >= Start) {
            _text = _source.substring(Start, End + 1);
        }
        return _text;
    }

    public void setSource(String source) {
        _source = source;
    }

    public CalculateTree() {
        Type = CalculateTreeType.String;
    }
}
