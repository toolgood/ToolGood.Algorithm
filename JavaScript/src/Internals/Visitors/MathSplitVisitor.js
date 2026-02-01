/**
 * MathSplitvisitor.js
 * 访问者类，用于解析数学表达式并构建条件树
 */
import mathVisitor from '../../math/mathVisitor.js';
import { CharUtil } from './CharUtil.js';
import { ConditionTreeType } from '../../Enums/ConditionTreeType.js';

class MathSplitVisitor extends mathVisitor  {
    visitProg(context) {
        return context.expr().accept(this);
    }

    visitAndOr_fun(context) {
        let tree = {
            nodes: []
        };
        let t = context.op.text;
        if (CharUtil.Equals3(t, "&&", "and")) {
            tree.Type = ConditionTreeType.And;
        } else {
            tree.Type = ConditionTreeType.Or;
        }
        let exprs = context.expr();

        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start ? context.start.start : 0;
        tree.end = context.stop ? context.stop.stop : context.getText().length - 1;
        tree.conditionString = context.getText();
        return tree;
    }

    visitBracket_fun(context) {
        return context.expr().accept(this);
    }

    visitChildren(context) {
        let tree = {
            Type: ConditionTreeType.String,
            start: context.start ? context.start.start : 0,
            end: context.stop ? context.stop.stop : context.getText().length - 1,
            conditionString: context.getText()
        };
        return tree;
    }
}

export { MathSplitVisitor };