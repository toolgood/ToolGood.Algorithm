import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_ENDSWITH
 */
export class Function_ENDSWITH extends Function_3 {
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
            args1 = args1.ToText(StringCache.Function_parameter_error, 'EndsWith', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToText(StringCache.Function_parameter_error, 'EndsWith', 2);
            if (args2.IsError) {
                return args2;
            }
        let text = args1.TextValue;
        if (this.c === null) {
            return Operand.Create(text.endsWith(args2.TextValue));
        }
        let args3 = this.c.Evaluate(engine, tempParameter);
            args3 = args3.ToBoolean(StringCache.Function_parameter_error, 'EndsWith', 3);
            if (args3.IsError) {
                return args3;
            }
        let ignoreCase = args3.BooleanValue;
        if (ignoreCase) {
            return Operand.Create(text.toLowerCase().endsWith(args2.TextValue.toLowerCase()));
        } else {
            return Operand.Create(text.endsWith(args2.TextValue));
        }
    }
    
    
}

