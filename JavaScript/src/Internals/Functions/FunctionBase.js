/**
 * Represents the base class for all function implementations that can be calculated by an algorithm engine.
 */
import { Operand } from '../../Operand.js';
import { OperandType } from '../../Enums/OperandType.js';

export class FunctionBase {
    /**
     * 名称
     */
    get Name() {
        throw new Error('FIXME');
    }
    /**
     * 进行计算
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {Operand}
     */
    evaluate(work, tempParameter = null) {
        throw new Error('FIXME');
    }

    /**
     * Returns a string that represents the current object.
     * @returns {string}
     */
    toString2() {
        const stringBuilder = [];
        this.toString2(stringBuilder, false);
        return stringBuilder.join('');
    }

    /**
     * Appends a string representation of the current object to the specified array, optionally including brackets.
     * @param {Array} stringBuilder The array to which the string representation will be appended.
     * @param {boolean} addBrackets true to enclose the string representation in brackets; otherwise, false.
     */
    toString2(stringBuilder, addBrackets) {
        throw new Error('FIXME');
    }

    /**
     * 转换参数为文本
     * @param {Operand} arg
     * @param {number} paramIndex
     * @returns {Operand}
     */
    convertToText(arg, paramIndex) {
        return arg.ToText("Function '{0}' parameter {1} is error!", this.Name, paramIndex);
    }

    /**
     * 转换参数为布尔值
     * @param {Operand} arg
     * @param {number} paramIndex
     * @returns {Operand}
     */
    convertToBoolean(arg, paramIndex) {
        return arg.ToBoolean("Function '{0}' parameter {1} is error!", this.Name, paramIndex);
    }

    /**
     * 转换参数为数字
     * @param {Operand} arg
     * @param {number} paramIndex
     * @returns {Operand}
     */
    convertToNumber(arg, paramIndex) {
        return arg.ToNumber("Function '{0}' parameter {1} is error!", this.Name, paramIndex);
    }

    /**
     * 转换参数为数组
     * @param {Operand} arg
     * @param {number} paramIndex
     * @returns {Operand}
     */
    convertToArray(arg, paramIndex) {
        return arg.ToArray("Function '{0}' parameter {1} is error!", this.Name, paramIndex);
    }

    /**
     * 转换参数为日期
     * @param {Operand} arg
     * @param {number} paramIndex
     * @returns {Operand}
     */
    convertToDate(arg, paramIndex) {
        return arg.ToMyDate("Function '{0}' parameter {1} is error!", this.Name, paramIndex);
    }

    /**
     * Creates an error operand indicating that a specific function parameter is invalid.
     * @param {number} paramIndex The zero-based index of the parameter that caused the error.
     * @returns {Operand} An operand representing an error for the specified parameter.
     */
    parameterError(paramIndex) {
        return Operand.Error("Function '{0}' parameter {1} is error!", this.Name, paramIndex);
    }

    /**
     * Creates an error operand indicating that a function parameter is invalid.
     * @returns {Operand} An operand representing an error state for the function due to an invalid parameter.
     */
    functionError() {
        return Operand.Error("Function '{0}' parameter is error!", this.Name);
    }

    /**
     * Creates an error operand indicating that a comparison error occurred.
     * @returns {Operand} An operand representing a comparison error.
     */
    compareError() {
        return Operand.Error("Function '{0}' compare is error.", this.Name);
    }

    /**
     * Creates an error operand indicating a division by zero error.
     * @returns {Operand} An operand representing a division by zero error.
     */
    div0Error() {
        return Operand.Error("Function '{0}' Div 0 error!", this.Name);
    }

    /**
     * 获取结果类型（对齐 C# 抽象方法 GetResultType，子类可覆写）
     * @returns {number} OperandType
     */
    getResultType() {
        return OperandType.NONE;
    }

