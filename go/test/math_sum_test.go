package test

import "testing"

// TestMathSumTest 对应 C# ToolGood.Algorithm.Test.MathSum.MathSumTest。
func TestMathSumTest(t *testing.T) {
	t.Run("MAX_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 4.0, evalFloat(e, "max(1,2,3,4,2,2,1,4)"))
	})

	t.Run("MEDIAN_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 2.0, evalFloat(e, "MEDIAN(1,2,3,4,2,2,1,4)"))
		assertFloat(t, 1.5, evalFloat(e, "MEDIAN(1,1,2,2)"))
	})

	t.Run("MIN_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.0, evalFloat(e, "MIN(1,2,3,4,2,2,1,4)"))
	})

	t.Run("QUARTILE_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.0, evalFloat(e, "QUARTILE(array(1,2,3,4,2,2,1,4),0)"))
		assertFloat(t, 1.75, evalFloat(e, "QUARTILE(array(1,2,3,4,2,2,1,4),1)"))
		assertFloat(t, 2.0, evalFloat(e, "QUARTILE(array(1,2,3,4,2,2,1,4),2)"))
		assertFloat(t, 3.25, evalFloat(e, "QUARTILE(array(1,2,3,4,2,2,1,4),3)"))
		assertFloat(t, 4.0, evalFloat(e, "QUARTILE(array(1,2,3,4,2,2,1,4),4)"))
	})

	t.Run("MODE_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 2.0, evalFloat(e, "MODE(1,2,3,4,2,2,1,4)"))
	})

	t.Run("PERCENTILE_test", func(t *testing.T) {
		e := newEngine()
		// 原始用例使用全角逗号，验证词法标准化的全角转半角
		assertFloat(t, 2.0, evalFloat(e, "PERCENTILE(array(1,2,3,4,2,2,1,4)，0.4)"))
	})

	t.Run("PERCENTRANK_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 0.714, evalFloat(e, "PERCENTRANK(array(1,2,3,4,2,2,1,4)，3)"))
	})

	t.Run("PERCENTRANK_significance_test", func(t *testing.T) {
		e := newEngine()
		// significance 指定保留小数位数(原始值 5/7≈0.7142857)
		assertFloat(t, 0.7143, evalFloat(e, "PERCENTRANK(array(1,2,3,4,2,2,1,4),3,4)"))

		// 28 为允许的最大位数
		assertFloat(t, 5.0/7.0, evalFloat(e, "PERCENTRANK(array(1,2,3,4,2,2,1,4),3,28)"))

		// 超过 28 报错(Math.Round(decimal,int) 上限,修复前会抛 ArgumentOutOfRangeException)
		evalFloat(e, "PERCENTRANK(array(1,2,3,4,2,2,1,4),3,29)")
		assertLastError(t, e)

		// 负数位数报错
		evalFloat(e, "PERCENTRANK(array(1,2,3,4,2,2,1,4),3,-1)")
		assertLastError(t, e)
	})

	t.Run("AVERAGE_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 2.375, evalFloat(e, "AVERAGE(1,2,3,4,2,2,1,4)"))
	})

	t.Run("GEOMEAN_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 2.213363839, evalFloat(e, "GEOMEAN(1,2,3,4)"), 6)
	})

	t.Run("HARMEAN_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.92, evalFloat(e, "HARMEAN(1,2,3,4)"), 6)
	})

	t.Run("COUNT_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 8.0, evalFloat(e, "COUNT(1,2,3,4,2,2,1,4)"))
	})

	t.Run("AVEDEV_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 0.96875, evalFloat(e, "AVEDEV(1,2,3,4,2,2,1,4)"))
	})

	t.Run("STDEV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.187734939, evalFloat(e, "STDEV(1,2,3,4,2,2,1,4)"), 6)
	})

	t.Run("STDEVP_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.111024302, evalFloat(e, "STDEVP(1,2,3,4,2,2,1,4)"), 6)
	})

	t.Run("DEVSQ_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 9.875, evalFloat(e, "DEVSQ(1,2,3,4,2,2,1,4)"), 6)
	})

	t.Run("VAR_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.410714286, evalFloat(e, "VAR(1,2,3,4,2,2,1,4)"), 6)
	})

	t.Run("VARP_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.234375, evalFloat(e, "VARP(1,2,3,4,2,2,1,4)"), 6)
	})

	t.Run("RANK_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 3, evalInt(e, "RANK(3, array(1, 2, 3, 4, 5))"))
		assertInt(t, 3, evalInt(e, "RANK(3, array(1, 2, 3, 4, 5), 0)"))
		assertInt(t, 3, evalInt(e, "RANK(3, array(1, 2, 3, 4, 5), 1)"))
		assertInt(t, 1, evalInt(e, "RANK(5, array(1, 2, 3, 4, 5))"))
		assertInt(t, 5, evalInt(e, "RANK(1, array(1, 2, 3, 4, 5))"))
	})

	t.Run("FORECAST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 10.6073, evalFloat(e, "FORECAST(30, array(6, 7, 9, 15, 21), array(20, 28, 31, 38, 40))"), 4)
	})

	t.Run("INTERCEPT_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.0484, evalFloat(e, "INTERCEPT(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))"), 4)
	})

	t.Run("SLOPE_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.6694, evalFloat(e, "SLOPE(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))"), 4)
	})

	t.Run("CORREL_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.9971, evalFloat(e, "CORREL(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))"), 4)
	})

	t.Run("PEARSON_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.9971, evalFloat(e, "PEARSON(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))"), 4)
	})

	t.Run("LARGE_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 3.0, evalFloat(e, "LARGE(array(1,2,3,4,2,2,1,4),3)"), 6)
	})

	t.Run("SMALL_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 2.0, evalFloat(e, "SMALL(array(1,2,3,4,2,2,1,4),3)"), 6)
	})

	t.Run("COVAR_test", func(t *testing.T) {
		e := newEngine()
		got := evalFloat(e, "COVAR(array(3,7,6,11),array(5,15,13,9))")
		got1 := evalFloat(e, "COVARIANCE.P(array(3,7,6,11),array(5,15,13,9))")
		assertFloatRound(t, 3.375, got, 6)
		assertFloatRound(t, 3.375, got1, 6)
	})

	t.Run("COVARIANCES_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 4.5, evalFloat(e, "COVARIANCE.S(array(3,7,6,11),array(5,15,13,9))"), 6)
	})

	t.Run("COUNTIF_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 6.0, evalFloat(e, "COUNTIF(array(1,2,3,4,2,2,1,4),'>1')"))
		assertFloat(t, 8.0, evalFloat(e, "COUNTIF(array(1,2,3,4,2,2,1,4),'>=1')"))
		assertFloat(t, 6.0, evalFloat(e, "COUNTIF(array(1,2,3,4,2,2,1,4),'!=1')"))
		assertFloat(t, 2.0, evalFloat(e, "COUNTIF(array(1,2,3,4,2,2,1,4),'=1')"))
		assertFloat(t, 2.0, evalFloat(e, "COUNTIF(array(1,2,3,4,2,2,1,4),'<=1')"))
		assertFloat(t, 2.0, evalFloat(e, "COUNTIF(array(1,2,3,4,2,2,1,4),'<2')"))
	})

	t.Run("COUNTIF_boolean_criteria_test", func(t *testing.T) {
		e := newEngine()
		// 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
		evalFloat(e, "COUNTIF(array(1,2,3), true)")
		assertLastError(t, e)
	})

	t.Run("SUMIF_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 17, evalFloat(e, "SUMIF(array(1,2,3,4,2,2,1,4),'>1')"))
		assertFloat(t, 6, evalFloat(e, "SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))"))
		assertFloat(t, 6, evalFloat(e, "SUMIF(array(1,2,3,4,2,2,1,4),'>=2',array(1,1,1,1,1,1,1,1))"))
		assertFloat(t, 0, evalFloat(e, "SUMIF(array(1,2,3,4,2,2,1,4),'<1',array(1,1,1,1,1,1,1,1))"))
		assertFloat(t, 2, evalFloat(e, "SUMIF(array(1,2,3,4,2,2,1,4),'==1',array(1,1,1,1,1,1,1,1))"))
	})

	t.Run("SUMIF_boolean_criteria_test", func(t *testing.T) {
		e := newEngine()
		// 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
		evalFloat(e, "SUMIF(array(1,2,3), true)")
		assertLastError(t, e)
	})

	t.Run("AVERAGEIF_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 2.833333333, evalFloat(e, "AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1')"), 6)
		assertFloat(t, 1, evalFloat(e, "AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))"))
	})

	t.Run("AVERAGEIF_boolean_criteria_test", func(t *testing.T) {
		e := newEngine()
		// 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
		evalFloat(e, "AVERAGEIF(array(1,2,3), true)")
		assertLastError(t, e)
	})

	t.Run("SERIESSUM_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 15.0, evalFloat(e, "SERIESSUM(2, 0, 1, array(1, 1, 1, 1))"))
	})

	t.Run("SERIESSUM_non_numeric_coefficient_test", func(t *testing.T) {
		e := newEngine()
		// 系数数组含非数字元素时跳过但保持索引位置,避免后续指数 n + i*m 错位
		// 2*2^0 + 3*2^2 = 2 + 12 = 14(修复前会算成 2*2^0 + 3*2^1 = 8)
		assertFloat(t, 14.0, evalFloat(e, "SERIESSUM(2, 0, 1, array(2, 'x', 3))"))
	})

	t.Run("SUMPRODUCT_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 32.0, evalFloat(e, "SUMPRODUCT(array(1, 2, 3), array(4, 5, 6))"))
	})

	t.Run("SUMPRODUCT_single_array_test", func(t *testing.T) {
		e := newEngine()
		// 单数组参数等价于求和(修复前被误判为参数错误)
		assertFloat(t, 6.0, evalFloat(e, "SUMPRODUCT(array(1, 2, 3))"))
	})

	t.Run("SUMX2MY2_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, -63.0, evalFloat(e, "SUMX2MY2(array(1, 2, 3), array(4, 5, 6))"))
	})

	t.Run("SUMX2PY2_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 91.0, evalFloat(e, "SUMX2PY2(array(1, 2, 3), array(4, 5, 6))"))
	})

	t.Run("SUMXMY2_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 27.0, evalFloat(e, "SUMXMY2(array(1, 2, 3), array(4, 5, 6))"))
	})
}
