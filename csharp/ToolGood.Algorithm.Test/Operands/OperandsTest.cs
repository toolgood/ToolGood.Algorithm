using PetaTest;
using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Test.Operands
{
    [TestFixture]
    public class OperandsTest
    {
        #region Operand 静态属性测试

        [Test]
        public void Operand_StaticProperties_Test()
        {
            Assert.AreEqual("ToolGood.Algorithm 6.2", Operand.Version.TextValue);
            Assert.AreEqual(true, Operand.True.BooleanValue);
            Assert.AreEqual(false, Operand.False.BooleanValue);
            Assert.AreEqual(1, Operand.One.IntValue);
            Assert.AreEqual(0, Operand.Zero.IntValue);
            Assert.IsTrue(Operand.Null.IsNull);
        }

        #endregion

        #region Operand Create 测试

        [Test]
        public void Operand_Create_Bool_Test()
        {
            var op = Operand.Create(true);
            Assert.IsTrue(op.IsBoolean);
            Assert.AreEqual(true, op.BooleanValue);

            op = Operand.Create(false);
            Assert.IsTrue(op.IsBoolean);
            Assert.AreEqual(false, op.BooleanValue);
        }

        [Test]
        public void Operand_Create_Int_Test()
        {
            var op = Operand.Create(100);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(100, op.IntValue);
            Assert.AreEqual(100m, op.NumberValue);

            op = Operand.Create(-100);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(-100, op.IntValue);

            op = Operand.Create(0);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(0, op.IntValue);

            op = Operand.Create(1000);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(1000, op.IntValue);

            op = Operand.Create(-1000);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(-1000, op.IntValue);
        }

        [Test]
        public void Operand_Create_Long_Test()
        {
            var op = Operand.Create(100000L);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(100000L, op.LongValue);
        }

        [Test]
        public void Operand_Create_Double_Test()
        {
            var op = Operand.Create(3.14159);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(3.14159, op.DoubleValue, 0.00001);
        }

        [Test]
        public void Operand_Create_Decimal_Test()
        {
            var op = Operand.Create(3.14159m);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(3.14159m, op.NumberValue);
        }

        [Test]
        public void Operand_Create_String_Test()
        {
            var op = Operand.Create("hello");
            Assert.IsTrue(op.IsText);
            Assert.AreEqual("hello", op.TextValue);

            op = Operand.Create("");
            Assert.IsTrue(op.IsText);
            Assert.AreEqual("", op.TextValue);

            op = Operand.Create((string)null);
            Assert.IsTrue(op.IsNull);
        }

        [Test]
        public void Operand_Create_DateTime_Test()
        {
            var dt = new DateTime(2024, 1, 15, 10, 30, 45);
            var op = Operand.Create(dt);
            Assert.IsTrue(op.IsDate);
            Assert.AreEqual(2024, op.DateValue.Year);
            Assert.AreEqual(1, op.DateValue.Month);
            Assert.AreEqual(15, op.DateValue.Day);
            Assert.AreEqual(10, op.DateValue.Hour);
            Assert.AreEqual(30, op.DateValue.Minute);
            Assert.AreEqual(45, op.DateValue.Second);
        }

        [Test]
        public void Operand_Create_TimeSpan_Test()
        {
            var ts = new TimeSpan(1, 2, 3, 4);
            var op = Operand.Create(ts);
            Assert.IsTrue(op.IsDate);
            Assert.AreEqual(1, op.DateValue.Day);
            Assert.AreEqual(2, op.DateValue.Hour);
            Assert.AreEqual(3, op.DateValue.Minute);
            Assert.AreEqual(4, op.DateValue.Second);
        }

        [Test]
        public void Operand_Create_List_Test()
        {
            var list = new List<Operand> { Operand.Create(1), Operand.Create(2), Operand.Create(3) };
            var op = Operand.Create(list);
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(3, op.ArrayValue.Count);
            Assert.AreEqual(1, op.ArrayValue[0].IntValue);
            Assert.AreEqual(2, op.ArrayValue[1].IntValue);
            Assert.AreEqual(3, op.ArrayValue[2].IntValue);
        }

        [Test]
        public void Operand_Create_Collection_String_Test()
        {
            var list = new List<string> { "a", "b", "c" };
            var op = Operand.Create((ICollection<string>)list);
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(3, op.ArrayValue.Count);
            Assert.AreEqual("a", op.ArrayValue[0].TextValue);
            Assert.AreEqual("b", op.ArrayValue[1].TextValue);
            Assert.AreEqual("c", op.ArrayValue[2].TextValue);
        }

        [Test]
        public void Operand_Create_Collection_Int_Test()
        {
            var list = new List<int> { 1, 2, 3 };
            var op = Operand.Create((ICollection<int>)list);
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(3, op.ArrayValue.Count);
            Assert.AreEqual(1, op.ArrayValue[0].IntValue);
            Assert.AreEqual(2, op.ArrayValue[1].IntValue);
            Assert.AreEqual(3, op.ArrayValue[2].IntValue);
        }

        [Test]
        public void Operand_Create_Collection_Double_Test()
        {
            var list = new List<double> { 1.1, 2.2, 3.3 };
            var op = Operand.Create((ICollection<double>)list);
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(3, op.ArrayValue.Count);
            Assert.AreEqual(1.1, op.ArrayValue[0].DoubleValue, 0.001);
            Assert.AreEqual(2.2, op.ArrayValue[1].DoubleValue, 0.001);
            Assert.AreEqual(3.3, op.ArrayValue[2].DoubleValue, 0.001);
        }

        [Test]
        public void Operand_Create_Collection_Bool_Test()
        {
            var list = new List<bool> { true, false, true };
            var op = Operand.Create((ICollection<bool>)list);
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(3, op.ArrayValue.Count);
            Assert.AreEqual(true, op.ArrayValue[0].BooleanValue);
            Assert.AreEqual(false, op.ArrayValue[1].BooleanValue);
            Assert.AreEqual(true, op.ArrayValue[2].BooleanValue);
        }

        [Test]
        public void Operand_CreateJson_Test()
        {
            var op = Operand.CreateJson("{\"name\":\"test\",\"value\":123}");
            Assert.IsTrue(op.IsJson);

            op = Operand.CreateJson("[1,2,3]");
            Assert.IsTrue(op.IsJson);

            op = Operand.CreateJson("invalid json");
            Assert.IsTrue(op.IsError);
        }

        [Test]
        public void Operand_Error_Test()
        {
            var op = Operand.Error("test error");
            Assert.IsTrue(op.IsError);
            Assert.AreEqual("test error", op.ErrorMsg);

            op = Operand.Error("error {0}", "param");
            Assert.IsTrue(op.IsError);
            Assert.AreEqual("error param", op.ErrorMsg);
        }

        [Test]
        public void Operand_CreateNull_Test()
        {
            var op = Operand.Null;
            Assert.IsTrue(op.IsNull);
        }

        #endregion

        #region OperandInt 测试

        [Test]
        public void OperandInt_Properties_Test()
        {
            var op = Operand.Create(42);
            Assert.AreEqual(OperandType.NUMBER, op.Type);
            Assert.IsTrue(op.IsNumber);
            Assert.IsFalse(op.IsNull);
            Assert.IsFalse(op.IsText);
            Assert.IsFalse(op.IsBoolean);
            Assert.IsFalse(op.IsArray);
            Assert.IsFalse(op.IsDate);
            Assert.IsFalse(op.IsJson);
            Assert.IsFalse(op.IsError);
        }

        [Test]
        public void OperandInt_ToNumber_Test()
        {
            var op = Operand.Create(42);
            var result = op.ToNumber();
            Assert.IsTrue(result.IsNumber);
            Assert.AreEqual(42, result.IntValue);
        }

        [Test]
        public void OperandInt_ToBoolean_Test()
        {
            var op = Operand.Create(0);
            var result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(false, result.BooleanValue);

            op = Operand.Create(1);
            result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(true, result.BooleanValue);

            op = Operand.Create(2);
            result = op.ToBoolean();
            Assert.IsTrue(result.IsError);
        }

        [Test]
        public void OperandInt_ToText_Test()
        {
            var op = Operand.Create(42);
            var result = op.ToText();
            Assert.IsTrue(result.IsText);
            Assert.AreEqual("42", result.TextValue);
        }

        [Test]
        public void OperandInt_ToString_Test()
        {
            var op = Operand.Create(42);
            Assert.AreEqual("42", op.ToString());
        }

        #endregion

        #region OperandDouble 测试

        [Test]
        public void OperandDouble_Properties_Test()
        {
            var op = Operand.Create(3.14);
            Assert.AreEqual(OperandType.NUMBER, op.Type);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(3.14, op.DoubleValue, 0.001);
            Assert.AreEqual(3.14m, op.NumberValue);
            Assert.AreEqual(3, op.IntValue);
            Assert.AreEqual(3L, op.LongValue);
        }

        [Test]
        public void OperandDouble_ToBoolean_Test()
        {
            var op = Operand.Create(0.0);
            var result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(false, result.BooleanValue);

            op = Operand.Create(1.0);
            result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(true, result.BooleanValue);

            op = Operand.Create(2.5);
            result = op.ToBoolean();
            Assert.IsTrue(result.IsError);
        }

        [Test]
        public void OperandDouble_ToText_Test()
        {
            var op = Operand.Create(3.14);
            var result = op.ToText();
            Assert.IsTrue(result.IsText);
            Assert.AreEqual("3.14", result.TextValue);
        }

        #endregion

        #region OperandDecimal 测试

        [Test]
        public void OperandDecimal_Properties_Test()
        {
            var op = Operand.Create(123.456m);
            Assert.AreEqual(OperandType.NUMBER, op.Type);
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(123.456m, op.NumberValue);
            Assert.AreEqual(123.456, op.DoubleValue, 0.001);
            Assert.AreEqual(123, op.IntValue);
        }

        [Test]
        public void OperandDecimal_ToBoolean_Test()
        {
            var op = Operand.Create(0m);
            var result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(false, result.BooleanValue);

            op = Operand.Create(1m);
            result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(true, result.BooleanValue);
        }

        [Test]
        public void OperandDecimal_ToText_Test()
        {
            var op = Operand.Create(123.45m);
            var result = op.ToText();
            Assert.IsTrue(result.IsText);
            Assert.AreEqual("123.45", result.TextValue);
        }

        #endregion

        #region OperandBoolean 测试

        [Test]
        public void OperandBoolean_Properties_Test()
        {
            var op = Operand.Create(true);
            Assert.AreEqual(OperandType.BOOLEAN, op.Type);
            Assert.IsTrue(op.IsBoolean);
            Assert.IsFalse(op.IsNumber);
            Assert.AreEqual(true, op.BooleanValue);

            op = Operand.Create(false);
            Assert.AreEqual(OperandType.BOOLEAN, op.Type);
            Assert.IsTrue(op.IsBoolean);
            Assert.AreEqual(false, op.BooleanValue);
        }

        [Test]
        public void OperandBoolean_ToNumber_Test()
        {
            var op = Operand.Create(true);
            var result = op.ToNumber();
            Assert.IsTrue(result.IsNumber);
            Assert.AreEqual(1, result.IntValue);

            op = Operand.Create(false);
            result = op.ToNumber();
            Assert.IsTrue(result.IsNumber);
            Assert.AreEqual(0, result.IntValue);
        }

        [Test]
        public void OperandBoolean_ToText_Test()
        {
            var op = Operand.Create(true);
            var result = op.ToText();
            Assert.IsTrue(result.IsText);
            Assert.AreEqual("TRUE", result.TextValue);

            op = Operand.Create(false);
            result = op.ToText();
            Assert.IsTrue(result.IsText);
            Assert.AreEqual("FALSE", result.TextValue);
        }

        [Test]
        public void OperandBoolean_ToString_Test()
        {
            var op = Operand.Create(true);
            Assert.AreEqual("true", op.ToString());

            op = Operand.Create(false);
            Assert.AreEqual("false", op.ToString());
        }

        #endregion

        #region OperandString 测试

        [Test]
        public void OperandString_Properties_Test()
        {
            var op = Operand.Create("hello world");
            Assert.AreEqual(OperandType.TEXT, op.Type);
            Assert.IsTrue(op.IsText);
            Assert.IsFalse(op.IsNumber);
            Assert.IsFalse(op.IsBoolean);
            Assert.AreEqual("hello world", op.TextValue);
        }

        [Test]
        public void OperandString_ToNumber_Test()
        {
            var op = Operand.Create("42");
            var result = op.ToNumber();
            Assert.IsTrue(result.IsNumber);
            Assert.AreEqual(42, result.IntValue);

            op = Operand.Create("3.14");
            result = op.ToNumber();
            Assert.IsTrue(result.IsNumber);
            Assert.AreEqual(3.14, result.DoubleValue, 0.001);

            op = Operand.Create("not a number");
            result = op.ToNumber();
            Assert.IsTrue(result.IsError);
        }

        [Test]
        public void OperandString_ToBoolean_Test()
        {
            var op = Operand.Create("true");
            var result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(true, result.BooleanValue);

            op = Operand.Create("false");
            result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(false, result.BooleanValue);

            op = Operand.Create("TRUE");
            result = op.ToBoolean();
            Assert.IsTrue(result.IsBoolean);
            Assert.AreEqual(true, result.BooleanValue);

            op = Operand.Create("not bool");
            result = op.ToBoolean();
            Assert.IsTrue(result.IsError);
        }

        [Test]
        public void OperandString_ToMyDate_Test()
        {
            var op = Operand.Create("2024-01-15");
            var result = op.ToMyDate();
            Assert.IsTrue(result.IsDate);
            Assert.AreEqual(2024, result.DateValue.Year);
            Assert.AreEqual(1, result.DateValue.Month);
            Assert.AreEqual(15, result.DateValue.Day);

            op = Operand.Create("10:30:45");
            result = op.ToMyDate();
            Assert.IsTrue(result.IsDate);
            Assert.AreEqual(10, result.DateValue.Hour);
            Assert.AreEqual(30, result.DateValue.Minute);
            Assert.AreEqual(45, result.DateValue.Second);

            op = Operand.Create("not a date");
            result = op.ToMyDate();
            Assert.IsTrue(result.IsError);
        }

        [Test]
        public void OperandString_ToJson_Test()
        {
            var op = Operand.Create("{\"name\":\"test\"}");
            var result = op.ToJson();
            Assert.IsTrue(result.IsJson);

            op = Operand.Create("[1,2,3]");
            result = op.ToJson();
            Assert.IsTrue(result.IsJson);

            op = Operand.Create("not json");
            result = op.ToJson();
            Assert.IsTrue(result.IsError);
        }

        [Test]
        public void OperandString_ToArray_Test()
        {
            var op = Operand.Create("test");
            var result = op.ToArray();
            Assert.IsTrue(result.IsError);
        }

        [Test]
        public void OperandString_ToString_Test()
        {
            var op = Operand.Create("hello");
            Assert.AreEqual("\"hello\"", op.ToString());

            op = Operand.Create("hello\"world");
            Assert.AreEqual("\"hello\\\"world\"", op.ToString());

            op = Operand.Create("hello\nworld");
            Assert.AreEqual("\"hello\\nworld\"", op.ToString());

            op = Operand.Create("hello\tworld");
            Assert.AreEqual("\"hello\\tworld\"", op.ToString());
        }

        #endregion

        #region OperandNull 测试

        [Test]
        public void OperandNull_Properties_Test()
        {
            var op = Operand.Null;
            Assert.AreEqual(OperandType.NULL, op.Type);
            Assert.IsTrue(op.IsNull);
            Assert.IsFalse(op.IsNumber);
            Assert.IsFalse(op.IsText);
            Assert.IsFalse(op.IsBoolean);
            Assert.IsFalse(op.IsError);
        }

        [Test]
        public void OperandNull_ToString_Test()
        {
            var op = Operand.Null;
            Assert.AreEqual("null", op.ToString());
        }

        #endregion

        #region OperandError 测试

        [Test]
        public void OperandError_Properties_Test()
        {
            var op = Operand.Error("test error message");
            Assert.AreEqual(OperandType.ERROR, op.Type);
            Assert.IsTrue(op.IsError);
            Assert.IsFalse(op.IsNull);
            Assert.IsFalse(op.IsNumber);
            Assert.IsFalse(op.IsText);
            Assert.AreEqual("test error message", op.ErrorMsg);
        }

        [Test]
        public void OperandError_Conversions_Test()
        {
            var op = Operand.Error("error");

            Assert.IsTrue(op.ToNumber().IsError);
            Assert.IsTrue(op.ToBoolean().IsError);
            Assert.IsTrue(op.ToText().IsError);
            Assert.IsTrue(op.ToArray().IsError);
            Assert.IsTrue(op.ToMyDate().IsError);
            Assert.IsTrue(op.ToJson().IsError);
        }

        #endregion

        #region OperandArray 测试

        [Test]
        public void OperandArray_Properties_Test()
        {
            var list = new List<Operand> { Operand.Create(1), Operand.Create(2), Operand.Create(3) };
            var op = Operand.Create(list);
            Assert.AreEqual(OperandType.ARRAY, op.Type);
            Assert.IsTrue(op.IsArray);
            Assert.IsFalse(op.IsNumber);
            Assert.IsFalse(op.IsText);
            Assert.AreEqual(3, op.ArrayValue.Count);
        }

        [Test]
        public void OperandArray_ToText_Test()
        {
            var list = new List<Operand> { Operand.Create(1), Operand.Create(2), Operand.Create(3) };
            var op = Operand.Create(list);
            var result = op.ToText();
            Assert.IsTrue(result.IsText);
            Assert.AreEqual("[1,2,3]", result.TextValue);
        }

        [Test]
        public void OperandArray_ToArray_Test()
        {
            var list = new List<Operand> { Operand.Create(1), Operand.Create(2) };
            var op = Operand.Create(list);
            var result = op.ToArray("");
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(2, result.ArrayValue.Count);
        }

        [Test]
        public void OperandArray_ToJson_Test()
        {
            var list = new List<Operand> { Operand.Create(1), Operand.Create(2) };
            var op = Operand.Create(list);
            var result = op.ToJson();
            Assert.IsTrue(result.IsJson);
        }

        [Test]
        public void OperandArray_ToString_Test()
        {
            var list = new List<Operand> { Operand.Create(1), Operand.Create("test"), Operand.Create(true) };
            var op = Operand.Create(list);
            Assert.AreEqual("[1,\"test\",true]", op.ToString());
        }

        #endregion

        #region OperandMyDate 测试

        [Test]
        public void OperandMyDate_Properties_Test()
        {
            var dt = new DateTime(2024, 6, 15, 10, 30, 45);
            var op = Operand.Create(dt);
            Assert.AreEqual(OperandType.DATE, op.Type);
            Assert.IsTrue(op.IsDate);
            Assert.IsFalse(op.IsNumber);
            Assert.IsFalse(op.IsText);
            Assert.AreEqual(2024, op.DateValue.Year);
            Assert.AreEqual(6, op.DateValue.Month);
            Assert.AreEqual(15, op.DateValue.Day);
            Assert.AreEqual(10, op.DateValue.Hour);
            Assert.AreEqual(30, op.DateValue.Minute);
            Assert.AreEqual(45, op.DateValue.Second);
        }

        [Test]
        public void OperandMyDate_ToText_Test()
        {
            var dt = new DateTime(2024, 6, 15, 10, 30, 45);
            var op = Operand.Create(dt);
            var result = op.ToText();
            Assert.IsTrue(result.IsText);
        }

        [Test]
        public void OperandMyDate_ToMyDate_Test()
        {
            var dt = new DateTime(2024, 6, 15, 10, 30, 45);
            var op = Operand.Create(dt);
            var result = op.ToMyDate();
            Assert.IsTrue(result.IsDate);
            Assert.AreEqual(2024, result.DateValue.Year);
        }

        [Test]
        public void OperandMyDate_ToString_Test()
        {
            var dt = new DateTime(2024, 6, 15, 10, 30, 45);
            var op = Operand.Create(dt);
            Assert.IsTrue(op.ToString().StartsWith("\"2024-06-15"));
        }

        #endregion

        #region Implicit Operator 测试

        [Test]
        public void Operand_Implicit_Int_Test()
        {
            Operand op = 42;
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(42, op.IntValue);
        }

        [Test]
        public void Operand_Implicit_Double_Test()
        {
            Operand op = 3.14;
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(3.14, op.DoubleValue, 0.001);
        }

        [Test]
        public void Operand_Implicit_Decimal_Test()
        {
            Operand op = 123.45m;
            Assert.IsTrue(op.IsNumber);
            Assert.AreEqual(123.45m, op.NumberValue);
        }

        [Test]
        public void Operand_Implicit_Bool_Test()
        {
            Operand op = true;
            Assert.IsTrue(op.IsBoolean);
            Assert.AreEqual(true, op.BooleanValue);

            op = false;
            Assert.IsTrue(op.IsBoolean);
            Assert.AreEqual(false, op.BooleanValue);
        }

        [Test]
        public void Operand_Implicit_String_Test()
        {
            Operand op = "hello";
            Assert.IsTrue(op.IsText);
            Assert.AreEqual("hello", op.TextValue);
        }

        [Test]
        public void Operand_Implicit_DateTime_Test()
        {
            var dt = new DateTime(2024, 1, 15);
            Operand op = dt;
            Assert.IsTrue(op.IsDate);
            Assert.AreEqual(2024, op.DateValue.Year);
        }

        [Test]
        public void Operand_Implicit_TimeSpan_Test()
        {
            var ts = new TimeSpan(10, 30, 0);
            Operand op = ts;
            Assert.IsTrue(op.IsDate);
            Assert.AreEqual(10, op.DateValue.Hour);
            Assert.AreEqual(30, op.DateValue.Minute);
        }

        [Test]
        public void Operand_Implicit_List_Test()
        {
            Operand op = new List<int> { 1, 2, 3 };
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(3, op.ArrayValue.Count);

            op = new List<string> { "a", "b" };
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(2, op.ArrayValue.Count);

            op = new List<double> { 1.1, 2.2 };
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(2, op.ArrayValue.Count);

            op = new List<bool> { true, false };
            Assert.IsTrue(op.IsArray);
            Assert.AreEqual(2, op.ArrayValue.Count);
        }

        #endregion
    }
}
