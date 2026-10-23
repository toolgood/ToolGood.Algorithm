// MathTransformation.h - 对应 csharp/.../Internals/Functions/MathTransformation/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_ARABIC(std::vector<Function> args);
Function NewFunction_BIN2DEC(std::vector<Function> args);
Function NewFunction_BIN2HEX(std::vector<Function> args);
Function NewFunction_BIN2OCT(std::vector<Function> args);
Function NewFunction_DEC2BIN(std::vector<Function> args);
Function NewFunction_DEC2HEX(std::vector<Function> args);
Function NewFunction_DEC2OCT(std::vector<Function> args);
Function NewFunction_HEX2BIN(std::vector<Function> args);
Function NewFunction_HEX2DEC(std::vector<Function> args);
Function NewFunction_HEX2OCT(std::vector<Function> args);
Function NewFunction_OCT2BIN(std::vector<Function> args);
Function NewFunction_OCT2DEC(std::vector<Function> args);
Function NewFunction_OCT2HEX(std::vector<Function> args);
Function NewFunction_ROMAN(std::vector<Function> args);

/// 注册 MathTransformation 组全部函数。
void Registermathtransformation();

}  // namespace toolgood
