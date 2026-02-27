/**
 * DiyNameVisitor - DIY 名称访问者
 */
import mathVisitor from '../../math/mathVisitor.js';
import { CharUtil } from './CharUtil.js';
import { funcDict } from './MathFunctionVisitor.js';

class DiyNameVisitor extends mathVisitor {
    constructor() {
        super();
        this.diy = {
            Parameters: [],
            Functions: []
        };
    }

    /**
     * 访问 PARAMETER 函数节点
     */
    visitPARAMETER_fun(context) {
        let node = context.PARAMETER();
        if (node) {
            let t = CharUtil.standardString(node.getText());
            if (t === "E") {
                return this.visitChildren(context);
            } else if (t === "PI") {
                return this.visitChildren(context);
            } else if (t === "TRUE" || t === "YES" || t === "是" || t === "有") {
                return this.visitChildren(context);
            } else if (t === "FALSE" || t === "NO" || t === "不是" || t === "否" || t === "没有" || t === "无") {
                return this.visitChildren(context);
            } else if (t === "ALGORITHMVERSION" || t === "ENGINEVERSION") {
                return this.visitChildren(context);
            }
            let symbol = node.getSymbol();
            this.diy.Parameters.push({
                Name: node.getText(),
                Start: symbol.start,
                End: symbol.stop
            });
        }
        return this.visitChildren(context);
    }

    /**
     * 访问 GetJsonValue 函数节点
     */
    visitGetJsonValue_fun(context) {
        let node = context.PARAMETER();
        if (node) {
            let symbol = node.getSymbol();
            this.diy.Parameters.push({
                Name: node.getText(),
                Start: symbol.start,
                End: symbol.stop
            });
        }
        return this.visitChildren(context);
    }

    /**
     * 访问 DIY 函数节点
     */
    visitDiyFunction_fun(context) {
        let node = context.f;
        if (!node) {
            return this.visitChildren(context);
        }
        let t = CharUtil.standardString(node.text);
        if (t === "E") {
            return this.visitChildren(context);
        } else if (t === "PI") {
            return this.visitChildren(context);
        } else if (t === "TRUE" || t === "YES" || t === "是" || t === "有") {
            return this.visitChildren(context);
        } else if (t === "FALSE" || t === "NO" || t === "不是" || t === "否" || t === "没有" || t === "无") {
            return this.visitChildren(context);
        } else if (t === "ALGORITHMVERSION" || t === "ENGINEVERSION") {
            return this.visitChildren(context);
        } else if (t === "RAND") {
            return this.visitChildren(context);
        } else if (t === "GUID") {
            return this.visitChildren(context);
        } else if (t === "NOW") {
            return this.visitChildren(context);
        } else if (t === "TODAY") {
            return this.visitChildren(context);
        }
        if (funcDict[t]) {
            return this.visitChildren(context);
        }
        this.diy.Functions.push({
            Name: node.text,
            Start: node.start,
            End: node.stop
        });
        return this.visitChildren(context);
    }
}

export { DiyNameVisitor };
