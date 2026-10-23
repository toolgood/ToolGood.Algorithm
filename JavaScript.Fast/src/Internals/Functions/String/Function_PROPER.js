import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_PROPER extends Function_1 {
    get Name() {
        return "Proper";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        let text = args1.TextValue;
        if (text.length === 0) {
            return Operand.Create(text);
        }
        let needModify = false;
        let isFirst = true;
        for (let i = 0; i < text.length; i++) {
            let t = text[i];
            // 与 C# char.IsLetter 一致:Unicode 字母判定,非字母重置为首字母
            if (!/\p{L}/u.test(t)) {
                isFirst = true;
            } else {
                if (isFirst) {
                    // 首字母为小写字母时需要修改
                    if (t === t.toLowerCase() && t !== t.toUpperCase()) {
                        needModify = true;
                        break;
                    }
                } else {
                    // 非首字母为大写字母时需要修改
                    if (t === t.toUpperCase() && t !== t.toLowerCase()) {
                        needModify = true;
                        break;
                    }
                }
                isFirst = false;
            }
        }
        if (!needModify) {
            return args1; // no change
        }
        let chars = text.split('');
        isFirst = true;
        for (let i = 0; i < chars.length; i++) {
            let t = chars[i];
            if (!/\p{L}/u.test(t)) {
                isFirst = true;
            } else {
                if (isFirst) {
                    chars[i] = t.toUpperCase();
                } else {
                    // 与 C# 一致:非首字母字母转为小写
                    chars[i] = t.toLowerCase();
                }
                isFirst = false;
            }
        }
        return Operand.Create(chars.join(''));
    }
}

export { Function_PROPER };

