package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestIssuesTest 移植自
// csharp/ToolGood.Algorithm.Test/Tests/IssuesTest.cs。
func TestIssuesTest(t *testing.T) {
	t.Run("issues_12", func(t *testing.T) {
		engine := newEngine()
		// 防止有误解，禁止数字直接转时间、禁止时间直接转数字；
		// 原 C# 用例断言部分均被注释，这里只保留引擎构造。
		assertNotNull(t, engine)
	})

	t.Run("issues_13", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1275, evalInt(engine, "days360(date(2020,5,31),date(2023,12,15))"))
	})

	t.Run("issues_0", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("瓦楞", "BC")
		exp := `{"A": 0.6,"B": 0.4,"C": 0.6,"E": 0.33,"F": 0.29,"Z": 0.15
,"EB": 0.7,"EE": 0.65,"EA": 0.85,"AB": 1.0,"BC": 1.0,"AA":1.0
,"EBC": 1.15,"BAB": 1.25,"BCB": 1.25,"BBC": 1.25,"CBB": 1.25,"EBA": 1.2,"AAA": 1.4}[瓦楞]`
		assertInt(t, 1, evalInt(engine.AlgorithmEngine, exp))
	})
}
