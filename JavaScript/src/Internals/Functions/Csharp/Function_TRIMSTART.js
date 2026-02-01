import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_TRIMSTART
 */
export class Function_TRIMSTART extends Function_2 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(funcs) {
    super(funcs);
  }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'TrimStart', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        
        if (this.func2 == null) {
            return Operand.Create(args1.TextValue.trimStart());
        }
        
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error, 'TrimStart', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        let trimChars = args2.TextValue.split('');
        let text = args1.TextValue;
        let index = 0;
        
        while (index < text.length && trimChars.includes(text[index])) {
            index++;
        }
        
        return Operand.Create(text.substring(index));
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

