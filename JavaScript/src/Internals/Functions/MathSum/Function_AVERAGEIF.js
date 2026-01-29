import { Function_3 } from '../Function_3.js';

class Function_AVERAGEIF extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        const args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsError) { return args1; }
        const args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.IsError) { return args2; }

        const list = [];
        if (args1.IsArray) {
            for (const item of args1.ArrayValue) {
                if (item.IsNumber) {
                    list.push(item.NumberValue);
                }
            }
        } else if (args1.IsNumber) {
            list.push(args1.NumberValue);
        } else {
            return engine.createErrorOperand("Function '{0}' parameter {1} is error!", "AverageIf", 1);
        }

        let sumdbs;
        if (this._arg3 !== null) {
            const args3 = this._arg3.evaluate(engine, tempParameter);
            if (args3.IsError) { return args3; }
            sumdbs = [];
            if (args3.IsArray) {
                for (const item of args3.ArrayValue) {
                    if (item.IsNumber) {
                        sumdbs.push(item.NumberValue);
                    }
                }
            } else if (args3.IsNumber) {
                sumdbs.push(args3.NumberValue);
            } else {
                return engine.createErrorOperand("Function '{0}' parameter {1} is error!", "AverageIf", 3);
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
                const textValue = args2.TextValue.trim();
                const parsedValue = parseFloat(textValue);
                if (!isNaN(parsedValue)) {
                    count = Function_AVERAGEIF.countif(list, parsedValue);
                    sum = Function_AVERAGEIF.sumif(list, parsedValue, sumdbs);
                } else {
                    // 处理匹配模式
                    const matchResult = Function_AVERAGEIF.sumifMatch(textValue);
                    if (matchResult) {
                        count = Function_AVERAGEIF.countif(list, matchResult[0], matchResult[1]);
                        sum = Function_AVERAGEIF.sumif(list, matchResult[0], matchResult[1], sumdbs);
                    } else {
                        return engine.createErrorOperand("Function '{0}' parameter {1} is error!", "AverageIf", 2);
                    }
                }
            } else {
                return engine.createErrorOperand("Function '{0}' parameter {1} is error!", "AverageIf", 2);
            }
        }

        if (count === 0) {
            return engine.createErrorOperand("Function '{0}' div 0 error!", "AverageIf");
        }

        return engine.createOperand(sum / count);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "AverageIf");
    }

    // 辅助函数：countif
    static countif(list, value, operator = '=') {
        let count = 0;
        for (const item of list) {
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

    // 辅助函数：比较值
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
        const operators = ['=', '>', '<', '>=', '<=', '<>'];
        for (const op of operators) {
            if (pattern.startsWith(op)) {
                const value = parseFloat(pattern.substring(op.length).trim());
                if (!isNaN(value)) {
                    return [value, op];
                }
            }
        }
        return null;
    }
}

export { Function_AVERAGEIF };
