import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_SUBSTRING
 */
export class Function_SUBSTRING extends Function_3 {
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
            args1 = args1.ToText(StringCache.Function_parameter_error, 'Substring', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.b.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'Substring', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        let text = args1.TextValue;
        let startIndex = args2.IntValue - engine.ExcelIndex;
        
        if (this.c === null) {
            return Operand.Create(text.substring(startIndex));
        }
        
        let args3 = this.c.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'Substring', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        
        let length = args3.IntValue;
        let endIndex = startIndex + length;
        return Operand.Create(text.substring(startIndex, endIndex));
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

