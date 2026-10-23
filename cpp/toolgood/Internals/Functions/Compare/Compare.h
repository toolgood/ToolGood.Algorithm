// Compare.h - 对应 csharp/.../Internals/Functions/Compare/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_EQ(std::vector<Function> args);
Function NewFunction_GE(std::vector<Function> args);
Function NewFunction_GT(std::vector<Function> args);
Function NewFunction_LE(std::vector<Function> args);
Function NewFunction_LT(std::vector<Function> args);
Function NewFunction_NE(std::vector<Function> args);

/// 注册 Compare 组全部函数。
void Registercompare();

}  // namespace toolgood
