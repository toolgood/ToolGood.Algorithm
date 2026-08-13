import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_INDEXOF
 */
export class Function_INDEXOF extends Function_4 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "IndexOf";
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getText_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let text = args1.TextValue;
        if (this.c == null) {
            let index = text.indexOf(args2.TextValue);
            return Operand.Create(index + engine.ExcelIndex);
        }

        let args3 = this.getNumber_3(engine, tempParameter);
        if (args3.IsError) { return args3; }

        let startIndex = args3.IntValue - engine.ExcelIndex;
        if (startIndex < 0 || startIndex > text.length) {
            return this.parameterError(3);
        }

        if (this.d == null) {
            let index = text.indexOf(args2.TextValue, startIndex);
            return Operand.Create(index + engine.ExcelIndex);
        }

        let args4 = this.getNumber_4(engine, tempParameter);
        if (args4.IsError) { return args4; }

        let count = args4.IntValue;
        if (count < 0 || startIndex + count > text.length) {
            return this.parameterError(4);
        }

        let substring = text.substring(startIndex, startIndex + count);
        let index = substring.indexOf(args2.TextValue);
        if (index === -1) {
            return Operand.Create(-1 + engine.ExcelIndex);
        }
        return Operand.Create(index + startIndex + engine.ExcelIndex);
    }
    

}

