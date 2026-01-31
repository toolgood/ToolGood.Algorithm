package toolgood.algorithm.litJson;

import java.math.BigDecimal;

public class JsonMapper {
    private static JsonData ReadValue(JsonReader reader) throws JsonException
    {
        reader.Read();

        if (reader.Token() == JsonToken.ArrayEnd) return null;

        JsonData instance = new JsonData();

        if (reader.Token() == JsonToken.String) {
            instance.SetString((String)reader.Value());
            return instance;
        }

        if (reader.Token() == JsonToken.Double) {
            instance.SetDouble((BigDecimal)reader.Value());
            return instance;
        }

        if (reader.Token() == JsonToken.Boolean) {
            instance.SetBoolean((boolean)reader.Value());
            return instance;
        }
        if (reader.Token() == JsonToken.Null)
        {
            instance.SetNull();
            return instance;
        }


        if (reader.Token() == JsonToken.ArrayStart) {
            instance.SetJsonType(JsonType.Array);

            while (true) {
                JsonData item = ReadValue(reader);
                if (item == null && reader.Token() == JsonToken.ArrayEnd) break;
                instance.Add((IJsonWrapper)item);
            }
        } else if (reader.Token() == JsonToken.ObjectStart) {
            instance.SetJsonType(JsonType.Object);

            while (true) {
                reader.Read();

                if (reader.Token() == JsonToken.ObjectEnd) break;

                String property = (String)reader.Value();
                instance.Set(property,(IJsonWrapper) ReadValue(reader));
            }

        }

        return instance;
    }


    public static JsonData ToObject(String json) throws JsonException
    {
        JsonReader reader = new JsonReader(json);
        return (JsonData)ReadValue(reader) ;
    }
}