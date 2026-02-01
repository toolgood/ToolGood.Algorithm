import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';
import HmacSHA256 from 'crypto-js/hmac-sha256.js';

/**
 * Represents the HMACSHA256 encryption function
 */
export class Function_HMACSHA256 extends Function_2 {
    /**
     * @param {FunctionBase} a
     * @param {FunctionBase} b
     */
    constructor(z) {
    super(z);
  }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    Evaluate(work, tempParameter = null) {
        let args1 = this.a.Evaluate(work, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_error, "HMACSHA256", 1);
            if (args1.IsError) return args1;

        let args2 = this.b.Evaluate(work, tempParameter);
            args2 = args2.ToText(StringCache.Function_parameter_error, "HMACSHA256", 2);
            if (args2.IsError) return args2;

        try {
            let hmacHash = HmacSHA256(args1.TextValue,args2.TextValue || '');
            let result = hmacHash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return Operand.Error(StringCache.Function_error, "HMACSHA256");
        }
    }
}
