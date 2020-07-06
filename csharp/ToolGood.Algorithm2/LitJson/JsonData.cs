using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;


namespace ToolGood.Algorithm.LitJson
{
    class JsonData : IJsonWrapper, IEnumerable
    {
        #region Fields
        private IList<JsonData> inst_array;
        private bool inst_boolean;
        private double inst_double;
        private int inst_int;
        private long inst_long;
        private IDictionary<string, JsonData> inst_object;
        private string inst_string;
        private JsonType type;
        private IList<KeyValuePair<string, JsonData>> object_list;
        #endregion


        #region Properties
        public int Count { get { return EnsureCollection().Count; } }
        public bool IsArray { get { return type == JsonType.Array; } }
        public bool IsBoolean { get { return type == JsonType.Boolean; } }
        public bool IsDouble { get { return type == JsonType.Double; } }
        public bool IsInt { get { return type == JsonType.Int; } }
        public bool IsLong { get { return type == JsonType.Long; } }
        public bool IsObject { get { return type == JsonType.Object; } }
        public bool IsString { get { return type == JsonType.String; } }
        #endregion



        #region Public Indexers
        public JsonData this[string prop_name] {
            get {
                EnsureDictionary();
                JsonData data;
                if (inst_object.TryGetValue(prop_name,out data))
                {
                    return data;
                }
                return null;
            }
            set {
                EnsureDictionary();

                KeyValuePair<string, JsonData> entry = new KeyValuePair<string, JsonData>(prop_name, value);

                if (inst_object.ContainsKey(prop_name)) {
                    for (int i = 0; i < object_list.Count; i++) {
                        if (object_list[i].Key == prop_name) {
                            object_list[i] = entry;
                            break;
                        }
                    }
                } else
                    object_list.Add(entry);

                inst_object[prop_name] = value;
            }
        }

        public JsonData this[int index] {
            get {
                EnsureCollection();

                if (type == JsonType.Array)
                    return inst_array[index];

                return object_list[index].Value;
            }

            set {
                EnsureCollection();

                if (type == JsonType.Array)
                    inst_array[index] = value;
                else {
                    KeyValuePair<string, JsonData> entry = object_list[index];
                    KeyValuePair<string, JsonData> new_entry = new KeyValuePair<string, JsonData>(entry.Key, value);

                    object_list[index] = new_entry;
                    inst_object[entry.Key] = value;
                }

                //json = null;
            }
        }
        #endregion


        #region Constructors
        public JsonData()
        {
        }

        public JsonData(object obj)
        {
            if (obj is Boolean) {
                type = JsonType.Boolean;
                inst_boolean = (bool)obj;
                return;
            }

            if (obj is Double) {
                type = JsonType.Double;
                inst_double = (double)obj;
                return;
            }

            if (obj is Int32) {
                type = JsonType.Int;
                inst_int = (int)obj;
                return;
            }

            if (obj is Int64) {
                type = JsonType.Long;
                inst_long = (long)obj;
                return;
            }

            if (obj is String) {
                type = JsonType.String;
                inst_string = (string)obj;
                return;
            }

            throw new ArgumentException(
                "Unable to wrap the given object with JsonData");
        }

        #endregion



        #region IJsonWrapper Methods

        void IJsonWrapper.SetBoolean(bool val)
        {
            type = JsonType.Boolean;
            inst_boolean = val;
        }

        void IJsonWrapper.SetDouble(double val)
        {
            type = JsonType.Double;
            inst_double = val;
        }

        void IJsonWrapper.SetInt(int val)
        {
            type = JsonType.Int;
            inst_int = val;
        }

        void IJsonWrapper.SetLong(long val)
        {
            type = JsonType.Long;
            inst_long = val;
        }

        void IJsonWrapper.SetString(string val)
        {
            type = JsonType.String;
            inst_string = val;
        }

        void IJsonWrapper.Add(IJsonWrapper val)
        {
            JsonData data = ToJsonData(val);
            EnsureList().Add(data);
        }

        void IJsonWrapper.Set(string key, IJsonWrapper val)
        {
            JsonData data = ToJsonData(val);
            EnsureDictionary().Add(key, data);
            KeyValuePair<string, JsonData> entry = new KeyValuePair<string, JsonData>((string)key, data);
            object_list.Add(entry);
        }

        #endregion



        #region Private Methods
        private ICollection EnsureCollection()
        {
            if (type == JsonType.Array) return (ICollection)inst_array;
            if (type == JsonType.Object) return (ICollection)inst_object;
            throw new InvalidOperationException("The JsonData instance has to be initialized first");
        }

        private IDictionary EnsureDictionary()
        {
            if (type == JsonType.Object) return (IDictionary)inst_object;
            if (type != JsonType.None) throw new InvalidOperationException("Instance of JsonData is not a dictionary");
            type = JsonType.Object;
            inst_object = new Dictionary<string, JsonData>();
            object_list = new List<KeyValuePair<string, JsonData>>();
            return (IDictionary)inst_object;
        }

        private IList EnsureList()
        {
            if (type == JsonType.Array) return (IList)inst_array;
            if (type != JsonType.None) throw new InvalidOperationException("Instance of JsonData is not a list");
            type = JsonType.Array;
            inst_array = new List<JsonData>();
            return (IList)inst_array;
        }

        private JsonData ToJsonData(object obj)
        {
            if (obj == null) return null;
            if (obj is JsonData) return (JsonData)obj;
            return new JsonData(obj);
        }

        #endregion

        void IJsonWrapper.SetJsonType(JsonType type)
        {
            if (this.type == type)
                return;

            switch (type) {
                case JsonType.None:
                    break;

                case JsonType.Object:
                    inst_object = new Dictionary<string, JsonData>();
                    object_list = new List<KeyValuePair<string, JsonData>>();
                    break;

                case JsonType.Array:
                    inst_array = new List<JsonData>();
                    break;

                case JsonType.String:
                    inst_string = default(String);
                    break;

                case JsonType.Int:
                    inst_int = default(Int32);
                    break;

                case JsonType.Long:
                    inst_long = default(Int64);
                    break;

                case JsonType.Double:
                    inst_double = default(Double);
                    break;

                case JsonType.Boolean:
                    inst_boolean = default(Boolean);
                    break;
            }

            this.type = type;
        }

        public override string ToString()
        {
            switch (type) {
                case JsonType.Array:
                    return "JsonData array";

                case JsonType.Boolean:
                    return inst_boolean.ToString();

                case JsonType.Double:
                    return inst_double.ToString();

                case JsonType.Int:
                    return inst_int.ToString();

                case JsonType.Long:
                    return inst_long.ToString();

                case JsonType.Object:
                    return "JsonData object";

                case JsonType.String:
                    return inst_string;
            }

            return "Uninitialized JsonData";
        }

        public IEnumerator GetEnumerator()
        {
            return EnsureList().GetEnumerator();
        }
    }
}
