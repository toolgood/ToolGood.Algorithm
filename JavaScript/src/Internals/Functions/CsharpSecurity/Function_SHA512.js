import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { CryptoUtils } from './CryptoUtils.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_SHA512
 */
export class Function_SHA512 extends Function_2 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(func1, func2) {
        super(func1, func2);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, "SHA512", 1);
            if (args1.IsError) {
                return args1;
            }
        }
        
        try {
            let encoding = 'utf-8';
            if (this.func2 !== null) {
                let args2 = this.func2.Evaluate(engine, tempParameter);
                if (args2.IsNotText) {
                    args2 = args2.ToText(StringCache.Function_parameter_error, "SHA512", 2);
                    if (args2.IsError) {
                        return args2;
                    }
                }
                encoding = args2.TextValue;
            }
            
            // 使用浏览器兼容的CryptoUtils计算SHA512哈希值
            let t = CryptoUtils.hash('SHA512', args1.TextValue, encoding);
            return Operand.Create(t);
        } catch (ex) {
            return Operand.Error('Function \'SHA512\'is error!' + ex.message);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

