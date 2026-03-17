package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.ParserRuleContext;
import org.antlr.v4.runtime.tree.RuleNode;

import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.math.mathBaseVisitor;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ExprContext;

import java.util.ArrayList;
import java.util.List;

public final class MathSplitVisitor2 extends mathBaseVisitor<CalculateTree> {
    private boolean hasBracket = false;

    @Override
    public CalculateTree visitProg(mathParser.ProgContext context) {
        hasBracket = false;
        return context.expr().accept(this);
    }

    @Override
    public CalculateTree visitBracket_fun(mathParser.Bracket_funContext context) {
        hasBracket = true;
        return context.expr().accept(this);
    }

    @Override
    public CalculateTree visitMulDiv_fun(mathParser.MulDiv_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.Nodes = new ArrayList<>(2);
        tree.HasBracket = hasBracket;
        hasBracket = false;
        List<ExprContext> exprs = context.expr();
        String t = context.op.getText();
        if (CharUtil.Equals(t, '*')) {
            tree.Type = CalculateTreeType.Mul;
        } else if (CharUtil.Equals(t, '/')) {
            tree.Type = CalculateTreeType.Div;
        } else {
            tree.Type = CalculateTreeType.Mod;
        }
        tree.Nodes.add(exprs.get(0).accept(this));
        tree.Nodes.add(exprs.get(1).accept(this));
        tree.Start = context.getStart().getStartIndex();
        tree.End = context.getStop().getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitAddSub_fun(mathParser.AddSub_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.Nodes = new ArrayList<>(2);
        tree.HasBracket = hasBracket;
        hasBracket = false;
        List<ExprContext> exprs = context.expr();
        String t = context.op.getText();
        if (CharUtil.Equals(t, '+')) {
            tree.Type = CalculateTreeType.Add;
        } else if (CharUtil.Equals(t, '-')) {
            tree.Type = CalculateTreeType.Sub;
        } else {
            tree.Type = CalculateTreeType.Connect;
        }
        tree.Nodes.add(exprs.get(0).accept(this));
        tree.Nodes.add(exprs.get(1).accept(this));
        tree.Start = context.getStart().getStartIndex();
        tree.End = context.getStop().getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitChildren(RuleNode node) {
        ParserRuleContext context = (ParserRuleContext) node;
        CalculateTree tree = new CalculateTree();
        tree.Start = context.getStart().getStartIndex();
        tree.End = context.getStop().getStopIndex();
        tree.Text = context.getText();
        return tree;
    }
}
