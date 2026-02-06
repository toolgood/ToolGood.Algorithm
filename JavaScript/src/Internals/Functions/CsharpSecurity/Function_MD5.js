import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';
import MD5 from 'crypto-js/md5.js';

/**
 * Represents the MD5 encryption function
 */
export class Function_MD5 extends Function_1 {
    get Name() {
        return "MD5";
    }


    constructor(a) {
        super(a);
    }

    /**
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter
     */
    Evaluate(work, tempParameter = null) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        try {
            let md5Hash = MD5(args1.TextValue);
            let result = md5Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.FunctionError();
        }
    }
}
