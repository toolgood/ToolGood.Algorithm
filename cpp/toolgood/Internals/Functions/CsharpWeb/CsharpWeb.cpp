// CsharpWeb.cpp - 对应 csharp/.../Internals/Functions/CsharpWeb/（8 个 Web 编解码函数）
// 说明：C++ 无 System.Web.HttpUtility / Convert 等价物，这里按 .NET 的实际行为自行实现：
//   - UrlEncode 对应 HttpUtility.UrlEncode：空格转 '+'，A-Za-z0-9 与 "-_.!*()" 不编码，
//     其余字节按 UTF-8 转 %xx（十六进制小写）；UrlDecode 反之，非法百分号序列原样保留。
//   - HtmlEncode 对应 HttpUtility.HtmlEncode：'&' '<' '>' '"' 与 '\'' 转实体，
//     U+00A0..U+00FF 转数字实体，其余字符（含中文）原样输出；HtmlDecode 支持基本实体、
//     数字实体与 Latin-1 命名实体，未知实体原样保留。
//   - Base64 编码为 UTF-8 字节 + 标准表；URL 安全变体替换 '+' '/' 并去掉 '=' 填充。
//   文本一律按 UTF-8 字节处理，编解码全程无第三方依赖。
#include "CsharpWeb.h"

#include <cstddef>
#include <cstdint>
#include <string>
#include <utility>
#include <vector>

#include "../../../AlgorithmEngine.h"
#include "../../../Lexer.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

using Bytes = std::vector<uint8_t>;

// ---------------------------------------------------------------------------
// 通用工具
// ---------------------------------------------------------------------------

/// std::string（UTF-8 字节）转字节数组
Bytes ToBytes(const std::string& text) { return Bytes(text.begin(), text.end()); }

/// 追加一个码点的 UTF-8 编码
void AppendUtf8(std::string& sb, uint32_t cp) {
    if (cp < 0x80) {
        sb.push_back(static_cast<char>(cp));
    } else if (cp < 0x800) {
        sb.push_back(static_cast<char>(0xC0 | (cp >> 6)));
        sb.push_back(static_cast<char>(0x80 | (cp & 0x3F)));
    } else if (cp < 0x10000) {
        sb.push_back(static_cast<char>(0xE0 | (cp >> 12)));
        sb.push_back(static_cast<char>(0x80 | ((cp >> 6) & 0x3F)));
        sb.push_back(static_cast<char>(0x80 | (cp & 0x3F)));
    } else {
        sb.push_back(static_cast<char>(0xF0 | (cp >> 18)));
        sb.push_back(static_cast<char>(0x80 | ((cp >> 12) & 0x3F)));
        sb.push_back(static_cast<char>(0x80 | ((cp >> 6) & 0x3F)));
        sb.push_back(static_cast<char>(0x80 | (cp & 0x3F)));
    }
}

/// 十六进制字符转数值，非法返回 -1
int HexValue(char c) {
    if (c >= '0' && c <= '9') return c - '0';
    if (c >= 'a' && c <= 'f') return c - 'a' + 10;
    if (c >= 'A' && c <= 'F') return c - 'A' + 10;
    return -1;
}

