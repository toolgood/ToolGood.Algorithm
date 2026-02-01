import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_REGEX
 */
export class Function_REGEX extends Function_2 {
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
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'Regex', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.b.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error, 'Regex', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        try {
            let regex = new RegExp(args2.TextValue);
            let b = regex.exec(args1.TextValue);
            if (!b) {
                return Operand.Error(StringCache.Function_error, 'Regex');
            }
            return Operand.Create(b[0]);
        } catch (e) {
            return Operand.Error(StringCache.Function_error, 'Regex');
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

