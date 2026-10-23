// MathBase.h - 对应 csharp/.../Internals/Functions/MathBase/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_ABS(std::vector<Function> args);
Function NewFunction_CEILING(std::vector<Function> args);
Function NewFunction_COMBIN(std::vector<Function> args);
Function NewFunction_DELTA(std::vector<Function> args);
Function NewFunction_EVEN(std::vector<Function> args);
Function NewFunction_EXP(std::vector<Function> args);
Function NewFunction_FACT(std::vector<Function> args);
Function NewFunction_FACTDOUBLE(std::vector<Function> args);
Function NewFunction_FIXED(std::vector<Function> args);
Function NewFunction_FLOOR(std::vector<Function> args);
Function NewFunction_GCD(std::vector<Function> args);
Function NewFunction_GESTEP(std::vector<Function> args);
Function NewFunction_INT(std::vector<Function> args);
Function NewFunction_LCM(std::vector<Function> args);
Function NewFunction_LN(std::vector<Function> args);
Function NewFunction_LOG(std::vector<Function> args);
Function NewFunction_LOG10(std::vector<Function> args);
Function NewFunction_MROUND(std::vector<Function> args);
Function NewFunction_MULTINOMIAL(std::vector<Function> args);
Function NewFunction_ODD(std::vector<Function> args);
Function NewFunction_PERMUT(std::vector<Function> args);
Function NewFunction_POWER(std::vector<Function> args);
Function NewFunction_PRODUCT(std::vector<Function> args);
Function NewFunction_Percentage(std::vector<Function> args);
Function NewFunction_QUOTIENT(std::vector<Function> args);
Function NewFunction_RAND(std::vector<Function> args);
Function NewFunction_RANDBETWEEN(std::vector<Function> args);
Function NewFunction_ROUND(std::vector<Function> args);
Function NewFunction_ROUNDDOWN(std::vector<Function> args);
Function NewFunction_ROUNDUP(std::vector<Function> args);
Function NewFunction_SIGN(std::vector<Function> args);
Function NewFunction_SQRT(std::vector<Function> args);
Function NewFunction_SQRTPI(std::vector<Function> args);
Function NewFunction_TRUNC(std::vector<Function> args);

/// 注册 MathBase 组全部函数。
void Registermathbase();

}  // namespace toolgood
