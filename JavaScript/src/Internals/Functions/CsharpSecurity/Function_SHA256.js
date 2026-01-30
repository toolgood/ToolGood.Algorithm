import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_SHA256
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
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    async Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText('Function \'{0}\' parameter {1} is error!', 'SHA256', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        
        try {
            let encoding = 'utf-8';
            if (this.func2 !== null) {
                let args2 = this.func2.Evaluate(engine, tempParameter);
                if (args2.IsNotText) {
                    args2 = args2.ToText('Function \'{0}\' parameter {1} is error!', 'SHA256', 2);
                    if (args2.IsError) {
                        return args2;
                    }
                }
                encoding = args2.TextValue;
            }
            
            let encoder = new TextEncoder(encoding);
            let buffer = encoder.encode(args1.TextValue);
            let t = await this.getSha256String(buffer);
            return Operand.Create(t);
        } catch (ex) {
            return Operand.error('Function \'SHA256\'is error!' + ex.message);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'SHA256');
    }
    
    /**
     * @param {Uint8Array} buffer
     * @returns {Promise<string>}
     */
    async getSha256String(buffer) {
        let hashmessage = await crypto.subtle.digest('SHA-256', buffer);
        let hashArray = Array.from(new Uint8Array(hashmessage));
        return hashArray.map(byte => byte.toString(16).padStart(2, '0')).join('');
    }
}
