package toolgood.algorithm;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.enums.AreaUnitType;
import toolgood.algorithm.enums.DistanceUnitType;
import toolgood.algorithm.enums.MassUnitType;
import toolgood.algorithm.enums.VolumeUnitType;
import toolgood.algorithm.internals.*;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ProgContext;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

/**
 * 算法引擎 扩展版
 * 支持 一层算法 内套 一层算法
 * 支持多条件过滤
 */
public class AlgorithmEngineEx {
    /**
     * 使用EXCEL索引
     */
    public boolean UseExcelIndex = true;
    /**
     * 最后一个错误
     */
    public String LastError;
    /**
     * 保存到临时文档
     */
    public boolean UseTempDict = true;
    /**
     * 是否忽略大小写
     */
    public final boolean IgnoreCase;
    /**
     * 跳过条件错误
     */
    public Boolean JumpConditionError = true;
    /**
     * 跳过公式错误
     */
    public Boolean JumpFormulaError = false;
    /**
     * 使用本地时区
     */
    public Boolean UseLocalTime = false;
    private final Map<String, Operand> _tempdict;
    private ConditionCache MultiConditionCache;
    public DistanceUnitType DistanceUnit = DistanceUnitType.M;
    public AreaUnitType AreaUnit = AreaUnitType.M2;
    public VolumeUnitType VolumeUnit = VolumeUnitType.M3;
    public MassUnitType MassUnit = MassUnitType.KG;

    public AlgorithmEngineEx(ConditionCache multiConditionCache) {
        MultiConditionCache = multiConditionCache;
        IgnoreCase = false;
        _tempdict = new TreeMap<String, Operand>();

    }

    public AlgorithmEngineEx(ConditionCache multiConditionCache, boolean ignoreCase) {
        MultiConditionCache = multiConditionCache;
        IgnoreCase = ignoreCase;
        if (ignoreCase) {
            _tempdict = new TreeMap<String, Operand>(String.CASE_INSENSITIVE_ORDER);
        } else {
            _tempdict = new TreeMap<String, Operand>();
        }
    }

    private Operand GetDiyParameterInside(String parameter) {
        if (_tempdict.containsKey(parameter)) {
            return _tempdict.get(parameter);
        }
        Operand result = GetParameter(parameter);
        if (UseTempDict) {
            _tempdict.put(parameter, result);
        }
        return result;
    }

