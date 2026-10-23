package toolgood.algorithm.calculationlogic;

import toolgood.algorithm.Operand;

/**
 * 计算逻辑信息
 */
public class CalculationLogicInfo {
    /**
     * 类型
     */
    public CalculationLogicType LogicType;
    /**
     * 层级
     */
    public int Layer;
    /**
     * 逻辑名称
     */
    public String Name;
    /**
     * 公式
     */
    public String Exp;
    /**
     * 公式2
     */
    public String Exp2;
    /**
     * 备注
     */
    public String Remark;
    /**
     * 值
     */
    public Operand Value;

    /**
     * 转换为信息字符串
     */
    public String ToInfoString() {
        String remark = (Remark == null || Remark.isEmpty()) ? "" : " // " + Remark;
        String layerStr = Repeat(' ', Math.min(Math.max(0, Layer), 1000) * 3);
        switch (LogicType) {
            case BlankLine:
                return "";
            case Scene:
                return "===== " + Name + " =====";
            case InitValue:
                return Name + "=" + Exp + ";";
            case Condition:
                if (Value.IsError()) {
                    return "[条件][错误] " + layerStr + "if " + Exp + ": // " + Exp2 + " = " + Value.ErrorMsg() + remark;
                }
                if (Value.BooleanValue()) {
                    return "[成功] " + layerStr + "if " + Exp + ": // " + Exp2 + remark;
                }
                return "[失败] " + layerStr + "if " + Exp + ": // " + Exp2 + remark;
            case SetFormula:
                if (Value.IsError()) {
                    return "[赋值][错误] " + layerStr + Name + " = " + Exp + " = " + Exp2 + " // " + Value.ErrorMsg() + remark;
                }
                return "[赋值] " + layerStr + Name + " = " + Exp + " = " + Exp2 + " = " + Value.ToText(null).TextValue() + remark;
            case SetValue:
                return "[赋值] " + layerStr + Name + " = " + Exp + remark;
            case Error:
                return "[错误] " + layerStr + Exp;
            default:
                break;
        }
        return "[错误]: " + layerStr + Name + Exp + remark;
    }

    private static String Repeat(char c, int count) {
        if (count <= 0) return "";
        StringBuilder sb = new StringBuilder(count);
        for (int i = 0; i < count; i++) sb.append(c);
        return sb.toString();
    }
}
