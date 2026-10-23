// UnitConversion.h - 对应 csharp/ToolGood.Algorithm/UnitConversion/*.cs
// 四个换算器（距离/面积/体积/质量），换算表与 C# 一一对应，
// 字典查找不区分大小写（对应 StringComparer.OrdinalIgnoreCase）。
// 换算公式：right = left / 源单位系数 * 目标单位系数。
#pragma once

#include <string>

#include "../Decimal.h"

namespace toolgood {

/// 距离换算（对应 DistanceConverter）。
class DistanceConverter {
public:
    /// 换算成功返回 true 并写入 right；任一单位未知时 right = 0 且返回 false。
    static bool TryConvert(const std::string& leftSynonym, const std::string& rightSynonym,
                           const Decimal& left, Decimal& right);
};

/// 面积换算（对应 AreaConverter）。
class AreaConverter {
public:
    static bool TryConvert(const std::string& leftSynonym, const std::string& rightSynonym,
                           const Decimal& left, Decimal& right);
};

/// 体积换算（对应 VolumeConverter）。
class VolumeConverter {
public:
    static bool TryConvert(const std::string& leftSynonym, const std::string& rightSynonym,
                           const Decimal& left, Decimal& right);
};

/// 质量换算（对应 MassConverter）。
class MassConverter {
public:
    static bool TryConvert(const std::string& leftSynonym, const std::string& rightSynonym,
                           const Decimal& left, Decimal& right);
};

}  // namespace toolgood
