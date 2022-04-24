package toolgood.algorithm.internals;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;

import java.util.List;

public class ConditionTree {
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
     * 条件
     */
    public String ConditionString;
    /**
     * 出错信息
     */
    public String ErrorMessage;

    public ConditionTree() {

    }


    public static ConditionTree Parse(String condition) {
        ConditionTree tree = new ConditionTree();
        if (condition == null || condition.equals("")) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = "condition 为空";
            return tree;
        }
        try {
            final AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(condition));
            final mathLexer lexer = new mathLexer(stream);
            final CommonTokenStream tokens = new CommonTokenStream(lexer);
            final mathParser parser = new mathParser(tokens);
            final AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
            parser.removeErrorListeners();
            parser.addErrorListener(antlrErrorListener);
            final mathParser.ProgContext context = parser.prog();

            if (antlrErrorListener.IsError) {
                tree.ErrorMessage = antlrErrorListener.ErrorMsg;
                return tree;
            }

            final MathSplitVisitor visitor = new MathSplitVisitor();
            return visitor.visit(context);
        } catch (Exception ex) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = ex.getMessage();
        }
        return tree;
    }

}
