using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.LitJson
{
	internal sealed class JsonData : IJsonWrapper, IEnumerable
	{
		#region Fields

		private List<JsonData> inst_array;
		private bool inst_boolean;
		private decimal inst_double;
		internal Dictionary<string, JsonData> inst_object;
		private string inst_string;
		private JsonType type;
		//private IList<KeyValuePair<string, JsonData>> object_list;

		#endregion Fields

		#region Properties

		public int Count {
			get {
				if (type == JsonType.Array) return inst_array.Count;
				return inst_object.Count;
			}
		}
		public bool IsArray { get { return type == JsonType.Array; } }
		public bool IsBoolean { get { return type == JsonType.Boolean; } }
		public bool IsDouble { get { return type == JsonType.Double; } }
		public bool IsObject { get { return type == JsonType.Object; } }
		public bool IsString { get { return type == JsonType.String; } }
		public bool IsNull { get { return type == JsonType.Null; } }

		#endregion Properties

		#region Public Indexers

		public JsonData this[string prop_name] {
			get {
				//EnsureDictionary();
				if (inst_object.TryGetValue(prop_name, out JsonData data)) {
					return data;
				}
				return null;
			}
		}

		public JsonData this[int index] {
			get {
				if (type == JsonType.Array)
					return inst_array[index];
				return null;
				//return object_list[index].Value;
			}
		}

		#endregion Public Indexers

		#region Constructors

		public JsonData()
		{
		}

		#endregion Constructors

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

		#endregion IJsonWrapper Methods

		#region Private Methods

		private Dictionary<string, JsonData> EnsureDictionary()
		{
			if (type == JsonType.Object) return inst_object;
			type = JsonType.Object;
			inst_object = new Dictionary<string, JsonData>();
			//object_list = new List<KeyValuePair<string, JsonData>>();
			return inst_object;
		}

		private List<JsonData> EnsureList()
		{
			if (type == JsonType.Array) return inst_array;
			type = JsonType.Array;
			inst_array = new List<JsonData>();
			return inst_array;
		}

		#endregion Private Methods

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
					inst_string = default(string);
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

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			ToString(stringBuilder);
			return stringBuilder.ToString();
		}
		private void ToString(StringBuilder stringBuilder)
		{
			if (IsNull) {
				stringBuilder.Append("null");
			} else if (IsBoolean) {
				stringBuilder.Append(inst_boolean ? "true" : "false");
			} else if (IsArray) {
				stringBuilder.Append("[");
				for (int i = 0; i < inst_array.Count; i++) {
					if (i > 0) {
						stringBuilder.Append(",");
					}
					inst_array[i].ToString(stringBuilder);
				}
				stringBuilder.Append("]");
			} else if (IsObject) {
				stringBuilder.Append("{");
				bool first = true;
				foreach (var kv in inst_object) {
					if (!first) {
						stringBuilder.Append(",");
					}
					first = false;
					stringBuilder.Append("\"");
					stringBuilder.Append(kv.Key.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r")
						.Replace("\t", "\\t").Replace("\0", "\\0").Replace("\v", "\\v")
						.Replace("\a", "\\a").Replace("\b", "\\b").Replace("\f", "\\f"));
					stringBuilder.Append("\":");
					kv.Value.ToString(stringBuilder);
				}
				stringBuilder.Append("}");
			} else if (IsString) {
				stringBuilder.Append("\"");
				stringBuilder.Append(inst_string.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r")
						.Replace("\t", "\\t").Replace("\0", "\\0").Replace("\v", "\\v")
						.Replace("\a", "\\a").Replace("\b", "\\b").Replace("\f", "\\f"));
				stringBuilder.Append("\"");
			} else if (IsDouble) {
				stringBuilder.Append(inst_double.ToString(CultureInfo.InvariantCulture));
			}
		}


	}
}