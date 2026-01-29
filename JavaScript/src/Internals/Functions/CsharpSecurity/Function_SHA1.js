import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_SHA1
 */
export class Function_SHA1 extends Function_2 {
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
    async evaluate(engine) {
        const args1 = this._arg1.evaluate(engine);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'SHA1', 1);
            if (args1.isError) {
                return args1;
            }
        }
        
        try {
            let encoding = 'utf-8';
            if (this._arg2 !== null) {
                const args2 = this._arg2.evaluate(engine);
                if (args2.isNotText) {
                    args2.toText('Function \'{0}\' parameter {1} is error!', 'SHA1', 2);
                    if (args2.isError) {
                        return args2;
                    }
                }
                encoding = args2.textValue;
            }
            
            const encoder = new TextEncoder(encoding);
            const buffer = encoder.encode(args1.textValue);
            const t = await this.getSha1String(buffer);
            return Operand.create(t);
        } catch (ex) {
            return Operand.error('Function \'SHA1\'is error!' + ex.message);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'SHA1');
    }
    
    /**
     * @param {Uint8Array} buffer
     * @returns {Promise<string>}
     */
    async getSha1String(buffer) {
        const hashmessage = await crypto.subtle.digest('SHA-1', buffer);
        const hashArray = Array.from(new Uint8Array(hashmessage));
        return hashArray.map(byte => byte.toString(16).padStart(2, '0')).join('');
    }
}
