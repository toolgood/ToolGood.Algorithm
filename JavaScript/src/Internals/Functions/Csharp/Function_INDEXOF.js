import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_INDEXOF
 */
export class Function_INDEXOF extends Function_4 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     * @param {FunctionBase} func3
     * @param {FunctionBase} func4
     */
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'IndexOf', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'IndexOf', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const text = args1.textValue;
        if (this.func3 === null) {
            const index = text.indexOf(args2.textValue);
            return Operand.Create(index + engine.excelIndex);
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            args3.ToNumber('Function \'{0}\' parameter {1} is error!', 'IndexOf', 3);
            if (args3.isError) {
                return args3;
            }
        }
        const startIndex = args3.intValue;
        if (this.func4 === null) {
            const index = text.indexOf(args2.textValue, startIndex);
            return Operand.Create(index + engine.excelIndex);
        }
        const args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.isNotNumber) {
            args4.ToNumber('Function \'{0}\' parameter {1} is error!', 'IndexOf', 4);
            if (args4.isError) {
                return args4;
            }
        }
        const count = args4.intValue;
        const endIndex = startIndex + count;
        const substring = text.substring(startIndex, endIndex);
        const index = substring.indexOf(args2.textValue);
        if (index === -1) {
            return Operand.Create(-1 + engine.excelIndex);
        }
        return Operand.Create(index + startIndex + engine.excelIndex);
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'IndexOf');
    }
}
