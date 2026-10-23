// Operator.h - 对应 csharp/.../Internals/Functions/Operator/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_AND(std::vector<Function> args);
Function NewFunction_AND_N(std::vector<Function> args);
Function NewFunction_Add(std::vector<Function> args);
Function NewFunction_Connect(std::vector<Function> args);
Function NewFunction_Div(std::vector<Function> args);
Function NewFunction_Mod(std::vector<Function> args);
Function NewFunction_Mul(std::vector<Function> args);
Function NewFunction_OR(std::vector<Function> args);
Function NewFunction_OR_N(std::vector<Function> args);
Function NewFunction_Sub(std::vector<Function> args);
Function NewFunction_XOR(std::vector<Function> args);

/// 注册 Operator 组全部函数。
void Registerop();

}  // namespace toolgood
