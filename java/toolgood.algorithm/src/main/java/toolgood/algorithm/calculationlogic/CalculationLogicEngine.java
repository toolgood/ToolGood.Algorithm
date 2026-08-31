package toolgood.algorithm.calculationlogic;

import toolgood.algorithm.AlgorithmEngineEx;
import toolgood.algorithm.AlgorithmEngineHelper;
import toolgood.algorithm.FunctionCache;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.DiyNameInfo;
import toolgood.algorithm.internals.DiyNameKeyInfo;
import toolgood.algorithm.internals.functions.FunctionBase;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.Supplier;

/**
 * 计算逻辑引擎
 */
public class CalculationLogicEngine {
    private final FunctionCache _functionCache;
    private final List<CalculationLogicInfo> _initValueInfos = new ArrayList<>();
    private final List<CalculationLogicInfo> _calculationLogicInfos = new ArrayList<>();
    private final boolean _useCalculationLogicInfo;
    private final AlgorithmEngineEx _engine;

    /**
     * 计算逻辑引擎
     */
    public CalculationLogicEngine(FunctionCache functionCache, boolean useCalculationLogicInfo) {
        _functionCache = functionCache;
        _useCalculationLogicInfo = useCalculationLogicInfo;
        _engine = new AlgorithmEngineEx();
    }

    // #region SetScene

    /**
     * 设置场景名称
     */
    public void SetSceneName(String scene) {
        if (_useCalculationLogicInfo) {
            CalculationLogicInfo info = new CalculationLogicInfo();
            info.LogicType = CalculationLogicType.Scene;
            info.Name = scene;
            _calculationLogicInfos.add(info);
        }
    }

    // #endregion

    // #region InitValue

    /**
     * 初始化值
     */
    public void InitValue(String key, String value) {
        InitValue(key, value, 0, null);
    }

    public void InitValue(String key, String value, int layer) {
        InitValue(key, value, layer, null);
    }

    public void InitValue(String key, String value, String remark) {
        InitValue(key, value, 0, remark);
    }

    public void InitValue(String key, String value, int layer, String remark) {
        InitValueCore(key, Operand.Create(value), () -> value, layer, remark);
    }

    /**
     * 初始化值
     */
    public void InitValue(String key, BigDecimal value) {
        InitValue(key, value, 0, null);
    }

    public void InitValue(String key, BigDecimal value, int layer) {
        InitValue(key, value, layer, null);
    }

    public void InitValue(String key, BigDecimal value, String remark) {
        InitValue(key, value, 0, remark);
    }

    public void InitValue(String key, BigDecimal value, int layer, String remark) {
        InitValueCore(key, Operand.Create(value), value::toPlainString, layer, remark);
    }

    /**
     * 初始化值
     */
    public void InitValue(String key, double value) {
        InitValue(key, value, 0, null);
    }

    public void InitValue(String key, double value, int layer) {
        InitValue(key, value, layer, null);
    }

    public void InitValue(String key, double value, String remark) {
        InitValue(key, value, 0, remark);
    }

    public void InitValue(String key, double value, int layer, String remark) {
        InitValueCore(key, Operand.Create(value), () -> Double.toString(value), layer, remark);
    }

    /**
     * 初始化值
     */
    public void InitValue(String key, boolean value) {
        InitValue(key, value, 0, null);
    }

    public void InitValue(String key, boolean value, int layer) {
        InitValue(key, value, layer, null);
    }

    public void InitValue(String key, boolean value, String remark) {
        InitValue(key, value, 0, remark);
    }

    public void InitValue(String key, boolean value, int layer, String remark) {
        InitValueCore(key, Operand.Create(value), () -> value ? "True" : "False", layer, remark);
    }

    /**
     * 初始化值
     */
    public void InitValue(String key, int value) {
        InitValue(key, value, 0, null);
    }

    public void InitValue(String key, int value, int layer) {
        InitValue(key, value, layer, null);
    }

    public void InitValue(String key, int value, String remark) {
        InitValue(key, value, 0, remark);
    }

    public void InitValue(String key, int value, int layer, String remark) {
        InitValueCore(key, Operand.Create(value), () -> Integer.toString(value), layer, remark);
    }

    private void InitValueCore(String key, Operand value, Supplier<String> expFactory, int layer, String remark) {
        _engine.AddParameter(key, value);
        if (_useCalculationLogicInfo) {
            CalculationLogicInfo info = new CalculationLogicInfo();
            info.LogicType = CalculationLogicType.InitValue;
            info.Name = key;
            info.Exp = expFactory.get();
            info.Layer = layer;
            info.Remark = remark;
            _initValueInfos.add(info);
        }
    }

