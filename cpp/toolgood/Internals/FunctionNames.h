// FunctionNames.h - 函数别名表
// C# 的 ANTLR 词法把 BETA.DIST、COVARIANCE.S 等带点号写法识别为单个 token；
// 本移植用「标识符 + 别名归一」实现等价效果。
#pragma once

#include <string>

namespace toolgood {

/// 是否为「带点号的函数别名」（如 COVARIANCE.S、NORM.S.DIST）。
bool IsDottedAlias(const std::string& upperName);

/// 把别名归一为规范函数名（传入大写名；非别名原样返回）。
std::string CanonicalFunctionName(const std::string& upperName);

/// 规范名是否对应已注册的内建函数（含别名）。
/// 注意：这里只做名字判断，不做注册表查询，避免循环依赖。
bool IsKnownFunctionName(const std::string& upperName);

}  // namespace toolgood
