import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';
import SHA256 from 'crypto-js/sha256.js';

/**
 * Represents the SHA256 encryption function
 */
export class Function_SHA256 extends Function_1 {
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
            args1 = args1.ToText(StringCache.Function_parameter_error, "SHA256", 1);
            if (args1.IsError) return args1;

        try {
            let md5Hash = SHA256(args1.TextValue);
            let result = md5Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return Operand.Error(StringCache.Function_error, "SHA256");
        }
    }
}
