// Financial.h - 对应 csharp/.../Internals/Functions/Financial/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_DB(std::vector<Function> args);
Function NewFunction_DDB(std::vector<Function> args);
Function NewFunction_FV(std::vector<Function> args);
Function NewFunction_IPMT(std::vector<Function> args);
Function NewFunction_IRR(std::vector<Function> args);
Function NewFunction_MIRR(std::vector<Function> args);
Function NewFunction_NPER(std::vector<Function> args);
Function NewFunction_NPV(std::vector<Function> args);
Function NewFunction_PMT(std::vector<Function> args);
Function NewFunction_PPMT(std::vector<Function> args);
Function NewFunction_PV(std::vector<Function> args);
Function NewFunction_RATE(std::vector<Function> args);
Function NewFunction_SLN(std::vector<Function> args);
Function NewFunction_SYD(std::vector<Function> args);
Function NewFunction_XIRR(std::vector<Function> args);
Function NewFunction_XNPV(std::vector<Function> args);

/// 注册 Financial 组全部函数。
void Registerfinancial();

}  // namespace toolgood
