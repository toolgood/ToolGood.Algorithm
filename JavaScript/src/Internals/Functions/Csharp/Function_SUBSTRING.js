import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_SUBSTRING
 */
export class Function_SUBSTRING extends Function_3 {
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
    Evaluate(engine) {
        const args1 = this.func1.Evaluate(engine);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'Substring', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine);
        if (args2.isNotNumber) {
            args2.toNumber('Function \'{0}\' parameter {1} is error!', 'Substring', 2);
            if (args2.isError) {
                return args2;
            }
        }
        
        const text = args1.textValue;
        const startIndex = args2.intValue - engine.excelIndex;
        
        if (this.func3 === null) {
            return Operand.create(text.substring(startIndex));
        }
        
        const args3 = this.func3.Evaluate(engine);
        if (args3.isNotNumber) {
            args3.toNumber('Function \'{0}\' parameter {1} is error!', 'Substring', 3);
            if (args3.isError) {
                return args3;
            }
        }
        
        const length = args3.intValue;
        const endIndex = startIndex + length;
        return Operand.create(text.substring(startIndex, endIndex));
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Substring');
    }
}
