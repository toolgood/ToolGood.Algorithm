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

    public ConditionTree() {
        Type = ConditionTreeType.String;
    }
}
