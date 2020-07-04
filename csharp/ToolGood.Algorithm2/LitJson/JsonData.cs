using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;


namespace ToolGood.Algorithm.LitJson
{
    class JsonData : IJsonWrapper, IList, IDictionary
    {
        #region Fields
        private IList<JsonData> inst_array;
        private bool inst_boolean;
        private double inst_double;
        private int inst_int;
        private long inst_long;
        private IDictionary<string, JsonData> inst_object;
        private string inst_string;
        //private string json;
        private JsonType type;

        // Used to implement the IOrderedDictionary interface
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


        #region ICollection Properties
        int ICollection.Count { get { return Count; } }
        bool ICollection.IsSynchronized { get { return EnsureCollection().IsSynchronized; } }

        object ICollection.SyncRoot { get { return EnsureCollection().SyncRoot; } }
        #endregion


        #region IDictionary Properties
        bool IDictionary.IsFixedSize { get { return EnsureDictionary().IsFixedSize; } }
        bool IDictionary.IsReadOnly { get { return EnsureDictionary().IsReadOnly; } }

        ICollection IDictionary.Keys {
            get {
                EnsureDictionary();
                IList<string> keys = new List<string>();

                foreach (KeyValuePair<string, JsonData> entry in
                         object_list) {
                    keys.Add(entry.Key);
                }

                return (ICollection)keys;
            }
        }

        ICollection IDictionary.Values {
            get {
                EnsureDictionary();
                IList<JsonData> values = new List<JsonData>();

                foreach (KeyValuePair<string, JsonData> entry in
                         object_list) {
                    values.Add(entry.Value);
                }

                return (ICollection)values;
            }
        }
        #endregion


        #region IList Properties
        bool IList.IsFixedSize { get { return EnsureList().IsFixedSize; } }
        bool IList.IsReadOnly { get { return EnsureList().IsReadOnly; } }
        #endregion


        #region IDictionary Indexer
        object IDictionary.this[object key] {
            get {
                return EnsureDictionary()[key];
            }

            set {
                if (!(key is String))
                    throw new ArgumentException(
                        "The key has to be a string");

                JsonData data = ToJsonData(value);

                this[(string)key] = data;
            }
        }
        #endregion

        #region IList Indexer
        object IList.this[int index] {
            get {
                return EnsureList()[index];
            }

            set {
                EnsureList();
                JsonData data = ToJsonData(value);

                this[index] = data;
            }
        }
        #endregion


        #region Public Indexers
        public JsonData this[string prop_name] {
            get {
                EnsureDictionary();
                return inst_object[prop_name];
            }

            set {
                EnsureDictionary();

                KeyValuePair<string, JsonData> entry =
                    new KeyValuePair<string, JsonData>(prop_name, value);

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

                //json = null;
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
                    KeyValuePair<string, JsonData> new_entry =
                        new KeyValuePair<string, JsonData>(entry.Key, value);

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

        #region ICollection Methods
        void ICollection.CopyTo(Array array, int index)
        {
            EnsureCollection().CopyTo(array, index);
        }
        #endregion


        #region IDictionary Methods
        void IDictionary.Add(object key, object value)
        {
            JsonData data = ToJsonData(value);

            EnsureDictionary().Add(key, data);

            KeyValuePair<string, JsonData> entry =
                new KeyValuePair<string, JsonData>((string)key, data);
            object_list.Add(entry);

            //json = null;
        }

        void IDictionary.Clear()
        {
            EnsureDictionary().Clear();
            object_list.Clear();
            //json = null;
        }

        bool IDictionary.Contains(object key)
        {
            return EnsureDictionary().Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IOrderedDictionary)this).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            EnsureDictionary().Remove(key);

            for (int i = 0; i < object_list.Count; i++) {
                if (object_list[i].Key == (string)key) {
                    object_list.RemoveAt(i);
                    break;
                }
            }

            //json = null;
        }
        #endregion


        #region IEnumerable Methods
        IEnumerator IEnumerable.GetEnumerator()
        {
            return EnsureCollection().GetEnumerator();
        }
        #endregion


        #region IJsonWrapper Methods

        void IJsonWrapper.SetBoolean(bool val)
        {
            type = JsonType.Boolean;
            inst_boolean = val;
            //json = null;
        }

        void IJsonWrapper.SetDouble(double val)
        {
            type = JsonType.Double;
            inst_double = val;
            //json = null;
        }

        void IJsonWrapper.SetInt(int val)
        {
            type = JsonType.Int;
            inst_int = val;
            //json = null;
        }

        void IJsonWrapper.SetLong(long val)
        {
            type = JsonType.Long;
            inst_long = val;
            //json = null;
        }

        void IJsonWrapper.SetString(string val)
        {
            type = JsonType.String;
            inst_string = val;
            //json = null;
        }

        #endregion


        #region IList Methods
        int IList.Add(object value)
        {
            return Add(value);
        }

        void IList.Clear()
        {
            EnsureList().Clear();
        }

        bool IList.Contains(object value)
        {
            return EnsureList().Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return EnsureList().IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            EnsureList().Insert(index, value);
        }

        void IList.Remove(object value)
        {
            EnsureList().Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            EnsureList().RemoveAt(index);
        }
        #endregion


        #region Private Methods
        private ICollection EnsureCollection()
        {
            if (type == JsonType.Array)
                return (ICollection)inst_array;

            if (type == JsonType.Object)
                return (ICollection)inst_object;

            throw new InvalidOperationException(
                "The JsonData instance has to be initialized first");
        }

        private IDictionary EnsureDictionary()
        {
            if (type == JsonType.Object)
                return (IDictionary)inst_object;

            if (type != JsonType.None)
                throw new InvalidOperationException(
                    "Instance of JsonData is not a dictionary");

            type = JsonType.Object;
            inst_object = new Dictionary<string, JsonData>();
            object_list = new List<KeyValuePair<string, JsonData>>();

            return (IDictionary)inst_object;
        }

        private IList EnsureList()
        {
            if (type == JsonType.Array)
                return (IList)inst_array;

            if (type != JsonType.None)
                throw new InvalidOperationException("Instance of JsonData is not a list");

            type = JsonType.Array;
            inst_array = new List<JsonData>();

            return (IList)inst_array;
        }

        private JsonData ToJsonData(object obj)
        {
            if (obj == null)
                return null;

            if (obj is JsonData)
                return (JsonData)obj;

            return new JsonData(obj);
        }

        #endregion


        public int Add(object value)
        {
            JsonData data = ToJsonData(value);

            return EnsureList().Add(data);
        }

        public void SetJsonType(JsonType type)
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
    }
}
