package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.ParserRuleContext;
import org.antlr.v4.runtime.tree.RuleNode;

import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.math.mathBaseVisitor;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ExprContext;

import java.util.ArrayList;
import java.util.List;

public final class MathSplitVisitor extends mathBaseVisitor<ConditionTree> {
    private boolean hasBracket = false;

    @Override
    public ConditionTree visitProg(mathParser.ProgContext context) {
        hasBracket = false;
        return context.expr().accept(this);
    }

    @Override
    public ConditionTree visitAndOr_fun(mathParser.AndOr_funContext context) {
        ConditionTree tree = new ConditionTree();
        tree.Nodes = new ArrayList<>(2);
        tree.HasBracket = hasBracket;
        hasBracket = false;
        String t = context.op.getText();
        if (CharUtil.Equals(t, "&&")) {
            tree.Type = ConditionTreeType.And;
        } else {
            tree.Type = ConditionTreeType.Or;
        }
        List<ExprContext> exprs = context.expr();

        tree.Nodes.add(exprs.get(0).accept(this));
        tree.Nodes.add(exprs.get(1).accept(this));
        tree.Start = context.getStart().getStartIndex();
        tree.End = context.getStop().getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public ConditionTree visitBracket_fun(mathParser.Bracket_funContext context) {
        hasBracket = true;
        return context.expr().accept(this);
    }

    @Override
    public ConditionTree visitChildren(RuleNode node) {
        ParserRuleContext context = (ParserRuleContext) node;
        ConditionTree tree = new ConditionTree();
        tree.Start = context.getStart().getStartIndex();
        tree.End = context.getStop().getStopIndex();
        tree.Text = context.getText();
        return tree;
    }
}
