package test

import (
	"testing"
)

// transformationPlacesError 等价 C# MathTransformationTest.PlacesError：求值后只要有错误即返回 true。
func transformationPlacesError(formula string) bool {
	e := newEngine()
	evalText(e, formula)
	return e.LastError != ""
}

// assertTransformationError 断言表达式返回错误（对应 C# Assert.IsTrue(PlacesError(...))）。
func assertTransformationError(t *testing.T, formula string) {
	t.Helper()
	assertTrue(t, transformationPlacesError(formula), "期望 %s 返回错误，实际无错误", formula)
}

// TestMathTransformationTest 对应 C# ToolGood.Algorithm.Test.MathTransformationTest。
func TestMathTransformationTest(t *testing.T) {
	t.Run("BIN2DEC_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 21, evalInt(e, "BIN2DEC(10101)"))
	})

	t.Run("OCT2DEC_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 5422, evalInt(e, "OCT2DEC(12456)"))
	})

	t.Run("HEX2DEC_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 2177759, evalInt(e, "HEX2DEC('213adf')"))
	})

	t.Run("DEC2BIN_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "1010", evalText(e, "DEC2BIN(10)"))
		assertText(t, "00001010", evalText(e, "DEC2BIN(10,8)"))
	})

	t.Run("OCT2BIN_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "111010001", evalText(e, "OCT2BIN('721')"))
	})

	t.Run("HEX2BIN_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "11111010", evalText(e, "HEX2BIN('fa')"))
	})

	t.Run("BIN2OCT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "2", evalText(e, "BIN2OCT(10)"))
	})

	t.Run("DEC2OCT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "113", evalText(e, "DEC2OCT('75')"))
	})

	t.Run("HEX2OCT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "365", evalText(e, "HEX2OCT('f5')"))
	})

	t.Run("BIN2HEX_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "154", evalText(e, "BIN2HEX(101010100)"))
	})

	t.Run("OCT2HEX_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "7A8A", evalText(e, "OCT2HEX(75212)"))
	})

	t.Run("DEC2HEX_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "3B8", evalText(e, "DEC2HEX(952)"))
	})

	t.Run("ARABIC_test", func(t *testing.T) {
		e := newEngine()
		cases := []struct {
			exp  string
			want int
		}{
			{"ARABIC('I')", 1},
			{"ARABIC('IV')", 4},
			{"ARABIC('IX')", 9},
			{"ARABIC('X')", 10},
			{"ARABIC('XL')", 40},
			{"ARABIC('L')", 50},
			{"ARABIC('XC')", 90},
			{"ARABIC('C')", 100},
			{"ARABIC('CD')", 400},
			{"ARABIC('D')", 500},
			{"ARABIC('CM')", 900},
			{"ARABIC('M')", 1000},
			{"ARABIC('MMXXIII')", 2023},
		}
		for _, c := range cases {
			assertInt(t, c.want, evalInt(e, c.exp))
		}
	})

	t.Run("ROMAN_test", func(t *testing.T) {
		e := newEngine()
		cases := []struct {
			exp  string
			want string
		}{
			{"ROMAN(1)", "I"},
			{"ROMAN(4)", "IV"},
			{"ROMAN(9)", "IX"},
			{"ROMAN(10)", "X"},
			{"ROMAN(40)", "XL"},
			{"ROMAN(50)", "L"},
			{"ROMAN(90)", "XC"},
			{"ROMAN(100)", "C"},
			{"ROMAN(400)", "CD"},
			{"ROMAN(500)", "D"},
			{"ROMAN(900)", "CM"},
			{"ROMAN(1000)", "M"},
			{"ROMAN(2023)", "MMXXIII"},
		}
		for _, c := range cases {
			assertText(t, c.want, evalText(e, c.exp))
		}
	})

	t.Run("ARABIC_error_test", func(t *testing.T) {
		e := newEngine()
		// 空文本报错（Excel 返回 #VALUE!）
		evalInt(e, "ARABIC('')")
		assertLastError(t, e)

		// 非法字符报错
		e = newEngine()
		evalInt(e, "ARABIC('ABC')")
		assertLastError(t, e)

		// 支持小写
		e = newEngine()
		assertInt(t, 1900, evalInt(e, "ARABIC('mcm')"))

		// Excel 官方例子：ARABIC("MCMLXXII") = 1972
		assertInt(t, 1972, evalInt(e, "ARABIC('MCMLXXII')"))
	})

	t.Run("ROMAN_form_test", func(t *testing.T) {
		e := newEngine()
		// form 0~4 使用不同的简化规则
		assertText(t, "CDXCIX", evalText(e, "ROMAN(499)"))
		assertText(t, "LDVLIV", evalText(e, "ROMAN(499,1)"))
		assertText(t, "XDIX", evalText(e, "ROMAN(499,2)"))
		assertText(t, "VDIV", evalText(e, "ROMAN(499,3)"))
		assertText(t, "ID", evalText(e, "ROMAN(499,4)"))

		// 0 返回空文本
		assertText(t, "", evalTextDef(e, "ROMAN(0)", "x"))

		// 负数与大于 3999 报错
		e = newEngine()
		evalText(e, "ROMAN(-1)")
		assertLastError(t, e)

		e = newEngine()
		evalText(e, "ROMAN(4000)")
		assertLastError(t, e)

		// form 超出 0~4 报错
		e = newEngine()
		evalText(e, "ROMAN(499,5)")
		assertLastError(t, e)

		e = newEngine()
		evalText(e, "ROMAN(499,-1)")
		assertLastError(t, e)
	})

	t.Run("ARABIC_invalid_test", func(t *testing.T) {
		// 非法重复：IIII 不是标准罗马数字
		e := newEngine()
		evalInt(e, "ARABIC('IIII')")
		assertLastError(t, e)

		// 非法减法：IC 不是标准罗马数字
		e = newEngine()
		evalInt(e, "ARABIC('IC')")
		assertLastError(t, e)

		// 非法重复：VV 不是标准罗马数字
		e = newEngine()
		evalInt(e, "ARABIC('VV')")
		assertLastError(t, e)
	})

	t.Run("places_out_of_range_test", func(t *testing.T) {
		// places 超过 10 应返回 #NUM!
		assertTransformationError(t, "DEC2BIN(10,20)")
		assertTransformationError(t, "DEC2HEX(952,20)")
		assertTransformationError(t, "DEC2OCT(75,20)")
		assertTransformationError(t, "BIN2OCT(10,20)")
		assertTransformationError(t, "BIN2HEX(101010100,20)")
		assertTransformationError(t, "OCT2BIN(721,20)")
		assertTransformationError(t, "OCT2HEX(75212,20)")
		assertTransformationError(t, "HEX2BIN('fa',20)")
		assertTransformationError(t, "HEX2OCT('f5',20)")
	})

	t.Run("negative_places_padding_test", func(t *testing.T) {
		e := newEngine()
		// 10 位补码解析为 -1，places=3 时应输出 "-01" 而非 "0-1"
		assertText(t, "-01", evalText(e, "BIN2DEC('1111111111',3)"))
		assertText(t, "-01", evalText(e, "OCT2DEC('7777777777',3)"))
		assertText(t, "-01", evalText(e, "HEX2DEC('FFFFFFFFFF',3)"))
	})

	t.Run("dec_from_base_places_test", func(t *testing.T) {
		// *2DEC 的 places 超出 0~10 应返回 #NUM!
		assertTransformationError(t, "BIN2DEC('1010',20)")
		assertTransformationError(t, "OCT2DEC('12',20)")
		assertTransformationError(t, "HEX2DEC('A',20)")
		// places 不足以容纳结果时应返回 #NUM!
		assertTransformationError(t, "BIN2DEC('1010',1)")

		// places 充足时左侧补零
		assertText(t, "000010", evalText(newEngine(), "BIN2DEC('1010',6)"))
	})

	t.Run("trim_blank_test", func(t *testing.T) {
		// 首尾空白应先被裁剪
		assertInt(t, 10, evalInt(newEngine(), "BIN2DEC(' 1010 ')"))
		assertInt(t, 255, evalInt(newEngine(), "HEX2DEC('  FF ')"))
		assertInt(t, 4, evalInt(newEngine(), "ARABIC(' iv ')"))
	})

	t.Run("int_overflow_no_exception_test", func(t *testing.T) {
		// number/places 超出 int 范围时应返回错误，而不是抛出溢出异常
		assertTransformationError(t, "DEC2BIN(5000000000)")
		assertTransformationError(t, "ROMAN(5000000000)")
		assertTransformationError(t, "ROMAN(499,5000000000)")

		assertTransformationError(t, "DEC2BIN(10,5000000000)")
		assertTransformationError(t, "DEC2HEX(952,5000000000)")
		assertTransformationError(t, "DEC2OCT(75,5000000000)")
		assertTransformationError(t, "BIN2OCT(10,5000000000)")
		assertTransformationError(t, "BIN2HEX('101010100',5000000000)")
		assertTransformationError(t, "OCT2BIN('721',5000000000)")
		assertTransformationError(t, "OCT2HEX('75212',5000000000)")
		assertTransformationError(t, "HEX2BIN('fa',5000000000)")
		assertTransformationError(t, "HEX2OCT('f5',5000000000)")
		assertTransformationError(t, "BIN2DEC('1010',5000000000)")
		assertTransformationError(t, "OCT2DEC('12',5000000000)")
		assertTransformationError(t, "HEX2DEC('A',5000000000)")
	})

	t.Run("negative_ignores_places_test", func(t *testing.T) {
		// 负数结果固定为 10 位补码，应忽略 places（即使 places 小于结果位数）
		assertText(t, "1111111111", evalText(newEngine(), "DEC2BIN(-1,4)"))
		assertText(t, "FFFFFFFFF7", evalText(newEngine(), "DEC2HEX(-9,4)"))
		assertText(t, "7777777777", evalText(newEngine(), "DEC2OCT(-1,4)"))

		assertText(t, "FFFFFFFFFF", evalText(newEngine(), "BIN2HEX('1111111111',4)"))
		assertText(t, "7777777777", evalText(newEngine(), "BIN2OCT('1111111111',4)"))
		assertText(t, "1111111111", evalText(newEngine(), "OCT2BIN('7777777777',4)"))
		assertText(t, "FFFFFFFFFF", evalText(newEngine(), "OCT2HEX('7777777777',4)"))
		assertText(t, "1111111111", evalText(newEngine(), "HEX2BIN('FFFFFFFFFF',4)"))
		assertText(t, "7777777777", evalText(newEngine(), "HEX2OCT('FFFFFFFFFF',4)"))

		// number 为负时 places 非法仍应返回 #NUM!（校验先于负数分支）
		assertTransformationError(t, "DEC2BIN(-1,-1)")
		assertTransformationError(t, "DEC2HEX(-1,-1)")
		assertTransformationError(t, "DEC2OCT(-1,-1)")
		assertTransformationError(t, "BIN2HEX('1111111111',-1)")
	})

	t.Run("dec_truncate_range_test", func(t *testing.T) {
		// 小数先截断再校验范围：-549755813888.5 截断后仍在范围内
		assertText(t, "8000000000", evalText(newEngine(), "DEC2HEX(-549755813888.5)"))
		assertText(t, "4000000000", evalText(newEngine(), "DEC2OCT(-536870912.5)"))

		// 截断后越界应返回 #NUM!
		assertTransformationError(t, "DEC2HEX(-549755813889)")
		assertTransformationError(t, "DEC2OCT(-536870913)")
		assertTransformationError(t, "DEC2HEX(549755813888)")
		assertTransformationError(t, "DEC2OCT(536870912)")
	})

	t.Run("roman_boolean_form_test", func(t *testing.T) {
		// Excel：form 为 TRUE 时使用古典形式（等价于 0），为 FALSE 时使用最简化形式（等价于 4）
		assertText(t, "CDXCIX", evalText(newEngine(), "ROMAN(499,TRUE())"))
		assertText(t, "ID", evalText(newEngine(), "ROMAN(499,FALSE())"))
	})
}
