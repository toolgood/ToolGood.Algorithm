package toolgood.algorithm.fast;

import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.DiyNameKeyInfo;
import toolgood.algorithm.internals.functions.FunctionBase;

/**
 * 手写递归下降语法分析器，等价替代 ANTLR 生成的 {@code mathParser}。
 * <p>
 * {@code math.g4} 中 {@code expr} 是左递归规则，ANTLR 会把它改写成带优先级循环的实现；
 * 这里用经典的「优先级爬升（precedence climbing）」还原同一套优先级，语义与文法一致。
 * </p>
 * <p>优先级由高到低：{@code 函数调用/下标访问} &gt; {@code !} &gt; {@code %} &gt; {@code * / %}
 * &gt; {@code + - &} &gt; {@code > >= < <=} &gt; {@code != =} &gt; {@code &&}
 * &gt; {@code ||} &gt; {@code ? :}</p>
 */
public final class FastParser {
    private static final int PRECEDENCE_UNARY = 14;
    private static final int PRECEDENCE_PERCENTAGE = 13;
    private static final int PRECEDENCE_MUL_DIV = 12;
    private static final int PRECEDENCE_ADD_SUB = 11;
    private static final int PRECEDENCE_JUDGE = 10;
    private static final int PRECEDENCE_EQUALITY = 9;
    private static final int PRECEDENCE_AND = 8;
    private static final int PRECEDENCE_OR = 7;
    private static final int PRECEDENCE_CONDITIONAL = 6;

    /**
     * 逻辑嵌套上限（兜底，防止异常公式构造超大语法树）。
     * <p>Java 的 {@link StackOverflowError} 是可以捕获的，因此真实栈安全由入口方法兜底转换，
     * 这里只做纯逻辑上限。</p>
     */
    private static final int MAX_DEPTH = 5000;

    private static final int MIN_CAPACITY = 16;

    private final String text;

    private int[] tokenTypes;
    private int[] tokenStarts;
    private int[] tokenEnds;
    private int tokenCount;
    private int index;
    private int lastEnd;

    private final List<FastNode> nodes;
    private int[] scratch;
    private int scratchCount;

    private int depth;
    private int root;

    private FastParser(String text) {
        this.text = text;
        int capacity = text.length() / 4 + MIN_CAPACITY;
        this.tokenTypes = new int[capacity];
        this.tokenStarts = new int[capacity];
        this.tokenEnds = new int[capacity];
        this.nodes = new ArrayList<>(capacity);
        this.scratch = new int[MIN_CAPACITY];
        this.root = -1;
    }

    // ------------------------------------------------------------------ 对外入口

    /** 解析为可执行的函数树 */
    public static FunctionBase parseFunction(String text) {
        FastParser parser = new FastParser(text);
        parser.run();
        return FastAstBuilder.build(text, parser.nodes, parser.root);
    }

    /** 仅检查语法（不构造函数树） */
    public static void checkSyntax(String text) {
        new FastParser(text).run();
    }

    /** 解析为条件树 */
    public static ConditionTree parseConditionTree(String text) {
        FastParser parser = new FastParser(text);
        parser.run();
        return FastTreeBuilder.toConditionTree(text, parser.nodes, parser.root);
    }

    /** 解析为计算树 */
    public static CalculateTree parseCalculateTree(String text) {
        FastParser parser = new FastParser(text);
        parser.run();
        return FastTreeBuilder.toCalculateTree(text, parser.nodes, parser.root);
    }

    /** 解析并收集自定义参数 / 自定义函数 */
    public static void parseDiyNames(String text, List<DiyNameKeyInfo> parameters, List<DiyNameKeyInfo> functions) {
        FastParser parser = new FastParser(text);
        parser.run();
        FastTreeBuilder.collectDiyNames(parser.nodes, parser.root, parameters, functions);
    }

    // ------------------------------------------------------------------ 词法

    private void run() {
        try {
            tokenize();
            root = parseProgram();
        } catch (StackOverflowError e) {
            // Java 的 StackOverflowError 可捕获：转成普通异常，避免调用方直接崩掉
            throw new FastParseException(
                    "Expression is nested too deeply for the current thread stack.", e);
        }
    }

