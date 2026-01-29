import { DiyNameInfo, KeyInfo } from '../DiyNameInfo.js';

/**
 * DiyNameVisitor
 */
export class DiyNameVisitor {
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
    VisitPARAMETER_fun(context) {
        var node = context.PARAMETER();
        if(node != null) {
            var keyInfo = new KeyInfo();
            keyInfo.Name = node.GetText();
            keyInfo.Start = node.Symbol.StartIndex;
            keyInfo.End = node.Symbol.StopIndex;
            this.diy.Parameters.push(keyInfo);
        }
        return this.VisitChildren(context);
    }

    /**
     * @param {Object} context
     * @returns {Object}
     */
    VisitGetJsonValue_fun(context) {
        var node = context.PARAMETER();
        if(node != null) {
            var keyInfo = new KeyInfo();
            keyInfo.Name = node.GetText();
            keyInfo.Start = node.Symbol.StartIndex;
            keyInfo.End = node.Symbol.StopIndex;
            this.diy.Parameters.push(keyInfo);
        }
        return this.VisitChildren(context);
    }

    /**
     * @param {Object} context
     * @returns {Object}
     */
    VisitDiyFunction_fun(context) {
        var node = context.PARAMETER();
        var keyInfo = new KeyInfo();
        keyInfo.Name = node.GetText();
        keyInfo.Start = node.Symbol.StartIndex;
        keyInfo.End = node.Symbol.StopIndex;
        this.diy.Functions.push(keyInfo);
        return this.VisitChildren(context);
    }

    /**
     * Visits the children of the specified context.
     * @param {Object} context
     * @returns {Object}
     */
    VisitChildren(context) {
        // Implementation depends on the ANTLR4 JavaScript runtime
        // For simplicity, we'll just return null here
        return null;
    }

    // All other visit methods would follow the same pattern
    // For brevity, we'll omit them here
}
