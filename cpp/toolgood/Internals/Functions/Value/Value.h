// Value.h - 对应 csharp/.../Internals/Functions/Value/
// 注意：本组函数签名不统一——数据型叶子节点直接携带数值/文本，
// 表达式型节点接收子 Function 列表。请按下列声明实现。
#pragma once

#include <string>
#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

// ---- 表达式型（由注册表按名称创建）----
Function NewFunction_ARRAY(std::vector<Function> args);
Function NewFunction_ArrayJson(std::vector<Function> args);
Function NewFunction_ERROR(std::vector<Function> args);
Function NewFunction_GetJsonValue(std::vector<Function> args);
Function NewFunction_JSON(std::vector<Function> args);
Function NewFunction_PARAM(std::vector<Function> args);

// ---- 数据型（由语法分析器直接构造）----
/// 键值项：key 为 JSON 键名。
Function NewFunction_ArrayJsonItem(const std::string& key, Function value);
/// 自定义函数调用：name 为原始函数名（大小写保留）。
Function NewFunction_DiyFunction(const std::string& name, std::vector<Function> args);
/// 数值常量。
Function NewFunction_Number(const Decimal& value);
/// 带单位的数值常量（如 5M / 3.5KG）。
Function NewFunction_Number2(const Decimal& value, const std::string& unit);
Function NewFunction_Number_E();
Function NewFunction_Number_PI();
/// 参数引用（对应 class Function_Parameter，注意与 NewFunction_PARAMETER 区分）。
Function NewFunction_Parameter(const std::string& name);
Function NewFunction_ValueBoolean(bool value);
Function NewFunction_ValueText(const std::string& text);
Function NewFunction_NULL();
Function NewFunction_Version();

}  // namespace toolgood
