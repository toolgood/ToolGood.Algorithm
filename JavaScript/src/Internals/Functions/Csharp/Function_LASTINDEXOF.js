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
    evaluate(engine) {
        const args1 = this._arg1.evaluate(engine);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'LastIndexOf', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this._arg2.evaluate(engine);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'LastIndexOf', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const text = args1.textValue;
        if (this._arg3 === null) {
            const index = text.lastIndexOf(args2.textValue);
            return Operand.create(index + engine.excelIndex);
        }
        const args3 = this._arg3.evaluate(engine);
        if (args3.isNotNumber) {
            args3.toNumber('Function \'{0}\' parameter {1} is error!', 'LastIndexOf', 3);
            if (args3.isError) {
                return args3;
            }
        }
        const startIndex = args3.intValue;
        if (this._arg4 === null) {
            const substring = text.substring(0, startIndex);
            const index = substring.lastIndexOf(args2.textValue);
            return Operand.create(index + engine.excelIndex);
        }
        const args4 = this._arg4.evaluate(engine);
        if (args4.isNotNumber) {
            args4.toNumber('Function \'{0}\' parameter {1} is error!', 'LastIndexOf', 4);
            if (args4.isError) {
                return args4;
            }
        }
        const count = args4.intValue;
        const endIndex = startIndex + count;
        const substring = text.substring(startIndex, endIndex);
        const index = substring.lastIndexOf(args2.textValue);
        if (index === -1) {
            return Operand.create(-1 + engine.excelIndex);
        }
        return Operand.create(index + startIndex + engine.excelIndex);
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'LastIndexOf');
    }
}
