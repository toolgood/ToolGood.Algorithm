// JsonData.h - 对应 csharp/ToolGood.Algorithm/LitJson
// 提供 JSON 树结构与解析（JsonMapper.ToObject）。
#pragma once

#include <string>
#include <utility>
#include <vector>

#include "../Decimal.h"
#include "../Enums.h"

namespace toolgood {

class JsonData;

/// JSON 节点。默认构造为 null 节点。
class JsonData {
public:
    JsonData() = default;

    // ---- 类型判定 ----
    bool IsArray() const { return type_ == JsonType::Array; }
    bool IsBoolean() const { return type_ == JsonType::Boolean; }
    bool IsDouble() const { return type_ == JsonType::Double; }
    bool IsObject() const { return type_ == JsonType::Object; }
    bool IsString() const { return type_ == JsonType::String; }
    bool IsNull() const { return type_ == JsonType::Null || type_ == JsonType::None; }
    JsonType Type() const { return type_; }

    /// 元素个数（对象为键数，数组为长度，字符串为长度）。
    int Count() const;

    // ---- 取值 ----
    bool BooleanValue() const { return boolean_; }
    const Decimal& NumberValue() const { return number_; }
    const std::string& StringValue() const { return string_; }

    // ---- 子元素访问 ----
    /// 对象取键；不存在返回 nullptr。
    JsonData* GetChild(const std::string& key);
    const JsonData* GetChild(const std::string& key) const;
    /// 数组/对象按下标取（对象按插入顺序）。
    JsonData& operator[](int index) { return array_[static_cast<size_t>(index)]; }
    const JsonData& operator[](int index) const { return array_[static_cast<size_t>(index)]; }
    /// 对象取键；不存在时插入 null 节点（对齐 LitJson 行为）。
    JsonData& operator[](const std::string& key);

    bool ContainsKey(const std::string& key) const;
    std::vector<std::string> Keys() const;
    const std::vector<JsonData>& Items() const { return array_; }

    // ---- 构造 ----
    static JsonData NewObject();
    static JsonData NewArray();
    static JsonData NewString(const std::string& v);
    static JsonData NewBoolean(bool v);
    static JsonData NewNumber(const Decimal& v);
    static JsonData NewNull();
    void Add(const JsonData& v);                       // 数组追加
    void Set(const std::string& key, const JsonData& v);  // 对象设置

    /// 序列化为紧凑 JSON（对象键顺序保持插入顺序）。
    std::string ToString() const;

    static std::string EscapeJson(const std::string& s);

private:
    JsonType type_ = JsonType::Null;
    bool boolean_ = false;
    Decimal number_;
    std::string string_;
    std::vector<JsonData> array_;                       // 数组元素或对象值
    std::vector<std::string> objectKeys_;               // 对象键（与 array_ 同序）

    void WriteTo(std::string& sb) const;
};

/// JSON 解析入口（对应 LitJson.JsonMapper）。
class JsonMapper {
public:
    /// 解析 JSON 文本；失败返回 false。
    static bool TryParse(const std::string& text, JsonData& out);
    /// 解析 JSON 文本；失败抛 std::runtime_error。
    static JsonData ToObject(const std::string& text);
};

}  // namespace toolgood
