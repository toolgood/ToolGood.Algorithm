// Operand.h - 对应 csharp/ToolGood.Algorithm/Operand.cs + Operands/*
// C# 的 Operand 是抽象基类 + 多个子类；C++ 用「句柄 + 多态实现」保持同样语义。
#pragma once

#include <cstdint>
#include <initializer_list>
#include <memory>
#include <string>
#include <utility>
#include <vector>

#include "../Enums.h"
#include "../LitJson/JsonData.h"
#include "../MyDate.h"
#include "../Decimal.h"

namespace toolgood {

class Operand;
class OperandBase;
struct KeyValue;
class OperandKeyValueListImpl;

/// 操作数（值语义句柄，内部共享实现）。
class Operand {
public:
    Operand() = default;
    explicit Operand(std::shared_ptr<OperandBase> impl) : p_(std::move(impl)) {}

    // ---- 类型判定 ----
    bool IsNone() const;
    bool IsNull() const;
    bool IsNumber() const;
    bool IsText() const;
    bool IsBoolean() const;
    bool IsArray() const;
    bool IsDate() const;
    bool IsJson() const;
    bool IsArrayJson() const;
    bool IsError() const;
    /// 是否错误或未指定（错误传播用）
    bool IsErrorOrNone() const;
    OperandType Type() const;

    std::string ErrorMsg() const;

    // ---- 取值（类型不符时返回默认值）----
    Decimal NumberValue() const;
    double DoubleValue() const;
    int IntValue() const;
    int64_t LongValue() const;
    std::string TextValue() const;
    bool BooleanValue() const;
    std::vector<Operand> ArrayValue() const;
    const JsonData* JsonValue() const;
    MyDate DateValue() const;

    // ---- 类型转换（失败返回错误操作数）----
    Operand ToNumber(const std::string& errorMessage = std::string()) const;
    Operand ToBoolean(const std::string& errorMessage = std::string()) const;
    Operand ToText(const std::string& errorMessage = std::string()) const;
    Operand ToMyDate(const std::string& errorMessage = std::string()) const;
    Operand ToArray(const std::string& errorMessage = std::string()) const;
    Operand ToJson(const std::string& errorMessage = std::string()) const;

    // ---- 常量 ----
    static const Operand& True();
    static const Operand& False();
    static const Operand& One();
    static const Operand& Zero();
    static const Operand& Null();
    static const Operand& None();
    static const Operand& Version();

    // ---- 工厂 ----
    static Operand Create(bool obj);
    static Operand Create(int obj);
    static Operand Create(int64_t obj);
    static Operand Create(double obj);
    static Operand Create(const Decimal& obj);
    static Operand Create(const std::string& obj);
    static Operand Create(const char* obj);
    static Operand Create(const MyDate& obj);
    static Operand Create(const JsonData& obj);
    static Operand Create(std::vector<Operand> obj);
    static Operand CreateJson(const std::string& txt);
    static Operand Error(const std::string& msg);
    /// 以 {0}/{1}/... 占位符格式化错误信息。
    static Operand ErrorFormat(const std::string& msg, std::initializer_list<std::string> args);

    // ---- JSON 风格输出（对应 ToString）----
    std::string ToString() const;

    /// 是否为键值列表（对应 `is OperandKeyValueList`）
    OperandKeyValueListImpl* AsKeyValueList() const;
    /// 是否为键值项（对应 `is OperandKeyValue`）
    const KeyValue* AsKeyValue() const;

    bool IsValid() const { return p_ != nullptr; }

private:
    std::shared_ptr<OperandBase> p_;
};

/// 键值对（对应 Operands.KeyValue）
struct KeyValue {
    std::string Key;
    Operand Value;
};

/// 操作数实现基类（对应 C# abstract class Operand）。
class OperandBase {
public:
    virtual ~OperandBase() = default;

    virtual OperandType Type() const = 0;

    virtual bool IsNone() const { return false; }
    virtual bool IsNull() const { return false; }
    virtual bool IsNumber() const { return false; }
    virtual bool IsText() const { return false; }
    virtual bool IsBoolean() const { return false; }
    virtual bool IsArray() const { return false; }
    virtual bool IsDate() const { return false; }
    virtual bool IsJson() const { return false; }
    virtual bool IsArrayJson() const { return false; }
    virtual bool IsError() const { return false; }
    virtual bool IsErrorOrNone() const { return false; }
    virtual std::string ErrorMsg() const { return std::string(); }

    virtual Decimal NumberValue() const { return Decimal(); }
    virtual double DoubleValue() const { return 0.0; }
    virtual int IntValue() const { return 0; }
    virtual int64_t LongValue() const { return 0; }
    virtual std::string TextValue() const { return std::string(); }
    virtual bool BooleanValue() const { return false; }
    virtual std::vector<Operand> ArrayValue() const { return {}; }
    virtual const JsonData* JsonValue() const { return nullptr; }
    virtual MyDate DateValue() const { return MyDate(); }

    virtual Operand ToNumber(const std::string& msg) const;
    virtual Operand ToBoolean(const std::string& msg) const;
    virtual Operand ToText(const std::string& msg) const;
    virtual Operand ToMyDate(const std::string& msg) const;
    virtual Operand ToArray(const std::string& msg) const;
    virtual Operand ToJson(const std::string& msg) const;

    virtual std::string ToString() const { return std::string(); }

    virtual OperandKeyValueListImpl* AsKeyValueList() { return nullptr; }
    virtual const KeyValue* AsKeyValue() const { return nullptr; }
};

/// 键值列表实现（对应 Operands.OperandKeyValueList）
class OperandKeyValueListImpl : public OperandBase {
public:
    OperandType Type() const override { return OperandType::ARRAYJSON; }
    bool IsArrayJson() const override { return true; }

    std::vector<Operand> ArrayValue() const override;
    Operand ToText(const std::string& msg) const override;
    Operand ToArray(const std::string& msg) const override;
    Operand ToJson(const std::string& msg) const override;
    std::string ToString() const override;

    void AddValue(KeyValue kv) { list_.push_back(std::move(kv)); }
    bool TryGetValue(const std::string& key, Operand& value) const;
    bool ContainsKey(const std::string& key) const;
    bool ContainsValue(const std::string& text) const;
    const std::vector<KeyValue>& List() const { return list_; }

    OperandKeyValueListImpl* AsKeyValueList() override { return this; }

private:
    std::vector<KeyValue> list_;
};

/// 创建键值项操作数（对应 new OperandKeyValue(keyValue)）
Operand CreateKeyValueOperand(KeyValue kv);

}  // namespace toolgood
