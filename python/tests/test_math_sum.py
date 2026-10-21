"""Port of ``MathSumTest.java``."""
from __future__ import annotations

import pytest

from toolgood_algorithm import AlgorithmEngine


def test_max():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("max(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(4.0, abs=0.0)


def test_median():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MEDIAN(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)

    t = engine.try_evaluate("MEDIAN(1,1,2,2)", 0.0)
    assert t == pytest.approx(1.5, abs=1e-6)


def test_min():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MIN(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)


def test_quartile():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("QUARTILE(array(1,2,3,4,2,2,1,4),0)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)
    t = engine.try_evaluate("QUARTILE(array(1,2,3,4,2,2,1,4),1)", 0.0)
    assert t == pytest.approx(1.75, abs=1e-6)
    t = engine.try_evaluate("QUARTILE(array(1,2,3,4,2,2,1,4),2)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)
    t = engine.try_evaluate("QUARTILE(array(1,2,3,4,2,2,1,4),3)", 0.0)
    assert t == pytest.approx(3.25, abs=1e-6)
    t = engine.try_evaluate("QUARTILE(array(1,2,3,4,2,2,1,4),4)", 0.0)
    assert t == pytest.approx(4.0, abs=0.0)


def test_mode():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MODE(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)


def test_percentile():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PERCENTILE(array(1,2,3,4,2,2,1,4)，0.4)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)


def test_percentrank():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4)，3)", 0.0)
    assert t == pytest.approx(0.714, abs=1e-6)


def test_average():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(2.375, abs=1e-6)


def test_geomean():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("GEOMEAN(1,2,3,4)", 0.0)
    assert t == pytest.approx(2.213363839, abs=1e-6)


def test_harmean():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("HARMEAN(1,2,3,4)", 0.0)
    assert t == pytest.approx(1.92, abs=1e-6)


