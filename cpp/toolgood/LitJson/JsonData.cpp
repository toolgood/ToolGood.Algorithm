#include "JsonData.h"

#include <cmath>
#include <cstdio>
#include <cstdlib>
#include <stdexcept>

namespace toolgood {

int JsonData::Count() const {
    switch (type_) {
        case JsonType::Array:
        case JsonType::Object:
            return static_cast<int>(array_.size());
        case JsonType::String:
            return static_cast<int>(string_.size());
        default:
            return 0;
    }
}

JsonData* JsonData::GetChild(const std::string& key) {
    if (type_ != JsonType::Object) return nullptr;
    for (size_t i = 0; i < objectKeys_.size(); i++) {
        if (objectKeys_[i] == key) return &array_[i];
    }
    return nullptr;
}

const JsonData* JsonData::GetChild(const std::string& key) const {
    return const_cast<JsonData*>(this)->GetChild(key);
}

JsonData& JsonData::operator[](const std::string& key) {
    if (type_ != JsonType::Object) {
        type_ = JsonType::Object;
    }
    for (size_t i = 0; i < objectKeys_.size(); i++) {
        if (objectKeys_[i] == key) return array_[i];
    }
    objectKeys_.push_back(key);
    array_.push_back(JsonData());
    return array_.back();
}

bool JsonData::ContainsKey(const std::string& key) const {
    if (type_ != JsonType::Object) return false;
    for (const auto& k : objectKeys_) {
        if (k == key) return true;
    }
    return false;
}

std::vector<std::string> JsonData::Keys() const { return objectKeys_; }

JsonData JsonData::NewObject() {
    JsonData d;
    d.type_ = JsonType::Object;
    return d;
}

JsonData JsonData::NewArray() {
    JsonData d;
    d.type_ = JsonType::Array;
    return d;
}

JsonData JsonData::NewString(const std::string& v) {
    JsonData d;
    d.type_ = JsonType::String;
    d.string_ = v;
    return d;
}

JsonData JsonData::NewBoolean(bool v) {
    JsonData d;
    d.type_ = JsonType::Boolean;
    d.boolean_ = v;
    return d;
}

JsonData JsonData::NewNumber(const Decimal& v) {
    JsonData d;
    d.type_ = JsonType::Double;
    d.number_ = v;
    return d;
}

JsonData JsonData::NewNull() { return JsonData(); }

void JsonData::Add(const JsonData& v) {
    if (type_ != JsonType::Array) type_ = JsonType::Array;
    array_.push_back(v);
}

void JsonData::Set(const std::string& key, const JsonData& v) {
    if (type_ != JsonType::Object) type_ = JsonType::Object;
    for (size_t i = 0; i < objectKeys_.size(); i++) {
        if (objectKeys_[i] == key) {
            array_[i] = v;
            return;
        }
    }
    objectKeys_.push_back(key);
    array_.push_back(v);
}

std::string JsonData::EscapeJson(const std::string& s) {
    std::string sb;
    sb.reserve(s.size() + 8);
    char buf[8];
    for (char c : s) {
        switch (c) {
            case '"': sb += "\\\""; break;
            case '\\': sb += "\\\\"; break;
            case '\n': sb += "\\n"; break;
            case '\r': sb += "\\r"; break;
            case '\t': sb += "\\t"; break;
            case '\b': sb += "\\b"; break;
            case '\f': sb += "\\f"; break;
            default:
                if (static_cast<unsigned char>(c) < 0x20) {
                    std::snprintf(buf, sizeof(buf), "\\u%04x", static_cast<unsigned char>(c));
                    sb += buf;
                } else {
                    sb += c;
                }
                break;
        }
    }
    return sb;
}

void JsonData::WriteTo(std::string& sb) const {
    switch (type_) {
        case JsonType::None:
        case JsonType::Null:
            sb += "null";
            break;
        case JsonType::Boolean:
            sb += boolean_ ? "true" : "false";
            break;
        case JsonType::Double:
            sb += number_.PlainStrip();
            break;
        case JsonType::String:
            sb += '"';
            sb += EscapeJson(string_);
            sb += '"';
            break;
        case JsonType::Array:
            sb += '[';
            for (size_t i = 0; i < array_.size(); i++) {
                if (i > 0) sb += ',';
                array_[i].WriteTo(sb);
            }
            sb += ']';
            break;
        case JsonType::Object:
            sb += '{';
            for (size_t i = 0; i < array_.size(); i++) {
                if (i > 0) sb += ',';
                sb += '"';
                sb += EscapeJson(objectKeys_[i]);
                sb += "\":";
                array_[i].WriteTo(sb);
            }
            sb += '}';
            break;
    }
}

std::string JsonData::ToString() const {
    std::string sb;
    WriteTo(sb);
    return sb;
}

namespace {

class JsonParser {
public:
    explicit JsonParser(const std::string& text) : s_(text) {}

