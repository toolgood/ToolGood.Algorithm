// CalculateTree.h - 对应 csharp/ToolGood.Algorithm/Internals/CalculateTree.cs
// 由 AlgorithmEngineHelper::ParseCalculate（对应 ANTLR 的 MathSplitVisitor2）生成，
// 供 FunctionCache 把表达式按运算符拆成可缓存的子表达式。
//
// 说明：C# 的 CombineCalculateType 定义在 Enums/CombineCalculateType.cs，
// 其数值与 CalculateTreeType 完全一致（Add=1 ... OpNe=14）。本移植不修改已存在的
// Enums.h，故把该枚举放在此处，与计算树一起使用。
#pragma once

#include <cstdint>
#include <memory>
#include <string>
#include <vector>

#include "../Enums.h"

namespace toolgood {

/// 合并类型（对应 Enums/CombineCalculateType.cs）
enum class CombineCalculateType : uint8_t {
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
};

/// 计算树（对应 CalculateTree）。节点按运算符二分，叶子 Type = String。
class CalculateTree {
public:
    CalculateTree() = default;

    /// 子节点（运算符节点的左右操作数）
    std::vector<CalculateTree> Nodes;
    /// 开始位置（原文码点下标）
    int Start = 0;
    /// 结束位置（原文码点下标，含）
    int End = -1;
    /// 类型
    CalculateTreeType Type = CalculateTreeType::String;
    /// 外面是否有括号
    bool HasBracket = false;
    /// 出错信息
    std::string ErrorMessage;

    /// 绑定原文（各节点共享同一份码点数组）。
    void SetSource(std::shared_ptr<const std::vector<uint32_t>> source);

    /// 文本（懒加载，返回原文 [Start, End] 区间；C# 里可能为 null，这里返回空串）。
    std::string Text() const;

private:
    std::shared_ptr<const std::vector<uint32_t>> source_;
    mutable std::string text_;
    mutable bool textReady_ = false;
};

}  // namespace toolgood
