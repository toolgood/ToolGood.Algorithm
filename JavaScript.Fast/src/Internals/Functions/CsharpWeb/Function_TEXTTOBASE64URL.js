import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_TEXTTOBASE64URL
 */
export class Function_TEXTTOBASE64URL extends Function_1 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "TextToBase64Url";
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
            let buffer = Buffer.from(args1.TextValue, 'utf-8');
            let t = buffer.toString('base64')
                .replace(/=/g, '')
                .replace(/\+/g, '-')
                .replace(/\//g, '_');
            return Operand.Create(t);
        } catch (e) {
            return this.parameterError(1);
        }
    }
}
