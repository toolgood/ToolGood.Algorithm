package algorithm

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

	// 触发各函数子包的 init()，把函数节点注册到 internals 注册表。
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/compare"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/csharp"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/csharpsecurity"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/csharpweb"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/datetimes"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/financial"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/flow"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/mathbase"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/mathsum"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/mathsum2"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/mathtransformation"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/mathtrigonometric"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/operator"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/string"
	_ "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/value"
)

// init 把本包的解析器注入内核引擎（内核不反向依赖解析器，避免循环依赖）。
func init() {
	internals.ParseExpression = func(text string) (internals.Function, error) {
		return ParseFunction(text)
	}
}
