import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_RMB extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1.ToNumber('Function {0} parameter is error!', 'RMB');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(this.F_base_ToChineseRMB(args1.NumberValue));
    }

    F_base_ToChineseRMB(x) {
        let s = x.toFixed(2).replace(/\d/g, (m, i, str) => {
            let len = str.length - i - 3;
            let chars = "#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A";
            return chars[len * 2 + 1];
        });
        let Regex1 = /((?<=-|^)[^1-9]*)|((\d)0[A-E]*((?=[1-9])|$))|(([F-L])(0)[0A-L]*((?=[1-9])|$))/g;
        let d = s.replace(Regex1, "$2$5$6");
        let Regex2 = /./g;
        let result = d.replace(Regex2, (m) => {
            let chars = "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰";
            return chars[m.charCodeAt(0) - '-'.charCodeAt(0)] || '';
        });
        return result;
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'RMB');
    }
}

export { Function_RMB };
