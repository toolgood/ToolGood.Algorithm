import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_AVERAGEIF extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let list = [];
        if (args1.IsArray) {
            for (let item of args1.ArrayValue) {
                if (item.IsNumber) {
                    list.push(item.NumberValue);
                }
            }
        } else if (args1.IsNumber) {
            list.push(args1.NumberValue);
        } else {
            return Operand.Error(StringCache.Function_parameter_error, "AverageIf", 1);
        }

        let sumdbs;
        if (this.func3 !== null) {
            let args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.IsError) { return args3; }
            sumdbs = [];
            if (args3.IsArray) {
                for (let item of args3.ArrayValue) {
                    if (item.IsNumber) {
                        sumdbs.push(item.NumberValue);
                    }
                }
            } else if (args3.IsNumber) {
                sumdbs.push(args3.NumberValue);
            } else {
                return Operand.Error(StringCache.Function_parameter_error, "AverageIf", 3);
            }
        } else {
            sumdbs = list;
        }

        let sum = 0;
        let count = 0;

        if (args2.IsNumber) {
            count = Function_AVERAGEIF.countif(list, args2.NumberValue);
            sum = count * args2.NumberValue;
        } else {
            if (args2.IsText) {
                let TextValue = args2.TextValue.trim();
                let parsedValue = parseFloat(TextValue);
                if (!isNaN(parsedValue)) {
                    count = Function_AVERAGEIF.countif(list, parsedValue);
                    sum = Function_AVERAGEIF.sumif(list, parsedValue, sumdbs);
                } else {
                    // 处理匹配模式
                    let matchResult = Function_AVERAGEIF.sumifMatch(TextValue);
                    if (matchResult) {
                        count = Function_AVERAGEIF.countif(list, matchResult[0], matchResult[1]);
                    sum = Function_AVERAGEIF.sumif(list, matchResult[0], sumdbs, matchResult[1]);
                    } else {
                        return Operand.Error(StringCache.Function_parameter_error, "AverageIf", 2);
                    }
                }
            } else {
                return Operand.Error(StringCache.Function_parameter_error, "AverageIf", 2);
            }
        }

        if (count === 0) {
            return Operand.Error(StringCache.Function_error, "AverageIf");
        }

        return Operand.Create(sum / count);
    }

    // 辅助函数：countif
    static countif(list, value, operator = '=') {
        let count = 0;
        for (let item of list) {
            if (Function_AVERAGEIF.compareValues(item, value, operator)) {
                count++;
            }
        }
        return count;
    }

    // 辅助函数：sumif
    static sumif(list, value, sumdbs, operator = '=') {
        let sum = 0;
        for (let i = 0; i < list.length && i < sumdbs.length; i++) {
            if (Function_AVERAGEIF.compareValues(list[i], value, operator)) {
                sum += sumdbs[i];
            }
        }
        return sum;
    }

    // 辅助函数：比较�?
    static compareValues(a, b, operator) {
        switch (operator) {
            case '=':
                return a === b;
            case '>':
                return a > b;
            case '<':
                return a < b;
            case '>=':
                return a >= b;
            case '<=':
                return a <= b;
            case '<>':
                return a !== b;
            default:
                return false;
        }
    }

    // 辅助函数：sumifMatch
    static sumifMatch(pattern) {
        let operators = ['=', '>', '<', '>=', '<=', '<>', '!='];
        for (let op of operators) {
            if (pattern.startsWith(op)) {
                let value = parseFloat(pattern.substring(op.length).trim());
                if (!isNaN(value)) {
                    return [value, op === '!=' ? '<>' : op];
                }
            }
        }
        return null;
    }
}

export { Function_AVERAGEIF };

