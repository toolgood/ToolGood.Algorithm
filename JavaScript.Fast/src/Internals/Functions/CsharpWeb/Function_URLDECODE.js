import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_URLDECODE
 */
export class Function_URLDECODE extends Function_1 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }

    get Name() {
        return "UrlDecode";
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
        let s2 = s.replace(/\+/g, ' ');
        try {
            let r = decodeURIComponent(s2);
            return Operand.Create(r);
        } catch (e) {
            // C# HttpUtility.UrlDecode 对无效编码不抛异常，返回部分解码结果
            return Operand.Create(s2);
        }
    }
}