    bool Parse(JsonData& out) {
        SkipWs();
        if (!ParseValue(out)) return false;
        SkipWs();
        return i_ >= s_.size();
    }

private:
    const std::string& s_;
    size_t i_ = 0;

    void SkipWs() {
        while (i_ < s_.size()) {
            char c = s_[i_];
            if (c == ' ' || c == '\t' || c == '\r' || c == '\n') i_++;
            else break;
        }
    }

    bool Expect(char c) {
        SkipWs();
        if (i_ < s_.size() && s_[i_] == c) {
            i_++;
            return true;
        }
        return false;
    }

    bool ParseValue(JsonData& out) {
        SkipWs();
        if (i_ >= s_.size()) return false;
        char c = s_[i_];
        if (c == '{') return ParseObject(out);
        if (c == '[') return ParseArray(out);
        if (c == '"') {
            std::string v;
            if (!ParseString(v)) return false;
            out = JsonData::NewString(v);
            return true;
        }
        if (c == 't') {
            if (s_.compare(i_, 4, "true") != 0) return false;
            i_ += 4;
            out = JsonData::NewBoolean(true);
            return true;
        }
        if (c == 'f') {
            if (s_.compare(i_, 5, "false") != 0) return false;
            i_ += 5;
            out = JsonData::NewBoolean(false);
            return true;
        }
        if (c == 'n') {
            if (s_.compare(i_, 4, "null") != 0) return false;
            i_ += 4;
            out = JsonData::NewNull();
            return true;
        }
        return ParseNumber(out);
    }

    bool ParseNumber(JsonData& out) {
        size_t start = i_;
        if (i_ < s_.size() && (s_[i_] == '-' || s_[i_] == '+')) i_++;
        bool any = false;
        while (i_ < s_.size() && s_[i_] >= '0' && s_[i_] <= '9') {
            i_++;
            any = true;
        }
        if (i_ < s_.size() && s_[i_] == '.') {
            i_++;
            while (i_ < s_.size() && s_[i_] >= '0' && s_[i_] <= '9') {
                i_++;
                any = true;
            }
        }
        if (!any) return false;
        if (i_ < s_.size() && (s_[i_] == 'e' || s_[i_] == 'E')) {
            size_t save = i_;
            i_++;
            if (i_ < s_.size() && (s_[i_] == '-' || s_[i_] == '+')) i_++;
            bool expAny = false;
            while (i_ < s_.size() && s_[i_] >= '0' && s_[i_] <= '9') {
                i_++;
                expAny = true;
            }
            if (!expAny) i_ = save;
        }
        Decimal d;
        if (!Decimal::TryParse(s_.substr(start, i_ - start), d)) return false;
        out = JsonData::NewNumber(d);
        return true;
    }

    static void AppendUtf8(std::string& out, unsigned cp) {
        if (cp < 0x80) {
            out += static_cast<char>(cp);
        } else if (cp < 0x800) {
            out += static_cast<char>(0xC0 | (cp >> 6));
            out += static_cast<char>(0x80 | (cp & 0x3F));
        } else if (cp < 0x10000) {
            out += static_cast<char>(0xE0 | (cp >> 12));
            out += static_cast<char>(0x80 | ((cp >> 6) & 0x3F));
            out += static_cast<char>(0x80 | (cp & 0x3F));
        } else {
            out += static_cast<char>(0xF0 | (cp >> 18));
            out += static_cast<char>(0x80 | ((cp >> 12) & 0x3F));
            out += static_cast<char>(0x80 | ((cp >> 6) & 0x3F));
            out += static_cast<char>(0x80 | (cp & 0x3F));
        }
    }