    private void tokenize() {
        FastLexer lexer = new FastLexer(text);
        FastLexeme token = new FastLexeme();
        while (true) {
            if (!lexer.next(token)) {
                throw new FastParseException(lexer.getErrorMessage());
            }
            if (tokenCount == tokenTypes.length) {
                growTokens();
            }
            tokenTypes[tokenCount] = token.type;
            tokenStarts[tokenCount] = token.start;
            tokenEnds[tokenCount] = token.end;
            tokenCount++;
            if (token.type == FastToken.EOF) {
                return;
            }
        }
    }

    private void growTokens() {
        int size = tokenTypes.length * 2;
        int[] types = new int[size];
        int[] starts = new int[size];
        int[] ends = new int[size];
        System.arraycopy(tokenTypes, 0, types, 0, tokenCount);
        System.arraycopy(tokenStarts, 0, starts, 0, tokenCount);
        System.arraycopy(tokenEnds, 0, ends, 0, tokenCount);
        tokenTypes = types;
        tokenStarts = starts;
        tokenEnds = ends;
    }

    private void growScratch() {
        int[] next = new int[scratch.length * 2];
        System.arraycopy(scratch, 0, next, 0, scratchCount);
        scratch = next;
    }

    private int addNode(FastNode node) {
        nodes.add(node);
        return nodes.size() - 1;
    }

    private void pushScratch(int node) {
        if (scratchCount == scratch.length) {
            growScratch();
        }
        scratch[scratchCount++] = node;
    }

    // ------------------------------------------------------------------ 语法

    private int parseProgram() {
        int node = parseExpr(0);
        if (currentType() != FastToken.EOF) {
            throw unexpected();
        }
        return node;
    }

    /**
     * 优先级爬升主循环。
     * <p>
     * 为了在有限的线程栈上支持尽可能深的嵌套，这里刻意把「括号/前缀/后缀」直接内联，
     * 并把二元运算循环挪到 {@link #parseBinaryRest(int, int)}，于是每层括号嵌套只压 1 个栈帧。
     * </p>
     */
    private int parseExpr(int minPrecedence) {
        if (++depth > MAX_DEPTH) {
            throw new FastParseException(
                    "Expression is nested too deeply (more than " + MAX_DEPTH + " levels).");
        }

        int left;
        int type = currentType();
        if (type == FastToken.BANG) {
            int tokenStart = currentStart();
            next();
            int operand = parseExpr(PRECEDENCE_UNARY);
            left = addNode(FastNode.unary(FastNodeKind.Not, operand, tokenStart, nodes.get(operand).end));
        } else if (type == FastToken.LPAREN) {
            // 括号在这里直接递归，少一个 parsePrimary 栈帧
            int bracketStart = currentStart();
            next();
            int inner = parseExpr(0);
            expect(FastToken.RPAREN);
            // 括号节点保留括号本身的区间，条件树/计算树的文本才能带上首尾括号
            left = addNode(FastNode.unary(FastNodeKind.Bracket, inner, bracketStart, lastEnd));
        } else {
            left = parsePrimary();
            left = parsePostfix(left);
        }

        left = parseBinaryRest(left, minPrecedence);
        depth--;
        return left;
    }

    /** 二元/三目运算循环（优先级爬升） */
    private int parseBinaryRest(int left, int minPrecedence) {
        while (true) {
            int type = currentType();
            boolean postfixPercentage = false;
            int precedence = binaryPrecedence(type);
            if (type == FastToken.OPMOD && !canStartExpression(index + 1)) {
                precedence = PRECEDENCE_PERCENTAGE;
                postfixPercentage = true;
            }
            if (precedence == 0 || precedence < minPrecedence) {
                return left;
            }

            int leftStart = nodes.get(left).start;
            next();
            if (postfixPercentage) {
                left = addNode(FastNode.unary(FastNodeKind.Percentage, left, leftStart, lastEnd));
                continue;
            }
            if (type == FastToken.QUESTION) {
                // 与 ANTLR 生成代码保持一致：'expr ? expr : expr' 未声明 assoc，
                // ANTLR 按左结合生成 —— 中间项按 0 级解析、末项按 prec+1 解析；
                // 因此 a?b:c?d:e 等价于 If(If(a,b,c),d,e)，而 a?b?c:d:e 等价于 If(a, If(b,c,d), e)
                int whenTrue = parseExpr(0);
                expect(FastToken.COLON);
                int whenFalse = parseExpr(precedence + 1);
                left = addNode(FastNode.three(FastNodeKind.If, left, whenTrue, whenFalse,
                        leftStart, nodes.get(whenFalse).end));
                continue;
            }
            int right = parseExpr(precedence + 1);
            left = addNode(FastNode.binary(binaryKind(type), left, right, leftStart, nodes.get(right).end));
        }
    }

