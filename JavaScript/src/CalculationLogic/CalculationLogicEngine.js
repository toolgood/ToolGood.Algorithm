import { AlgorithmEngineEx } from '../AlgorithmEngineEx.js';
import { AlgorithmEngineHelper } from '../AlgorithmEngineHelper.js';
import { OperandType } from '../Enums/OperandType.js';
import { CalculationLogicType } from './CalculationLogicType.js';
import { CalculationLogicInfo } from './CalculationLogicInfo.js';

/**
 * 计算逻辑引擎
 */
export class CalculationLogicEngine {
    /**
     * @param {boolean} [useCalculationLogicInfo]
     */
    constructor(useCalculationLogicInfo = true) {
        this._useCalculationLogicInfo = useCalculationLogicInfo;
        this._initValueInfos = [];
        this._calculationLogicInfos = [];
        this._engine = new AlgorithmEngineEx();
    }

    // #region SetScene

    /**
     * 设置场景名称
     * @param {string} scene
     */
    SetSceneName(scene) {
        if (this._useCalculationLogicInfo) {
            this._calculationLogicInfos.push(new CalculationLogicInfo({
                LogicType: CalculationLogicType.Scene,
                Name: scene
            }));
        }
    }

    // #endregion

    // #region InitValue

    /**
     * 初始化值
     * @param {string} key
     * @param {*} value
     * @param {number|string} [layer]
     * @param {string} [remark]
     */
    InitValue(key, value, layer = 0, remark = null) {
        if (typeof layer === 'string') { remark = layer; layer = 0; }
        this._engine.AddParameter(key, value);
        if (this._useCalculationLogicInfo) {
            this._initValueInfos.push(new CalculationLogicInfo({
                LogicType: CalculationLogicType.InitValue,
                Name: key,
                Exp: this._initExp(value),
                Layer: layer,
                Remark: remark
            }));
        }
    }

    // #endregion

    // #region CheckCondition

    /**
     * 检查条件
     * @param {string} condition
     * @param {number|string} [layer]
     * @param {string} [remark]
     * @returns {boolean}
     */
    CheckCondition(condition, layer = 0, remark = null) {
        if (typeof layer === 'string') { remark = layer; layer = 0; }
        const func = AlgorithmEngineHelper.ParseFormula(condition);
        let operand = this._engine.Evaluate(func);
        operand = operand.ToBoolean("The condition must be a boolean value!");
        if (operand.IsError) {
            if (this._useCalculationLogicInfo) {
                const expStr = this._expAnalysis(condition);
                this._calculationLogicInfos.push(new CalculationLogicInfo({
                    LogicType: CalculationLogicType.Condition,
                    Exp: condition,
                    Exp2: expStr,
                    Value: operand,
                    Layer: layer,
                    Remark: remark
                }));
            }
            throw new Error(operand.ErrorMsg);
        }
        if (this._useCalculationLogicInfo) {
            const expStr = this._expAnalysis(condition);
            this._calculationLogicInfos.push(new CalculationLogicInfo({
                LogicType: CalculationLogicType.Condition,
                Exp: condition,
                Exp2: expStr,
                Value: operand,
                Layer: layer,
                Remark: remark
            }));
        }
        return operand.BooleanValue;
    }

    // #endregion

    // #region SetFormula