    // #endregion

    // #region CheckCondition

    /**
     * 检查条件
     */
    public boolean CheckCondition(String condition) {
        return CheckCondition(condition, 0, null);
    }

    public boolean CheckCondition(String condition, int layer) {
        return CheckCondition(condition, layer, null);
    }

    public boolean CheckCondition(String condition, String remark) {
        return CheckCondition(condition, 0, remark);
    }

    public boolean CheckCondition(String condition, int layer, String remark) {
        FunctionBase func = _functionCache.ParseWithCache(condition);
        Operand operand = _engine.Evaluate(func);
        operand = operand.ToBoolean("The condition must be a boolean value!");
        if (operand.IsError()) {
            if (_useCalculationLogicInfo) {
                String expStr = ExpAnalysis(condition);
                CalculationLogicInfo info = new CalculationLogicInfo();
                info.LogicType = CalculationLogicType.Condition;
                info.Exp = condition;
                info.Exp2 = expStr;
                info.Value = operand;
                info.Layer = layer;
                info.Remark = remark;
                _calculationLogicInfos.add(info);
            }
            throw new IllegalArgumentException(operand.ErrorMsg());
        }
        if (_useCalculationLogicInfo) {
            String expStr = ExpAnalysis(condition);
            CalculationLogicInfo info = new CalculationLogicInfo();
            info.LogicType = CalculationLogicType.Condition;
            info.Exp = condition;
            info.Exp2 = expStr;
            info.Value = operand;
            info.Layer = layer;
            info.Remark = remark;
            _calculationLogicInfos.add(info);
        }
        return operand.BooleanValue();
    }

    // #endregion

    // #region SetFormula

    /**
     * 用公式赋值
     */
    public void SetFormula(String key, String exp) {
        SetFormula(key, exp, 0, null);
    }

    public void SetFormula(String key, String exp, int layer) {
        SetFormula(key, exp, layer, null);
    }

    public void SetFormula(String key, String exp, String remark) {
        SetFormula(key, exp, 0, remark);
    }

    public void SetFormula(String key, String exp, int layer, String remark) {
        FunctionBase func = _functionCache.ParseWithCache(exp);
        Operand operand = _engine.Evaluate(func);
        if (operand.IsError()) {
            if (_useCalculationLogicInfo) {
                String expStr = ExpAnalysis(exp);
                CalculationLogicInfo info = new CalculationLogicInfo();
                info.LogicType = CalculationLogicType.SetFormula;
                info.Name = key;
                info.Exp = exp;
                info.Exp2 = expStr;
                info.Value = operand;
                info.Layer = layer;
                info.Remark = remark;
                _calculationLogicInfos.add(info);
            }
            throw new IllegalArgumentException(operand.ErrorMsg());
        }
        _engine.AddParameter(key, operand);

        if (_useCalculationLogicInfo) {
            String expStr = ExpAnalysis(exp);
            CalculationLogicInfo info = new CalculationLogicInfo();
            info.LogicType = CalculationLogicType.SetFormula;
            info.Name = key;
            info.Exp = exp;
            info.Exp2 = expStr;
            info.Value = operand;
            info.Layer = layer;
            info.Remark = remark;
            _calculationLogicInfos.add(info);
        }
    }

    // #endregion

    // #region SetValue

    /**
     * 设置值
     */
    public void SetValue(String key, String value) {
        SetValue(key, value, 0, null);
    }

    public void SetValue(String key, String value, int layer) {
        SetValue(key, value, layer, null);
    }

    public void SetValue(String key, String value, String remark) {
        SetValue(key, value, 0, remark);
    }

    public void SetValue(String key, String value, int layer, String remark) {
        SetValueCore(key, Operand.Create(value), () -> "\"" + value.replace("\"", "\\\"") + "\"", layer, remark);
    }

    /**
     * 设置值
     */
    public void SetValue(String key, BigDecimal value) {
        SetValue(key, value, 0, null);
    }

    public void SetValue(String key, BigDecimal value, int layer) {
        SetValue(key, value, layer, null);
    }

    public void SetValue(String key, BigDecimal value, String remark) {
        SetValue(key, value, 0, remark);
    }

    public void SetValue(String key, BigDecimal value, int layer, String remark) {
        SetValueCore(key, Operand.Create(value), value::toPlainString, layer, remark);
    }

