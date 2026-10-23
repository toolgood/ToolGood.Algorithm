// String.h - 对应 csharp/.../Internals/Functions/String/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_ASC(std::vector<Function> args);
Function NewFunction_CHAR(std::vector<Function> args);
Function NewFunction_CLEAN(std::vector<Function> args);
Function NewFunction_CODE(std::vector<Function> args);
Function NewFunction_CONCATENATE(std::vector<Function> args);
Function NewFunction_EXACT(std::vector<Function> args);
Function NewFunction_FIND(std::vector<Function> args);
Function NewFunction_JIS(std::vector<Function> args);
Function NewFunction_LEFT(std::vector<Function> args);
Function NewFunction_LEN(std::vector<Function> args);
Function NewFunction_LOWER(std::vector<Function> args);
Function NewFunction_MID(std::vector<Function> args);
Function NewFunction_PROPER(std::vector<Function> args);
Function NewFunction_REPLACE(std::vector<Function> args);
Function NewFunction_REPT(std::vector<Function> args);
Function NewFunction_RIGHT(std::vector<Function> args);
Function NewFunction_RMB(std::vector<Function> args);
Function NewFunction_SEARCH(std::vector<Function> args);
Function NewFunction_SUBSTITUTE(std::vector<Function> args);
Function NewFunction_T(std::vector<Function> args);
Function NewFunction_TEXT(std::vector<Function> args);
Function NewFunction_TRIM(std::vector<Function> args);
Function NewFunction_UNICHAR(std::vector<Function> args);
Function NewFunction_UNICODE(std::vector<Function> args);
Function NewFunction_UPPER(std::vector<Function> args);
Function NewFunction_VALUE(std::vector<Function> args);

/// 注册 String 组全部函数。
void Registerstr();

}  // namespace toolgood
