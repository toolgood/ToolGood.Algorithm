using System;
using System.Collections;
using System.Collections.Generic;


namespace ToolGood.Algorithm.LitJson
{
  sealed  class JsonData : IJsonWrapper, IEnumerable
    {
        #region Fields
        private IList<JsonData> inst_array;
        private bool inst_boolean;
        private decimal inst_double;
        internal IDictionary<string, JsonData> inst_object;
        private string inst_string;
        private JsonType type;
        //private IList<KeyValuePair<string, JsonData>> object_list;
        #endregion


        #region Properties
        public int Count { get { return EnsureCollection().Count; } }
        public bool IsArray { get { return type == JsonType.Array; } }
        public bool IsBoolean { get { return type == JsonType.Boolean; } }
        public bool IsDouble { get { return type == JsonType.Double; } }
        public bool IsObject { get { return type == JsonType.Object; } }
        public bool IsString { get { return type == JsonType.String; } }
        public bool IsNull { get { return type == JsonType.Null; } }
        #endregion



        #region Public Indexers
        public JsonData this[string prop_name] {
            get {
                EnsureDictionary();
                if (inst_object.TryGetValue(prop_name, out JsonData data)) {
                    return data;
                }
                return null;
            }
        }

        public JsonData this[int index] {
            get {
                EnsureCollection();

                if (type == JsonType.Array)
                    return inst_array[index];
                return null;
                //return object_list[index].Value;
            }
        }
        #endregion


        #region Constructors
        public JsonData()
        {
        }

        #endregion



        #region IJsonWrapper Methods

        void IJsonWrapper.SetBoolean(bool val)
        {
            type = JsonType.Boolean;
            inst_boolean = val;
        }

        void IJsonWrapper.SetDouble(decimal val)
        {
            type = JsonType.Double;
            inst_double = (decimal)val;
        }

        void IJsonWrapper.SetString(string val)
        {
            type = JsonType.String;
            inst_string = val;
        }
        void IJsonWrapper.SetNull()
        {
            type = JsonType.Null;
        }

        void IJsonWrapper.Add(IJsonWrapper val)
        {
            EnsureList().Add((val as JsonData));
        }

        void IJsonWrapper.Set(string key, IJsonWrapper val)
        {
            JsonData data = val as JsonData;
            EnsureDictionary()[key] = data;
            //KeyValuePair<string, JsonData> entry = new KeyValuePair<string, JsonData>((string)key, data);
            //object_list.Add(entry);
        }

        #endregion



        #region Private Methods
        private ICollection EnsureCollection()
        {
            if (type == JsonType.Array) return (ICollection)inst_array;
            return (ICollection)inst_object;
        }

        private IDictionary EnsureDictionary()
        {
            if (type == JsonType.Object) return (IDictionary)inst_object;
            type = JsonType.Object;
            inst_object = new Dictionary<string, JsonData>();
            //object_list = new List<KeyValuePair<string, JsonData>>();
            return (IDictionary)inst_object;
        }

        private IList EnsureList()
        {
            if (type == JsonType.Array) return (IList)inst_array;
            type = JsonType.Array;
            inst_array = new List<JsonData>();
            return (IList)inst_array;
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
                    //object_list = new List<KeyValuePair<string, JsonData>>();
                    break;

                case JsonType.Array:
                    inst_array = new List<JsonData>();
                    break;

                case JsonType.String:
                    inst_string = default(String);
                    break;

                case JsonType.Double:
                    inst_double = default(decimal);
                    break;

                case JsonType.Boolean:
                    inst_boolean = default(Boolean);
                    break;
            }

            this.type = type;
        }

        public IEnumerator GetEnumerator()
        {
            return EnsureList().GetEnumerator();
        }


        public bool BooleanValue { get { return inst_boolean; } }
        public decimal NumberValue { get { return inst_double; } }
        public string StringValue { get { return inst_string; } }
    }
}