/// 对应 Encoding.UTF8.GetString 的替换回退：非法 UTF-8 序列替换为 U+FFFD。
/// 逐字节前进（与 .NET 的替换行为一致），合法序列原样保留。
std::string SanitizeUtf8(const std::string& text) {
    static const char kReplacement[] = "\xEF\xBF\xBD";
    std::string sb;
    sb.reserve(text.size());
    const size_t n = text.size();
    size_t i = 0;
    while (i < n) {
        const unsigned char b = static_cast<unsigned char>(text[i]);
        if (b < 0x80) {
            sb.push_back(static_cast<char>(b));
            i++;
            continue;
        }
        size_t len = 0;
        uint32_t cp = 0;
        if ((b & 0xE0) == 0xC0) {
            len = 2;
            cp = b & 0x1F;
        } else if ((b & 0xF0) == 0xE0) {
            len = 3;
            cp = b & 0x0F;
        } else if ((b & 0xF8) == 0xF0) {
            len = 4;
            cp = b & 0x07;
        } else {
            sb += kReplacement;
            i++;
            continue;
        }
        if (i + len > n) {
            sb += kReplacement;
            i++;
            continue;
        }
        bool ok = true;
        for (size_t k = 1; k < len; k++) {
            const unsigned char c = static_cast<unsigned char>(text[i + k]);
            if ((c & 0xC0) != 0x80) {
                ok = false;
                break;
            }
            cp = (cp << 6) | (c & 0x3F);
        }
        // 过长编码 / 代理区 / 超出 Unicode 范围
        const bool overlong = (len == 2 && cp < 0x80) || (len == 3 && cp < 0x800) ||
                              (len == 4 && cp < 0x10000);
        if (!ok || overlong || cp > 0x10FFFF || (cp >= 0xD800 && cp <= 0xDFFF)) {
            sb += kReplacement;
            i++;
            continue;
        }
        sb.append(text, i, len);
        i += len;
    }
    return sb;
}

// ---------------------------------------------------------------------------
// URL 编解码（对应 HttpUtility.UrlEncode / UrlDecode）
// ---------------------------------------------------------------------------

std::string UrlEncodeText(const std::string& text) {
    static const char kHex[] = "0123456789abcdef";
    std::string sb;
    sb.reserve(text.size() * 3);
    for (char ch : text) {
        const unsigned char c = static_cast<unsigned char>(ch);
        if (c == ' ') {
            sb.push_back('+');
        } else if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') ||
                   c == '-' || c == '_' || c == '.' || c == '!' || c == '*' || c == '(' || c == ')') {
            sb.push_back(static_cast<char>(c));
        } else {
            sb.push_back('%');
            sb.push_back(kHex[c >> 4]);
            sb.push_back(kHex[c & 0x0F]);
        }
    }
    return sb;
}

std::string UrlDecodeText(const std::string& text) {
    if (text.find_first_of("+%") == std::string::npos) return text;
    std::string sb;
    sb.reserve(text.size());
    const size_t n = text.size();
    for (size_t i = 0; i < n; i++) {
        const char c = text[i];
        if (c == '+') {
            sb.push_back(' ');
        } else if (c == '%' && i + 2 < n) {
            const int hi = HexValue(text[i + 1]);
            const int lo = HexValue(text[i + 2]);
            if (hi >= 0 && lo >= 0) {
                sb.push_back(static_cast<char>((hi << 4) | lo));
                i += 2;
            } else {
                sb.push_back(c);
            }
        } else {
            sb.push_back(c);
        }
    }
    return SanitizeUtf8(sb);
}

// ---------------------------------------------------------------------------
// HTML 编解码（对应 HttpUtility.HtmlEncode / HtmlDecode）
// ---------------------------------------------------------------------------

std::string HtmlEncodeText(const std::string& text) {
    std::string sb;
    sb.reserve(text.size());
    // C# 的 string 一定是合法 UTF-16，这里的 UTF-8 输入先做替换回退保证语义一致
    for (uint32_t cp : Lexer::ToCodePoints(SanitizeUtf8(text))) {
        switch (cp) {
            case '&': sb += "&amp;"; break;
            case '<': sb += "&lt;"; break;
            case '>': sb += "&gt;"; break;
            case '"': sb += "&quot;"; break;
            case '\'': sb += "&#39;"; break;
            default:
                if (cp >= 160 && cp <= 255) {
                    sb += "&#";
                    sb += std::to_string(cp);
                    sb += ';';
                } else {
                    AppendUtf8(sb, cp);
                }
                break;
        }
    }
    return sb;
}

