// MathTrigonometric.h - 对应 csharp/.../Internals/Functions/MathTrigonometric/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_ACOS(std::vector<Function> args);
Function NewFunction_ACOSH(std::vector<Function> args);
Function NewFunction_ACOT(std::vector<Function> args);
Function NewFunction_ACOTH(std::vector<Function> args);
Function NewFunction_ASIN(std::vector<Function> args);
Function NewFunction_ASINH(std::vector<Function> args);
Function NewFunction_ATAN(std::vector<Function> args);
Function NewFunction_ATAN2(std::vector<Function> args);
Function NewFunction_ATANH(std::vector<Function> args);
Function NewFunction_COS(std::vector<Function> args);
Function NewFunction_COSH(std::vector<Function> args);
Function NewFunction_COT(std::vector<Function> args);
Function NewFunction_COTH(std::vector<Function> args);
Function NewFunction_CSC(std::vector<Function> args);
Function NewFunction_CSCH(std::vector<Function> args);
Function NewFunction_DEGREES(std::vector<Function> args);
Function NewFunction_RADIANS(std::vector<Function> args);
Function NewFunction_SEC(std::vector<Function> args);
Function NewFunction_SECH(std::vector<Function> args);
Function NewFunction_SIN(std::vector<Function> args);
Function NewFunction_SINH(std::vector<Function> args);
Function NewFunction_TAN(std::vector<Function> args);
Function NewFunction_TANH(std::vector<Function> args);

/// 注册 MathTrigonometric 组全部函数。
void Registermathtrigonometric();

}  // namespace toolgood
