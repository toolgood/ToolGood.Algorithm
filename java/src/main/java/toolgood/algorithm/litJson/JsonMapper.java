package toolgood.algorithm.litJson;

public class JsonMapper {
    private static JsonData ReadValue(JsonReader reader) throws Exception {
        reader.Read();

        if (reader.Token() == JsonToken.ArrayEnd) return null;

        JsonData instance = new JsonData();

        if (reader.Token() == JsonToken.String) {
            instance.SetString((String) reader.Value());
            return instance;
        }

        if (reader.Token() == JsonToken.Double) {
            Object value = reader.Value();
            if (value instanceof Number) {
                instance.SetDouble(((Number) value).doubleValue());
            } else {
                instance.SetDouble(0);
            }
            return instance;
        }

        if (reader.Token() == JsonToken.Boolean) {
            instance.SetBoolean((Boolean) reader.Value());
            return instance;
        }
        if (reader.Token() == JsonToken.Null) {
            instance.SetNull();
            return instance;
        }

        if (reader.Token() == JsonToken.ArrayStart) {
            instance.SetJsonType(JsonType.Array);

            while (true) {
                JsonData item = ReadValue(reader);
                if (item == null && reader.Token() == JsonToken.ArrayEnd) break;
                instance.Add(item);
            }
        } else if (reader.Token() == JsonToken.ObjectStart) {
            instance.SetJsonType(JsonType.Object);

            while (true) {
                reader.Read();

                if (reader.Token() == JsonToken.ObjectEnd) break;

                String property = (String) reader.Value();
                instance.Set(property, ReadValue(reader));
            }
        }

        return instance;
    }

    public static JsonData ToObject(String json) throws Exception {
        JsonReader reader = new JsonReader(json);
        return ReadValue(reader);
    }
}
