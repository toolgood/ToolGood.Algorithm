import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_ROMAN extends Function_N {
    get Name() {
        return "ROMAN";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 1) return this.parameterError(1);

        const numArg = this.getNumber(engine, tempParameter, 0);
        if (numArg.IsError) return numArg;
        const num = Math.floor(numArg.NumberValue);

        // Excel: number 为负或大于 3999 时返回 #VALUE!,number 为 0 返回空文本
        if (num < 0 || num > 3999) return this.parameterError(1);

        let form = 0;
        if (this.z.length > 1) {
            const formArg = this.getNumber(engine, tempParameter, 1);
            if (formArg.IsError) return formArg;
            form = Math.floor(formArg.NumberValue);
            // Excel: form 超出 0~4 范围返回 #VALUE!
            if (form < 0 || form > 4) return this.parameterError(2);
        }

        return Operand.Create(this.arabicToRoman(num, form));
    }

    // form 0: 经典形式(标准减法表示)
    // form 1: 增加 V-L=45, V-C=95, L-D=450, L-M=950
    // form 2: 再增加 I-L=49, I-C=99, X-D=490, X-M=990
    // form 3: 再增加 V-D=495, V-M=995
    // form 4: 再增加 I-D=499, I-M=999(最简化)
    arabicToRoman(num, form) {
        if (num === 0) return "";

        const values = this.romanValues[form];
        const numerals = this.romanSymbols[form];

        let result = "";
        for (let i = 0; i < values.length; i++) {
            while (num >= values[i]) {
                result += numerals[i];
                num -= values[i];
            }
        }

        return result;
    }

    get romanValues() {
        return [
            [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1],
            [1000, 950, 900, 500, 450, 400, 100, 95, 90, 50, 45, 40, 10, 9, 5, 4, 1],
            [1000, 990, 950, 900, 500, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1],
            [1000, 995, 990, 950, 900, 500, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1],
            [1000, 999, 995, 990, 950, 900, 500, 499, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1]
        ];
    }

    get romanSymbols() {
        return [
            ["M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"],
            ["M", "LM", "CM", "D", "LD", "CD", "C", "VC", "XC", "L", "VL", "XL", "X", "IX", "V", "IV", "I"],
            ["M", "XM", "LM", "CM", "D", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"],
            ["M", "VM", "XM", "LM", "CM", "D", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"],
            ["M", "IM", "VM", "XM", "LM", "CM", "D", "ID", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"]
        ];
    }
}

export { Function_ROMAN };
