import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_COUNTIF extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotArray) {
            args1.toArray('Function \'{0}\' parameter {1} is error!', 'CountIf', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.isError) {
            return args2;
        }
        const list = [];
        const o = FunctionUtil.F_base_GetList(args1, list);
        if (o === false) {
            return Operand.error('Function \'{0}\' parameter {1} is error!', 'CountIf', 1);
        }
        let count;
        if (args2.isNumber) {
            count = FunctionUtil.F_base_countif(list, args2.numberValue);
        } else {
            const trimmedText = args2.textValue.trim();
            const parsedValue = parseFloat(trimmedText);
            if (!isNaN(parsedValue)) {
                count = FunctionUtil.F_base_countif(list, parsedValue);
            } else {
                const sunif = trimmedText;
                const m2 = FunctionUtil.sumifMatch(sunif);
                if (m2 !== null) {
                    count = FunctionUtil.F_base_countif(list, m2[0], m2[1]);
                } else {
                    return Operand.error('Function \'{0}\' parameter {1} is error!', 'CountIf', 2);
                }
            }
        }
        return Operand.Create(count);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'CountIf');
    }
}

const FunctionUtil = {
    F_base_GetList(args, list) {
        if (args.isError) {
            return false;
        }
        if (args.isNumber) {
            list.push(args.numberValue);
        } else if (args.isArray) {
            for (const item of args.arrayValue) {
                const o = this.F_base_GetList(item, list);
                if (o === false) {
                    return false;
                }
            }
        } else if (args.isJson) {
            const i = args.toArray(null);
            if (i.isError) {
                return false;
            }
            for (const item of i.arrayValue) {
                const o = this.F_base_GetList(item, list);
                if (o === false) {
                    return false;
                }
            }
        } else {
            const o = args.ToNumber(null);
            if (o.isError) {
                return false;
            }
            list.push(o.numberValue);
        }
        return true;
    },

    F_base_countif(dbs, d) {
        let count = 0;
        for (let i = 0; i < dbs.length; i++) {
            const item = dbs[i];
            if (item === d) {
                count++;
            }
        }
        return count;
    },

    F_base_countif(dbs, s, d) {
        let count = 0;
        for (let i = 0; i < dbs.length; i++) {
            const item = dbs[i];
            if (this.F_base_compare(item, d, s)) {
                count++;
            }
        }
        return count;
    },

    F_base_compare(a, b, ss) {
        if (ss === '<') {
            return a < b;
        } else if (ss === '<=') {
            return a <= b;
        } else if (ss === '>') {
            return a > b;
        } else if (ss === '>=') {
            return a >= b;
        } else if (ss === '=') {
            return a === b;
        }
        return a !== b;
    },

    sumifMatch(s) {
        const c = s[0];
        if (c === '>' || c === '＞') {
            if (s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                const d = parseFloat(s.substring(2).trim());
                if (!isNaN(d)) {
                    return ['>=', d];
                }
            } else {
                const d = parseFloat(s.substring(1).trim());
                if (!isNaN(d)) {
                    return ['>', d];
                }
            }
        } else if (c === '<' || c === '＜') {
            if (s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                const d = parseFloat(s.substring(2).trim());
                if (!isNaN(d)) {
                    return ['<=', d];
                }
            } else if (s.length > 1 && (s[1] === '>' || s[1] === '＞')) {
                const d = parseFloat(s.substring(2).trim());
                if (!isNaN(d)) {
                    return ['!=', d];
                }
            } else {
                const d = parseFloat(s.substring(1).trim());
                if (!isNaN(d)) {
                    return ['<', d];
                }
            }
        } else if (c === '=' || c === '＝') {
            let index = 1;
            if (s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                index = 2;
                if (s.length > 2 && (s[2] === '=' || s[2] === '＝')) {
                    index = 3;
                }
            }
            const d = parseFloat(s.substring(index).trim());
            if (!isNaN(d)) {
                return ['=', d];
            }
        } else if (c === '!' || c === '！') {
            let index = 1;
            if (s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                index = 2;
                if (s.length > 2 && (s[2] === '=' || s[2] === '＝')) {
                    index = 3;
                }
            }
            const d = parseFloat(s.substring(index).trim());
            if (!isNaN(d)) {
                return ['!=', d];
            }
        }
        return null;
    }
};

export { Function_COUNTIF };