    /** 后缀链：函数调用风格的 .name(...) 与下标/属性访问 */
    private int parsePostfix(int left) {
        while (true) {
            if (currentType() == FastToken.DOT) {
                left = parseMemberAccess(left);
            } else if (currentType() == FastToken.LBRACKET) {
                int leftStart = nodes.get(left).start;
                next();
                int indexNode = parseExpr(0);
                expect(FastToken.RBRACKET);
                left = addNode(FastNode.binary(FastNodeKind.GetJson, left, indexNode, leftStart, lastEnd));
            } else {
                return left;
            }
        }
    }

    private int parsePrimary() {
        int type = currentType();
        switch (type) {
            case FastToken.NUM: {
                int start = currentStart();
                int end = currentEnd();
                next();
                return addNode(FastNode.leaf(FastNodeKind.Number, start, end, null));
            }
            case FastToken.OPSUB: {
                // 文法只允许 OPSUB? NUM 形式的负数字面量
                int start = currentStart();
                next();
                if (currentType() != FastToken.NUM) {
                    throw unexpected();
                }
                int end = currentEnd();
                next();
                return addNode(FastNode.leaf(FastNodeKind.Number, start, end, null));
            }
            case FastToken.STRING: {
                int start = currentStart();
                int end = currentEnd();
                next();
                return addNode(FastNode.leaf(FastNodeKind.Text, start, end, null));
            }
            case FastToken.PARAMETER: {
                int start = currentStart();
                int end = currentEnd();
                String name = slice(start, end);
                next();
                if (currentType() == FastToken.LPAREN) {
                    return parseCall(type, name, start, end, -1, false);
                }
                return addNode(FastNode.leaf(FastNodeKind.Parameter, start, end, name));
            }
            case FastToken.TRUE:
            case FastToken.FALSE:
            case FastToken.NULL:
            case FastToken.ALGORITHMVERSION: {
                int start = currentStart();
                next();
                // ('(' ')')?  允许 TRUE() 这种写法
                if (currentType() == FastToken.LPAREN && peekType(1) == FastToken.RPAREN) {
                    next();
                    next();
                }
                return newCall(type, start, lastEnd, null, 0, 0, 0);
            }
            case FastToken.RAND:
            case FastToken.NOW:
            case FastToken.TODAY:
            case FastToken.GUID:
            case FastToken.E:
            case FastToken.PI: {
                int start = currentStart();
                next();
                expect(FastToken.LPAREN);
                expect(FastToken.RPAREN);
                return newCall(type, start, lastEnd, null, 0, 0, 0);
            }
            case FastToken.LBRACE:
                return parseArrayJson();
            case FastToken.LBRACKET:
                return parseArray();
            default:
                if (type > 0 && type <= FastToken.PARAMETER && FastToken.IsFunctionHead[type]) {
                    int start = currentStart();
                    int end = currentEnd();
                    String name = slice(start, end);
                    next();
                    return parseCall(type, name, start, end, -1, false);
                }
                throw unexpected();
        }
    }

    /** 构造一个函数调用节点，参数来自暂存区 [offset, offset+count) */
    private int newCall(int type, int start, int end, String name, int offset, int count, int nameStart) {
        FastNode node = FastNode.nary(FastNodeKind.Call, scratch, offset, count, start, end, name);
        node.type = type;
        node.nameStart = nameStart;
        return addNode(node);
    }

