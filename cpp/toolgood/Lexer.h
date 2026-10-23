// Lexer.h - 表达式词法分析（替代 C# 的 ANTLR mathLexer）
// 与 C# 一致：先做字符标准化（小写转大写、全角转半角、中文转 '_'），
// 但 token 文本保留原文。
#pragma once

#include <cstdint>
#include <string>
#include <vector>

namespace toolgood {

/// 词法单元。
struct Token {
    std::string kind;   // NUM / STRING / IDENT / EOF / 运算符 token 名
    std::string text;   // 原文（UTF-8）
    int start = 0;      // 码点下标
    int end = 0;        // 码点下标（含）
};

/// 词法分析器。
class Lexer {
public:
    explicit Lexer(const std::string& text);

    /// 执行分析；语法错误抛 std::runtime_error。
    std::vector<Token> Tokenize();

    /// 字符标准化（对应 CharUtil.StandardChar）。
    static uint32_t StandardChar(uint32_t c);
    /// UTF-8 解码
    static std::vector<uint32_t> ToCodePoints(const std::string& text);
    /// UTF-8 编码
    static std::string FromCodePoints(const std::vector<uint32_t>& cps, size_t from, size_t to);

    const std::vector<uint32_t>& CodePoints() const { return orig_; }

private:
    std::vector<uint32_t> orig_;
    std::vector<uint32_t> std_;
    size_t pos_ = 0;
    std::vector<Token> tokens_;

    void ReadNumber();
    void ReadString(uint32_t quote);
    void ReadIdent();
    void ReadOperator();
    std::string MatchNumSuffix(size_t p) const;
};

/// 便捷函数
std::vector<Token> Tokenize(const std::string& text);

}  // namespace toolgood
