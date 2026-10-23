import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_URLENCODE
 */
export class Function_URLENCODE extends Function_1 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "UrlEncode";
    }

    /**
     * @param {AlgorithmEngine} engine
     * @param {Function} tempParameter
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let s = args1.TextValue;
        let r = encodeURIComponent(s)
            .replace(/%20/g, '+')
            // 与 C# HttpUtility.UrlEncode 一致：hex 输出小写
            .replace(/%[A-F0-9]{2}/g, (m) => m.toLowerCase());
        return Operand.Create(r);
    }
}
