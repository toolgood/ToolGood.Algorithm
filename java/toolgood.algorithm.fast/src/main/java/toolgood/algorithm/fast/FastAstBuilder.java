package toolgood.algorithm.fast;

import java.math.BigDecimal;
import java.util.List;
import java.util.Map;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.compare.Function_EQ;
import toolgood.algorithm.internals.functions.compare.Function_GE;
import toolgood.algorithm.internals.functions.compare.Function_GT;
import toolgood.algorithm.internals.functions.compare.Function_LE;
import toolgood.algorithm.internals.functions.compare.Function_LT;
import toolgood.algorithm.internals.functions.compare.Function_NE;
import toolgood.algorithm.internals.functions.flow.Function_IF;
import toolgood.algorithm.internals.functions.operator.Function_AND;
import toolgood.algorithm.internals.functions.operator.Function_Add;
import toolgood.algorithm.internals.functions.operator.Function_Connect;
import toolgood.algorithm.internals.functions.operator.Function_Div;
import toolgood.algorithm.internals.functions.operator.Function_Mod;
import toolgood.algorithm.internals.functions.operator.Function_Mul;
import toolgood.algorithm.internals.functions.flow.Function_NOT;
import toolgood.algorithm.internals.functions.operator.Function_OR;
import toolgood.algorithm.internals.functions.mathBase.Function_Percentage;
import toolgood.algorithm.internals.functions.operator.Function_Sub;
import toolgood.algorithm.internals.functions.value.Function_ARRAY;
import toolgood.algorithm.internals.functions.value.Function_ArrayJson;
import toolgood.algorithm.internals.functions.value.Function_ArrayJsonItem;
import toolgood.algorithm.internals.functions.value.Function_GetJsonValue;
import toolgood.algorithm.internals.functions.value.Function_Number;
import toolgood.algorithm.internals.functions.value.Function_Number2;
import toolgood.algorithm.internals.functions.value.Function_PARAMETER;
import toolgood.algorithm.internals.functions.value.Function_ValueText;

/**
 * 把手写语法树转换为可执行的 {@link FunctionBase} 树，等价于原来的 {@code MathFunctionVisitor}。
 * <p>
 * 采用<b>显式栈的后序遍历</b>而不是递归：{@code 1+1+1+...} 这类左深链会让递归版本的栈帧
 * 达到数千层，直接 StackOverflow 杀掉线程；显式栈的深度受堆限制，不会崩。
 * </p>
 */
final class FastAstBuilder {
    private static final FunctionBase[] EMPTY = new FunctionBase[0];

    private FastAstBuilder() {
    }

    static FunctionBase build(String text, List<FastNode> nodes, int root) {
        int size = nodes.size();
        FunctionBase[] built = new FunctionBase[size];
        int[] stackNode = new int[size + 1];
        int[] stackChild = new int[size + 1];

        int top = 0;
        stackNode[top++] = root;
        FunctionBase result = null;
        while (top > 0) {
            int nodeIndex = stackNode[top - 1];
            FastNode node = nodes.get(nodeIndex);
            int child = stackChild[top - 1];
            if (child < node.childCount()) {
                stackChild[top - 1] = child + 1;
                stackNode[top] = node.child(child);
                stackChild[top] = 0;
                top++;
                continue;
            }
            result = create(text, nodes, built, node);
            built[nodeIndex] = result;
            top--;
        }
        return result;
    }

