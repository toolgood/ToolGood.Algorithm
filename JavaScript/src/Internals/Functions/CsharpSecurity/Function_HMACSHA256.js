import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import * as crypto from 'crypto';
import * as iconv from 'iconv-lite';
import { StringCache } from '../../../Internals/StringCache.js';

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
    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'HmacSHA256', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error, 'HmacSHA256', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        try {
            let encoding = 'utf-8';
            if (this.func3 !== undefined && this.func3 !== null) {
                let args3 = this.func3.Evaluate(engine, tempParameter);
                if (args3.IsNotText) {
                    args3 = args3.ToText(StringCache.Function_parameter_error, 'HmacSHA256', 3);
                    if (args3.IsError) {
                        return args3;
                    }
                }
                encoding = args3.TextValue;
            }
            
            // 使用iconv-lite处理不同编码
            let buffer;
            if (iconv.encodingExists(encoding)) {
                buffer = iconv.encode(args1.TextValue, encoding);
            } else {
                // 如果编码不支持，默认使用utf-8
                buffer = Buffer.from(args1.TextValue, 'utf-8');
            }
            
            // 使用Node.js的crypto模块计算HMAC-SHA256哈希�?
            let hmac = crypto.createHmac('sha256', args2.TextValue);
            hmac.update(buffer);
            let t = hmac.digest('hex').toUpperCase();
            return Operand.Create(t);
        } catch (ex) {
            return Operand.Error('Function \'HmacSHA256\'is error!' + ex.message);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

