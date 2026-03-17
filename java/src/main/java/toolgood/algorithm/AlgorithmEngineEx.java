package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.operands.MyDate;

public class AlgorithmEngineEx extends AlgorithmEngine {
    private final ConcurrentHashMap<String, Operand> _tempdict;

    public final boolean IgnoreCase;

    public boolean UseTempDict = false;

    public AlgorithmEngineEx() {
        IgnoreCase = false;
        _tempdict = new ConcurrentHashMap<>();
    }

    public AlgorithmEngineEx(boolean ignoreCase) {
        IgnoreCase = ignoreCase;
        _tempdict = new ConcurrentHashMap<>();
    }

    private String normalizeKey(String key) {
        return IgnoreCase ? key.toLowerCase() : key;
    }

    @Override
    public Operand GetParameter(String parameter) {
        String key = normalizeKey(parameter);
        Operand operand = _tempdict.get(key);
        if (operand != null) {
            return operand;
        }
        Operand result = GetParameterEx(parameter);
        if (UseTempDict) {
            _tempdict.putIfAbsent(key, result);
        }
        return result;
    }

    public Operand GetParameterEx(String parameter) {
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    public void ClearParameters() {
        _tempdict.clear();
    }

    public void AddParameter(String key, Operand obj) {
        _tempdict.put(normalizeKey(key), obj);
    }

    public void AddParameter(String key, boolean obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    public void AddParameter(String key, short obj) {
        _tempdict.put(normalizeKey(key), Operand.Create((int) obj));
    }

    public void AddParameter(String key, int obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    public void AddParameter(String key, long obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    public void AddParameter(String key, float obj) {
        _tempdict.put(normalizeKey(key), Operand.Create((double) obj));
    }

    public void AddParameter(String key, double obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    public void AddParameter(String key, BigDecimal obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    public void AddParameter(String key, String obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    public void AddParameter(String key, MyDate obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    public void AddParameter(String key, List<Operand> obj) {
        _tempdict.put(normalizeKey(key), Operand.Create(obj));
    }

    public void AddParameter(String key, String[] obj) {
        _tempdict.put(normalizeKey(key), Operand.CreateStringCollection(obj));
    }

    public void AddParameterDoubles(String key, Double[] obj) {
        _tempdict.put(normalizeKey(key), Operand.CreateDoubleCollection(obj));
    }

    public void AddParameterIntegers(String key, Integer[] obj) {
        _tempdict.put(normalizeKey(key), Operand.CreateIntCollection(obj));
    }

    public void AddParameterBooleans(String key, Boolean[] obj) {
        _tempdict.put(normalizeKey(key), Operand.CreateBooleanCollection(obj));
    }

    public void AddParameterFromJson(String json) throws Exception {
        if (json == null) {
            throw new Exception("Parameter is not json string.");
        }
        String trimmed = json.trim();
        if (trimmed.startsWith("{") && trimmed.endsWith("}")) {
            JsonData jo = JsonMapper.ToObject(trimmed);
            if (jo.IsObject()) {
                for (java.util.Map.Entry<String, JsonData> item : jo.inst_object.entrySet()) {
                    JsonData v = item.getValue();
                    Operand operand;
                    if (v.IsString()) {
                        operand = Operand.Create(v.StringValue());
                    } else if (v.IsBoolean()) {
                        operand = Operand.Create(v.BooleanValue());
                    } else if (v.IsDouble()) {
                        operand = Operand.Create(v.NumberValue());
                    } else if (v.IsObject()) {
                        operand = Operand.Create(v);
                    } else if (v.IsArray()) {
                        operand = Operand.Create(v);
                    } else if (v.IsNull()) {
                        operand = Operand.Null;
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
