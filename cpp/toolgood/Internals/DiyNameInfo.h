// DiyNameInfo.h - 对应 csharp/ToolGood.Algorithm/Internals/DiyNameInfo.cs
// 与 Internals/DiyNameKeyInfo.cs（两个只含数据的结构体，故合并为一个头文件）。
#pragma once

#include <string>
#include <utility>
#include <vector>

namespace toolgood {

/// 关键字信息（对应 DiyNameKeyInfo）：名称 + 在原文中的码点区间。
struct DiyNameKeyInfo {
    std::string Name;
    int Start = 0;
    int End = -1;

    std::string ToString() const { return Name; }
};

/// 自定义类型（对应 DiyNameInfo）：自定义参数与自定义方法。
struct DiyNameInfo {
    std::vector<DiyNameKeyInfo> Parameters;
    std::vector<DiyNameKeyInfo> Functions;

    DiyNameInfo() = default;
    DiyNameInfo(std::vector<DiyNameKeyInfo> parameters, std::vector<DiyNameKeyInfo> functions)
        : Parameters(std::move(parameters)), Functions(std::move(functions)) {}
};

}  // namespace toolgood
