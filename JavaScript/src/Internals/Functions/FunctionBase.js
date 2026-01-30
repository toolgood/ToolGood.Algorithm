/**
 * Represents the base class for all function implementations that can be calculated by an algorithm engine.
 */
export class FunctionBase {
    /**
     * 进行计算
     * @param {AlgorithmEngine} work
     * @param {Function} tempParameter 临时参数，未找到返回null
     * @returns {Operand}
     */
    Evaluate(work, tempParameter = null) {
        throw new Error('Not implemented');
    }

    /**
     * 执行函数,如果异常，返回默认�?
     * @param {AlgorithmEngine} work
     * @param {number} def
     * @param {Function} tempParameter
     * @returns {number}
     */
    TryEvaluate(work, def, tempParameter = null) {
        try {
            var obj = this.Evaluate(work, tempParameter);
            if(obj.IsNotNumber) {
                obj = obj.ToNumber("It can't be converted to number!");
                if(obj.IsError) {
                    work.LastError = obj.ErrorMsg;
                    return def;
                }
            }
            return obj.IntValue;
        } catch(ex) {
            work.LastError = ex.message + "\r\n" + ex.stack;
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     * @param {AlgorithmEngine} work
     * @param {number} def
     * @param {Function} tempParameter
     * @returns {number}
     */
    TryEvaluateNumber(work, def, tempParameter = null) {
        try {
            var obj = this.Evaluate(work, tempParameter);
            if(obj.IsNotNumber) {
                obj = obj.ToNumber("It can't be converted to number!");
                if(obj.IsError) {
                    work.LastError = obj.ErrorMsg;
                    return def;
                }
            }
            return obj.NumberValue;
        } catch(ex) {
            work.LastError = ex.message + "\r\n" + ex.stack;
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     * @param {AlgorithmEngine} work
     * @param {boolean} def
     * @param {Function} tempParameter
     * @returns {boolean}
     */
    TryEvaluateBool(work, def, tempParameter = null) {
        try {
            var obj = this.Evaluate(work, tempParameter);
            if(obj.IsNotBool) {
                obj = obj.ToBool("It can't be converted to bool!");
                if(obj.IsError) {
                    work.LastError = obj.ErrorMsg;
                    return def;
                }
            }
            return obj.BoolValue;
        } catch(ex) {
            work.LastError = ex.message + "\r\n" + ex.stack;
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     * @param {AlgorithmEngine} work
     * @param {string} def
     * @param {Function} tempParameter
     * @returns {string}
     */
    TryEvaluateString(work, def, tempParameter = null) {
        try {
            var obj = this.Evaluate(work, tempParameter);
            if(obj.IsNotText) {
                obj = obj.ToText("It can't be converted to Text!");
                if(obj.IsError) {
                    work.LastError = obj.ErrorMsg;
                    return def;
                }
            }
            return obj.TextValue;
        } catch(ex) {
            work.LastError = ex.message + "\r\n" + ex.stack;
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     * @param {AlgorithmEngine} work
     * @param {Date} def
     * @param {Function} tempParameter
     * @returns {Date}
     */
    TryEvaluateDate(work, def, tempParameter = null) {
        try {
            var obj = this.Evaluate(work, tempParameter);
            if(obj.IsNotDate) {
                obj = obj.ToDate("It can't be converted to date!");
                if(obj.IsError) {
                    work.LastError = obj.ErrorMsg;
                    return def;
                }
            }
            return obj.DateValue;
        } catch(ex) {
            work.LastError = ex.message + "\r\n" + ex.stack;
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     * @param {AlgorithmEngine} work
     * @param {Array} def
     * @param {Function} tempParameter
     * @returns {Array}
     */
    TryEvaluateArray(work, def, tempParameter = null) {
        try {
            var obj = this.Evaluate(work, tempParameter);
            if(obj.IsNotArray) {
                obj = obj.ToArray("It can't be converted to array!");
                if(obj.IsError) {
                    work.LastError = obj.ErrorMsg;
                    return def;
                }
            }
            return obj.ArrayValue;
        } catch(ex) {
            work.LastError = ex.message + "\r\n" + ex.stack;
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     * @param {AlgorithmEngine} work
     * @param {Object} def
     * @param {Function} tempParameter
     * @returns {Object}
     */
    TryEvaluateJson(work, def, tempParameter = null) {
        try {
            var obj = this.Evaluate(work, tempParameter);
            if(obj.IsNotJson) {
                obj = obj.ToJson("It can't be converted to json!");
                if(obj.IsError) {
                    work.LastError = obj.ErrorMsg;
                    return def;
                }
            }
            return obj.JsonValue;
        } catch(ex) {
            work.LastError = ex.message + "\r\n" + ex.stack;
        }
        return def;
    }

    toString(){
        const stringBuilder=[];
        this.toString(stringBuilder,false);
        return stringBuilder.join("");
    }
}

