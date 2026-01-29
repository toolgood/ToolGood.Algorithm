import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_LASTINDEXOF
 */
export class Function_LASTINDEXOF extends Function_4 {
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
        if (args1.IsNotText) {
            args1.ToText('Function \'{0}\' parameter {1} is error!', 'LastIndexOf', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2.ToText('Function \'{0}\' parameter {1} is error!', 'LastIndexOf', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const text = args1.TextValue;
        if (this.func3 === null) {
            const index = text.lastIndexOf(args2.TextValue);
            return Operand.Create(index + engine.excelIndex);
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3.ToNumber('Function \'{0}\' parameter {1} is error!', 'LastIndexOf', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        const startIndex = args3.IntValue;
        if (this.func4 === null) {
            const substring = text.substring(0, startIndex);
            const index = substring.lastIndexOf(args2.TextValue);
            return Operand.Create(index + engine.excelIndex);
        }
        const args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotNumber) {
            args4.ToNumber('Function \'{0}\' parameter {1} is error!', 'LastIndexOf', 4);
            if (args4.IsError) {
                return args4;
            }
        }
        const count = args4.IntValue;
        const endIndex = startIndex + count;
        const substring = text.substring(startIndex, endIndex);
        const index = substring.lastIndexOf(args2.TextValue);
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
        this.AddFunction(stringBuilder, 'LastIndexOf');
    }
}
