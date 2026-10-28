namespace ToolGood.Algorithm.LitJson
{
    internal sealed class JsonMapper
    {
        /// <summary>
        /// 解析 JSON 文本为 <see cref="JsonData"/>。
        /// 实现在 <see cref="JsonFastParser"/>：对 string 直接下标扫描的递归下降解析，
        /// 替代原来的 Lexer(字符流状态机) + JsonReader(LL 自动机) + 本类递归组装三层结构。
        /// </summary>
        public static JsonData ToObject(string json)
        {
            return JsonFastParser.Parse(json);
        }
    }
}
