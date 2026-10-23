#include "Lexer.h"

#include "Internals/FunctionNames.h"

#include <stdexcept>

namespace toolgood {

namespace {

bool IsDigit(uint32_t c) { return c >= '0' && c <= '9'; }
bool IsIdentStart(uint32_t c) { return (c >= 'A' && c <= 'Z') || c == '_'; }
bool IsIdentPart(uint32_t c) { return (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_'; }

}  // namespace

std::vector<uint32_t> Lexer::ToCodePoints(const std::string& text) {
    std::vector<uint32_t> out;
    out.reserve(text.size());
    size_t i = 0;
    const size_t n = text.size();
    while (i < n) {
        unsigned char b = static_cast<unsigned char>(text[i]);
        uint32_t cp = 0;
        int len = 0;
        if (b < 0x80) {
            cp = b;
            len = 1;
        } else if ((b & 0xE0) == 0xC0) {
            cp = b & 0x1F;
            len = 2;
        } else if ((b & 0xF0) == 0xE0) {
            cp = b & 0x0F;
            len = 3;
        } else if ((b & 0xF8) == 0xF0) {
            cp = b & 0x07;
            len = 4;
        } else {
            cp = b;
            len = 1;
        }
        if (len > 1) {
            if (i + static_cast<size_t>(len) > n) {
                cp = b;
                len = 1;
            } else {
                for (int k = 1; k < len; k++) {
                    unsigned char cont = static_cast<unsigned char>(text[i + k]);
                    if ((cont & 0xC0) != 0x80) {
                        cp = b;
                        len = 1;
                        break;
                    }
                    cp = (cp << 6) | (cont & 0x3F);
                }
            }
        }
        out.push_back(cp);
        i += static_cast<size_t>(len);
    }
    return out;
}

std::string Lexer::FromCodePoints(const std::vector<uint32_t>& cps, size_t from, size_t to) {
    std::string out;
    out.reserve(to - from);
    for (size_t i = from; i < to; i++) {
        uint32_t cp = cps[i];
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
    return out;
}

uint32_t Lexer::StandardChar(uint32_t c) {
    if (c == '\r') return '\n';
    if (c == '\t' || c == '\f') return ' ';
    if (c < 'a') return c;
    if (c <= 'z') return c - 32;
    if (c > 65280 && c < 65375) {
        c = c - 65248;
        if (c >= 'a' && c <= 'z') c -= 32;
    }
    if ((c >= 0x00c0 && c <= 0x00d6) || (c >= 0x00d8 && c <= 0x00f6) ||
        (c >= 0x00f8 && c <= 0x00ff) || (c >= 0x0100 && c <= 0x1fff) ||
        (c >= 0x2c00 && c <= 0x2fff) || (c >= 0x3040 && c <= 0x318f) ||
        (c >= 0x3300 && c <= 0x337f) || (c >= 0x3400 && c <= 0x3fff) ||
        (c >= 0x4e00 && c <= 0x9fff) || (c >= 0xa000 && c <= 0xd7ff) ||
        (c >= 0xf900 && c <= 0xfaff) || (c >= 0xff00 && c <= 0xfff0)) {
        return '_';
    }
    if (c < 127) return c;
    switch (c) {
        case 215: return '*';
        case 247: return '/';
        case 8216:
        case 8217: return '\'';
        case 8220:
        case 8221: return '"';
        case 12288: return ' ';
        case 12304: return '[';
        case 12305: return ']';
        case 12308: return '(';
        case 12309: return ')';
        default: break;
    }
    if (c >= 'A' && c <= 'Z') return c;
    return c;
}

Lexer::Lexer(const std::string& text) : orig_(ToCodePoints(text)) {
    std_.resize(orig_.size());
    for (size_t i = 0; i < orig_.size(); i++) std_[i] = StandardChar(orig_[i]);
}

std::string Lexer::MatchNumSuffix(size_t p) const {
    const size_t n = std_.size();
    std::string best;
    auto consider = [&](size_t from, size_t to) {
        if (to > from && to - from > best.size()) best = FromCodePoints(std_, from, to);
    };
    // E[+-]?digits
    if (p < n && std_[p] == 'E') {
        size_t i = p + 1;
        if (i < n && (std_[i] == '+' || std_[i] == '-')) i++;
        size_t st = i;
        while (i < n && IsDigit(std_[i])) i++;
        if (i > st) consider(p, i);
    }
    // [KDCM]?M[23]?
    if (p < n) {
        size_t i = p;
        if ((std_[i] == 'K' || std_[i] == 'D' || std_[i] == 'C' || std_[i] == 'M') && i + 1 < n &&
            std_[i + 1] == 'M') {
            i += 2;
        } else if (std_[i] == 'M') {
            i++;
        }
        if (i > p) {
            if (i < n && (std_[i] == '2' || std_[i] == '3')) i++;
            consider(p, i);
        }
    }
    // M?L
    if (p < n) {
        if (std_[p] == 'M' && p + 1 < n && std_[p + 1] == 'L') {
            consider(p, p + 2);
        } else if (std_[p] == 'L') {
            consider(p, p + 1);
        }
    }
    // K?G
    if (p < n) {
        if (std_[p] == 'K' && p + 1 < n && std_[p + 1] == 'G') {
            consider(p, p + 2);
        } else if (std_[p] == 'G') {
            consider(p, p + 1);
        }
    }
    // T
    if (p < n && std_[p] == 'T') consider(p, p + 1);
    return best;
}

void Lexer::ReadNumber() {
    size_t i = pos_;
    const size_t n = std_.size();
    while (i < n && IsDigit(std_[i])) i++;
    if (i < n && std_[i] == '.' && i + 1 < n && IsDigit(std_[i + 1])) {
        i++;
        while (i < n && IsDigit(std_[i])) i++;
    }
    std::string suffix = MatchNumSuffix(i);
    i += ToCodePoints(suffix).size();
    Token t;
    t.kind = "NUM";
    t.text = FromCodePoints(orig_, pos_, i);
    t.start = static_cast<int>(pos_);
    t.end = static_cast<int>(i) - 1;
    tokens_.push_back(t);
    pos_ = i;
}

void Lexer::ReadString(uint32_t quote) {
    size_t i = pos_ + 1;
    const size_t n = std_.size();
    while (i < n) {
        if (std_[i] == '\\') {
            i += 2;
            continue;
        }
        if (std_[i] == quote) {
            i++;
            Token t;
            t.kind = "STRING";
            t.text = FromCodePoints(orig_, pos_, i);
            t.start = static_cast<int>(pos_);
            t.end = static_cast<int>(i) - 1;
            tokens_.push_back(t);
            pos_ = i;
            return;
        }
        i++;
    }
    throw std::runtime_error("token recognition error at: '" +
                             FromCodePoints(orig_, pos_, n) + "'");
}

void Lexer::ReadIdent() {
    size_t i = pos_;
    const size_t n = std_.size();
    while (i < n && IsIdentPart(std_[i])) i++;

    // 尝试吸收 BETA.DIST / COVARIANCE.S / NORM.S.DIST 形式的函数别名
    {
        size_t m1 = 0, m2 = 0;
        if (i < n && std_[i] == '.' && i + 1 < n && IsIdentStart(std_[i + 1])) {
            size_t k = i + 1;
            while (k < n && IsIdentPart(std_[k])) k++;
            m1 = k;
            if (k < n && std_[k] == '.' && k + 1 < n && IsIdentStart(std_[k + 1])) {
                size_t k2 = k + 1;
                while (k2 < n && IsIdentPart(std_[k2])) k2++;
                m2 = k2;
            }
        }
        if (m2 != 0 && IsDottedAlias(FromCodePoints(std_, pos_, m2))) {
            i = m2;
        } else if (m1 != 0 && IsDottedAlias(FromCodePoints(std_, pos_, m1))) {
            i = m1;
        }
    }
    Token t;
    t.kind = "IDENT";
    t.text = FromCodePoints(orig_, pos_, i);
    t.start = static_cast<int>(pos_);
    t.end = static_cast<int>(i) - 1;
    tokens_.push_back(t);
    pos_ = i;
}

void Lexer::ReadOperator() {
    const size_t i = pos_;
    const size_t n = std_.size();
    auto add = [&](const char* kind, size_t len) {
        Token t;
        t.kind = kind;
        t.text = FromCodePoints(orig_, i, i + len);
        t.start = static_cast<int>(i);
        t.end = static_cast<int>(i + len) - 1;
        tokens_.push_back(t);
        pos_ = i + len;
    };
    if (i + 1 < n) {
        uint32_t c0 = std_[i], c1 = std_[i + 1];
        if (c0 == '&' && c1 == '&') return add("OPAND", 2);
        if (c0 == '|' && c1 == '|') return add("OPOR", 2);
        if (c0 == '>' && c1 == '=') return add("OPGE", 2);
        if (c0 == '<' && c1 == '=') return add("OPLE", 2);
        if (c0 == '<' && c1 == '>') return add("OPNE", 2);
        if (c0 == '!' && c1 == '=') {
            if (i + 2 < n && std_[i + 2] == '=') return add("OPNE", 3);
            return add("OPNE", 2);
        }
        if (c0 == '=' && c1 == '=') {
            if (i + 2 < n && std_[i + 2] == '=') return add("OPEQ", 3);
            return add("OPEQ", 2);
        }
    }
    uint32_t c = std_[i];
    switch (c) {
        case '=': return add("OPEQ", 1);
        case '!': return add("NOT", 1);
        case '>': return add("OPGT", 1);
        case '<': return add("OPLT", 1);
        case '(': return add("LPAREN", 1);
        case ')': return add("RPAREN", 1);
        case '[': return add("LBRACKET", 1);
        case ']': return add("RBRACKET", 1);
        case '{': return add("LBRACE", 1);
        case '}': return add("RBRACE", 1);
        case ',': return add("COMMA", 1);
        case ':': return add("COLON", 1);
        case '?': return add("QUESTION", 1);
        case '.': return add("DOT", 1);
        case '+': return add("OPADD", 1);
        case '-': return add("OPSUB", 1);
        case '*': return add("OPMUL", 1);
        case '/': return add("OPDIV", 1);
        case '%': return add("OPMOD", 1);
        case '&': return add("OPCAT", 1);
        default: break;
    }
    std::string bad = FromCodePoints(orig_, i, i + 1);
    throw std::runtime_error("token recognition error at: '" + bad + "'");
}

std::vector<Token> Lexer::Tokenize() {
    const size_t n = std_.size();
    while (pos_ < n) {
        size_t i = pos_;
        uint32_t c = std_[i];
        if (c == ' ' || c == '\n') {
            pos_++;
        } else if (c == '/' && i + 1 < n && std_[i + 1] == '*') {
            size_t j = i + 2;
            bool closed = false;
            while (j + 1 < n) {
                if (std_[j] == '*' && std_[j + 1] == '/') {
                    j += 2;
                    closed = true;
                    break;
                }
                j++;
            }
            if (!closed) {
                throw std::runtime_error("token recognition error at: '" +
                                         FromCodePoints(orig_, i, n) + "'");
            }
            pos_ = j;
        } else if (c == '/' && i + 1 < n && std_[i + 1] == '/') {
            size_t j = i + 2;
            while (j < n && std_[j] != '\n') j++;
            pos_ = j;
        } else if (IsDigit(c)) {
            ReadNumber();
        } else if (c == '\'' || c == '"' || c == '`') {
            ReadString(c);
        } else if (IsIdentStart(c)) {
            ReadIdent();
        } else {
            ReadOperator();
        }
    }
    Token eof;
    eof.kind = "EOF";
    eof.start = static_cast<int>(n);
    eof.end = static_cast<int>(n);
    tokens_.push_back(eof);
    return tokens_;
}

std::vector<Token> Tokenize(const std::string& text) {
    Lexer lexer(text);
    return lexer.Tokenize();
}

}  // namespace toolgood
