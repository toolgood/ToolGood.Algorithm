package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.Collection;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import toolgood.algorithm.operands.MyDate;

/**
 * AlgorithmEngine 扩展�?
 * 增加自定义参数缓存功�?
 */
public class AlgorithmEngineEx extends AlgorithmEngine {

    private final ConcurrentHashMap<String, Operand> _tempdict;

    /**
     * 是否忽略大小�?
     */
    public final boolean IgnoreCase;

    /**
     * 保存到临时文�?
     */
    public boolean UseTempDict = false;

    // -------------------------------------------------------------------------
    // 构造函�?
    // -------------------------------------------------------------------------

    /**
     * 默认不带缓存（区分大小写�?
     */
    public AlgorithmEngineEx() {
        IgnoreCase = false;
        _tempdict = new ConcurrentHashMap<>();
    }

    /**
     * 带缓存关键字大小写参�?
     *
     * @param ignoreCase 是否忽略大小�?
     */
    public AlgorithmEngineEx(boolean ignoreCase) {
        IgnoreCase = ignoreCase;
        // Java �?ConcurrentHashMap 本身不支持自定义 key 比较器，
        // 忽略大小写时需�?put/get 时统一转小写�?
        _tempdict = new ConcurrentHashMap<>();
    }

    // -------------------------------------------------------------------------
    // 内部 key 规范�?
    // -------------------------------------------------------------------------

    private String normalizeKey(String key) {
        return IgnoreCase ? key.toLowerCase() : key;
    }

    // -------------------------------------------------------------------------
    // getParameter 重写
    // -------------------------------------------------------------------------

    /**
     * AlgorithmEngineEx 请重�?getParameterEx
     */
    @Override
    public Operand getParameter(String parameter) {
        String key = normalizeKey(parameter);
        Operand operand = _tempdict.get(key);
        if (operand != null) {
            return operand;
        }
        Operand result = getParameterEx(parameter);
        if (UseTempDict) {
            _tempdict.putIfAbsent(key, result);
        }
        return result;
    }

    /**
     * 获取参数扩展方法，子类可重写
     */
    public Operand getParameterEx(String parameter) {
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    // -------------------------------------------------------------------------
    // Parameter 管理
    // -------------------------------------------------------------------------

    /**
     * 清理所有参�?
     */
    public void ClearParameters() {
        _tempdict.clear();
    }

    /**
     * 添加自定义参�?
     */
    public void AddParameter(String key, Operand obj) {
        _tempdict.put(normalizeKey(key), obj);
    }

    /**
     * 添加自定义参数（boolean�?
     */
    public void AddParameter(String key, boolean obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    // ------ number ------

    /**
     * 添加自定义参数（short�?
     */
    public void AddParameter(String key, short obj) {
        _tempdict.put(normalizeKey(key), Operand.Create((int) obj));
    }

    /**
     * 添加自定义参数（int�?
     */
    public void AddParameter(String key, int obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    /**
     * 添加自定义参数（long�?
     */
    public void AddParameter(String key, long obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    /**
     * 添加自定义参数（float�?
     */
    public void AddParameter(String key, float obj) {
        _tempdict.put(normalizeKey(key), Operand.Create((double) obj));
    }

    /**
     * 添加自定义参数（double�?
     */
    public void AddParameter(String key, double obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    /**
     * 添加自定义参数（BigDecimal，对�?C# decimal�?
     */
    public void AddParameter(String key, BigDecimal obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    // ------ string ------

    /**
     * 添加自定义参数（String�?
     */
    public void AddParameter(String key, String obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    // ------ MyDate / date ------

    /**
     * 添加自定义参数（MyDate�?
     */
    public void AddParameter(String key, MyDate obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    // ------ array ------

    /**
     * 添加自定义参数（List&lt;Operand&gt;�?
     */
    public void AddParameter(String key, List<Operand> obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    /**
     * 添加自定义参数（Collection&lt;String&gt;�?
     */
    public void AddParameter(String key, Collection<String> obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj.toArray(new String[0])));
    }

    /**
     * 添加自定义参数（Collection&lt;Double&gt;�?
     */
    public void AddParameterDoubles(String key, Collection<Double> obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj.toArray(new Double[0])));
    }

    /**
     * 添加自定义参数（Collection&lt;Integer&gt;�?
     */
    public void AddParameterIntegers(String key, Collection<Integer> obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj.toArray(new Integer[0])));
    }

    /**
     * 添加自定义参数（Collection&lt;Boolean&gt;�?
     */
    public void AddParameterBooleans(String key, Collection<Boolean> obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj.toArray(new Boolean[0])));
    }

    // -------------------------------------------------------------------------
    // AddParameterFromJson
    // -------------------------------------------------------------------------

    /**
     * �?JSON 字符串批量添加参数�?
     * JSON 必须�?{...} 对象格式�?
     *
     * @param json JSON 字符�?
     * @throws Exception 若不是合�?JSON 对象
     */
    public void AddParameterFromJson(String json) throws Exception {
        if (json == null) {
            throw new Exception("Parameter is not json string.");
        }
        String trimmed = json.trim();
        if (trimmed.startsWith("{") && trimmed.endsWith("}")) {
            // 使用项目内置�?LitJson 解析
            toolgood.algorithm.litJson.JsonData jo = toolgood.algorithm.litJson.JsonMapper.ToObject(trimmed);
            if (jo.IsObject()) {
                for (java.util.Map.Entry<String, toolgood.algorithm.litJson.JsonData> item : jo.inst_object.entrySet()) {
                    toolgood.algorithm.litJson.JsonData v = item.getValue();
                    Operand operand;
                    if (v.IsString()) {
                        operand = Operand.Create(v.StringValue());
                    } else if (v.IsBoolean()) {
                        operand = Operand.Create(v.BooleanValue());
                    } else if (v.IsDouble()) {
                        operand = Operand.Create(v.NumberValue());
                    } else if (v.IsNull()) {
                        operand = Operand.NULL_OPERAND;
                    } else {
                        operand = Operand.Create(v);
                    }
                    _tempdict.put(normalizeKey(item.getKey()), operand);
                }
                return;
            }
        }
        throw new Exception("Parameter is not json string.");
    }
}
