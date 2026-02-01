import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_REMOVEEND
 */
export class Function_REMOVEEND extends Function_3 {
    /**
     * @param {FunctionBase} a
     * @param {FunctionBase} b
     * @param {FunctionBase} c
     */
    constructor(z) {
    super(z);
  }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_error, 'RemoveEnd', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToText(StringCache.Function_parameter_error, 'RemoveEnd', 2);
            if (args2.IsError) {
                return args2;
            }
        
        let ignoreCase = false;
        if (this.c !== null) {
            let args3 = this.c.Evaluate(engine, tempParameter);
                args3 = args3.ToBoolean(StringCache.Function_parameter_error, 'RemoveEnd', 3);
                if (args3.IsError) {
                    return args3;
                }
            ignoreCase = args3.BooleanValue;
        }
        
        let text = args1.TextValue;
        let suffix = args2.TextValue;
        let endsWith = false;
        
        if (ignoreCase) {
            endsWith = text.toLowerCase().endsWith(suffix.toLowerCase());
        } else {
            endsWith = text.endsWith(suffix);
        }
        
        if (endsWith) {
            return Operand.Create(text.substring(0, text.length - suffix.length));
        }
        return args1;
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

