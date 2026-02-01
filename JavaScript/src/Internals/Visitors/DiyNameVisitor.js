/**
 * DiyNameVisitor - DIY 名称访问者
 */
import mathVisitor from '../../math/mathVisitor.js';

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
            this.diy.Parameters.push({
                Name: node.getText(),
                Start: node.symbol.start,
                End: node.symbol.stop
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
            this.diy.Parameters.push({
                Name: node.getText(),
                Start: node.symbol.start,
                End: node.symbol.stop
            });
        }
        return this.visitChildren(context);
    }

    /**
     * 访问 DIY 函数节点
     */
    visitDiyFunction_fun(context) {
        let node = context.PARAMETER();
        this.diy.Functions.push({
            Name: node.getText(),
            Start: node.symbol.start,
            End: node.symbol.stop
        });
        return this.visitChildren(context);
    }
}

export { DiyNameVisitor };