    /**
     * 设置值
     */
    public void SetValue(String key, double value) {
        SetValue(key, value, 0, null);
    }

    public void SetValue(String key, double value, int layer) {
        SetValue(key, value, layer, null);
    }

    public void SetValue(String key, double value, String remark) {
        SetValue(key, value, 0, remark);
    }

    public void SetValue(String key, double value, int layer, String remark) {
        SetValueCore(key, Operand.Create(value), () -> Double.toString(value), layer, remark);
    }

    /**
     * 设置值
     */
    public void SetValue(String key, boolean value) {
        SetValue(key, value, 0, null);
    }

    public void SetValue(String key, boolean value, int layer) {
        SetValue(key, value, layer, null);
    }

    public void SetValue(String key, boolean value, String remark) {
        SetValue(key, value, 0, remark);
    }

    public void SetValue(String key, boolean value, int layer, String remark) {
        SetValueCore(key, Operand.Create(value), () -> value ? "True" : "False", layer, remark);
    }

    /**
     * 设置值
     */
    public void SetValue(String key, int value) {
        SetValue(key, value, 0, null);
    }

    public void SetValue(String key, int value, int layer) {
        SetValue(key, value, layer, null);
    }

    public void SetValue(String key, int value, String remark) {
        SetValue(key, value, 0, remark);
    }

    public void SetValue(String key, int value, int layer, String remark) {
        SetValueCore(key, Operand.Create(value), () -> Integer.toString(value), layer, remark);
    }

    private void SetValueCore(String key, Operand value, Supplier<String> expFactory, int layer, String remark) {
        _engine.AddParameter(key, value);
        if (_useCalculationLogicInfo) {
            CalculationLogicInfo info = new CalculationLogicInfo();
            info.LogicType = CalculationLogicType.SetValue;
            info.Name = key;
            info.Exp = expFactory.get();
            info.Layer = layer;
            info.Remark = remark;
            _calculationLogicInfos.add(info);
        }
    }

    // #endregion

    // #region ExpAnalysis

    private String ExpAnalysis(String exp) {
        try {
            DiyNameInfo diyNameInfo = AlgorithmEngineHelper.GetDiyNames(exp);
            int index = 0;
            StringBuilder stringBuilder = new StringBuilder();
            List<DiyNameKeyInfo> parameters = diyNameInfo.Parameters;
            for (int i = 0; i < parameters.size(); i++) {
                DiyNameKeyInfo parameter = parameters.get(i);
                if (index < parameter.Start) {
                    stringBuilder.append(exp.substring(index, parameter.Start));
                }
                String formulaItem = GetOperand(_engine, parameter.Name);
                stringBuilder.append(formulaItem);
                index = parameter.End + 1;
            }
            if (index < exp.length()) {
                stringBuilder.append(exp.substring(index));
            }
            return stringBuilder.toString();
        } catch (Exception ex) {
            throw new RuntimeException(ex);
        }
    }

    private String GetOperand(AlgorithmEngineEx engine, String name) {
        Operand operand = engine.GetParameter(name);
        if (operand.IsError()) {
            return "error(\"" + operand.ErrorMsg().replace("\\", "\\\\").replace("\"", "\\\"") + "\")";
        } else if (operand.Type() == OperandType.TEXT) {
            return operand.toString();
        }
        return operand.ToText(null).TextValue();
    }

    // #endregion

    // #region GetValue

    /**
     * 获取值
     */
    public Operand GetValue(String key) {
        return _engine.GetParameter(key);
    }

    // #endregion

    // #region BlankLine

    /**
     * 添加空行
     */
    public void BlankLine() {
        if (_useCalculationLogicInfo) {
            CalculationLogicInfo info = new CalculationLogicInfo();
            info.LogicType = CalculationLogicType.BlankLine;
            _calculationLogicInfos.add(info);
        }
    }

    // #endregion

    // #region ToInfoString

    /**
     * 转换为信息字符串
     */
    public String ToInfoString() {
        if (_useCalculationLogicInfo) {
            StringBuilder sb = new StringBuilder();
            if (!_initValueInfos.isEmpty()) {
                sb.append("[初始] ");
                for (CalculationLogicInfo item : _initValueInfos) {
                    sb.append(item.ToInfoString());
                }
                sb.append("\n");
            }
            for (CalculationLogicInfo item : _calculationLogicInfos) {
                sb.append(item.ToInfoString());
                sb.append("\n");
            }
            return sb.toString();
        }
        return "";
    }

    // #endregion
}
