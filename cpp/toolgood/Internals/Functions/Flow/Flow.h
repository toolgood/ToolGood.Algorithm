// Flow.h - 对应 csharp/.../Internals/Functions/Flow/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_IF(std::vector<Function> args);
Function NewFunction_IFERROR(std::vector<Function> args);
Function NewFunction_IFS(std::vector<Function> args);
Function NewFunction_ISERROR(std::vector<Function> args);
Function NewFunction_ISEVEN(std::vector<Function> args);
Function NewFunction_ISLOGICAL(std::vector<Function> args);
Function NewFunction_ISNONTEXT(std::vector<Function> args);
Function NewFunction_ISNULL(std::vector<Function> args);
Function NewFunction_ISNULLOREMPTY(std::vector<Function> args);
Function NewFunction_ISNULLORERROR(std::vector<Function> args);
Function NewFunction_ISNULLORWHITESPACE(std::vector<Function> args);
Function NewFunction_ISNUMBER(std::vector<Function> args);
Function NewFunction_ISODD(std::vector<Function> args);
Function NewFunction_ISTEXT(std::vector<Function> args);
Function NewFunction_NOT(std::vector<Function> args);
Function NewFunction_SWITCH(std::vector<Function> args);

/// 注册 Flow 组全部函数。
void Registerflow();

}  // namespace toolgood
