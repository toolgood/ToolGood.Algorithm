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

        if (num < 0 || num > 3999) return Operand.Create("");

        let form = 0;
        if (this.z.length > 1) {
            const formArg = this.getNumber(engine, tempParameter, 1);
            if (formArg.IsError) return formArg;
            form = Math.floor(formArg.NumberValue);
        }

        return Operand.Create(this.arabicToRoman(num, form));
    }

    arabicToRoman(num, form) {
        if (num === 0) return "";

        const values = [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1];
        const numerals = ["M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"];

        let result = "";
        for (let i = 0; i < values.length; i++) {
            while (num >= values[i]) {
                result += numerals[i];
                num -= values[i];
            }
        }

        return result;
    }
}

export { Function_ROMAN };
