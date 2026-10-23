// Parser.h - 递归下降语法分析（替代 C# 的 ANTLR mathParser + MathFunctionVisitor）
// 直接生成可执行的 Function 树。
#pragma once

#include <string>
#include <vector>

#include "Internals/FunctionBase.h"
#include "Lexer.h"

namespace toolgood {

/// 表达式解析器。错误通过 std::runtime_error 抛出。
class Parser {
public:
    explicit Parser(const std::string& text);

    /// 解析整个表达式。
    Function Parse();

    /// 便捷入口。
    static Function ParseFunction(const std::string& text);

private:
    std::string text_;
    std::vector<Token> tokens_;
    size_t pos_ = 0;

    const Token& Current() const { return tokens_[pos_]; }
    const Token& Peek(size_t offset) const;
    Token Advance();
    Token Expect(const std::string& kind);

    Function ParseExpr(int minPrecedence);
    Function ParsePrefix();
    Function ParsePrimary();
    Function ParsePostfix(Function node);
    std::vector<Function> ParseArguments();
    std::pair<std::string, Function> ParseArrayJsonItem();

    bool StartsOperand(const Token& t) const;
    Function MakeNumber(const Token& numToken, bool negated);
    Function MakeCall(const std::string& name, const std::string& originalName,
                      std::vector<Function> args);
};

}  // namespace toolgood