/// Latin-1 区（U+00A0..U+00FF）的命名实体表
uint32_t LookupLatin1Entity(const std::string& name) {
    static const struct {
        const char* name;
        uint32_t cp;
    } kTable[] = {
        {"nbsp", 160},   {"iexcl", 161},  {"cent", 162},   {"pound", 163},  {"curren", 164},
        {"yen", 165},    {"brvbar", 166}, {"sect", 167},   {"uml", 168},    {"copy", 169},
        {"ordf", 170},   {"laquo", 171},  {"not", 172},    {"shy", 173},    {"reg", 174},
        {"macr", 175},   {"deg", 176},    {"plusmn", 177}, {"sup2", 178},   {"sup3", 179},
        {"acute", 180},  {"micro", 181},  {"para", 182},   {"middot", 183}, {"cedil", 184},
        {"sup1", 185},   {"ordm", 186},   {"raquo", 187},  {"frac14", 188}, {"frac12", 189},
        {"frac34", 190}, {"iquest", 191}, {"Agrave", 192}, {"Aacute", 193}, {"Acirc", 194},
        {"Atilde", 195}, {"Auml", 196},   {"Aring", 197},  {"AElig", 198},  {"Ccedil", 199},
        {"Egrave", 200}, {"Eacute", 201}, {"Ecirc", 202},  {"Euml", 203},   {"Igrave", 204},
        {"Iacute", 205}, {"Icirc", 206},  {"Iuml", 207},   {"ETH", 208},    {"Ntilde", 209},
        {"Ograve", 210}, {"Oacute", 211}, {"Ocirc", 212},  {"Otilde", 213}, {"Ouml", 214},
        {"times", 215},  {"Oslash", 216}, {"Ugrave", 217}, {"Uacute", 218}, {"Ucirc", 219},
        {"Uuml", 220},   {"Yacute", 221}, {"THORN", 222},  {"szlig", 223},  {"agrave", 224},
        {"aacute", 225}, {"acirc", 226},  {"atilde", 227}, {"auml", 228},   {"aring", 229},
        {"aelig", 230},  {"ccedil", 231}, {"egrave", 232}, {"eacute", 233}, {"ecirc", 234},
        {"euml", 235},   {"igrave", 236}, {"iacute", 237}, {"icirc", 238},  {"iuml", 239},
        {"eth", 240},    {"ntilde", 241}, {"ograve", 242}, {"oacute", 243}, {"ocirc", 244},
        {"otilde", 245}, {"ouml", 246},   {"divide", 247}, {"oslash", 248}, {"ugrave", 249},
        {"uacute", 250}, {"ucirc", 251},  {"uuml", 252},   {"yacute", 253}, {"thorn", 254},
        {"yuml", 255}};
    for (const auto& item : kTable) {
        if (name == item.name) return item.cp;
    }
    return 0;
}

/// 解析实体名（不含 '&' 与 ';'），失败返回 false
bool DecodeEntity(const std::string& entity, uint32_t& cp) {
    if (entity.empty()) return false;
    if (entity == "amp") {
        cp = '&';
        return true;
    }
    if (entity == "lt") {
        cp = '<';
        return true;
    }
    if (entity == "gt") {
        cp = '>';
        return true;
    }
    if (entity == "quot") {
        cp = '"';
        return true;
    }
    if (entity == "apos") {
        cp = '\'';
        return true;
    }
    if (entity[0] == '#') {
        std::string body = entity.substr(1);
        int base = 10;
        if (!body.empty() && (body[0] == 'x' || body[0] == 'X')) {
            base = 16;
            body = body.substr(1);
        }
        if (body.empty()) return false;
        uint64_t value = 0;
        for (char ch : body) {
            const int v = HexValue(ch);
            if (v < 0 || (base == 10 && v > 9)) return false;
            value = value * static_cast<uint64_t>(base) + static_cast<uint64_t>(v);
            if (value > 0x10FFFF) return false;
        }
        cp = static_cast<uint32_t>(value);
        return true;
    }
    const uint32_t latin1 = LookupLatin1Entity(entity);
    if (latin1 != 0) {
        cp = latin1;
        return true;
    }
    return false;
}

