// CalculationLogic.h - 对应 csharp/ToolGood.Algorithm/CalculationLogic/*.cs
//   CalculationLogicType.cs -> CalculationLogicType
//   CalculationLogicInfo.cs -> CalculationLogicInfo
//   CalculationLogicEngine.cs -> CalculationLogicEngine
//
// 说明：C# 的 CalculationLogicEngine 并不继承 AlgorithmEngine，而是内部持有一个
// AlgorithmEngineEx（参数表 + 求值），并通过 IFunctionCache 复用解析结果；本移植
// 保持同样的组合关系，以便与 C# 行为逐一对应。
//
// 异常约定：C# 的 FormatException -> std::runtime_error。
#pragma once

#include <cstdint>
#include <string>
#include <vector>

#include "../AlgorithmEngineEx.h"
#include "../AlgorithmEngineHelper.h"
#include "../Enums.h"

namespace toolgood {

/// 计算逻辑类型（对应 CalculationLogicType）
enum class CalculationLogicType : uint8_t {
    Scene = 0,
    InitValue = 1,
    Condition = 2,
    SetFormula = 3,
    SetValue = 4,
    Error = 5,
    BlankLine = 6,
};

/// 计算逻辑信息（对应 CalculationLogicInfo）
struct CalculationLogicInfo {
    /// 类型
    CalculationLogicType LogicType = CalculationLogicType::Scene;
    /// 层级
    int Layer = 0;
    /// 逻辑名称
    std::string Name;
    /// 公式
    std::string Exp;
    /// 公式2（把参数替换成值之后的公式）
    std::string Exp2;
    /// 备注
    std::string Remark;
    /// 值
    Operand Value;

    /// 转换为信息字符串（对应 ToInfoString）
    std::string ToInfoString() const;
};

/// 计算逻辑引擎（对应 CalculationLogicEngine）
class CalculationLogicEngine {
public:
    /// functionCache 需在引擎生命周期内保持有效。
    explicit CalculationLogicEngine(IFunctionCache& functionCache,
                                    bool useCalculationLogicInfo = false);

    // ---- SetScene ----
    /// 设置场景名称
    void SetSceneName(const std::string& scene);

    // ---- InitValue ----
    void InitValue(const std::string& key, const std::string& value, int layer = 0,
                   const std::string& remark = std::string());
    void InitValue(const std::string& key, const std::string& value, const std::string& remark);
    void InitValue(const std::string& key, const Decimal& value, int layer = 0,
                   const std::string& remark = std::string());
    void InitValue(const std::string& key, const Decimal& value, const std::string& remark);
    void InitValue(const std::string& key, double value, int layer = 0,
                   const std::string& remark = std::string());
    void InitValue(const std::string& key, double value, const std::string& remark);
    void InitValue(const std::string& key, bool value, int layer = 0,
                   const std::string& remark = std::string());
    void InitValue(const std::string& key, bool value, const std::string& remark);
    void InitValue(const std::string& key, int value, int layer = 0,
                   const std::string& remark = std::string());
    void InitValue(const std::string& key, int value, const std::string& remark);
    void InitValue(const std::string& key, const MyDate& value, int layer = 0,
                   const std::string& remark = std::string());
    void InitValue(const std::string& key, const MyDate& value, const std::string& remark);

    // ---- CheckCondition ----
    /// 检查条件；条件不是布尔值或求值出错时抛 std::runtime_error。
    bool CheckCondition(const std::string& condition, int layer = 0,
                        const std::string& remark = std::string());
    bool CheckCondition(const std::string& condition, const std::string& remark);

    // ---- SetFormula ----
    void SetFormula(const std::string& key, const std::string& exp, int layer = 0,
                    const std::string& remark = std::string());
    void SetFormula(const std::string& key, const std::string& exp, const std::string& remark);

    // ---- SetValue ----
    void SetValue(const std::string& key, const std::string& value, int layer = 0,
                  const std::string& remark = std::string());
    void SetValue(const std::string& key, const std::string& value, const std::string& remark);
    void SetValue(const std::string& key, const Decimal& value, int layer = 0,
                  const std::string& remark = std::string());
    void SetValue(const std::string& key, const Decimal& value, const std::string& remark);
    void SetValue(const std::string& key, double value, int layer = 0,
                  const std::string& remark = std::string());
    void SetValue(const std::string& key, double value, const std::string& remark);
    void SetValue(const std::string& key, bool value, int layer = 0,
                  const std::string& remark = std::string());
    void SetValue(const std::string& key, bool value, const std::string& remark);
    void SetValue(const std::string& key, int value, int layer = 0,
                  const std::string& remark = std::string());
    void SetValue(const std::string& key, int value, const std::string& remark);
    void SetValue(const std::string& key, const MyDate& value, int layer = 0,
                  const std::string& remark = std::string());
    void SetValue(const std::string& key, const MyDate& value, const std::string& remark);

    // ---- BlankLine ----
    /// 添加空行
    void BlankLine();

    // ---- GetValue ----
    /// 获取值
    Operand GetValue(const std::string& key);

    // ---- ToInfoString ----
    /// 转换为信息字符串（C# 用 Environment.NewLine，这里统一用 "\n"）
    std::string ToInfoString() const;

    // ---- 移植便利接口（C# 中为私有字段）----
    AlgorithmEngineEx& Engine() { return engine_; }
    const AlgorithmEngineEx& Engine() const { return engine_; }
    const std::vector<CalculationLogicInfo>& InitValueInfos() const { return initValueInfos_; }
    const std::vector<CalculationLogicInfo>& CalculationLogicInfos() const {
        return calculationLogicInfos_;
    }

private:
    void InitValueCore(const std::string& key, const Operand& value, const std::string& exp,
                       int layer, const std::string& remark);
    void SetValueCore(const std::string& key, const Operand& value, const std::string& exp,
                      int layer, const std::string& remark);
    /// 把公式里的自定义参数替换成当前值（对应 ExpAnalysis）
    std::string ExpAnalysis(const std::string& exp);
    /// 参数值的文本形式（对应私有 GetOperand）
    static std::string GetOperandText(AlgorithmEngineEx& engine, const std::string& name);

    IFunctionCache& functionCache_;
    std::vector<CalculationLogicInfo> initValueInfos_;
    std::vector<CalculationLogicInfo> calculationLogicInfos_;
    bool useCalculationLogicInfo_ = false;
    AlgorithmEngineEx engine_;
};

}  // namespace toolgood
