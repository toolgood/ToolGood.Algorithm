// Registry.h - 函数注册表
#pragma once

#include <functional>
#include <string>
#include <vector>

#include "FunctionBase.h"

namespace toolgood {

using FunctionFactory = std::function<Function(const std::vector<Function>&)>;

/// 注册函数（同名重复注册以先注册为准）。
void RegisterFunction(const std::string& key, FunctionFactory f);
/// 按名称创建函数；未注册返回 nullptr。参数个数不符时抛 std::runtime_error。
Function GetFunction(const std::string& key, const std::vector<Function>& args);
/// 是否已注册该名称。
bool HasFunction(const std::string& key);
/// 注册全部内建函数（幂等）。
void RegisterAllFunctions();

}  // namespace toolgood
