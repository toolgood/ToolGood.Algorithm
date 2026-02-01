import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_ISREGEX
 */
export class Function_ISREGEX extends Function_2 {
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
            args1 = args1.ToText(StringCache.Function_parameter_error, 'IsRegex', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToText(StringCache.Function_parameter_error, 'IsRegex', 2);
            if (args2.IsError) {
                return args2;
            }
        try {
            let regex = new RegExp(args2.TextValue);
            let b = regex.test(args1.TextValue);
            return Operand.Create(b);
        } catch (e) {
            return Operand.Create(false);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

