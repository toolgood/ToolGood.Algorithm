package toolgood.algorithm.calculationlogic;

/**
 * 计算逻辑类型
 */
public enum CalculationLogicType {
    /**
     * 场景
     */
    Scene,
    /**
     * 值
     */
    InitValue,
    /**
     * 条件
     */
    Condition,
    /**
     * 用公式赋值
     */
    SetFormula,
    /**
     * 赋值
     */
    SetValue,
    /**
     * 错误
     */
    Error,
    /**
     * 空行
     */
    BlankLine,
}
