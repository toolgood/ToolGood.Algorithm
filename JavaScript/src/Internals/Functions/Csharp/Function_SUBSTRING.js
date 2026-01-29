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
    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function \'{0}\' parameter {1} is error!', 'Substring', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function \'{0}\' parameter {1} is error!', 'Substring', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        let text = args1.TextValue;
        let startIndex = args2.IntValue - engine.excelIndex;
        
        if (this.func3 === null) {
            return Operand.Create(text.substring(startIndex));
        }
        
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3.ToNumber('Function \'{0}\' parameter {1} is error!', 'Substring', 3);
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
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Substring');
    }
}
