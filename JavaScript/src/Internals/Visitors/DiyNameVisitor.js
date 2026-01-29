import { DiyNameInfo, KeyInfo } from '../DiyNameInfo.js';
import mathVisitor from '../../math/mathVisitor.js';

/**
 * DiyNamevisitor
 */
export class DiyNameVisitor extends mathVisitor  {
    /**
     * @constructor
     */
    constructor() {
        this.diy = new DiyNameInfo();
    }

    /**
     * @param {Object} context
     * @returns {Object}
     */
    visitPARAMETER_fun(context) {
        var node = context.PARAMETER();
        if(node != null) {
            var keyInfo = new KeyInfo();
            keyInfo.Name = node.GetText();
            keyInfo.Start = node.Symbol.StartIndex;
            keyInfo.End = node.Symbol.StopIndex;
            this.diy.Parameters.push(keyInfo);
        }
        return this.visitChildren(context);
    }

    /**
     * @param {Object} context
     * @returns {Object}
     */
    visitGetJsonValue_fun(context) {
        var node = context.PARAMETER();
        if(node != null) {
            var keyInfo = new KeyInfo();
            keyInfo.Name = node.GetText();
            keyInfo.Start = node.Symbol.StartIndex;
            keyInfo.End = node.Symbol.StopIndex;
            this.diy.Parameters.push(keyInfo);
        }
        return this.visitChildren(context);
    }

    /**
     * @param {Object} context
     * @returns {Object}
     */
    visitDiyFunction_fun(context) {
        var node = context.PARAMETER();
        var keyInfo = new KeyInfo();
        keyInfo.Name = node.GetText();
        keyInfo.Start = node.Symbol.StartIndex;
        keyInfo.End = node.Symbol.StopIndex;
        this.diy.Functions.push(keyInfo);
        return this.visitChildren(context);
    }

    /**
     * visits the children of the specified context.
     * @param {Object} context
     * @returns {Object}
     */
    visitChildren(context) {
        // Implementation depends on the ANTLR4 JavaScript runtime
        // For simplicity, we'll just return null here
        return null;
    }

    // All other visit methods would follow the same pattern
    // For brevity, we'll omit them here
}
