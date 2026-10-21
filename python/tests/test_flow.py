"""Tests ported from ``java/.../AlgorithmEngine/FlowTest.java``.

One Java ``@Test`` maps to one pytest function (snake_case).
"""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine


def test_if():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("if(1=1,1,2)", 0)
    assert t == 1
    t = engine.try_evaluate("if(1=1,1)", 0)
    assert t == 1

    t = engine.try_evaluate("if(1=1，1)", 0)
    assert t == 1

    t = engine.try_evaluate("if('1',1,2)", 0)
    assert t == 1
    t = engine.try_evaluate("if(0,1,2)", 0)
    assert t == 2


def test_iferror():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("iferror(1/0,1,2)", 0)
    assert t == 1

    t = engine.try_evaluate("iferror(1-'rrr',1,2)", 0)
    assert t == 1


def test_iserror():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("iserror(1/0,1)", 0)
    assert t == 1

    t = engine.try_evaluate("iserror(1-'rrr',1)", 0)
    assert t == 1


def test_ifnull():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("isnull(null,1)", 0)
    assert t == 1

    t = engine.try_evaluate("isnull(1,2)", 0)
    assert t == 1


def test_isnullorerror():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("isnullorerror(null,1)", 0)
    assert t == 1

    t = engine.try_evaluate("isnullorerror(1/0,1)", 0)
    assert t == 1

    t = engine.try_evaluate("isnullorerror(1,2)", 0)
    assert t == 1


def test_isnumber():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("if(ISNUMBER(1),1,2)", 0)
    assert t == 1

    t = engine.try_evaluate("if(ISNUMBER('e'),1,2)", 0)
    assert t == 2
    t = engine.try_evaluate("if(ISNUMBER('11'),1,2)", 0)
    assert t == 2

    t = engine.try_evaluate("if(ISNUMBER('2016-1-2'),1,2)", 0)
    assert t == 2


def test_istext():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("if(ISTEXT(1),1,2)", 0)
    assert t == 2

    t = engine.try_evaluate("if(ISTEXT('e'),1,2)", 0)
    assert t == 1
    t = engine.try_evaluate("if(ISTEXT('11'),1,2)", 0)
    assert t == 1

    t = engine.try_evaluate("if(ISTEXT('2016-1-2'),1,2)", 0)
    assert t == 1


def test_isnontext():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("if(ISNONTEXT(1),1,2)", 0)
    assert t == 1

    t = engine.try_evaluate("if(ISNONTEXT('e'),1,2)", 0)
    assert t == 2
    t = engine.try_evaluate("if(ISNONTEXT('11'),1,2)", 0)
    assert t == 2

    t = engine.try_evaluate("if(ISNONTEXT('2016-1-2'),1,2)", 0)
    assert t == 2


def test_islogical():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("if(ISLOGICAL(1),1,2)", 0)
    assert t == 2

    t = engine.try_evaluate("if(ISLOGICAL('e'),1,2)", 0)
    assert t == 2

    t = engine.try_evaluate("if(ISLOGICAL(true),1,2)", 0)
    assert t == 1

    t = engine.try_evaluate("if(ISLOGICAL(false),1,2)", 0)
    assert t == 1


def test_iseven():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("if(ISEVEN(1),1,2)", 0)
    assert t == 2

    t = engine.try_evaluate("if(ISEVEN(2),1,2)", 0)
    assert t == 1

    t = engine.try_evaluate("if(ISEVEN('e'),1,2)", 0)
    assert t == 2


def test_isodd():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("if(ISODD(1),1,2)", 0)
    assert t == 1

    t = engine.try_evaluate("if(ISODD(2),1,2)", 0)
    assert t == 2

    t = engine.try_evaluate("if(ISODD('e'),1,2)", 0)
    assert t == 2


def test_and():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("and(true(),1=1)", False)
    assert t is True
    t = engine.try_evaluate("and(true(),1)", False)
    assert t is True

    t = engine.try_evaluate("and(true(),false(),1=1)", True)
    assert t is False
    t = engine.try_evaluate("and(true(),false(),1)", True)
    assert t is False

    t = engine.try_evaluate("and(true(),0)", True)
    assert t is False


