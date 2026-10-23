package toolgood.algorithm.fast;

import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.DiyNameKeyInfo;

/**
 * 语法树到 {@link ConditionTree} / {@link CalculateTree} 的转换，以及自定义名称收集。
 * 等价于原来的 MathSplitVisitor / MathSplitVisitor2 / DiyNameVisitor。
 */
final class FastTreeBuilder {
    /** 条件树/计算树允许的最大嵌套层数（逻辑上限） */
    private static final int MAX_TREE_DEPTH = 2000;

    private FastTreeBuilder() {
    }

    // ------------------------------------------------------------------ ConditionTree

    /**
     * 只有 And / Or 会拆分成节点，其余整体作为文本叶子；
     * 括号本身不产生节点，而是标记被包裹节点的 {@link ConditionTree#HasBracket}。
     */
    static ConditionTree toConditionTree(String source, List<FastNode> nodes, int root) {
        try {
            return buildCondition(source, nodes, nodes.get(root), 0);
        } catch (StackOverflowError e) {
            throw new IllegalArgumentException(
                    "Condition is nested too deeply for the current thread stack.");
        }
    }

    private static ConditionTree buildCondition(String source, List<FastNode> nodes, FastNode node, int depth) {
        if (++depth > MAX_TREE_DEPTH) {
            throw new IllegalArgumentException(
                    "Condition is nested too deeply (more than " + MAX_TREE_DEPTH + " levels).");
        }
        ConditionTreeType type;
        switch (node.kind) {
            case Bracket: {
                ConditionTree inner = buildCondition(source, nodes, nodes.get(node.child(0)), depth);
                inner.HasBracket = true;
                return inner;
            }
            case And:
                type = ConditionTreeType.And;
                break;
            case Or:
                type = ConditionTreeType.Or;
                break;
            default: {
                ConditionTree leaf = new ConditionTree();
                leaf.Start = node.start;
                leaf.End = node.end;
                leaf.setSource(source);
                return leaf;
            }
        }

        ConditionTree tree = new ConditionTree();
        tree.Start = node.start;
        tree.End = node.end;
        tree.Type = type;
        tree.setSource(source);
        tree.HasBracket = false;
        List<ConditionTree> children = new ArrayList<>(2);
        children.add(buildCondition(source, nodes, nodes.get(node.child(0)), depth));
        children.add(buildCondition(source, nodes, nodes.get(node.child(1)), depth));
        tree.Nodes = children;
        return tree;
    }

    // ------------------------------------------------------------------ CalculateTree

    static CalculateTree toCalculateTree(String source, List<FastNode> nodes, int root) {
        try {
            return buildCalculate(source, nodes, nodes.get(root), 0);
        } catch (StackOverflowError e) {
            throw new IllegalArgumentException(
                    "Expression is nested too deeply for the current thread stack.");
        }
    }

    private static CalculateTree buildCalculate(String source, List<FastNode> nodes, FastNode node, int depth) {
        if (++depth > MAX_TREE_DEPTH) {
            throw new IllegalArgumentException(
                    "Expression is nested too deeply (more than " + MAX_TREE_DEPTH + " levels).");
        }
        switch (node.kind) {
            case Bracket: {
                CalculateTree inner = buildCalculate(source, nodes, nodes.get(node.child(0)), depth);
                inner.HasBracket = true;
                return inner;
            }
            case Mul:
            case Div:
            case Mod:
            case Add:
            case Sub:
            case Concat:
            case GreaterThan:
            case GreaterOrEqual:
            case LessThan:
            case LessOrEqual:
            case Equal:
            case NotEqual:
            case And:
            case Or: {
                CalculateTree tree = new CalculateTree();
                tree.Start = node.start;
                tree.End = node.end;
                tree.Type = toCalculateTreeType(node.kind);
                tree.setSource(source);
                List<CalculateTree> children = new ArrayList<>(2);
                children.add(buildCalculate(source, nodes, nodes.get(node.child(0)), depth));
                children.add(buildCalculate(source, nodes, nodes.get(node.child(1)), depth));
                tree.Nodes = children;
                return tree;
            }
            default: {
                CalculateTree leaf = new CalculateTree();
                leaf.Start = node.start;
                leaf.End = node.end;
                leaf.setSource(source);
                return leaf;
            }
        }
    }

    private static CalculateTreeType toCalculateTreeType(FastNodeKind kind) {
        switch (kind) {
            case Mul:
                return CalculateTreeType.Mul;
            case Div:
                return CalculateTreeType.Div;
            case Mod:
                return CalculateTreeType.Mod;
            case Add:
                return CalculateTreeType.Add;
            case Sub:
                return CalculateTreeType.Sub;
            case Concat:
                return CalculateTreeType.Connect;
            case GreaterThan:
                return CalculateTreeType.OpGt;
            case GreaterOrEqual:
                return CalculateTreeType.OpGe;
            case LessThan:
                return CalculateTreeType.OpLt;
            case LessOrEqual:
                return CalculateTreeType.OpLe;
            case Equal:
                return CalculateTreeType.OpEq;
            case NotEqual:
                return CalculateTreeType.OpNe;
            case And:
                return CalculateTreeType.And;
            default:
                return CalculateTreeType.Or;
        }
    }

    // ------------------------------------------------------------------ DiyName

    /**
     * 收集自定义参数与自定义函数。
     * <p>规则与 DiyNameVisitor 一致：PARAMETER 记为参数，自定义函数名记为函数，
     * {@code expr.关键字} 形式的关键字不计入。采用显式栈遍历，长链公式不会把栈打爆。</p>
     */
    static void collectDiyNames(List<FastNode> nodes, int root,
                                List<DiyNameKeyInfo> parameters, List<DiyNameKeyInfo> functions) {
        int[] stack = new int[16];
        int top = 0;
        stack[top++] = root;
        while (top > 0) {
            FastNode node = nodes.get(stack[--top]);
            if (node.kind == FastNodeKind.Parameter) {
                parameters.add(createInfo(node.text, node.start, node.end));
            } else if (node.kind == FastNodeKind.Call && node.type == FastToken.PARAMETER) {
                functions.add(createInfo(node.text, node.nameStart, node.nameStart + node.text.length() - 1));
            }
            // 逆序压栈，保证子节点按原文从左到右的顺序被访问
            for (int i = node.childCount() - 1; i >= 0; i--) {
                if (top == stack.length) {
                    int[] bigger = new int[stack.length * 2];
                    System.arraycopy(stack, 0, bigger, 0, top);
                    stack = bigger;
                }
                stack[top++] = node.child(i);
            }
        }
    }

    private static DiyNameKeyInfo createInfo(String name, int start, int end) {
        DiyNameKeyInfo info = new DiyNameKeyInfo();
        info.Name = name;
        info.Start = start;
        info.End = end;
        return info;
    }
}
