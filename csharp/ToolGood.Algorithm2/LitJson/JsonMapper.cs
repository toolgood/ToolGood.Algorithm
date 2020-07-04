using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;


namespace ToolGood.Algorithm.LitJson
{
    internal delegate object ImporterFunc(object input);
    class JsonMapper
    {

        #region Private Methods

        private static IJsonWrapper ReadValue(JsonReader reader)
        {
            reader.Read();

            if (reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null) return null;

            IJsonWrapper instance = new JsonData();

            if (reader.Token == JsonToken.String) {
                instance.SetString((string)reader.Value);
                return instance;
            }

            if (reader.Token == JsonToken.Double) {
                instance.SetDouble((double)reader.Value);
                return instance;
            }

            if (reader.Token == JsonToken.Int) {
                instance.SetInt((int)reader.Value);
                return instance;
            }

            if (reader.Token == JsonToken.Long) {
                instance.SetLong((long)reader.Value);
                return instance;
            }

            if (reader.Token == JsonToken.Boolean) {
                instance.SetBoolean((bool)reader.Value);
                return instance;
            }

            if (reader.Token == JsonToken.ArrayStart) {
                instance.SetJsonType(JsonType.Array);

                while (true) {
                    IJsonWrapper item = ReadValue(reader);
                    if (item == null && reader.Token == JsonToken.ArrayEnd) break;
                    instance.Add(item);
                    //((IList)instance).Add(item);
                }
            } else if (reader.Token == JsonToken.ObjectStart) {
                instance.SetJsonType(JsonType.Object);

                while (true) {
                    reader.Read();

                    if (reader.Token == JsonToken.ObjectEnd) break;

                    string property = (string)reader.Value;
                    instance.Set(property, ReadValue(reader));
                    //((IDictionary)instance)[property] = ReadValue(factory, reader);
                }

            }

            return instance;
        }

        #endregion



        public static JsonData ToObject(string json)
        {
            return (JsonData)ToWrapper(json);
        }


        public static IJsonWrapper ToWrapper(string json)
        {
            JsonReader reader = new JsonReader(json);
            return ReadValue(reader);
        }



    }
}
