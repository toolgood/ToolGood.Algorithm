// CsharpSecurity.h - 对应 csharp/.../Internals/Functions/CsharpSecurity/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_HMACMD5(std::vector<Function> args);
Function NewFunction_HMACSHA1(std::vector<Function> args);
Function NewFunction_HMACSHA256(std::vector<Function> args);
Function NewFunction_HMACSHA512(std::vector<Function> args);
Function NewFunction_MD5(std::vector<Function> args);
Function NewFunction_SHA1(std::vector<Function> args);
Function NewFunction_SHA256(std::vector<Function> args);
Function NewFunction_SHA512(std::vector<Function> args);

/// 注册 CsharpSecurity 组全部函数。
void Registercsharpsecurity();

}  // namespace toolgood
