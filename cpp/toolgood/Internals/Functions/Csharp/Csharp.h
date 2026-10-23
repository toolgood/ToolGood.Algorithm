// Csharp.h - 对应 csharp/.../Internals/Functions/Csharp/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_ENDSWITH(std::vector<Function> args);
Function NewFunction_GUID(std::vector<Function> args);
Function NewFunction_HAS(std::vector<Function> args);
Function NewFunction_HASVALUE(std::vector<Function> args);
Function NewFunction_INDEXOF(std::vector<Function> args);
Function NewFunction_ISREGEX(std::vector<Function> args);
Function NewFunction_JOIN(std::vector<Function> args);
Function NewFunction_LASTINDEXOF(std::vector<Function> args);
Function NewFunction_LOOKCEILING(std::vector<Function> args);
Function NewFunction_LOOKFLOOR(std::vector<Function> args);
Function NewFunction_REGEX(std::vector<Function> args);
Function NewFunction_REGEXREPLACE(std::vector<Function> args);
Function NewFunction_REMOVEEND(std::vector<Function> args);
Function NewFunction_REMOVESTART(std::vector<Function> args);
Function NewFunction_SPLIT(std::vector<Function> args);
Function NewFunction_STARTSWITH(std::vector<Function> args);
Function NewFunction_SUBSTRING(std::vector<Function> args);
Function NewFunction_TRIMEND(std::vector<Function> args);
Function NewFunction_TRIMSTART(std::vector<Function> args);

/// 注册 Csharp 组全部函数。
void Registercsharp();

}  // namespace toolgood
