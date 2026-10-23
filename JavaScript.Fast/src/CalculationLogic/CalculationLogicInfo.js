import { CalculationLogicType } from './CalculationLogicType.js';

/**
 * 计算逻辑信息
 */
export class CalculationLogicInfo {
    /**
     * @param {object} [options]
     * @param {string} [options.LogicType] 类型
     * @param {number} [options.Layer] 层级
     * @param {string} [options.Name] 逻辑名称
     * @param {string} [options.Exp] 公式
     * @param {string} [options.Exp2] 公式2
     * @param {string} [options.Remark] 备注
     * @param {import('../Operand.js').Operand} [options.Value] 值
     */
    constructor(options = {}) {
        this.LogicType = options.LogicType ?? null;
        this.Layer = options.Layer ?? 0;
        this.Name = options.Name ?? null;
        this.Exp = options.Exp ?? null;
        this.Exp2 = options.Exp2 ?? null;
        this.Remark = options.Remark ?? null;
        this.Value = options.Value ?? null;
    }

    /**
     * 转换为信息字符串
     * @returns {string}
     */
    ToInfoString() {
        const remark = (this.Remark === null || this.Remark === '') ? '' : ' // ' + this.Remark;
        const layerStr = ' '.repeat(Math.min(Math.max(0, this.Layer), 1000) * 3);
        switch (this.LogicType) {
            case CalculationLogicType.BlankLine:
                return '';
            case CalculationLogicType.Scene:
                return '===== ' + this.Name + ' =====';
            case CalculationLogicType.InitValue:
                return this.Name + '=' + this.Exp + ';';
            case CalculationLogicType.Condition:
                if (this.Value.IsError) {
                    return '[条件][错误] ' + layerStr + 'if ' + this.Exp + ': // ' + this.Exp2 + ' = ' + this.Value.ErrorMsg + remark;
                }
                if (this.Value.BooleanValue) {
                    return '[成功] ' + layerStr + 'if ' + this.Exp + ': // ' + this.Exp2 + remark;
                }
                return '[失败] ' + layerStr + 'if ' + this.Exp + ': // ' + this.Exp2 + remark;
            case CalculationLogicType.SetFormula:
                if (this.Value.IsError) {
                    return '[赋值][错误] ' + layerStr + this.Name + ' = ' + this.Exp + ' = ' + this.Exp2 + ' // ' + this.Value.ErrorMsg + remark;
                }
                return '[赋值] ' + layerStr + this.Name + ' = ' + this.Exp + ' = ' + this.Exp2 + ' = ' + this.Value.ToText(null).TextValue + remark;
            case CalculationLogicType.SetValue:
                return '[赋值] ' + layerStr + this.Name + ' = ' + this.Exp + remark;
            case CalculationLogicType.Error:
                return '[错误] ' + layerStr + this.Exp;
            default:
                break;
        }
        return '[错误]: ' + layerStr + this.Name + this.Exp + remark;
    }
}
