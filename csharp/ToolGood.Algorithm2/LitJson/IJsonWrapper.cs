using System.Collections;
using System.Collections.Specialized;


namespace ToolGood.Algorithm.LitJson
{
   enum JsonType
    {
        None,

        Object,
        Array,
        String,
        Int,
        Long,
        Double,
        Boolean
    }

    interface IJsonWrapper  
    {
        bool IsArray { get; }
        bool IsBoolean { get; }
        bool IsDouble { get; }
        bool IsInt { get; }
        bool IsLong { get; }
        bool IsObject { get; }
        bool IsString { get; }

        void SetBoolean(bool val);
        void SetDouble(double val);
        void SetInt(int val);
        void SetJsonType(JsonType type);
        void SetLong(long val);
        void SetString(string val);

    }
}
