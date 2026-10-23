// CsharpWeb.h - 对应 csharp/.../Internals/Functions/CsharpWeb/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_BASE64TOTEXT(std::vector<Function> args);
Function NewFunction_BASE64URLTOTEXT(std::vector<Function> args);
Function NewFunction_HTMLDECODE(std::vector<Function> args);
Function NewFunction_HTMLENCODE(std::vector<Function> args);
Function NewFunction_TEXTTOBASE64(std::vector<Function> args);
Function NewFunction_TEXTTOBASE64URL(std::vector<Function> args);
Function NewFunction_URLDECODE(std::vector<Function> args);
Function NewFunction_URLENCODE(std::vector<Function> args);

/// 注册 CsharpWeb 组全部函数。
void Registercsharpweb();

}  // namespace toolgood