    /**
     * 尝试执行计算，如果出错返回默认值（对齐 C# 私有泛型方法 TryEvaluate<T>）
     * @param {AlgorithmEngine} work
     * @param {*} def 默认值
     * @param {Function} converter 类型转换函数
     * @param {Function} resultConverter 结果提取函数
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {*}
     */
    _tryEvaluate(work, def, converter, resultConverter, tempParameter = null) {
        try {
            const obj = this.evaluate(work, tempParameter);
            const converted = converter(obj);
            if (converted.IsError) {
                work.LastError = converted.ErrorMsg;
                return def;
            }
            return resultConverter(converted);
        } catch (ex) {
            work.LastError = ex.message;
        }
        return def;
    }

    /**
     * 按默认值类型分派（对齐 C# 重载集）。number 无法区分 int/decimal，统一按 decimal 处理。
     * @param {AlgorithmEngine} work
     * @param {*} def 默认值
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {*}
     */
    tryEvaluate(work, def, tempParameter = null) {
        const type = typeof def;
        if (type === 'number') {
            return this.tryEvaluateDecimal(work, def, tempParameter);
        }
        if (type === 'string') {
            return this.tryEvaluateString(work, def, tempParameter);
        }
        if (type === 'boolean') {
            return this.tryEvaluateBoolean(work, def, tempParameter);
        }
        return def;
    }

    /**
     * 尝试执行计算，返回 int 类型，如果出错返回默认值
     * @param {AlgorithmEngine} work
     * @param {number} def
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {number}
     */
    tryEvaluateInt(work, def, tempParameter = null) {
        return this._tryEvaluate(work, def,
            obj => obj.IsNumber ? obj : obj.ToNumber("It can't be converted to number!"),
            obj => obj.IntValue, tempParameter);
    }

    /**
     * 尝试执行计算，返回 decimal 类型，如果出错返回默认值
     * @param {AlgorithmEngine} work
     * @param {number} def
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {number}
     */
    tryEvaluateDecimal(work, def, tempParameter = null) {
        return this._tryEvaluate(work, def,
            obj => obj.IsNumber ? obj : obj.ToNumber("It can't be converted to number!"),
            obj => obj.NumberValue, tempParameter);
    }

    /**
     * 尝试执行计算，返回 string 类型，如果出错返回默认值
     * @param {AlgorithmEngine} work
     * @param {string} def
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {string}
     */
    tryEvaluateString(work, def, tempParameter = null) {
        return this._tryEvaluate(work, def,
            obj => obj.IsText ? obj : obj.ToText("It can't be converted to string!"),
            obj => obj.TextValue, tempParameter);
    }

    /**
     * 尝试执行计算，返回 bool 类型，如果出错返回默认值
     * @param {AlgorithmEngine} work
     * @param {boolean} def
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {boolean}
     */
    tryEvaluateBoolean(work, def, tempParameter = null) {
        return this._tryEvaluate(work, def,
            obj => obj.IsBoolean ? obj : obj.ToBoolean("It can't be converted to bool!"),
            obj => obj.BooleanValue, tempParameter);
    }

    /**
     * 尝试执行计算，返回 DateTime 类型，如果出错返回默认值
     * @param {AlgorithmEngine} work
     * @param {Date} def
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {Date}
     */
    tryEvaluateDateTime(work, def, tempParameter = null) {
        return this._tryEvaluate(work, def,
            obj => obj.IsDate ? obj : obj.ToMyDate("It can't be converted to DateTime!"),
            obj => {
                if (work.UseLocalTime) {
                    return obj.DateValue.ToDateTime(1);
                }
                return obj.DateValue.ToDateTime(0);
            }, tempParameter);
    }

    /**
     * 尝试执行计算，返回 TimeSpan 类型，如果出错返回默认值
     * @param {AlgorithmEngine} work
     * @param {Object} def
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {Object}
     */
    tryEvaluateTimeSpan(work, def, tempParameter = null) {
        return this._tryEvaluate(work, def,
            obj => obj.IsDate ? obj : obj.ToMyDate("It can't be converted to DateTime!"),
            obj => obj.DateValue.ToTimeSpan(), tempParameter);
    }
}

