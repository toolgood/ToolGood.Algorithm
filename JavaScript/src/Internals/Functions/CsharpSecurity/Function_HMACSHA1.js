import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

import HmacSHA1 from 'crypto-js/hmac-sha1.js';
import Utf8 from 'crypto-js/enc-utf8.js';

/**
 * Represents the HMACSHA1 encryption function
 */
export class Function_HMACSHA1 extends Function_2 {

    get Name() {
        return "HmacSHA1";
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
        // 与 C# 一致:错误或空值直接传播
        if (args1.IsError || args1.IsNull) { return args1; }

        let args2 = this.getText_2(work, tempParameter);
        if (args2.IsError || args2.IsNull) { return args2; }

        try {
            // 与 C# 一致:message 与 key 均先按 UTF-8 编码
            let hmacHash = HmacSHA1(Utf8.parse(args1.TextValue), Utf8.parse(args2.TextValue || ''));
            let result = hmacHash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.parameterError(1);
        }
    }
}
