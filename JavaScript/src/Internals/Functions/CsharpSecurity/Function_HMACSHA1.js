import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

import HmacSHA1 from 'crypto-js/hmac-sha1.js';
import Utf8 from 'crypto-js/enc-utf8.js';

/**
 * Function_HMACSHA1
 */
export class Function_HMACSHA1 extends Function_2 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "HmacSHA1";
    }

    /**
     * @param {AlgorithmEngine} engine
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getText_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        try {
            let hmacHash = HmacSHA1(Utf8.parse(args1.TextValue), Utf8.parse(args2.TextValue || ''));
            let result = hmacHash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.parameterError(1);
        }
    }
}
