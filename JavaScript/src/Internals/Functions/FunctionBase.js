/**
 * Represents the base class for all function implementations that can be calculated by an algorithm engine.
 */
import { Operand } from '../../Operand.js';

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
}

