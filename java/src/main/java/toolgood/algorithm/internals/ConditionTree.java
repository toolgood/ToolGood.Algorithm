package toolgood.algorithm.internals;

import java.util.List;
import toolgood.algorithm.enums.ConditionTreeType;

public final class ConditionTree {
    public List<ConditionTree> Nodes;
    public int Start;
    public int End;
    public ConditionTreeType Type;
    public String Text;
    public boolean HasBracket;
    public String ErrorMessage;
}
