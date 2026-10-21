// 本文件移植 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/ValueTest.cs。
package test

import (
	"math"
	"strings"
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestValueTest 对应 C# ValueTest 测试类。
func TestValueTest(t *testing.T) {
	t.Run("constant_test", func(t *testing.T) {
		engine := newEngine()
		assertFloatRound(t, math.E, evalFloat(engine, "e"), 10)
		assertFloatRound(t, math.Pi, evalFloat(engine, "pi"), 10)

		assertBool(t, true, evalBoolDef(engine, "true", false))
		assertBool(t, false, evalBoolDef(engine, "false", true))
	})

	t.Run("boolean_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 1, evalInt(engine, "if(true,1,2)"))
		assertInt(t, 2, evalInt(engine, "if(false,1,2)"))
	})

	t.Run("array_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 4, evalInt(engine, "count(Array(1,2,3,4))"))
		assertInt(t, 11, evalInt(engine, "(1=1)*9+2"))
		assertInt(t, 2, evalInt(engine, "(1=2)*9+2"))
	})

	t.Run("TestVersion", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "ToolGood.Algorithm 6.3", evalText(engine, "Engineversion"))
		assertText(t, "ToolGood.Algorithm 6.3", evalText(engine, "Algorithmversion"))
	})

	t.Run("Test_Json", func(t *testing.T) {
		engine := newEngine()
		c := evalOperand(engine, `{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}`).String()
		assertText(t, `{"灰色":"L","canBookCount":905,"saleCount":91,"specId":"43b0e72e98731aed69e1f0cc7d64bf4d"}`, c)
	})

	t.Run("Json_large_number_error_test", func(t *testing.T) {
		engine := newEngine()
		// Bug A 回归: 超出 decimal 范围的超大数解析应报错(修复前静默返回 0)
		_ = evalFloat(engine, `json('{"a":99999999999999999999999999999999999}')['a']`)
		assertLastError(t, engine)
	})

	t.Run("Json_string_indexer_no_exception_test", func(t *testing.T) {
		engine := newEngine()
		// Bug B 回归: 非对象 JSON 值被字符串索引不应抛 NullReferenceException
		_ = evalText(engine, `json('123')['a']`)
		assertLastError(t, engine)

		// 不存在的键返回错误而非异常
		_ = evalText(engine, `json('{"a":1}')['b']`)
		assertLastError(t, engine)
	})

	t.Run("Json_backslash_escape_test", func(t *testing.T) {
		engine := newEngine()
		// Bug C 回归: 序列化字符串应转义反斜杠(修复前漏转义, 输出为 C:\temp)
		c := evalOperand(engine, `{'a':'C:\\temp'}`).String()
		assertText(t, `{"a":"C:\\temp"}`, c)

		// 反斜杠与引号同时转义
		c = evalOperand(engine, `{'a':'a\"b\\c'}`).String()
		assertText(t, `{"a":"a\"b\\c"}`, c)
	})

	t.Run("PARAM_test", func(t *testing.T) {
		engine := newCylinder(10, 15)
		assertInt(t, 10, evalInt(engine.AlgorithmEngine, "PARAM('半径')"))
		assertInt(t, 10, evalInt(engine.AlgorithmEngine, "PARAMETER('半径')"))
		assertInt(t, 10, evalInt(engine.AlgorithmEngine, "GETPARAMETER('半径')"))
		assertInt(t, 10, evalInt(engine.AlgorithmEngine, "半径"))
	})

	t.Run("Error_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "", evalText(engine, "Error('出错了')"))
		assertText(t, "出错了", engine.LastError)
	})

	t.Run("Json_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "toolgood", evalText(engine, `{name:'toolgood', age:'12',}['name']`))
		assertText(t, "IT", evalText(engine, `{name:'toolgood', age:'12',other:{work:'IT'}}['other']['work']`))
		assertText(t, "toolgood", evalText(engine, `json("{'name':'toolgood', 'age':'12','other':{'work':'IT'}}")['name']`))
		assertText(t, "IT", evalText(engine, `json("{'name':'toolgood', 'age':'12','other':{'work':'IT'}}")['other']['work']`))

		assertBool(t, true, evalBool(engine, `{name:'toolgood', age:'12',other:{work:'IT'}}.has('age')`))
		assertBool(t, true, evalBool(engine, `{name:'toolgood', age:'12',other:{work:'IT'}}.hasKey('age')`))
		assertBool(t, true, evalBool(engine, `{e:'toolgood', pi:'12',other:{work:'IT'}}.hasKey('e')`))
		assertBool(t, true, evalBool(engine, `json("{'name':'toolgood', 'age':'12','other':{'work':'IT'}}").has('age')`))
		assertBool(t, false, evalBoolDef(engine, `{name:'toolgood', age:'12',other:{work:'IT'}}.has('work')`, true))
		assertBool(t, true, evalBool(engine, `{name:'toolgood', age:'12',other:{work:'IT'}}.hasValue('toolgood')`))
		assertBool(t, true, evalBool(engine, `json("{'name':'toolgood', 'age':'12','other':{'work':'IT'}}").hasValue('toolgood')`))
	})

	t.Run("array_test2", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(true)
		assertInt(t, 2, evalInt(engine, "[1,2,3,4,][2]"))
		assertText(t, "555", evalText(engine, "[1,2,3,4,'555'][5]"))

		assertBool(t, true, evalBool(engine, "[1,2,3,4,].has('1')"))
		assertBool(t, true, evalBool(engine, "['abc','age'].hasKey('age')"))
		assertBool(t, true, evalBool(engine, "['abc','age'].hasValue('age')"))
		assertBool(t, true, evalBool(engine, `json("['abc','age']").has('age')`))
		assertBool(t, true, evalBool(engine, `json("['abc','age']").hasValue('age')`))
	})

	t.Run("Distance_M_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "1=1m"))
		assertBool(t, true, evalBool(engine, "1=10dm"))
		assertBool(t, true, evalBool(engine, "1=100cm"))
		assertBool(t, true, evalBool(engine, "1=1000mm"))
		assertBool(t, true, evalBool(engine, "1=0.001km"))
	})

	t.Run("Distance_DM_Test", func(t *testing.T) {
		engine := newEngine()
		engine.DistanceUnit = alg.DistanceUnitTypeDM
		assertBool(t, true, evalBool(engine, "1=0.1m"))
		assertBool(t, true, evalBool(engine, "1=1dm"))
		assertBool(t, true, evalBool(engine, "1=10cm"))
		assertBool(t, true, evalBool(engine, "1=100mm"))
		assertBool(t, true, evalBool(engine, "1=0.0001km"))
	})

	t.Run("Distance_CM_Test", func(t *testing.T) {
		engine := newEngine()
		engine.DistanceUnit = alg.DistanceUnitTypeCM
		assertBool(t, true, evalBool(engine, "1=0.01m"))
		assertBool(t, true, evalBool(engine, "1=0.1dm"))
		assertBool(t, true, evalBool(engine, "1=1cm"))
		assertBool(t, true, evalBool(engine, "1=10mm"))
		assertBool(t, true, evalBool(engine, "1=0.00001km"))
	})

	t.Run("Distance_MM_Test", func(t *testing.T) {
		engine := newEngine()
		engine.DistanceUnit = alg.DistanceUnitTypeMM
		assertBool(t, true, evalBool(engine, "1=0.001m"))
		assertBool(t, true, evalBool(engine, "1=0.01dm"))
		assertBool(t, true, evalBool(engine, "1=0.1cm"))
		assertBool(t, true, evalBool(engine, "1=1mm"))
		assertBool(t, true, evalBool(engine, "1=0.000001km"))
	})

	t.Run("Distance_KM_Test", func(t *testing.T) {
		engine := newEngine()
		engine.DistanceUnit = alg.DistanceUnitTypeKM
		assertBool(t, true, evalBool(engine, "1=1m*1000"))
		assertBool(t, true, evalBool(engine, "1=10dm*1000"))
		assertBool(t, true, evalBool(engine, "1=100cm*1000"))
		assertBool(t, true, evalBool(engine, "1=1000mm*1000"))
		assertBool(t, true, evalBool(engine, "1=0.001km*1000"))
	})

	t.Run("Area_M2_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "1=1m*1m"))
		assertBool(t, true, evalBool(engine, "1m2=1m*1m"))
		assertBool(t, true, evalBool(engine, "1m2=10dm*10dm"))
		assertBool(t, true, evalBool(engine, "1m2=100cm*100cm"))
		assertBool(t, true, evalBool(engine, "1m2=1000mm*1000mm"))
		assertBool(t, true, evalBool(engine, "1m2=0.001km*0.001km"))
		assertBool(t, true, evalBool(engine, "1=1m2"))
		assertBool(t, true, evalBool(engine, "1=100dm2"))
		assertBool(t, true, evalBool(engine, "1=10000cm2"))
		assertBool(t, true, evalBool(engine, "1=1000000mm2"))
		assertBool(t, true, evalBool(engine, "1=0.000001km2"))
		assertBool(t, true, evalBool(engine, "1km2=1km*1km"))
	})

	t.Run("Area_DM2_Test", func(t *testing.T) {
		engine := newEngine()
		engine.AreaUnit = alg.AreaUnitTypeDM2
		assertBool(t, true, evalBool(engine, "1=1m2/100"))
		assertBool(t, true, evalBool(engine, "1=100dm2/100"))
		assertBool(t, true, evalBool(engine, "1=10000cm2/100"))
		assertBool(t, true, evalBool(engine, "1=1000000mm2/100"))
		assertBool(t, true, evalBool(engine, "1=0.000001km2/100"))
	})

	t.Run("Area_CM2_Test", func(t *testing.T) {
		engine := newEngine()
		engine.AreaUnit = alg.AreaUnitTypeCM2
		assertBool(t, true, evalBool(engine, "1=1m2/100/100"))
		assertBool(t, true, evalBool(engine, "1=100dm2/100/100"))
		assertBool(t, true, evalBool(engine, "1=10000cm2/100/100"))
		assertBool(t, true, evalBool(engine, "1=1000000mm2/100/100"))
		assertBool(t, true, evalBool(engine, "1=0.000001km2/100/100"))
	})

	t.Run("Area_MM2_Test", func(t *testing.T) {
		engine := newEngine()
		engine.AreaUnit = alg.AreaUnitTypeMM2
		assertBool(t, true, evalBool(engine, "1=1m2/100/100/100"))
		assertBool(t, true, evalBool(engine, "1=100dm2/100/100/100"))
		assertBool(t, true, evalBool(engine, "1=10000cm2/100/100/100"))
		assertBool(t, true, evalBool(engine, "1=1000000mm2/100/100/100"))
		assertBool(t, true, evalBool(engine, "1=0.000001km2/100/100/100"))
	})

	t.Run("Area_KM2_Test", func(t *testing.T) {
		engine := newEngine()
		engine.AreaUnit = alg.AreaUnitTypeKM2
		assertBool(t, true, evalBool(engine, "1=1m2*1000*1000"))
		assertBool(t, true, evalBool(engine, "1=100dm2*1000*1000"))
		assertBool(t, true, evalBool(engine, "1=10000cm2*1000*1000"))
		assertBool(t, true, evalBool(engine, "1=1000000mm2*1000*1000"))
		assertBool(t, true, evalBool(engine, "1=0.000001km2*1000*1000"))
	})

	t.Run("Volume_M3_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "1=1m*1m*1m"))
		assertBool(t, true, evalBool(engine, "1m3=1m*1m*1m"))
		assertBool(t, true, evalBool(engine, "1m3=1m2*1m"))
		assertBool(t, true, evalBool(engine, "1m3=1000L"))
		assertBool(t, true, evalBool(engine, "1L=1000ml"))
		assertBool(t, true, evalBool(engine, "1m3=1000ml*1000"))
		assertBool(t, true, evalBool(engine, "1m3=10dm*10dm*10dm"))
		assertBool(t, true, evalBool(engine, "1m3=100cm*100cm*100cm"))
		assertBool(t, true, evalBool(engine, "1m3=1000mm*1000mm*1000mm"))
		assertBool(t, true, evalBool(engine, "1m3=0.001km*0.001km*0.001km"))
		assertBool(t, true, evalBool(engine, "1=1m3"))
		assertBool(t, true, evalBool(engine, "1=1000dm3"))
		assertBool(t, true, evalBool(engine, "1=1000000cm3"))
		assertBool(t, true, evalBool(engine, "1=1000000000mm3"))
		assertBool(t, true, evalBool(engine, "1=1km3/1000/1000/1000"))
	})

	t.Run("Volume_DM3_Test", func(t *testing.T) {
		engine := newEngine()
		engine.VolumeUnit = alg.VolumeUnitTypeDM3
		assertBool(t, true, evalBool(engine, "1=1m3/1000"))
		assertBool(t, true, evalBool(engine, "1=1000dm3/1000"))
		assertBool(t, true, evalBool(engine, "1=1000000cm3/1000"))
		assertBool(t, true, evalBool(engine, "1=1000000000mm3/1000"))
		assertBool(t, true, evalBool(engine, "1=1km3/1000/1000/1000/1000"))
	})

	t.Run("Volume_CM3_Test", func(t *testing.T) {
		engine := newEngine()
		engine.VolumeUnit = alg.VolumeUnitTypeCM3
		assertBool(t, true, evalBool(engine, "1=1m3/1000/1000"))
		assertBool(t, true, evalBool(engine, "1=1000dm3/1000/1000"))
		assertBool(t, true, evalBool(engine, "1=1000000cm3/1000/1000"))
		assertBool(t, true, evalBool(engine, "1=1000000000mm3/1000/1000"))
		assertBool(t, true, evalBool(engine, "1=1km3/1000/1000/1000/1000/1000"))
	})

	t.Run("Volume_MM3_Test", func(t *testing.T) {
		engine := newEngine()
		engine.VolumeUnit = alg.VolumeUnitTypeMM3
		assertBool(t, true, evalBool(engine, "1=1m3/1000/1000/1000"))
		assertBool(t, true, evalBool(engine, "1=1000dm3/1000/1000/1000"))
		assertBool(t, true, evalBool(engine, "1=1000000cm3/1000/1000/1000"))
		assertBool(t, true, evalBool(engine, "1=1000000000mm3/1000/1000/1000"))
		assertBool(t, true, evalBool(engine, "1=1km3/1000/1000/1000/1000/1000/1000"))
	})

	t.Run("Volume_KM3_Test", func(t *testing.T) {
		engine := newEngine()
		engine.VolumeUnit = alg.VolumeUnitTypeKM3
		assertBool(t, true, evalBool(engine, "1=1m3*1000*1000*1000"))
		assertBool(t, true, evalBool(engine, "1=1000dm3*1000*1000*1000"))
		assertBool(t, true, evalBool(engine, "1=1000000cm3*1000*1000*1000"))
		assertBool(t, true, evalBool(engine, "1=1000000000mm3*1000*1000*1000"))
		assertBool(t, true, evalBool(engine, "1=1km3"))
	})

	t.Run("Mass_KG_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "1=1kg"))
		assertBool(t, true, evalBool(engine, "1=1000g"))
		assertBool(t, true, evalBool(engine, "1=0.001t"))
	})

	t.Run("Mass_G_Test", func(t *testing.T) {
		engine := newEngine()
		engine.MassUnit = alg.MassUnitTypeG
		assertBool(t, true, evalBool(engine, "1=1kg/1000"))
		assertBool(t, true, evalBool(engine, "1=1000g/1000"))
		assertBool(t, true, evalBool(engine, "1=0.001t/1000"))
	})

	t.Run("Mass_T_Test", func(t *testing.T) {
		engine := newEngine()
		engine.MassUnit = alg.MassUnitTypeT
		assertBool(t, true, evalBool(engine, "1=1kg*1000"))
		assertBool(t, true, evalBool(engine, "1=1000g*1000"))
		assertBool(t, true, evalBool(engine, "1=0.001t*1000"))
	})

	t.Run("Unit_Error_Test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "1m=1kg"))
		assertBool(t, true, evalBool(engine, "1m=1m2"))
		assertBool(t, true, evalBool(engine, "1m=1m3"))
	})

	t.Run("P0_1_number_tostring_invariant_culture_test", func(t *testing.T) {
		// P0-1 回归: ToString 必须使用 InvariantCulture。
		// 差异：Go 无 Thread.CurrentCulture，数字格式化天然为 InvariantCulture，
		// 因此这里去掉 de-DE 区域切换，其余断言保持一致。
		engine := newEngine()
		fn, err := engine.Parse("1.5")
		assertTrue(t, err == nil && fn != nil, "Parse 1.5 失败: %v", err)
		assertText(t, "1.5", alg.ToString(fn))

		fn, err = engine.Parse("1.5M")
		assertTrue(t, err == nil && fn != nil, "Parse 1.5M 失败: %v", err)
		assertText(t, "1.5M", alg.ToString(fn))

		fn, err = engine.Parse("1234.5 + 0")
		assertTrue(t, err == nil && fn != nil, "Parse 1234.5 + 0 失败: %v", err)
		assertText(t, "1234.5 + 0", alg.ToString(fn))

		fn15, err := engine.Parse("1.5")
		assertTrue(t, err == nil && fn15 != nil, "Parse 1.5 失败: %v", err)
		e2 := newEngine()
		assertFloatRound(t, 1.5, evalFloat(e2, alg.ToString(fn15)), 10)

		assertRoundTrip(t, "1.5")
		assertRoundTrip(t, "1.5 + 1")
		assertRoundTrip(t, "1.5M")
		assertRoundTrip(t, "1.5M + 1M")
		assertRoundTrip(t, "1234.5 + 0")
	})

	t.Run("P0_2_arrayjson_error_propagation_test", func(t *testing.T) {
		engine := newEngine()
		// P0-2 回归: JSON 项内的错误必须向上传播
		r := evalOperand(engine, `{"a":1, "b":1/0}`)
		assertBool(t, true, r.IsError())
		assertText(t, "Function '/' Div 0 error!", r.ErrorMsg())

		r = evalOperand(engine, `{"a":1/0}`)
		assertBool(t, true, r.IsError())

		// 正常 JSON 不受影响
		r = evalOperand(engine, `{"a":1,"b":2}`)
		assertBool(t, false, r.IsError())
		assertText(t, `{"a":1,"b":2}`, r.String())

		assertRoundTrip(t, `{"a":1,"b":2}`)
		assertRoundTrip(t, `{"a":1, "b":1/0}`)
	})

	t.Run("P0_3_number_out_of_range_parse_test", func(t *testing.T) {
		engine := newEngine()
		// P0-3 回归: 超出 decimal 范围的纯数字必须在 Parse 阶段报错
		_, err := engine.Parse("99999999999999999999999999999")
		assertTrue(t, err != nil, "P0-3 回归失败: 29 位纯数字应在 Parse 阶段报错")
		assertContains(t, err.Error(), "is invalid or out of range")

		// ParseWithoutError 不抛异常，仅返回 nil
		assertNull(t, engine.ParseWithoutError("99999999999999999999999999999"))

		// 边界内数值仍可正常解析求值
		r := evalOperand(engine, "9999999999999999999999999999")
		assertBool(t, false, r.IsError())
		assertDecimal(t, "9999999999999999999999999999", r.NumberValue())
	})

	t.Run("P0_4_number_unit_overflow_test", func(t *testing.T) {
		engine := newEngine()
		// P0-4 回归: 单位换算放大溢出应返回 #NUM! 错误操作数
		r := evalOperand(engine, "9999999999999999999999999999KM3")
		assertBool(t, true, r.IsError())
		assertText(t, "Function 'Num' Num error!", r.ErrorMsg())

		// 溢出后 TryEvaluate 返回默认值，并填充 LastError
		assertFloatRound(t, -1.0, evalFloatDef(engine, "9999999999999999999999999999KM3", -1.0), 10)
		assertLastError(t, engine)

		// 未溢出的单位换算不受影响
		r = evalOperand(engine, "1KM")
		assertBool(t, false, r.IsError())
		assertDecimal(t, "1000", r.NumberValue())

		// 单位数值同样支持往返
		assertRoundTrip(t, "1KM")
		assertRoundTrip(t, "1.5M")
	})

	t.Run("P1_1_arrayjson_key_escape_test", func(t *testing.T) {
		engine := newEngine()
		// P1-1 回归: 含引号/空格的 key 必须按字符串字面量转义输出
		f, err := engine.Parse(`{"a\"b":1}`)
		assertTrue(t, err == nil && f != nil, "Parse 失败: %v", err)
		assertText(t, `{"a\"b":1}`, alg.ToString(f))
		assertRoundTrip(t, `{"a\"b":1}`)

		f, err = engine.Parse(`{"a b":1}`)
		assertTrue(t, err == nil && f != nil, "Parse 失败: %v", err)
		assertText(t, `{"a b":1}`, alg.ToString(f))
		assertRoundTrip(t, `{"a b":1}`)

		// 含换行转义的 key
		assertRoundTrip(t, `{"a\nb":1}`)

		// 单引号字面量同样解转义，输出统一规范化为双引号
		f, err = engine.Parse(`{'a\'b':1}`)
		assertTrue(t, err == nil && f != nil, "Parse 失败: %v", err)
		assertText(t, `{"a'b":1}`, alg.ToString(f))
	})

	t.Run("P1_2_diyfunction_error_propagation_test", func(t *testing.T) {
		// P1-2 回归: 自定义函数的参数为错误操作数时必须在引擎内被拦截
		engine := newCylinder(10, 15)
		r := evalOperand(engine.AlgorithmEngine, "求面积(1/0)")
		assertBool(t, true, r.IsError())
		assertText(t, "Function '/' Div 0 error!", r.ErrorMsg())

		// 溢出后 TryEvaluate 返回默认值，并填充 LastError
		assertFloatRound(t, -1.0, evalFloatDef(engine.AlgorithmEngine, "求面积(1/0)", -1.0), 10)
		assertLastError(t, engine.AlgorithmEngine)

		// 正常参数不受影响
		r = evalOperand(engine.AlgorithmEngine, "求面积(2)")
		assertBool(t, false, r.IsError())
		assertFloat(t, 4*math.Pi, r.NumberValue().Float64())
	})

	t.Run("P1_3_string_unicode_escape_test", func(t *testing.T) {
		engine := newEngine()
		// P1-3 回归: \uXXXX 十六进制转义必须被解析
		r := evalOperand(engine, `"a\u0041b"`)
		assertText(t, "aAb", r.TextValue())

		// 控制字符往返
		assertRoundTrip(t, `"a\u000bb"`)
		assertRoundTrip(t, `"a\u0007b"`)

		// 不完整转义按原样保留，不吞字符
		r = evalOperand(engine, `"a\ub"`)
		assertText(t, "aub", r.TextValue())
	})

	t.Run("Visitors_P1_1_num_unit_split_test", func(t *testing.T) {
		engine := newEngine()
		// Visitors P1-1 回归: 超出 decimal 范围的指数记法必须在 Parse 阶段失败
		for _, exp := range []string{"1E29", "12E29", "1e29", "1E+30", "1E300"} {
			_, err := engine.Parse(exp)
			assertTrue(t, err != nil, "Visitors P1-1 回归失败: [%s] 应在 Parse 阶段报错", exp)
			assertFalse(t, strings.Contains(err.Error(), "unit"),
				"Visitors P1-1 回归失败: [%s] 报出单位错误而非数值越界: %v", exp, err)
			assertNull(t, engine.ParseWithoutError(exp))
		}

		// 范围内的指数记法走数值分支
		r := evalOperand(engine, "1E2")
		assertBool(t, false, r.IsError())
		assertDecimal(t, "100", r.NumberValue())
		r = evalOperand(engine, "1.5e2")
		assertBool(t, false, r.IsError())
		assertDecimal(t, "150", r.NumberValue())

		// 长度 1/2/3 的单位后缀都必须按完整后缀切分
		exps := []string{"1KM2", "1M2", "1ML", "1KG", "1T", "1L"}
		for _, exp := range exps {
			f, err := engine.Parse(exp)
			assertTrue(t, err == nil && f != nil, "Parse %s 失败: %v", exp, err)
			assertText(t, exp, alg.ToString(f))
		}

		// 单位数值语义不变，大小写单位等价
		assertDecimal(t, "1000", evalOperand(engine, "1KM").NumberValue())
		assertDecimal(t, "1000", evalOperand(engine, "1km").NumberValue())
		assertDecimal(t, "1234", evalOperand(engine, "1234M").NumberValue())
		assertDecimal(t, "12", evalOperand(engine, "12M").NumberValue())
		assertDecimal(t, "1", evalOperand(engine, "1m").NumberValue())

		assertRoundTrip(t, "1KM2")
		assertRoundTrip(t, "1ML")
		assertRoundTrip(t, "12M")
		assertRoundTrip(t, "1E2")
	})

	t.Run("Visitors_P1_2_function_branch_split_test", func(t *testing.T) {
		// Visitors P1-2 回归: VisitFunction_fun 的 PARAMETER(自定义函数) 分支拆分后行为必须不变
		engine := newCylinder(10, 15)
		r := evalOperand(engine.AlgorithmEngine, "求面积(2)")
		assertBool(t, false, r.IsError())
		assertFloat(t, 4*math.Pi, r.NumberValue().Float64())

		// 自定义函数与内建函数混用时各走各的分支
		r = evalOperand(engine.AlgorithmEngine, "求面积(2) + ABS(-1)")
		assertBool(t, false, r.IsError())
		assertFloat(t, 4*math.Pi+1, r.NumberValue().Float64())

		// 未注册的自定义函数在调用时给出明确错误
		engine2 := newEngine()
		r = evalOperand(engine2, "求面积(2)")
		assertBool(t, true, r.IsError())
		assertText(t, "DiyFunction [求面积] is missing.", r.ErrorMsg())

		// 自定义函数名参与往返
		assertRoundTrip(t, "求面积(2)")
	})
}

