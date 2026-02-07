import { JsonData } from './JsonData.js';
import { JsonReader,JsonToken } from './JsonReader.js';
import { JsonType } from './JsonType.js';

class JsonMapper {
    static ReadValue(reader) {
        reader.read();

        if (reader.Token === JsonToken.ArrayEnd) return null;

        let instance = new JsonData();

        if (reader.Token === JsonToken.String) {
            instance.SetString(reader.Value);
            return instance;
        }

        if (reader.Token === JsonToken.Double) {
            instance.SetDouble(reader.Value);
            return instance;
        }

        if (reader.Token === JsonToken.Boolean) {
            instance.SetBoolean(reader.Value);
            return instance;
        }
        if (reader.Token === JsonToken.Null) {
            instance.SetNull();
            return instance;
        }

        if (reader.Token === JsonToken.ArrayStart) {
            instance.SetJsonType(JsonType.Array);

            while (true) {
                let item = JsonMapper.ReadValue(reader);
                if (item === null && reader.Token === JsonToken.ArrayEnd) break;
                instance.Add(item);
            }
        } else if (reader.Token === JsonToken.ObjectStart) {
            instance.SetJsonType(JsonType.Object);

            while (true) {
                reader.read();

                if (reader.Token === JsonToken.ObjectEnd) break;

                let property = reader.Value;
                instance.Set(property, JsonMapper.ReadValue(reader));
            }
        }

        return instance;
    }

    static toObject(json) {
        let reader = new JsonReader(json);
        return JsonMapper.ReadValue(reader);
    }
}

export { JsonMapper };