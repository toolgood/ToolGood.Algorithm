import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

import HmacSHA256 from 'crypto-js/hmac-sha256.js';

/**
 * Represents the HMACSHA256 encryption function
 */
export class Function_HMACSHA256 extends Function_2 {

    get Name() {
        return "HmacSHA256";
    }

    constructor(z) {
    super(z);
  }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    evaluate(work, tempParameter = null) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getText_2(work, tempParameter);
        if (args2.IsError) { return args2; }

        try {
            let hmacHash = HmacSHA256(args1.TextValue,args2.TextValue || '');
            let result = hmacHash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.functionError();
        }
    }
}
