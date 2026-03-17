package toolgood.algorithm.litJson;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

final class JsonData implements Iterable<JsonData> {
    private List<JsonData> inst_array;
    private boolean inst_boolean;
    private BigDecimal inst_double;
    Map<String, JsonData> inst_object;
    private String inst_string;
    private JsonType type;

    public int Count() {
        if (type == JsonType.Array) return inst_array.size();
        return inst_object.size();
    }

    public boolean IsArray() { return type == JsonType.Array; }
    public boolean IsBoolean() { return type == JsonType.Boolean; }
    public boolean IsDouble() { return type == JsonType.Double; }
    public boolean IsObject() { return type == JsonType.Object; }
    public boolean IsString() { return type == JsonType.String; }
    public boolean IsNull() { return type == JsonType.Null; }

    public JsonData get(String prop_name) {
        if (inst_object != null) {
            if (inst_object.containsKey(prop_name)) {
                return inst_object.get(prop_name);
            }
        }
        return null;
    }

    public JsonData get(int index) {
        if (type == JsonType.Array)
            return inst_array.get(index);
        return null;
    }

    public JsonData() {
    }

    void SetBoolean(boolean val) {
        type = JsonType.Boolean;
        inst_boolean = val;
    }

    void SetDouble(BigDecimal val) {
        type = JsonType.Double;
        inst_double = val;
    }

    void SetString(String val) {
        type = JsonType.String;
        inst_string = val;
    }

    void SetNull() {
        type = JsonType.Null;
    }

    void Add(JsonData val) {
        EnsureList().add(val);
    }

    void Set(String key, JsonData val) {
        EnsureDictionary().put(key, val);
    }

    private Map<String, JsonData> EnsureDictionary() {
        if (type == JsonType.Object) return inst_object;
        type = JsonType.Object;
        inst_object = new HashMap<>();
        return inst_object;
    }

    private List<JsonData> EnsureList() {
        if (type == JsonType.Array) return inst_array;
        type = JsonType.Array;
        inst_array = new ArrayList<>();
        return inst_array;
    }

    void SetJsonType(JsonType type) {
        if (this.type == type)
            return;

        switch (type) {
            case None:
                break;

            case Object:
                inst_object = new HashMap<>();
                break;

            case Array:
                inst_array = new ArrayList<>();
                break;

            case String:
                inst_string = null;
                break;

            case Double:
                inst_double = BigDecimal.ZERO;
                break;

            case Boolean:
                inst_boolean = false;
                break;
        }

        this.type = type;
    }

    public Iterator<JsonData> iterator() {
        return EnsureList().iterator();
    }

    public boolean BooleanValue() { return inst_boolean; }
    public BigDecimal NumberValue() { return inst_double; }
    public String StringValue() { return inst_string; }

    public List<JsonData> getArray() {
        return inst_array;
    }

    @Override
    public String toString() {
        StringBuilder stringBuilder = new StringBuilder();
        ToString(stringBuilder);
        return stringBuilder.toString();
    }

    private void ToString(StringBuilder stringBuilder) {
        if (IsNull()) {
            stringBuilder.append("null");
        } else if (IsBoolean()) {
            stringBuilder.append(inst_boolean ? "true" : "false");
        } else if (IsArray()) {
            stringBuilder.append("[");
            for (int i = 0; i < inst_array.size(); i++) {
                if (i > 0) {
                    stringBuilder.append(",");
                }
                inst_array.get(i).ToString(stringBuilder);
            }
            stringBuilder.append("]");
        } else if (IsObject()) {
            stringBuilder.append("{");
            boolean first = true;
            for (Map.Entry<String, JsonData> kv : inst_object.entrySet()) {
                if (!first) {
                    stringBuilder.append(",");
                }
                first = false;
                stringBuilder.append("\"");
                stringBuilder.append(kv.getKey().replace("\\", "\\\\").replace("\"", "\\\"")
                        .replace("\n", "\\n").replace("\r", "\\r")
                        .replace("\t", "\\t").replace("\0", "\\0")
                        .replace("\u000b", "\\v")
                        .replace("\u0007", "\\a")
                        .replace("\b", "\\b").replace("\f", "\\f"));
                stringBuilder.append("\":");
                kv.getValue().ToString(stringBuilder);
            }
            stringBuilder.append("}");
        } else if (IsString()) {
            stringBuilder.append("\"");
            stringBuilder.append(inst_string.replace("\\", "\\\\").replace("\"", "\\\"")
                    .replace("\n", "\\n").replace("\r", "\\r")
                    .replace("\t", "\\t").replace("\0", "\\0")
                    .replace("\u000b", "\\v")
                    .replace("\u0007", "\\a")
                    .replace("\b", "\\b").replace("\f", "\\f"));
            stringBuilder.append("\"");
        } else if (IsDouble()) {
            stringBuilder.append(inst_double.toPlainString());
        }
    }
}
