import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import CryptoJS from 'crypto-js';
import iconv from 'iconv-lite';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Represents the SHA256 encryption function
 */
export class Function_SHA256 extends Function_2 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(func1, func2) {
        super(func1, func2);
    }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    Evaluate(work, tempParameter = null) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            const errorArgs1 = args1.ToText(StringCache.Function_parameter_error, "SHA256", 1);
            if (errorArgs1.IsError) return errorArgs1;
            return errorArgs1;
        }

        try {
            let encoding = 'utf8';
            if (this.func2 !== null) {
                const args2 = this.func2.Evaluate(work, tempParameter);
                if (args2.IsNotText) {
                    const errorArgs2 = args2.ToText(StringCache.Function_parameter_error, "SHA256", 2);
                    if (errorArgs2.IsError) return errorArgs2;
                    return errorArgs2;
                }
                encoding = args2.TextValue;
            }

            const bytes = iconv.encode(args1.TextValue, encoding);
            const sha256Hash = CryptoJS.SHA256(CryptoJS.lib.WordArray.create(bytes));
            const result = sha256Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return Operand.Error(StringCache.Function_error, "SHA256");
        }
    }
}
