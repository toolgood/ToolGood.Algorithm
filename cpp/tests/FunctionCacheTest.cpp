// 移植自 csharp/ToolGood.Algorithm.Test/FunctionCacheTest.cs
//
// 类型对应：
//   FunctionCache cache; cache.ParseWithCache(...)   -> 同（返回 Function = shared_ptr<FunctionBase>）
//   r != null                                        -> r != nullptr
//   ReferenceEquals(r1, r2)                          -> r1.get() == r2.get()
#include "cpptest.h"

#include <string>

#include "toolgood/AlgorithmEngineHelper.h"

using namespace toolgood;

namespace {

TEST(FunctionCache, ParseWithCache_SameFormula_ReturnsSameInstance) {
    FunctionCache cache;
    Function r1 = cache.ParseWithCache("abs(1)");
    Function r2 = cache.ParseWithCache("abs(1)");
    Assert::IsTrue(r1 != nullptr);
    Assert::IsTrue(r1.get() == r2.get());
}

TEST(FunctionCache, ParseWithCache_DifferentFormulas_ReturnsDifferentInstances) {
    FunctionCache cache;
    Function r1 = cache.ParseWithCache("abs(1)");
    Function r2 = cache.ParseWithCache("abs(2)");
    Assert::IsTrue(r1 != nullptr && r2 != nullptr);
    Assert::IsTrue(!(r1.get() == r2.get()));
}

TEST(FunctionCache, ParseWithCache_StringLeaf_ReturnsFunctionBase) {
    FunctionCache cache;
    // 修复: String 叶子节点也写入缓存(修复前直接返回不缓存)
    Function r1 = cache.ParseWithCache("'abc'");
    Function r2 = cache.ParseWithCache("'abc'");
    Assert::IsTrue(r1 != nullptr);
    Assert::IsTrue(r1.get() == r2.get());
}

TEST(FunctionCache, ParseWithCache_NestedFunction_ReturnsFunctionBase) {
    FunctionCache cache;
    Function r = cache.ParseWithCache("max(1,2,3)");
    Assert::IsTrue(r != nullptr);
}

TEST(FunctionCache, ParseWithCache_DiyExpression_ReturnsFunctionBase) {
    FunctionCache cache;
    Function r = cache.ParseWithCache("myVar");
    Assert::IsTrue(r != nullptr);
}

TEST(FunctionCache, ParseConditionWithCache_SimpleCondition_ReturnsFunctionBase) {
    FunctionCache cache;
    Function r1 = cache.ParseConditionWithCache("1=1");
    Function r2 = cache.ParseConditionWithCache("1=1");
    Assert::IsTrue(r1 != nullptr);
    Assert::IsTrue(r1.get() == r2.get());
}

TEST(FunctionCache, ParseConditionWithCache_ComplexCondition_ReturnsFunctionBase) {
    FunctionCache cache;
    Function r = cache.ParseConditionWithCache("(1=1 || 2=2) && 3=3");
    Assert::IsTrue(r != nullptr);
}

TEST(FunctionCache, ParseConditionWithCache_StringLeaf_NoReentrantGetOrAdd) {
    FunctionCache cache;
    // 修复: 条件表达式内的字符串叶子直接走计算树路径(CreateCalculate)，
    // 避免对同一 key 重入 GetOrAdd(ConcurrentDictionary 禁止 valueFactory 递归调用)
    Function r1 = cache.ParseConditionWithCache("'abc' > ''");
    Function r2 = cache.ParseConditionWithCache("'abc' > ''");
    Assert::IsTrue(r1 != nullptr);
    Assert::IsTrue(r1.get() == r2.get());
}

TEST(FunctionCache, ParseConditionWithCache_FormulaInCondition_ReturnsFunctionBase) {
    FunctionCache cache;
    Function r = cache.ParseConditionWithCache("abs(1) > 0");
    Assert::IsTrue(r != nullptr);
}

TEST(FunctionCache, ParseWithCache_And_ParseConditionWithCache_Independent) {
    FunctionCache cache;
    Function f1 = cache.ParseWithCache("abs(1)");
    Function f2 = cache.ParseConditionWithCache("1=1");
    Assert::IsTrue(f1 != nullptr && f2 != nullptr);
    Assert::IsTrue(!(f1.get() == f2.get()));
}

}  // namespace
