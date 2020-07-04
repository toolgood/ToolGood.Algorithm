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
        #region Fields
        private static readonly IFormatProvider datetime_format;

        private static readonly IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table;

        #endregion


        #region Constructors
        static JsonMapper()
        {
            datetime_format = DateTimeFormatInfo.InvariantInfo;

            base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();

            RegisterBaseImporters();
        }
        #endregion


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

        private static void RegisterBaseImporters()
        {
            ImporterFunc importer;

            importer = delegate (object input) {
                return Convert.ToByte((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(byte), importer);

            importer = delegate (object input) {
                return Convert.ToUInt64((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(ulong), importer);

            importer = delegate (object input) {
                return Convert.ToInt64((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(long), importer);

            importer = delegate (object input) {
                return Convert.ToSByte((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(sbyte), importer);

            importer = delegate (object input) {
                return Convert.ToInt16((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(short), importer);

            importer = delegate (object input) {
                return Convert.ToUInt16((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(ushort), importer);

            importer = delegate (object input) {
                return Convert.ToUInt32((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(uint), importer);

            importer = delegate (object input) {
                return Convert.ToSingle((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(float), importer);

            importer = delegate (object input) {
                return Convert.ToDouble((int)input);
            };
            RegisterImporter(base_importers_table, typeof(int), typeof(double), importer);

            importer = delegate (object input) {
                return Convert.ToDecimal((double)input);
            };
            RegisterImporter(base_importers_table, typeof(double), typeof(decimal), importer);


            importer = delegate (object input) {
                return Convert.ToUInt32((long)input);
            };
            RegisterImporter(base_importers_table, typeof(long), typeof(uint), importer);

            importer = delegate (object input) {
                return Convert.ToChar((string)input);
            };
            RegisterImporter(base_importers_table, typeof(string), typeof(char), importer);

            importer = delegate (object input) {
                return Convert.ToDateTime((string)input, datetime_format);
            };
            RegisterImporter(base_importers_table, typeof(string), typeof(DateTime), importer);

            importer = delegate (object input) {
                return DateTimeOffset.Parse((string)input, datetime_format);
            };
            RegisterImporter(base_importers_table, typeof(string), typeof(DateTimeOffset), importer);
        }

        private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
        {
            if (!table.ContainsKey(json_type))
                table.Add(json_type, new Dictionary<Type, ImporterFunc>());

            table[json_type][value_type] = importer;
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
