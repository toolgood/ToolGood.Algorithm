import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ROMAN extends Function_2 {
    get Name() {
        return "ROMAN";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let numArg = this.getNumber_1(engine, tempParameter);
        if (numArg.IsError) return numArg;
        let num = numArg.IntValue;

        if (num < 0 || num > 3999) return Operand.Create("");

        let form = 0;
        if (this.b != null) {
            let formArg = this.getNumber_2(engine, tempParameter);
            if (formArg.IsError) return formArg;
            form = formArg.IntValue;
        }

        return Operand.Create(this.arabicToRoman(num, form));
    }

    arabicToRoman(num, form) {
        if (num == 0) return "";

        let sb = "";
        let values = [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1];
        let numerals = ["M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"];

        for (let i = 0; i < values.length; i++) {
            while (num >= values[i]) {
                sb += numerals[i];
                num -= values[i];
            }
        }

        return sb;
    }
}

export { Function_ROMAN };