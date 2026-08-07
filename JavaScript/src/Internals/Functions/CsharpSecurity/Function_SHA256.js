import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

import SHA256 from 'crypto-js/sha256.js';
import Utf8 from 'crypto-js/enc-utf8.js';

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
    evaluate(work, tempParameter = null) {
        let args1 = this.getText_1(work, tempParameter);
        // 与 C# 一致:错误或空值直接传播
        if (args1.IsError || args1.IsNull) { return args1; }

        try {
            // 与 C# 一致:先按 UTF-8 编码再哈希
            let md5Hash = SHA256(Utf8.parse(args1.TextValue));
            let result = md5Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.parameterError(1);
        }
    }
}
