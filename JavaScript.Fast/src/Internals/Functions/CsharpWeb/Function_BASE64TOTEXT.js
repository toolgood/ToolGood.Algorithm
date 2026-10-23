import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_BASE64TOTEXT
 */
export class Function_BASE64TOTEXT extends Function_1 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "Base64ToText";
    }

    /**
     * @param {AlgorithmEngine} engine
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        try {
            let buffer = Buffer.from(args1.TextValue, 'base64');
            let t = buffer.toString('utf-8');
            return Operand.Create(t);
        } catch (e) {
            return this.parameterError(1);
        }
    }
}
