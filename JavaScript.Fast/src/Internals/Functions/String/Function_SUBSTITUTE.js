import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_SUBSTITUTE extends Function_4 {
    get Name() {
        return "Substitute";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getText_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        let args3 = this.getText_3(work, tempParameter);
        if (args3.IsError) { return args3; }
        if (this.d === null || this.d === undefined) {
            if (args2.TextValue.length === 0) {
                return Operand.Create(args1.TextValue);
            }
            return Operand.Create(args1.TextValue.replace(new RegExp(args2.TextValue.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'g'), args3.TextValue));
        }
        let args4 = this.getNumber_4(work, tempParameter);
        if (args4.IsError) { return args4; }
        let text = args1.TextValue;
        let oldtext = args2.TextValue;
        let newtext = args3.TextValue;
        let index = args4.IntValue;

        // 与 C# 一致:旧文本为空时返回原文
        if (oldtext.length === 0) {
            return Operand.Create(text);
        }
        if (index < 1) {
            return this.parameterError(4);
        }

        let foundCount = 0;
        let searchPos = 0;
        while (searchPos <= text.length - oldtext.length) {
            let foundPos = text.indexOf(oldtext, searchPos);
            if (foundPos < 0) break;
            foundCount++;
            if (foundCount === index) {
                // 替换第 index 次匹配
                return Operand.Create(text.substring(0, foundPos) + newtext + text.substring(foundPos + oldtext.length));
            }
            searchPos = foundPos + oldtext.length;
        }
        // 找不到第 index 次匹配,返回原文
        return Operand.Create(text);
    }
}

export { Function_SUBSTITUTE };

