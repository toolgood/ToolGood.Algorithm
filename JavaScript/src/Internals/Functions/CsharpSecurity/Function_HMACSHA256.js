import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import CryptoJS from 'crypto-js';
import iconv from 'iconv-lite';

/**
 * Represents the HMACSHA256 encryption function
 */
export class Function_HMACSHA256 extends Function_3 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     * @param {FunctionBase} func3
     */
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    Evaluate(work, tempParameter = null) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            const errorArgs1 = args1.ToText("Function '{0}' parameter {1} is error!", "HMACSHA256", 1);
            if (errorArgs1.IsError) return errorArgs1;
            return errorArgs1;
        }

        const args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotText) {
            const errorArgs2 = args2.ToText("Function '{0}' parameter {1} is error!", "HMACSHA256", 2);
            if (errorArgs2.IsError) return errorArgs2;
            return errorArgs2;
        }

        try {
            let encoding = 'utf8';
            if (this.func3 !== null) {
                const args3 = this.func3.Evaluate(work, tempParameter);
                if (args3.IsNotText) {
                    const errorArgs3 = args3.ToText("Function '{0}' parameter {1} is error!", "HMACSHA256", 3);
                    if (errorArgs3.IsError) return errorArgs3;
                    return errorArgs3;
                }
                encoding = args3.TextValue;
            }

            const bytes = iconv.encode(args1.TextValue, encoding);
            const key = args2.TextValue || '';
            const keyBytes = iconv.encode(key, 'utf8');
            const hmacHash = CryptoJS.HmacSHA256(CryptoJS.lib.WordArray.create(bytes), CryptoJS.lib.WordArray.create(keyBytes));
            const result = hmacHash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return Operand.Error("Function 'HMACSHA256' is error!" + ex.message);
        }
    }
}
