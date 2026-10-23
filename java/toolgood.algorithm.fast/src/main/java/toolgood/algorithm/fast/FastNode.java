package toolgood.algorithm.fast;

/**
 * 手写语法分析生成的语法树节点种类。
 */
enum FastNodeKind {
    /** 数值字面量（可能带负号或单位后缀） */
    Number,
    /** 字符串字面量（含首尾引号） */
    Text,
    /** 参数名 */
    Parameter,
    /** 括号包裹 */
    Bracket,
    /** 逻辑非 */
    Not,
    /** 百分号后缀 */
    Percentage,

    Mul,
    Div,
    Mod,
    Add,
    Sub,
    Concat,
    GreaterThan,
    GreaterOrEqual,
    LessThan,
    LessOrEqual,
    Equal,
    NotEqual,
    And,
    Or,
    /** 三目运算 */
    If,

    /** 函数调用，type 为函数名 Token */
    Call,
    /** [a, b, c] */
    Array,
    /** {k:v, ...} */
    ArrayJson,
    /** json 中的一项 */
    ArrayJsonItem,
    /** expr[index] */
    GetJson,
    /** expr.name */
    GetJsonProperty,
}

/**
 * 语法树节点。节点之间用下标互相引用（下标指向 {@link FastParser} 里的节点表），
 * 因此整棵树只有「节点对象」这一层，不再有 ANTLR 的 ParserRuleContext 层级与虚拟分派。
 */
final class FastNode {
    final FastNodeKind kind;
    /** 函数名 Token 类型（仅 Call 使用） */
    int type;
    /** 原文起始下标 */
    final int start;
    /** 原文结束下标（闭区间） */
    final int end;
    /** 参数名 / 自定义函数名 / json key */
    String text;
    /** 自定义函数名 Token 的起始下标（仅 Call + PARAMETER 使用，收集 DIY 名称时需要） */
    int nameStart;
    /** 子节点在节点表中的下标 */
    final int[] children;

    private FastNode(FastNodeKind kind, int start, int end, int[] children) {
        this.kind = kind;
        this.start = start;
        this.end = end;
        this.children = children;
    }

    int childCount() {
        return children == null ? 0 : children.length;
    }

    int child(int index) {
        return children[index];
    }

    FastNode withText(String text) {
        this.text = text;
        return this;
    }

    static FastNode leaf(FastNodeKind kind, int start, int end, String text) {
        FastNode node = new FastNode(kind, start, end, null);
        node.text = text;
        return node;
    }

    static FastNode unary(FastNodeKind kind, int child, int start, int end) {
        return new FastNode(kind, start, end, new int[]{child});
    }

    static FastNode binary(FastNodeKind kind, int left, int right, int start, int end) {
        return new FastNode(kind, start, end, new int[]{left, right});
    }

    static FastNode three(FastNodeKind kind, int first, int second, int third, int start, int end) {
        return new FastNode(kind, start, end, new int[]{first, second, third});
    }

    /**
     * 从暂存区 {@code children} 的 [offset, offset+count) 区间创建多子节点。
     */
    static FastNode nary(FastNodeKind kind, int[] children, int offset, int count, int start, int end, String text) {
        int[] copy = count == 0 ? null : new int[count];
        if (count > 0) {
            System.arraycopy(children, offset, copy, 0, count);
        }
        FastNode node = new FastNode(kind, start, end, copy);
        node.text = text;
        return node;
    }
}
