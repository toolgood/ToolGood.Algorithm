import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_REMOVEEND
 */
export class Function_REMOVEEND extends Function_3 {
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
            args1 = args1.ToText('Function \'{0}\' parameter {1} is error!', 'RemoveEnd', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText('Function \'{0}\' parameter {1} is error!', 'RemoveEnd', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        let ignoreCase = false;
        if (this.func3 !== null) {
            let args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.IsNotBoolean) {
                args3.ToBoolean('Function \'{0}\' parameter {1} is error!', 'RemoveEnd', 3);
                if (args3.IsError) {
                    return args3;
                }
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
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'RemoveEnd');
    }
}
