import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

import MD5 from 'crypto-js/md5.js';
import Utf8 from 'crypto-js/enc-utf8.js';

/**
 * Function_MD5
 */
export class Function_MD5 extends Function_1 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "MD5";
    }

    /**
     * @param {AlgorithmEngine} engine
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        try {
            let md5Hash = MD5(Utf8.parse(args1.TextValue));
            let result = md5Hash.toString().toUpperCase();
            return Operand.Create(result);
        } catch (ex) {
            return this.parameterError(1);
        }
    }
}
