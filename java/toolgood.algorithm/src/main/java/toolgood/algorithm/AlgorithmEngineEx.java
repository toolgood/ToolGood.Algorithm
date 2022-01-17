package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;

import org.joda.time.DateTime;

import toolgood.algorithm.internals.ConditionCacheInfo;
import toolgood.algorithm.internals.MathVisitor;
import toolgood.algorithm.internals.MyFunction;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathParser2.ProgContext;

/**
 * 算法引擎 扩展版
 * 支持 一层算法 内套 一层算法
 * 支持多条件过滤
 */
public class AlgorithmEngineEx {
    public boolean UseExcelIndex = true;
    public String LastError;
    private final Map<String, Operand> _dict = new HashMap<String, Operand>();
    public Function<MyFunction, Operand> DiyFunction;
    private ConditionCache MultiConditionCache;
    /**
     * 跳过条件错误
     */
    public Boolean JumpConditionError = false;
    /**
     * 跳过公式错误
     */
    public Boolean JumpFormulaError = false;

    public AlgorithmEngineEx(ConditionCache multiConditionCache) {
        MultiConditionCache = multiConditionCache;
    }

    protected Operand GetParameter(final String parameter) {
        if (_dict.containsKey(parameter)) {
            return _dict.get(parameter);
        }
        ArrayList<ConditionCacheInfo> conditionCaches = MultiConditionCache.GetConditionCaches(parameter);
        for (ConditionCacheInfo conditionCache : conditionCaches) {
            if (conditionCache.GetFormulaProg() == null)
                continue;
            if (conditionCache.GetConditionProg() != null) {
                Operand b = Evaluate(conditionCache.GetConditionProg());
                if (b.IsError()) {
                    LastError = "Parameter [" + parameter + "]," + conditionCache.Remark
                            + " condition `" + conditionCache.ConditionString + "` is error.\r\n" + b.ErrorMsg();
                    if (JumpConditionError)
                        continue;
                    return Operand.Error(LastError);
                }
                if (b.BooleanValue() == false)
                    continue;
            }
            Operand operand = Evaluate(conditionCache.GetFormulaProg());
            if (operand.IsError()) {
                LastError = "Parameter [" + parameter + "]," + conditionCache.Remark
                        + " formula `" + conditionCache.FormulaString + "` is error.\r\n" + operand.ErrorMsg();
                if (JumpFormulaError)
                    continue;
                return Operand.Error(LastError);
            }
            _dict.put(parameter, operand);
            return operand;
        }
        if (LastError != null)
            return Operand.Error(LastError);
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    protected Operand ExecuteDiyFunction(final String funcName, final List<Operand> operands) {
        if (DiyFunction != null) {
            MyFunction f = new MyFunction();
            f.Name = funcName;
            f.OperandList = operands;
            return DiyFunction.apply(f);
        }
        return Operand.Error("DiyFunction [" + funcName + "] is missing.");
    }

    public void ClearDiyFunctions() {
        DiyFunction = null;
    }

    public void ClearParameters() {
        _dict.clear();
    }

    /**
     * 执行
     * 
     * @param categoryName
     * @return
     */
    public Operand Evaluate(String categoryName) {
        LastError = null;
        Operand operand;
        ArrayList<ConditionCacheInfo> conditionCaches = MultiConditionCache.GetConditionCaches(categoryName);
        for (ConditionCacheInfo conditionCache : conditionCaches) {
            if (conditionCache.FormulaString == null || conditionCache.FormulaString.length() == 0)
                continue;
            if (conditionCache.ConditionString != null && conditionCache.ConditionString.length() > 0) {
                if (conditionCache.GetConditionProg() == null) {
                    return Operand.Error("CategoryName [" + categoryName + "]," + conditionCache.Remark
                            + "  parse condition  `" + conditionCache.ConditionString + "` is error.\r\n"
                            + conditionCache.LastError);
                }
                Operand b = Evaluate(conditionCache.GetConditionProg());
                if (b.IsError()) {
                    LastError = "Parameter [" + categoryName + "]," + conditionCache.Remark
                            + " condition `" + conditionCache.ConditionString + "` is error.\r\n" + b.ErrorMsg();
                    if (JumpConditionError)
                        continue;
                    return Operand.Error(LastError);
                }
                if (b.BooleanValue() == false)
                    continue;
            }
            if (conditionCache.GetFormulaProg() == null) {
                return Operand.Error("CategoryName [" + categoryName + "]," + conditionCache.Remark
                        + "  parse formula  `" + conditionCache.FormulaString + "` is error.\r\n"
                        + conditionCache.LastError);
            }
            operand = Evaluate(conditionCache.GetFormulaProg());
            if (operand.IsError()) {
                LastError = "Parameter [" + categoryName + "]," + conditionCache.Remark
                        + " formula `" + conditionCache.FormulaString + "` is error.\r\n" + operand.ErrorMsg();
                if (JumpFormulaError)
                    continue;
                operand = Operand.Error(LastError);
            }
            return operand;
        }
        if (LastError != null)
            return Operand.Error(LastError);
        return Operand.Error("CategoryName [" + categoryName + "] is missing.");
    }

    /**
     * 查找备注
     * 
     * @param categoryName
     * @return
     * @throws Exception
     */
    public String SearchRemark(String categoryName) throws Exception {
        LastError = null;
        ArrayList<ConditionCacheInfo> conditionCaches = MultiConditionCache.GetConditionCaches(categoryName);
        for (ConditionCacheInfo conditionCache : conditionCaches) {
            if (conditionCache.ConditionString != null && conditionCache.ConditionString.length() > 0) {
                if (conditionCache.GetConditionProg() == null) {
                    throw new Exception("CategoryName [" + categoryName + "]," + conditionCache.Remark
                            + "  parse condition  `" + conditionCache.ConditionString + "` is error.\r\n"
                            + conditionCache.LastError);
                }
                Operand b = Evaluate(conditionCache.GetConditionProg());
                if (b.IsError()) {
                    LastError = "Parameter [" + categoryName + "]," + conditionCache.Remark
                            + " condition `" + conditionCache.ConditionString + "` is error.\r\n" + b.ErrorMsg();
                    if (JumpConditionError)
                        continue;
                    throw new Exception(LastError);
                }
                if (b.BooleanValue() == false)
                    continue;
            }
            return conditionCache.Remark;
        }
        if (LastError != null)
            new Exception(LastError);
        throw new Exception("CategoryName [" + categoryName + "] is missing.");
    }

    /**
     * 查找备注,如果异常，返回默认值
     */
    public String TrySearchRemark(String categoryName, String def) {
        try {
            return SearchRemark(categoryName);
        } catch (Exception ex) {
            LastError = ex.getMessage();
        }
        return def;
    }

    public void AddParameter(final String key, final Operand obj) {
        _dict.put(key, obj);
    }

    public void AddParameter(final String key, final boolean obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final short obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final int obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final long obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final float obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final double obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final BigDecimal obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final String obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final MyDate obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final List<Operand> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameterFromJson(final String json) throws Exception {
        if (json.startsWith("{") && json.endsWith("}")) {
            final JsonData jo = (JsonData) JsonMapper.ToObject(json);
            if (jo.IsObject()) {
                for (String item : jo.inst_object.keySet()) {
                    final JsonData v = jo.inst_object.get(item);
                    if (v.IsString())
                        _dict.put(item, Operand.Create(v.StringValue()));
                    else if (v.IsBoolean())
                        _dict.put(item, Operand.Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        _dict.put(item, Operand.Create(v.NumberValue()));
                    else if (v.IsObject())
                        _dict.put(item, Operand.Create(v));
                    else if (v.IsArray())
                        _dict.put(item, Operand.Create(v));
                    else if (v.IsNull())
                        _dict.put(item, Operand.CreateNull());
                }
                return;
            }
        }
        throw new Exception("Parameter is not json String.");
    }

    public int TryEvaluate(final String categoryName, final int defvalue) {
        try {
            Operand obj = Evaluate(categoryName);
            obj = obj.ToNumber("It can't be converted to number!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.IntValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public double TryEvaluate(final String categoryName, final double defvalue) {
        try {
            Operand obj = Evaluate(categoryName);
            obj = obj.ToNumber("It can't be converted to number!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.NumberValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public String TryEvaluate(final String categoryName, final String defvalue) {
        try {
            Operand obj = Evaluate(categoryName);
            if (obj.IsNull()) {
                return null;
            }
            obj = obj.ToText("It can't be converted to String!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.TextValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public boolean TryEvaluate(final String categoryName, final boolean defvalue) {
        try {
            Operand obj = Evaluate(categoryName);
            obj = obj.ToBoolean("It can't be converted to bool!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.BooleanValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public DateTime TryEvaluate(final String categoryName, final DateTime defvalue) {
        try {
            Operand obj = Evaluate(categoryName);
            obj = obj.ToDate("It can't be converted to bool!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.DateValue().ToDateTime();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public MyDate TryEvaluate(final String categoryName, final MyDate defvalue) {
        try {
            Operand obj = Evaluate(categoryName);
            obj = obj.ToDate("It can't be converted to bool!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.DateValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    private Operand Evaluate(ProgContext context) {
        try {
            final MathVisitor visitor = new MathVisitor();
            visitor.GetParameter = f -> {
                try {
                    return GetParameter(f);
                } catch (Exception e) {
                }
                return null;
            };
            visitor.excelIndex = UseExcelIndex ? 1 : 0;
            visitor.DiyFunction = f -> {
                return ExecuteDiyFunction(f.Name, f.OperandList);
            };
            return visitor.visit(context);
        } catch (Exception ex) {
            LastError = ex.getMessage();
            return Operand.Error(ex.getMessage());
        }
    }

}
