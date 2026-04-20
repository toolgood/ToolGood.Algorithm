using System;
using System.Collections.Generic;
using System.IO;
using Antlr4Helper.CSharpHelper.RegexEngine;

namespace Antlr4Helper.CSharpHelper.Tests
{
    public static class RegexEngineTests
    {
        public static void RunAllTests()
        {
            Console.WriteLine("=== 正则表达式引擎测试 ===\n");

            TestLexer();
            TestParser();
            TestNFABuilder();
            TestDFACoversion();
            TestDFAMinimization();
            TestPatternMerging();
            TestCodeGeneration();
            TestErrorHandling();

            Console.WriteLine("\n所有测试完成！");
        }

        static void TestLexer()
        {
            Console.WriteLine("1. 词法分析器测试");

            var lexer = new RegexLexer(@"a*b+c?|d[0-9]");
            var tokens = lexer.Tokenize();

            Assert(tokens.Count > 0, "应该生成token");
            Assert(tokens[tokens.Count - 1].Type == RegexTokenType.EndOfInput, "最后一个token应该是EndOfInput");

            Console.WriteLine("   词法分析器测试通过\n");
        }

        static void TestParser()
        {
            Console.WriteLine("2. 语法解析器测试");

            var testCases = new Dictionary<string, Type>
            {
                { "a", typeof(CharNode) },
                { "a|b", typeof(AlternationNode) },
                { "ab", typeof(ConcatNode) },
                { "a*", typeof(QuantifierNode) },
                { "a+", typeof(QuantifierNode) },
                { "a?", typeof(QuantifierNode) },
                { "a{3}", typeof(QuantifierNode) },
                { "a{3,5}", typeof(QuantifierNode) },
                { "a{3,}", typeof(QuantifierNode) },
                { "[abc]", typeof(CharClassNode) },
                { "[a-z]", typeof(CharClassNode) },
                { "[^abc]", typeof(CharClassNode) },
                { ".", typeof(AnyCharNode) },
                { "(a)", typeof(GroupNode) },
                { "(?:a)", typeof(GroupNode) },
                { @"\d", typeof(CharClassNode) },
                { @"\w", typeof(CharClassNode) },
            };

            var parser = new RegexParser();

            foreach (var testCase in testCases)
            {
                try
                {
                    var ast = parser.Parse(testCase.Key);
                    Assert(ast != null, $"解析 '{testCase.Key}' 应该成功");
                }
                catch (Exception ex)
                {
                    Assert(false, $"解析 '{testCase.Key}' 失败: {ex.Message}");
                }
            }

            Console.WriteLine("   语法解析器测试通过\n");
        }

        static void TestNFABuilder()
        {
            Console.WriteLine("3. NFA构建器测试");

            var parser = new RegexParser();
            var builder = new NFABuilder();

            var testCases = new Dictionary<string, int>
            {
                { "a", 2 },
                { "a*", 2 },
                { "a|b", 4 },
                { "ab", 3 },
            };

            foreach (var testCase in testCases)
            {
                var ast = parser.Parse(testCase.Key);
                var nfa = builder.Build(ast, 0);

                Assert(nfa.States.Count >= testCase.Value,
                    $"'{testCase.Key}' 应该至少有 {testCase.Value} 个状态，实际有 {nfa.States.Count}");
            }

            Console.WriteLine("   NFA构建器测试通过\n");
        }

        static void TestDFACoversion()
        {
            Console.WriteLine("4. DFA转换测试");

            var parser = new RegexParser();
            var builder = new NFABuilder();
            var converter = new NFAToDFAConverter();

            var testCases = new List<string> { "a", "a*", "a|b", "ab", "[a-z]+" };

            foreach (var pattern in testCases)
            {
                var ast = parser.Parse(pattern);
                var nfa = builder.Build(ast, 0);
                var dfa = converter.Convert(nfa);

                Assert(dfa.States.Count > 0, $"'{pattern}' DFA应该有状态");
                Assert(dfa.StartState != null, $"'{pattern}' DFA应该有起始状态");
            }

            Console.WriteLine("   DFA转换测试通过\n");
        }

        static void TestDFAMinimization()
        {
            Console.WriteLine("5. DFA最小化测试");

            var parser = new RegexParser();
            var builder = new NFABuilder();
            var converter = new NFAToDFAConverter();
            var minimizer = new DFAMinimizer();

            var ast = parser.Parse("(a|b)*abb");
            var nfa = builder.Build(ast, 0);
            var dfa = converter.Convert(nfa);
            var minDfa = minimizer.Minimize(dfa);

            Assert(minDfa.States.Count <= dfa.States.Count,
                $"最小化DFA状态数 ({minDfa.States.Count}) 应该小于等于原始DFA ({dfa.States.Count})");

            Console.WriteLine("   DFA最小化测试通过\n");
        }

        static void TestPatternMerging()
        {
            Console.WriteLine("6. 模式合并测试");

            var merger = new RegexMerger();

            var patterns = new List<string> { "a+", "b+", "c+" };
            var dfa = merger.MergePatterns(patterns);

            Assert(dfa.States.Count > 0, "合并后的DFA应该有状态");

            var result1 = dfa.Match("aaa");
            Assert(result1.Success && result1.PatternId == 0, "'aaa' 应该匹配模式 0");

            var result2 = dfa.Match("bbb");
            Assert(result2.Success && result2.PatternId == 1, "'bbb' 应该匹配模式 1");

            var result3 = dfa.Match("ccc");
            Assert(result3.Success && result3.PatternId == 2, "'ccc' 应该匹配模式 2");

            Console.WriteLine("   模式合并测试通过\n");
        }

        static void TestCodeGeneration()
        {
            Console.WriteLine("7. 代码生成测试");

            var merger = new RegexMerger();
            var generator = new DFACodeGenerator();

            var patterns = new List<string> { "[a-z]+", "[0-9]+" };
            var dfa = merger.MergePatterns(patterns);

            var code = generator.GenerateCode(dfa, "TestLexer", "TestNamespace");

            Assert(!string.IsNullOrEmpty(code), "生成的代码不应为空");
            Assert(code.Contains("TestLexer"), "代码应包含类名");
            Assert(code.Contains("TestNamespace"), "代码应包含命名空间");
            Assert(code.Contains("Match"), "代码应包含Match方法");

            Console.WriteLine("   代码生成测试通过\n");
        }

        static void TestErrorHandling()
        {
            Console.WriteLine("8. 错误处理测试");

            var parser = new RegexParser();

            var invalidPatterns = new List<string>
            {
                "((a",
                "[a-z",
                "a{",
                "a{3,2}",
            };

            foreach (var pattern in invalidPatterns)
            {
                bool caught = false;
                try
                {
                    parser.Parse(pattern);
                }
                catch (RegexParseException)
                {
                    caught = true;
                }
                Assert(caught, $"'{pattern}' 应该抛出解析异常");
            }

            Console.WriteLine("   错误处理测试通过\n");
        }

        static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception($"断言失败: {message}");
            }
        }
    }
}
