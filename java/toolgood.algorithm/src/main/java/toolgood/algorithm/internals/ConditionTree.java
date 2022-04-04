package toolgood.algorithm.internals;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser2;

import java.util.ArrayList;
import java.util.List;

public class ConditionTree {
    public List<ConditionTree> Nodes;

    public int Start;
    public int End;

    public ConditionTreeType Type;

    public String ErrorMessage;

    public ConditionTree() {
        Nodes = new ArrayList<>();
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
            final mathParser2.ProgContext context = parser.prog();

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
