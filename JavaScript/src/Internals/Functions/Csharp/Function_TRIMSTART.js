import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_TRIMSTART
 */
export class Function_TRIMSTART extends Function_2 {
    /**
     * @param {FunctionBase} a
     * @param {FunctionBase} b
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
            args1 = args1.ToText(StringCache.Function_parameter_error, 'TrimStart', 1);
            if (args1.IsError) {
                return args1;
            }
        
        if (this.b == null) {
            return Operand.Create(args1.TextValue.trimStart());
        }
        
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToText(StringCache.Function_parameter_error, 'TrimStart', 2);
            if (args2.IsError) {
                return args2;
            }
        
        let trimChars = args2.TextValue.split('');
        let text = args1.TextValue;
        let index = 0;
        
        while (index < text.length && trimChars.includes(text[index])) {
            index++;
        }
        
        return Operand.Create(text.substring(index));
    }
    

}

