import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

import SHA256 from 'crypto-js/sha256.js';

/**
 * Represents the SHA256 encryption function
 */
export class Function_SHA256 extends Function_1 {
    get Name() {
        return "SHA256";
    }


    constructor(a) {
        super(a);
    }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    Evaluate(work, tempParameter = null) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        try {
            let md5Hash = SHA256(args1.TextValue);
            let result = md5Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.FunctionError();
        }
    }
}
