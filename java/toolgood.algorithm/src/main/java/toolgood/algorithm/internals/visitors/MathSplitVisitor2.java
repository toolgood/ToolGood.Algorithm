package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.ParserRuleContext;
import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import org.antlr.v4.runtime.tree.RuleNode;

import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.*;
import toolgood.algorithm.math.mathVisitor;

import java.util.ArrayList;
import java.util.Arrays;

public class MathSplitVisitor2 extends AbstractParseTreeVisitor<CalculateTree> implements mathVisitor<CalculateTree> {
    private boolean hasBracket = false;

    @Override
    public CalculateTree visitProg(mathParser.ProgContext context) {
        hasBracket = false;
        return visit(context.expr());
    }

    @Override
    public CalculateTree visitBracket_fun(Bracket_funContext context) {
        hasBracket = true;
        return visit(context.expr());
    }

    @Override
    public CalculateTree visitOr_fun(Or_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.HasBracket = hasBracket;
        hasBracket = false;
        CalculateTree f1 = visit(context.expr(0));
        CalculateTree f2 = visit(context.expr(1));
        tree.Nodes = new ArrayList<>(Arrays.asList(f1, f2));
        tree.Type = CalculateTreeType.Or;
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitAnd_fun(And_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.HasBracket = hasBracket;
        hasBracket = false;
        CalculateTree f1 = visit(context.expr(0));
        CalculateTree f2 = visit(context.expr(1));
        tree.Nodes = new ArrayList<>(Arrays.asList(f1, f2));
        tree.Type = CalculateTreeType.And;
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitMulDiv_fun(MulDiv_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.HasBracket = hasBracket;
        hasBracket = false;
        CalculateTree f1 = visit(context.expr(0));
        CalculateTree f2 = visit(context.expr(1));
        tree.Nodes = new ArrayList<>(Arrays.asList(f1, f2));
        int t = context.op.getType();
        if (t == mathParser.OPMUL) {
            tree.Type = CalculateTreeType.Mul;
        } else if (t == mathParser.OPDIV) {
            tree.Type = CalculateTreeType.Div;
        } else {
            tree.Type = CalculateTreeType.Mod;
        }
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitAddSub_fun(AddSub_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.HasBracket = hasBracket;
        hasBracket = false;
        CalculateTree f1 = visit(context.expr(0));
        CalculateTree f2 = visit(context.expr(1));
        tree.Nodes = new ArrayList<>(Arrays.asList(f1, f2));
        int t = context.op.getType();
        if (t == mathParser.OPADD) {
            tree.Type = CalculateTreeType.Add;
        } else if (t == mathParser.OPSUB) {
            tree.Type = CalculateTreeType.Sub;
        } else {
            tree.Type = CalculateTreeType.Connect;
        }
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitJudge_fun(Judge_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.HasBracket = hasBracket;
        hasBracket = false;
        CalculateTree f1 = visit(context.expr(0));
        CalculateTree f2 = visit(context.expr(1));
        tree.Nodes = new ArrayList<>(Arrays.asList(f1, f2));
        int t = context.op.getType();
        if (t == mathParser.OPGE) {
            tree.Type = CalculateTreeType.OpGe;
        } else if (t == mathParser.OPLE) {
            tree.Type = CalculateTreeType.OpLe;
        } else if (t == mathParser.OPGT) {
            tree.Type = CalculateTreeType.OpGt;
        } else if (t == mathParser.OPLT) {
            tree.Type = CalculateTreeType.OpLt;
        } else if (t == mathParser.OPEQ) {
            tree.Type = CalculateTreeType.OpEq;
        } else {
            tree.Type = CalculateTreeType.OpNe;
        }
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitChildren(RuleNode node) {
        ParserRuleContext context = (ParserRuleContext) node;
        CalculateTree tree = new CalculateTree();
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.Text = context.getText();
        return tree;
    }
}
