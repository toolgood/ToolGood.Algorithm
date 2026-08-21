import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_LASTINDEXOF
 */
export class Function_LASTINDEXOF extends Function_4 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "LastIndexOf";
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
        let searchStr = args2.TextValue;
        if (this.c == null) {
            let index = text.lastIndexOf(searchStr);
            return Operand.Create(index + engine.ExcelIndex);
        }

        let args3 = this.getNumber_3(engine, tempParameter);
        if (args3.IsError) { return args3; }

        let startIndex = args3.IntValue - engine.ExcelIndex;
        if (startIndex < 0 || startIndex > text.length) {
            return this.parameterError(3);
        }

        if (this.d == null) {
            // C# LastIndexOf(str, startIndex): 匹配的最后一个字符位置 ≤ startIndex
            // JS lastIndexOf(str, fromIndex): 匹配的第一个字符位置 ≤ fromIndex
            // 转换: fromIndex = startIndex - searchStr.length + 1
            let fromIndex = startIndex - searchStr.length + 1;
            // JS 对负数 fromIndex 会将其视为 0 搜索整个字符串，需手动返回 -1
            if (fromIndex < 0 && searchStr.length > 0) {
                return Operand.Create(-1 + engine.ExcelIndex);
            }
            let index = text.lastIndexOf(searchStr, fromIndex);
            return Operand.Create(index + engine.ExcelIndex);
        }

        let args4 = this.getNumber_4(engine, tempParameter);
        if (args4.IsError) { return args4; }

        let count = args4.IntValue;
        if (count < 0 || count > startIndex + 1) {
            return this.parameterError(4);
        }

        // C# LastIndexOf(str, startIndex, count): 在 [startIndex-count+1, startIndex] 窗口内搜索
        // 使用 substring 窗口法确保语义一致
        let offset = startIndex - count + 1;
        let localIndex = text.substring(offset, startIndex + 1).lastIndexOf(searchStr);
        let index = localIndex === -1 ? -1 : localIndex + offset;
        return Operand.Create(index + engine.ExcelIndex);
    }
    

}

