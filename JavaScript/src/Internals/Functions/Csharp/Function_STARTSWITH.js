import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

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
    evaluate(engine) {
        const args1 = this.func1.evaluate(engine);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'StartsWith', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.evaluate(engine);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'StartsWith', 2);
            if (args2.isError) {
                return args2;
            }
        }
        
        const text = args1.textValue;
        const prefix = args2.textValue;
        
        if (this.func3 === null) {
            return Operand.create(text.startsWith(prefix));
        }
        
        const args3 = this.func3.evaluate(engine);
        if (args3.isNotBoolean) {
            args3.toBoolean('Function \'{0}\' parameter {1} is error!', 'StartsWith', 3);
            if (args3.isError) {
                return args3;
            }
        }
        
        const ignoreCase = args3.booleanValue;
        if (ignoreCase) {
            return Operand.create(text.toLowerCase().startsWith(prefix.toLowerCase()));
        } else {
            return Operand.create(text.startsWith(prefix));
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'StartsWith');
    }
}
