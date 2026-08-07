/**
 * MathSplitvisitor2.js
 * 访问者类，用于解析数学表达式并构建计算树
 */
import mathVisitor from '../../math/mathVisitor.js';
import { CharUtil } from './CharUtil.js';
import { CalculateTreeType } from '../../Enums/CalculateTreeType.js';

export class MathSplitVisitor2 extends mathVisitor  {
    visitProg(context) {
        this.hasBracket = false;
        return context.expr().accept(this);
    }

    visitBracket_fun(context) {
        this.hasBracket = true;
        return context.expr().accept(this);
    }

    visitAndOr_fun(context) {
        let tree = {
            HasBracket: this.hasBracket,
            nodes: []
        };
        this.hasBracket = false;
        let exprs = context.expr();
        let t = context.op.text;
        if (CharUtil.equals(t, "&&")) {
            tree.Type = CalculateTreeType.And;
        } else {
            tree.Type = CalculateTreeType.Or;
        }
        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start ? context.start.start : 0;
        tree.end = context.stop ? context.stop.stop : context.getText().length - 1;
        tree.conditionString = context.getText();
        return tree;
    }

    visitJudge_fun(context) {
        let tree = {
            HasBracket: this.hasBracket,
            nodes: []
        };
        this.hasBracket = false;
        let exprs = context.expr();
        let t = context.op.text;
        if (CharUtil.equals(t, ">=")) {
            tree.Type = CalculateTreeType.OpGe;
        } else if (CharUtil.equals(t, "<=")) {
            tree.Type = CalculateTreeType.OpLe;
        } else if (CharUtil.equals(t, ">")) {
            tree.Type = CalculateTreeType.OpGt;
        } else if (CharUtil.equals(t, "<")) {
            tree.Type = CalculateTreeType.OpLt;
        } else if (CharUtil.equals4(t, "=", "==", "===")) {
            tree.Type = CalculateTreeType.OpEq;
        } else {
            tree.Type = CalculateTreeType.OpNe;
        }
        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start ? context.start.start : 0;
        tree.end = context.stop ? context.stop.stop : context.getText().length - 1;
        tree.conditionString = context.getText();
        return tree;
    }

    visitMulDiv_fun(context) {
        let tree = {
            HasBracket: this.hasBracket,
            nodes: []
        };
        this.hasBracket = false;
        let exprs = context.expr();
        let t = context.op.text;
        if (CharUtil.equals(t, '*')) {
            tree.Type = CalculateTreeType.Mul;
        } else if (CharUtil.equals(t, '/')) {
            tree.Type = CalculateTreeType.Div;
        } else {
            tree.Type = CalculateTreeType.Mod;
        }
        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start ? context.start.start : 0;
        tree.end = context.stop ? context.stop.stop : context.getText().length - 1;
        tree.conditionString = context.getText();
        return tree;
    }

    visitAddSub_fun(context) {
        let tree = {
            HasBracket: this.hasBracket,
            nodes: []
        };
        this.hasBracket = false;
        let exprs = context.expr();
        let t = context.op.text;
        if (CharUtil.equals(t, '+')) {
            tree.Type = CalculateTreeType.Add;
        } else if (CharUtil.equals(t, '-')) {
            tree.Type = CalculateTreeType.Sub;
        } else {
            tree.Type = CalculateTreeType.Connect;
        }
        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start ? context.start.start : 0;
        tree.end = context.stop ? context.stop.stop : context.getText().length - 1;
        tree.conditionString = context.getText();
        return tree;
    }

    visitChildren(context) {
        let tree = {
            Type: CalculateTreeType.String,
            start: context.start ? context.start.start : 0,
            end: context.stop ? context.stop.stop : context.getText().length - 1,
            conditionString: context.getText()
        };
        return tree;
    }
}
