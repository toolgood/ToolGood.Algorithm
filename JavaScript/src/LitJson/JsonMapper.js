import { JsonData } from './JsonData.js';
import { JsonReader,JsonToken } from './JsonReader.js';
import { JsonType } from './JsonType.js';

class JsonMapper {
    static readValue(reader) {
        reader.read();

        if (reader.Token === JsonToken.ArrayEnd) return null;

        let instance = new JsonData();

        if (reader.Token === JsonToken.String) {
            instance.setString(reader.Value);
            return instance;
        }

        if (reader.Token === JsonToken.Double) {
            instance.setDouble(reader.Value);
            return instance;
        }

        if (reader.Token === JsonToken.Boolean) {
            instance.setBoolean(reader.Value);
            return instance;
        }
        if (reader.Token === JsonToken.Null) {
            instance.setNull();
            return instance;
        }

        if (reader.Token === JsonToken.ArrayStart) {
            instance.setJsonType(JsonType.Array);

            while (true) {
                let item = JsonMapper.readValue(reader);
                if (item === null && reader.Token === JsonToken.ArrayEnd) break;
                instance.add(item);
            }
        } else if (reader.Token === JsonToken.ObjectStart) {
            instance.setJsonType(JsonType.Object);

            while (true) {
                reader.read();

                if (reader.Token === JsonToken.ObjectEnd) break;

                let property = reader.Value;
                instance.set(property, JsonMapper.readValue(reader));
            }
        }

        return instance;
    }

    static toObject(json) {
        let reader = new JsonReader(json);
        return JsonMapper.readValue(reader);
    }
}

export { JsonMapper };