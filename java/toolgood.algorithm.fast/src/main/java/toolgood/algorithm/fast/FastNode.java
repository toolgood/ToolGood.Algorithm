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
 * <p>
 * 子节点下标放在 c0/c1/c2 三个内联字段里，超过 3 个才退化成数组：四则运算、比较、三目、
 * 括号这些最常见的节点只 new 一个 {@code FastNode}，不再额外分配 {@code int[]}。
 * </p>
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

    private final int c0;
    private final int c1;
    private final int c2;
    private final int count;
    /** 子节点超过 3 个时才非空，此时子节点下标全部放在这里 */
    private final int[] more;

    private FastNode(FastNodeKind kind, int start, int end, int c0, int c1, int c2, int count, int[] more) {
        this.kind = kind;
        this.start = start;
        this.end = end;
        this.c0 = c0;
        this.c1 = c1;
        this.c2 = c2;
        this.count = count;
        this.more = more;
    }

    int childCount() {
        return more != null ? more.length : count;
    }

    int child(int index) {
        if (more != null) {
            return more[index];
        }
        if (index == 1) {
            return c1;
        }
        if (index == 2) {
            return c2;
        }
        return c0;
    }

    FastNode withText(String text) {
        this.text = text;
        return this;
    }

    static FastNode leaf(FastNodeKind kind, int start, int end, String text) {
        FastNode node = new FastNode(kind, start, end, 0, 0, 0, 0, null);
        node.text = text;
        return node;
    }

    static FastNode unary(FastNodeKind kind, int child, int start, int end) {
        return new FastNode(kind, start, end, child, 0, 0, 1, null);
    }

    static FastNode binary(FastNodeKind kind, int left, int right, int start, int end) {
        return new FastNode(kind, start, end, left, right, 0, 2, null);
    }

    static FastNode three(FastNodeKind kind, int first, int second, int third, int start, int end) {
        return new FastNode(kind, start, end, first, second, third, 3, null);
    }

    /**
     * 从暂存区 {@code children} 的 [offset, offset+count) 区间创建多子节点。
     * 不超过 3 个时直接放进内联字段，避免任何数组分配。
     */
    static FastNode nary(FastNodeKind kind, int[] children, int offset, int count, int start, int end, String text) {
        FastNode node;
        if (count <= 3) {
            int a = count > 0 ? children[offset] : 0;
            int b = count > 1 ? children[offset + 1] : 0;
            int c = count > 2 ? children[offset + 2] : 0;
            node = new FastNode(kind, start, end, a, b, c, count, null);
        } else {
            int[] copy = new int[count];
            System.arraycopy(children, offset, copy, 0, count);
            node = new FastNode(kind, start, end, 0, 0, 0, count, copy);
        }
        node.text = text;
        return node;
    }
}
