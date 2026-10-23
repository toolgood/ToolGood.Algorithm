import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

import SHA1 from 'crypto-js/sha1.js';
import Utf8 from 'crypto-js/enc-utf8.js';

/**
 * Function_SHA1
 */
export class Function_SHA1 extends Function_1 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "SHA1";
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
            let sha1Hash = SHA1(Utf8.parse(args1.TextValue));
            let result = sha1Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.parameterError(1);
        }
    }
}
