import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_COUNTIF extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotArray) {
            args1.ToArray('Function \'{0}\' parameter {1} is error!', 'CountIf', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsError) {
            return args2;
        }
        let list = [];
        let o = FunctionUtil.F_base_GetList(args1, list);
        if (o === false) {
            return Operand.error('Function \'{0}\' parameter {1} is error!', 'CountIf', 1);
        }
        let count;
        if (args2.IsNumber) {
            count = FunctionUtil.F_base_countif(list, args2.NumberValue);
        } else {
            let trimmedText = args2.TextValue.trim();
            let parsedValue = parseFloat(trimmedText);
            if (!isNaN(parsedValue)) {
                count = FunctionUtil.F_base_countif(list, parsedValue);
            } else {
                let sunif = trimmedText;
                let m2 = FunctionUtil.sumifMatch(sunif);
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
        this.AddFunction(stringBuilder, 'CountIf');
    }
}

let FunctionUtil = {
    F_base_GetList(args, list) {
        if (args.IsError) {
            return false;
        }
        if (args.IsNumber) {
            list.push(args.NumberValue);
        } else if (args.IsArray) {
            for (let item of args.ArrayValue) {
                let o = this.F_base_GetList(item, list);
                if (o === false) {
                    return false;
                }
            }
        } else if (args.IsJson) {
            let i = args.ToArray(null);
            if (i.IsError) {
                return false;
            }
            for (let item of i.ArrayValue) {
                let o = this.F_base_GetList(item, list);
                if (o === false) {
                    return false;
                }
            }
        } else {
            let o = args.ToNumber(null);
            if (o.IsError) {
                return false;
            }
            list.push(o.NumberValue);
        }
        return true;
    },

    F_base_countif(dbs, d) {
        let count = 0;
        for (let i = 0; i < dbs.length; i++) {
            let item = dbs[i];
            if (item === d) {
                count++;
            }
        }
        return count;
    },

    F_base_countif(dbs, s, d) {
        let count = 0;
        for (let i = 0; i < dbs.length; i++) {
            let item = dbs[i];
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
        let c = s[0];
        if (c === '>' || c === '＞') {
            if (s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                let d = parseFloat(s.substring(2).trim());
                if (!isNaN(d)) {
                    return ['>=', d];
                }
            } else {
                let d = parseFloat(s.substring(1).trim());
                if (!isNaN(d)) {
                    return ['>', d];
                }
            }
        } else if (c === '<' || c === '＜') {
            if (s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                let d = parseFloat(s.substring(2).trim());
                if (!isNaN(d)) {
                    return ['<=', d];
                }
            } else if (s.length > 1 && (s[1] === '>' || s[1] === '＞')) {
                let d = parseFloat(s.substring(2).trim());
                if (!isNaN(d)) {
                    return ['!=', d];
                }
            } else {
                let d = parseFloat(s.substring(1).trim());
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
            let d = parseFloat(s.substring(index).trim());
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
            let d = parseFloat(s.substring(index).trim());
            if (!isNaN(d)) {
                return ['!=', d];
            }
        }
        return null;
    }
};

export { Function_COUNTIF };