def test_count():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("COUNT(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(8.0, abs=0.0)


def test_avedev():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("AVEDEV(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(0.96875, abs=1e-6)


def test_stdev():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("STDEV(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(1.187734939, abs=1e-6)


def test_stdevp():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("STDEVP(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(1.111024302, abs=1e-6)


def test_devsq():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("DEVSQ(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(9.875, abs=1e-6)


def test_var():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("VAR(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(1.410714286, abs=1e-6)


def test_varp():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("VARP(1,2,3,4,2,2,1,4)", 0.0)
    assert t == pytest.approx(1.234375, abs=1e-6)


def test_rank():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("RANK(3, array(1, 2, 3, 4, 5))", 0)
    assert t == 3

    t = engine.try_evaluate("RANK(3, array(1, 2, 3, 4, 5), 0)", 0)
    assert t == 3

    t = engine.try_evaluate("RANK(3, array(1, 2, 3, 4, 5), 1)", 0)
    assert t == 3

    t = engine.try_evaluate("RANK(5, array(1, 2, 3, 4, 5))", 0)
    assert t == 1

    t = engine.try_evaluate("RANK(1, array(1, 2, 3, 4, 5))", 0)
    assert t == 5


def test_forecast():
    engine = AlgorithmEngine()
    t = engine.try_evaluate(
        "FORECAST(30, array(6, 7, 9, 15, 21), array(20, 28, 31, 38, 40))", 0.0
    )
    assert t == pytest.approx(10.6073, abs=1e-4)


def test_intercept():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("INTERCEPT(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0)
    assert t == pytest.approx(0.0484, abs=1e-4)


def test_slope():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SLOPE(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0)
    assert t == pytest.approx(0.6694, abs=1e-4)


def test_correl():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("CORREL(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0)
    assert t == pytest.approx(0.9971, abs=1e-4)


def test_pearson():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PEARSON(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0)
    assert t == pytest.approx(0.9971, abs=1e-4)


def test_large():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LARGE(array(1,2,3,4,2,2,1,4),3)", 0.0)
    assert t == pytest.approx(3.0, abs=1e-6)


def test_small():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SMALL(array(1,2,3,4,2,2,1,4),3)", 0.0)
    assert t == pytest.approx(2.0, abs=1e-6)


def test_covar():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("COVAR(array(3,7,6,11),array(5,15,13,9))", 0.0)
    t1 = engine.try_evaluate("COVARIANCE.P(array(3,7,6,11),array(5,15,13,9))", 0.0)
    assert t == pytest.approx(3.375, abs=1e-6)
    assert t1 == pytest.approx(3.375, abs=1e-6)


def test_covariances():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("COVARIANCE.S(array(3,7,6,11),array(5,15,13,9))", 0.0)
    assert t == pytest.approx(4.5, abs=1e-6)


def test_countif():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0)
    assert t == pytest.approx(6.0, abs=0.0)
    t = engine.try_evaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>=1')", 0.0)
    assert t == pytest.approx(8.0, abs=0.0)
    t = engine.try_evaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'!=1')", 0.0)
    assert t == pytest.approx(6.0, abs=0.0)
    t = engine.try_evaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'=1')", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)
    t = engine.try_evaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<=1')", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)
    t = engine.try_evaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<2')", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)


def test_countif_boolean_criteria():
    engine = AlgorithmEngine()
    # 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 UnsupportedOperationException)
    t = engine.try_evaluate("COUNTIF(array(1,2,3), true)", 0.0)
    assert engine.LastError is not None


def test_sumif():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0)
    assert t == pytest.approx(17.0, abs=0.0)
    t = engine.try_evaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0)
    assert t == pytest.approx(6.0, abs=0.0)
    t = engine.try_evaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>=2',array(1,1,1,1,1,1,1,1))", 0.0)
    assert t == pytest.approx(6.0, abs=0.0)
    t = engine.try_evaluate("SUMIF(array(1,2,3,4,2,2,1,4),'<1',array(1,1,1,1,1,1,1,1))", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)
    t = engine.try_evaluate("SUMIF(array(1,2,3,4,2,2,1,4),'==1',array(1,1,1,1,1,1,1,1))", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)


def test_sumif_boolean_criteria():
    engine = AlgorithmEngine()
    # 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 UnsupportedOperationException)
    t = engine.try_evaluate("SUMIF(array(1,2,3), true)", 0.0)
    assert engine.LastError is not None


def test_averageif():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0)
    assert t == pytest.approx(2.833333333, abs=1e-6)

    t = engine.try_evaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0)
    assert t == pytest.approx(1.0, abs=1e-6)


def test_averageif_boolean_criteria():
    engine = AlgorithmEngine()
    # 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 UnsupportedOperationException)
    t = engine.try_evaluate("AVERAGEIF(array(1,2,3), true)", 0.0)
    assert engine.LastError is not None


def test_seriessum():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SERIESSUM(2, 0, 1, array(1, 1, 1, 1))", 0.0)
    assert t == pytest.approx(15.0, abs=0.0)


def test_seriessum_non_numeric_coefficient():
    engine = AlgorithmEngine()
    # 系数数组含非数字元素时跳过但保持索引位置,避免后续指数 n + i*m 错位
    # 2*2^0 + 3*2^2 = 2 + 12 = 14(修复前会算成 2*2^0 + 3*2^1 = 8)
    t = engine.try_evaluate("SERIESSUM(2, 0, 1, array(2, 'x', 3))", 0.0)
    assert t == pytest.approx(14.0, abs=0.0)


def test_sumproduct():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SUMPRODUCT(array(1, 2, 3), array(4, 5, 6))", 0.0)
    assert t == pytest.approx(32.0, abs=0.0)


def test_sumproduct_single_array():
    engine = AlgorithmEngine()
    # 单数组参数等价于求和(修复前被误判为参数错误)
    t = engine.try_evaluate("SUMPRODUCT(array(1, 2, 3))", 0.0)
    assert t == pytest.approx(6.0, abs=0.0)


def test_sumx2my2():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SUMX2MY2(array(1, 2, 3), array(4, 5, 6))", 0.0)
    assert t == pytest.approx(-63.0, abs=0.0)


def test_sumx2py2():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SUMX2PY2(array(1, 2, 3), array(4, 5, 6))", 0.0)
    assert t == pytest.approx(91.0, abs=0.0)


def test_sumxmy2():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SUMXMY2(array(1, 2, 3), array(4, 5, 6))", 0.0)
    assert t == pytest.approx(27.0, abs=0.0)


def test_percentrank_significance():
    engine = AlgorithmEngine()
    # significance 指定保留小数位数(原始值 5/7≈0.7142857)
    t = engine.try_evaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,4)", 0.0)
    assert t == pytest.approx(0.7143, abs=1e-9)

    # 28 为允许的最大位数
    t = engine.try_evaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,28)", 0.0)
    assert t == pytest.approx(5.0 / 7.0, abs=1e-12)

    # 超过 28 报错(修复前抛 ArgumentOutOfRangeException)
    t = engine.try_evaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,29)", 0.0)
    assert engine.LastError is not None

    # 负数位数报错
    t = engine.try_evaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,-1)", 0.0)
    assert engine.LastError is not None
