import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_FIND extends Function_3 {
    get Name() {
        return "Find";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getText_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        if (this.c === null || this.c === undefined) {
            let index = args2.TextValue.indexOf(args1.TextValue);
            if (index < 0) {
                // 未找到:Excel 模式(索引从1开始)返回错误,C# 模式(索引从0开始)返回 -1
                return work.ExcelIndex === 1 ? this.functionError() : Operand.Create(-1);
            }
            return Operand.Create(index + work.ExcelIndex);
        }
        let count = this.getNumber_3(work, tempParameter);
        if (count.IsError) { return count; }
        let startIndex = count.IntValue - work.ExcelIndex;
        if (startIndex < 0 || startIndex >= args2.TextValue.length) {
            return this.parameterError(3);
        }
        let p2 = args2.TextValue.indexOf(args1.TextValue, startIndex);
        if (p2 < 0) {
            return work.ExcelIndex === 1 ? this.functionError() : Operand.Create(-1);
        }
        return Operand.Create(p2 + startIndex + work.ExcelIndex);
    }
}

export { Function_FIND };

