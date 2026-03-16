import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

import SHA1 from 'crypto-js/sha1.js';

/**
 * Represents the SHA1 encryption function
 */
export class Function_SHA1 extends Function_1 {
    get Name() {
        return "SHA1";
    }


    constructor(a) {
        super(a);
    }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    evaluate(work, tempParameter = null) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        try {
            let md5Hash = SHA1(args1.TextValue);
            let result = md5Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.functionError();
        }
    }
}
