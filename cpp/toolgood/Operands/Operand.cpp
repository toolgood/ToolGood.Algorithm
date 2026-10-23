#include "Operand.h"

#include <cstdio>
#include <cstdlib>

#include "../Internals/FunctionUtil.h"

namespace toolgood {

namespace {

std::string Format(const std::string& fmt, const std::initializer_list<std::string>& args) {
    std::string out;
    out.reserve(fmt.size() + 16);
    size_t i = 0;
    while (i < fmt.size()) {
        if (fmt[i] == '{') {
            size_t j = i + 1;
            int idx = 0;
            bool numeric = false;
            while (j < fmt.size() && fmt[j] >= '0' && fmt[j] <= '9') {
                idx = idx * 10 + (fmt[j] - '0');
                numeric = true;
                j++;
            }
            if (numeric && j < fmt.size() && fmt[j] == '}') {
                size_t k = 0;
                for (auto it = args.begin(); it != args.end(); ++it, ++k) {
                    if (static_cast<int>(k) == idx) {
                        out += *it;
                        break;
                    }
                }
                i = j + 1;
                continue;
            }
        }
        out += fmt[i++];
    }
    return out;
}

// ---- 具体实现 ----

class OperandIntImpl : public OperandBase {
public:
    explicit OperandIntImpl(int v) : v_(v) {}
    OperandType Type() const override { return OperandType::NUMBER; }
    bool IsNumber() const override { return true; }
    int IntValue() const override { return v_; }
    int64_t LongValue() const override { return v_; }
    Decimal NumberValue() const override { return Decimal::FromInt(v_); }
    double DoubleValue() const override { return static_cast<double>(v_); }
    Operand ToNumber(const std::string&) const override { return Operand(std::make_shared<OperandIntImpl>(v_)); }
    Operand ToBoolean(const std::string& msg) const override {
        if (v_ == 0) return Operand::False();
        if (v_ == 1) return Operand::True();
        return OperandBase::ToBoolean(msg);
    }
    Operand ToText(const std::string&) const override { return Operand::Create(std::to_string(v_)); }
    std::string ToString() const override { return std::to_string(v_); }

private:
    int v_;
};

class OperandDecimalImpl : public OperandBase {
public:
    explicit OperandDecimalImpl(const Decimal& v) : v_(v) {}
    OperandType Type() const override { return OperandType::NUMBER; }
    bool IsNumber() const override { return true; }
    int IntValue() const override { return v_.Int(); }
    int64_t LongValue() const override { return v_.Int64(); }
    Decimal NumberValue() const override { return v_; }
    double DoubleValue() const override { return v_.Float64(); }
    Operand ToNumber(const std::string&) const override { return Operand(std::make_shared<OperandDecimalImpl>(v_)); }
    Operand ToBoolean(const std::string& msg) const override {
        if (v_.IsZero()) return Operand::False();
        if (v_.Equal(Decimal::One)) return Operand::True();
        return OperandBase::ToBoolean(msg);
    }
    Operand ToText(const std::string&) const override { return Operand::Create(v_.ToString()); }
    std::string ToString() const override { return v_.ToString(); }

private:
    Decimal v_;
};

class OperandStringImpl : public OperandBase {
public:
    explicit OperandStringImpl(std::string v) : v_(std::move(v)) {}
    OperandType Type() const override { return OperandType::TEXT; }
    bool IsText() const override { return true; }
    std::string TextValue() const override { return v_; }

    Operand ToNumber(const std::string& msg) const override {
        const std::string& text = v_;
        if (text.find('.') == std::string::npos) {
            // 尝试 int
            bool ok = !text.empty();
            size_t i = 0;
            if (ok && (text[0] == '+' || text[0] == '-')) i = 1;
            if (i >= text.size()) ok = false;
            for (size_t k = i; ok && k < text.size(); k++) {
                if (text[k] < '0' || text[k] > '9') ok = false;
            }
            if (ok) {
                errno = 0;
                long long val = std::strtoll(text.c_str(), nullptr, 10);
                if (errno == 0 && val >= INT32_MIN && val <= INT32_MAX) {
                    return Operand::Create(static_cast<int>(val));
                }
            }
        }
        Decimal d;
        if (Decimal::TryParse(text, d)) return Operand::Create(d);
        return Operand::Error(msg.empty() ? "Convert to number error!" : msg);
    }

    Operand ToText(const std::string&) const override { return Operand(std::make_shared<OperandStringImpl>(v_)); }

    Operand ToBoolean(const std::string& msg) const override {
        bool b = false;
        if (TryParseBoolean(v_, b)) return b ? Operand::True() : Operand::False();
        return Operand::Error(msg.empty() ? "Convert to bool error!" : msg);
    }

