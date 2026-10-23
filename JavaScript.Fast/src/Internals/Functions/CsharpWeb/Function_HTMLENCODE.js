import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_HTMLENCODE
 */
export class Function_HTMLENCODE extends Function_1 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "HtmlEncode";
    }

    /**
     * @param {AlgorithmEngine} engine
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let s = args1.TextValue;
        let r = htmlEncode(s);
        return Operand.Create(r);
    }
}

function htmlEncode(input) {
    if (input == null) return '';
    return input.toString().replace(/[&<>'"]/g, function (match) {
        switch (match) {
            case '&': return '&amp;';
            case '<': return '&lt;';
            case '>': return '&gt;';
            case '"': return '&quot;';
            case "'": return '&#39;';
            default: return match;
        }
    });
}
