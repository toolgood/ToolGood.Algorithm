// ConditionTree.h - 对应 csharp/ToolGood.Algorithm/Internals/ConditionTree.cs
// 由 AlgorithmEngineHelper::ParseCondition（对应 ANTLR 的 MathSplitVisitor）生成：
// 只按 And / Or 分组，其余子表达式作为 String 叶子交给计算树处理。
//
// 说明：C# 的 ConditionTreeType 定义在 Enums/ConditionTreeType.cs，本移植不修改
// 已存在的 Enums.h，故把该枚举放在此处。
#pragma once

#include <cstdint>
#include <memory>
#include <string>
#include <vector>

#include "../Enums.h"

namespace toolgood {

/// 条件树类型（对应 Enums/ConditionTreeType.cs）
enum class ConditionTreeType : uint8_t {
    String = 0,
    And = 1,
    Or = 2,
    Error = 3,
};

/// 条件树（对应 ConditionTree）。
class ConditionTree {
public:
    ConditionTree() = default;

    /// 子节点（And / Or 节点的左右操作数）
    std::vector<ConditionTree> Nodes;
    /// 开始位置（原文码点下标）
    int Start = 0;
    /// 结束位置（原文码点下标，含）
    int End = -1;
    /// 类型
    ConditionTreeType Type = ConditionTreeType::String;
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
