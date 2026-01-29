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
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'RemoveEnd', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'RemoveEnd', 2);
            if (args2.isError) {
                return args2;
            }
        }
        
        let ignoreCase = false;
        if (this.func3 !== null) {
            const args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.isNotBoolean) {
                args3.toBoolean('Function \'{0}\' parameter {1} is error!', 'RemoveEnd', 3);
                if (args3.isError) {
                    return args3;
                }
            }
            ignoreCase = args3.booleanValue;
        }
        
        const text = args1.textValue;
        const suffix = args2.textValue;
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
        this.addFunction(stringBuilder, 'RemoveEnd');
    }
}