    /** 解析 {@code (a, b, ...)}；{@code receiver} 非 -1 时为 {@code expr.name(...)} 形式 */
    private int parseCall(int type, String name, int nameStart, int nameEnd, int receiver, boolean memberStyle) {
        expect(FastToken.LPAREN);
        int mark = scratchCount;
        if (receiver >= 0) {
            pushScratch(receiver);
        }

        if (currentType() == FastToken.RPAREN) {
            // 文法中 NAME '(' expr (',' expr)* ')' 至少需要一个参数；
            // 而 expr.NAME '(' (expr (',' expr)*)? ')' 允许无参
            if (!memberStyle) {
                throw new FastParseException(
                        "Function '" + name + "' requires at least one argument.");
            }
        } else {
            pushScratch(parseExpr(0));
            while (currentType() == FastToken.COMMA) {
                next();
                pushScratch(parseExpr(0));
            }
        }
        expect(FastToken.RPAREN);

        int start = receiver >= 0 ? nodes.get(receiver).start : nameStart;
        boolean isDiy = type == FastToken.PARAMETER;
        int node = newCall(type, start, lastEnd, isDiy ? name : null, mark, scratchCount - mark, nameStart);
        scratchCount = mark;
        return node;
    }

    /** 解析 {@code expr.name} / {@code expr.name(...)} */
    private int parseMemberAccess(int receiver) {
        next(); // '.'
        int type = currentType();
        int nameStart = currentStart();
        int nameEnd = currentEnd();
        String name = slice(nameStart, nameEnd);
        if (type == FastToken.PARAMETER) {
            next();
            if (currentType() == FastToken.LPAREN) {
                return parseCall(type, name, nameStart, nameEnd, receiver, true);
            }
            return addNode(FastNode.unary(FastNodeKind.GetJsonProperty, receiver,
                    nodes.get(receiver).start, nameEnd).withText(name));
        }
        if (type > 0 && type <= FastToken.PARAMETER) {
            if (FastToken.IsMethodHead[type] && peekType(1) == FastToken.LPAREN) {
                next();
                return parseCall(type, name, nameStart, nameEnd, receiver, true);
            }
            if (FastToken.IsParameter2[type]) {
                next();
                return addNode(FastNode.unary(FastNodeKind.GetJsonProperty, receiver,
                        nodes.get(receiver).start, nameEnd).withText(name));
            }
        }
        throw unexpected();
    }

    private int parseArray() {
        int start = currentStart();
        next(); // '['
        int mark = scratchCount;
        pushScratch(parseExpr(0));
        while (currentType() == FastToken.COMMA) {
            next();
            if (currentType() == FastToken.RBRACKET) {
                break; // 允许尾逗号
            }
            pushScratch(parseExpr(0));
        }
        expect(FastToken.RBRACKET);
        int node = addNode(FastNode.nary(FastNodeKind.Array, scratch, mark, scratchCount - mark,
                start, lastEnd, null));
        scratchCount = mark;
        return node;
    }

    private int parseArrayJson() {
        int start = currentStart();
        next(); // '{'
        int mark = scratchCount;
        while (true) {
            pushScratch(parseArrayJsonItem());
            if (currentType() != FastToken.COMMA) {
                break;
            }
            next();
            if (currentType() == FastToken.RBRACE) {
                break; // 允许尾逗号
            }
        }
        expect(FastToken.RBRACE);
        int node = addNode(FastNode.nary(FastNodeKind.ArrayJson, scratch, mark, scratchCount - mark,
                start, lastEnd, null));
        scratchCount = mark;
        return node;
    }

    private int parseArrayJsonItem() {
        int type = currentType();
        if (type != FastToken.STRING && type != FastToken.NUM
                && !(type > 0 && type <= FastToken.PARAMETER && FastToken.IsParameter2[type])) {
            throw unexpected();
        }
        int keyStart = currentStart();
        int keyEnd = currentEnd();
        String keyText = slice(keyStart, keyEnd); // STRING 保留引号，由 builder 统一反转义
        next();
        expect(FastToken.COLON);
        int value = parseExpr(0);
        return addNode(FastNode.unary(FastNodeKind.ArrayJsonItem, value, keyStart,
                nodes.get(value).end).withText(keyText));
    }

    // ------------------------------------------------------------------ 辅助

    private int currentType() {
        return tokenTypes[index];
    }

    private int currentStart() {
        return tokenStarts[index];
    }

