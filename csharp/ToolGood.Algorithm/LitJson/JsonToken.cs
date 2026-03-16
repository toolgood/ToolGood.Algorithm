namespace ToolGood.Algorithm.LitJson
{
	internal enum JsonToken
    {
        None,

        ObjectStart,
        PropertyName,
        ObjectEnd,

        ArrayStart,
        ArrayEnd,

        Double,

        String,

        Boolean,
        Null
    }
}