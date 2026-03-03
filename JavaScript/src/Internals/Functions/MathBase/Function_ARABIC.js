import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ARABIC extends Function_1 {
    get Name() {
        return "ARABIC";
    }

    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const arg = this.getText_1(engine, tempParameter);
        if (arg.IsError) return arg;
        const text = arg.TextValue.toUpperCase();
        return Operand.Create(this.romanToArabic(text));
    }

    romanToArabic(roman) {
        let result = 0;
        let prevValue = 0;

        for (let i = roman.length - 1; i >= 0; i--) {
            const value = this.getRomanValue(roman[i]);
            if (value < prevValue) {
                result -= value;
            } else {
                result += value;
            }
            prevValue = value;
        }

        return result;
    }

    getRomanValue(c) {
        switch (c) {
            case 'I': return 1;
            case 'V': return 5;
            case 'X': return 10;
            case 'L': return 50;
            case 'C': return 100;
            case 'D': return 500;
            case 'M': return 1000;
            default: return 0;
        }
    }
}

export { Function_ARABIC };