std::string HtmlDecodeText(const std::string& text) {
    if (text.find('&') == std::string::npos) return text;
    std::string sb;
    sb.reserve(text.size());
    const size_t n = text.size();
    for (size_t i = 0; i < n;) {
        const char c = text[i];
        if (c != '&') {
            sb.push_back(c);
            i++;
            continue;
        }
        const size_t end = text.find(';', i);
        if (end == std::string::npos) {
            sb.push_back(c);
            i++;
            continue;
        }
        uint32_t cp = 0;
        if (!DecodeEntity(text.substr(i + 1, end - i - 1), cp)) {
            sb.push_back(c);
            i++;
            continue;
        }
        AppendUtf8(sb, cp);
        i = end + 1;
    }
    return sb;
}

// ---------------------------------------------------------------------------
// Base64 编解码（对应 Convert.ToBase64String / FromBase64String）
// ---------------------------------------------------------------------------

const char* Base64Table() {
    return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
}

/// Base64 字符转数值；'=' 返回 -2，非法字符返回 -1
int Base64Value(char c) {
    if (c >= 'A' && c <= 'Z') return c - 'A';
    if (c >= 'a' && c <= 'z') return c - 'a' + 26;
    if (c >= '0' && c <= '9') return c - '0' + 52;
    if (c == '+') return 62;
    if (c == '/') return 63;
    if (c == '=') return -2;
    return -1;
}

std::string Base64Encode(const Bytes& data) {
    const char* table = Base64Table();
    std::string sb;
    sb.reserve(((data.size() + 2) / 3) * 4);
    size_t i = 0;
    for (; i + 2 < data.size(); i += 3) {
        const uint32_t v = (static_cast<uint32_t>(data[i]) << 16) |
                           (static_cast<uint32_t>(data[i + 1]) << 8) |
                           static_cast<uint32_t>(data[i + 2]);
        sb.push_back(table[(v >> 18) & 0x3F]);
        sb.push_back(table[(v >> 12) & 0x3F]);
        sb.push_back(table[(v >> 6) & 0x3F]);
        sb.push_back(table[v & 0x3F]);
    }
    const size_t remain = data.size() - i;
    if (remain == 1) {
        const uint32_t v = static_cast<uint32_t>(data[i]) << 16;
        sb.push_back(table[(v >> 18) & 0x3F]);
        sb.push_back(table[(v >> 12) & 0x3F]);
        sb += "==";
    } else if (remain == 2) {
        const uint32_t v = (static_cast<uint32_t>(data[i]) << 16) |
                           (static_cast<uint32_t>(data[i + 1]) << 8);
        sb.push_back(table[(v >> 18) & 0x3F]);
        sb.push_back(table[(v >> 12) & 0x3F]);
        sb.push_back(table[(v >> 6) & 0x3F]);
        sb += '=';
    }
    return sb;
}

/// 严格解码：忽略空白，长度须为 4 的倍数，'=' 只能出现在末尾且最多两个
bool Base64Decode(const std::string& text, Bytes& out) {
    std::string s;
    s.reserve(text.size());
    for (char c : text) {
        if (c == ' ' || c == '\t' || c == '\r' || c == '\n') continue;
        s.push_back(c);
    }
    out.clear();
    if (s.empty()) return true;
    if (s.size() % 4 != 0) return false;

    size_t pad = 0;
    while (pad < 2 && s[s.size() - 1 - pad] == '=') pad++;
    const size_t dataLen = s.size() - pad;
    // '=' 只能出现在末尾
    for (size_t i = 0; i < dataLen; i++) {
        if (s[i] == '=') return false;
    }
    if (dataLen % 4 == 1) return false;

    out.reserve(dataLen * 3 / 4);
    uint32_t acc = 0;
    int bits = 0;
    for (size_t i = 0; i < dataLen; i++) {
        const int v = Base64Value(s[i]);
        if (v < 0) return false;
        acc = (acc << 6) | static_cast<uint32_t>(v);
        bits += 6;
        if (bits >= 8) {
            bits -= 8;
            out.push_back(static_cast<uint8_t>((acc >> bits) & 0xFF));
        }
    }
    return true;
}

