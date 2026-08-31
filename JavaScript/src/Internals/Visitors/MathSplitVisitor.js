/**
 * MathSplitvisitor.js
 * 访问者类，用于解析数学表达式并构建条件树
 */
import mathVisitor from '../../math/mathVisitor.js';
import { CharUtil } from './CharUtil.js';
import { ConditionTreeType } from '../../Enums/ConditionTreeType.js';

// 共享的懒加载 getter，避免为每个节点创建函数对象
function conditionStringGetter() {
    if (this._text == null && this._source != null && this.end >= this.start) {
        this._text = this._source.substring(this.start, this.end + 1);
    }
    return this._text;
}

class MathSplitVisitor extends mathVisitor  {
    createTree(base) {
        base._source = this.Source;
        Object.defineProperty(base, 'conditionString', {
            enumerable: true,
            configurable: true,
            get: conditionStringGetter
        });
        return base;
    }

    visitProg(context) {
        this.hasBracket = false;
        return context.expr().accept(this);
    }

    visitAndOr_fun(context) {
        let tree = this.createTree({
            HasBracket: this.hasBracket,
            nodes: []
        });
        this.hasBracket = false;
        let t = context.op.text;
        if (CharUtil.equals3(t, "&&", "and")) {
            tree.Type = ConditionTreeType.And;
        } else {
            tree.Type = ConditionTreeType.Or;
        }
        let exprs = context.expr();

        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start ? context.start.start : 0;
        tree.end = context.stop ? context.stop.stop : context.getText().length - 1;
        return tree;
    }

    visitBracket_fun(context) {
        this.hasBracket = true;
        return context.expr().accept(this);
    }

    visitChildren(context) {
        return this.createTree({
            Type: ConditionTreeType.String,
            start: context.start ? context.start.start : 0,
            end: context.stop ? context.stop.stop : context.getText().length - 1
        });
    }
}

export { MathSplitVisitor };
