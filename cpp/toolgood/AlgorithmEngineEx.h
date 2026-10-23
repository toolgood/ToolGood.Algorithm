// AlgorithmEngineEx.h - 对应 csharp/ToolGood.Algorithm/AlgorithmEngineEx.cs
// AlgorithmEngine 的扩展：带自定义参数缓存（可忽略大小写），由调用方重写
// GetParameterEx 提供参数来源，再用 AddParameter 写入中间结果。
#pragma once

#include <map>
#include <string>
#include <type_traits>
#include <vector>

#include "AlgorithmEngine.h"
#include "Decimal.h"
#include "MyDate.h"
#include "Operands/Operand.h"

namespace toolgood {

/// 参数名比较器：IgnoreCase 为 true 时按 ASCII 忽略大小写（对应 StringComparer.OrdinalIgnoreCase）。
struct ParameterKeyLess {
    bool IgnoreCase = false;

    bool operator()(const std::string& a, const std::string& b) const;
};

/// 算法引擎扩展（对应 AlgorithmEngineEx）。
class AlgorithmEngineEx : public AlgorithmEngine {
public:
    /// 默认不带缓存
    AlgorithmEngineEx();
    /// 指定参数名是否忽略大小写
    explicit AlgorithmEngineEx(bool ignoreCase);

    /// 是否忽略大小写（对应只读属性 IgnoreCase）
    bool IgnoreCase() const { return ignoreCase_; }

    /// 保存到临时字典（对应 UseTempDict）
    bool UseTempDict = false;

    /// 取参数：先查缓存，再调 GetParameterEx
    Operand GetParameter(const std::string& parameter) override;

    /// 参数来源（请重写）
    virtual Operand GetParameterEx(const std::string& parameter);

    /// 清理参数
    virtual void ClearParameters();

    // ---- AddParameter ----
    virtual void AddParameter(const std::string& key, const Operand& obj);
    virtual void AddParameter(const std::string& key, bool obj);
    virtual void AddParameter(const std::string& key, int obj);
    virtual void AddParameter(const std::string& key, int64_t obj);
    virtual void AddParameter(const std::string& key, double obj);
    virtual void AddParameter(const std::string& key, const Decimal& obj);
    virtual void AddParameter(const std::string& key, const std::string& obj);
    /// 字符串字面量；避免 const char* 被隐式转成 bool
    void AddParameter(const std::string& key, const char* obj);
    virtual void AddParameter(const std::string& key, const MyDate& obj);
    virtual void AddParameter(const std::string& key, const std::vector<Operand>& obj);
    virtual void AddParameter(const std::string& key, const std::vector<std::string>& obj);
    virtual void AddParameter(const std::string& key, const std::vector<double>& obj);
    virtual void AddParameter(const std::string& key, const std::vector<int>& obj);
    virtual void AddParameter(const std::string& key, const std::vector<bool>& obj);

    /// 其它整数类型（short/ushort/uint/long/ulong/char ...），
    /// C# 里是若干重载，这里用模板避免 long 与 int64_t 的重载歧义。
    template <typename T,
              typename std::enable_if<std::is_integral<T>::value &&
                                          !std::is_same<T, bool>::value,
                                      int>::type = 0>
    void AddParameter(const std::string& key, T obj) {
        if constexpr (std::is_unsigned<T>::value) {
            tempdict_[key] = Operand::Create(Decimal::FromUInt(static_cast<uint64_t>(obj)));
        } else {
            tempdict_[key] = Operand::Create(static_cast<int64_t>(obj));
        }
    }

    /// 由 JSON 对象批量添加参数（对应 AddParameterFromJson）
    virtual void AddParameterFromJson(const std::string& json);

protected:
    std::map<std::string, Operand, ParameterKeyLess> tempdict_;

private:
    bool ignoreCase_ = false;
};

}  // namespace toolgood
