package toolgood.algorithm.litJson;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class JsonData implements IJsonWrapper {

    public ArrayList<JsonData> inst_array;
    private boolean inst_boolean;
    private double inst_double;
    public Map<String, JsonData> inst_object;
    private String inst_string;
    private JsonType type;
    // private IList<KeyValuePair<string, JsonData>> object_list;

    public int Count() {
        return EnsureCollection().size();
    }

    public boolean IsArray() {
        return type == JsonType.Array;
    }

    public boolean IsBoolean() {
        return type == JsonType.Boolean;
    }

    public boolean IsDouble() {
        return type == JsonType.Double;
    }

    public boolean IsObject() {
        return type == JsonType.Object;
    }

    public boolean IsString() {
        return type == JsonType.String;
    }

    public boolean IsNull() {
        return type == JsonType.Null;
    }

    public JsonData GetChild(final String prop_name) {
        EnsureDictionary();
        if (inst_object.containsKey(prop_name)) {
            return inst_object.get(prop_name);
        }
        return null;
    }

    public JsonData GetChild(final int index) {
        EnsureCollection();
        if (type == JsonType.Array)
            return inst_array.get(index);
        return null;
    }

    public JsonData() {
    }

    public void SetBoolean(final boolean val) {
        type = JsonType.Boolean;
        inst_boolean = val;
    }

    public void SetDouble(final double val) {
        type = JsonType.Double;
        inst_double = val;
    }

    public void SetString(final String val) {
        type = JsonType.String;
        inst_string = val;
    }

    public void SetNull() {
        type = JsonType.Null;
    }

    public void Add(final IJsonWrapper val) {
        EnsureList().add((JsonData) val);
    }

    public void Set(final String key, final IJsonWrapper val) {
        final JsonData data = (JsonData) val;
        EnsureDictionary().put(key, data);
        // KeyValuePair<string, JsonData> entry = new KeyValuePair<string,
        // JsonData>((string)key, data);
        // object_list.Add(entry);
    }

    private Collection EnsureCollection() {
        if (type == JsonType.Array)
            return (Collection) inst_array;
        return (Collection) inst_object;
    }

    private Map<String, JsonData> EnsureDictionary() {
        if (type == JsonType.Object)
            return inst_object;
        type = JsonType.Object;
        inst_object = new HashMap<String, JsonData>();
        // object_list = new List<KeyValuePair<string, JsonData>>();
        return inst_object;
    }

    private List<JsonData> EnsureList() {
        if (type == JsonType.Array)
            return inst_array;
        type = JsonType.Array;
        inst_array = new ArrayList<JsonData>();
        return inst_array;
    }

    public void SetJsonType(final JsonType type) {
        if (this.type == type)
            return;

        switch (type) {
            case None:
                break;

            case Object:
                inst_object = new HashMap<String, JsonData>();
                break;

            case Array:
                inst_array = new ArrayList<JsonData>();
                break;

            case String:
                inst_string = null;
                break;

            case Double:
                inst_double = 0;
                break;

            case Boolean:
                inst_boolean = false;
                break;
            default:
        }
        this.type = type;
    }

 

    public boolean BooleanValue() {
        return inst_boolean;
    }

    public double NumberValue() {
        return inst_double;
    }

    public String StringValue() {
        return inst_string;
    }

}