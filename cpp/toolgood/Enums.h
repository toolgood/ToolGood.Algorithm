// Enums.h - 对应 csharp/ToolGood.Algorithm/Enums 下的枚举
#pragma once

#include <cstdint>
#include <string>

namespace toolgood {

/// 操作数类型（对应 OperandType）
enum class OperandType : uint8_t {
    NONE = 0,
    NULL_VALUE = 1,
    ERROR = 2,
    DATE = 3,
    ARRAY = 4,
    NUMBER = 5,
    BOOLEAN = 6,
    TEXT = 7,
    JSON = 8,
    ARRAYJSON = 9,
};

/// JSON 节点类型（对应 LitJson.JsonType）
enum class JsonType : uint8_t {
    None = 0,
    Object = 1,
    Array = 2,
    String = 3,
    Double = 4,
    Boolean = 5,
    Null = 6,
};

/// 参数类型（对应 Internals.ParameterType）
struct ParameterType {
    std::string Name;
    OperandType Type = OperandType::NONE;
    std::string Operator;
    std::string Value;
};

/// 长度单位
enum class DistanceUnitType : uint8_t {
    MM = 1,
    CM = 2,
    DM = 3,
    M = 4,
    KM = 5,
};

/// 面积单位
enum class AreaUnitType : uint8_t {
    MM2 = 11,
    CM2 = 12,
    DM2 = 13,
    M2 = 14,
    KM2 = 15,
};

/// 体积单位
enum class VolumeUnitType : uint8_t {
    MM3 = 21,
    CM3 = 22,
    DM3 = 23,
    M3 = 24,
    KM3 = 25,
};

/// 重量单位
enum class MassUnitType : uint8_t {
    G = 31,
    KG = 32,
    T = 33,
};

/// 数字单位（内部使用，覆盖长度/面积/体积/重量）
enum class NumberUnitType : uint8_t {
    NONE = 0,
    MM = 1,
    CM = 2,
    DM = 3,
    M = 4,
    KM = 5,
    MM2 = 11,
    CM2 = 12,
    DM2 = 13,
    M2 = 14,
    KM2 = 15,
    MM3 = 21,
    CM3 = 22,
    DM3 = 23,
    M3 = 24,
    KM3 = 25,
    G = 31,
    KG = 32,
    T = 33,
};

/// 计算树类型（对应 CalculateTreeType）
enum class CalculateTreeType : uint8_t {
    String = 0,
    Add = 1,
    Sub = 2,
    Mul = 3,
    Div = 4,
    Mod = 5,
    Connect = 6,
    And = 7,
    Or = 8,
    OpGt = 9,
    OpLt = 10,
    OpGe = 11,
    OpLe = 12,
    OpEq = 13,
    OpNe = 14,
    Error = 15,
};

}  // namespace toolgood