    /**
     * 用公式赋值
     * @param {string} key
     * @param {string} exp
     * @param {number|string} [layer]
     * @param {string} [remark]
     */
    SetFormula(key, exp, layer = 0, remark = null) {
        if (typeof layer === 'string') { remark = layer; layer = 0; }
        const func = AlgorithmEngineHelper.ParseFormula(exp);
        const operand = this._engine.Evaluate(func);
        if (operand.IsError) {
            if (this._useCalculationLogicInfo) {
                const expStr = this._expAnalysis(exp);
                this._calculationLogicInfos.push(new CalculationLogicInfo({
                    LogicType: CalculationLogicType.SetFormula,
                    Name: key,
                    Exp: exp,
                    Exp2: expStr,
                    Value: operand,
                    Layer: layer,
                    Remark: remark
                }));
            }
            throw new Error(operand.ErrorMsg);
        }
        this._engine.AddParameter(key, operand);

        if (this._useCalculationLogicInfo) {
            const expStr = this._expAnalysis(exp);
            this._calculationLogicInfos.push(new CalculationLogicInfo({
                LogicType: CalculationLogicType.SetFormula,
                Name: key,
                Exp: exp,
                Exp2: expStr,
                Value: operand,
                Layer: layer,
                Remark: remark
            }));
        }
    }

    // #endregion

    // #region SetValue

    /**
     * 设置值
     * @param {string} key
     * @param {*} value
     * @param {number|string} [layer]
     * @param {string} [remark]
     */
    SetValue(key, value, layer = 0, remark = null) {
        if (typeof layer === 'string') { remark = layer; layer = 0; }
        this._engine.AddParameter(key, value);
        if (this._useCalculationLogicInfo) {
            this._calculationLogicInfos.push(new CalculationLogicInfo({
                LogicType: CalculationLogicType.SetValue,
                Name: key,
                Exp: this._valueExp(value),
                Layer: layer,
                Remark: remark
            }));
        }
    }

    // #endregion

    // #region ExpAnalysis

    _expAnalysis(exp) {
        const diyNameInfo = AlgorithmEngineHelper.GetDiyNames(exp);
        let index = 0;
        let result = '';
        const parameters = diyNameInfo.Parameters || [];
        for (let i = 0; i < parameters.length; i++) {
            const parameter = parameters[i];
            if (index < parameter.Start) {
                result += exp.substring(index, parameter.Start);
            }
            result += this._getOperand(this._engine, parameter.Name);
            index = parameter.End + 1;
        }
        if (index < exp.length) {
            result += exp.substring(index);
        }
        return result;
    }

    _getOperand(engine, name) {
        const operand = engine.GetParameter(name);
        if (operand.IsError) {
            return 'error("' + operand.ErrorMsg.replace(/\\/g, '\\\\').replace(/"/g, '\\"') + '")';
        } else if (operand.Type === OperandType.TEXT) {
            return operand.toString();
        }
        return operand.ToText(null).TextValue;
    }

    _initExp(value) {
        if (typeof value === 'string') return value;
        if (typeof value === 'boolean') return value ? 'True' : 'False';
        return String(value);
    }

    _valueExp(value) {
        if (typeof value === 'string') return '"' + value.replace(/"/g, '\\"') + '"';
        if (typeof value === 'boolean') return value ? 'True' : 'False';
        return String(value);
    }

    // #endregion

    // #region GetValue

    /**
     * 获取值
     * @param {string} key
     * @returns {Operand}
     */
    GetValue(key) {
        return this._engine.GetParameter(key);
    }

    // #endregion

    // #region BlankLine

    /**
     * 添加空行
     */
    BlankLine() {
        if (this._useCalculationLogicInfo) {
            this._calculationLogicInfos.push(new CalculationLogicInfo({
                LogicType: CalculationLogicType.BlankLine
            }));
        }
    }

    // #endregion

    // #region ToInfoString

    /**
     * 转换为信息字符串
     * @returns {string}
     */
    ToInfoString() {
        if (this._useCalculationLogicInfo) {
            let sb = '';
            if (this._initValueInfos.length > 0) {
                sb += '[初始] ';
                for (const item of this._initValueInfos) {
                    sb += item.ToInfoString();
                }
                sb += '\n';
            }
            for (const item of this._calculationLogicInfos) {
                sb += item.ToInfoString() + '\n';
            }
            return sb;
        }
        return '';
    }

    // #endregion
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.CalculationLogicEngine = CalculationLogicEngine;
}