/// URL 安全变体：'+' -> '-'，'/' -> '_'，去掉 '=' 填充
std::string ToBase64Url(const std::string& base64) {
    std::string sb;
    sb.reserve(base64.size());
    for (char c : base64) {
        if (c == '=') break;
        if (c == '+') {
            sb.push_back('-');
        } else if (c == '/') {
            sb.push_back('_');
        } else {
            sb.push_back(c);
        }
    }
    return sb;
}

/// URL 安全变体还原为标准 Base64（含补齐 '='）
std::string FromBase64Url(const std::string& base64Url) {
    std::string s = base64Url;
    for (char& c : s) {
        if (c == '-') {
            c = '+';
        } else if (c == '_') {
            c = '/';
        }
    }
    const size_t padding = 4 - (s.size() % 4);
    if (padding < 4) s.append(padding, '=');
    return s;
}

// ---------------------------------------------------------------------------
// 函数实现
// ---------------------------------------------------------------------------

class Function_URLENCODE : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_URLENCODE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "UrlEncode", 1, 1);
    }

    std::string Name() const override { return "UrlEncode"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(UrlEncodeText(args1.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_URLDECODE : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_URLDECODE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "UrlDecode", 1, 1);
    }

    std::string Name() const override { return "UrlDecode"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(UrlDecodeText(args1.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_HTMLENCODE : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_HTMLENCODE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HtmlEncode", 1, 1);
    }

    std::string Name() const override { return "HtmlEncode"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(HtmlEncodeText(args1.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_HTMLDECODE : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_HTMLDECODE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HtmlDecode", 1, 1);
    }

    std::string Name() const override { return "HtmlDecode"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(HtmlDecodeText(args1.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_TEXTTOBASE64 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_TEXTTOBASE64(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "TextToBase64", 1, 1);
    }

    std::string Name() const override { return "TextToBase64"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(Base64Encode(ToBytes(args1.TextValue())));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_TEXTTOBASE64URL : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_TEXTTOBASE64URL(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "TextToBase64Url", 1, 1);
    }

    std::string Name() const override { return "TextToBase64Url"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(ToBase64Url(Base64Encode(ToBytes(args1.TextValue()))));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_BASE64TOTEXT : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_BASE64TOTEXT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Base64ToText", 1, 1);
    }

    std::string Name() const override { return "Base64ToText"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Bytes bytes;
        if (!Base64Decode(args1.TextValue(), bytes)) return ParameterError(1);
        return Operand::Create(SanitizeUtf8(std::string(bytes.begin(), bytes.end())));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_BASE64URLTOTEXT : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_BASE64URLTOTEXT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Base64UrlToText", 1, 1);
    }

    std::string Name() const override { return "Base64UrlToText"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Bytes bytes;
        if (!Base64Decode(FromBase64Url(args1.TextValue()), bytes)) return ParameterError(1);
        return Operand::Create(SanitizeUtf8(std::string(bytes.begin(), bytes.end())));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

}  // namespace

Function NewFunction_URLENCODE(std::vector<Function> args) {
    return std::make_shared<Function_URLENCODE>(std::move(args));
}

Function NewFunction_URLDECODE(std::vector<Function> args) {
    return std::make_shared<Function_URLDECODE>(std::move(args));
}

Function NewFunction_HTMLENCODE(std::vector<Function> args) {
    return std::make_shared<Function_HTMLENCODE>(std::move(args));
}

Function NewFunction_HTMLDECODE(std::vector<Function> args) {
    return std::make_shared<Function_HTMLDECODE>(std::move(args));
}

Function NewFunction_TEXTTOBASE64(std::vector<Function> args) {
    return std::make_shared<Function_TEXTTOBASE64>(std::move(args));
}

Function NewFunction_TEXTTOBASE64URL(std::vector<Function> args) {
    return std::make_shared<Function_TEXTTOBASE64URL>(std::move(args));
}

Function NewFunction_BASE64TOTEXT(std::vector<Function> args) {
    return std::make_shared<Function_BASE64TOTEXT>(std::move(args));
}

Function NewFunction_BASE64URLTOTEXT(std::vector<Function> args) {
    return std::make_shared<Function_BASE64URLTOTEXT>(std::move(args));
}

}  // namespace toolgood
