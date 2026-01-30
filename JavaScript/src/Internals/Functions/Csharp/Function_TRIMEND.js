import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_TRIMEND
 */
export class Function_TRIMEND extends Function_2 {
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
            args1 = args1.ToText(StringCache.Function_parameter_error2, 'TrimEnd', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        
        if (this.func2 == null) {
            return Operand.Create(args1.TextValue.trimEnd());
        }
        
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error2, 'TrimEnd', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        let trimChars = args2.TextValue.split('');
        let text = args1.TextValue;
        let index = text.length - 1;
        
        while (index >= 0 && trimChars.includes(text[index])) {
            index--;
        }
        
        return Operand.Create(text.substring(0, index + 1));
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'TrimEnd');
    }
}