    bool ParseString(std::string& out) {
        if (!Expect('"')) return false;
        out.clear();
        while (i_ < s_.size()) {
            char c = s_[i_++];
            if (c == '"') return true;
            if (c == '\\') {
                if (i_ >= s_.size()) return false;
                char e = s_[i_++];
                switch (e) {
                    case '"': out += '"'; break;
                    case '\\': out += '\\'; break;
                    case '/': out += '/'; break;
                    case 'b': out += '\b'; break;
                    case 'f': out += '\f'; break;
                    case 'n': out += '\n'; break;
                    case 'r': out += '\r'; break;
                    case 't': out += '\t'; break;
                    case 'u': {
                        if (i_ + 4 > s_.size()) return false;
                        unsigned cp = 0;
                        for (int k = 0; k < 4; k++) {
                            char h = s_[i_ + k];
                            unsigned d;
                            if (h >= '0' && h <= '9') d = static_cast<unsigned>(h - '0');
                            else if (h >= 'a' && h <= 'f') d = static_cast<unsigned>(h - 'a' + 10);
                            else if (h >= 'A' && h <= 'F') d = static_cast<unsigned>(h - 'A' + 10);
                            else return false;
                            cp = cp * 16 + d;
                        }
                        i_ += 4;
                        // 处理代理对
                        if (cp >= 0xD800 && cp <= 0xDBFF && i_ + 6 <= s_.size() &&
                            s_[i_] == '\\' && s_[i_ + 1] == 'u') {
                            unsigned lo = 0;
                            bool ok = true;
                            for (int k = 0; k < 4; k++) {
                                char h = s_[i_ + 2 + k];
                                unsigned d;
                                if (h >= '0' && h <= '9') d = static_cast<unsigned>(h - '0');
                                else if (h >= 'a' && h <= 'f') d = static_cast<unsigned>(h - 'a' + 10);
                                else if (h >= 'A' && h <= 'F') d = static_cast<unsigned>(h - 'A' + 10);
                                else {
                                    ok = false;
                                    break;
                                }
                                lo = lo * 16 + d;
                            }
                            if (ok && lo >= 0xDC00 && lo <= 0xDFFF) {
                                i_ += 6;
                                cp = 0x10000 + ((cp - 0xD800) << 10) + (lo - 0xDC00);
                            }
                        }
                        AppendUtf8(out, cp);
                        break;
                    }
                    default: return false;
                }
            } else {
                out += c;
            }
        }
        return false;
    }

    bool ParseObject(JsonData& out) {
        if (!Expect('{')) return false;
        out = JsonData::NewObject();
        SkipWs();
        if (i_ < s_.size() && s_[i_] == '}') {
            i_++;
            return true;
        }
        for (;;) {
            std::string key;
            SkipWs();
            if (i_ < s_.size() && s_[i_] != '"') return false;
            if (!ParseString(key)) return false;
            if (!Expect(':')) return false;
            JsonData v;
            if (!ParseValue(v)) return false;
            out.Set(key, v);
            SkipWs();
            if (i_ < s_.size() && s_[i_] == ',') {
                i_++;
                continue;
            }
            if (i_ < s_.size() && s_[i_] == '}') {
                i_++;
                return true;
            }
            return false;
        }
    }

    bool ParseArray(JsonData& out) {
        if (!Expect('[')) return false;
        out = JsonData::NewArray();
        SkipWs();
        if (i_ < s_.size() && s_[i_] == ']') {
            i_++;
            return true;
        }
        for (;;) {
            JsonData v;
            if (!ParseValue(v)) return false;
            out.Add(v);
            SkipWs();
            if (i_ < s_.size() && s_[i_] == ',') {
                i_++;
                continue;
            }
            if (i_ < s_.size() && s_[i_] == ']') {
                i_++;
                return true;
            }
            return false;
        }
    }
};

}  // namespace

bool JsonMapper::TryParse(const std::string& text, JsonData& out) {
    JsonParser p(text);
    try {
        return p.Parse(out);
    } catch (...) {
        return false;
    }
}

JsonData JsonMapper::ToObject(const std::string& text) {
    JsonData d;
    if (!TryParse(text, d)) throw std::runtime_error("invalid json");
    return d;
}

}  // namespace toolgood
