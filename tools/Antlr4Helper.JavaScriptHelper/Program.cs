using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4Helper.JavaScriptHelper.Helpers;

namespace Antlr4Helper.JavaScriptHelper
{
	class Program
	{
		static void Main(string[] args)
		{
			var filePath = Path.GetFullPath(@"..\..\..\..\..\g4\antlr4\mathParser.js");
			var text = File.ReadAllText(filePath);
			text = text.Replace("import antlr4 from 'antlr4';", "import antlr4 from '../antlr4/index.web.js';");
			text = text.Replace("import mathVisitor from './mathVisitor.js';", "");
			text = text.Replace("mathParser.EOF = antlr4.Token.EOF;", "");
			text = Regex.Replace(text, @"mathParser\..*?Context = .*?Context;", "");
			text = Regex.Replace(text, @"this\.state = \d+;[\r\n\t ]*this.match", "this.match");

			// 替换所有的 mathParser.xxx = n 为字典，然后再替换回去
			var ms = Regex.Matches(text, @"(mathParser\..*) = (\d+);");
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict["mathParser.EOF"] = "-1";
			foreach(Match m in ms) {
				dict[m.Groups[1].Value] = m.Groups[2].Value;
			}
			text = Regex.Replace(text, @"(mathParser\..*) = (\d+);", "");
			var keys = dict.Keys.OrderByDescending(q => q.Length).ToList();
			foreach(var item in keys) {
				text = Regex.Replace(text, @$"\b{item}\b", dict[item]);
			}


			text = Regex.Replace(text, @"([A-Z][0-9A-Z]*\(\) \{)[\r\n\t ]*(return this\.getToken\()", "$1$2");
			text = Regex.Replace(text, @"([A-Z][0-9A-Z]*\(\) \{return this\.getToken.*?;)[\r\n\t ]*};", "$1};");
			text = Regex.Replace(text, @"([A-Z][0-9A-Z]*\(\) \{return this\.getToken.*?};)", "//$1");
			text = text.Replace("//PARAMETER()", "PARAMETER()");
			//text = text.Replace("//NUM()", "NUM()");
			//text = text.Replace("//STRING()", "STRING()");

			text = text.Replace("if ( visitor instanceof mathVisitor ) {", "");
			text = Regex.Replace(text, @"\} else \{[\r\n\t ]*return visitor\.visitChildren\(this\);[\r\n\t ]*\}", "");


			text = text.Replace("static grammarFileName = \"math.g4\";", "static grammarFileName = \"\";");
			text = Regex.Replace(text, @"static literalNames = \[[\s\S]+?\];", "static literalNames =[];");
			text = Regex.Replace(text, @"static symbolicNames = \[[\s\S]+?\];", "static symbolicNames =[];");
			text = Regex.Replace(text, @"static ruleNames = \[[\s\S]+?\];", "static ruleNames =[];");

			text = Regex.Replace(text, @"[\r\n]+[\t ]*throw new antlr4\.error\.FailedPredicateException.*?[\r\n]+", "");
			text = Regex.Replace(text, @"[\r\n]+[\t ]*if \(!\( this\.precpred\(this\._ctx, \d+\)\)\) \{[\s\t ]+\}", "");
			text = Regex.Replace(text, @"this\.state = \d+;[\r\n\t ]*this\.match", "this.match");
			text = Regex.Replace(text, @"this\.state = \d+;[\r\n\t ]*this\._errHandler\.sync\(this\);", "this._errHandler.sync(this);");
			text = Regex.Replace(text, @"this\.state = \d+;[\r\n\t ]*_la", "_la");
			text = Regex.Replace(text, @"this\.state = \d+;[\r\n\t ]*localctx", "localctx");
			text = Regex.Replace(text, @"this\.state = \d+;[\r\n\t ]*localctx", "localctx");




			// expr 一般情况下返回 ExprContext 的数组 或 单个 ExprContext
			text = Regex.Replace(text, @"expr = function\(i\) \{[\s\S]+?\};", "expr = function(i) {return this.getTypedRuleContexts(ExprContext);};");
			text = Regex.Replace(text, @"arrayJson = function\(i\) \{[\s\S]+?\};", "arrayJson = function(i) {return this.getTypedRuleContexts(ArrayJsonContext);};");

			text = Regex.Replace(text, @"if\(parent===undefined\) \{[\r\n\t ]*parent = null;[\r\n\t ]*\}", "");
			text = Regex.Replace(text, @"if\(invokingState===undefined \|\| invokingState===null\) \{[\r\n\t ]*invokingState = -1;[\r\n\t ]*}", "");

			text = Regex.Replace(text, @"let _prevctx = localctx;", "");
			text = Regex.Replace(text, @"_prevctx = localctx;", "");

 


			text = Regex.Replace(text, @"sempred\(localctx, ruleIndex, predIndex\) \{[\s\S]+?\};", @" sempred(localctx, ruleIndex, predIndex){return true;};");

			// 清理多余的空行和注释
			text = Regex.Replace(text, @"[\t ]*//.*([\r\n])", "$1");
			text = Regex.Replace(text, @"[\t ]*([\r\n])([\r\n])+", "$1");
			text = Regex.Replace(text, @"([\r\n])([\r\n])+", "$1");


			File.WriteAllText("mathParser.js", text);



			filePath = Path.GetFullPath(@"..\..\..\..\..\g4\antlr4\mathLexer.js");
			text = File.ReadAllText(filePath);
			text = text.Replace("import antlr4 from 'antlr4';", "import antlr4 from '../antlr4/index.web.js';");


			text = text.Replace("static grammarFileName = \"math.g4\";", "static grammarFileName = \"\";");
			text = Regex.Replace(text, @"static literalNames = \[[\s\S]+?\];", "static literalNames =[];");
			text = Regex.Replace(text, @"static symbolicNames = \[[\s\S]+?\];", "static symbolicNames =[];");
			text = Regex.Replace(text, @"static ruleNames = \[[\s\S]+?\];", "static ruleNames =[];");
			text = Regex.Replace(text, @"static channelNames = \[[\s\S]+?\];", "static channelNames =[];");
			text = Regex.Replace(text, @"static modeNames = \[[\s\S]+?\];", "static modeNames =[];");
			text = Regex.Replace(text, @"(mathLexer\..*) = (\d+);", "");


			text = text.Replace("mathLexer.EOF = antlr4.Token.EOF;", "");
			File.WriteAllText("mathLexer.js", text);


			filePath = Path.GetFullPath(@"..\..\..\..\..\g4\antlr4\mathVisitor.js");
			text = File.ReadAllText(filePath);
			text = text.Replace("import antlr4 from 'antlr4';", "import antlr4 from '../antlr4/index.web.js';");
			File.WriteAllText("mathVisitor.js", text);

		}
	}
}
