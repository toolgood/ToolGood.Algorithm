using PetaTest;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    public class FunctionCacheTest
    {
        [Test]
        public void ParseWithCache_SameFormula_ReturnsSameInstance()
        {
            FunctionCache cache = new FunctionCache();
            var r1 = cache.ParseWithCache("abs(1)");
            var r2 = cache.ParseWithCache("abs(1)");
            Assert.IsTrue(r1 != null);
            Assert.IsTrue(ReferenceEquals(r1, r2));
        }

        [Test]
        public void ParseWithCache_DifferentFormulas_ReturnsDifferentInstances()
        {
            FunctionCache cache = new FunctionCache();
            var r1 = cache.ParseWithCache("abs(1)");
            var r2 = cache.ParseWithCache("abs(2)");
            Assert.IsTrue(r1 != null && r2 != null);
            Assert.IsTrue(!ReferenceEquals(r1, r2));
        }

        [Test]
        public void ParseWithCache_StringLeaf_ReturnsFunctionBase()
        {
            FunctionCache cache = new FunctionCache();
            // 修复: String 叶子节点也写入缓存(修复前直接返回不缓存)
            var r1 = cache.ParseWithCache("'abc'");
            var r2 = cache.ParseWithCache("'abc'");
            Assert.IsTrue(r1 != null);
            Assert.IsTrue(ReferenceEquals(r1, r2));
        }

        [Test]
        public void ParseWithCache_NestedFunction_ReturnsFunctionBase()
        {
            FunctionCache cache = new FunctionCache();
            var r = cache.ParseWithCache("max(1,2,3)");
            Assert.IsTrue(r != null);
        }

        [Test]
        public void ParseWithCache_DiyExpression_ReturnsFunctionBase()
        {
            FunctionCache cache = new FunctionCache();
            var r = cache.ParseWithCache("myVar");
            Assert.IsTrue(r != null);
        }

        [Test]
        public void ParseConditionWithCache_SimpleCondition_ReturnsFunctionBase()
        {
            FunctionCache cache = new FunctionCache();
            var r1 = cache.ParseConditionWithCache("1=1");
            var r2 = cache.ParseConditionWithCache("1=1");
            Assert.IsTrue(r1 != null);
            Assert.IsTrue(ReferenceEquals(r1, r2));
        }

        [Test]
        public void ParseConditionWithCache_ComplexCondition_ReturnsFunctionBase()
        {
            FunctionCache cache = new FunctionCache();
            var r = cache.ParseConditionWithCache("(1=1 || 2=2) && 3=3");
            Assert.IsTrue(r != null);
        }

        [Test]
        public void ParseConditionWithCache_StringLeaf_NoReentrantGetOrAdd()
        {
            FunctionCache cache = new FunctionCache();
            // 修复: 条件表达式内的字符串叶子直接走计算树路径(CreateCalculate),
            // 避免对同一 key 重入 GetOrAdd(ConcurrentDictionary 禁止 valueFactory 递归调用)
            var r1 = cache.ParseConditionWithCache("'abc' > ''");
            var r2 = cache.ParseConditionWithCache("'abc' > ''");
            Assert.IsTrue(r1 != null);
            Assert.IsTrue(ReferenceEquals(r1, r2));
        }

        [Test]
        public void ParseConditionWithCache_FormulaInCondition_ReturnsFunctionBase()
        {
            FunctionCache cache = new FunctionCache();
            var r = cache.ParseConditionWithCache("abs(1) > 0");
            Assert.IsTrue(r != null);
        }

        [Test]
        public void ParseWithCache_And_ParseConditionWithCache_Independent()
        {
            FunctionCache cache = new FunctionCache();
            var f1 = cache.ParseWithCache("abs(1)");
            var f2 = cache.ParseConditionWithCache("1=1");
            Assert.IsTrue(f1 != null && f2 != null);
            Assert.IsTrue(!ReferenceEquals(f1, f2));
        }
    }
}
