import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_REGEXREPALCE
 */
export class Function_REGEXREPALCE extends Function_3 {
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
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'RegexReplace', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.b.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error, 'RegexReplace', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.c.Evaluate(engine, tempParameter);
        if (args3.IsNotText) {
            args3 = args3.ToText(StringCache.Function_parameter_error, 'RegexReplace', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        
        try {
            let regex = new RegExp(args2.TextValue, 'g');
            let b = args1.TextValue.replace(regex, args3.TextValue);
            return Operand.Create(b);
        } catch (e) {
            return Operand.Error(StringCache.Function_error, 'RegexReplace');
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

