// MathSum2.h - 对应 csharp/.../Internals/Functions/MathSum2/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_BESSELI(std::vector<Function> args);
Function NewFunction_BESSELJ(std::vector<Function> args);
Function NewFunction_BESSELK(std::vector<Function> args);
Function NewFunction_BESSELY(std::vector<Function> args);
Function NewFunction_BETADIST(std::vector<Function> args);
Function NewFunction_BETAINV(std::vector<Function> args);
Function NewFunction_BINOMDIST(std::vector<Function> args);
Function NewFunction_ERF(std::vector<Function> args);
Function NewFunction_ERFC(std::vector<Function> args);
Function NewFunction_EXPONDIST(std::vector<Function> args);
Function NewFunction_FDIST(std::vector<Function> args);
Function NewFunction_FINV(std::vector<Function> args);
Function NewFunction_FISHER(std::vector<Function> args);
Function NewFunction_FISHERINV(std::vector<Function> args);
Function NewFunction_GAMMADIST(std::vector<Function> args);
Function NewFunction_GAMMAINV(std::vector<Function> args);
Function NewFunction_GAMMALN(std::vector<Function> args);
Function NewFunction_HYPGEOMDIST(std::vector<Function> args);
Function NewFunction_LOGINV(std::vector<Function> args);
Function NewFunction_LOGNORMDIST(std::vector<Function> args);
Function NewFunction_NEGBINOMDIST(std::vector<Function> args);
Function NewFunction_NORMDIST(std::vector<Function> args);
Function NewFunction_NORMINV(std::vector<Function> args);
Function NewFunction_NORMSDIST(std::vector<Function> args);
Function NewFunction_NORMSINV(std::vector<Function> args);
Function NewFunction_TDIST(std::vector<Function> args);
Function NewFunction_TINV(std::vector<Function> args);
Function NewFunction_WEIBULL(std::vector<Function> args);

/// 注册 MathSum2 组全部函数。
void Registermathsum2();

}  // namespace toolgood