    protected Operand GetParameter(final String parameter) {
        ArrayList<ConditionCacheInfo> conditionCaches = MultiConditionCache.GetConditionCaches(parameter);
        for (ConditionCacheInfo conditionCache : conditionCaches) {
            if (conditionCache.GetFormulaProg() == null)
                continue;
            if (conditionCache.GetConditionProg() != null) {
                Operand b = EvaluateCategory(conditionCache.GetConditionProg());
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
            Operand operand = EvaluateCategory(conditionCache.GetFormulaProg());
            if (operand.IsError()) {
                LastError = "Parameter [" + parameter + "]," + conditionCache.Remark
                        + " formula `" + conditionCache.FormulaString + "` is error.\r\n" + operand.ErrorMsg();
                if (JumpFormulaError)
                    continue;
                return Operand.Error(LastError);
            }
            return operand;
        }
        if (LastError != null)
            return Operand.Error(LastError);
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    protected Operand ExecuteDiyFunction(final String funcName, final List<Operand> operands) {
        return Operand.Error("DiyFunction [" + funcName + "] is missing.");
    }

    public void ClearParameters() {
        _tempdict.clear();
    }

    /**
     * 执行
     *
     * @param categoryName
     * @return
     */
    public Operand EvaluateCategory(String categoryName) {
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
                Operand b = EvaluateCategory(conditionCache.GetConditionProg());
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
            operand = EvaluateCategory(conditionCache.GetFormulaProg());
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
                Operand b = EvaluateCategory(conditionCache.GetConditionProg());
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

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final Operand obj) {
        _tempdict.put(key, obj);
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final boolean obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final short obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final int obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final long obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final float obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final double obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final BigDecimal obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final String obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final MyDate obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final List<Operand> obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameterFromJson(final String json) throws Exception {
        if (json.startsWith("{") && json.endsWith("}")) {
            final JsonData jo = (JsonData) JsonMapper.ToObject(json);
            if (jo.IsObject()) {
                for (String item : jo.inst_object.keySet()) {
                    final JsonData v = jo.inst_object.get(item);
                    if (v.IsString())
                        _tempdict.put(item, Operand.Create(v.StringValue()));
                    else if (v.IsBoolean())
                        _tempdict.put(item, Operand.Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        _tempdict.put(item, Operand.Create(v.NumberValue()));
                    else if (v.IsObject())
                        _tempdict.put(item, Operand.Create(v));
                    else if (v.IsArray())
                        _tempdict.put(item, Operand.Create(v));
                    else if (v.IsNull())
                        _tempdict.put(item, Operand.CreateNull());
                }
                return;
            }
        }
        throw new Exception("Parameter is not json String.");
    }

    public BigDecimal TryEvaluateCategory(final String categoryName, final BigDecimal defvalue) {
        try {
            Operand obj = EvaluateCategory(categoryName);
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

    public int TryEvaluateCategory(final String categoryName, final int defvalue) {
        try {
            Operand obj = EvaluateCategory(categoryName);
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

    public double TryEvaluateCategory(final String categoryName, final double defvalue) {
        try {
            Operand obj = EvaluateCategory(categoryName);
            obj = obj.ToNumber("It can't be converted to number!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.DoubleValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public long TryEvaluateCategory(final String categoryName, final long defvalue) {
        try {
            Operand obj = EvaluateCategory(categoryName);
            obj = obj.ToNumber("It can't be converted to number!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.LongValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public String TryEvaluateCategory(final String categoryName, final String defvalue) {
        try {
            Operand obj = EvaluateCategory(categoryName);
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

    public boolean TryEvaluateCategory(final String categoryName, final boolean defvalue) {
        try {
            Operand obj = EvaluateCategory(categoryName);
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

    public DateTime TryEvaluateCategory(final String categoryName, final DateTime defvalue) {
        try {
            Operand obj = EvaluateCategory(categoryName);
            obj = obj.ToDate("It can't be converted to bool!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            if (UseLocalTime) {
                return obj.DateValue().ToDateTime(DateTimeZone.getDefault());
            }
            return obj.DateValue().ToDateTime(DateTimeZone.UTC);
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public MyDate TryEvaluateCategory(final String categoryName, final MyDate defvalue) {
        try {
            Operand obj = EvaluateCategory(categoryName);
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

    public BigDecimal TryEvaluate(final String exp, final BigDecimal defvalue) {
        try {
            Operand obj = Evaluate(exp);
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

    public int TryEvaluate(final String exp, final int defvalue) {
        try {
            Operand obj = Evaluate(exp);
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

    public double TryEvaluate(final String exp, final double defvalue) {
        try {
            Operand obj = Evaluate(exp);
            obj = obj.ToNumber("It can't be converted to number!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.DoubleValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public long TryEvaluate(final String exp, final long defvalue) {
        try {
            Operand obj = Evaluate(exp);
            obj = obj.ToNumber("It can't be converted to number!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            return obj.LongValue();
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public String TryEvaluate(final String exp, final String defvalue) {
        try {
            Operand obj = Evaluate(exp);
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

    public boolean TryEvaluate(final String exp, final boolean defvalue) {
        try {
            Operand obj = Evaluate(exp);
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

    public DateTime TryEvaluate(final String exp, final DateTime defvalue) {
        try {
            Operand obj = Evaluate(exp);
            obj = obj.ToDate("It can't be converted to bool!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return defvalue;
            }
            if (UseLocalTime) {
                return obj.DateValue().ToDateTime(DateTimeZone.getDefault());
            }
            return obj.DateValue().ToDateTime(DateTimeZone.UTC);
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public MyDate TryEvaluate(final String exp, final MyDate defvalue) {
        try {
            Operand obj = Evaluate(exp);
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

    private Operand EvaluateCategory(ProgContext context) {
        try {
            final MathVisitor visitor = new MathVisitor();
            visitor.GetParameter = f -> {
                try {
                    return GetDiyParameterInside(f);
                } catch (Exception e) {
                }
                return null;
            };
            visitor.excelIndex = UseExcelIndex ? 1 : 0;
            visitor.DiyFunction = f -> {
                return ExecuteDiyFunction(f.Name, f.OperandList);
            };
            visitor.useLocalTime = UseLocalTime;
            visitor.DistanceUnit= DistanceUnit;
            visitor.AreaUnit=AreaUnit;
            visitor.VolumeUnit=VolumeUnit;
            visitor.MassUnit=MassUnit;
            return visitor.visit(context);
        } catch (Exception ex) {
            LastError = ex.getMessage();
            return Operand.Error(ex.getMessage());
        }
    }

    /**
     * 获取简化公式
     *
     * @param formula 公式
     */
    public String GetSimplifiedFormula(final String formula) {
        try {
            ProgContext _context = Parse(formula);
            final MathSimplifiedFormulaVisitor visitor = new MathSimplifiedFormulaVisitor();
            visitor.GetParameter = f -> {
                try {
                    return GetDiyParameterInside(f);
                } catch (Exception e) {
                }
                return null;
            };
            visitor.excelIndex = UseExcelIndex ? 1 : 0;
            visitor.DiyFunction = f -> {
                return ExecuteDiyFunction(f.Name, f.OperandList);
            };
            visitor.useLocalTime = UseLocalTime;
            visitor.DistanceUnit= DistanceUnit;
            visitor.AreaUnit=AreaUnit;
            visitor.VolumeUnit=VolumeUnit;
            visitor.MassUnit=MassUnit;
            Operand obj = visitor.visit(_context);
            obj = obj.ToText("It can't be converted to String!");
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return null;
            }
            return obj.TextValue();

        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return null;
    }

    /**
     * 计算公式
     *
     * @param formula   公式
     * @param splitChar 分隔符
     * @return
     */
    public String EvaluateFormula(String formula, Character splitChar) {
        if (formula == null || formula.equals(""))
            return "";
        List<Character> splitChars = new ArrayList<>();
        splitChars.add(splitChar);
        return EvaluateFormula(formula, splitChars);
    }

    /**
     * 计算公式
     *
     * @param formula    公式
     * @param splitChars 分隔符
     * @return
     */
    public String EvaluateFormula(String formula, List<Character> splitChars) {
        if (formula == null || formula.equals(""))
            return "";
        List<String> sp = CharUtil.SplitFormula(formula, splitChars);

        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < sp.size(); i++) {
            String s = sp.get(i);
            if (s.length() == 1 && splitChars.contains(s.charAt(0))) {
                stringBuilder.append(s);
            } else {
                String d = "";
                try {
                    Operand operand = Evaluate(s);
                    d = operand.ToText("").TextValue();
                } catch (Exception ex) {
                }
                stringBuilder.append(d);
            }
        }
        return stringBuilder.toString();
    }

    /// <summary>
    /// 执行一次
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    public Operand Evaluate(String exp) {
        ProgContext context = Parse(exp);
        if (context == null) {
            return Operand.Create(LastError);
        }
        return EvaluateCategory(context);
    }

    private ProgContext Parse(String exp) {
        if (exp == null || exp.equals("")) {
            LastError = "Parameter exp invalid !";
            return null;
        }
        try {
            final AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
            final mathLexer lexer = new mathLexer(stream);
            final CommonTokenStream tokens = new CommonTokenStream(lexer);
            final mathParser parser = new mathParser(tokens);
            final AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
            parser.removeErrorListeners();
            parser.addErrorListener(antlrErrorListener);
            final ProgContext context = parser.prog();

            if (antlrErrorListener.IsError) {
                LastError = antlrErrorListener.ErrorMsg;
                return null;
            }
            return context;
        } catch (Exception ex) {
            LastError = ex.getMessage();
        }
        return null;
    }

}
