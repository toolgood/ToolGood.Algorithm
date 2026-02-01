import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';
import SHA512 from 'crypto-js/sha512.js';

/**
 * Represents the SHA512 encryption function
 */
export class Function_SHA512 extends Function_1 {
    /**
     * @param {FunctionBase} a
     */
    constructor(a) {
        super(a);
    }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    Evaluate(work, tempParameter = null) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            let errorArgs1 = args1.ToText(StringCache.Function_parameter_error, "SHA512", 1);
            if (errorArgs1.IsError) return errorArgs1;
            return errorArgs1;
        }

        try {
            let md5Hash = SHA512(args1.TextValue);
            let result = md5Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return Operand.Error(StringCache.Function_error, "SHA512");
        }
    }
}
