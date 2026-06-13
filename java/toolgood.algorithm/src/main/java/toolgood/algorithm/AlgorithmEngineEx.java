package toolgood.algorithm;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import org.joda.time.DateTime;

import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;

/**
 * AlgorithmEngine 扩展类
 * 增加自定义参数缓存功能
 */
public class AlgorithmEngineEx extends AlgorithmEngine {
    private final ConcurrentHashMap<String, Operand> _tempdict;

    /**
     * 是否忽略大小写
     */
    public boolean IgnoreCase;

    /**
     * 保存到临时文档
     */
    public boolean UseTempDict = false;

    // #region 构造函数

    /**
     * 默认不带缓存
     */
    public AlgorithmEngineEx() {
        IgnoreCase = false;
        _tempdict = new ConcurrentHashMap<>();
    }

    /**
     * 带缓存关键字大小写参数
     */
    public AlgorithmEngineEx(boolean ignoreCase) {
        IgnoreCase = ignoreCase;
        _tempdict = new ConcurrentHashMap<>();
    }

    // #endregion 构造函数

    /**
     * AlgorithmEngineEx 请重写 GetParameterEx
     */
    @Override
    public Operand GetParameter(String parameter) {
        if (IgnoreCase) {
            for (java.util.Map.Entry<String, Operand> entry : _tempdict.entrySet()) {
                if (entry.getKey().equalsIgnoreCase(parameter)) {
                    return entry.getValue();
                }
            }
        } else {
            Operand operand = _tempdict.get(parameter);
            if (operand != null) {
                return operand;
            }
        }
        Operand result = GetParameterEx(parameter);
        if (UseTempDict) {
            _tempdict.put(parameter, result);
        }
        return result;
    }

    /**
     * 获取参数扩展方法
     */
    public Operand GetParameterEx(String parameter) {
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    // #region Parameter

    /**
     * 清理参数
     */
    public void ClearParameters() {
        _tempdict.clear();
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, Operand obj) {
        _tempdict.put(key, obj);
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, boolean obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    // #region number

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, short obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, int obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, long obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, float obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, double obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, java.math.BigDecimal obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    // #endregion number

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, String obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    // #region MyDate

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, MyDate obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, DateTime obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    // #endregion MyDate

    // #region array

    /**
     * 添加自定义参数
     */
    public void AddParameter(String key, List<Operand> obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    // #endregion array

    /**
     * 添加自定义参数
     */
    public void AddParameterFromJson(String json) throws Exception {
        if (json.startsWith("{") && json.endsWith("}")) {
            JsonData jo = JsonMapper.ToObject(json);
            if (jo.IsObject()) {
                for (String item : jo.inst_object.keySet()) {
                    JsonData v = jo.inst_object.get(item);
                    if (v.IsString()) {
                        _tempdict.put(item, Operand.Create(v.StringValue()));
                    } else if (v.IsBoolean()) {
                        _tempdict.put(item, Operand.Create(v.BooleanValue()));
                    } else if (v.IsDouble()) {
                        _tempdict.put(item, Operand.Create(v.NumberValue()));
                    } else if (v.IsObject()) {
                        _tempdict.put(item, Operand.Create(v));
                    } else if (v.IsArray()) {
                        _tempdict.put(item, Operand.Create(v));
                    } else if (v.IsNull()) {
                        _tempdict.put(item, Operand.Null);
                    } else {
                        _tempdict.put(item, Operand.Create(v));
                    }
                }
                return;
            }
        }
        throw new IllegalArgumentException("Parameter is not json string.");
    }

    // #endregion Parameter
}