// newCylinder 对应 C# 测试辅助类 Cylinder(radius, height)：
// 自定义参数「半径 / 直径 / 高」与自定义函数「求面积」。
func newCylinder(radius, height int) *alg.AlgorithmEngineEx {
	e := alg.NewAlgorithmEngineEx(false)
	e.GetParameterExFunc = func(name string) alg.Operand {
		switch name {
		case "半径":
			return alg.CreateInt(radius)
		case "直径":
			return alg.CreateInt(radius * 2)
		case "高":
			return alg.CreateInt(height)
		}
		// 对应 C# Cylinder 回退到 base.GetParameter(parameter)
		return alg.Errorf("Parameter [{0}] is missing.", name)
	}
	e.ExecuteDiyFunctionFunc = func(name string, args []alg.Operand) alg.Operand {
		if name == "求面积" && len(args) == 1 {
			r := args[0].ToNumber("").NumberValue().Float64()
			return alg.CreateFloat(r * r * math.Pi)
		}
		return alg.Errorf("DiyFunction [{0}] is missing.", name)
	}
	return e
}

// assertRoundTrip 对应 C# 的 AssertRoundTrip：
// Parse(exp) -> ToString -> 重新 Parse 求值，要求还原前后语义完全一致。
func assertRoundTrip(t *testing.T, exp string) {
	t.Helper()
	engine1 := newEngine()
	function, err := engine1.Parse(exp)
	if err != nil || function == nil {
		t.Fatalf("往返测试失败: [%s] 解析失败: %v", exp, err)
	}
	text := alg.ToString(function)
	result1 := engine1.EvaluateSafe(function)

	engine2 := newEngine()
	function2, err2 := engine2.Parse(text)
	if err2 != nil || function2 == nil {
		t.Fatalf("往返测试失败: [%s] 还原为 [%s] 后重新求值抛出: %v", exp, text, err2)
	}
	result2 := engine2.EvaluateSafe(function2)

	if result1.IsError() != result2.IsError() || result1.String() != result2.String() {
		t.Fatalf("往返测试失败: [%s] 还原为 [%s]，原结果=[%s](IsError=%v)，往返结果=[%s](IsError=%v)",
			exp, text, result1.String(), result1.IsError(), result2.String(), result2.IsError())
	}
}
