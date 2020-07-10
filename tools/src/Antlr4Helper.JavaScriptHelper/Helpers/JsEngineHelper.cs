using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Jint;
using ToolGood.AntiDuplication;

namespace Antlr4Helper.JavaScriptHelper.Helpers
{
    public class JsEngineHelper
    {
        private static AntiDupCache<string, string> antiDupCache = new AntiDupCache<string, string>(10000);
        private static JsEngineSwitcher JsEngineSwitcher;

        private static JsEngineSwitcher GetJsEngineSwitcher()
        {
            if (JsEngineSwitcher == null)
            {
                JsEngineSwitcher engineSwitcher = JsEngineSwitcher.Current as JsEngineSwitcher;
                engineSwitcher.EngineFactories.AddJint(new JintSettings { StrictMode = true });
                //engineSwitcher.EngineFactories.AddJurassic(new JurassicSettings { StrictMode = true, });
                JsEngineSwitcher = engineSwitcher;
            }
            return JsEngineSwitcher;
        }

        public static string GetCondition( string condition,int max)
        {
            GetJsEngineSwitcher();
            return antiDupCache.GetOrAdd(condition, () =>
            {
                List<int> useConstants = new List<int>();
                for (int i = 1; i <= max; i++)
                {
                    useConstants.Add(i);
                }

                var js = @"var antlr4={};
antlr4.Token={};
antlr4.Token.EOF=-1;
var testNums=[{1}];
var result=[];
for(var i=0;i<testNums.length;i++){
    var _la=testNums[i];
    if({2}){
        result.push(_la)
    }
}
outResult=result.join(',');
";
                js = js.Replace("{1}", string.Join(",", useConstants));
                js = js.Replace("{2}", condition);

                IJsEngine engine = JsEngineSwitcher.Current.CreateEngine(JintJsEngine.EngineName);
                engine.SetVariableValue("outResult", "");
                engine.Evaluate(js);
                var result = engine.GetVariableValue<string>("outResult");
                engine.Dispose();
                return result;
            });
        }



        public static string GetCondition(List<string> constants, List<int> useConstants, string condition, string className)
        {
            GetJsEngineSwitcher();
            return antiDupCache.GetOrAdd(condition, () =>
            {
                var js = @"var antlr4={};
antlr4.Token={};
antlr4.Token.EOF=-1;
var {3}={};
{3}.EOF = antlr4.Token.EOF;
{0}
var testNums=[{1}];
var result=[];
for(var i=0;i<testNums.length;i++){
    var _la=testNums[i];
    if({2}){
        result.push(_la)
    }
}
outResult=result.join(',');
";
                js = js.Replace("{0}", string.Join("\r\n", constants));
                js = js.Replace("{1}", string.Join(",", useConstants));
                js = js.Replace("{2}", condition);
                js = js.Replace("{3}", className);

                IJsEngine engine = JsEngineSwitcher.Current.CreateEngine(JintJsEngine.EngineName);
                engine.SetVariableValue("outResult", "");
                engine.Evaluate(js);
                var result = engine.GetVariableValue<string>("outResult");
                engine.Dispose();
                return result;
            });
        }

    }
}
