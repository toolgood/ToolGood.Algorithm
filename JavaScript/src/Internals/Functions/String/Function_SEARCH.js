import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_SEARCH extends Function_3 {
    get Name() {
        return "Search";
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
            let index = args2.TextValue.toLowerCase().indexOf(args1.TextValue.toLowerCase());
            if (index < 0) {
                // 未找到:Excel 模式(索引从1开始)返回错误,C# 模式(索引从0开始)返回 -1
                return work.ExcelIndex === 1 ? this.functionError() : Operand.Create(-1);
            }
            return Operand.Create(index + work.ExcelIndex);
        }
        let args3 = this.getNumber_3(work, tempParameter);
        if (args3.IsError) { return args3; }
        let startIndex = args3.IntValue - work.ExcelIndex;
        if (startIndex < 0 || startIndex >= args2.TextValue.length) {
            return this.functionError();
        }
        let p2 = args2.TextValue.toLowerCase().indexOf(args1.TextValue.toLowerCase(), startIndex);
        if (p2 < 0) {
            return work.ExcelIndex === 1 ? this.functionError() : Operand.Create(-1);
        }
        return Operand.Create(p2 + startIndex + work.ExcelIndex);
    }
}

export { Function_SEARCH };

