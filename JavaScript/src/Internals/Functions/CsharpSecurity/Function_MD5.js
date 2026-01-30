import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import * as crypto from 'crypto';
import * as iconv from 'iconv-lite';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_MD5
 */
export class Function_MD5 extends Function_2 {
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
            args1 = args1.ToText(StringCache.Function_parameter_error2, "MD5", 1);
            if (args1.IsError) {
                return args1;
            }
        }
        
        try {
            let encoding = 'utf-8';
            if (this.func2 !== null) {
                let args2 = this.func2.Evaluate(engine, tempParameter);
                if (args2.IsNotText) {
                    args2 = args2.ToText(StringCache.Function_parameter_error2, "MD5", 2);
                    if (args2.IsError) {
                        return args2;
                    }
                }
                encoding = args2.TextValue;
            }
            
            // 使用iconv-lite处理不同编码
            let buffer;
            if (iconv.encodingExists(encoding)) {
                buffer = iconv.encode(args1.TextValue, encoding);
            } else {
                // 如果编码不支持，默认使用utf-8
                buffer = Buffer.from(args1.TextValue, 'utf-8');
            }
            
            // 使用Node.js的crypto模块计算MD5哈希值
            let hash = crypto.createHash('md5');
            hash.update(buffer);
            let t = hash.digest('hex').toUpperCase();
            return Operand.Create(t);
        } catch (ex) {
            return Operand.error('Function \'MD5\'is error!' + ex.message);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'MD5');
    }
}
