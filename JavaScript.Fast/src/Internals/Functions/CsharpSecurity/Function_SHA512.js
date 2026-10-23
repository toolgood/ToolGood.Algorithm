import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

import SHA512 from 'crypto-js/sha512.js';
import Utf8 from 'crypto-js/enc-utf8.js';

/**
 * Function_SHA512
 */
export class Function_SHA512 extends Function_1 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "SHA512";
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
            let sha512Hash = SHA512(Utf8.parse(args1.TextValue));
            let result = sha512Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.parameterError(1);
        }
    }
}
