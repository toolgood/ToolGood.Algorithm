import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_HMACSHA512
 */
export class Function_HMACSHA512 extends Function_3 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     * @param {FunctionBase} func3
     */
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    async Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText('Function \'{0}\' parameter {1} is error!', 'HmacSHA512', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText('Function \'{0}\' parameter {1} is error!', 'HmacSHA512', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        try {
            let encoding = 'utf-8';
            if (this.func3 !== null) {
                let args3 = this.func3.Evaluate(engine, tempParameter);
                if (args3.IsNotText) {
                    args3 = args3.ToText('Function \'{0}\' parameter {1} is error!', 'HmacSHA512', 3);
                    if (args3.IsError) {
                        return args3;
                    }
                }
                encoding = args3.TextValue;
            }
            
            let encoder = new TextEncoder(encoding);
            let buffer = encoder.encode(args1.TextValue);
            let t = await this.getHmacSha512String(buffer, args2.TextValue);
            return Operand.Create(t);
        } catch (ex) {
            return Operand.error('Function \'HmacSHA512\'is error!' + ex.message);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'HmacSHA512');
    }
    
    /**
     * @param {Uint8Array} buffer
     * @param {string} secret
     * @returns {Promise<string>}
     */
    async getHmacSha512String(buffer, secret) {
        let encoder = new TextEncoder('utf-8');
        let keyByte = encoder.encode(secret || '');
        
        let key = await crypto.subtle.importKey(
            'raw',
            keyByte,
            { name: 'HMAC', hash: 'SHA-512' },
            false,
            ['sign', 'verify']
        );
        
        let hashmessage = await crypto.subtle.sign('HMAC', key, buffer);
        let hashArray = Array.from(new Uint8Array(hashmessage));
        return hashArray.map(byte => byte.toString(16).padStart(2, '0')).join('');
    }
}