    private int currentEnd() {
        return tokenEnds[index];
    }

    private int peekType(int offset) {
        int i = index + offset;
        return i < tokenCount ? tokenTypes[i] : tokenTypes[tokenCount - 1];
    }

    private void next() {
        lastEnd = tokenEnds[index];
        if (index + 1 < tokenCount) {
            index++;
        }
    }

    private void expect(int type) {
        if (currentType() != type) {
            throw unexpected();
        }
        next();
    }

    private String slice(int start, int end) {
        if (end < start) {
            return "";
        }
        return text.substring(start, end + 1);
    }

    private FastParseException unexpected() {
        int type = currentType();
        if (type == FastToken.EOF) {
            return new FastParseException(
                    "Unexpected end of expression at position " + text.length() + ".");
        }
        return new FastParseException(
                "Unexpected token '" + slice(currentStart(), currentEnd()) + "' at position " + currentStart() + ".");
    }

    /** 判断从 tokenIndex 开始的词法单元能否作为一个表达式的开头 */
    private boolean canStartExpression(int tokenIndex) {
        if (tokenIndex >= tokenCount) {
            return false;
        }
        int type = tokenTypes[tokenIndex];
        switch (type) {
            case FastToken.LPAREN:
            case FastToken.NUM:
            case FastToken.STRING:
            case FastToken.PARAMETER:
            case FastToken.BANG:
            case FastToken.LBRACE:
            case FastToken.LBRACKET:
            case FastToken.TRUE:
            case FastToken.FALSE:
            case FastToken.NULL:
            case FastToken.ALGORITHMVERSION:
            case FastToken.RAND:
            case FastToken.NOW:
            case FastToken.TODAY:
            case FastToken.GUID:
            case FastToken.E:
            case FastToken.PI:
                return true;
            case FastToken.OPSUB:
                // 负号只能引导数字字面量
                return tokenIndex + 1 < tokenCount && tokenTypes[tokenIndex + 1] == FastToken.NUM;
            default:
                return type > 0 && type <= FastToken.PARAMETER && FastToken.IsFunctionHead[type];
        }
    }

    private static int binaryPrecedence(int type) {
        switch (type) {
            case FastToken.OPMOD:
            case FastToken.OPMUL:
            case FastToken.OPDIV:
                return PRECEDENCE_MUL_DIV;
            case FastToken.OPADD:
            case FastToken.OPSUB:
            case FastToken.OPCAT:
                return PRECEDENCE_ADD_SUB;
            case FastToken.OPGT:
            case FastToken.OPGE:
            case FastToken.OPLT:
            case FastToken.OPLE:
                return PRECEDENCE_JUDGE;
            case FastToken.OPNE:
            case FastToken.OPEQ:
                return PRECEDENCE_EQUALITY;
            case FastToken.OPAND:
                return PRECEDENCE_AND;
            case FastToken.OPOR:
                return PRECEDENCE_OR;
            case FastToken.QUESTION:
                return PRECEDENCE_CONDITIONAL;
            default:
                return 0;
        }
    }

    private static FastNodeKind binaryKind(int type) {
        switch (type) {
            case FastToken.OPMUL:
                return FastNodeKind.Mul;
            case FastToken.OPDIV:
                return FastNodeKind.Div;
            case FastToken.OPMOD:
                return FastNodeKind.Mod;
            case FastToken.OPADD:
                return FastNodeKind.Add;
            case FastToken.OPSUB:
                return FastNodeKind.Sub;
            case FastToken.OPCAT:
                return FastNodeKind.Concat;
            case FastToken.OPGT:
                return FastNodeKind.GreaterThan;
            case FastToken.OPGE:
                return FastNodeKind.GreaterOrEqual;
            case FastToken.OPLT:
                return FastNodeKind.LessThan;
            case FastToken.OPLE:
                return FastNodeKind.LessOrEqual;
            case FastToken.OPEQ:
                return FastNodeKind.Equal;
            case FastToken.OPNE:
                return FastNodeKind.NotEqual;
            case FastToken.OPAND:
                return FastNodeKind.And;
            case FastToken.OPOR:
                return FastNodeKind.Or;
            default:
                return FastNodeKind.Add;
        }
    }
}