def test_or():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("or(true(),1=1)", False)
    assert t is True
    t = engine.try_evaluate("or(true(),1)", False)
    assert t is True

    t = engine.try_evaluate("or(true(),false(),1=1)", False)
    assert t is True
    t = engine.try_evaluate("or(true(),false(),1)", False)
    assert t is True

    t = engine.try_evaluate("or(true(),0)", False)
    assert t is True

    t = engine.try_evaluate("or(false(),1=2)", True)
    assert t is False


def test_not():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("not(true())", True)
    assert t is False
    t = engine.try_evaluate("not(false())", False)
    assert t is True


def test_true():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("true()", False)
    assert t is True


def test_false():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("false()", True)
    assert t is False


def test_andor():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("1=1 && 2==2 && 3=3", False)
    assert t is True

    t = engine.try_evaluate("1=1 && 2!=2", True)
    assert t is False

    t = engine.try_evaluate("1=1 || 2!=2", False)
    assert t is True


def test_ifs():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("IFS(1=1, 'a', 1=2, 'b')", "")
    assert t == "a"

    t = engine.try_evaluate("IFS(1=2, 'a', 2=2, 'b')", "")
    assert t == "b"

    t2 = engine.try_evaluate("IFS(1=1, 100, 1=2, 200)", 0)
    assert t2 == 100


def test_switch():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SWITCH(1, 1, 'one', 2, 'two')", "")
    assert t == "one"

    t = engine.try_evaluate("SWITCH(2, 1, 'one', 2, 'two')", "")
    assert t == "two"

    t2 = engine.try_evaluate("SWITCH('a', 'a', 1, 'b', 2)", 0)
    assert t2 == 1

    t = engine.try_evaluate("SWITCH(5, 1, 'one', 2, 'two')", "")
    assert t == ""


def test_switch_excel_compatible():
    engine = AlgorithmEngine()
    # 有默认值的情况
    t = engine.try_evaluate("SWITCH(3, 1, 'one', 2, 'two', 'unknown')", "")
    assert t == "unknown"

    t = engine.try_evaluate("SWITCH(1, 1, 'one', 2, 'two', 'unknown')", "")
    assert t == "one"

    # 数字类型默认值
    t2 = engine.try_evaluate("SWITCH(5, 1, 100, 2, 200, 0)", 0)
    assert t2 == 0

    # 多个 value/result 对 + 默认值
    t = engine.try_evaluate("SWITCH(4, 1, 'A', 2, 'B', 3, 'C', 'D')", "")
    assert t == "D"

    t = engine.try_evaluate("SWITCH(3, 1, 'A', 2, 'B', 3, 'C', 'D')", "")
    assert t == "C"

    # 字符串匹配 + 默认值
    t3 = engine.try_evaluate("SWITCH('x', 'a', 1, 'b', 2, -1)", 0)
    assert t3 == -1


def test_xor():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("XOR(true(), false())", False)
    assert t is True

    t = engine.try_evaluate("XOR(true(), true())", True)
    assert t is False

    t = engine.try_evaluate("XOR(false(), false())", True)
    assert t is False

    t = engine.try_evaluate("XOR(true(), true(), true())", False)
    assert t is True

    t = engine.try_evaluate("XOR(true(), false(), false())", False)
    assert t is True

    t = engine.try_evaluate("XOR(1, 0)", False)
    assert t is True

    t = engine.try_evaluate("XOR(1, 1)", True)
    assert t is False


def test_method_style_isnumber():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("123.ISNUMBER()", False)
    assert t is True

    t = engine.try_evaluate("'abc'.ISNUMBER()", True)
    assert t is False

    t = engine.try_evaluate("true.ISNUMBER()", True)
    assert t is False


def test_method_style_istext():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abc'.ISTEXT()", False)
    assert t is True

    t = engine.try_evaluate("123.ISTEXT()", True)
    assert t is False


def test_method_style_isnontext():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("123.ISNONTEXT()", False)
    assert t is True

    t = engine.try_evaluate("'abc'.ISNONTEXT()", True)
    assert t is False


def test_method_style_islogical():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("true.ISLOGICAL()", False)
    assert t is True

    t = engine.try_evaluate("false.ISLOGICAL()", False)
    assert t is True

    t = engine.try_evaluate("123.ISLOGICAL()", True)
    assert t is False


def test_method_style_isnull():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("null.ISNULL()", False)
    assert t is True

    t = engine.try_evaluate("123.ISNULL()", True)
    assert t is False


def test_method_style_isnullorerror():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("null.ISNULLORERROR()", False)
    assert t is True
