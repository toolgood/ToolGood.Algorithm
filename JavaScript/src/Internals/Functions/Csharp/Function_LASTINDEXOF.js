import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

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
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'LastIndexOf', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error, 'LastIndexOf', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let text = args1.TextValue;
        if (this.func3 == null) {
            let index = text.lastIndexOf(args2.TextValue);
            return Operand.Create(index + engine.ExcelIndex);
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'LastIndexOf', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        let startIndex = args3.IntValue;
        if (this.func4 == null) {
            let substring = text.substring(0, startIndex);
            let index = substring.lastIndexOf(args2.TextValue);
            return Operand.Create(index + engine.ExcelIndex);
        }
        let args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotNumber) {
            args4 = args4.ToNumber(StringCache.Function_parameter_error, 'LastIndexOf', 4);
            if (args4.IsError) {
                return args4;
            }
        }
        let count = args4.IntValue;
        let endIndex = startIndex + count;
        let substring = text.substring(startIndex, endIndex);
        let index = substring.lastIndexOf(args2.TextValue);
        if (index === -1) {
            return Operand.Create(-1 + engine.ExcelIndex);
        }
        return Operand.Create(index + startIndex + engine.ExcelIndex);
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
}

