import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

import HMACMD5 from 'crypto-js/hmac-md5.js';

/**
 * Represents the HMACMD5 encryption function
 */
export class Function_HMACMD5 extends Function_2 {

    get Name() {
        return "HmacMD5";
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
            let hmacHash = HMACMD5(args1.TextValue,args2.TextValue || '');
            let result = hmacHash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.functionError();
        }
    }
}