    private static FunctionBase create(String text, List<FastNode> nodes, FunctionBase[] built, FastNode node) {
        switch (node.kind) {
            case Number:
                return buildNumber(text, node);
            case Text:
                return new Function_ValueText(Operand.Create(unescapeString(slice(text, node))));
            case Parameter:
                return new Function_PARAMETER(node.text);
            case Bracket:
                return built[node.child(0)];
            case Not:
                return new Function_NOT(built[node.child(0)]);
            case Percentage:
                return new Function_Percentage(built[node.child(0)]);

            case Mul:
                return new Function_Mul(built[node.child(0)], built[node.child(1)]);
            case Div:
                return new Function_Div(built[node.child(0)], built[node.child(1)]);
            case Mod:
                return new Function_Mod(built[node.child(0)], built[node.child(1)]);
            case Add:
                return new Function_Add(built[node.child(0)], built[node.child(1)]);
            case Sub:
                return new Function_Sub(built[node.child(0)], built[node.child(1)]);
            case Concat:
                return new Function_Connect(built[node.child(0)], built[node.child(1)]);
            case GreaterThan:
                return new Function_GT(built[node.child(0)], built[node.child(1)]);
            case GreaterOrEqual:
                return new Function_GE(built[node.child(0)], built[node.child(1)]);
            case LessThan:
                return new Function_LT(built[node.child(0)], built[node.child(1)]);
            case LessOrEqual:
                return new Function_LE(built[node.child(0)], built[node.child(1)]);
            case Equal:
                return new Function_EQ(built[node.child(0)], built[node.child(1)]);
            case NotEqual:
                return new Function_NE(built[node.child(0)], built[node.child(1)]);
            case And:
                return new Function_AND(built[node.child(0)], built[node.child(1)]);
            case Or:
                return new Function_OR(built[node.child(0)], built[node.child(1)]);
            case If:
                return new Function_IF(buildArguments(nodes, built, node));

            case Call:
                return FastFunctionFactory.create(node.type, buildArguments(nodes, built, node), node.text);
            case Array:
                return new Function_ARRAY(buildArguments(nodes, built, node));
            case ArrayJson:
                return new Function_ArrayJson(buildArguments(nodes, built, node));
            case ArrayJsonItem:
                return new Function_ArrayJsonItem(unescapeKey(node.text), built[node.child(0)]);
            case GetJson:
                return new Function_GetJsonValue(built[node.child(0)], built[node.child(1)]);
            case GetJsonProperty:
                return new Function_GetJsonValue(built[node.child(0)],
                        new Function_ValueText(Operand.Create(node.text)));
            default:
                throw new IllegalArgumentException("Node '" + node.kind + "' is not implemented.");
        }
    }

    private static FunctionBase[] buildArguments(List<FastNode> nodes, FunctionBase[] built, FastNode node) {
        int count = node.childCount();
        if (count == 0) {
            return EMPTY;
        }
        FunctionBase[] list = new FunctionBase[count];
        for (int i = 0; i < count; i++) {
            list[i] = built[node.child(i)];
        }
        return list;
    }

    private static String slice(String text, FastNode node) {
        return text.substring(node.start, node.end + 1);
    }

    private static FunctionBase buildNumber(String text, FastNode node) {
        String span = slice(text, node);
        try {
            BigDecimal d = new BigDecimal(span);
            return new Function_Number(Operand.Create(d));
        } catch (NumberFormatException ignored) {
            // 解析失败只可能是: 数值超出范围, 或数值后带单位，继续按单位切分
        }
        // 仅当后缀确实命中单位字典时才按单位切分, 否则一律视为非法数值,
        // 避免把指数记法的 'E' 或超长数字的末位误当作单位。
        // 单位字典的 key 是大写形式（ANTLR 版靠字符流归一化后才查表），这里显式转大写。
        Map<String, ?> dict = Function_Number2.GetUnitTypedict();
        for (int n = 3; n >= 1; n--) {
            if (span.length() <= n) {
                continue;
            }
            String unit = span.substring(span.length() - n);
            if (dict.containsKey(unit.toUpperCase(java.util.Locale.ROOT))) {
                String txt = span.substring(0, span.length() - n);
                BigDecimal d2;
                try {
                    d2 = new BigDecimal(txt);
                } catch (NumberFormatException e) {
                    throw new IllegalArgumentException(
                            "Number '" + span + "' is invalid or out of range.");
                }
                return new Function_Number2(d2, unit);
            }
        }
        throw new IllegalArgumentException("Number '" + span + "' is invalid or out of range.");
    }

    /** json 的 key：字符串字面量需去掉首尾引号并反转义，其余形式直接用原文 */
    private static String unescapeKey(String keyText) {
        if (keyText != null && keyText.length() > 1
                && (keyText.charAt(0) == '"' || keyText.charAt(0) == '\'' || keyText.charAt(0) == '`')) {
            return unescapeString(keyText);
        }
        return keyText;
    }

    /**
     * 将字符串字面量(含首尾引号)转为实际文本值。
     * 必须与 Operand / Function_ValueText 的转义输出保持对称，否则字面量往返会静默失真。
     */
    static String unescapeString(String text) {
        StringBuilder sb = new StringBuilder(text.length());
        int index = 1;
        int end = text.length() - 1;
        while (index < end) {
            char c = text.charAt(index++);
            if (c != '\\') {
                sb.append(c);
                continue;
            }
            char c2 = text.charAt(index++);
            switch (c2) {
                case 'n':
                    sb.append('\n');
                    break;
                case 'r':
                    sb.append('\r');
                    break;
                case 't':
                    sb.append('\t');
                    break;
                case '0':
                    sb.append('\0');
                    break;
                case 'v':
                    sb.append('\u000B');
                    break;
                case 'a':
                    sb.append('\u0007');
                    break;
                case 'b':
                    sb.append('\b');
                    break;
                case 'f':
                    sb.append('\f');
                    break;
                default:
                    sb.append(c2);
                    break;
            }
        }
        return sb.toString();
    }
}
