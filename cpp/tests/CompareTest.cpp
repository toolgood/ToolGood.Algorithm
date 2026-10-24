// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/CompareTest.cs
#include "cpptest.h"

#include "toolgood/AlgorithmEngine.h"

using namespace toolgood;

namespace {

TEST(Compare, base_test) {
    AlgorithmEngine engine;
    Assert::AreEqual(true, engine.TryEvaluate("1=1", false));
    Assert::AreEqual(false, engine.TryEvaluate("1=2", true));
    Assert::AreEqual(true, engine.TryEvaluate("1<>2", false));
    Assert::AreEqual(true, engine.TryEvaluate("1!=2", false));
    Assert::AreEqual(false, engine.TryEvaluate("1>2", true));
    Assert::AreEqual(true, engine.TryEvaluate("1<2", false));
    Assert::AreEqual(true, engine.TryEvaluate("1<=2", false));
    Assert::AreEqual(false, engine.TryEvaluate("1>=2", true));
    Assert::AreEqual(true, engine.TryEvaluate("'1'='1'", false));
    Assert::AreEqual(true, engine.TryEvaluate("'e'='e'", false));
    Assert::AreEqual(false, engine.TryEvaluate("'1'='2'", true));
    Assert::AreEqual(false, engine.TryEvaluate("'我'='你'", true));
    Assert::AreEqual(true, engine.TryEvaluate("'1'!='2'", false));
}

TEST(Compare, strict_equality_test) {
    AlgorithmEngine engine;
    Assert::AreEqual(true, engine.TryEvaluate("1===1", false));
    Assert::AreEqual(false, engine.TryEvaluate("1===2", true));
    Assert::AreEqual(true, engine.TryEvaluate("'1'==='1'", false));
    Assert::AreEqual(false, engine.TryEvaluate("'1'==='2'", true));
    Assert::AreEqual(true, engine.TryEvaluate("1!==2", false));
    Assert::AreEqual(false, engine.TryEvaluate("1!==1", true));
    Assert::AreEqual(true, engine.TryEvaluate("'1'!=='2'", false));
    Assert::AreEqual(false, engine.TryEvaluate("'1'!=='1'", true));
}

TEST(Compare, null_compare_test) {
    AlgorithmEngine engine;

    bool bb2 = engine.TryEvaluate("1>null", true);
    Assert::AreEqual(false, bb2);
    bb2 = engine.TryEvaluate("1>=null", true);
    Assert::AreEqual(false, bb2);
    bb2 = engine.TryEvaluate("1<=null", true);
    Assert::AreEqual(false, bb2);
    bb2 = engine.TryEvaluate("1<null", true);
    Assert::AreEqual(false, bb2);
    bb2 = engine.TryEvaluate("1==null", true);
    Assert::AreEqual(false, bb2);
    bb2 = engine.TryEvaluate("1!=null", false);
    Assert::AreEqual(true, bb2);

    // 两侧同为 null 属于异常数据，返回错误，TryEvaluate 返回默认值
    bb2 = engine.TryEvaluate("null=null", false);
    Assert::AreEqual(false, bb2);
    Assert::IsNotNull(engine.LastError);

    bb2 = engine.TryEvaluate("null!=null", true);
    Assert::AreEqual(true, bb2);
    Assert::IsNotNull(engine.LastError);

    bb2 = engine.TryEvaluate("'111'=null", true);
    Assert::AreEqual(false, bb2);
    bb2 = engine.TryEvaluate("'111'!=null", false);
    Assert::AreEqual(true, bb2);
}

TEST(Compare, null_same_type_compare_test) {
    AlgorithmEngine engine;
    // 两侧同为 null 属于异常数据，所有比较运算符均返回错误，TryEvaluate 返回默认值
    bool bb2 = engine.TryEvaluate("null>null", true);
    Assert::AreEqual(true, bb2);
    Assert::IsNotNull(engine.LastError);
    bb2 = engine.TryEvaluate("null>=null", true);
    Assert::AreEqual(true, bb2);
    Assert::IsNotNull(engine.LastError);
    bb2 = engine.TryEvaluate("null<=null", true);
    Assert::AreEqual(true, bb2);
    Assert::IsNotNull(engine.LastError);
    bb2 = engine.TryEvaluate("null<null", true);
    Assert::AreEqual(true, bb2);
    Assert::IsNotNull(engine.LastError);
}

TEST(Compare, negative_compare_test) {
    AlgorithmEngine engine;
    Assert::AreEqual(true, engine.TryEvaluate("1 > (-2)", false));
    Assert::AreEqual(true, engine.TryEvaluate("(-1) > (-2）", false));
    Assert::AreEqual(true, engine.TryEvaluate("-1 > (-2)", false));
    Assert::AreEqual(true, engine.TryEvaluate("-1 > -2", false));
    Assert::AreEqual(true, engine.TryEvaluate("-7 < -2", false));
    Assert::AreEqual(true, engine.TryEvaluate("-7*Yes < -2", false));
    Assert::AreEqual(true, engine.TryEvaluate("-7*No > -2", false));
}

}  // namespace