    Operand ToMyDate(const std::string& msg) const override {
        MyDate d;
        if (MyDate::TryParse(v_, d)) return Operand::Create(d);
        return Operand::Error(msg.empty() ? "Convert to date error!" : msg);
    }

    Operand ToArray(const std::string& msg) const override {
        return Operand::Error(msg.empty() ? "Convert to array error!" : msg);
    }

    Operand ToJson(const std::string& msg) const override {
        // 去首尾空白
        size_t b = 0, e = v_.size();
        while (b < e && (v_[b] == ' ' || v_[b] == '\t' || v_[b] == '\r' || v_[b] == '\n')) b++;
        while (e > b && (v_[e - 1] == ' ' || v_[e - 1] == '\t' || v_[e - 1] == '\r' || v_[e - 1] == '\n')) e--;
        if (b < e) {
            char c0 = v_[b], c1 = v_[e - 1];
            if ((c0 == '{' && c1 == '}') || (c0 == '[' && c1 == ']')) {
                JsonData json;
                if (JsonMapper::TryParse(v_.substr(b, e - b), json)) return Operand::Create(json);
            }
        }
        return Operand::Error(msg.empty() ? "Convert to json error!" : msg);
    }

    std::string ToString() const override {
        if (v_.empty()) return "\"\"";
        bool needEscape = false;
        for (char c : v_) {
            if (c == '"' || c == '\\' || static_cast<unsigned char>(c) < ' ') {
                needEscape = true;
                break;
            }
        }
        if (!needEscape) return "\"" + v_ + "\"";
        std::string sb = "\"";
        char buf[8];
        for (char c : v_) {
            switch (c) {
                case '"': sb += "\\\""; break;
                case '\\': sb += "\\\\"; break;
                case '\n': sb += "\\n"; break;
                case '\r': sb += "\\r"; break;
                case '\t': sb += "\\t"; break;
                case '\b': sb += "\\b"; break;
                case '\f': sb += "\\f"; break;
                default:
                    if (static_cast<unsigned char>(c) < ' ') {
                        std::snprintf(buf, sizeof(buf), "\\u%04x", static_cast<unsigned char>(c));
                        sb += buf;
                    } else {
                        sb += c;
                    }
                    break;
            }
        }
        sb += "\"";
        return sb;
    }

private:
    std::string v_;
};

class OperandBooleanImpl : public OperandBase {
public:
    explicit OperandBooleanImpl(bool v) : v_(v) {}
    OperandType Type() const override { return OperandType::BOOLEAN; }
    bool IsBoolean() const override { return true; }
    bool BooleanValue() const override { return v_; }
    Operand ToNumber(const std::string&) const override {
        return v_ ? Operand::One() : Operand::Zero();
    }
    Operand ToBoolean(const std::string&) const override { return Operand(std::make_shared<OperandBooleanImpl>(v_)); }
    Operand ToText(const std::string&) const override { return Operand::Create(v_ ? "TRUE" : "FALSE"); }
    std::string ToString() const override { return v_ ? "true" : "false"; }

private:
    bool v_;
};

class OperandNullImpl : public OperandBase {
public:
    OperandType Type() const override { return OperandType::NULL_VALUE; }
    bool IsNull() const override { return true; }
    std::string ToString() const override { return "null"; }
};

class OperandNoneImpl : public OperandBase {
public:
    OperandType Type() const override { return OperandType::NONE; }
    bool IsNone() const override { return true; }
    bool IsErrorOrNone() const override { return true; }
    std::string ToString() const override { return "none"; }
};

class OperandArrayImpl : public OperandBase {
public:
    explicit OperandArrayImpl(std::vector<Operand> v) : v_(std::move(v)) {}
    OperandType Type() const override { return OperandType::ARRAY; }
    bool IsArray() const override { return true; }
    std::vector<Operand> ArrayValue() const override { return v_; }
    Operand ToText(const std::string&) const override { return Operand::Create(ToString()); }
    Operand ToArray(const std::string&) const override { return Operand(std::make_shared<OperandArrayImpl>(v_)); }
    Operand ToJson(const std::string& msg) const override {
        JsonData json;
        if (JsonMapper::TryParse(ToString(), json)) return Operand::Create(json);
        return Operand::Error(msg.empty() ? "Convert to json error!" : msg);
    }
    std::string ToString() const override {
        std::string sb = "[";
        for (size_t i = 0; i < v_.size(); i++) {
            if (i > 0) sb += ',';
            sb += v_[i].ToString();
        }
        sb += ']';
        return sb;
    }

private:
    std::vector<Operand> v_;
};

class OperandErrorImpl : public OperandBase {
public:
    explicit OperandErrorImpl(std::string msg) : msg_(std::move(msg)) {}
    OperandType Type() const override { return OperandType::ERROR; }
    bool IsError() const override { return true; }
    bool IsErrorOrNone() const override { return true; }
    std::string ErrorMsg() const override { return msg_; }
    Operand ToNumber(const std::string&) const override { return Operand(std::make_shared<OperandErrorImpl>(msg_)); }
    Operand ToBoolean(const std::string&) const override { return Operand(std::make_shared<OperandErrorImpl>(msg_)); }
    Operand ToText(const std::string&) const override { return Operand(std::make_shared<OperandErrorImpl>(msg_)); }
    Operand ToMyDate(const std::string&) const override { return Operand(std::make_shared<OperandErrorImpl>(msg_)); }
    Operand ToArray(const std::string&) const override { return Operand(std::make_shared<OperandErrorImpl>(msg_)); }
    Operand ToJson(const std::string&) const override { return Operand(std::make_shared<OperandErrorImpl>(msg_)); }
    std::string ToString() const override { return msg_; }

private:
    std::string msg_;
};

class OperandMyDateImpl : public OperandBase {
public:
    explicit OperandMyDateImpl(const MyDate& v) : v_(v) {}
    OperandType Type() const override { return OperandType::DATE; }
    bool IsDate() const override { return true; }
    MyDate DateValue() const override { return v_; }
    Operand ToText(const std::string&) const override { return Operand::Create(v_.ToString()); }
    Operand ToMyDate(const std::string&) const override { return Operand(std::make_shared<OperandMyDateImpl>(v_)); }
    std::string ToString() const override { return "\"" + v_.ToString() + "\""; }

private:
    MyDate v_;
};

class OperandJsonImpl : public OperandBase {
public:
    explicit OperandJsonImpl(JsonData v) : v_(std::move(v)) {}
    OperandType Type() const override { return OperandType::JSON; }
    bool IsJson() const override { return true; }
    const JsonData* JsonValue() const override { return &v_; }
    Operand ToText(const std::string&) const override { return Operand::Create(v_.ToString()); }
    Operand ToArray(const std::string& msg) const override {
        if (v_.IsArray()) {
            std::vector<Operand> list;
            for (const auto& item : v_.Items()) {
                if (item.IsString()) list.push_back(Operand::Create(item.StringValue()));
                else if (item.IsBoolean()) list.push_back(Operand::Create(item.BooleanValue()));
                else if (item.IsDouble()) list.push_back(Operand::Create(item.NumberValue()));
                else if (item.IsNull()) list.push_back(Operand::Null());
                else list.push_back(Operand::Create(item));
            }
            return Operand::Create(std::move(list));
        }
        return Operand::Error(msg.empty() ? "Convert to array error!" : msg);
    }
    Operand ToJson(const std::string&) const override { return Operand(std::make_shared<OperandJsonImpl>(v_)); }
    std::string ToString() const override { return v_.ToString(); }

private:
    JsonData v_;
};

class OperandKeyValueImpl : public OperandBase {
public:
    explicit OperandKeyValueImpl(KeyValue v) : v_(std::move(v)) {}
    OperandType Type() const override { return OperandType::ARRAYJSON; }
    bool IsArrayJson() const override { return true; }
    const KeyValue* AsKeyValue() const override { return &v_; }
    std::string ToString() const override {
        return Operand::Create(v_.Key).ToString() + ":" + v_.Value.ToString();
    }
    KeyValue& Value() { return v_; }

private:
    KeyValue v_;
};

}  // namespace

// ================= Operand 转发 =================

bool Operand::IsNone() const { return p_ && p_->IsNone(); }
bool Operand::IsNull() const { return !p_ || p_->IsNull(); }
bool Operand::IsNumber() const { return p_ && p_->IsNumber(); }
bool Operand::IsText() const { return p_ && p_->IsText(); }
bool Operand::IsBoolean() const { return p_ && p_->IsBoolean(); }
bool Operand::IsArray() const { return p_ && p_->IsArray(); }
bool Operand::IsDate() const { return p_ && p_->IsDate(); }
bool Operand::IsJson() const { return p_ && p_->IsJson(); }
bool Operand::IsArrayJson() const { return p_ && p_->IsArrayJson(); }
bool Operand::IsError() const { return p_ && p_->IsError(); }
bool Operand::IsErrorOrNone() const { return !p_ || p_->IsErrorOrNone(); }

OperandType Operand::Type() const {
    return p_ ? p_->Type() : OperandType::NULL_VALUE;
}

std::string Operand::ErrorMsg() const { return p_ ? p_->ErrorMsg() : std::string(); }
Decimal Operand::NumberValue() const { return p_ ? p_->NumberValue() : Decimal(); }
double Operand::DoubleValue() const { return p_ ? p_->DoubleValue() : 0.0; }
int Operand::IntValue() const { return p_ ? p_->IntValue() : 0; }
int64_t Operand::LongValue() const { return p_ ? p_->LongValue() : 0; }
std::string Operand::TextValue() const { return p_ ? p_->TextValue() : std::string(); }
bool Operand::BooleanValue() const { return p_ ? p_->BooleanValue() : false; }
std::vector<Operand> Operand::ArrayValue() const { return p_ ? p_->ArrayValue() : std::vector<Operand>(); }
const JsonData* Operand::JsonValue() const { return p_ ? p_->JsonValue() : nullptr; }
MyDate Operand::DateValue() const { return p_ ? p_->DateValue() : MyDate(); }

Operand Operand::ToNumber(const std::string& msg) const {
    if (!p_) return Error(msg.empty() ? "Convert to number error!" : msg);
    return p_->ToNumber(msg);
}
Operand Operand::ToBoolean(const std::string& msg) const {
    if (!p_) return Error(msg.empty() ? "Convert to bool error!" : msg);
    return p_->ToBoolean(msg);
}
Operand Operand::ToText(const std::string& msg) const {
    if (!p_) return Error(msg.empty() ? "Convert to string error!" : msg);
    return p_->ToText(msg);
}
Operand Operand::ToMyDate(const std::string& msg) const {
    if (!p_) return Error(msg.empty() ? "Convert to date error!" : msg);
    return p_->ToMyDate(msg);
}
Operand Operand::ToArray(const std::string& msg) const {
    if (!p_) return Error(msg.empty() ? "Convert to array error!" : msg);
    return p_->ToArray(msg);
}
Operand Operand::ToJson(const std::string& msg) const {
    if (!p_) return Error(msg.empty() ? "Convert to json error!" : msg);
    return p_->ToJson(msg);
}

std::string Operand::ToString() const { return p_ ? p_->ToString() : "null"; }

OperandKeyValueListImpl* Operand::AsKeyValueList() const {
    return p_ ? p_->AsKeyValueList() : nullptr;
}

const KeyValue* Operand::AsKeyValue() const { return p_ ? p_->AsKeyValue() : nullptr; }

// ================= 常量 =================

const Operand& Operand::True() {
    static const Operand v(std::make_shared<OperandBooleanImpl>(true));
    return v;
}

const Operand& Operand::False() {
    static const Operand v(std::make_shared<OperandBooleanImpl>(false));
    return v;
}

const Operand& Operand::One() {
    static const Operand v = Create(1);
    return v;
}

const Operand& Operand::Zero() {
    static const Operand v = Create(0);
    return v;
}

const Operand& Operand::Null() {
    static const Operand v(std::make_shared<OperandNullImpl>());
    return v;
}

const Operand& Operand::None() {
    static const Operand v(std::make_shared<OperandNoneImpl>());
    return v;
}

const Operand& Operand::Version() {
    static const Operand v = Create("ToolGood.Algorithm 6.3");
    return v;
}

// ================= 工厂 =================

namespace {
std::vector<Operand>& IntCache() {
    static std::vector<Operand> cache = [] {
        std::vector<Operand> v;
        v.reserve(2001);
        for (int i = -1000; i <= 1000; i++) {
            v.push_back(Operand(std::make_shared<OperandIntImpl>(i)));
        }
        return v;
    }();
    return cache;
}
const int kIntCacheOffset = 1000;
}  // namespace

Operand Operand::Create(bool obj) { return obj ? True() : False(); }

Operand Operand::Create(int obj) {
    if (obj >= -kIntCacheOffset && obj <= kIntCacheOffset) {
        return IntCache()[static_cast<size_t>(obj + kIntCacheOffset)];
    }
    return Operand(std::make_shared<OperandIntImpl>(obj));
}

Operand Operand::Create(int64_t obj) {
    if (obj >= -kIntCacheOffset && obj <= kIntCacheOffset) {
        return IntCache()[static_cast<size_t>(obj + kIntCacheOffset)];
    }
    return Operand(std::make_shared<OperandDecimalImpl>(Decimal::FromInt(obj)));
}

Operand Operand::Create(double obj) { return Create(Decimal::FromDouble(obj)); }

Operand Operand::Create(const Decimal& obj) {
    if (obj.IsInteger() && obj.FitsInt32()) {
        int v = obj.Int();
        if (v >= -kIntCacheOffset && v <= kIntCacheOffset) {
            return IntCache()[static_cast<size_t>(v + kIntCacheOffset)];
        }
    }
    return Operand(std::make_shared<OperandDecimalImpl>(obj));
}

Operand Operand::Create(const std::string& obj) {
    return Operand(std::make_shared<OperandStringImpl>(obj));
}

Operand Operand::Create(const char* obj) {
    if (obj == nullptr) return Null();
    return Operand(std::make_shared<OperandStringImpl>(std::string(obj)));
}

Operand Operand::Create(const MyDate& obj) {
    return Operand(std::make_shared<OperandMyDateImpl>(obj));
}

Operand Operand::Create(const JsonData& obj) {
    return Operand(std::make_shared<OperandJsonImpl>(obj));
}

Operand Operand::Create(std::vector<Operand> obj) {
    return Operand(std::make_shared<OperandArrayImpl>(std::move(obj)));
}

Operand Operand::CreateJson(const std::string& txt) {
    if (txt.size() >= 2) {
        char c0 = txt[0], c1 = txt[txt.size() - 1];
        if ((c0 == '{' && c1 == '}') || (c0 == '[' && c1 == ']')) {
            JsonData json;
            if (JsonMapper::TryParse(txt, json)) return Operand::Create(json);
        }
    }
    return Operand::Error("Convert to json error!");
}

Operand Operand::Error(const std::string& msg) {
    return Operand(std::make_shared<OperandErrorImpl>(msg));
}

Operand Operand::ErrorFormat(const std::string& msg, std::initializer_list<std::string> args) {
    return Error(Format(msg, args));
}

// ================= 实现基类默认行为 =================

Operand OperandBase::ToNumber(const std::string& msg) const {
    return Operand::Error(msg.empty() ? "Convert to number error!" : msg);
}
Operand OperandBase::ToBoolean(const std::string& msg) const {
    return Operand::Error(msg.empty() ? "Convert to bool error!" : msg);
}
Operand OperandBase::ToText(const std::string& msg) const {
    return Operand::Error(msg.empty() ? "Convert to string error!" : msg);
}
Operand OperandBase::ToMyDate(const std::string& msg) const {
    return Operand::Error(msg.empty() ? "Convert to date error!" : msg);
}
Operand OperandBase::ToArray(const std::string& msg) const {
    return Operand::Error(msg.empty() ? "Convert to array error!" : msg);
}
Operand OperandBase::ToJson(const std::string& msg) const {
    return Operand::Error(msg.empty() ? "Convert to json error!" : msg);
}

// ================= OperandKeyValueListImpl =================

std::vector<Operand> OperandKeyValueListImpl::ArrayValue() const {
    std::vector<Operand> list;
    list.reserve(list_.size());
    for (const auto& kv : list_) list.push_back(kv.Value);
    return list;
}

Operand OperandKeyValueListImpl::ToText(const std::string&) const { return Operand::Create(ToString()); }

Operand OperandKeyValueListImpl::ToArray(const std::string&) const {
    return Operand::Create(ArrayValue());
}

Operand OperandKeyValueListImpl::ToJson(const std::string& msg) const {
    JsonData json;
    if (JsonMapper::TryParse(ToString(), json)) return Operand::Create(json);
    return Operand::Error(msg.empty() ? "Convert to json error!" : msg);
}

std::string OperandKeyValueListImpl::ToString() const {
    std::string sb = "{";
    for (size_t i = 0; i < list_.size(); i++) {
        if (i > 0) sb += ',';
        sb += Operand::Create(list_[i].Key).ToString();
        sb += ':';
        sb += list_[i].Value.ToString();
    }
    sb += '}';
    return sb;
}

bool OperandKeyValueListImpl::TryGetValue(const std::string& key, Operand& value) const {
    for (const auto& kv : list_) {
        if (kv.Key == key) {
            value = kv.Value;
            return true;
        }
    }
    return false;
}

bool OperandKeyValueListImpl::ContainsKey(const std::string& key) const {
    for (const auto& kv : list_) {
        if (kv.Key == key) return true;
    }
    return false;
}

bool OperandKeyValueListImpl::ContainsValue(const std::string& text) const {
    for (const auto& kv : list_) {
        if (OperandValueEquals(kv.Value, text)) return true;
    }
    return false;
}

/// 创建键值项操作数（对应 `new OperandKeyValue(keyValue)`）
Operand CreateKeyValueOperand(KeyValue kv) {
    return Operand(std::make_shared<OperandKeyValueImpl>(std::move(kv)));
}

}  // namespace toolgood
