using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4Helper.JavaScriptHelper.Helpers;

namespace Antlr4Helper.JavaScriptHelper
{
	class Program2
	{
		static void Main2(string[] args)
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
			text = text.Replace("//NUM()", "NUM()");
			text = text.Replace("//STRING()", "STRING()");

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


			text = Regex.Replace(text, @"this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.state = (\d+);[\r\n\t ]*this\.expr\((\d+)\);"
										, "this.F($1,$2,$3,$4,$5,$6);");
			text = Regex.Replace(text, @"this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.state = (\d+);[\r\n\t ]*this\.expr\((\d+)\);"
										, "this.G($1,$2,$3,$4,$5);");
			text = Regex.Replace(text, @"this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.state = (\d+);[\r\n\t ]*this\.expr\((\d+)\);"
										, "this.I($1,$2,$3,$4);");
			text = Regex.Replace(text, @"this\.match\((\d+)\);[\r\n\t ]*this\.state = (\d+);[\r\n\t ]*this\.expr\((\d+)\);"
										, "this.J($1,$2,$3);");
			text = Regex.Replace(text, @"this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);", "this.D($1,$2,$3,$4);");
			text = Regex.Replace(text, @"this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);", "this.C($1,$2,$3);");
			text = Regex.Replace(text, @"this\.match\((\d+)\);[\r\n\t ]*this\.match\((\d+)\);", "this.B($1,$2);");
			text = Regex.Replace(text, @"this\.match\((\d+)\);", "this.A($1);");
			text = Regex.Replace(text, @"this\.state = (\d+);[\r\n\t ]*this\.expr\((\d+)\);", "this.E($1,$2);");
			text = Regex.Replace(text, @"this\._errHandler\.sync\(this\);[\r\n\t ]*_la = this\._input\.LA\(1\);", "_la = this.Z();");
			//text = Regex.Replace(text, @"this\._input\.LA\(1\);", "this.Y();");
			text = Regex.Replace(text, @"this\._errHandler\.sync\(this\);", "this.X();");
			text = text.Replace("this.pushNewRecursionContext(localctx, _startState, 1);", "this.W(localctx, _startState, 1);");
			text = Regex.Replace(text, @"this\._errHandler\.reportMatch\(this\);[\r\n\t ]*this\.consume\(\);", "this.Q();");
			text = text.Replace("this._input.LA(1);", "this.Y();");
			text = text.Replace("this._input.LT(1);", "this.R();");


			text = Regex.Replace(text, @"sempred\(localctx, ruleIndex, predIndex\) \{[\s\S]+?\};", @" sempred(localctx, ruleIndex, predIndex){return true;};
	A(a){this.match(a);}
	B(a,b){this.match(a);this.match(b);}
	C(a,b,c){this.match(a);this.match(b);this.match(c);}
	D(a,b,c,d){this.match(a);this.match(b);this.match(c);this.match(d);}
	E(a,b){this.state=a;this.expr(b);}
	F(a,b,c,d,e,f){this.match(a);this.match(b);this.match(c);this.match(d);this.state=e;this.expr(f);}
	G(a,b,c,e,f){this.match(a);this.match(b);this.match(c);this.state=e;this.expr(f);}
	I(a,b,e,f){this.match(a);this.match(b);this.state=e;this.expr(f);}
	J(a,e,f){this.match(a);this.state=e;this.expr(f);}


	R(){return this._input.LT(1);}
	Q(){this._errHandler.reportMatch(this);this.consume();}
	W(a,b,c){this.pushNewRecursionContext(a, b, c);}
	X(){this._errHandler.sync(this);}
	Y(){return this._input.LA(1);}
	Z(){this._errHandler.sync(this);return this._input.LA(1);}
");

			// 清理多余的空行和注释
			text = Regex.Replace(text, @"[\t ]*//.*([\r\n])", "$1");
			text = Regex.Replace(text, @"[\t ]*([\r\n])([\r\n])+", "$1");
			text = Regex.Replace(text, @"([\r\n])([\r\n])+", "$1");


			File.WriteAllText("mathParser.js", text);




		}
	}
}
