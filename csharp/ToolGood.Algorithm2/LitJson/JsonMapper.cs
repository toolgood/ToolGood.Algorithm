namespace ToolGood.Algorithm.LitJson
{
   sealed class JsonMapper
    {

        #region Private Methods

        private static IJsonWrapper ReadValue(JsonReader reader)
        {
            reader.Read();

            if (reader.Token == JsonToken.ArrayEnd) return null;

            IJsonWrapper instance = new JsonData();

            if (reader.Token == JsonToken.String) {
                instance.SetString((string)reader.Value);
                return instance;
            }

            if (reader.Token == JsonToken.Double) {
                instance.SetDouble((decimal)reader.Value);
                return instance;
            }

            if (reader.Token == JsonToken.Boolean) {
                instance.SetBoolean((bool)reader.Value);
                return instance;
            }
            if (reader.Token == JsonToken.Null) {
                instance.SetNull();
                return instance;
            }


            if (reader.Token == JsonToken.ArrayStart) {
                instance.SetJsonType(JsonType.Array);

                while (true) {
                    IJsonWrapper item = ReadValue(reader);
                    if (item == null && reader.Token == JsonToken.ArrayEnd) break;
                    instance.Add(item);
                }
            } else if (reader.Token == JsonToken.ObjectStart) {
                instance.SetJsonType(JsonType.Object);

                while (true) {
                    reader.Read();

                    if (reader.Token == JsonToken.ObjectEnd) break;

                    string property = (string)reader.Value;
                    instance.Set(property, ReadValue(reader));
                }

            }

            return instance;
        }

        #endregion



        public static JsonData ToObject(string json)
        {
            JsonReader reader = new JsonReader(json);
            return ReadValue(reader) as JsonData;
        }

    }
}
