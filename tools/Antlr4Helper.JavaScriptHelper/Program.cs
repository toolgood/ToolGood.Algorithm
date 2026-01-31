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
			text = text.Replace("mathParser.EOF = antlr4.Token.EOF;", "");
			text = Regex.Replace(text, @"mathParser\..*?_funContext = .*?_funContext;", "");
			text = Regex.Replace(text, @"this\.state = \d+;[\r\n\t ]*this.match", "this.match");
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
			text = Regex.Replace(text, @"([A-Z]*\(\) \{)[\r\n\t ]*(return this\.getToken\()", "$1$2");
			text = Regex.Replace(text, @"([A-Z]*\(\) \{return this\.getToken.*?;)[\r\n\t ]*};", "$1};");
			text = Regex.Replace(text, @"([A-Z]*\(\) \{return this\.getToken.*?};)", "//$1");
			text = text.Replace("//PARAMETER()", "PARAMETER()");
			text = text.Replace("//NUM()", "NUM()");
			text = text.Replace("//STRING()", "STRING()");



			File.WriteAllText("mathParser.js", text);




		}
	}
}
