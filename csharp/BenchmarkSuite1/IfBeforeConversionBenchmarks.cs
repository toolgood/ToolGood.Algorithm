using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm;

namespace BenchmarkSuite1
{
    [CPUUsageDiagnoser]
    public class IfBeforeConversionBenchmarks
    {
        [Benchmark]
        public Operand String2Number_1()
        {
            var obj = Operand.Create("666");
            if (obj.IsNotNumber) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    return obj;
                }
            }
            return obj;
        }
        [Benchmark]
        public Operand String2Number_1_2()
        {
            var obj = Operand.Create("666");
            obj = obj.ToNumber("It can't be converted to number!");
            if (obj.IsError) {
                return obj;
            }
            return obj;
        }
        [Benchmark]
        public Operand String2String_1()
        {
            var obj = Operand.Create("666");
            if (obj.IsNotNumber) {
                obj = obj.ToText("It can't be converted to number!");
                if (obj.IsError) {
                    return obj;
                }
            }
            return obj;
        }
        [Benchmark]
        public Operand String2String_1_2()
        {
            var obj = Operand.Create("666");
            obj = obj.ToText("It can't be converted to number!");
            if (obj.IsError) {
                return obj;
            }
            return obj;
        }

        [Benchmark]
        public Operand Number2Number_1()
        {
            var obj = Operand.Create(666);
            if (obj.IsNotNumber) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    return obj;
                }
            }
            return obj;
        }
        [Benchmark]
        public Operand Number2Number_1_1()
        {
            var obj = Operand.Create(666);
            obj = obj.ToNumber("It can't be converted to number!");
            if (obj.IsError) {
                return obj;
            }
            return obj;
        }
        [Benchmark]
        public Operand Number2String_1()
        {
            var obj = Operand.Create(666);
            if (obj.IsNotText) {
                obj = obj.ToText("It can't be converted to number!");
                if (obj.IsError) {
                    return obj;
                }
            }
            return obj;
        }
        [Benchmark]
        public Operand Number2String_1_1()
        {
            var obj = Operand.Create(666);
            obj = obj.ToText("It can't be converted to number!");
            if (obj.IsError) {
                return obj;
            }
            return obj;
        }

    }
}
