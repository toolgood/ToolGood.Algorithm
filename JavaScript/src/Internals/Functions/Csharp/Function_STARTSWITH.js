import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

/**
 * Function_STARTSWITH
 */
export class Function_STARTSWITH extends Function_3 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     * @param {FunctionBase} func3
     */
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'StartsWith', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error, 'StartsWith', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        let text = args1.TextValue;
        let prefix = args2.TextValue;
        
        if (this.func3 == null) {
            return Operand.Create(text.startsWith(prefix));
        }
        
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotBoolean) {
            args3 = args3.ToBoolean(StringCache.Function_parameter_error, 'StartsWith', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        
        let ignoreCase = args3.BooleanValue;
        if (ignoreCase) {
            return Operand.Create(text.toLowerCase().startsWith(prefix.toLowerCase()));
        } else {
            return Operand.Create(text.startsWith(prefix));
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

