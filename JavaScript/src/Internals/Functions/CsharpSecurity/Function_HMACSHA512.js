import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';
import HmacSHA512 from 'crypto-js/hmac-sha512.js';

/**
 * Represents the HMACSHA512 encryption function
 */
export class Function_HMACSHA512 extends Function_2 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(funcs) {
    super(funcs);
  }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    Evaluate(work, tempParameter = null) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            const errorArgs1 = args1.ToText(StringCache.Function_parameter_error, "HMACSHA512", 1);
            if (errorArgs1.IsError) return errorArgs1;
            return errorArgs1;
        }

        const args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotText) {
            const errorArgs2 = args2.ToText(StringCache.Function_parameter_error, "HMACSHA512", 2);
            if (errorArgs2.IsError) return errorArgs2;
            return errorArgs2;
        }

        try {
            const hmacHash = HmacSHA512(args1.TextValue,args2.TextValue || '');
            const result = hmacHash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return Operand.Error(StringCache.Function_error, "HMACSHA512");
        }
    }
}
