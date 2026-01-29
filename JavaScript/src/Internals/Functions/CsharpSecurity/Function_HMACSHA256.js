import { Function_3 } from '../Function_3';
import { Operand } from '../../../Operand';

/**
 * Function_HMACSHA256
 */
export class Function_HMACSHA256 extends Function_3 {
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
    async evaluate(engine) {
        const args1 = this._arg1.evaluate(engine);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'HmacSHA256', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this._arg2.evaluate(engine);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'HmacSHA256', 2);
            if (args2.isError) {
                return args2;
            }
        }
        
        try {
            let encoding = 'utf-8';
            if (this._arg3 !== null) {
                const args3 = this._arg3.evaluate(engine);
                if (args3.isNotText) {
                    args3.toText('Function \'{0}\' parameter {1} is error!', 'HmacSHA256', 3);
                    if (args3.isError) {
                        return args3;
                    }
                }
                encoding = args3.textValue;
            }
            
            const encoder = new TextEncoder(encoding);
            const buffer = encoder.encode(args1.textValue);
            const t = await this.getHmacSha256String(buffer, args2.textValue);
            return Operand.create(t);
        } catch (ex) {
            return Operand.error('Function \'HmacSHA256\'' is error!' + ex.message);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'HmacSHA256');
    }
    
    /**
     * @param {Uint8Array} buffer
     * @param {string} secret
     * @returns {Promise<string>}
     */
    async getHmacSha256String(buffer, secret) {
        const encoder = new TextEncoder('utf-8');
        const keyByte = encoder.encode(secret || '');
        
        const key = await crypto.subtle.importKey(
            'raw',
            keyByte,
            { name: 'HMAC', hash: 'SHA-256' },
            false,
            ['sign', 'verify']
        );
        
        const hashmessage = await crypto.subtle.sign('HMAC', key, buffer);
        const hashArray = Array.from(new Uint8Array(hashmessage));
        return hashArray.map(byte => byte.toString(16).padStart(2, '0')).join('');
    }
}
