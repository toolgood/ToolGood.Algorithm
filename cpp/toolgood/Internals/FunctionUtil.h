// FunctionUtil.h - 对应 csharp/.../Internals/Functions/FunctionUtil.cs 与 Internals/RegexHelper.cs
#pragma once

#include <cstdint>
#include <regex>
#include <string>
#include <vector>

#include "../Operands/Operand.h"

namespace toolgood {

// ---------------- 字符串/正则辅助（RegexHelper） ----------------

bool RegexIsHex(const std::string& value);
bool RegexIsOct(const std::string& value);
bool RegexIsBin(const std::string& value);
/// 获取（并缓存）正则对象；模式非法时抛 std::regex_error。
const std::regex& GetRegex(const std::string& pattern);

// ---------------- 值比较（FunctionUtil） ----------------

bool JsonValueEquals(const JsonData* v, const std::string& text);
bool OperandValueEquals(const Operand& op, const std::string& text);
bool TryParseBoolean(const std::string& text, bool& boolValue);

// ---------------- 数组展开 ----------------

int EstimateCount(const std::vector<Operand>& args);
bool FlattenToList(const std::vector<Operand>& args, std::vector<Operand>& list);
bool FlattenToNumberList(const std::vector<Operand>& args, std::vector<Decimal>& list);
bool FlattenToNumberList(const Operand& arg, std::vector<Decimal>& list);
bool FlattenToStringList(const Operand& arg, std::vector<std::string>& list);

// ---------------- 条件统计 ----------------

int GetCountIf(const std::vector<Decimal>& dbs, const Decimal& d);
int GetCountIf(const std::vector<Decimal>& dbs, const std::string& op, const Decimal& d);
Decimal GetSumIf(const std::vector<Decimal>& dbs, const Decimal& d,
                 const std::vector<Decimal>& sumDbs);
Decimal GetSumIf(const std::vector<Decimal>& dbs, const std::string& op, const Decimal& d,
                 const std::vector<Decimal>& sumDbs);
bool CompareValues(const Decimal& a, const Decimal& b, const std::string& op);

// ---------------- 数论/统计辅助 ----------------

Decimal GetGCD(const std::vector<Decimal>& list);
Decimal GetGCD(const Decimal& a, const Decimal& b);
Decimal GetLCM(const std::vector<Decimal>& list);
Decimal GetFactorial(const Decimal& a);
/// 解析 ">=5" / "<3" 之类的条件串；失败返回 false。
bool ParseSumIfMatch(const std::string& s, std::string& op, Decimal& value);
Decimal QuickSelect(std::vector<Decimal> list, int k, bool largest);
int GetRank(const std::vector<Decimal>& values, const Decimal& num, bool descending);

}  // namespace toolgood
