package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.ParserRuleContext;
import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import org.antlr.v4.runtime.tree.RuleNode;

import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.math.mathBaseVisitor;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.*;

import java.util.ArrayList;
import java.util.Arrays;

public class MathSplitVisitor extends mathBaseVisitor<ConditionTree> {
    private boolean hasBracket = false;

    @Override
    public ConditionTree visitProg(mathParser.ProgContext context) {
        hasBracket = false;
        return visit(context.expr());
    }

    @Override
    public ConditionTree visitOr_fun(Or_funContext context) {
        ConditionTree tree = new ConditionTree();
        tree.HasBracket = hasBracket;
        hasBracket = false;
        ConditionTree f1 = visit(context.expr(0));
        ConditionTree f2 = visit(context.expr(1));
        tree.Nodes = new ArrayList<>(Arrays.asList(f1, f2));
        tree.Type = ConditionTreeType.Or;
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public ConditionTree visitAnd_fun(And_funContext context) {
        ConditionTree tree = new ConditionTree();
        tree.HasBracket = hasBracket;
        hasBracket = false;
        ConditionTree f1 = visit(context.expr(0));
        ConditionTree f2 = visit(context.expr(1));
        tree.Nodes = new ArrayList<>(Arrays.asList(f1, f2));
        tree.Type = ConditionTreeType.And;
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public ConditionTree visitBracket_fun(Bracket_funContext context) {
        hasBracket = true;
        return visit(context.expr());
    }

    @Override
    public ConditionTree visitParameter2(mathParser.Parameter2Context ctx) {
        return visitChildren(ctx);
    }

    @Override
    public ConditionTree visitArrayJson(mathParser.ArrayJsonContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public ConditionTree visitChildren(RuleNode node) {
        ParserRuleContext context = (ParserRuleContext) node;
        ConditionTree tree = new ConditionTree();
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }
}
