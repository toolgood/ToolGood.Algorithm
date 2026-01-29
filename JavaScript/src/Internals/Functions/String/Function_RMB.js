import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_RMB extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function {0} parameter is error!', 'RMB');
            if (args1.isError) {
                return args1;
            }
        }
        return Operand.create(this.F_base_ToChineseRMB(args1.numberValue));
    }

    F_base_ToChineseRMB(x) {
        const s = x.toFixed(2).replace(/\d/g, (m, i, str) => {
            const len = str.length - i - 3;
            const chars = "#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A";
            return chars[len * 2 + 1];
        });
        const Regex1 = /((?<=-|^)[^1-9]*)|((\d)0[A-E]*((?=[1-9])|$))|(([F-L])(0)[0A-L]*((?=[1-9])|$))/g;
        const d = s.replace(Regex1, "$2$5$6");
        const Regex2 = /./g;
        const result = d.replace(Regex2, (m) => {
            const chars = "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰";
            return chars[m.charCodeAt(0) - '-'.charCodeAt(0)] || '';
        });
        return result;
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'RMB');
    }
}

export { Function_RMB };
